using Microsoft.Identity.Client;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using School_View_Models.PowerBi_MOdels;


namespace School_Services.PowerBIServices
{
    public class PowerBIService
    {
        private readonly PowerBISettings _settings;

        public PowerBIService(PowerBISettings settings)
        {
            _settings = settings;
        }

        //private async Task<string> GetAccessToken()
        //{
        //    var app = ConfidentialClientApplicationBuilder
        //        .Create(_settings.ClientId)
        //        .WithClientSecret(_settings.ClientSecret)
        //        .WithAuthority($"https://login.microsoftonline.com/{_settings.TenantId}")
        //        .Build();

        //    var result = await app
        //        .AcquireTokenForClient(new[] { "https://analysis.windows.net/powerbi/api/.default" })
        //        .ExecuteAsync();

        //    return result.AccessToken;
        //}

        //public async Task<EmbedReport> GetEmbedInfo()
        //{
        //    var token = await GetAccessToken();

        //    var credentials = new TokenCredentials(token);
        //    var client = new PowerBIClient(new Uri("https://api.powerbi.com/"), credentials);

        //    var reportResponse = await client.Reports.GetReportInGroupAsync(
        //        Guid.Parse(_settings.WorkspaceId),
        //        Guid.Parse(_settings.ReportId)
        //    );

        //    var report = reportResponse.Value; // IMPORTANT

        //    var generateTokenRequest = new GenerateTokenRequestV2(
        //        accessLevel: "view"
        //    );

        //    var embedToken = await client.Reports.GenerateTokenInGroupAsync(
        //        Guid.Parse(_settings.WorkspaceId),
        //        Guid.Parse(_settings.ReportId),
        //        generateTokenRequest
        //    );

        //    return new EmbedReport
        //    {
        //        ReportId = report.Id.ToString(),
        //        EmbedUrl = report.EmbedUrl,
        //        EmbedToken = embedToken.Token
        //    };
        //}
    }
}
