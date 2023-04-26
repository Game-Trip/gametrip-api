using GameTrip.Domain.Entities;
using GameTrip.Domain.Settings;
using Microsoft.AspNetCore.Identity;

namespace GameTrip.EFCore.Data
{
    public class DBInitializer
    {
        private readonly GameTripContext _context;
        private readonly UserManager<GameTripUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public DBInitializer(GameTripContext context, UserManager<GameTripUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> Initialize()
        {
            _context.Database.EnsureCreated();

            if (_context.Roles.Any() || _context.Users.Any()) return false;

            //Adding roles
            string[] roles = Roles.GetAllRoles();

            foreach (string role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    IdentityResult resultAddRole = await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                    if (!resultAddRole.Succeeded)
                        throw new ApplicationException("Adding role '" + role + "' failed with error(s): " + resultAddRole.Errors);
                }
            }

            //Adding Admin
            GameTripUser admin = new()
            {
                UserName = "Dercraker",
                Email = "antoine.capitain@gmail.com",
                EmailConfirmed = true,
            };

            string pwd = "NMdRx$HqyT8jX6";

            IdentityResult? resultAddUser = await _userManager.CreateAsync(admin, pwd);
            if (!resultAddUser.Succeeded)
                throw new ApplicationException("Adding user '" + admin.UserName + "' failed with error(s): " + resultAddUser.Errors);

            IdentityResult resultAddRoleToUser = await _userManager.AddToRoleAsync(admin, Roles.Admin);
            if (!resultAddRoleToUser.Succeeded)
                throw new ApplicationException("Adding user '" + admin.UserName + "' to role '" + Roles.Admin + "' failed with error(s): " + resultAddRoleToUser.Errors);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}