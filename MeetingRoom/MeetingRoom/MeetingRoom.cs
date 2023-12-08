using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeetingRoom
{
    public class MeetingRoom
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public bool IsAvailable { get; set; }
        public List<Meeting> Meetings { get; set; } // List to store scheduled meetings


        public MeetingRoom()
        {
            Meetings = new List<Meeting>();
            IsAvailable = true; 
        }

        public bool BookMeeting(DateTime date, Employee employee)
        {
            if (!IsAvailable)
            {
                Console.WriteLine("Room is already booked for the specified date and time.");
                return false;
            }

            var meeting = new Meeting
            {
                StartTime = date,
                Organizer = employee,
            };

            Meetings.Add(meeting);

            IsAvailable = false;


            // Optionally, save the meeting and room data to the database
            // You will need to implement this based on your database logic

            Console.WriteLine("Meeting booked successfully.");
            return true;
        }

        public bool CancelMeeting(DateTime date, Employee employee)
        {
            var meetingToRemove = Meetings.FirstOrDefault(m => m.StartTime == date);

            if (meetingToRemove == null)
            {
                Console.WriteLine("No meeting found for the specified date and time.");
                return false;
            }

            if (meetingToRemove.Organizer.EmployeeId != employee.EmployeeId)
            {
                Console.WriteLine("You do not have permission to cancel this meeting.");
                return false;
            }

            Meetings.Remove(meetingToRemove);

            IsAvailable = Meetings.Count == 0;

            // Optionally, delete the meeting record from the database
            // You will need to implement this based on your database logic

            Console.WriteLine("Meeting canceled successfully.");
            return true;
        }

        public List<Meeting> GetMeetings(DateTime date)
        {
            var meetingsOnDate = Meetings.Where(m => m.StartTime.Date == date.Date).ToList();

            return meetingsOnDate;
        }
    }

}