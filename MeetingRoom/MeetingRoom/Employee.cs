using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeetingRoom
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public DateTime DateOfBirth { get; set; }

        // Constructor
        public Employee()
        {
            // Initialize properties or add any necessary logic here
        }
    }

}