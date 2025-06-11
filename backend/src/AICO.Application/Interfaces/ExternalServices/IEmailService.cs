using System.Threading.Tasks;

namespace AICO.Application.Interfaces.ExternalServices
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string email, string userName);
        Task SendCampaignReportAsync(string email, CampaignReportData reportData);
        Task SendAbTestResultsAsync(string email, AbTestResultsData resultsData);
        Task SendPasswordResetEmailAsync(string email, string resetToken);
        Task SendSubscriptionNotificationAsync(string email, SubscriptionNotificationData data);
    }

    public record CampaignReportData(
        string CampaignName,
        string ReportPeriod,
        decimal Revenue,
        int Conversions,
        double ConversionRate,
        string ReportUrl
    );

    public record AbTestResultsData(
        string TestName,
        string WinningVariant,
        double ConfidenceLevel,
        string ResultsSummary,
        string DetailsUrl
    );

    public record SubscriptionNotificationData(
        string PlanName,
        string Status,
        decimal Amount,
        DateTime NextBillingDate
    );
}