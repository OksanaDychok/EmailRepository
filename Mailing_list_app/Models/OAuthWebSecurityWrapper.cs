namespace Mailing_list_app.Models
{
	using System.Collections.Generic;
	using Microsoft.Web.WebPages.OAuth;

	public class OAuthWebSecurityWrapper : IOAuthWebSecurity
	{
		public bool HasLocalAccount(int userId)
		{
			return OAuthWebSecurity.HasLocalAccount(userId);
		}

	}
}