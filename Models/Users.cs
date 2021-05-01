using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HardstyleFamily.Models
{
    public partial class Users : IdentityUser
    {
        public string City { get; set; }
        public string EventsAttending { get; set; }
    }
}
