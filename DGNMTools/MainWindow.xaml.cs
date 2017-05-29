using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DGNMTools.Dto;
using DGNMTools.Model;
using System.Collections.Generic;
using System.Configuration;
using DGNMTools.Genero;

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

            new GenderMainWindow().Show();
            //new SociosPorGenero().Show();

            
        }


        
    }
}
