using System.Collections.Generic;

namespace Hospital.Domain
{
    public class DiseaseHistory
    {
        public int Id { get; set; }

        public IEnumerable<DiseaseTreatment> Diseases { get; set; }
    }
}