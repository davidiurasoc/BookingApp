using BookingApp_v2.Data;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BookingApp_v2.Contracts
{
    public interface IRoomRepository : IRepositoryBase<Room>
    {
        ICollection<Room> GetEmployeesByRoomType(int id);
        List<RoomBooking> GetRoomBookingsPerRoomR(int roomId);
        //public bool IsIntervalOverlapping(DateTime startDate, DateTime endDate, List<RoomBooking> roomBookings);

    }
}
