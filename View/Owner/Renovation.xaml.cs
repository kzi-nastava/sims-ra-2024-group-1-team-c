﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for Renovation.xaml
    /// </summary>
    public partial class Renovation : Page
    {
        public OwnerMainWindow OwnerMainWindow {  get; set; }
        public Renovation(OwnerMainWindow OwnerMainWindow)
        {
            this.OwnerMainWindow = OwnerMainWindow;
            InitializeComponent();
        }

        private void HistoryClick(object sender, RoutedEventArgs e)
        {
            OwnerMainWindow.mainFrame.Navigate(OwnerMainWindow.RenovationHistory);
        }
    }
}
