using DGNMTools.Dto;
using ScjnUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGNMTools.Model
{
    public class ValidaSocioModel
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        /// <summary>
        /// Obtiene el listado de socios que tienen CURP y RFC dentro de la base de datos
        /// </summary>
        /// <returns></returns>
        public List<ValidaSocios> GetSocios()
        {
            List<ValidaSocios> listaSocios = new List<ValidaSocios>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd;
            SqlDataReader reader;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * from Socios where dsRFC is not null", connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ValidaSocios socio = new ValidaSocios();
                    socio.IdSocio = Convert.ToInt32(reader["Id"]);
                    socio.Nombre = reader["dsNombreSocio"].ToString();
                    socio.Paterno = reader["DsApellidoPaterno"].ToString();
                    socio.Materno = reader["DsApellidoMaterno"].ToString();
                    socio.Rfc = reader["DsRfc"].ToString();
                    socio.Curp = reader["DsCurp"].ToString();
                    socio.FcNacimiento = reader["FcNacimiento"].ToString();

                    listaSocios.Add(socio);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
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

            return listaSocios;
        }


    }
}
