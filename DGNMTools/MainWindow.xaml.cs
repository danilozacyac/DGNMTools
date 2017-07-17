using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DGNMTools.Dto;
using DGNMTools.Model;
using System.Collections.Generic;
using System.Configuration;
using DGNMTools.Genero;
using DGNMTools.TipoSociedad;
using DGNMTools.UtilitiesDgnm;
using Microsoft.Win32;

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
            new VerificaInfoSocios().Show();
        }

        private void BtnTipoSociedad_Click(object sender, RoutedEventArgs e)
        {
            GeneraTipoSociedad tipo = new GeneraTipoSociedad();
            tipo.ShowDialog();
        }

        private void BtnSplitTxt_Click(object sender, RoutedEventArgs e)
        {



            new FileUtilities().SplitBigTextFile(null,null,0);
        }

        private void BtnCountTxtRows_Click(object sender, RoutedEventArgs e)
        {
           // OpenFileDialog

           MessageBox.Show( new FileUtilities().GetTotalLinesOnTxt().ToString());
        }


        
    }
}
