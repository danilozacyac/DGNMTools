using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using ScjnUtilities;

namespace DGNMTools.Reportes
{
    public class InscripcionModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Promedio"].ConnectionString;
        private readonly string sqlConnectionString = ConfigurationManager.ConnectionStrings["TipoSociedad"].ConnectionString;


        /// <summary>
        /// Obtiene el listado de socios que tienen CURP y RFC dentro de la base de datos
        /// </summary>
        /// <returns></returns>
        public void GetFechas()
        {
            List<Inscripcion> listaFechas = new List<Inscripcion>();

            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd;
            OleDbDataReader reader;

            try
            {
                connection.Open();

                cmd = new OleDbCommand("SELECT Id, FechaIngreso, FechaInscripcion from 2017_Consolidado", connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Inscripcion insc = new Inscripcion();
                    insc.Id = Convert.ToInt32(reader["Id"]);
                    insc.Fechaingreso = DateTimeUtilities.GetDateFromReader(reader, "Fechaingreso");
                    insc.Fechainscripcion = DateTimeUtilities.GetDateFromReader(reader, "Fechainscripcion");
                    insc.HorasInscripcion = (insc.Fechainscripcion.Value - insc.Fechaingreso.Value).TotalHours;
                    insc.DiasInscripcion = (insc.Fechainscripcion.Value - insc.Fechaingreso.Value).TotalDays;

                    listaFechas.Add(insc);
                }

                this.SetTiempoInscripcion(listaFechas);

                reader.Close();
                cmd.Dispose();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ValidaSocioModel", "DGNMTools");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ValidaSocioModel", "DGNMTools");
            }
            finally
            {
                connection.Close();
            }

        }


        public void SetTiempoInscripcion(List<Inscripcion> inscripciones)
        {
            foreach (Inscripcion insc in inscripciones)
            {

                OleDbConnection connection = new OleDbConnection(connectionString);
                OleDbDataAdapter dataAdapter;
                OleDbCommand cmd = connection.CreateCommand();
                cmd.Connection = connection;

                bool insertCompleted = false;

                try
                {
                    connection.Open();

                    DataSet dataSet = new DataSet();
                    DataRow dr;

                    const string OleDbQuery = "SELECT * FROM 2017_Consolidado WHERE Id = @Id";

                    dataAdapter = new OleDbDataAdapter();
                    dataAdapter.SelectCommand = new OleDbCommand(OleDbQuery, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@Id", insc.Id);
                    dataAdapter.Fill(dataSet, "2017_Consolidado");

                    if (dataSet.Tables["2017_Consolidado"].Rows.Count > 0)
                    {
                        dr = dataSet.Tables["2017_Consolidado"].Rows[0];
                        dr.BeginEdit();
                        dr["DiasInsc"] = insc.DiasInscripcion;
                        dr["HorasInsc"] = insc.HorasInscripcion;
                        dr.EndEdit();

                        dataAdapter.UpdateCommand = connection.CreateCommand();

                        dataAdapter.UpdateCommand.CommandText = "UPDATE 2017_Consolidado SET DiasInsc = @DiasInsc, HorasInsc = @HorasInsc WHERE Id = @Id"; //Aqui Tambien hay que validar con el nombre

                        dataAdapter.UpdateCommand.Parameters.Add("@DiasInsc", OleDbType.Numeric, 0, "DiasInsc");
                        dataAdapter.UpdateCommand.Parameters.Add("@HorasInsc", OleDbType.Numeric, 0, "HorasInsc");
                        dataAdapter.UpdateCommand.Parameters.Add("@Id", OleDbType.Numeric, 0, "Id");
                        dataAdapter.Update(dataSet, "2017_Consolidado");
                    }

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                    insertCompleted = true;
                }
                catch (OleDbException ex)
                {
                    //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    //ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
                }
                catch (Exception ex)
                {
                    //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    //ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
                }
                finally
                {
                    connection.Close();
                }
            }

        }



        public void GetPromedios(int minValue, int maxValue)
        {
            List<KeyValuePair<int, double>> promedios = new List<KeyValuePair<int, double>>();


            while (minValue <= maxValue)
            {

                OleDbConnection connection = new OleDbConnection(connectionString);
                OleDbCommand cmd;
                OleDbDataReader reader;

                try
                {
                    connection.Open();

                    cmd = new OleDbCommand("SELECT AVG(DiasInsc) AS Total FROM 2017_Consolidado WHERE diasInsc >= 0 and DiasInsc < @Valor", connection);
                    cmd.Parameters.AddWithValue("@Valor", minValue);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        KeyValuePair<int,double> valores = new KeyValuePair<int,double>(minValue,Convert.ToDouble(reader["Total"]));

                        promedios.Add(valores);
                    }

                    reader.Close();
                    cmd.Dispose();
                }
                catch (OleDbException ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ValidaSocioModel", "DGNMTools");
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ValidaSocioModel", "DGNMTools");
                }
                finally
                {
                    connection.Close();
                }

                minValue++;
            }
            this.SetPromedios(promedios);
        }



        private void SetPromedios(List<KeyValuePair<int,double>> promedios)
        {
            foreach (KeyValuePair<int,double> promedio in promedios)
            {

                OleDbConnection connection = new OleDbConnection(connectionString);

                bool insertCompleted = false;

                try
                {
                    connection.Open();

                    const string sqlQuery = "INSERT INTO AvgTime (TopeDias,Promedio) VALUES (@TopeDias,@Promedio)";

                    OleDbCommand cmd = new OleDbCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@TopeDias", promedio.Key);
                    cmd.Parameters.AddWithValue("@Promedio", promedio.Value);
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    insertCompleted = true;
                }
                catch (OleDbException ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
                }
                finally
                {
                    connection.Close();
                }
            }

        }





        public void GetFechasMUA(int minValue, int maxValue)
        {
            List<Inscripcion> listaFechas = new List<Inscripcion>();

            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd;
            OleDbDataReader reader;

            try
            {
                connection.Open();

                cmd = new OleDbCommand("SELECT Id, FechaSol, FechaDict FROM 2017_MUA WHERE horasdictamen IS NULL and Id BETWEEN @Min and @Max", connection);
                cmd.Parameters.AddWithValue("@Min", minValue);
                cmd.Parameters.AddWithValue("@Max", maxValue);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Inscripcion insc = new Inscripcion();
                    insc.Id = Convert.ToInt32(reader["Id"]);
                    insc.Fechaingreso = DateTimeUtilities.GetDateFromReader(reader, "FechaSol");
                    insc.Fechainscripcion = DateTimeUtilities.GetDateFromReader(reader, "FechaDict");
                    insc.DiasInscripcion = (insc.Fechainscripcion.Value - insc.Fechaingreso.Value).TotalDays;
                    insc.HorasInscripcion = (insc.Fechainscripcion.Value - insc.Fechaingreso.Value).TotalHours;

                    int semanaSol = this.GetWeekNumber((DateTime)insc.Fechaingreso.Value);
                    int semanaDic = this.GetWeekNumber((DateTime)insc.Fechainscripcion.Value);

                    int semanasDiferencia = semanaDic - semanaSol;

                    if (semanaDic - semanaSol == 0)
                    {
                        insc.DiasInscripcionSinFines = insc.DiasInscripcion;
                        insc.HorasInscripcionSinfines = insc.HorasInscripcion;
                    }
                    else
                    {
                        insc.DiasInscripcionSinFines = insc.DiasInscripcion - (2 * semanasDiferencia);
                        insc.HorasInscripcionSinfines = insc.HorasInscripcion - (48 * semanasDiferencia);
                    }


                    listaFechas.Add(insc);
                }

                this.SetTiempoInscripcionMua(listaFechas);

                reader.Close();
                cmd.Dispose();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ValidaSocioModel", "DGNMTools");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ValidaSocioModel", "DGNMTools");
            }
            finally
            {
                connection.Close();
            }

        }

        public void SetTiempoInscripcionMua(List<Inscripcion> inscripciones)
        {
            foreach (Inscripcion insc in inscripciones)
            {

                OleDbConnection connection = new OleDbConnection(connectionString);
                OleDbDataAdapter dataAdapter;
                OleDbCommand cmd = connection.CreateCommand();
                cmd.Connection = connection;

                bool insertCompleted = false;

                try
                {
                    connection.Open();

                    DataSet dataSet = new DataSet();
                    DataRow dr;

                    const string OleDbQuery = "SELECT * FROM 2017_MUA WHERE Id = @Id";

                    dataAdapter = new OleDbDataAdapter();
                    dataAdapter.SelectCommand = new OleDbCommand(OleDbQuery, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@Id", insc.Id);
                    dataAdapter.Fill(dataSet, "2017_MUA");

                    

                    if (dataSet.Tables["2017_MUA"].Rows.Count > 0)
                    {
                        dr = dataSet.Tables["2017_MUA"].Rows[0];
                        dr.BeginEdit();
                        dr["YearSol"] = insc.Fechaingreso.Value.Year;
                        dr["MesSol"] = insc.Fechaingreso.Value.Month;
                        dr["Diasol"] = insc.Fechaingreso.Value.Day;
                        dr["DiasDictamen"] = insc.DiasInscripcion;
                        dr["DictamensinFines"] = insc.DiasInscripcionSinFines;
                        dr["HorasDictamen"] = insc.HorasInscripcion;
                        dr["HorasSinFines"] = insc.HorasInscripcionSinfines;
                        dr.EndEdit();

                        dataAdapter.UpdateCommand = connection.CreateCommand();

                        dataAdapter.UpdateCommand.CommandText = "UPDATE 2017_MUA SET YearSol = @YearSol, MesSol = @MesSol, DiaSol = @DiaSol, DiasDictamen = @DiasDictamen," + 
                                " DictamensinFines = @DictamensinFines, HorasDictamen = @HorasDictamen, HorasSinfines = @HorasSinFines  WHERE Id = @Id"; //Aqui Tambien hay que validar con el nombre

                        dataAdapter.UpdateCommand.Parameters.Add("@YearSol", OleDbType.Numeric, 0, "YearSol");
                        dataAdapter.UpdateCommand.Parameters.Add("@MesSol", OleDbType.Numeric, 0, "MesSol");
                        dataAdapter.UpdateCommand.Parameters.Add("@DiaSol", OleDbType.Numeric, 0, "DiaSol");
                        dataAdapter.UpdateCommand.Parameters.Add("@DiasDictamen", OleDbType.Numeric, 0, "DiasDictamen");
                        dataAdapter.UpdateCommand.Parameters.Add("@DictamensinFines", OleDbType.Numeric, 0, "DictamensinFines");
                        dataAdapter.UpdateCommand.Parameters.Add("@HorasDictamen", OleDbType.Numeric, 0, "HorasDictamen");
                        dataAdapter.UpdateCommand.Parameters.Add("@HorasSinFines", OleDbType.Numeric, 0, "HorasSinFines");
                        dataAdapter.UpdateCommand.Parameters.Add("@Id", OleDbType.Numeric, 0, "Id");
                        dataAdapter.Update(dataSet, "2017_MUA");
                    }

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                    insertCompleted = true;
                }
                catch (OleDbException ex)
                {
                    //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    //ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
                }
                catch (Exception ex)
                {
                    //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    //ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
                }
                finally
                {
                    connection.Close();
                }
            }

        }


        /// <summary>
        /// Obtiene la fecha de inscripción y el número de días que tomó dictaminar una 
        /// solicitud sin contabilizar los fines de semana
        /// </summary>
        public void GetFechasMua()
        {
            List<Inscripcion> listaFechas = new List<Inscripcion>();

            SqlConnection connection = new SqlConnection(sqlConnectionString);
            SqlCommand cmd;
            SqlDataReader reader;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT Id, FechaSol,FechaDict FROM [2017MUA] WHERE YearSol IS NULL", connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Inscripcion insc = new Inscripcion();
                    insc.Id = Convert.ToInt32(reader["Id"]);
                    insc.Fechaingreso = DateTimeUtilities.GetDateFromReader(reader, "FechaSol");
                    insc.Fechainscripcion = DateTimeUtilities.GetDateFromReader(reader, "FechaDict");
                    insc.DiasInscripcion = (insc.Fechainscripcion.Value - insc.Fechaingreso.Value).TotalDays;
                    insc.HorasInscripcion = (insc.Fechainscripcion.Value - insc.Fechaingreso.Value).TotalHours;

                    int semanaSol = this.GetWeekNumber((DateTime)insc.Fechaingreso.Value);
                    int semanaDic = this.GetWeekNumber((DateTime)insc.Fechainscripcion.Value);

                    int semanasDiferencia = semanaDic - semanaSol;

                    if (semanaDic - semanaSol == 0)
                    {
                        insc.DiasInscripcionSinFines = insc.DiasInscripcion;
                        insc.HorasInscripcionSinfines = insc.HorasInscripcion;
                    }
                    else
                    {
                        insc.DiasInscripcionSinFines = insc.DiasInscripcion - (2 * semanasDiferencia);
                        insc.HorasInscripcionSinfines = insc.HorasInscripcion - (48 * semanasDiferencia);
                    }

                    listaFechas.Add(insc);
                }

                this.SetFechaReoprtada(listaFechas);

                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,InscripcionModel", "DGNMTools");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,InscripcionModel", "DGNMTools");
            }
            finally
            {
                connection.Close();
            }

        }


        /// <summary>
        /// Inserta a la base de datos la fecha y hora que se reportaran al Doing Bussines para efectos
        /// del tiempo de respuesta de una solicitud de denominación. Esta fecha toma como base la fecha de solicitud
        /// y le suma las horas laborales que tomó el dictamen. Posteriormente para evitar que el minuto 
        /// de dictamen sea el mismo de solicitud se genera un número aleatorio que va de -59 a 59 que 
        /// representa minutos y se le suma a la fecha señalada
        /// </summary>
        /// <param name="inscripciones"></param>
        public void SetFechaReoprtada(List<Inscripcion> inscripciones)
        {
            Random rnd = new Random();

            foreach (Inscripcion insc in inscripciones)
            {
                Console.WriteLine(insc.Id);
                SqlConnection connection = new SqlConnection(sqlConnectionString);
                SqlDataAdapter dataAdapter;
                SqlCommand cmd = connection.CreateCommand();
                cmd.Connection = connection;

                bool insertCompleted = false;

                DateTime fechaReporte = insc.Fechaingreso.Value.AddHours(insc.DiasInscripcionSinFines * 8);

                

                try
                {
                    connection.Open();

                    DataSet dataSet = new DataSet();
                    DataRow dr;

                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM [2017MUA] WHERE Id = @Id", connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@Id", insc.Id);
                    dataAdapter.Fill(dataSet, "2017_MUA");



                    if (dataSet.Tables["2017_MUA"].Rows.Count > 0)
                    {
                        dr = dataSet.Tables["2017_MUA"].Rows[0];
                        dr.BeginEdit();
                        dr["YearSol"] = insc.Fechaingreso.Value.Year;
                        dr["MesSol"] = insc.Fechaingreso.Value.Month;
                        dr["Diasol"] = insc.Fechaingreso.Value.Day;
                        dr["DiasDictamen"] = insc.DiasInscripcion;
                        dr["DictamensinFines"] = insc.DiasInscripcionSinFines;
                        dr["HorasDictamen"] = insc.HorasInscripcion;
                        dr["HorasSinFines"] = insc.HorasInscripcionSinfines;
                        dr["FechaRep"] = fechaReporte.AddMinutes(rnd.Next(-59,59));
                        dr["HorasLaborales"] = insc.DiasInscripcionSinFines * 8;
                        dr.EndEdit();

                        dataAdapter.UpdateCommand = connection.CreateCommand();

                        dataAdapter.UpdateCommand.CommandText = "UPDATE [2017MUA] SET YearSol = @YearSol, MesSol = @MesSol, DiaSol = @DiaSol, DiasDictamen = @DiasDictamen," +
                               " DictamensinFines = @DictamensinFines, HorasDictamen = @HorasDictamen, HorasSinfines = @HorasSinFines,FechaRep = @FechaRep,HorasLaborales = @HorasLaborales WHERE Id = @Id"; //Aqui Tambien hay que validar con el nombre

                        dataAdapter.UpdateCommand.Parameters.Add("@YearSol", SqlDbType.Int, 0, "YearSol");
                        dataAdapter.UpdateCommand.Parameters.Add("@MesSol", SqlDbType.Int, 0, "MesSol");
                        dataAdapter.UpdateCommand.Parameters.Add("@DiaSol", SqlDbType.Int, 0, "DiaSol");
                        dataAdapter.UpdateCommand.Parameters.Add("@DiasDictamen", SqlDbType.Int, 0, "DiasDictamen");
                        dataAdapter.UpdateCommand.Parameters.Add("@DictamensinFines", SqlDbType.Int, 0, "DictamensinFines");
                        dataAdapter.UpdateCommand.Parameters.Add("@HorasDictamen", SqlDbType.Int, 0, "HorasDictamen");
                        dataAdapter.UpdateCommand.Parameters.Add("@HorasSinFines", SqlDbType.Int, 0, "HorasSinFines");
                        dataAdapter.UpdateCommand.Parameters.Add("@FechaRep", SqlDbType.DateTime, 0, "FechaRep");
                        dataAdapter.UpdateCommand.Parameters.Add("@HorasLaborales", SqlDbType.Int, 0, "HorasLaborales");
                        dataAdapter.UpdateCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                        dataAdapter.Update(dataSet, "2017_MUA");
                    }

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                    insertCompleted = true;
                }
                catch (SqlException ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
                }
                catch (Exception ex)
                {
                    //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    //ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
                }
                finally
                {
                    connection.Close();
                }
            }

        }




        public int GetWeekNumber(DateTime fecha)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(fecha, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

    }
}
