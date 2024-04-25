using System.Threading.Tasks;

namespace Route.C41.G03.PL.Services
{
	public interface IEmailSender
	{
		Task SendAsync(string form, string recipients, string subject, string body);
	}
}
