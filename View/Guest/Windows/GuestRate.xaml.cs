﻿using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.View.Guest.Windows
{
    /// <summary>
    /// Interaction logic for GuestRate.xaml
    /// </summary>
    public partial class GuestRate : Window
    {
        public ObservableCollection<Image> Images { get; set; }
        public GuestRate(User user, ReservedAccommodation reservedAccommodation)
        {
            InitializeComponent();
            Images = new ObservableCollection<Image>();
            GuestRateViewModel guestRateViewModel = new GuestRateViewModel(this, user, reservedAccommodation, Images);
            DataContext = guestRateViewModel;
        }
    }
}
