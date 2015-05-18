using System;
using UnityEngine;

public class AuthenticationViewModel
{
	public GUIStyle[] styles;
	public GUIStyle buttonStyle;
	public GUIStyle titleStyle;
	public GUIStyle textFieldStyle;
	public GUIStyle passwordFieldStyle;
	public GUIStyle labelStyle;
	public GUIStyle errorStyle;
	public string username;
	public string email;
	public string password1;
	public string password2;
	public string usernameError;
	public string emailError;
	public string passwordError;
	public bool guiEnabled;

	public AuthenticationViewModel ()
	{
		this.styles = new GUIStyle[0];
		this.buttonStyle = new GUIStyle ();
		this.titleStyle = new GUIStyle ();
		this.textFieldStyle = new GUIStyle ();
		this.passwordFieldStyle = new GUIStyle ();
		this.labelStyle = new GUIStyle ();
		this.errorStyle = new GUIStyle ();
		this.username = "";
		this.email = "";
		this.password1 = "";
		this.password2 = "";
		this.usernameError = "";
		this.emailError = "";
		this.passwordError = "";
		this.guiEnabled = false;
	}
	public void initStyles()
	{
		this.buttonStyle = this.styles [0];
		this.titleStyle = this.styles [1];
		this.textFieldStyle = this.styles [2];
		this.passwordFieldStyle = this.styles [3];
		this.labelStyle = this.styles [4];
		this.errorStyle = this.styles [5];
	}
	public void resize(int heightScreen)
	{
		this.buttonStyle.fontSize = heightScreen * 3 / 100;
		this.titleStyle.fontSize = heightScreen * 3 / 100;
		this.textFieldStyle.fontSize = heightScreen * 2 / 100;
		this.passwordFieldStyle.fontSize = heightScreen * 2 / 100;
		this.labelStyle.fontSize = heightScreen * 2 / 100;
		this.errorStyle.fontSize = heightScreen * 2 / 100;
	}
}

