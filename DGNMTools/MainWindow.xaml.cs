using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using DGNMTools.MigracionDf;
using DGNMTools.Model;
using DGNMTools.Reportes;
using DGNMTools.Socios;
using DGNMTools.UtilitiesDgnm;
using ScjnUtilities;
using DGNMTools.Dto;

namespace DGNMTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // new GenderMainWindow().Show();
            //// new SociosPorGenero().Show();
            // //new AsignaGenero().Show();
            // //this.Close();
            // List < GeneroPorAnio >  fechas = new NombreModel().GetFechasToConvert();
            // int yearInicio = 2012;
            // while (yearInicio <= DateTime.Now.Year)
            // {
            //     int currentStatus = 1;
            //     List<GeneroPorAnio> cuantos = fechas.Where(x => x.FechaInt.ToString().StartsWith(yearInicio.ToString())).ToList();
            //     while (currentStatus < 7)
            //     {
            //         List<GeneroPorAnio> cuantosGenero = cuantos.Where(x => x.Genero == currentStatus).ToList();
            //         Console.WriteLine(String.Format("{0} -- {1} -- {2}", yearInicio, currentStatus, cuantosGenero));
            //         currentStatus++;
            //     }
            //     yearInicio++;
            // }
            //new BasicEntityInfoModel().PruebaConexion();
            //new VerificaInfoSocios().Show();
        }

        private void BtnTipoSociedad_Click(object sender, RoutedEventArgs e)
        {
            new BasicEntityInfoModel().GetEntitiesForReasign(1);
            //GeneraTipoSociedad tipo = new GeneraTipoSociedad();
            //tipo.ShowDialog();
        }

        private void BtnSplitTxt_Click(object sender, RoutedEventArgs e)
        {
            new FileUtilities().SplitBigTextFile(null, null, 0);
        }

        private void BtnCountTxtRows_Click(object sender, RoutedEventArgs e)
        {
            // OpenFileDialog
            MessageBox.Show(new FileUtilities().GetTotalLinesOnTxt().ToString());
        }

        private void BtnGetMunicipio_Click(object sender, RoutedEventArgs e)
        {
            new DataFromTextoModel().GetCampoTexto();
        }

        private void BtnRelex_Click(object sender, RoutedEventArgs e)
        {
            new DataFromTextoModel().GetCampoTextoSinGuinoes();
        }

        private void BtnPorcentajesSocios_Click(object sender, RoutedEventArgs e)
        {
            totalAccionesDenSocialModel model = new totalAccionesDenSocialModel();
            model.GetSociedadesDosSocios();
        }

        private void BtnPorcentajeSociosDfNl_Click(object sender, RoutedEventArgs e)
        {
            int errores = 0;
            totalAccionesDenSocialModel model = new totalAccionesDenSocialModel();

            List<int> socs2Socios = model.GetSociedadesDosSociosNLCD();
            List<InfoNlyCd> informacion = model.GetSociosNlyDf();

            int contador = 1;

            foreach (int mainId in socs2Socios)
            {
                TotalAccionesDensocial sociedad = new TotalAccionesDensocial();

                List<Socio> socios = new List<Socio>();
                foreach (InfoNlyCd info in (from n in informacion
                                            where n.MainId == mainId
                                            select n))
                {
                    sociedad.Folio = info.Folio;
                    sociedad.DenSocial = String.Format("{0}{1}{2}", info.Oficina, info.Folio, info.IdGen);

                    Socio socio = new Socio();

                    sociedad.TotalAcciones += info.NumAcciones;
                    socio.Accion = info.NumAcciones;

                    sociedad.TotalValor += info.Total;
                    socio.Total = info.Total;

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

                if (!model.SetSocForAnalisisNLCD(sociedad))
                    errores++;

                Console.WriteLine(contador.ToString());
                contador++;
            }

            Console.WriteLine("Errores: " + errores);
            Console.ReadLine();
        }

        private Int64 ObtenerPorcentaje(Int64 sumaTotal, Int64 cuantoTiene)
        {
            return (cuantoTiene * 100) / sumaTotal;
        }

        private void BtnGetCostAcciones_Click(object sender, RoutedEventArgs e)
        {
            new DataFromTextoModel().GetTextoForAcciones(1);
        }

        private void BtnGetValorAcciones_Click(object sender, RoutedEventArgs e)
        {
            //new DataFromTextoModel().GetTextoForAcciones(2);
            new ValorAccionesWin().ShowDialog();
        }

        private void BtnDescargaContrato_Click(object sender, RoutedEventArgs e)
        {
            foreach (string url in GetUrlContratoSocial())
            {
                string name = Path.GetFileName(url);

                WebClient client = new WebClient();
                string reply = client.DownloadString(url);
                System.IO.File.WriteAllText(String.Format(@"C:\Users\Luis\Documents\Contratos\{0}", name), reply);
            }
        }

        public List<string> GetUrlContratoSocial()
        {
            List<string> urls = new List<string>();

            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["BaseDf"].ConnectionString);
            OleDbCommand cmd = null;
            OleDbDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new OleDbCommand("SELECT * FROM ContratosSocialesRelex", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        urls.Add(reader["UrlcontratoSocial"].ToString());
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (OleDbException ex)
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
            return urls;
        }

        private void BtnTiempoPromedio_Click(object sender, RoutedEventArgs e)
        {
            InscripcionModel model = new InscripcionModel();
            model.GetFechas();
        }

        private void BtnAvg2017_Click(object sender, RoutedEventArgs e)
        {
            new InscripcionModel().GetPromedios(15, 100);
        }

        private void BtnMua2017_Click(object sender, RoutedEventArgs e)
        {
            new InscripcionModel().GetFechasMUA(0, 10);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new InscripcionModel().GetFechasMua();
        }

        private void BtnSimilares_Click(object sender, RoutedEventArgs e)
        {
            new SociedadesModel().GetSociedadesTocompare();
        }

        private void BtnSimilaresSat_Click(object sender, RoutedEventArgs e)
        {
            new SociedadesModel().GetSatInfo();
        }

        private void BtnProximidad_Click(object sender, RoutedEventArgs e)
        {
            new BasicEntityInfoModel().GetEntitiesForReasign(2);
        }

        private void BtnSocUpdates_Click(object sender, RoutedEventArgs e)
        {
            List<int> tiposUpdates = new List<int>()
            {
                10,
                11,
                12,
                32,
                45,
                50,
                56,
                71,
                72,
                73,
                82,
                84,
                85,
                86,
                95,
                97,
                98
            };

            foreach (int tipo in tiposUpdates)
            {
                ObservableCollection<UpdateSociedad> listaUpdates = new BasicEntityInfoModel().GetSociedadesForUpdate(tipo);

                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(String.Format(@"C:\Users\Luis\Documents\UpdateSociedades\{0}.txt", tipo)))
                {
                    foreach (UpdateSociedad update in listaUpdates)
                    {
                        file.WriteLine(update.UpdateString);
                    }
                }
                
            }
        }

        private void BtnInconsistencias_Click(object sender, RoutedEventArgs e)
        {
            new BasicEntityInfoModel().GetInfoDenominacionForChanges();
        }
    }
}