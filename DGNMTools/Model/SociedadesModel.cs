using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using DGNMTools.Dto;
using ScjnUtilities;

namespace DGNMTools.Model
{
    public class SociedadesModel
    {

        private readonly string sqlConnectionString = ConfigurationManager.ConnectionStrings["TipoSociedad"].ConnectionString;

        public void GetSociedadesTocompare()
        {
            List<BasicEntityInfo> listaSociedades = new List<BasicEntityInfo>();

            SqlConnection connection = new SqlConnection(sqlConnectionString);
            SqlCommand cmd;
            SqlDataReader reader;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT Id, dsdensocial FROM Sociedades where CLAVE_FORMA in ('M4','M47')", connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BasicEntityInfo sociedad = new BasicEntityInfo();
                    sociedad.Id = Convert.ToInt32(reader["Id"]);
                    sociedad.Sociedad = reader["dsdensocial"].ToString();

                    listaSociedades.Add(sociedad);
                }


                reader.Close();
                cmd.Dispose();

                this.LookForSimilar(listaSociedades);
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



        private void LookForSimilar(List<BasicEntityInfo> listaSociedades)
        {

            foreach (BasicEntityInfo sociedad in listaSociedades)
            {
                if (sociedad.Sociedad.Length > 0)
                {

                    foreach (BasicEntityInfo soc in listaSociedades)
                    {
                        if (soc.Sociedad.Length > 0)
                        {

                            if (sociedad.Id != soc.Id)
                            {
                                double porcentaje = 0.0;
                                int total = StringUtilities.LevenshteinDistance(sociedad.Sociedad, soc.Sociedad, out porcentaje);

                                if (porcentaje < 0.10)
                                {
                                    MessageBox.Show("Similar");
                                }
                            }
                        }
                    }
                }

            }


        }


        public void GetSatInfo()
        {
            List<KeyValuePair<string, string>> listaSat = new List<KeyValuePair<string, string>>();

            SqlConnection connection = new SqlConnection(sqlConnectionString);
            SqlCommand cmd;
            SqlDataReader reader;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT rfc,denominacion FROM SatInfo ", connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    KeyValuePair<string, string> datos = new KeyValuePair<string, string>(reader["RFC"].ToString(),reader["Denominacion"].ToString());

                    listaSat.Add(datos);
                }


                reader.Close();
                cmd.Dispose();

                this.GetSatVsRpc(listaSat);

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


        private void GetSatVsRpc(List<KeyValuePair<string, string>> listaSat)
        {

            foreach (KeyValuePair<string, string> dato in listaSat)
            {

                SqlConnection connection = new SqlConnection(sqlConnectionString);
                SqlCommand cmd;
                SqlDataReader reader;

                try
                {
                    connection.Open();

                    cmd = new SqlCommand("SELECT * FROM Sociedades WHERE DsDenSocial LIKE '" + dato.Value + "%' ", connection);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        this.SetRelSat(reader, dato);
                    }


                    reader.Close();
                    cmd.Dispose();

                }
                catch (OleDbException ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    //ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ValidaSocioModel", "DGNMTools");
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    //ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ValidaSocioModel", "DGNMTools");
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public void SetRelSat(SqlDataReader reader, KeyValuePair<string,string> dato)
        {
            SqlConnection connection = new SqlConnection(sqlConnectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd;

            bool insertCompleted = false;

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO RelateRfcs(RFC,SatDenominacion,folio,rpcDenominacion,estado,municipio,C_Oficina,S_Oficina)" +
                                "VALUES (@RFC,@SatDen,@Folio,@RpcDen,@Estado,@Municipio,@Oficina,@Oficina2)";

                cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@RFC", dato.Key);
                cmd.Parameters.AddWithValue("@SatDen", dato.Value);
                cmd.Parameters.AddWithValue("@Folio", reader["folio"].ToString());
                cmd.Parameters.AddWithValue("@RpcDen", reader["DsDenSocial"].ToString());
                cmd.Parameters.AddWithValue("@Estado", reader["Estado"].ToString());
                cmd.Parameters.AddWithValue("@Municipio", reader["Municipio"].ToString());
                cmd.Parameters.AddWithValue("@Oficina", reader["C_Oficina"].ToString());
                cmd.Parameters.AddWithValue("@Oficina2", reader["S_Oficina"].ToString());
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

        }

    }
}
