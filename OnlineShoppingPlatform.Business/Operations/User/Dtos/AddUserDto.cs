using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Business.Operations.User.Dtos
{
    public class AddUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName  { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    
    }
}
