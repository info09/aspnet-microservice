namespace Shared.Configurations
{
    public class HangfireSettings
    {
        public string Route { get; set; } = string.Empty;

        public string ServerName { get; set; } = string.Empty;

        public DatabaseSettings Storage { get; set; } = new DatabaseSettings();

        public Dashboard Dashboard { get; set; } = new Dashboard();
    }

    public class Dashboard
    {
        public string AppPath { get; set; } = string.Empty;
        public int StatsPollingInterval { get; set; }
        public string DashboardTitle { get; set; } = string.Empty;
    }
}
