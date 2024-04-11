﻿using BookingApp.Domain.Model;
using BookingApp.View.Guest.Windows;
using BookingApp.View.Owner;
using BookingApp.ViewModel.Guest;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Guest.Pages
{
    /// <summary>
    /// Interaction logic for GuestReservations.xaml
    /// </summary>
    public partial class GuestReservations : Page
    {
        public User user { get; set; }

        public GuestReservationsViewModel GuestReservationsViewModel { get; set; }

        public ReservedAccommodation reservedAccommodation;
        public GuestReservations(User user)
        {
            InitializeComponent();
            this.user = user;
            GuestReservationsViewModel = new GuestReservationsViewModel(user);
            DataContext = GuestReservationsViewModel;

        }

        private void ReschedulingClick(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as ReservedAccommodation;
            ReschedulingReservation reschedulingReservation = new ReschedulingReservation(user, selectedCard);
            reschedulingReservation.Show();
            reschedulingReservation.Focus();

        }

        private void clickOnCard(object sender, MouseButtonEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as ReservedAccommodation;
            reservedAccommodation = selectedCard;
        }
    }
}
