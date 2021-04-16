using MongoDB.Bson;
using System;

namespace COVIDVaccinationCount.Data.Models
{
    class VaccinationRecord
    {
        public ObjectId Id { get; set; }
        public int FirstDosesAdministered { get; set; }
        public int SecondDosesAdministered { get; set; }
        public int FullyVaccinatedMinors { get; set; }
        public int DosesDistributed { get; set; }
        public int GlobalDosesAdministered { get; set; }
        public DateTime DateTimeAdded { get; set; }
    }
}
