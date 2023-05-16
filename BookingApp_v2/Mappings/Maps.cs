﻿using AutoMapper;
using BookingApp_v2.Data;
using BookingApp_v2.Models;

namespace BookingApp_v2.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<RoomType, RoomTypeVM>().ReverseMap();
            CreateMap<RoomBooking, RoomBookingVM>().ReverseMap();
            CreateMap<Client, ClientVM>().ReverseMap();
        }
    }
}
