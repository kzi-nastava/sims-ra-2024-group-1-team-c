﻿using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.ViewModel.Tourist;
using System;
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
using System.Windows.Shapes;
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourDetailed.xaml
    /// </summary>
    public partial class TourDetailed : Window
    {
        public TourDetailedViewModel TourDetailedViewModel { get; set; }
        public TourDetailed(Tour selectedTour,User user)
        {

            InitializeComponent();
            TourDetailedViewModel = new TourDetailedViewModel(this,selectedTour,user);
            this.DataContext = TourDetailedViewModel;
        }
        private void LoadedFunctions(object sender, RoutedEventArgs e)
        {
            CenterWindow();
        }

        private void CenterWindow()
        {
            double SWidth = SystemParameters.PrimaryScreenWidth;
            double SHeight = SystemParameters.PrimaryScreenHeight;
            double WWidth = this.Width;
            double WHeight = this.Height;

            this.Left = (SWidth - WWidth) / 2;
            this.Top = (SHeight - WHeight) / 2;
        }

        public void GoBack(object sender, RoutedEventArgs e)
        {
            TourDetailedViewModel.GoBack(sender, e);
        }
        public void OpenReservationWindow(object sender, RoutedEventArgs e)
        {
            TourDetailedViewModel.OpenReservationWindow(sender, e);
        }
    }
}
