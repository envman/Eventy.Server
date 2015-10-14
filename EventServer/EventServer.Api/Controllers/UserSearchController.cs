using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EventServer.Api.Models;

namespace EventServer.Api.Controllers
{
    public class UserSearchController : ApiController
    {
        public IEnumerable<UserHeader> Get(string query)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Users
                    .Where(u => u.UserName.ToLower().Contains(query) || u.Email.ToLower().Contains(query))
                    .Select(u => new UserHeader
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                    }).ToList();
            }
        }
    }
}
