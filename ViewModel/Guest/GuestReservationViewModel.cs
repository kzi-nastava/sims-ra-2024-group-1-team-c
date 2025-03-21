﻿using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest;
using BookingApp.Repository.AccommodationRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GuestReservations = BookingApp.View.Guest.Windows.GuestReservation;
using System.Collections.ObjectModel;
using Image = BookingApp.Domain.Model.Image;
using System.ComponentModel;
using System.Windows.Data;
using Notification.Wpf;
using BookingApp.View.Guest.Windows;

namespace BookingApp.ViewModel.Guest
{
    public class GuestReservationViewModel : INotifyPropertyChanged
    {

        private int currentImageIndex = 0;
        public User user { get; set; }
        public INotificationManager notificationManager = App.GetNotificationManager();
        public ObservableCollection<string> ImagePaths { get; set; }
        public GuestReservations GuestReservations { get; set; }
        public ObservableCollection<AvailableDate> printDates { get; set; }

        public ReservedAccommodation reservedAccommodation { get; set; }

        public Accommodation? Accommodation { get; set; }

        public string CurrentImagePath => ImagePaths.ElementAtOrDefault(CurrentImageIndex);
        public int TotalImages => ImagePaths.Count;
        public RelayCommand Exit => new RelayCommand(execute => CloseWindow());
        public RelayCommand Next => new RelayCommand(execute => Next1());
        public RelayCommand Previous => new RelayCommand(execute => Previous1());
        public RelayCommand FocusStartDatePicker => new RelayCommand(execute => FocusStartDate());
        public RelayCommand FocusEndDatePicker => new RelayCommand(execute => FocusEndDate());
        public RelayCommand ReservationSearchButton => new RelayCommand(execute => ReservationSearch(), canExecute => AvailableReservationSearch());
        public RelayCommand ReservationClickButton => new RelayCommand(execute => ReservationClick(), canExecute => AvailableReservationClick());
        public RelayCommand List => new RelayCommand(execute => SelectDate());
        public RelayCommand DaysTextBox => new RelayCommand(execute => AddDays());
        public RelayCommand GuestNumber => new RelayCommand(execute => AddGuest());
        public GuestReservationViewModel(GuestReservations GuestReservations, Accommodation selectedAccommodation, User user) 
        {
            this.GuestReservations = GuestReservations;
            reservedAccommodation = new ReservedAccommodation();
            Accommodation = AccommodationService.GetInstance().GetById(selectedAccommodation.Id);
            printDates = new ObservableCollection<AvailableDate>();
            ImagePaths = new ObservableCollection<string>();
            GuestReservations.GuestNumberTextBox.Text = "Max guest number " + selectedAccommodation.MaxGuestNumber;
            GuestReservations.ReservationDaysTextBox.Text = "Min reservation days " + selectedAccommodation.MinReservationDays;
            this.user = user;
            GuestReservations.AccommodationName.Content += Accommodation.Name;
            GuestReservations.AccommodationLocation.Content += selectedAccommodation.Location.State + ", " + selectedAccommodation.Location.City;
            foreach(Image image in Accommodation.Images)
                ImagePaths.Add(image.Path);
        }

