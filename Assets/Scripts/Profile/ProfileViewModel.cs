using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ProfileViewModel {

	public IList<GUIStyle> contactsPicturesButtonStyle;
	public GUIStyle[] styles;
	public GUIStyle labelNoStyle;
	public GUIStyle actionButtonStyle;
	public GUIStyle titleStyle;
	public GUIStyle paginationStyle;
	public GUIStyle paginationActivatedStyle;
	public GUIStyle contactsUsernameStyle;
	public GUIStyle contactsInformationStyle;
	public GUIStyle contactsBackgroundStyle;
	public bool isMyProfile;
	public bool buttonsEnabled; 
	public bool guiEnabled;
	public bool isEditing;
	public string tempSurname;
	public string tempFirstName;
	public string tempMail;

	public ProfileViewModel ()
	{
		this.isMyProfile = true;
		this.buttonsEnabled = true;
		this.guiEnabled = true;
		this.tempSurname = "";
		this.tempFirstName = "";
		this.tempMail = "";
	}
	public void initStyles()
	{
		this.labelNoStyle = this.styles [0];
		this.actionButtonStyle = this.styles [1];
		this.titleStyle = this.styles [2];
		this.contactsUsernameStyle = this.styles [3];
		this.contactsInformationStyle = this.styles [4];
		this.paginationStyle = this.styles [5];
		this.paginationActivatedStyle = this.styles [6];
		this.contactsBackgroundStyle = this.styles [7];
	}
	public void resize(int heightScreen)
	{
		this.labelNoStyle.fontSize = heightScreen*2/100;
		this.actionButtonStyle.fontSize = heightScreen*2/100;
		this.titleStyle.fontSize = heightScreen * 25 / 1000;
		this.contactsUsernameStyle.fontSize = heightScreen * 2 / 100;
		this.contactsInformationStyle.fontSize = heightScreen * 15 / 1000;
		this.paginationStyle.fontSize = heightScreen*2/100;
		this.paginationActivatedStyle.fontSize = heightScreen*2/100;
	}
}
