﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using BookingApp.Serializer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using BookingApp.Services;

namespace BookingApp.Domain.Model
{
    public class RenovationRequest : INotifyPropertyChanged, ISerializable
    {
        private int id { get; set; }
        private int accommodationId { get; set; }
        private int guestId { get; set; }
        private int commentId { get; set; }
        private int level { get; set; }
        private DateTime requestDate { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged(nameof(id));
                }
            }
        }
        public DateTime RequestDate
        {
            get
            {
                return requestDate;
            }
            set
            {
                if (value != requestDate)
                {
                    requestDate = value;
                    OnPropertyChanged(nameof(requestDate));
                }
            }
        }
        public int AccommodationId
        {
            get
            {
                return accommodationId;
            }
            set
            {
                if (value != accommodationId)
                {
                    accommodationId = value;
                    OnPropertyChanged(nameof(accommodationId));
                }
            }
        }
        public int GuestId
        {
            get
            {
                return guestId;
            }
            set
            {
                if (value != guestId)
                {
                    guestId = value;
                    OnPropertyChanged(nameof(guestId));
                }
            }
        }
        public int CommentId
        {
            get
            {
                return commentId;
            }
            set
            {
                if (value != commentId)
                {
                    commentId = value;
                    OnPropertyChanged(nameof(commentId));
                }
            }
        }
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                if (value != level)
                {
                    level = value;
                    OnPropertyChanged(nameof(level));
                }
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),
                AccommodationId.ToString(),
                GuestId.ToString(),
                Level.ToString(),
                CommentId.ToString(),
                RequestDate.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            Level = Convert.ToInt32(values[3]);
            CommentId = Convert.ToInt32(values[4]);
            RequestDate = Convert.ToDateTime(values[5]);
        }
        public string PrintRequests
        {
            get
            {
                string str = "Accommodation Name: " + AccommodationService.GetInstance().GetById(AccommodationId).Name + "\n";
                str += "Guest Username: " + UserService.GetInstance().GetById(GuestId).Username + "\n";
                str += "Level of Renovation Request: " + Level.ToString() + "\n";
                str += "Comment: " + CommentService.GetInstance().GetById(CommentId).Text + "\n";
                return str;
            }
            set
            {
                if (value != PrintRequests)
                {
                    PrintRequests = value;
                    OnPropertyChanged("PrintNotifications");
                }
            }
        }
    }
}
