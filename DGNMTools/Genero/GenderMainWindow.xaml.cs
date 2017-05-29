using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using DGNMTools.Dto;
using DGNMTools.Model;

namespace DGNMTools.Genero
{
    /// <summary>
    /// Interaction logic for GenderMainWindow.xaml
    /// </summary>
    public partial class GenderMainWindow
    {
        public GenderMainWindow()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnConcuerda_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Nombre> listaBasePadron = new NombreModel().GetListaPadron();
            ObservableCollection<Nombre> catalogoSocios = new NombreModel().GetSociosSiger();

            MessageBox.Show(catalogoSocios.Count.ToString());

            List<string> updates = new List<string>();


            int cuantos = 0;
            NombreModel model = new NombreModel();
            foreach (Nombre name in catalogoSocios)
            {
                Nombre count = listaBasePadron.FirstOrDefault(x => x.NombreString.Equals(name.NombreString));

                if (count != null)
                {
                    cuantos++;
                    // model.SetGeneroSocio(name, count.Genero);
                    updates.Add(String.Format("UPDATE SociosSiger SET Genero = {0} WHERE IdSocio = {1};", count.Genero, name.IdNombre));
                }
                else
                {
                    //model.SetGeneroSocio(name, 3);
                    //updates.Add(String.Format("UPDATE SociosSiger SET Genero = 3 WHERE IdSocio = {0}", name.IdNombre));

                    if (name.NombreString.StartsWith("MA "))
                        updates.Add(String.Format("UPDATE SociosSiger SET Genero = 2 WHERE IdSocio = {0};", name.IdNombre));

                    string[] splitNames = name.NombreDesc.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string sName in splitNames)
                    {
                        Nombre thisName = listaBasePadron.FirstOrDefault(x => x.NombreString.Equals(sName));

                        if (thisName != null)
                        {
                            updates.Add(String.Format("UPDATE SociosSiger SET Genero = {0} WHERE IdSocio = {1};", thisName.Genero, name.IdNombre));
                            cuantos++;
                            break;
                        }
                    }

                }
            }

            MessageBox.Show(cuantos.ToString());

            String errorFilePath = ConfigurationManager.AppSettings.Get("SqlUpdatesFilePath");

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(errorFilePath, true))
            {
                foreach (string update in updates)
                {
                    file.WriteLine(update);
                }
            }

            MessageBox.Show("Script finalizado");
        }

        private void BtnAsigna_Click(object sender, RoutedEventArgs e)
        {
            new AsignaGenero().Show();

        }
    }
}
