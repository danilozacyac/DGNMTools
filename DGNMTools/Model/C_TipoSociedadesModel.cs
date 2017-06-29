using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using DGNMTools.Dto;
using ScjnUtilities;
using System.Configuration;
using System.Collections.Generic;

namespace DGNMTools.Model
{
    public class C_TipoSociedadesModel
    {

        private string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        public ObservableCollection<C_TipoSociedades> GetTipoSociedades()
        {
            ObservableCollection<C_TipoSociedades> obrasSinPadron = new ObservableCollection<C_TipoSociedades>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM MyCatalog ORDER BY Id", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        C_TipoSociedades element = new C_TipoSociedades();
                        //element.Id = Convert.ToInt32(reader["Id"]);
                        element.Siglas = reader["Siglas"].ToString();
                        element.SiglasStr = StringUtilities.PrepareToAlphabeticalOrder(element.Siglas).Replace(" ", "");
                        element.TipoSociedad = reader["TipoSociedad"].ToString();
                        element.TipoSociedadStr = StringUtilities.PrepareToAlphabeticalOrder(element.TipoSociedad);
                        element.Subtipo = reader["SubtipoSociedad"].ToString();
                        element.SubtipoStr = StringUtilities.PrepareToAlphabeticalOrder(element.Subtipo).Replace(" ", "");

                        obrasSinPadron.Add(element);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return obrasSinPadron.ToList().Distinct().ToObservableCollection();
        }

        /// <summary>
        /// Obtiene el listado de siglas del catálogo de tipo de sociedades todo en letras mayúsculas
        /// </summary>
        /// <returns></returns>
        public List<string> GetFlatCatalogo()
        {
            List<string> obrasSinPadron = new List<string>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT Siglas FROM MyCatalog ORDER BY Id", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string tipoSoc = StringUtilities.PrepareToAlphabeticalOrder(reader["Siglas"].ToString()).Replace(" ", "");

                        obrasSinPadron.Add(tipoSoc);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return obrasSinPadron.Distinct().ToList();
        }

    }
}
