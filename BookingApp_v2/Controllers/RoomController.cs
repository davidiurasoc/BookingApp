using AutoMapper;
using BookingApp_v2.Contracts;
using BookingApp_v2.Data;
using BookingApp_v2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp_v2.Controllers
{
    [Authorize(Roles = "Administrator, SuperAdministrator")]
    public class RoomController : Controller
    {
        private readonly IRoomRepository _roomRepo;
        private readonly IRoomBookingRepository _roomBookingRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Client> _userManager;

        public RoomController(
            IRoomRepository roomRepo,
            IRoomBookingRepository roomBookingRepo,
            IMapper mapper, 
            UserManager<Client> userManager
            )
        {
            _roomRepo = roomRepo;
            _roomBookingRepo = roomBookingRepo;
            _mapper = mapper;
            _userManager = userManager;
            
        }

        //[Authorize]
        // GET: RoomController
        public ActionResult Index()
        {
            var rooms = _roomRepo.FindAll().ToList();
            var model = _mapper.Map<List<Room>, List<RoomVM>>(rooms);
            return View(model);
        }


        // GET: RoomController/Details/5
        public ActionResult Details(int id)
        {
            if (!_roomRepo.IsExists(id))
            {
                return NotFound();
            }
            var room = _roomRepo.FindById(id);
            var model = _mapper.Map<RoomVM>(room);
            return View(model);
        }

        public ActionResult History(int id)
        {
            var roomBookings = _roomRepo.GetRoomBookingsPerRoomR(id);

            var roomBookingModel = _mapper.Map<List<RoomBookingVM>>(roomBookings);

            //var model = new ClientRoomBookingViewVM
            //{
            //    RoomBookings = roomBookingModel
            //};
            return View(roomBookingModel);
        }

        // GET: RoomController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var room = _mapper.Map<Room>(model);
                room.DateCreated = DateTime.Now;
                var isSucces = _roomRepo.Create(room);
                if (!isSucces)
                {
                    ModelState.AddModelError("", "Something Went Wrong...");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        // GET: RoomController/Edit/5
        public ActionResult Edit(int id)
        {
            if (!_roomRepo.IsExists(id))
            {
                return NotFound();
            }
            var room = _roomRepo.FindById(id);
            var model = _mapper.Map<RoomVM>(room);

            return View(model);
        }

        // POST: RoomController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoomVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var room = _mapper.Map<Room>(model);
                var isSucces = _roomRepo.Update(room);
                if (!isSucces)
                {
                    ModelState.AddModelError("", "Something Went Wrong...");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        // GET: RoomController/Delete/5
        public ActionResult Delete(int id)
        {
            var room = _roomRepo.FindById(id);
            if (room == null)
            {
                return NotFound();
            }
            var isSuccess = _roomRepo.Delete(room);
            if (!isSuccess)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: RoomController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, RoomVM model)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
    }
}
