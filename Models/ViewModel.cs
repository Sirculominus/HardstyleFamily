using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardstyleFamily.Models
{
    public class ViewModel
    {
        // From Events
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string Country { get; set; }
        public string Date { get; set; }
        public string Search { get; set; }
        public string Address { get; set; }
        public string Airport { get; set; }
        public int AttendingTotal { get; set; }
        // From Users
        public string UserId { get; set; }
        public string EventsAttending { get; set; }
    }
}