        public void CloseWindow()
        {
            GuestReservations.Close();
        }
        public void AddDays()
        {
            GuestReservations.ReservationDaysTextBox.Focus();
        }
        public void AddGuest()
        {
            GuestReservations.GuestNumberTextBox.Focus();
        }
        public void FocusStartDate()
        {
            GuestReservations.CheckInDatePicker.IsDropDownOpen = true;
        }
        public void FocusEndDate()
        {
            GuestReservations.CheckOutDatePicker.IsDropDownOpen = true;
        }
        public void Next1() 
        {
            if (CurrentImageIndex < TotalImages - 1)
            {
                CurrentImageIndex++;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }
        public void Previous1()
        {
            if (CurrentImageIndex > 0)
            {
                CurrentImageIndex--;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }
        public void SelectDate()
        {
            GuestReservations.AvailableDates.Focus();
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }

        public int CurrentImageIndex
        {
            get { return currentImageIndex; }
            set
            {
                if (value >= 0 && value < ImagePaths.Count)
                {
                    currentImageIndex = value;
                    OnPropertyChanged(nameof(CurrentImageIndex));
                }
            }
        }
        public void NextImage(object sender, RoutedEventArgs e)
        {
            if (CurrentImageIndex < TotalImages - 1)
            {
                CurrentImageIndex++;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }

        public void PreviousImage(object sender, RoutedEventArgs e)
        {
            if (CurrentImageIndex > 0)
            {
                CurrentImageIndex--;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }
        private bool AreDatesAvailable(DateTime startDate, DateTime endDate, int reservationDays)
        {
            if (!CheckDates(startDate, endDate, reservationDays)) return false;
            List<ReservedAccommodation> reservedAccommodations = ReservedAccommodationService.GetInstance().GetAll().Where(t => t.Accommodation.Id == Accommodation.Id).ToList();
            List<ScheduledRenovation> scheduledRenovations = ScheduledRenovationService.GetInstance().GetAll().Where(t => t.AccommodationId == Accommodation.Id).ToList();

            for (DateTime date = startDate; date <= startDate.AddDays(reservationDays); date = date.AddDays(1))
            {
                foreach (ReservedAccommodation reservedAccommodation in reservedAccommodations)
                        if (!CheckReservedDates(date, reservedAccommodation)) return false;

                foreach (ScheduledRenovation scheduledRenovation in scheduledRenovations)
                        if (!CheckRenovationDates(date, scheduledRenovation)) return false;
            }
            return true;
        }

        public bool CheckReservedDates(DateTime date, ReservedAccommodation reservedAccommodation)
        {
            if (date > reservedAccommodation.CheckInDate && date < reservedAccommodation.CheckOutDate) return false;
            return true;
        }

        public bool CheckRenovationDates(DateTime date, ScheduledRenovation scheduledRenovation)
        {
            if (date > scheduledRenovation.StartDate && date < scheduledRenovation.EndDate) return false;
            return true;
        }

        public bool CheckDates(DateTime startDate, DateTime endDate, int reservationDays)
        {
            if (endDate <= startDate) return false;

            if ((endDate - startDate).Days < reservationDays) return false;

            return true;
        }
        private List<DateTime> FindAvailableDates(DateTime startDate, DateTime endDate, int reservationDays)
        {
            List<DateTime> availableDates = new List<DateTime>();

            DateTime currentStartDate = startDate, currentEndDate = endDate;

            while (currentStartDate <= currentEndDate.AddHours(2))
            {
                if (AreDatesAvailable(currentStartDate, currentEndDate.AddHours(2), reservationDays)) availableDates.Add(currentStartDate);

                currentStartDate = currentStartDate.AddDays(1);
            }
            if (availableDates.Count == 0)
            {
                currentStartDate = startDate;
                int counterDates = 0;

                while (true)
                {
                    currentStartDate = currentStartDate.AddDays(1);
                    currentEndDate = currentStartDate.AddDays(reservationDays);

                    if (counterDates == 5)
                    {
                        notificationManager.Show("Info", "In the entered date range, there are no available slots. Here are some recommended dates", NotificationType.Information);
                        break;
                    } 

                    if (AreDatesAvailable(currentStartDate, currentEndDate, reservationDays))
                    {
                        availableDates.Add(currentStartDate);
                        counterDates++;
                    }
                }
            }
            return availableDates;
        }
        public bool IsNumeric(string text)
        {
            try
            {
                int number = int.Parse(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AvailableReservationSearch() 
        {
            if (!IsNumeric(GuestReservations.ReservationDaysTextBox.Text))
            {
                return false;
            }
            if (Convert.ToInt32(GuestReservations.ReservationDaysTextBox.Text) < GuestReservations.accommodation.MinReservationDays)
            {
                return false;
            }
            GuestReservations.CheckInDatePicker.IsDropDownOpen = false;
            GuestReservations.CheckOutDatePicker.IsDropDownOpen = false;
            return true;
        }

        public void ReservationSearch()
        {
            printDates.Clear();
            GuestReservations.AvailableDates.ItemsSource = printDates;
            DateTime startDate = GuestReservations.CheckInDatePicker.SelectedDate ?? DateTime.Now, endDate = GuestReservations.CheckOutDatePicker.SelectedDate ?? DateTime.Now;

            startDate = startDate.AddHours(12);
            endDate = endDate.AddHours(10);
            int reservationDays = Convert.ToInt32(GuestReservations.ReservationDaysTextBox.Text);

            List<DateTime> availableDates = new List<DateTime>();
            availableDates = FindAvailableDates(startDate, endDate, reservationDays);
            if (availableDates.Count != 0)
            {
                GuestReservations.AvailableDates.IsEnabled = true;
                GuestReservations.AvailableDates.ItemsSource = availableDates;
                foreach (DateTime availableDate in availableDates)
                {
                    AvailableDate dates = new AvailableDate();
                    dates.checkInDate = availableDate;
                    dates.checkOutDate = availableDate.AddDays(reservationDays - 1).AddHours(22);
                    printDates.Add(dates);
                }
                GuestReservations.AvailableDates.ItemsSource = printDates;
            }
            else GuestReservations.AvailableDates.ItemsSource = "Nema datuma";

            GuestReservations.ValidateTextBoxGuest.Text = "*Input guest number!";
            GuestReservations.ValidateTextBoxGuest.Visibility = Visibility.Visible;
            GuestReservations.GuestNumberTextBox.IsEnabled = true;
        }


        public bool AvailableReservationClick()
        {
            if (GuestReservations.AvailableDates.SelectedValue == null) return false;
            
            else if (GuestReservations.GuestNumberTextBox.Text.Equals("") || GuestReservations.GuestNumberTextBox.Text.Equals("Max guest number " + Accommodation.MaxGuestNumber)) return false;
            
            else if (!int.TryParse(GuestReservations.GuestNumberTextBox.Text, out int guestNumber) || guestNumber <= 0) return false;
            
            else if (Convert.ToInt32(GuestReservations.GuestNumberTextBox.Text) > Accommodation.MaxGuestNumber) return false;
            
            else return true;
            
        }
        public void ReservationClick() 
        {
            string? selectedDate = GuestReservations.AvailableDates.SelectedValue.ToString();
            string[] dates = selectedDate.Split('-');
            reservedAccommodation.CheckInDate = Convert.ToDateTime(dates[0].Trim());
            reservedAccommodation.CheckOutDate = Convert.ToDateTime(dates[1].Trim());
            reservedAccommodation.Accommodation = Accommodation;
            reservedAccommodation.GuestId = user.Id;
            foreach (Image image in Accommodation.Images) reservedAccommodation.Images.Add(image);
            reservedAccommodation.GuestNumber = Convert.ToInt32(GuestReservations.GuestNumberTextBox.Text);


            foreach (GuestBonus guestBonus in GuestBonusService.GetInstance().GetAll())
            {
                if(guestBonus.GuestId == user.Id && guestBonus.Bonus > 0)
                {
                    guestBonus.Bonus--;
                    GuestBonusService.GetInstance().Update(guestBonus);
                    break;
                }
            }
            ReportOnReservations reportOnReservations = new ReportOnReservations();
            reportOnReservations.GuestId = user.Id;
            reportOnReservations.AccommodationId = reservedAccommodation.Accommodation.Id;
            reportOnReservations.Date = DateTime.Now;
            reportOnReservations.TypeReport = "Reserved";
            reportOnReservations.CheckInDate = reservedAccommodation.CheckInDate;
            reportOnReservations.CheckOutDate = reservedAccommodation.CheckOutDate;
            ReservedAccommodationService.GetInstance().Add(reservedAccommodation);
            reportOnReservations.ReservedId = reservedAccommodation.Id;
            ReportOnReservationsService.GetInstance().Add(reportOnReservations);
            GuestReservations.Close();
            notificationManager.Show("Success", "Accommodation Successfully reserved!", NotificationType.Success);
        }
        public void AvailableDates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GuestReservations.AvailableDates.SelectedItem != null)
            {
                GuestReservations.selectedDates = GuestReservations.AvailableDates.SelectedItem.ToString();
            }
        }
    }
}