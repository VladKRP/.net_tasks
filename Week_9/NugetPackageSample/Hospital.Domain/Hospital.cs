using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Domain
{
    public class Hospital
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }

        public IEnumerable<Room> Rooms { get; set; }
    }
}
