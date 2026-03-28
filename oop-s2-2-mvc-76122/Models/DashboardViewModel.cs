namespace oop_s2_2_mvc_76122.Models;

public class DashboardViewModel
{
    public int InspectionsThisMonth { get; set; }
    public int FailedInspections { get; set; }
    public int OverdueFollowUps { get; set; }

    public string SelectedTown { get; set; }
    public string SelectedRisk { get; set; }
}