using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using DGNMTools.Dto;
using ScjnUtilities;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using DGNMTools.Genero;

namespace DGNMTools.Model
{
    public class NombreModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        /// <summary>
        /// Obtiene la lista completa de los nombres que se encuentran en el padròn de distribuciòn de la SCJN
        /// que ya estàn clasificados de acuerdo al gènero
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Nombre> GetListaPadron()
        {
            ObservableCollection<Nombre> catalogoTitulares = new ObservableCollection<Nombre>();

            string sqlCadena = "select distinct Nombre, genero from ListaNombresBase where Nombre IS NOT NULL";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            int queRegistro = 0;

            List<string> nombresAgregados = new List<string>();

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Nombre nombre = new Nombre();
                        //nombre.IdNombre = Convert.ToInt32(reader["IdTitular"]);
                        nombre.NombreDesc = reader["Nombre"].ToString();
                        nombre.NombreString = StringUtilities.PrepareToAlphabeticalOrder(nombre.NombreDesc);
                        nombre.Genero = Convert.ToInt16(reader["Genero"]); //1 - hombre 2 -mujer
                        catalogoTitulares.Add(nombre);

                        string[] splitName = nombre.NombreDesc.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (splitName.Count() > 1)
                        {

                            foreach (string nomComp in splitName)
                            {
                                if (nomComp.Equals("MARIA") || nomComp.Equals("JOSE") || nomComp.Equals("GUADALUPE") || nomComp.Length < 3)
                                {
                                    //No hace nada
                                }
                                else
                                {
                                    if (!nombresAgregados.Contains(nomComp))
                                    {
                                        Nombre sName = new Nombre();
                                        sName.NombreDesc = nomComp;
                                        sName.NombreString = StringUtilities.PrepareToAlphabeticalOrder(nomComp);
                                        sName.Genero = Convert.ToInt16(nombre.Genero); //
                                        catalogoTitulares.Add(sName);
                                        nombresAgregados.Add(nomComp);
                                    }
                                }
                            }
                        }
                    }
                }
                cmd.Dispose();
                reader.Close();


            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTitulares;
        }


        /// <summary>
        /// Obtiene la lista completa de los socios que se tienen registrados en el SIGER 
        /// en la primer etapa de bùsqueda todos los registros tienen Null en el campo Genero
        /// en la segunda etapa solo se devuelven aquellos que no tuvieron coincidencias en la primera etapa
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Nombre> GetSociosSiger()
        {
            ObservableCollection<Nombre> catalogoTitulares = new ObservableCollection<Nombre>();

            string sqlCadena = "select distinct top 500000 * from SociosSiger where genero is null order by dsnombresocio";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            int queRegistro = 0;

            string folio = String.Empty;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Nombre nombre = new Nombre();
                        folio = reader["crfme"].ToString();
                        nombre.Folio = folio;
                        nombre.IdNombre = Convert.ToInt32(reader["IdSocio"]);
                        nombre.NombreDesc = reader["DsNombreSocio"].ToString();
                        nombre.NombreString = StringUtilities.PrepareToAlphabeticalOrder(nombre.NombreDesc);
                        //nombre.Genero = Convert.ToInt16(reader["Genero"]); //1 - hombre 2 -mujer

                        catalogoTitulares.Add(nombre);
                    }
                }
                cmd.Dispose();
                reader.Close();


            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel" + folio, "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel" + folio, "Padron");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTitulares;
        }

        /// <summary>
        /// Devuelve unicamente el nombre del socio para ser clasificado de esta forma si funciona la claúsula distinct
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Nombre> GetSociosSigerForClasif()
        {
            ObservableCollection<Nombre> catalogoTitulares = new ObservableCollection<Nombre>();

            string sqlCadena = "select distinct DSNOMBRESOCIO from SociosSiger where genero is null order by dsnombresocio";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            int queRegistro = 0;

            string folio = String.Empty;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Nombre nombre = new Nombre();
                        nombre.NombreDesc = reader["DsNombreSocio"].ToString();
                        nombre.NombreString = StringUtilities.PrepareToAlphabeticalOrder(nombre.NombreDesc);
                        //nombre.Genero = Convert.ToInt16(reader["Genero"]); //1 - hombre 2 -mujer

                        catalogoTitulares.Add(nombre);
                    }
                }
                cmd.Dispose();
                reader.Close();


            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel" + folio, "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel" + folio, "Padron");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTitulares;
        }


        public ObservableCollection<Nombre> GetSociosSigerForClasif(int bottomBase)
        {
            ObservableCollection<Nombre> catalogoTitulares = new ObservableCollection<Nombre>();

            //string sqlCadena = String.Format("select distinct DSNOMBRESOCIO from SociosSiger where (IdSocio BETWEEN {0} and {1}) AND genero is null ", bottomBase, bottomBase + 100);

            string sqlCadena = String.Format("select distinct TOP 250 DSNOMBRESOCIO from SociosSiger where genero is null order by dsnombresocio desc");

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            int queRegistro = 0;

            string folio = String.Empty;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Nombre nombre = new Nombre();
                        nombre.NombreDesc = reader["DsNombreSocio"].ToString();
                        nombre.NombreString = StringUtilities.PrepareToAlphabeticalOrder(nombre.NombreDesc);
                        //nombre.Genero = Convert.ToInt16(reader["Genero"]); //1 - hombre 2 -mujer

                        catalogoTitulares.Add(nombre);
                    }
                }
                cmd.Dispose();
                reader.Close();


            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel" + folio, "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel" + folio, "Padron");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTitulares;
        }


        public ObservableCollection<GenPorYear> GetTotalPorGenero()
        {
            ObservableCollection<GenPorYear> generoPorAnio = new ObservableCollection<GenPorYear>();

            string sqlCadena = "select YEAR(FCINGRESO) ANIO, COUNT(FCINGRESO) TOTAL, " +
                               "(SELECT count(SG.GENERO) FROM SOCIOSSIGER SG WHERE GENERO = 1 AND YEAR(SG.FCINGRESO) = YEAR(SOCIOSSIGER.FCINGRESO)) HOMBRES, " +
                               "(SELECT count(SG.GENERO) FROM SOCIOSSIGER SG WHERE GENERO = 2 AND YEAR(SG.FCINGRESO) = YEAR(SOCIOSSIGER.FCINGRESO)) MUJERES, " +
                               "(SELECT count(SG.GENERO) FROM SOCIOSSIGER SG WHERE GENERO is null AND YEAR(SG.FCINGRESO) = YEAR(SOCIOSSIGER.FCINGRESO)) NODEF " +
                               "FROM SOCIOSSIGER  WHERE YEAR(FCINGRESO) >= 2012 GROUP BY YEAR(FCINGRESO) ORDER BY YEAR(FCINGRESO)";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            int queRegistro = 0;

            string folio = String.Empty;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        GenPorYear genero = new GenPorYear();
                        genero.Year = Convert.ToInt32(reader["Anio"]);
                        genero.Hombres = Convert.ToInt32(reader["Hombres"]);
                        genero.Mujeres = Convert.ToInt32(reader["Mujeres"]);
                        genero.Total = genero.Hombres + genero.Mujeres;

                        generoPorAnio.Add(genero);
                    }
                }
                cmd.Dispose();
                reader.Close();


            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel" + folio, "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel" + folio, "Padron");
            }
            finally
            {
                connection.Close();
            }

            return generoPorAnio;
        }



        public int GetSociosCount()
        {
            ObservableCollection<Nombre> catalogoTitulares = new ObservableCollection<Nombre>();

            string sqlCadena = "select COUNT(IdSocio) Total from SociosSiger";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            int totalRegistros = 0;

            

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        totalRegistros = Convert.ToInt32(reader["Total"]);
                    }
                }
                cmd.Dispose();
                reader.Close();


            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel" , "Padron");
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

            return totalRegistros;
        }


        public void SetGeneroSocio(Nombre nombre, int genero)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd;
            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool insertCompleted = false;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                string sqlQuery = "SELECT * FROM SociosSiger WHERE CrFme = @CrFme";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@CrFme", nombre.Folio);
                dataAdapter.Fill(dataSet, "SociosSiger");

                if (dataSet.Tables["SociosSiger"].Rows.Count > 0)
                {
                    dr = dataSet.Tables["SociosSiger"].Rows[0];
                    dr.BeginEdit();
                    dr["Genero"] = genero;
                    dr.EndEdit();

                    dataAdapter.UpdateCommand = connection.CreateCommand();

                    dataAdapter.UpdateCommand.CommandText = "UPDATE SociosSiger SET Genero = @Genero WHERE CrFme = @CrFme"; //Aqui Tambien hay que validar con el nombre

                    dataAdapter.UpdateCommand.Parameters.Add("@Genero", SqlDbType.Int, 0, "Genero");
                    dataAdapter.UpdateCommand.Parameters.Add("@CrFme", SqlDbType.VarChar, 0, "CrFme");
                    dataAdapter.Update(dataSet, "SociosSiger");
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
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

        }

        public bool InsertaNombreBase(Nombre nombre, int genero)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO ListaNombresBase(Nombre,Genero)" +
                                "VALUES (@Nombre,@Genero)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Nombre", StringUtilities.PrepareToAlphabeticalOrder( nombre.NombreDesc));
                cmd.Parameters.AddWithValue("@Genero", genero);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
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

            return insertCompleted;
        }



        public List<GeneroPorAnio> GetFechasToConvert()
        {
            List<GeneroPorAnio> fechas = new List<GeneroPorAnio>();

            string sqlCadena = "select idSocio, fcinscripcion, genero from SociosSiger";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            int queRegistro = 0;

            string folio = String.Empty;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        GeneroPorAnio year = new GeneroPorAnio();
                        year.IdSocio = Convert.ToInt32(reader["IdSocio"]);
                        year.FechaSocio = DateTimeUtilities.GetDateFromReader(reader, "FcInscripcion");
                        year.Genero = Convert.ToInt32(reader["Genero"]);
                        year.FechaInt = Convert.ToInt32(DateTimeUtilities.DateToInt(year.FechaSocio));

                        fechas.Add(year);
                    }
                }
                cmd.Dispose();
                reader.Close();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel" + folio, "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel" + folio, "Padron");
            }
            finally
            {
                connection.Close();
            }
            return fechas;
        }

        public void SetFechaInscripcion(List<KeyValuePair<int,DateTime?>> fechas)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd;
            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool insertCompleted = false;

            try
            {
                connection.Open();

                foreach (KeyValuePair<int, DateTime?> fecha in fechas)
                {

                    DataSet dataSet = new DataSet();
                    DataRow dr;

                    string sqlQuery = "SELECT * FROM SociosSiger WHERE IdSocio = @CrFme";

                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(sqlQuery, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@CrFme", fecha.Key);
                    dataAdapter.Fill(dataSet, "SociosSiger");

                    if (dataSet.Tables["SociosSiger"].Rows.Count > 0)
                    {
                        dr = dataSet.Tables["SociosSiger"].Rows[0];
                        dr.BeginEdit();
                        dr["Inscripcion"] = fecha.Value;
                        dr.EndEdit();

                        dataAdapter.UpdateCommand = connection.CreateCommand();

                        dataAdapter.UpdateCommand.CommandText = "UPDATE SociosSiger SET Inscripcion = @Inscripcion WHERE IdSocio = @IdSocio"; //Aqui Tambien hay que validar con el nombre

                        dataAdapter.UpdateCommand.Parameters.Add("@Inscripcion", SqlDbType.DateTime, 0, "Inscripcion");
                        dataAdapter.UpdateCommand.Parameters.Add("@IdSocio", SqlDbType.Int, 0, "IdSocio");
                        dataAdapter.Update(dataSet, "SociosSiger");
                    }

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                }
                
            }
            catch (SqlException ex)
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

    }
}
