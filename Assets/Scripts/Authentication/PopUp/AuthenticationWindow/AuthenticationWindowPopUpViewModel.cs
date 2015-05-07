
using System;
public class AuthenticationWindowPopUpViewModel
{
	public string error;
	public string username;
	public string password;
	public bool toMemorize;

	public AuthenticationWindowPopUpViewModel ()
	{
		this.toMemorize = false;
		this.username = ApplicationModel.username;
		this.password = "";
	}
}

