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

namespace BookingApp_v2.Controllers
{
    [Authorize]
    public class RoomBookingController : Controller
    {
        private readonly IRoomBookingRepository _roomBookingRepo;
        private readonly IRoomTypeRepository _roomRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Client> _userManager;

        public RoomBookingController(
            IRoomBookingRepository roomBookingRepo,
            IRoomTypeRepository roomRepo,
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
        // GET: LeaveRequestController
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
            //var client = _userManager.GetUserAsync(User).Result;
            //var id = client.Id;

            var roomBookings = _roomBookingRepo.FindAll()
                .Where(q => q.BookingClientId == id)
                .ToList();

            var clientBookingModel = _mapper.Map<List<RoomBookingVM>>(roomBookings);

            return View(clientBookingModel);
        }

        // GET: LeaveRequestController/Details/5
        public ActionResult Details(int id)
        {
            var roomBookings = _roomBookingRepo.FindById(id);
            var model = _mapper.Map<RoomBookingVM>(roomBookings);
            return View(model);
        }

        public ActionResult ListClients()
        {
            var clients = _userManager.GetUsersInRoleAsync("Client").Result;
            var model = _mapper.Map<List<ClientVM>>(clients);
            return View(model);
        }

        public static bool IsIntervalOverlapping(DateTime startDate, DateTime endDate, DateTime dateToCompareStartDate, DateTime dateToCompareEndDate)
        {
            return startDate <= dateToCompareEndDate && endDate >= dateToCompareStartDate;
        }


        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListClients");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListClients");
            }
        }

        // GET: LeaveRequestController/Create
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

        // POST: LeaveRequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomBookingVM model)
        {
            
            try
            {
                var startDate = Convert.ToDateTime(model.StartDate);
                var endDate = Convert.ToDateTime(model.EndDate);
                var rooms = _roomRepo.FindAll();
                var roomItems = rooms.Select(q => new SelectListItem
                {
                    Text = q.RoomName,
                    Value = q.Id.ToString()
                });
                model.Rooms = roomItems;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (DateTime.Compare(startDate, endDate) > 1)
                {
                    ModelState.AddModelError("", "Start date cannot be further int the future than the End date...");
                    return View(model);
                }

                //var roomBookings = _roomBookingRepo.GetRoomBookingsPerRoom(model.RoomId);
                //var counter = 0;

                //while (counter < roomBookings.Count)
                //{
                //    var bookings = roomBookings.Select(q => new RoomBooking
                //    {
                //        Id = q.Id,
                //        StartDate = q.StartDate,
                //        EndDate = q.EndDate,
                //    });

                //    if (_roomBookingRepo.ViewIfIsBookedDateIsOverlapping(startDate, endDate, model.StartDate, model.EndDate) == false)
                //    {

                //    }

                //}
                
                //if (IsIntervalOverlapping(startDate, endDate,) == false)
                //{
                //    ModelState.AddModelError("", "Please choose another date interval because there is booking overlapping");
                //}

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

        // GET: LeaveRequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveRequestController/Edit/5
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

        // GET: LeaveRequestController/Delete/5
        public ActionResult Delete(int id)
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
