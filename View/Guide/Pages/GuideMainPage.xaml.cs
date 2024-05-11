﻿using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using BookingApp.Services;
using BookingApp.ViewModel.Guide;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Guide.Pages
{
    /// <summary>
    /// Interaction logic for GuideMainPage.xaml
    /// </summary>
    public partial class GuideMainPage : Page
    {
        public EventHandler? ListUpdater {  get; set; }
        public GuideMainPageViewModel GuideMainPageViewModel { get;set; }
        public User User { get; set; }
        public string UserName { get; set; }
        public GuideMainPage(User user)
        {
            InitializeComponent();
            User = user;
            UserName = User.Username;
            GuideMainPageViewModel = new GuideMainPageViewModel(user);
            this.DataContext = GuideMainPageViewModel;
        }
        private void ClickCreateTour(object sender, RoutedEventArgs e)
        {
            CreateTourForm createTourForm = new CreateTourForm(User);
            createTourForm.RequestRefreshEvent += RequestRefreshEventAction;
            NavigationService.Navigate(createTourForm);
        }

        private void RequestRefreshEventAction()
        {
            GuideMainPageViewModel.Load();
        }

        private void ClickLogout(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            OnLogoutHandler?.Invoke(sender, e);
            signInForm.Show();
        }
        public EventHandler OnLogoutHandler { get; set; }

        private void ClickTourStatistics(object sender, RoutedEventArgs e)
        {
            TourStatistics tourStatistics = new TourStatistics(User);
            NavigationService.Navigate(tourStatistics);
        }

        private void ClickTourReviews(object sender, RoutedEventArgs e)
        {
            FinishedToursPage tourReviewsPage = new FinishedToursPage(this,User);
            NavigationService.Navigate(tourReviewsPage);
        }

        private void ClickTourSuggestions(object sender, RoutedEventArgs e)
        {
            TourRequestsPage tourRequestsPage = new TourRequestsPage();
            NavigationService.Navigate(tourRequestsPage);
        }
    }
}