namespace Mailing_list_app.Models
{
	using System.Security.Principal;

	public interface IWebSecurity
	{
		bool Login(string userName, string password, bool persistCookie = false);
		void Logout();
		string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false);
		int GetUserId(string userName);

		IPrincipal CurrentUser { get; }
	}
}