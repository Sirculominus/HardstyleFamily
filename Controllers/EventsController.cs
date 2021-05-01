using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HardstyleFamily.Data;
using HardstyleFamily.Models;

using System.Text.Json;
using Newtonsoft.Json;
using System.Security.Claims;

namespace HardstyleFamily.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public ViewResult Index()
        {
            var eventsDetails = _context.Events.ToList();
            List<ViewModel> em = new List<ViewModel>();

            // Check if logged in, if not redirects back to start
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                // Gets the Id to current logged in user
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                em = _context.Users.Where(x => x.Id == userId)
                    .Select(x => new ViewModel
                    {
                        UserId = x.Id,
                        EventsAttending = x.EventsAttending
                    }).ToList();
                /*
                userDetails.ForEachAsync(m =>
                {
                    em.Add(new ViewModel
                    {
                        UserId = m.Id,
                        EventsAttending = m.EventsAttending
                    });
                });
                */
            }
            eventsDetails.ForEach(m =>
            {
                em.Add(new ViewModel
                {
                    EventId = m.Id,
                    EventName = m.EventName,
                    Country = m.Country,
                    Date = m.Date,
                    Search = m.Search,
                    Address = m.Address,
                    Airport = m.Airport,
                    AttendingTotal = m.AttendingTotal
                });
            });
            return View(em);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventName,Country,Date,Search,Address,Airport")] Events events)
        {
            if (ModelState.IsValid)
            {
                _context.Add(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(events);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Events.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventName,Country,Date,Search,Address,Airport")] Events events)
        {
            if (id != events.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(events);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventsExists(events.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(events);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var events = await _context.Events.FindAsync(id);
            _context.Events.Remove(events);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventsExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        // Button
        public async Task<IActionResult> check(int button)
        {
            TempData["buttonval"] = button;
            var EventId = button;
            
            // retrieve entity
            var entity = _context.Events.FirstOrDefault(item => item.Id == button);

            if (entity != null)
            {
                entity.AttendingTotal++;
            }

            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }


        // Button

        public async Task<IActionResult> check1(int eventGoing, int eventNotGoing, int eventReset)
        {
            
            // If button "Going" is pushed
            if (eventGoing != 0)
            {
                // Set eventId = the Id of the event
                var eventId = eventGoing;

                // Retrieve DB entity for Events table (pretty much the same as "select * from x.dbo.Events where Id=eventId")
                var entity = _context.Events.FirstOrDefault(item => item.Id == eventId);

                // Set boolean varible to false, this will we change to true if we find the event in the entry (If the user already have answered "Going" or "Not Going")
                bool eventIdExists = false;

                // Check if logged in, if not redirects back to start
                bool isAuthenticated = User.Identity.IsAuthenticated;
                if (!isAuthenticated)
                {
                    return RedirectToAction("Index");
                }

                // Gets the Id to current logged in user
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Retrieve DB entity for Users table
                var eventsAttendingEntity = _context.Users.FirstOrDefault(item => item.Id == userId);

                // Create a Dictinary for our key, value pair information (eventId, Deserialize json
                Dictionary<int, bool> eventsAttendingEntityDeserialized = new Dictionary<int, bool>();

                // Add directly if new user with no eventattending values
                if (eventsAttendingEntity.EventsAttending == null)
                {
                    // Add 1 to total value
                    entity.AttendingTotal++;

                    // Add new key value pair with eventId and true
                    eventsAttendingEntityDeserialized.Add(eventId, true);
                }

                // Do this if the user have something in Eventsattending
                if (eventsAttendingEntity.EventsAttending != null)
                {
                    eventsAttendingEntityDeserialized = JsonConvert.DeserializeObject<Dictionary<int, bool>>(eventsAttendingEntity.EventsAttending);

                    // Check every value in dictionary 
                    foreach (KeyValuePair<int, bool> kvp in eventsAttendingEntityDeserialized)
                    {
                        // Check if the entry key is equal to eventId
                        if (kvp.Key == eventId)
                        {
                            // Check if the value is false (= Not attending)
                            if (kvp.Value == false)
                            {
                                // Add 1 to total value
                                entity.AttendingTotal++;

                                // Add true to the eventId value on user
                                eventsAttendingEntityDeserialized[kvp.Key] = true;
                                eventIdExists = true;
                                break;
                            }
                            // If not above is true, then the user must have the eventId set to true already, which breaks the loop 
                            eventIdExists = true;
                            Console.WriteLine("You are already attending the event!");
                            break;
                        }

                    }

                    // If not eventId is in dictionary, add it as a new key value pair
                    if (!eventIdExists)
                    {
                        // Add 1 to total value
                        entity.AttendingTotal++;

                        // Add new key value pair with eventId and true
                        eventsAttendingEntityDeserialized.Add(eventId, true);
                    }
                }

                // serialize dictionary for storage in DB
                eventsAttendingEntity.EventsAttending = JsonConvert.SerializeObject(eventsAttendingEntityDeserialized);
                
                // Update DB context
                _context.Users.Update(eventsAttendingEntity);


            }

            if (eventNotGoing != 0)
            {
                // Set eventId = the Id of the event
                var eventId = eventNotGoing;

                // Retrieve DB entity for Events table (pretty much the same as "select * from x.dbo.Events where Id=eventId")
                var entity = _context.Events.FirstOrDefault(item => item.Id == eventId);

                // Set boolean varible to false, this will we change to true if we find the event in the entry (If the user already have answered "Going" or "Not Going")
                bool eventIdExists = false;

                // Check if logged in, if not redirects back to start
                bool isAuthenticated = User.Identity.IsAuthenticated;
                if (!isAuthenticated)
                {
                    return RedirectToAction("Index");
                }

                // todo: bug when registering user, then immediately tries 
                // Gets the Id to current logged in user
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Retrieve DB entity for Users table
                var eventsAttendingEntity = _context.Users.FirstOrDefault(item => item.Id == userId);

                // Create a Dictinary for our key, value pair information (eventId, Deserialize json
                Dictionary<int, bool> eventsAttendingEntityDeserialized = new Dictionary<int, bool>();

                // Add directly if new user with no eventattending values
                if (eventsAttendingEntity.EventsAttending == null)
                {
                    // No need to change totalvalue

                    // Add new key value pair with eventId and true
                    eventsAttendingEntityDeserialized.Add(eventId, false);
                }

                if (eventsAttendingEntity.EventsAttending != null)
                {
                    eventsAttendingEntityDeserialized = JsonConvert.DeserializeObject<Dictionary<int, bool>>(eventsAttendingEntity.EventsAttending);

                    // Check every value in dictionary 
                    foreach (KeyValuePair<int, bool> kvp in eventsAttendingEntityDeserialized)
                    {
                        // Check if the entry key is equal to eventId
                        if (kvp.Key == eventId)
                        {
                            // Check if the value is true (= attending)
                            if (kvp.Value == true)
                            {
                                // Remove 1 from total value
                                entity.AttendingTotal--;

                                // Add false to the eventId value on user
                                eventsAttendingEntityDeserialized[kvp.Key] = false;
                                eventIdExists = true;
                                break;
                            }
                            // If not above is true, then the user must have the eventId set to true already, which breaks the loop 
                            eventIdExists = true;
                            Console.WriteLine("You are already not attending the event!");
                            break;
                        }

                    }

                    // If not eventId is in dictionary, add it as a new key value pair
                    if (!eventIdExists)
                    {
                        // Add new key value pair with eventId and true
                        eventsAttendingEntityDeserialized.Add(eventId, false);
                    }
                }


                

                // serialize dictionary for storage in DB
                eventsAttendingEntity.EventsAttending = JsonConvert.SerializeObject(eventsAttendingEntityDeserialized);

                // Update DB context
                _context.Users.Update(eventsAttendingEntity);

            }

            if (eventReset != 0)
            {
                var EventId = eventReset;

                // retrieve entity
                var entity = _context.Events.FirstOrDefault(item => item.Id == EventId);

                // Gets the Id to current logged in user
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (entity != null && userId == "8fee8746-be2e-4741-aed4-4b7900c1756f")
                {
                    entity.AttendingTotal=0;
                }

                
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        
    }
}
