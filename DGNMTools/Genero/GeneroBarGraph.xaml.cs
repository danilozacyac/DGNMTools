using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DGNMTools.Model;

namespace DGNMTools.Genero
{
    /// <summary>
    /// Interaction logic for GeneroBarGraph.xaml
    /// </summary>
    public partial class GeneroBarGraph : UserControl
    {
        public GeneroBarGraph()
        {
            InitializeComponent();
        }

        private void TxtIntro_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = ScjnUtilities.VerificationUtilities.IsTextAllowed(e.Text);
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            int inicio = Convert.ToInt32(TxtInicio.Text);
            int final = Convert.ToInt32(TxtFinal.Text);

            if (inicio < 1900)
            {
                MessageBox.Show("El año de inicio no puede ser inferior a 1900", "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if (inicio > DateTime.Now.Year)
            {
                MessageBox.Show("El año de inicio no puede ser superior al año actual", "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (final < inicio)
            {
                MessageBox.Show("El año final no puede ser superior al año de inicio", "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if (inicio > DateTime.Now.Year)
            {
                MessageBox.Show("El año final no puede ser superior al año actual", "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            if (devchart1.Diagram.Series != null)
                while (devchart1.Diagram.Series.Count() > 0)
                    devchart1.Diagram.Series.RemoveAt(0);

            while (inicio <= final)
            {
                devchart1.Diagram.Series.Add(new StatsModel().GetGenero(inicio));
                inicio++;
            }

        }
    }
}
