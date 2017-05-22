using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using DevExpress.Xpf.Charts;
using ScjnUtilities;
using System.Configuration;

namespace DGNMTools.Model
{
    public class StatsModel
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        public BarSideBySideSeries2D GetGenero(int year)
        {
            BarSideBySideSeries2D serie = new BarSideBySideSeries2D() { DisplayName = year.ToString() };
            //serie.Name = operativo.NombreCompleto;

            foreach (KeyValuePair<string, int> punto in GetGeneroPerYear(year))
                serie.Points.Add(new SeriesPoint(punto.Key, punto.Value));

            return serie;
        }

        private List<KeyValuePair<string, int>> GetGeneroPerYear(int year)
        {
            List<KeyValuePair<string, int>> paginasMes = new List<KeyValuePair<string, int>>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd;
            SqlDataReader reader;

            try
            {
                string sqlQuery = "SELECT CASE genero " +
                                    " WHEN 1 THEN 'Hombres' " +
                                    " WHEN 2 THEN 'Mujeres' " +
                                    " WHEN Null THEN 'No Definido' END AS Genero, " +
                                    " count(genero) TotGenero from sociossiger where genero is not null and YEAR(FCINSCRIPCION) = @Year group by genero";

                connection.Open();

                cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Year", year);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    paginasMes.Add(new KeyValuePair<string, int>(
                        reader["Genero"].ToString(),
                        Convert.ToInt32(reader["TotGenero"])));
                }

                reader.Close();
                cmd.Dispose();
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

            return paginasMes;
        }
    }
}
