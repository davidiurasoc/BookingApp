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
    [Authorize(Roles = "Administrator")]
    public class RoomController : Controller
    {
        private readonly IRoomTypeRepository _roomRepo;
        private readonly IRoomBookingRepository _roomBookingRepo;
        private readonly IMapper _mapper;

        private readonly UserManager<Client> _userManager;

        public RoomController(IRoomTypeRepository roomRepo, IMapper mapper, UserManager<Client> userManager, IRoomBookingRepository roomBookingRepository)
        {
            _roomRepo = roomRepo;
            _mapper = mapper;
            _userManager = userManager;
            _roomBookingRepo = roomBookingRepository;
        }

        [Authorize]
        // GET: LeaveTypesController
        public ActionResult Index()
        {
            var roomTypes = _roomRepo.FindAll().ToList();
            var model = _mapper.Map<List<Room>, List<RoomVM>>(roomTypes);
            return View(model);
        }


        // GET: LeaveTypesController/Details/5
        public ActionResult Details(int id)
        {
            if (!_roomRepo.isExists(id))
            {
                return NotFound();
            }
            var room = _roomRepo.FindById(id);
            var model = _mapper.Map<RoomVM>(room);
            return View(model);
        }

        public ActionResult History(int id)
        {
            var roomBookings = _roomRepo.GetRoomBookingsPerRoom(id);

            var roomBookingModel = _mapper.Map<List<RoomBookingVM>>(roomBookings);

            //var model = new ClientRoomBookingViewVM
            //{
            //    RoomBookings = roomBookingModel
            //};
            return View(roomBookingModel);
        }

        // GET: LeaveTypesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypesController/Create
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
                var roomType = _mapper.Map<Room>(model);
                roomType.DateCreated = DateTime.Now;
                var isSucces = _roomRepo.Create(roomType);
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

        // GET: LeaveTypesController/Edit/5
        public ActionResult Edit(int id)
        {
            if (!_roomRepo.isExists(id))
            {
                return NotFound();
            }
            var roomType = _roomRepo.FindById(id);
            var model = _mapper.Map<RoomVM>(roomType);

            return View(model);
        }

        // POST: LeaveTypesController/Edit/5
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
                var roomType = _mapper.Map<Room>(model);
                var isSucces = _roomRepo.Update(roomType);
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

        // GET: LeaveTypesController/Delete/5
        public ActionResult Delete(int id)
        {
            var roomType = _roomRepo.FindById(id);
            if (roomType == null)
            {
                return NotFound();
            }
            var isSuccess = _roomRepo.Delete(roomType);
            if (!isSuccess)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: LeaveTypesController/Delete/5
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
