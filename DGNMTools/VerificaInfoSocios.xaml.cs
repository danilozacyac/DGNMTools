using DGNMTools.Dto;
using DGNMTools.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DGNMTools
{
    /// <summary>
    /// Interaction logic for VerificaInfoSocios.xaml
    /// </summary>
    public partial class VerificaInfoSocios : Window
    {
        List<ValidaSocios> listaSocios;

        public VerificaInfoSocios()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listaSocios = new ValidaSocioModel().GetSocios();
            this.ReviewInformation();
        }


        private void ReviewInformation()
        {
            int totalDesigual = 0;
            foreach (ValidaSocios socio in listaSocios)
            {
                Regex rfcPattern = new Regex(@"^([A-ZÑ&]{3,4}) ?(?:- ?)?(\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])) ?(?:- ?)?([A-Z\d]{2})([A\d])$");
                
                Match rfcMatch = rfcPattern.Match(socio.Rfc);

                string iniciales = rfcMatch.Groups[1].Value;
                string rfcNacimi = rfcMatch.Groups[2].Value;


                Regex curpPattern = new Regex(@"^([A-Z]{4}) ?(\d{6}) ?([HM]) ?([A-Z]{2}[B-DF-HJ-NP-TV-Z]{3}[A-Z0-9][0-9])$");

                Match curpMatch = curpPattern.Match(socio.Curp);

                string inicialesCurp = curpMatch.Groups[1].Value;
                string curpNacimi = curpMatch.Groups[2].Value;

                if (iniciales.Equals(inicialesCurp) && rfcNacimi.Equals(curpNacimi))
                {
                    if (!String.IsNullOrEmpty(socio.FcNacimiento))
                    {
                        string[] fecha = socio.FcNacimiento.Split('/');

                        string fechaNacRegistrada = String.Format("{0}{1}{2}", fecha[2], fecha[1], fecha[0]);

                        if (fechaNacRegistrada.Equals(rfcNacimi) && fechaNacRegistrada.Equals(curpNacimi))
                        {

                        }
                        else
                        {
                            totalDesigual++;
                        }
                    }
                }
                else
                {
                    totalDesigual++;
                }

                

            }
            MessageBox.Show(totalDesigual.ToString());
        }
    }
}
