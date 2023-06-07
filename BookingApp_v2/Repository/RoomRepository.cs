using BookingApp_v2.Contracts;
using BookingApp_v2.Data;
using BookingApp_v2.Models;
using Itenso.TimePeriod;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp_v2.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _db;
        public RoomRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(Room entity)
        {
            _db.Rooms.Add(entity);
            return Save();
        }

        public bool Delete(Room entity)
        {
            _db.Rooms.Remove(entity);
            return Save();
        }

        public ICollection<Room> FindAll()
        {
            var rooms = _db.Rooms.ToList();
            return rooms;
        }

        public Room FindById(int id)
        {
            var room = _db.Rooms.Find(id);
            return room;
        }

        public List<RoomBooking> GetRoomBookingsPerRoomR(int roomId)
        {
            var roomBookings = _db.RoomBookings
                .Where(q => q.RoomId == roomId)
                .ToList();
            return roomBookings;
        }

        public bool IsExists(int id)
        {
            var exists = _db.Rooms.Any(q => q.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(Room entity)
        {
            _db.Rooms.Update(entity);
            return Save();
        }
    }
}
