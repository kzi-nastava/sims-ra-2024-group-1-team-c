﻿using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IUserRepository
    {
        void Add(User user);
        User? GetById(int Id);
        int NextId();
        User GetByUsername(string username);
    }
}
