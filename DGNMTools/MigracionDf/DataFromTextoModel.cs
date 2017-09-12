using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using DGNMTools.Dto;
using ScjnUtilities;
using System.Windows;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DGNMTools.MigracionDf
{
    public class DataFromTextoModel
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["BaseDf"].ConnectionString;


        public ObservableCollection<DataFromTexto> GetCampoTexto()
        {
            ObservableCollection<DataFromTexto> listaTexto = new ObservableCollection<DataFromTexto>();

            //string oleDbCadena = "select * from Municipio where esConstitucion = 1";
            string oleDbCadena = "select * from FedatariosNull";

            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;
            OleDbDataReader reader = null;

            int folio = 0;

            try
            {
                connection.Open();

                cmd = new OleDbCommand(oleDbCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DataFromTexto datos = new DataFromTexto();
                        datos.Id = Convert.ToInt32(reader["Id"]);
                        datos.Folio = Convert.ToInt32(reader["Folio"]);
                        folio = datos.Folio;
                        datos.Texto = reader["Texto"].ToString();
                        datos.TextoStr = VerificationUtilities.TextBoxStringValidation(StringUtilities.PrepareToAlphabeticalOrder(datos.Texto));

                        //if(!datos.TextoStr.Contains("SOCIEDAD COOPERATIVA"))
                            listaTexto.Add(datos);
                    }
                }
                cmd.Dispose();
                reader.Close();


            }
            catch (OleDbException ex)
            {
                MessageBox.Show(folio.ToString());
//                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
  //              ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                MessageBox.Show(folio.ToString());
                //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

            foreach (DataFromTexto data in listaTexto)
                this.GetMissingData(data);

            MessageBox.Show("Finito");
            return listaTexto;
        }

        public ObservableCollection<DataFromTexto> GetCampoTextoSinGuinoes()
        {
            ObservableCollection<DataFromTexto> listaTexto = new ObservableCollection<DataFromTexto>();

            //string oleDbCadena = "select * from Municipio where esConstitucion = 1";
            string oleDbCadena = "select * from Nombram where id < 65876 Order by id desc";

            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;
            OleDbDataReader reader = null;

            int folio = 0;
            
            try
            {
                connection.Open();

                cmd = new OleDbCommand(oleDbCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DataFromTexto datos = new DataFromTexto();
                        datos.Id = Convert.ToInt32(reader["Id"]);
                        datos.Folio = Convert.ToInt32(reader["Folio"]);
                        folio = datos.Folio;
                        datos.Texto = reader["Texto"].ToString();

                        if (!String.IsNullOrWhiteSpace(datos.Texto) && !String.IsNullOrEmpty(datos.Texto))
                        {
                            datos.TextoStr = VerificationUtilities.TextBoxStringValidation(StringUtilities.PrepareToAlphabeticalOrder(datos.Texto)).Replace("-", "");
                            listaTexto.Add(datos);
                        }
                        Console.WriteLine(datos.Id);
                    }
                }
                cmd.Dispose();
                reader.Close();


            }
            catch (OleDbException ex)
            {
                MessageBox.Show(folio.ToString());
                //                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //              ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                MessageBox.Show(folio.ToString());
                //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

            int totalErrores = 0;

            foreach (DataFromTexto data in listaTexto)
            {
                if (!this.GetRelexInfo(data, totalErrores))
                    totalErrores++;
            }

            MessageBox.Show(String.Format("{0} Errores de {1} registros",totalErrores,listaTexto.Count));
            return listaTexto;
        }


        public void SetEsConstitucion(DataFromTexto datos)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbDataAdapter dataAdapter;
            OleDbCommand cmd;
            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool insertCompleted = false;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                string OleDbQuery = "SELECT * FROM Municipio WHERE Id = @Id";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(OleDbQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@Id", datos.Id);
                dataAdapter.Fill(dataSet, "Municipio");

                if (dataSet.Tables["Municipio"].Rows.Count > 0)
                {
                    dr = dataSet.Tables["Municipio"].Rows[0];
                    dr.BeginEdit();
                    dr["EsConstitucion"] = datos.EsConstitucion;
                    dr.EndEdit();

                    dataAdapter.UpdateCommand = connection.CreateCommand();

                    dataAdapter.UpdateCommand.CommandText = "UPDATE Municipio SET EsConstitucion = @EsConstitucion WHERE Id = @Id"; //Aqui Tambien hay que validar con el nombre

                    dataAdapter.UpdateCommand.Parameters.Add("@EsConstitucion", OleDbType.Numeric, 0, "EsConstitucion");
                    dataAdapter.UpdateCommand.Parameters.Add("@Id", OleDbType.Numeric, 0, "Id");
                    dataAdapter.Update(dataSet, "Municipio");
                }

                dataSet.Dispose();
                dataAdapter.Dispose();
                insertCompleted = true;
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

        }


        public void GetMissingData(DataFromTexto datos)
        {
            try
            {
                int startIndex = datos.Texto.IndexOf("ANTE NOTARIO");

                if (startIndex != -1)
                {
                    string tempString = datos.Texto.Substring(startIndex);

                    int endIndex = tempString.IndexOf("A CONSTITUIR");

                    if (endIndex != -1)
                    {
                        tempString = tempString.Substring(0, endIndex);
                        tempString = tempString.Replace("ANTE ", "");

                        int indezNum = tempString.IndexOf("NO.");

                        datos.TipoFedatarios = tempString.Substring(0, indezNum);

                        tempString = VerificationUtilities.TextBoxStringValidation(tempString.Substring(indezNum + 3));

                        int defaultNumber = 0;
                        Int32.TryParse(tempString.Split(' ')[0], out defaultNumber);
                        datos.NumFedObt = defaultNumber;

                        if (tempString.Contains("MUNICIPIO"))
                        {
                            tempString = tempString.Substring(tempString.IndexOf("MUNICIPIO"));
                            datos.MunicipioObt = VerificationUtilities.TextBoxStringValidation(tempString.Substring(0, tempString.IndexOf("ESTADO")).Replace("MUNICIPIO", ""));
                        }

                        if (tempString.Contains("ESTADO"))
                        {
                            tempString = tempString.Substring(tempString.IndexOf("ESTADO"));

                            datos.EstadoObt = tempString.Substring(0, tempString.IndexOf("NOMBRE")).Replace("ESTADO", "");
                        }

                        datos.FedatarioObt = tempString.Substring(tempString.IndexOf("NOMBRE")).Replace("NOMBRE", "").Replace("COMPARECE (N)", "").Replace("-", "");

                        this.SetMissingInfo(datos);
                    }

                }
                else
                {
                    MessageBox.Show(datos.Id.ToString());
                }
            }
            catch (ArgumentOutOfRangeException) { }

        }


        public void SetMissingInfo(DataFromTexto datos)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbDataAdapter dataAdapter;
            OleDbCommand cmd;
            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool insertCompleted = false;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                string OleDbQuery = "SELECT * FROM FedatariosNull WHERE Id = @Id";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(OleDbQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@Id", datos.Id);
                dataAdapter.Fill(dataSet, "FedatariosNull");

                if (dataSet.Tables["FedatariosNull"].Rows.Count > 0)
                {
                    dr = dataSet.Tables["FedatariosNull"].Rows[0];
                    dr.BeginEdit();
                    dr["NumFedatarioObt"] = datos.NumFedObt;
                    dr["MunicipioObt"] = datos.MunicipioObt;
                    dr["EstadoObt"] = datos.EstadoObt;
                    dr["FedatarioObt"] = datos.FedatarioObt;
                    dr["TipoFedatario"] = datos.TipoFedatarios;
                    dr.EndEdit();

                    dataAdapter.UpdateCommand = connection.CreateCommand();

                    dataAdapter.UpdateCommand.CommandText = "UPDATE FedatariosNull SET NumFedatarioObt = @NumFedatarioObt, MunicipioObt = @MunicipioObt, EstadoObt = @EstadoObt, FedatarioObt = @FedatarioObt, TipoFedatario = @TipoFedatario WHERE Id = @Id"; //Aqui Tambien hay que validar con el nombre

                    dataAdapter.UpdateCommand.Parameters.Add("@NumFedatarioObt", OleDbType.Numeric, 0, "NumFedatarioObt");
                    dataAdapter.UpdateCommand.Parameters.Add("@MunicipioObt", OleDbType.VarChar, 0, "MunicipioObt");
                    dataAdapter.UpdateCommand.Parameters.Add("@EstadoObt", OleDbType.VarChar, 0, "EstadoObt");
                    dataAdapter.UpdateCommand.Parameters.Add("@FedatarioObt", OleDbType.VarChar, 0, "FedatarioObt");
                    dataAdapter.UpdateCommand.Parameters.Add("@TipoFedatario", OleDbType.VarChar, 0, "TipoFedatario");
                    dataAdapter.UpdateCommand.Parameters.Add("@Id", OleDbType.Numeric, 0, "Id");
                    dataAdapter.Update(dataSet, "FedatariosNull");
                }

                dataSet.Dispose();
                dataAdapter.Dispose();
                insertCompleted = true;
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

        }

        public bool GetRelexInfo(DataFromTexto datos, int errores)
        {
            try
            {
                int startIndex = datos.TextoStr.IndexOf("RELACIONES EXTERIORES");

                if (startIndex != -1)
                {

                    string tempString = datos.TextoStr.Substring(startIndex);

                    int endIndex = tempString.IndexOf("NO");

                    if (endIndex != -1)
                    {
                        tempString = tempString.Substring(endIndex + 3);

                        string permisoRelex = tempString.Substring(0, tempString.IndexOf(' '));

                        datos.PermisoRelex = permisoRelex;

                        tempString = tempString.Substring(tempString.IndexOf(' ') + 1).Replace("DE FECHA ","");

                        string[] completeText = tempString.Split(' ');


                        string fechaRelex = String.Format(completeText[0] + "{0}" + this.SetMonthLanguaje(completeText[1]) + "{0}" + completeText[2], "-");

                        DateTime dt = Convert.ToDateTime(fechaRelex,new CultureInfo("es-ES"));

                        datos.FechaPermisoRelex = dt.ToString("dd/MM/yyyy");
                        Console.WriteLine(String.Format("{0}    {1}",datos.Id, datos.FechaPermisoRelex));

                        tempString = tempString.Substring(tempString.IndexOf("NO")).Replace("NO ","");

                        //string expedienteRelex = tempString.Substring(0, tempString.IndexOf(' '));
                        //datos.ExpedienteRelex = expedienteRelex;

                        string originalSinGuion = StringUtilities.ReplaceDoubleSpaces( Regex.Replace(datos.Texto, "--+", "-"));

                        startIndex = datos.Texto.IndexOf("EXPEDIENTE NO.");

                        string[] orgString = datos.Texto.Substring(startIndex).Replace("EXPEDIENTE NO. ", "").Split(new[] { " ", "" }, StringSplitOptions.RemoveEmptyEntries);

                        char[] expediente = orgString[0].ToCharArray();

                        try { 
                        if (!VerificationUtilities.IsNumber(Char.ToString(expediente[0])))
                        {
                            datos.ExpedienteRelex = orgString[0];
                        }
                        else
                            datos.ExpedienteRelex = "0";
                        }
                        catch (Exception)
                        {
                            datos.ExpedienteRelex = "0";
                        }


                       // datos.TipoFedatarios = tempString.Substring(0, indezNum);

                       this.SetRelexInfo(datos);
                        
                    }

                }
                else
                {
                    //MessageBox.Show(datos.Id.ToString());
                }
            }
            catch (ArgumentOutOfRangeException) {
                //MessageBox.Show(datos.Id.ToString() + "Error");
                return false;
            }
            catch (FormatException)
            {
                //MessageBox.Show(datos.Id.ToString() + "Error");
                return false;
            }
            return true;
        }


        public void SetRelexInfo(DataFromTexto datos)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbDataAdapter dataAdapter;
            OleDbCommand cmd;
            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool insertCompleted = false;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                string OleDbQuery = "SELECT * FROM Nombram WHERE Id = @Id";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(OleDbQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@Id", datos.Id);
                dataAdapter.Fill(dataSet, "Nombram");

                if (dataSet.Tables["Nombram"].Rows.Count > 0)
                {
                    dr = dataSet.Tables["Nombram"].Rows[0];
                    dr.BeginEdit();
                    dr["PermisoRelex"] = datos.PermisoRelex;
                    dr["FechaPermisoRelex"] = datos.FechaPermisoRelex;
                    dr["ExpedienteRelex"] = datos.ExpedienteRelex;
                    dr.EndEdit();

                    dataAdapter.UpdateCommand = connection.CreateCommand();

                    dataAdapter.UpdateCommand.CommandText = "UPDATE Nombram SET PermisoRelex = @PermisoRelex, FechaPermisoRelex = @FechaPermisoRelex, ExpedienteRelex = @ExpedienteRelex WHERE Id = @Id"; //Aqui Tambien hay que validar con el nombre

                    dataAdapter.UpdateCommand.Parameters.Add("@PermisoRelex", OleDbType.VarChar, 0, "PermisoRelex");
                    dataAdapter.UpdateCommand.Parameters.Add("@FechaPermisoRelex", OleDbType.VarChar, 0, "FechaPermisoRelex");
                    dataAdapter.UpdateCommand.Parameters.Add("@ExpedienteRelex", OleDbType.VarChar, 0, "ExpedienteRelex");
                    dataAdapter.UpdateCommand.Parameters.Add("@Id", OleDbType.Numeric, 0, "Id");
                    dataAdapter.Update(dataSet, "Nombram");
                }

                dataSet.Dispose();
                dataAdapter.Dispose();
                insertCompleted = true;
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

        }


        private string SetMonthLanguaje(string month)
        {
            int mes = 0;
            Int32.TryParse(month, out mes);

            if (mes == 0)
            {
                switch (month.ToUpper())
                {
                    case "ENE": return "JAN";
                    case "ABR": return "APR";
                    case "AGO": return "AUG";
                    case "DIC": return "DEC";
                    default: return month.ToUpper();
                }
            }

            return month;
        }


    }
}
