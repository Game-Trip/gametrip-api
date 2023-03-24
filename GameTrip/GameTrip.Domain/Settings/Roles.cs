using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTrip.Domain.Settings
{
    public class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static string[] GetAllRoles() => new string[] { Admin, User };
    }
}
