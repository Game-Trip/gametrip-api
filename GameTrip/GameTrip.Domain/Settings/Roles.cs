namespace GameTrip.Domain.Settings
{
    public class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static string[] GetAllRoles()
        {
            return new string[] { Admin, User };
        }
    }
}