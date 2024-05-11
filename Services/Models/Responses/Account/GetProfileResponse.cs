using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class ProfileResponse
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; }
    }

    public class SearchUsersResponse
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public object Cover { get; set; }
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public object Address { get; set; }
        public bool Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
    }
}
