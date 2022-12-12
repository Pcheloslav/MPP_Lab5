using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class User
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string[] Orders { get; }
        public int[] Ints { get; }

        public User(string firstName, string lastName, string[] orders)
        {
            FirstName = firstName;
            LastName = lastName;
            Orders = orders;
        }
    }
}
