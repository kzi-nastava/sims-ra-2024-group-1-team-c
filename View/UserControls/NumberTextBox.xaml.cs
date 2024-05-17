﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BookingApp.View.UserControls
{
    /// <summary>
    /// Interaction logic for NumberTextBox.xaml
    /// </summary>
    public partial class NumberTextBox : UserControl, INotifyPropertyChanged
    {
        public NumberTextBox()
        {
            InitializeComponent();
        }
        private void UpClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NumTextBox.Text))
                NumTextBox.Text = "0";
            if (int.TryParse(NumTextBox.Text, out int number))
                NumTextBox.Text = (number + 1).ToString();
        }

        private void DownClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NumTextBox.Text))
                NumTextBox.Text = "0";
            if (int.TryParse(NumTextBox.Text, out int number))
                if(number > 0)
                    NumTextBox.Text = (number - 1).ToString();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
                e.Handled = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }

        public string placeholder { get; set; }

        public string Placeholder
        {
            get { return placeholder; }
            set
            {
                placeholder = value;
                PlaceholderTextBlock.Text = placeholder;
            }
        }
        private void InputTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(NumTextBox.Text))
                PlaceholderTextBlock.Visibility = Visibility.Visible;
            else
                PlaceholderTextBlock.Visibility = Visibility.Hidden;

        }
        private void StartClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void StopClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
