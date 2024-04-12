﻿using BookingApp.Domain.Model;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GuestRatingModel = BookingApp.Domain.Model.GuestRating;
using GuestRatingPage = BookingApp.View.Owner.GuestRating;
using System.Windows.Media;

namespace BookingApp.ViewModel.Owner
{
    public class OwnerMainWindowViewModel
    {
        public User user { get; set; }
        public OwnerMainWindow OwnerMainWindow { get; set; }
        public GuestReviewsViewModel GuestReviewsViewModel { get; set; }
        public ObservableCollection<ReservedAccommodation> ReservedAccommodations { get; set; }
        public GuestRatingPage GuestRatingPage { get; set; }
        public OwnerMainWindowViewModel(OwnerMainWindow OwnerMainWindow, User User)
        {
            this.user = User;
            this.OwnerMainWindow = OwnerMainWindow;
            GuestReviewsViewModel = new GuestReviewsViewModel(User);
            GuestRatingPage = new GuestRatingPage(OwnerMainWindow, user);
            ReservedAccommodations = GuestRatingPage.GuestRatingViewModel.Update();


            if (ReservedAccommodations.Count == 0)
            {
                OwnerMainWindow.NotificationListBox.BorderBrush = Brushes.Gray;
                OwnerMainWindow.NotificationListBox.BorderThickness = new Thickness(1);
            }

            if (GuestReviewsViewModel.MainWindowIsSuperOwner())
            {
                OwnerMainWindow.starImage.Visibility = Visibility.Visible;
            }
            else
            {
                OwnerMainWindow.starImage.Visibility = Visibility.Collapsed;
            }
        }
    }
}
