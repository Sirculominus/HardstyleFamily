using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardstyleFamily.Models
{
    public class Music
    {

        public int Id { get; set; }
        public string SongName { get; set; }
        public string Artist { get; set; }
        public string Search { get; set; }
        public string YtVideoId { get; set; }

        public Music()
        {

        }

    }
}
