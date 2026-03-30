namespace oop_s2_2_mvc_76122.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int InspectionsThisMonth { get; set; }
        public int FailedInspectionsThisMonth { get; set; }
        public int OverdueFollowUps { get; set; }
        public List<string> Towns { get; set; } = new List<string>();
        public string? SelectedTown { get; set; }
        public RiskRating? SelectedRiskRating { get; set; }
        public int TotalPremises { get; set; }
        public int TotalInspections { get; set; }
        public int OpenFollowUps { get; set; }
    }
}