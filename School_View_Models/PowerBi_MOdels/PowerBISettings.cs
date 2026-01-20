
namespace School_View_Models.PowerBi_MOdels
{
    public class PowerBISettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
        public string WorkspaceId { get; set; }
        public string ReportId { get; set; }
    }

    public class EmbedReport
    {
        public string ReportId { get; set; }
        public string EmbedUrl { get; set; }
        public string EmbedToken { get; set; }
    }
}
