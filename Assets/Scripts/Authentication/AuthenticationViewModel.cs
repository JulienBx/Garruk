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
	public GUIStyle toggleStyle;


	public string error;
	public string username;
	public string password;
	public bool toMemorize;
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
		this.toggleStyle = new GUIStyle ();

		this.toMemorize = false;
		this.username = ApplicationModel.username;
		this.password = "";
		this.guiEnabled = true;
		this.error = "";

	}
	public void initStyles()
	{
		this.buttonStyle = this.styles [0];
		this.titleStyle = this.styles [1];
		this.textFieldStyle = this.styles [2];
		this.passwordFieldStyle = this.styles [3];
		this.labelStyle = this.styles [4];
		this.errorStyle = this.styles [5];
		this.toggleStyle = this.styles [6];
	}
	public void resize(int heightScreen)
	{
		this.buttonStyle.fontSize = heightScreen * 3 / 100;
		this.titleStyle.fontSize = heightScreen * 3 / 100;
		this.textFieldStyle.fontSize = heightScreen * 2 / 100;
		this.passwordFieldStyle.fontSize = heightScreen * 2 / 100;
		this.labelStyle.fontSize = heightScreen * 2 / 100;
		this.errorStyle.fontSize = heightScreen * 2 / 100;
		this.toggleStyle.fontSize = heightScreen * 2 / 100;
	}




}

