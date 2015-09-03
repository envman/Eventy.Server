using System;

namespace EventServer.Api.Controllers
{
    public class UserHeader
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}