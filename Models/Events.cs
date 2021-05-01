using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardstyleFamily.Models
{
    public class Events
    {

        public int Id { get; set; }
        public string EventName { get; set; }
        public string Country { get; set; }
        public string Date { get; set; }
        public string Search { get; set; }
        public string Address { get; set; }
        public string Airport { get; set; }
        public int AttendingTotal { get; set; }


        public Events()
        {

        }

    }
}
