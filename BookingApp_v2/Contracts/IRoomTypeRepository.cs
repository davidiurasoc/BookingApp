using BookingApp_v2.Data;
using System.Collections;
using System.Collections.Generic;

namespace BookingApp_v2.Contracts
{
    public interface IRoomTypeRepository : IRepositoryBase<Room>
    {
        ICollection<Room> GetEmployeesByRoomType(int id);
        List<RoomBooking> GetRoomBookingsPerRoom(int roomId);
    }
}
