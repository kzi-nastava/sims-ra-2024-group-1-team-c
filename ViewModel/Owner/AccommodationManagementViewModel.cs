﻿using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Repository;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using BookingApp.View.Owner;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using Notification.Wpf;

namespace BookingApp.ViewModel.Owner
{
    public class AccommodationManagementViewModel : INotifyPropertyChanged
    {
        public INotificationManager notificationManager = App.GetNotificationManager();
        private int currentImageIndex = 0;
        public ObservableCollection<string> ImagePaths { get; set; }
        public string CurrentImagePath => ImagePaths.ElementAtOrDefault(CurrentImageIndex);
        public int TotalImages => ImagePaths.Count;
        public RelayCommand Accept => new RelayCommand(execute => AcceptExecute(), canExecute => AcceptCanExecute());
        public RelayCommand AddImage => new RelayCommand(execute => AddImageExecute()); 
        public RelayCommand DeleteImageCommand => new RelayCommand(execute => DeleteImage(), canExecute => CanDeleteImage());
        public RelayCommand PreviousImageCommand => new RelayCommand(execute => PreviousImage(), canExecute => CanPreviousImage());
        public RelayCommand NextImageCommand => new RelayCommand(execute => NextImage(), canExecute => CanNextImage());
        public AccommodationRegistration AccommodationRegistration {  get; set; }
        public User user { get; set; }
        public List<string> States { get; set; }
        public string SelectedState {  get; set; }
        public ObservableCollection<Location> Locations { get; set; }
        public Location SelectedLocation { get; set; }
        public List<Image> Images { get; set; }
        public ObservableCollection<string> RelativeImagePaths { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public AccommodationManagementViewModel(AccommodationRegistration AccommodationRegistration, User user)
        {
            this.user = user;
            this.AccommodationRegistration = AccommodationRegistration;
            RelativeImagePaths = new ObservableCollection<string>();
            ImagePaths = new ObservableCollection<string>();
            Locations = new ObservableCollection<Location>();
            Images = new List<Image>();
            States = LocationService.GetInstance().GetStates();
        }
        public void StatePicked()
        {
            LocationService.GetInstance().GetCitiesForState(Locations, SelectedState);
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
        public void NextImage()
        {
            if (CurrentImageIndex < TotalImages - 1)
            {
                CurrentImageIndex++;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }
        public bool CanNextImage()
        {
            if (CurrentImageIndex == TotalImages - 1 || TotalImages == 0)
                return false;
            return true;
        }
        public void PreviousImage()
        {
            if (CurrentImageIndex > 0)
            {
                CurrentImageIndex--;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }
        public bool CanPreviousImage()
        {
            if (CurrentImageIndex == 0 || TotalImages == 0)
                return false;
            return true;
        }
        public void DeleteImage()
        {
            try
            {
                ImagePaths.RemoveAt(CurrentImageIndex);
                if (CurrentImageIndex == TotalImages && TotalImages >= 1)
                    CurrentImageIndex--;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                CurrentImageIndex = 0;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
                Console.WriteLine("Error: Indeks not valid - " + ex.Message);
            }
        }
        public bool CanDeleteImage()
        {
            if(TotalImages == 0)
                return false;
            return true;
        }
        public AccommodationType ReturnAccommodationType()
        {
            if (AccommodationRegistration.AccommodationTypeComboBox.SelectionBoxItem.Equals("Apartment"))
                return AccommodationType.Apartment;
            else if (AccommodationRegistration.AccommodationTypeComboBox.SelectionBoxItem.Equals("House"))
                return AccommodationType.House;
            else
                return AccommodationType.Hut;
        }
        public void AcceptExecute()
        {
            Accommodation Accommodation = new Accommodation();
            Accommodation.Name = AccommodationRegistration.NameTextBox.Text;
            Accommodation.AccommodationType = ReturnAccommodationType();
            Accommodation.MaxGuestNumber = Convert.ToInt32(AccommodationRegistration.MaxGuestNumberTextBox.NumTextBox.Text);
            Accommodation.MinReservationDays = Convert.ToInt32(AccommodationRegistration.MinResDaysTextBox.NumTextBox.Text);
            Accommodation.CancelationDaysLimit = Convert.ToInt32(AccommodationRegistration.CancelationDaysLimitTextBox.NumTextBox.Text);
            Accommodation.Images = Images;

            //string[] temporaryStrings = SelectedLocation.Split(':');
            //Location? Location = LocationService.GetInstance().GetById(Convert.ToInt32(temporaryStrings[0]));
            Accommodation.Location = SelectedLocation;
            Accommodation.OwnerId = user.Id;
            AccommodationService.GetInstance().Add(Accommodation);

            ResetInputs();
            AccommodationRegistration.SuccessLabel.Visibility = Visibility.Visible;
            notificationManager.Show("Title", "Message", NotificationType.Success);
            //var content = new NotificationContent();
            //notificationManager.Show(content);
            //notificationManager.Show("Title", "Message");
        }
        public bool AcceptCanExecute()
        {
            //all the fields must be entered
            if (string.IsNullOrEmpty(AccommodationRegistration.NameTextBox.Text) || AccommodationRegistration.StateComboBox.SelectedItem == null ||
                AccommodationRegistration.CityComboBox.SelectedItem == null ||
                AccommodationRegistration.AccommodationTypeComboBox.SelectedItem == null || !IsNumeric(AccommodationRegistration.MaxGuestNumberTextBox.NumTextBox.Text) ||
                !IsNumeric(AccommodationRegistration.MinResDaysTextBox.NumTextBox.Text) || !IsNumeric(AccommodationRegistration.CancelationDaysLimitTextBox.NumTextBox.Text) ||
                AccommodationRegistration.MinResDaysTextBox.NumTextBox.Text.Equals("0") || AccommodationRegistration.MaxGuestNumberTextBox.NumTextBox.Text.Equals("0") ||
                AccommodationRegistration.CancelationDaysLimitTextBox.NumTextBox.Text.Equals("0") || RelativeImagePaths.Count == 0)
            {
                return false;
            }
            return true;
        }

        public void AddImageExecute()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true)
            {
                string binPath = AppDomain.CurrentDomain.BaseDirectory;
                string targetFolderPath = GetBaseFolder(binPath) + "\\Resources\\Images\\Accommodation";

                if (!Directory.Exists(targetFolderPath))
                    Directory.CreateDirectory(targetFolderPath);
                foreach (string filePath in dlg.FileNames)
                {
                    string fileName = System.IO.Path.GetFileName(filePath);
                    string destFilePath = System.IO.Path.Combine(targetFolderPath, fileName);
                    fileName = SaveImageFile(filePath, destFilePath, fileName);

                    string relativePath = System.IO.Path.Combine("../../../Resources/Images/Accommodation/", fileName);
                    RelativeImagePaths.Add(relativePath);
                    ImagePaths.Add(destFilePath);
                }
                if (RelativeImagePaths.Count > 0)
                    SaveImageIntoCSV(RelativeImagePaths);
                if (ImagePaths.Count > 0)
                {
                    CurrentImageIndex = 0;
                    OnPropertyChanged(nameof(CurrentImagePath));
                    OnPropertyChanged(nameof(CurrentImageIndex));
                }
            }
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
        public void ResetInputs()
        {
            AccommodationRegistration.NameTextBox.Text = string.Empty;
            AccommodationRegistration.MaxGuestNumberTextBox.NumTextBox.Text = string.Empty;
            AccommodationRegistration.MinResDaysTextBox.NumTextBox.Text = string.Empty;
            AccommodationRegistration.CancelationDaysLimitTextBox.NumTextBox.Text = string.Empty;
            AccommodationRegistration.StateComboBox.SelectedIndex = -1;
            AccommodationRegistration.CityComboBox.SelectedIndex = -1;
            AccommodationRegistration.AccommodationTypeComboBox.SelectedIndex = -1;
            Locations.Clear();
            RelativeImagePaths.Clear();
            Images.Clear();
            ImagePaths.Clear();
            currentImageIndex = 0;
            OnPropertyChanged(nameof(CurrentImagePath));
            OnPropertyChanged(nameof(CurrentImageIndex));
        }
        private void SaveImageIntoCSV(ObservableCollection<string> relativeImagePaths)
        {
            foreach (string filePath in relativeImagePaths)
            {
                Image? image = new Image();
                image.Path = filePath;
                image = ImageService.GetInstance().Add(image);
                Images.Add(image);
            }
        }
        private string SaveImageFile(string filePath, string destFilePath, string fileName)
        {
            if (!File.Exists(destFilePath))
            {
                File.Copy(filePath, destFilePath, false);
                return fileName;
            }
            else
            {
                string[] fileNameParts = fileName.Split(".");
                while (File.Exists(destFilePath))
                {
                    string[] name = destFilePath.Split(".");
                    name[0] = name[0] + "A";
                    destFilePath = name[0] + "." + name[1];
                    fileNameParts[0] = fileNameParts[0] + "A";
                }
                File.Copy(filePath, destFilePath, false);
                return fileNameParts[0] + "." + fileNameParts[1];
            }
        }
        private string GetBaseFolder(string path)
        {
            for (int i = 0; i < 4; i++)
            {
                DirectoryInfo? parentDirectory = Directory.GetParent(path);
                if (parentDirectory != null)
                {
                    path = parentDirectory.FullName;
                }
                else
                {
                    throw new InvalidOperationException("Cannot navigate up three directories from base.");
                }
            }
            return path;
        }
    }
}
