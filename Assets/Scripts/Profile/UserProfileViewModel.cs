using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class UserProfileViewModel {
	
	public Texture2D ProfilePicture;
	public User Profile;
	public int profilePictureWidth;
	public int profilePictureHeight;
	public int updateProfilePictureButtonHeight;
	public int updateProfilePictureButtonWidth;
	public string title;
	public string error;
	public string tempSurname;
	public string tempFirstName;
	public string tempMail;
	public bool isEditing;

	public GUIStyle[] styles;
	public GUIStyle profilePictureStyle;
	public GUIStyle profileDataStyle;
	public GUIStyle editProfilePictureButtonStyle;
	public GUIStyle editProfileDataButtonStyle;
	public GUIStyle inputTextfieldStyle;
	public GUIStyle errorStyle;
	public Rect profilePictureRect;
	public Rect updateProfilePictureButtonRect;
	
	public UserProfileViewModel (){
		this.tempSurname = "";
		this.tempFirstName = "";
		this.tempMail = "";
		this.title = "Mes informations";
		this.error = "";
		this.isEditing = false;
	}

	public void initStyles(){
		this.profilePictureStyle = this.styles [0];
		this.profileDataStyle = this.styles [1];
		this.editProfilePictureButtonStyle = this.styles [2];
		this.editProfileDataButtonStyle = this.styles [3];
		this.inputTextfieldStyle = this.styles [4];
		this.errorStyle = this.styles [5];
	}
	public void resize(int heightScreen){
		this.profileDataStyle.fontSize = heightScreen * 2 / 100;
		this.editProfilePictureButtonStyle.fontSize = heightScreen * 2 / 100;
		this.editProfileDataButtonStyle.fontSize = heightScreen * 2 / 100;
		this.inputTextfieldStyle.fontSize=heightScreen * 2 / 100;
		this.errorStyle.fontSize = heightScreen * 2 / 100;
	}


	
}
