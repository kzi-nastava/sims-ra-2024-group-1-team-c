﻿using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourCouponService
    {
        private TourCouponRepository tourCouponRepository = TourCouponRepository.GetInstance();
        public TourCouponService() { }
        public TourCouponService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourCouponService>();
        }
        public TourCoupon Add(TourCoupon newTourCoupon)
        {
            return tourCouponRepository.Add(newTourCoupon);
        }

        public List<TourCoupon> GetAll()
        {
            return tourCouponRepository.GetAll();
        }

        public TourCoupon? GetById(int Id)
        {
            return tourCouponRepository.GetById(Id);
        }
        public TourCoupon? Update(TourCoupon tourCoupon)
        {
            return tourCouponRepository.Add(tourCoupon);
        }
    }
}
