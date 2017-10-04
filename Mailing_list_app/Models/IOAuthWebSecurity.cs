namespace Mailing_list_app.Models
{
	using System.Collections.Generic;
	using Microsoft.Web.WebPages.OAuth;

	public interface IOAuthWebSecurity
	{
		bool HasLocalAccount(int userId);
	}
}