using DGNMTools.Dto;
using DGNMTools.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace DGNMTools.Genero
{
    /// <summary>
    /// Interaction logic for AsignaGenero.xaml
    /// </summary>
    public partial class AsignaGenero
    {

        int currentPosition = 0;
        int maxNameCount = 0;

        ObservableCollection<Nombre> listaNombreSd;


        public AsignaGenero()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listaNombreSd = new NombreModel().GetSociosSiger();
            maxNameCount = listaNombreSd.Count();

            LblNombre.Content = listaNombreSd[currentPosition];
            currentPosition++;
        }

        private void BtnHombre_Click(object sender, RoutedEventArgs e)
        {
            //Ingresa el dato del género a la base de datos

            LblNombre.Content = listaNombreSd[currentPosition];
            currentPosition++;
        }

        private void BtnMujer_Click(object sender, RoutedEventArgs e)
        {
            //Ingresa el dato del género a la base de datos

            LblNombre.Content = listaNombreSd[currentPosition];
            currentPosition++;
        }

        private void BtnSinDefinir_Click(object sender, RoutedEventArgs e)
        {
            //Ingresa el dato del género a la base de datos

            LblNombre.Content = listaNombreSd[currentPosition];
            currentPosition++;
        }
    }
}
