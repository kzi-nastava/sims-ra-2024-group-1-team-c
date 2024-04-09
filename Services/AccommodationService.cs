﻿using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Repository.TourRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class AccommodationService
    {
        private AccommodationRepository accommodationRepository = AccommodationRepository.GetInstance();
        public AccommodationService() { }
        public AccommodationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<AccommodationService>();
        }
        public void Add(Accommodation newAccommodation)
        {
            accommodationRepository.Add(newAccommodation);
        }

        public List<Accommodation> GetAll()
        {
            return accommodationRepository.GetAll();
        }

        public Accommodation? GetById(int Id)
        {
            return accommodationRepository.GetById(Id);
        }
    }
}
