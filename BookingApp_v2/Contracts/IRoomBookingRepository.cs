using BookingApp_v2.Data;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BookingApp_v2.Contracts
{
    public interface IRoomBookingRepository : IRepositoryBase<RoomBooking>
    {
        ICollection<RoomBooking> GetRoomBookingsPerClient(string employeeId);
        List<RoomBooking> GetRoomBookingsPerRoom(int roomId);
    }
}
