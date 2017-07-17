using System;
using System.Data.SqlClient;
using System.Linq;
using DGNMTools.Dto;
using ScjnUtilities;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Windows;
using System.Data.OleDb;
using System.Data.OracleClient;

namespace DGNMTools.Model
{
    public class BasicEntityInfoModel
    {

        private string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;
        private string connectionString2 = ConfigurationManager.ConnectionStrings["Espejo"].ConnectionString;





        /// <summary>
        /// Obtiene el listado de todas aquellas sociedades que no tienen asignado el tipo de sociedad
        /// </summary>
        /// <returns></returns>
        public void GetEntitiesForReasign(int tipoBusqueda)
        {
            ObservableCollection<C_TipoSociedades> tiposCatalogo = new C_TipoSociedadesModel().GetTipoSociedades();

            ObservableCollection<BasicEntityInfo> obrasSinPadron = new ObservableCollection<BasicEntityInfo>();

            string sqlCadena = "SELECT Id,DsDenSocial1,DsTipoSociedad1 FROM OneYear WHERE DsTipoSociedad1 IS NULL ORDER BY Id desc";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            int idCurrent = 0;
            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();
                int totalReview = 0, totalupdates = 0;

                if (tipoBusqueda == 1)
                    this.NoSpacesProces(reader, tiposCatalogo);
                else if (tipoBusqueda == 2)
                    this.AfterComaProces(reader, tiposCatalogo);



                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel" + idCurrent, "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel" + idCurrent, "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            
        }


        /// <summary>
        /// Elimina los espacios de la denominación y busca si es que dicha denominación culmina con 
        /// alguna de las siglas del catálogo de tipo de denominaciones
        /// </summary>
        private void NoSpacesProces(SqlDataReader reader, ObservableCollection<C_TipoSociedades> tiposCatalogo)
        {
            int idCurrent = 0;
            int totalReview = 0, totalupdates = 0;
            while (reader.Read())
            {
                try
                {
                    BasicEntityInfo info = new BasicEntityInfo();
                    info.Id = Convert.ToInt32(reader["Id"]);

                    idCurrent = info.Id;
                    info.Sociedad = reader["DsDenSocial1"].ToString();

                    if (!string.IsNullOrEmpty(info.Sociedad))
                    {
                        info.SociedadStr = StringUtilities.PrepareToAlphabeticalOrder(info.Sociedad).Replace(" ", "");
                        Console.WriteLine(info.Id);
                        totalReview++;

                        bool find = false;
                        foreach (C_TipoSociedades tipo in tiposCatalogo)
                        {
                            if (info.SociedadStr.EndsWith(tipo.SiglasStr))
                            {
                                this.UpdateOrden(info, tipo);
                                totalupdates++;
                                find = true;
                            }
                            else if (info.SociedadStr.Contains(tipo.SubtipoStr))
                            {
                                this.UpdateOrden(info, tipo);
                                totalupdates++;
                                find = true;
                            }


                            if (find)
                                break;
                        }
                    }

                }
                catch (SqlException ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel" + idCurrent, "PadronApi");
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel" + idCurrent, "PadronApi");
                }


            }

            MessageBox.Show(String.Format("Total revisados: {0}", totalReview));
            MessageBox.Show(String.Format("Total actualizados: {0}", totalupdates));
        }



        /// <summary>
        /// Verifica si la información que esta después de la última "," coincide con las siglas de los tipos de denominaciones
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="tiposCatalogo"></param>
        private void AfterComaProces(SqlDataReader reader, ObservableCollection<C_TipoSociedades> tiposCatalogo)
        {
            while (reader.Read())
            {
                BasicEntityInfo info = new BasicEntityInfo();
                info.Id = Convert.ToInt32(reader["Id"]);
                int idCurrent = info.Id;
                info.Sociedad = reader["DsDenSocial1"].ToString();

                if (!String.IsNullOrEmpty(info.Sociedad) && !info.Sociedad.EndsWith(","))
                {
                    int index = info.Sociedad.LastIndexOf(',');
                    if (index + 1 > info.Sociedad.Length) { }
                    else
                    {
                        if (index > -1)
                        {
                            info.AfterComa = info.Sociedad.Substring(index + 1);
                            info.AfterComaStr = StringUtilities.PrepareToAlphabeticalOrder(info.AfterComa);

                            C_TipoSociedades socCorrecta = tiposCatalogo.FirstOrDefault(x => x.SiglasStr.Equals(info.AfterComaStr));

                            if (socCorrecta != null)
                            {
                                this.UpdateOrden(info, socCorrecta);
                            }
                        }
                    }
                }

            }
        }



        /// <summary>
        /// Actualiza el tipo de sociedad, ya sea que este en blanco o que tenga un tipo incorrecto
        /// </summary>
        /// <param name="info"></param>
        /// <param name="socCorrecta"></param>
        /// <returns></returns>
        public bool UpdateOrden(BasicEntityInfo info, C_TipoSociedades socCorrecta)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool updateCompleted = false;

            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("UPDATE Datos SET DsTipoSociedad1 = @DsTipoSociedad1, Modificado = 1 WHERE Id = @Id", connection);
                cmd.Parameters.AddWithValue("@DsTipoSociedad1", socCorrecta.TipoSociedad);
                cmd.Parameters.AddWithValue("@Id", info.Id);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                updateCompleted = true;
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

            return updateCompleted;
        }

        public void PruebaConexion()
        {
            OracleConnection connection = new OracleConnection(connectionString2);

            try
            {
                

                connection.Open();

                Console.WriteLine("ConexionCorrecta");
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GraficasModel", "ControlDeTiempos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GraficasModel", "ControlDeTiempos");
            }
            finally
            {
                connection.Close();
            }

        }

    }
}
