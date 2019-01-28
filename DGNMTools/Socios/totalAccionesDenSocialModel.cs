using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using DGNMTools.Dto;
using ScjnUtilities;
using System.Data;

namespace DGNMTools.Socios
{
    public class totalAccionesDenSocialModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        private int cuantosErrores = 0;

        public ObservableCollection<TotalAccionesDensocial> GetSociedadesDosSocios()
        {
            ObservableCollection<TotalAccionesDensocial> sociedades = new ObservableCollection<TotalAccionesDensocial>();

            string sqlCadena = "select count(dsdensocial),crfme,dsdensocial from sociosacciones where (acciones is not null and total is not null) and (acciones <> '' and total <> '')  group by crfme,dsdensocial having count(dsdensocial) = 2 ";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            int queRegistro = 0;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TotalAccionesDensocial sociedad = new TotalAccionesDensocial();
                        sociedad.Folio = reader["CRFME"].ToString();
                        sociedad.DenSocial = reader["DSDENSOCIAL"].ToString();

                        sociedades.Add(sociedad);
                    }
                }
                cmd.Dispose();
                reader.Close();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,totalAccionesDenSocialModel", "DGNMTools");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,totalAccionesDenSocialModel", "DGNMTools");
            }
            finally
            {
                connection.Close();
            }

            this.GetSocios(sociedades);

            foreach (TotalAccionesDensocial sociedad in sociedades)
                this.SetSocForAnalisis(sociedad);

            return sociedades;
        }

        public void GetSocios(ObservableCollection<TotalAccionesDensocial> sociedades){

            int contadorSocs = 1;
            foreach (TotalAccionesDensocial sociedad in sociedades)
            {

                string sqlCadena = "select * from sociosacciones where crfme = @Folio and dsdensocial = @Den ";

                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cmd = null;
                SqlDataReader reader = null;

                int queRegistro = 0;

                try
                {
                    connection.Open();

                    cmd = new SqlCommand(sqlCadena, connection);
                    cmd.Parameters.AddWithValue("@Folio", sociedad.Folio);
                    cmd.Parameters.AddWithValue("@Den", sociedad.DenSocial);
                    reader = cmd.ExecuteReader();

                    List<Socio> socios = new List<Socio>();

                    while (reader.Read())
                    {
                        Socio socio = new Socio();

                        if (reader["Acciones"].ToString().Contains("+"))
                        {
                            decimal d = Decimal.Parse(reader["Acciones"].ToString(), System.Globalization.NumberStyles.Float);
                            sociedad.TotalAcciones += Convert.ToInt64(d);
                            socio.Accion = Convert.ToInt64(d);
                        }
                        else
                        {
                            //sociedad.TotalAcciones += reader["Acciones"] as int? ?? 0;
                            //socio.Accion = reader["Acciones"] as int? ?? 0;

                            sociedad.TotalAcciones += Convert.ToInt64(reader["Acciones"]);
                            socio.Accion = Convert.ToInt64(reader["Acciones"]);
                        }

                        if (reader["Total"].ToString().Contains("+"))
                        {
                            decimal d = Decimal.Parse(reader["Total"].ToString(), System.Globalization.NumberStyles.Float);
                            sociedad.TotalValor += Convert.ToInt64(d);
                            socio.Total = Convert.ToInt64(d);
                        }
                        else
                        {
                            //sociedad.TotalValor += reader["Total"] as int? ?? 0;
                            //socio.Total = reader["Total"] as int? ?? 0;

                            sociedad.TotalValor += Convert.ToInt64(reader["Total"]);
                            socio.Total = Convert.ToInt64(reader["Total"]);
                        }
                        socios.Add(socio);
                    }

                    Int64 socio1Acciones = socios[0].Accion;
                    Int64 socio2Acciones = socios[1].Accion;

                    if (socio1Acciones > socio2Acciones)
                    {
                        sociedad.PorcentajeAcciones = ObtenerPorcentaje(sociedad.TotalAcciones, socio1Acciones);
                    }
                    else if (socio2Acciones > socio1Acciones)
                    {
                        sociedad.PorcentajeAcciones = ObtenerPorcentaje(sociedad.TotalAcciones, socio2Acciones);
                    }
                    else
                    {
                        sociedad.PorcentajeAcciones = 50;
                    }

                    Int64 socio1Total = socios[0].Total;
                    Int64 socio2Total = socios[1].Total;

                    if (socio1Total > socio2Total)
                    {
                        sociedad.Porcentajevalor = ObtenerPorcentaje(sociedad.TotalValor, socio1Total);
                    }
                    else if (socio2Total > socio1Total)
                    {
                        sociedad.Porcentajevalor = ObtenerPorcentaje(sociedad.TotalValor, socio2Total);
                    }
                    else
                    {
                        sociedad.Porcentajevalor = 50;
                    }


                    cmd.Dispose();
                    reader.Close();

                }
                catch (SqlException ex)
                {
                    //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    //ErrorUtilities.SetNewErrorMessage(ex, methodName + sociedad.Folio + " Exception,totalAccionesDenSocialModel", "DGNMTools");
                    cuantosErrores++;
                }
                catch (Exception ex)
                {
                    //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    //ErrorUtilities.SetNewErrorMessage(ex, methodName + sociedad.Folio + " Exception,totalAccionesDenSocialModel", "DGNMTools");
                    cuantosErrores++;
                }
                finally
                {
                    connection.Close();
                }

                Console.WriteLine(contadorSocs);
                contadorSocs++;
            }
            



        }


        public bool SetSocForAnalisis(TotalAccionesDensocial sociedad)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO AnalisisPorcentajes(Crfme,dsdensocial,totalacciones,totalvalor,porcentajeacciones,porcentajevalor)" +
                                "VALUES (@Crfme,@dsdensocial,@totalacciones,@totalvalor,@porcentajeacciones,@porcentajevalor)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Crfme", sociedad.Folio);
                cmd.Parameters.AddWithValue("@dsdensocial", sociedad.DenSocial);
                cmd.Parameters.AddWithValue("@totalacciones", sociedad.TotalAcciones);
                cmd.Parameters.AddWithValue("@totalvalor", sociedad.TotalValor);
                cmd.Parameters.AddWithValue("@porcentajeacciones", sociedad.PorcentajeAcciones);
                cmd.Parameters.AddWithValue("@porcentajevalor", sociedad.Porcentajevalor);
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



        private Int64 ObtenerPorcentaje(Int64 sumaTotal, Int64 cuantoTiene)
        {
            return (cuantoTiene * 100) / sumaTotal;
        }



        public List<int> GetSociedadesDosSociosNLCD()
        {
            List<int> socs2Socios = new List<int>();

            string sqlCadena = "select distinct crfme from nuevoleon group by crfme having count(crfme) = 2";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            int queRegistro = 0;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        socs2Socios.Add(Convert.ToInt32(reader["crfme"]));
                    }
                }
                cmd.Dispose();
                reader.Close();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,totalAccionesDenSocialModel", "DGNMTools");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,totalAccionesDenSocialModel", "DGNMTools");
            }
            finally
            {
                connection.Close();
            }
            return socs2Socios;
           
        }


        public List<InfoNlyCd> GetSociosNlyDf()
        {

            List<InfoNlyCd> infoCompleta = new List<InfoNlyCd>();

            string sqlCadena = "select * from NuevoLeon where crfme in (select distinct crfme from nuevoleon group by crfme having count(crfme) = 2 )";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            string queRegistro = "";

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        InfoNlyCd info = new InfoNlyCd();
                        info.Folio = reader["Folio"].ToString();
                        queRegistro = info.Folio;
                        info.IdGen = Convert.ToInt32(reader["IdGen"]);
                        info.Oficina = reader["Oficina"].ToString();
                        //info.Nombre = reader["Nombre"].ToString();
                        //info.Paterno = reader["Paterno"].ToString();
                        //info.Materno = reader["Materno"].ToString();
                        info.NumAcciones = Convert.ToInt32(reader["NumAcciones"]);
                        info.ValorAcciones = Convert.ToInt32(reader["valorAcciones"]);
                        info.Total = reader["Total"] as int? ?? 0;
                        info.MainId = Convert.ToInt32(reader["CrFme"]);

                        infoCompleta.Add(info);

                    }
                }
                cmd.Dispose();
                reader.Close();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,totalAccionesDenSocialModel", "DGNMTools");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,totalAccionesDenSocialModel", "DGNMTools");
            }
            finally
            {
                connection.Close();
            }

            return infoCompleta;


        }

        public bool SetSocForAnalisisNLCD(TotalAccionesDensocial sociedad)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO AnalisisPorcentajesNLCD(Crfme,dsdensocial,totalacciones,totalvalor,porcentajeacciones,porcentajevalor)" +
                                "VALUES (@Crfme,@dsdensocial,@totalacciones,@totalvalor,@porcentajeacciones,@porcentajevalor)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Crfme", sociedad.Folio);
                cmd.Parameters.AddWithValue("@dsdensocial", sociedad.DenSocial);
                cmd.Parameters.AddWithValue("@totalacciones", sociedad.TotalAcciones);
                cmd.Parameters.AddWithValue("@totalvalor", sociedad.TotalValor);
                cmd.Parameters.AddWithValue("@porcentajeacciones", sociedad.PorcentajeAcciones);
                cmd.Parameters.AddWithValue("@porcentajevalor", sociedad.Porcentajevalor);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error");
                //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }


    }
}
