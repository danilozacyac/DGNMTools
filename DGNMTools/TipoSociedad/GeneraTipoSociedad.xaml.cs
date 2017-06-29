using DGNMTools.Model;
using System;
using System.Collections.Generic;
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

namespace DGNMTools.TipoSociedad
{
    /// <summary>
    /// Interaction logic for GeneraTipoSociedad.xaml
    /// </summary>
    public partial class GeneraTipoSociedad
    {
        public GeneraTipoSociedad()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSinEspacios_Click(object sender, RoutedEventArgs e)
        {
            new BasicEntityInfoModel().GetEntitiesForReasign(1);
        }

        private void BtnAfterComa_Click(object sender, RoutedEventArgs e)
        {
            new BasicEntityInfoModel().GetEntitiesForReasign(2);
        }
    }
}
