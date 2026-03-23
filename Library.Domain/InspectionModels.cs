using System;
using System.Collections.Generic;

namespace Library.Domain
{
    public class Premises
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Address { get; set; } = "";

        public string Type { get; set; } = "";

        public List<Inspection> Inspections { get; set; } = new();
    }

    public class Inspection
    {
        public int Id { get; set; }

        public int PremisesId { get; set; }

        public Premises? Premises { get; set; }

        public DateTime InspectionDate { get; set; }

        public string InspectionName { get; set; } = "";

        public string Result { get; set; } = "";

        public List<FollowUp> FollowUps { get; set; } = new();
    }

    public class FollowUp
    {
        public int Id { get; set; }

        public int InspectionId { get; set; }

        public Inspection? Inspection { get; set; }

        public string Notes { get; set; } = "";

        public DateTime FollowUpDate { get; set; }
    }
}