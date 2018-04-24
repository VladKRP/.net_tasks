using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Domain
{
    public class Room
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; }

        public HospitalRoomType RoomType { get; set; }

        public int Capacity { get; set; }

        public IEnumerable<Person> Persons { get; set; }
    }

    public enum HospitalRoomType
    {
        NotSpecified,
        HospitalStaff,
        Patient
    }
}
