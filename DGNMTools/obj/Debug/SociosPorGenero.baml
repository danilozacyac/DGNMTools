   M S B A M L   `   `   `  ����  	DGNMTools    DGNMTools.SociosPorGenero   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      d `DevExpress.Xpf.Charts.v13.1, Version=13.1.10.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a>dxc4http://schemas.devexpress.com/winfx/2008/xaml/charts  5      a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p
 lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
 5        � Title$  Socios Por Genero ��5      $	��640��5      $	��600��5	      .��5
      �  � RowDefinitions 5   
   �� 5      $��60 �6   �� 5      $��60 �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   (  "DevExpress.Xpf.Charts.ChartControl  -   5          Name$ 	devchart1��5      $��2��5      $��Stretch=�5      $��Stretch=�5      $��0q�5         EnableAnimation .    Palette 5      )  #DevExpress.Xpf.Charts.CustomPalette  5         Colors 5      ��5      #0C2016��6)   ��5      #FFE500��6)   ��5      #AEF400��6)   ��5      #3F0DC2��6)   ��5      #FF0000��6)   ��5       #C36AC8��6)   ��5!      #4FEAFF��6)   ��5"      #F1644D��6)   ��5#      #48DD00��6)   ��5$      #000000��6)   5&         Legend 5)      "  DevExpress.Xpf.Charts.Legend  5*         Diagram. 5,      '  !DevExpress.Xpf.Charts.XYDiagram2D     BarDistance$
 0.03Q�6   	   AxisY	 5-      #  DevExpress.Xpf.Charts.AxisY2D  5.         DevExpress.Xpf.Charts.Axis
   Title
 5/      %  DevExpress.Xpf.Charts.AxisTitle  50      %  DevExpress.Xpf.Charts.TitleBase   Content$ Páginas��6,   52      #	  DevExpress.Xpf.Charts.Diagram 	  Series. 54      1
  +DevExpress.Xpf.Charts.BarSideBySideSeries2D
  55         Titles 56      !  DevExpress.Xpf.Charts.Title  57         HorizontalAlignment$ Center=�6      Dock$	 Top=�69   . 6D   '%Promedio páginas trabajadas por hora6j   59      �� 5:   
   $��1��5;      $��	18,32,0,0q�5<      $
��Left=�5=      $	��Top=�5>      $	��De:��5?      �� 5@   
   -   5         ��Name$ 	TxtInicio��5A      $��1��5B      $��92��5C      $��23��5D      $��	50,32,0,0q�5E      $
��Left=�5F      $	��Top=�5G       �� TextWrapping$
 Wrap=�5I      �� 5J   
   $��1��5K      $��
165,32,0,0q�5L      $
��Left=�5M      $	��Top=�5N      $��Hasta:��5O      �� 5P   
   -   5        $ TxtFinal��5Q      $��1��5R      $��92��5S      $��23��5T      $��
212,32,0,0q�5U      $
��Left=�5V      $	��Top=�5W      $
 Wrap=�5Y      �� 5Z   
   $��1��5[      $��18,1,0,0q�5\      $
��Left=�5]      $	��Top=�5^      $��Visualizar por año��5_      $
��Bold"�5`      �� 5a   
   -   5         ��Name$ 	BtnBuscar��5b      $��1��5c      $��75��5d      $��
322,32,0,0q�5e      $
��Left=�5f      $	��Top=�5g      $��Buscar��5i      * 	 $Telerik.Windows.Controls.RadGridView  5j   
   -   5          Name$ GTotales��5k      $��3��5l      $��Stretch=�5m      $��Stretch=�5n      *  $Telerik.Windows.Controls.DataControl   ItemsSource 5o      �� 5p      5q      