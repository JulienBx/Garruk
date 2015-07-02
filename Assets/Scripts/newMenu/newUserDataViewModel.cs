using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class newUserDataViewModel 
{
	public int credits;
	public string username;
	public GUIStyle usernameStyle;
	public GUIStyle creditsStyle;
	public GUIStyle profilePictureStyle;
	public GUIStyle profilePictureBorderStyle;
	public GUIStyle creditPictureStyle;
	public Rect profileRect;
	public Rect profilePictureBorderRect;
	public Rect creditsRect;
	public float profilePictureHeight;
	public float creditsPictureHeight;
	
	public newUserDataViewModel ()
	{
		this.usernameStyle = new GUIStyle ();
		this.creditsStyle = new GUIStyle ();
		this.profilePictureStyle = new GUIStyle ();
		this.profilePictureBorderStyle = new GUIStyle ();
		this.profileRect = new Rect ();
		this.creditsRect = new Rect ();
		this.profilePictureBorderRect = new Rect ();
	}
	public void resize()
	{	

		float usernameFontRatio = 1;
		if(this.username.Length>5)
		{
			usernameFontRatio=5f/this.username.Length;
		}
		float creditsFontRatio = 1;
		if(this.credits.ToString().Length>5)
		{
			creditsFontRatio=5f/this.credits.ToString().Length;
		}
		this.usernameStyle.fontSize = (int)this.profilePictureHeight*((int)usernameFontRatio)*5/10;
		this.creditsStyle.fontSize = (int)this.profilePictureHeight*((int)creditsFontRatio)*6/10;
		
	}
	
}

