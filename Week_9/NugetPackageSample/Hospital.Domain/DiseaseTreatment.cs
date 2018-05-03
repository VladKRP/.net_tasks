using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hospital.Domain
{
    public class DiseaseTreatment
    {
        public int Id { get; set; }

        public string Diagnosis { get; set; }

        public HospitalEmployee AssignedMedicalOfficer { get; set; }

        public IEnumerable<string> PatientComplaints { get; set; }

        public bool IsTreated { get; set; }

        public DateTime DiseaseRevealingDate { get; set; }

        public DateTime? RecoveryDate { get; set; }
    }
}