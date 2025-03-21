﻿using BookingApp.Domain.Model;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Tourist
{
    public class TourReservationFailedViewModel
    {
        public TourReservationFailed TourReservationFailed { get; set; }
        public int FreeSlots { get; set; }
        private TourReservationWindow PreviousWindow;
        public Tour SelectedTour { get; set; }
        public RelayCommand ClickExit => new RelayCommand(execute => ExitExecute());
        public RelayCommand ClickGoBack => new RelayCommand(execute => GoBackExecute(),canExecute => GoBackCanExecute());
        public RelayCommand ClickSearchSimilarTours => new RelayCommand(execute => SearchSimilarToursExecute());
        public TourReservationFailedViewModel(TourReservationFailed tourReservationFailed,TourReservationWindow tourReservationWindow,int freeSlots,Tour selectedTour)
        { 
            this.TourReservationFailed = tourReservationFailed;
            this.PreviousWindow = tourReservationWindow;

            FreeSlots = freeSlots;
            SelectedTour = selectedTour;
            TourReservationFailed.FreeSlotsTextBlock.Text = FreeSlots.ToString();


            if (FreeSlots > 0)
            {
                TourReservationFailed.ExceededTheAmoutTextBlock.Text = "It looks like you have exceeded the amount of free slots on this tour!";
                //TourReservationFailed.GoBackButtonGrid.Visibility = Visibility.Visible;
            }
            else
            {
                TourReservationFailed.ExceededTheAmoutTextBlock.Text = "It looks like there aren't any more free slots on this tour!";
                //TourReservationFailed.GoBackButtonGrid.Visibility = Visibility.Collapsed;
            }


        }
        public void GoBackExecute()
        {
            if(FreeSlots > 0)       //Temporary solution
            {
                TourReservationFailed.Close();
            }
        }
        public bool GoBackCanExecute()
        {
            if(FreeSlots > 0) { return true; }
            return false;
        }

        public void ExitExecute()
        {
            
            if(PreviousWindow != null)
            {
                PreviousWindow.Close();
            }

            TourReservationFailed.Close();

        }
        public void SearchSimilarToursExecute()
        {
            if (PreviousWindow != null)
            {
                PreviousWindow.Close();
            }

            TourReservationFailed.Close();
            TourReservationSimilarTours tourReservationSimilarTours = new TourReservationSimilarTours(SelectedTour);
            tourReservationSimilarTours.ShowDialog();
        }
    }
}
