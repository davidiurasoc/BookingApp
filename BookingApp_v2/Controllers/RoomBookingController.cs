using AutoMapper;
using BookingApp_v2.Contracts;
using BookingApp_v2.Data;
using BookingApp_v2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itenso.TimePeriod;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookingApp_v2.Controllers
{
    [Authorize]
    public class RoomBookingController : Controller
    {
        private readonly IRoomBookingRepository _roomBookingRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Client> _userManager;

        public RoomBookingController(
            IRoomBookingRepository roomBookingRepo,
            IRoomRepository roomRepo,
            IMapper mapper,
            UserManager<Client> userManager
        )
        {
            _roomBookingRepo = roomBookingRepo;
            _roomRepo = roomRepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize(Roles = "Administrator")]
        // GET: RoomBookingController
        public ActionResult Index()
        { 
            var roomBookings = _roomBookingRepo.FindAll();
            var roomBookingsModel = _mapper.Map<List<RoomBookingVM>>( roomBookings );
            var model = new AdminRoomBookingsViewVM
            {
                TotalBookings = roomBookingsModel.Count,
                RoomBookings = roomBookingsModel
            };
            return View(model);
        }

        public ActionResult MyBooking()
        {
            var client = _userManager.GetUserAsync(User).Result;
            var clientId = client.Id;

            var clientBookings = _roomBookingRepo.GetRoomBookingsPerClient(clientId);

            var clientBookingModel = _mapper.Map<List<RoomBookingVM>>(clientBookings);

            var model = new ClientRoomBookingViewVM
            {
                RoomBookings = clientBookingModel
            };
            return View(model);
        }

        public ActionResult BookingsPerClient(string id)
        {
            var roomBookings = _roomBookingRepo.FindAll()
                .Where(q => q.BookingClientId == id)
                .ToList();

            var clientBookingModel = _mapper.Map<List<RoomBookingVM>>(roomBookings);

            return View(clientBookingModel);
        }

        // GET: RoomBookingController/Details/5
        public ActionResult Details(int id)
        {
            var roomBookings = _roomBookingRepo.FindById(id);
            var model = _mapper.Map<RoomBookingVM>(roomBookings);
            return View(model);
        }

        public ActionResult ClientDetails(string id)
        {
            var client = _userManager.FindByIdAsync(id).Result;
            var model = _mapper.Map<ClientVM>(client);
            return View(model);
        }

        public ActionResult EditClientDetails(string id)
        {
            var client = _userManager.FindByIdAsync(id).Result;
            var model = _mapper.Map<ClientVM>(client);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditClientDetails(ClientVM model)
        {
            if (ModelState.IsValid)
            {
                var client = await _userManager.FindByIdAsync(model.Id);

                if (client != null)
                {
                    client.UserName = model.UserName;
                    client.FirstName = model.FirstName;
                    client.LastName = model.LastName;
                    client.Email = model.Email;
                    client.PhoneNumber = model.PhoneNumber;
                    client.DateOfBirth = model.DateOfBirth;

                    var result = await _userManager.UpdateAsync(client);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(ListClients));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Something went wrong");
                        return View("Error");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Client not found.");
                }
            }
            return View(model);
        }

        public ActionResult ListClients()
        {
            var clients = _userManager.GetUsersInRoleAsync("Client").Result;
            var model = _mapper.Map<List<ClientVM>>(clients);
            return View(model);
        }

        public ActionResult DeleteClient(string id)
        {
            var client = _userManager.FindByIdAsync(id).Result;

            if (client != null)
            {
                var roomBookings = _roomBookingRepo.FindAll()
                    .Where(q => q.BookingClientId == id)
                    .ToList();

                foreach (var booking in roomBookings)
                {
                    _roomBookingRepo.Delete(booking);
                }

                var result = _userManager.DeleteAsync(client).Result;

                if (result.Succeeded)
                {
                    var clients = _userManager.GetUsersInRoleAsync("Client").Result;
                    var model = _mapper.Map<List<ClientVM>>(clients);
                    return View(model);
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return ListClients();
            }
            
        }

        public bool IsIntervalOverlapping(DateTime startDate, DateTime endDate, List<RoomBooking> roomBookings)
        {
            TimeRange wantToBeBooked = new TimeRange(startDate, endDate);

            bool isOverlapping = roomBookings.Any(rb =>
                wantToBeBooked.OverlapsWith(new TimeRange(rb.StartDate, rb.EndDate)));
            return isOverlapping;
        }


        [HttpPost]
        public ActionResult GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            var availableRooms = _roomBookingRepo.GetAvailableRooms(startDate, endDate);

            var roomItems = availableRooms.Select(q => new
            {
                Text = q.RoomName,
                Value = q.Id.ToString()
            });

            return Json(roomItems);
        }


        // GET: RoomBookingController/Create
        public ActionResult Create()
        {
            var rooms = _roomRepo.FindAll();
            var roomItems = rooms.Select(q => new SelectListItem
            {
                Text = q.RoomName,
                Value = q.Id.ToString()
            });
            var model = new RoomBookingVM
            {
                Rooms = roomItems
            };
            return View(model);
        }

        // POST: RoomBookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomBookingVM model)
        {


            try
            {
                var roomBookings = _roomBookingRepo.FindAll().ToList();
                var startDate = Convert.ToDateTime(model.StartDate);
                var endDate = Convert.ToDateTime(model.EndDate);

                if (DateTime.Compare(startDate, endDate) > 1)
                {
                    ModelState.AddModelError("", "Start date cannot be further in the future than the End date...");
                    return View(model);
                }

                if (DateTime.Compare(DateTime.Now, startDate) > 0)
                {
                    ModelState.AddModelError("", "Start date cannot be in the past...");
                    return View(model);
                }

                var availableRooms = _roomBookingRepo.GetAvailableRooms(startDate, endDate);

                var roomItems = availableRooms.Select(q => new SelectListItem
                {
                    Text = q.RoomName,
                    Value = q.Id.ToString()
                });

                model.Rooms = roomItems;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                //    //if (IsIntervalOverlapping(startDate, endDate, roomBookings))
                //    //{
                //    //    ModelState.AddModelError("", "The room is already booked somewhere in this interval!");
                //    //    return View(model);
                //    //}

                var client = _userManager.GetUserAsync(User).Result;
                int daysBooked = (int)(endDate - startDate).TotalDays;

                var startDateS = startDate.ToString();
                var endDateS = endDate.ToString();

                var roomBookingModel = new RoomBookingVM
                {
                    BookingClientId = client.Id,
                    StartDate = startDateS,
                    EndDate = endDateS,
                    DateRequested = DateTime.Now,
                    RoomId = model.RoomId,
                };

                var roomBooking = _mapper.Map<RoomBooking>(roomBookingModel);
                var isSuccess = _roomBookingRepo.Create(roomBooking);

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something Went Wrong with submitting your record...");
                    return View(model);
                }

                return RedirectToAction("MyBooking");
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        // GET: RoomBookingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RoomBookingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CancelBooking(int id)
        {
            var roomBookings = _roomBookingRepo.FindAll().ToList();

            foreach (var booking in roomBookings)
            {
                if (booking.Id == id)
                {
                    _roomBookingRepo.Delete(booking);
                }
            }

            return RedirectToAction(nameof(MyBooking));
        }

        // GET: RoomBookingController/Delete/5
        public ActionResult Delete(string id)
        {
            return View();
        }

        // POST: LeaveRequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
