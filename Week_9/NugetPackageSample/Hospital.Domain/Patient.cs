using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Domain
{
    public class Patient: Person
    {
        public DiseaseHistory DiseaseHistory { get; set; }
    }
}
