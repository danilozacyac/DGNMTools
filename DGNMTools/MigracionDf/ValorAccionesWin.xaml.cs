using ScjnUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DGNMTools.MigracionDf
{
    /// <summary>
    /// Interaction logic for ValorAccionesWin.xaml
    /// </summary>
    public partial class ValorAccionesWin : Window
    {
        ObservableCollection<DataFromTexto> listaPendientes;

        DataFromTexto currentData;
        int position = 0;

        DataFromTextoModel model = new DataFromTextoModel();

        public ValorAccionesWin()
        {
            InitializeComponent();
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listaPendientes = model.GetTextoForAcciones(0);
            this.SetDataInForm();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            currentData.ValorAcciones = ((Button)sender).Content.ToString();
            currentData.ValorIntAcciones = 0;
            model.SetCostoAcciones(currentData);
            this.SetDataInForm();
        }

        private void BtnSetValue_Click(object sender, RoutedEventArgs e)
        {
            currentData.ValorAcciones = txtValorTexto.Text;
            currentData.ValorIntAcciones = Convert.ToInt32(TxtNumerico.Text);
            model.SetCostoAcciones(currentData);
            this.SetDataInForm();
        }

        private void SetDataInForm()
        {
            txtValorTexto.Text = String.Empty;
            TxtNumerico.Text = String.Empty;

            currentData = listaPendientes[position];

            Console.WriteLine(currentData.Folio.ToString());

            int startIndex = StringUtilities.PrepareToAlphabeticalOrder( currentData.TextoStr).IndexOf("VALOR DE CADA");

            if (startIndex != -1)
            {

                string tempString = currentData.TextoStr.Substring(startIndex);

                int permisoIndex = tempString.IndexOf("PERMISO");

                tempString = StringUtilities.PrepareToAlphabeticalOrder( tempString.Substring(0, permisoIndex + 8).Replace("$ ", " $"));

                if (tempString.Contains("DIEZ PESOS"))
                {
                    txtValorTexto.Text = "DIEZ PESOS";
                    TxtNumerico.Text = "10";
                }
                else if (tempString.Contains("CIEN PESOS"))
                {
                    txtValorTexto.Text = "CIEN PESOS";
                    TxtNumerico.Text = "100";
                }
                else if (tempString.Contains("QUINIENTOS PESOS"))
                {
                    txtValorTexto.Text = "QUINIENTOS PESOS";
                    TxtNumerico.Text = "500";
                }
                else if (tempString.Contains("UN MIL PESOS"))
                {
                    txtValorTexto.Text = "UN MIL PESOS";
                    TxtNumerico.Text = "1000";
                }
                else if (tempString.Contains("ACCION MIL PESOS"))
                {
                    txtValorTexto.Text = "MIL PESOS";
                    TxtNumerico.Text = "1000";
                }
                else if (tempString.Contains("CINCUENTA PESOS"))
                {
                    txtValorTexto.Text = "CINCUENTA PESOS";
                    TxtNumerico.Text = "50";
                }
                else if (tempString.Contains("CINCUENTA MIL PESOS"))
                {
                    txtValorTexto.Text = "CINCUENTA MIL PESOS";
                    TxtNumerico.Text = "50000";
                }
                else if (tempString.Contains("UN PESO"))
                {
                    txtValorTexto.Text = "UN PESO";
                    TxtNumerico.Text = "1";
                }


                TxtTexto.Text = tempString;

                position++;

            }
            else
            {
                position++;
                SetDataInForm();
            }
                
        }

        private void BtnOmitir_Click(object sender, RoutedEventArgs e)
        {
            SetDataInForm();
        }
        

    }
}
