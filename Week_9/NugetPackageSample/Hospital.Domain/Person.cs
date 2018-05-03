using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Domain
{
    public class Person
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Sex Sex { get; set; }

        public byte Age { get; set; }
    }

    public enum Sex
    {
        NotSpecified,
        Male,
        Female
    }
}
