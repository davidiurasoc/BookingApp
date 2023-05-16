using BookingApp_v2.Contracts;
using BookingApp_v2.Data;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp_v2.Repository
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public RoomTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(RoomType entity)
        {
            _db.RoomTypes.Add(entity);
            return Save();
        }

        public bool Delete(RoomType entity)
        {
            _db.RoomTypes.Remove(entity);
            return Save();
        }

        public ICollection<RoomType> FindAll()
        {
            var roomTypes = _db.RoomTypes.ToList();
            return roomTypes;
        }

        public RoomType FindById(int id)
        {
            var roomTypes = _db.RoomTypes.Find(id);
            return roomTypes;
        }

        public ICollection<RoomType> GetEmployeesByRoomType(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool isExists(int id)
        {
            var exists = _db.RoomTypes.Any(q => q.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(RoomType entity)
        {
            _db.RoomTypes.Update(entity);
            return Save();
        }
    }
}
