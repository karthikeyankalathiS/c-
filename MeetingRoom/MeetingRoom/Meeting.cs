using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeetingRoom
{
    public class Meeting
    {
        public int MeetingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Employee Organizer { get; set; }
    }

}