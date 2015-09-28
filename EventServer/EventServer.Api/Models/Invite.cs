using System;

namespace EventServer.Api.Models
{
    public class Invite
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public string InvitedById { get; set; }
        public ApplicationUser InvitedBy { get; set; }
        public string CreatedUserId { get; set; }
        public ApplicationUser CreatedUser { get; set; }
        public string Email { get; set; }
    }
}
