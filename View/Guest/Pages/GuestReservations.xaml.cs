﻿using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Windows;
using BookingApp.View.Owner;
using BookingApp.ViewModel;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public ReservedAccommodation selectedAccommodation;

        public GuestReservations(User user)
        {
            InitializeComponent();
            this.user = user;
            GuestReservationsViewModel = new GuestReservationsViewModel(this, user);
            DataContext = GuestReservationsViewModel;
            selectedAccommodation = new ReservedAccommodation();

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

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as ReservedAccommodation;
            ReservedAccommodation? reserved = new ReservedAccommodation();
            reserved = ReservedAccommodationService.GetInstance().GetById(selectedCard.Id);
            Accommodation? accommodation = new Accommodation();
            accommodation = AccommodationService.GetInstance().GetById(selectedCard.AccommodationId);
            DateTime checkIn = reserved.CheckInDate;
            if ((checkIn - DateTime.Now).Days > accommodation.CancelationDaysLimit)
            {
                GuestCancelReservation guestCancelReservation = new GuestCancelReservation(user, selectedCard);
                guestCancelReservation.Show();
                guestCancelReservation.Focus();
            }
            else MessageBox.Show("The cancellation deadline has expired!");
        }

        private void RateItClick(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as ReservedAccommodation;
            ReservedAccommodation? reserved = new ReservedAccommodation();
            reserved = ReservedAccommodationService.GetInstance().GetById(selectedCard.Id);
            if ((DateTime.Now - reserved.CheckOutDate).Days <= 5  && reserved.CheckOutDate < DateTime.Now)
            {
                GuestRate guestRate = new GuestRate(user, selectedCard);
                guestRate.Show();
                guestRate.Focus();
            }
            else
            {
                MessageBox.Show("You can not rate the owner!!");
            }
        }

        private void RescheduleClick(object sender, RoutedEventArgs e)
        {
            RescheduleStatus rescheduleStatus = new RescheduleStatus(user);
            rescheduleStatus.Show();
            rescheduleStatus.Focus();
        }
    }
}
