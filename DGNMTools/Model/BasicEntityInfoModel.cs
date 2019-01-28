using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using DGNMTools.Dto;
using ScjnUtilities;
using System.Xml;

namespace DGNMTools.Model
{
    public class BasicEntityInfoModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["TipoSociedad"].ConnectionString;
        private readonly string connectionString2 = ConfigurationManager.ConnectionStrings["Espejo"].ConnectionString;

        /// <summary>
        /// Obtiene el número total de sociedades que no tienen asignado el tipo de sociedad
        /// </summary>
        /// <returns></returns>
        public int GetTotalEntities()
        {
            ObservableCollection<C_TipoSociedades> tiposCatalogo = new C_TipoSociedadesModel().GetTipoSociedades();

            const string SqlCadena = "SELECT COUNT(Id) Total FROM CaratulasJulio2018 WHERE (TipoSociedadSugerido is null and (CLAVE_FORMA <> 'M9' AND CLAVE_FORMA <> 'M28') and ACTO_FA = '6.- Sociedad en Nombre Colectivo')";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            int totalSociedades = 0;
            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                reader = cmd.ExecuteReader();
                //int totalReview = 0, totalupdates = 0;

                while (reader.Read())
                {
                    totalSociedades = Convert.ToInt32(reader["Total"]);
                }

                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, String.Format("{0} Exception,BasicEntityModel{1}", methodName, totalSociedades), "DGNMTools");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, String.Format("{0} Exception,BasicEntityModel{1}", methodName, totalSociedades), "DGNMTools");
            }
            finally
            {
                connection.Close();
            }
            return totalSociedades;
        }

        /// <summary>
        /// Obtiene el listado de todas aquellas sociedades que no tienen asignado el tipo de sociedad
        /// </summary>
        /// <returns></returns>
        public void GetEntitiesForReasign(int tipoBusqueda)
        {
            ObservableCollection<C_TipoSociedades> tiposCatalogo = new C_TipoSociedadesModel().GetTipoSociedades();

            //const string SqlCadena = "SELECT Id,dsdensocial FROM CaratulasJulio2018 WHERE (TipoSociedadSugerido is null and (CLAVE_FORMA = 'M9' )) ORDER BY Id desc";
            const string SqlCadena = "SELECT Id,dsdensocial FROM CaratulasJulio2018 WHERE CLAVE_FORMA <> 'M28' and IdTipoSociedadSugerido = 84  and DSDENSOCIAL IS NOT NULL ORDER BY Id desc";

            int totalReview = 0, totalupdates = 0;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            int idCurrent = 0;
            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                reader = cmd.ExecuteReader();
                //int totalReview = 0, totalupdates = 0;

                bool exitoNoSpace = false;

                while (reader.Read())
                {
                    idCurrent = Convert.ToInt32(reader["Id"]);

                    if (tipoBusqueda == 1)
                    {
                        exitoNoSpace = this.NoSpacesProces(Convert.ToInt32(reader["Id"]), reader["Dsdensocial"].ToString(), tiposCatalogo);
                        //if (!exitoNoSpace)
                        //    exitoNoSpace = this.AfterComaProces(Convert.ToInt32(reader["Id"]), reader["Dsdensocial"].ToString(), tiposCatalogo);
                    }
                    else if (tipoBusqueda == 2)
                    {
                        Console.WriteLine(idCurrent);
                        exitoNoSpace = this.GetCoincidencia(idCurrent, StringUtilities.PrepareToAlphabeticalOrder(reader["DSDENSOCIAL"].ToString()), tiposCatalogo[0]);
                    }
                    totalReview++;

                    if (exitoNoSpace)
                        totalupdates++;
                }

                cmd.Dispose();
                reader.Close();

                MessageBox.Show(String.Format("Total revisados: {0}", totalReview));
                MessageBox.Show(String.Format("Total actualizados: {0}", totalupdates));
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, String.Format("{0} Exception,BasicEntityModel{1}", methodName, idCurrent), "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, String.Format("{0} Exception,BasicEntityModel{1}", methodName, idCurrent), "PadronApi");
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
        private bool NoSpacesProces(int id, string denominacion, ObservableCollection<C_TipoSociedades> tiposCatalogo)
        {
            int idCurrent = 0;
            int totalReview = 0, totalupdates = 0;

            bool exito = false;

            try
            {
                BasicEntityInfo info = new BasicEntityInfo();
                info.Id = id;

                idCurrent = info.Id;
                info.Sociedad = denominacion;

                if (!string.IsNullOrEmpty(info.Sociedad))
                {
                    info.SociedadStr = StringUtilities.PrepareToAlphabeticalOrder(this.CleanDenominacion(info.Sociedad)).Replace(" ", "");
                    Console.WriteLine(info.Id);
                    totalReview++;

                    bool find = false;
                    foreach (C_TipoSociedades tipo in tiposCatalogo)
                    {
                        if (info.SociedadStr.EndsWith(tipo.SiglasStr))
                        {
                            this.UpdateOrden(info, tipo);
                            totalupdates++;
                            exito = true;
                        }
                        else if (info.SociedadStr.EndsWith(tipo.SubtipoStrWoSpaces))
                        {
                            this.UpdateOrden(info, tipo);
                            totalupdates++;
                            exito = true;
                        }

                        if (exito)
                            break;
                    }
                }
            }
            catch (SqlException ex)
            {
                return false;
                //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel" + idCurrent, "PadronApi");
            }
            catch (Exception ex)
            {
                return false;
                //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel" + idCurrent, "PadronApi");
            }

            return exito;
        }

        /// <summary>
        /// Verifica si la información que esta después de la última "," coincide con las siglas de los tipos de denominaciones
        /// </summary>
        private bool AfterComaProces(int id, string denominacion, ObservableCollection<C_TipoSociedades> tiposCatalogo)
        {
            BasicEntityInfo info = new BasicEntityInfo();
            info.Id = id;
            int idCurrent = info.Id;
            info.Sociedad = denominacion;

            if (!String.IsNullOrEmpty(info.Sociedad) && !info.Sociedad.EndsWith(","))
            {
                int index = info.Sociedad.LastIndexOf(',');
                if (index + 1 > info.Sociedad.Length)
                {
                }
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
                            return true;
                        }
                    }
                }
            }

            return false;
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

                SqlCommand cmd = new SqlCommand("UPDATE CaratulasJulio2018 SET IdTipoSociedadSugerido = @Tipo, TipoSociedadSugerido = @TipoSociedadSugerido, SubTipoSociedadSugerido = @SubTipoSociedadSugerido WHERE Id = @Id", connection);
                cmd.Parameters.AddWithValue("@Tipo", socCorrecta.Id);
                cmd.Parameters.AddWithValue("@TipoSociedadSugerido", socCorrecta.TipoSociedad);
                cmd.Parameters.AddWithValue("@SubTipoSociedadSugerido", socCorrecta.Subtipo);
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

        public string CleanDenominacion(string cCadena)
        {
            string sCadena = cCadena;

            sCadena = sCadena.Replace("(Sociedad con folio automático)", "");
            sCadena = sCadena.Replace("--FUSIONADA--", "");
            sCadena = sCadena.Replace("(EN LIQUIDACIÓN)", "");
            sCadena = sCadena.Replace("(EN LIQUIDACION)", "");
            sCadena = sCadena.Replace("SOCIEDAD LIQUIDADA", "");
            sCadena = sCadena.Replace("(FUSION, SOC. FUSIONADA)", "");

            sCadena = sCadena.Replace("(PART.451 VOL.XIV LIB.I SECC.COM.)", "");

            sCadena = sCadena.Replace("(SIC)", "");
            sCadena = sCadena.Replace("(OJO CAMBIO DE DOMICILIO)", "");
            sCadena = sCadena.Replace("DISUELTA", "");

            sCadena.ToUpper();
            return sCadena;
        }// fi

        public bool GetCoincidencia(int id, string denominacion, C_TipoSociedades tipoSociedad)
        {
            bool exito = false;

            string[] palabras = denominacion.Split(' ');

            if (palabras.Count() > 7)
            {
                string end = String.Join(" ", palabras, palabras.Count() - 7, 7);

                double porcentaje = 0;
                StringUtilities.LevenshteinDistance(end, tipoSociedad.SubtipoStr, out porcentaje);

                if (porcentaje < 0.1)
                {
                    BasicEntityInfo info = new BasicEntityInfo()
                    {
                        Id = id,
                        Sociedad = denominacion
                    };

                    this.UpdateOrden(info, tipoSociedad);
                    exito = true;
                }
            }

            return exito;
        }

        public ObservableCollection<UpdateSociedad> GetSociedadesForUpdate(int tipoSugerido)
        {
            ObservableCollection<UpdateSociedad> listaUpdate = new ObservableCollection<UpdateSociedad>();

            //const string SqlCadena = "SELECT Id,dsdensocial FROM CaratulasJulio2018 WHERE (TipoSociedadSugerido is null and (CLAVE_FORMA = 'M9' )) ORDER BY Id desc";
            const string SqlCadena = "SELECT * FROM CaratulasJulio2018 WHERE IdTipoSociedadSugerido = @tipo and (folio is not null and dsdensocial is not null and llestado is not null and llmunicipio is not null and lloficina is not null)";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            int idCurrent = 0;
            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                cmd.Parameters.AddWithValue("@tipo", tipoSugerido);

                reader = cmd.ExecuteReader();
                //int totalReview = 0, totalupdates = 0;

                bool exitoNoSpace = false;

                while (reader.Read())
                {
                    UpdateSociedad socUpdate = new UpdateSociedad(Convert.ToInt32(reader["LLCARATULA"]), reader["FOLIO"].ToString(), this.getEscapedCharacters(reader["DSDENSOCIAL"].ToString()), Convert.ToInt32(reader["LLESTADO"]), Convert.ToInt32(reader["LLMUNICIPIO"]), Convert.ToInt32(reader["LLOFICINA"]), Convert.ToInt32(reader["IDTIPOSOCIEDADSUGERIDO"]), reader["SUBTIPOSOCIEDADSUGERIDO"].ToString());

                    listaUpdate.Add(socUpdate);
                }

                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, String.Format("{0} Exception,BasicEntityModel{1}", methodName, idCurrent), "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, String.Format("{0} Exception,BasicEntityModel{1}", methodName, idCurrent), "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return listaUpdate;
        }

        public string getEscapedCharacters(string denominacion)
        {
            if (denominacion.Contains('\''))
                denominacion = denominacion.Replace("\'", "\'\'");
            if (denominacion.Contains('&'))
                denominacion = denominacion.Replace("&", "&\'||'");

            return denominacion;
        }

        public void GetInfoDenominacionForChanges()
        {
            ObservableCollection<BasicEntityInfo> sociedades = new ObservableCollection<BasicEntityInfo>();

            //const string SqlCadena = "SELECT Id,dsdensocial FROM CaratulasJulio2018 WHERE (TipoSociedadSugerido is null and (CLAVE_FORMA = 'M9' )) ORDER BY Id desc";
            const string SqlCadena = "SELECT * FROM InfoMovtos";

            int totalReview = 0, totalupdates = 0;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            int idCurrent = 0;
            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                reader = cmd.ExecuteReader();
                //int totalReview = 0, totalupdates = 0;

                bool exitoNoSpace = false;

                while (reader.Read())
                {
                    //idCurrent = Convert.ToInt32(reader["Id"]);
                    BasicEntityInfo sociedad = new BasicEntityInfo();
                    sociedad.Sociedad = reader["dsdensocial"].ToString();
                    sociedad.DenomDatoCadena = reader["dsdatocadena"].ToString();

                    string tempString = reader["dsxmldocumento"].ToString();

                    sociedad.BoletaInscripcion = this.extractSocNameFromXml(tempString);

                    if (sociedad.Sociedad.Equals(sociedad.DenomDatoCadena) && sociedad.Sociedad.Equals(sociedad.BoletaInscripcion) && sociedad.DenomDatoCadena.Equals(sociedad.BoletaInscripcion))
                        sociedad.IsSameName = true;

                }

                cmd.Dispose();
                reader.Close();

                MessageBox.Show(String.Format("Total revisados: {0}", totalReview));
                MessageBox.Show(String.Format("Total actualizados: {0}", totalupdates));
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, String.Format("{0} Exception,BasicEntityModel{1}", methodName, idCurrent), "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, String.Format("{0} Exception,BasicEntityModel{1}", methodName, idCurrent), "PadronApi");
            }
            finally
            {
                connection.Close();
            }
        }

        private string extractSocNameFromXml(string xml)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            XmlNodeList nodeList = xmldoc.GetElementsByTagName("elemento");

            XmlNode nodeSoc = nodeList[1];

            string denominacion = nodeSoc.Attributes["valor"].Value;

            return denominacion;
        }
    }
}