﻿#pragma checksum "..\..\SociosPorGenero.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "260E6FFA2DDCE3C4F29A83154F904EC1E10CB6E0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DevExpress.Xpf.Charts;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Animation;
using Telerik.Windows.Controls.Carousel;
using Telerik.Windows.Controls.Docking;
using Telerik.Windows.Controls.DragDrop;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.Legend;
using Telerik.Windows.Controls.Primitives;
using Telerik.Windows.Controls.TransitionEffects;
using Telerik.Windows.Controls.TreeListView;
using Telerik.Windows.Controls.TreeView;
using Telerik.Windows.Data;
using Telerik.Windows.DragDrop;
using Telerik.Windows.DragDrop.Behaviors;
using Telerik.Windows.Input.Touch;
using Telerik.Windows.Shapes;


namespace DGNMTools {
    
    
    /// <summary>
    /// SociosPorGenero
    /// </summary>
    public partial class SociosPorGenero : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\SociosPorGenero.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Charts.ChartControl devchart1;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\SociosPorGenero.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxtInicio;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\SociosPorGenero.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxtFinal;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\SociosPorGenero.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnBuscar;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\SociosPorGenero.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadGridView GTotales;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DGNMTools;component/sociosporgenero.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SociosPorGenero.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.devchart1 = ((DevExpress.Xpf.Charts.ChartControl)(target));
            return;
            case 2:
            this.TxtInicio = ((System.Windows.Controls.TextBox)(target));
            
            #line 72 "..\..\SociosPorGenero.xaml"
            this.TxtInicio.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.TxtIntro_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 3:
            this.TxtFinal = ((System.Windows.Controls.TextBox)(target));
            
            #line 88 "..\..\SociosPorGenero.xaml"
            this.TxtFinal.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.TxtIntro_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 4:
            this.BtnBuscar = ((System.Windows.Controls.Button)(target));
            
            #line 104 "..\..\SociosPorGenero.xaml"
            this.BtnBuscar.Click += new System.Windows.RoutedEventHandler(this.BtnBuscar_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.GTotales = ((Telerik.Windows.Controls.RadGridView)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

