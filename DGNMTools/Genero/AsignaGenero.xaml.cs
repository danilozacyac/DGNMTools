using DGNMTools.Dto;
using DGNMTools.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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

        List<string> listaEliminar;

        NombreModel model;

        public AsignaGenero()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listaNombreSd = new NombreModel().GetSociosSigerForClasif();
            maxNameCount = listaNombreSd.Count();

            listaEliminar = new List<string>();

            LblNombre.Content = listaNombreSd[currentPosition].NombreDesc;
            model = new NombreModel();
        }

        private void BtnHombre_Click(object sender, RoutedEventArgs e)
        {
            //Ingresa el dato del género a la base de datos
            model.InsertaNombreBase(listaNombreSd[currentPosition], 1);

            currentPosition += 1;
            LblNombre.Content = listaNombreSd[currentPosition].NombreDesc;
            
        }

        private void BtnMujer_Click(object sender, RoutedEventArgs e)
        {
            //Ingresa el dato del género a la base de datos
            model.InsertaNombreBase(listaNombreSd[currentPosition], 2);

            currentPosition += 1;
            LblNombre.Content = listaNombreSd[currentPosition].NombreDesc;
            
        }

        private void BtnSinDefinir_Click(object sender, RoutedEventArgs e)
        {
            //Ingresa el dato del género a la base de datos
            currentPosition += 1;
            LblNombre.Content = listaNombreSd[currentPosition].NombreDesc;
           
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            listaEliminar.Add(String.Format("DELETE FROM SociosSiger WHERE DSNOMBRESOCIO = '{0}'", listaNombreSd[currentPosition ].NombreDesc));

            currentPosition += 1;
            LblNombre.Content = listaNombreSd[currentPosition].NombreDesc;
        }

        private void BtnScript_Click(object sender, RoutedEventArgs e)
        {
            String errorFilePath = ConfigurationManager.AppSettings.Get("SqlDelFilePath");

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(errorFilePath, true))
            {
                foreach (string delete in listaEliminar)
                {
                    file.WriteLine(delete);
                }
            }

            MessageBox.Show("Script finalizado");
        }
    }
}
