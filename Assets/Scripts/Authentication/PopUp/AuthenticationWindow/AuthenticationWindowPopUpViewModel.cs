
using System;
public class AuthenticationWindowPopUpViewModel
{
	public string username;
	public string email;
	public string password1;
	public string password2;
	public string usernameError;
	public string emailError;
	public string passwordError;
	public bool guiEnabled;

	public AuthenticationWindowPopUpViewModel ()
	{
		this.username = "";
		this.email = "";
		this.password1 = "";
		this.password2 = "";
		this.usernameError = "";
		this.emailError = "";
		this.passwordError = "";
		this.guiEnabled = true;
	}
}

