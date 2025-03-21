﻿using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using BookingApp.View.Guest.Pages;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class CommentRepository : ICommentRepository
    {

        public static CommentRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<CommentRepository>();
        }
        private const string FilePath = "../../../Resources/Data/comments.csv";

        private readonly Serializer<Comment> _serializer;

        private List<Comment> _comments;

        public CommentRepository()
        {
            _serializer = new Serializer<Comment>();
            _comments = _serializer.FromCSV(FilePath);
        }

        public List<Comment> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Comment Save(Comment comment)
        {
            comment.Id = NextId();
            _comments = _serializer.FromCSV(FilePath);
            _comments.Add(comment);
            _serializer.ToCSV(FilePath, _comments);
            return comment;
        }

        public int NextId()
        {
            _comments = _serializer.FromCSV(FilePath);
            if (_comments.Count < 1)
            {
                return 1;
            }
            return _comments.Max(c => c.Id) + 1;
        }

        public void Delete(Comment comment)
        {
            _comments = _serializer.FromCSV(FilePath);
            Comment founded = _comments.Find(c => c.Id == comment.Id);
            _comments.Remove(founded);
            _serializer.ToCSV(FilePath, _comments);
        }

        public Comment Update(Comment comment)
        {
            _comments = _serializer.FromCSV(FilePath);
            Comment current = _comments.Find(c => c.Id == comment.Id);
            int index = _comments.IndexOf(current);
            _comments.Remove(current);
            _comments.Insert(index, comment);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _comments);
            return comment;
        }

        public List<Comment> GetByUser(User user)
        {
            _comments = _serializer.FromCSV(FilePath);
            return _comments.FindAll(c => c.User.Id == user.Id);
        }

        public Comment? GetById(int id)
        {
            _comments = _serializer.FromCSV(FilePath);
            return _comments.Find(c => c.Id == id);
        }
    }
}
