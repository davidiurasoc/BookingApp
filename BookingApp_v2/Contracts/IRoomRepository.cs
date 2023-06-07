using BookingApp_v2.Data;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BookingApp_v2.Contracts
{
    public interface IRoomRepository : IRepositoryBase<Room>
    {
        List<RoomBooking> GetRoomBookingsPerRoomR(int roomId);
    }
}
