﻿using BookingApp_v2.Contracts;
using BookingApp_v2.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp_v2.Repository
{
    public class RoomBookingRepository : IRoomBookingRepository
    {
        private readonly ApplicationDbContext _db;
        public RoomBookingRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(RoomBooking entity)
        {
            _db.RoomBookings.Add(entity);
            return Save();
        }

        public bool Delete(RoomBooking entity)
        {
            _db.RoomBookings.Remove(entity);
            return Save();
        }

        public ICollection<RoomBooking> FindAll()
        {
            var roomBookings = _db.RoomBookings
                .Include(q => q.BookingClient)
                .Include(q => q.RoomType)
                .ToList();
            return roomBookings;
        }

        public RoomBooking FindById(int id)
        {
            var roomHistories = _db.RoomBookings
                .Include(q => q.BookingClient)
                .Include(q => q.RoomType)
                .FirstOrDefault(q => q.Id == id);
            return roomHistories;
        }

        public ICollection<RoomBooking> GetRoomBookingsPerClient(string employeeId)
        {
            var leaveRequests = FindAll()
                .Where(q => q.BookingClientId == employeeId)
                .ToList();
            return leaveRequests;
        }

        public bool isExists(int id)
        {
            var exists = _db.RoomBookings.Any(q => q.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(RoomBooking entity)
        {
            _db.RoomBookings.Update(entity);
            return Save();
        }
    }
}
