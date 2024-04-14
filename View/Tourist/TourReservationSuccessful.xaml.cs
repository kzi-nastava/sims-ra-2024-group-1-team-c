﻿using BookingApp.Domain.Model;
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

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourReservationSuccessful.xaml
    /// </summary>
    public partial class TourReservationSuccessful : Window
    {
        TourReservationSuccessfulViewModel TourReservationSuccessfulViewModel { get; set; }
        public TourReservationSuccessful(Tour tour,TourReservation tourReservation)
        {
            InitializeComponent();
            TourReservationSuccessfulViewModel = new TourReservationSuccessfulViewModel(this,tour,tourReservation);
            this.DataContext = TourReservationSuccessfulViewModel;
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

        public void Close(object sender, RoutedEventArgs e)
        {
            TourReservationSuccessfulViewModel.Close(sender, e);
        }
    }
}
