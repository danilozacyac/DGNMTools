   M S B A M L   `   `   `  ����  	DGNMTools%    DGNMTools.Genero.GeneroBarGraph   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      d `DevExpress.Xpf.Charts.v13.1, Version=13.1.10.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a>dxc4http://schemas.devexpress.com/winfx/2008/xaml/charts  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5      a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p
 lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
 5      
�������.��5      �   � RowDefinitions  5   
   �� 5      $��60 �6   �� 5      $��60 �6   �� 5      $��* �6   .+�5   
   (  "DevExpress.Xpf.Charts.ChartControl  -   5          Name$ 	devchart1��5      $��2��5      $��Stretch=�5      $��Stretch=�5      $��0q�5         EnableAnimation .    Palette 5      )  #DevExpress.Xpf.Charts.CustomPalette  5         Colors 5      ��5      #0C2016��6)   ��5      #FFE500��6)   ��5      #AEF400��6)   ��5       #3F0DC2��6)   ��5!      #FF0000��6)   ��5"      #C36AC8��6)   ��5#      #4FEAFF��6)   ��5$      #F1644D��6)   ��5%      #48DD00��6)   ��5&      #000000��6)   5(         Legend 5+      "  DevExpress.Xpf.Charts.Legend  5,         Diagram. 5.      '  !DevExpress.Xpf.Charts.XYDiagram2D     BarDistance$
 0.03Q�6      AxisY 5/      #  DevExpress.Xpf.Charts.AxisY2D  50         DevExpress.Xpf.Charts.Axis	   Title	 51      %  DevExpress.Xpf.Charts.AxisTitle  52      %  DevExpress.Xpf.Charts.TitleBase
   Content$
 Páginas��6,   54      #	  DevExpress.Xpf.Charts.Diagram 	  Series. 56      1
  +DevExpress.Xpf.Charts.BarSideBySideSeries2D
  57         Titles 58      !  DevExpress.Xpf.Charts.Title  59         HorizontalAlignment$ Center=�6      Dock$	 Top=�69   .
 6D   '%Promedio páginas trabajadas por hora6j   5;      �� 5<   
   $��1��5=      $��	18,32,0,0q�5>      $
��Left=�5?      $	��Top=�5@      $	��De:��5A      �� 5B   
   -   5         ��Name$ 	TxtInicio��5C      $��1��5D      $��92��5E      $��23��5F      $��	50,32,0,0q�5G      $
��Left=�5H      $	��Top=�5I       �� TextWrapping$
 Wrap=�5K      �� 5L   
   $��1��5M      $��
165,32,0,0q�5N      $
��Left=�5O      $	��Top=�5P      $��Hasta:��5Q      �� 5R   
   -   5        $ TxtFinal��5S      $��1��5T      $��92��5U      $��23��5V      $��
212,32,0,0q�5W      $
��Left=�5X      $	��Top=�5Y      $
 Wrap=�5[      �� 5\   
   $��1��5]      $��18,1,0,0q�5^      $
��Left=�5_      $	��Top=�5`      $��Visualizar por año��5a      $
��Bold"�5b      �� 5c   
   -   5         ��Name$ 	BtnBuscar��5d      $��1��5e      $��75��5f      $��
322,32,0,0q�5g      $
��Left=�5h      $	��Top=�5i      $��Buscar��5k      5l      5m      