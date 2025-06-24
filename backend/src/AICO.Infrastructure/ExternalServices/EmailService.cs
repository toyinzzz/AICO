using AICO.Application.Interfaces.ExternalServices;
using System.Threading.Tasks;

namespace AICO.Infrastructure.ExternalServices
{
    public class EmailService : IEmailService
    {
        public Task SendWelcomeEmailAsync(string email, string userName)
        {
            throw new System.NotImplementedException();
        }

        public Task SendCampaignReportAsync(string email, CampaignReportData reportData)
        {
            throw new System.NotImplementedException();
        }

        public Task SendAbTestResultsAsync(string email, AbTestResultsData resultsData)
        {
            throw new System.NotImplementedException();
        }

        public Task SendPasswordResetEmailAsync(string email, string resetToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SendSubscriptionNotificationAsync(string email, SubscriptionNotificationData data)
        {
            throw new System.NotImplementedException();
        }
    }
}