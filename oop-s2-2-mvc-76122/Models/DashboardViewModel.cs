public class DashboardViewModel
{
    public int InspectionsThisMonth { get; set; }
    public int FailedInspections { get; set; }
    public int OverdueFollowUps { get; set; }

    public string SelectedTown { get; set; }
    public string SelectedRisk { get; set; }
}