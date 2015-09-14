using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventServer.Api.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public byte[] Data { get; set; }
    }
}
