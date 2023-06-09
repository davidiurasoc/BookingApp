﻿using AutoMapper;
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
using AspNetCoreHero.ToastNotification.Abstractions;

namespace BookingApp_v2.Controllers
{
    [Authorize]
    public class RoomBookingController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IRoomBookingRepository _roomBookingRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Client> _userManager;
        private readonly INotyfService _notyf;

        public RoomBookingController(
            IRoomBookingRepository roomBookingRepo,
            IRoomRepository roomRepo,
            IMapper mapper,
            UserManager<Client> userManager,
            ApplicationDbContext db,
            INotyfService notyf
        )
        {
            _roomBookingRepo = roomBookingRepo;
            _roomRepo = roomRepo;
            _mapper = mapper;
            _userManager = userManager;
            _db = db;
            _notyf = notyf;
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        // GET: RoomBookingController
        public ActionResult Index()
        { 
            var roomBookings = _roomBookingRepo.FindAll();
            var roomBookingsModel = _mapper.Map<List<RoomBookingVM>>( roomBookings );
            var model = new RoomBookingVM
            {
                TotalBookings = roomBookingsModel.Count,
                RoomBookings = roomBookingsModel
            };

            _notyf.Information("Here you can find all the bookings", 7);
            _notyf.Warning("Be aware when changing the status!!!", 8);
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

            _notyf.Information("Here you can see all your bookings", 7);
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

            _notyf.Information("Here you can edit user Details", 7);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public async Task<IActionResult> EditClientDetails(ClientVM model)
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

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var newPassword = _userManager.PasswordHasher.HashPassword(client, model.NewPassword);
                    client.PasswordHash = newPassword;
                }

                var result = await _userManager.UpdateAsync(client);

                if (result.Succeeded)
                {
                    _notyf.Success("User Details Succesfully Modified", 7);
                    return RedirectToAction(nameof(ListUsers));
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
            return View(model);
        }

        public ActionResult ListUsers()
        {
            var loggedInUser = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            var superAdminRole = "SuperAdministrator";

            var users = _userManager.Users.ToList();
            var wantedUsers = new List<IdentityUser>();

            foreach (var user in users)
            {
                var roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
                if (user.Id != loggedInUser.Id && !roles.Contains(superAdminRole))
                {
                    wantedUsers.Add(user);
                }
            }

            var model = _mapper.Map<List<ClientVM>>(wantedUsers);
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
                    _notyf.Success("User Succesfully Deleted", 5);
                    return RedirectToAction("ListUsers");
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
            
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

                if (DateTime.Compare(startDate, endDate) > 0)
                {
                    ModelState.AddModelError("", "Start date cannot be further in the future than the End date...");
                    return View(model);
                }

                if (DateTime.Compare(DateTime.Now.Date, startDate.Date) > 0)
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
                    Status = "Pending"
                };

                var roomBooking = _mapper.Map<RoomBooking>(roomBookingModel);
                var isSuccess = _roomBookingRepo.Create(roomBooking);

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something Went Wrong with submitting your record...");
                    return View(model);
                }

                _notyf.Success("You Booking Was Succesfull", 7);
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
            var client = _userManager.GetUserAsync(User).Result;
            var booking = _roomBookingRepo.FindById(id);

            if (booking != null)
            {
                booking.Status = "Cancelled";
                _roomBookingRepo.Update(booking);
            }
            return RedirectToAction("MyBooking");
        }

        public ActionResult ApproveUserBooking(int id, string viewName, string userId)
        {
            var client = _userManager.GetUserAsync(User).Result;
            var booking = _roomBookingRepo.FindById(id);

            if (booking != null)
            {
                booking.Status = "Approved";
                _roomBookingRepo.Update(booking);
            }

            if (viewName == "BookingsPerClient")
            {
                return RedirectToAction("BookingsPerClient", new { id = userId });
            }
            return RedirectToAction("Index");
        }

        public ActionResult CancelUserBooking(int id, string viewName, string userId)
        {
            var client = _userManager.GetUserAsync(User).Result;
            var booking = _roomBookingRepo.FindById(id);

            if (booking != null)
            {
                booking.Status = "Cancelled";
                _roomBookingRepo.Update(booking);
            }

            if (viewName == "BookingsPerClient")
            {
                return RedirectToAction("BookingsPerClient", new { id = userId });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: RoomBookingController/Delete/5
        public ActionResult Delete(string id)
        {
            return View();
        }

        // POST: RoomBookingController/Delete/5
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

        public ActionResult CreateNewUser()
        {
            _notyf.Information("Here you can create a new User", 7);
            return View();
        }

        [HttpPost]
        public ActionResult CreateNewUser(ClientVM model) 
        {
            if (User.IsInRole("Administrator") || User.IsInRole("SuperAdministrator"))
            {
                var newUser = new Client
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    DateJoined = model.DateJoined,
                };

                var result = _userManager.CreateAsync(newUser, model.PasswordHash).Result;
                if (result.Succeeded)
                {
                    var roleResult = _userManager.AddToRoleAsync(newUser, model.Role).Result;
                    if (roleResult.Succeeded)
                    {
                        _notyf.Success("User was created Succesfully", 7);
                        return RedirectToAction("ListUsers");
                    }
                    else
                    {
                        ModelState.AddModelError("", "A apărut o eroare la adăugarea rolului utilizatorului.");
                    }

                    return RedirectToAction("ListUsers");
                }
                else
                {
                    ModelState.AddModelError("", "A apărut o eroare la crearea utilizatorului.");
                }
            }
            else
            {
                return Forbid();
            }

            return View();
        }
    }
}
