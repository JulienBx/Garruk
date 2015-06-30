using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

public class ProfileController : MonoBehaviour 
{

	public GameObject MenuObject;
	public GameObject TutorialObject;
	private ProfileView view;
	public static ProfileController instance;
	private ProfileModel model;
	private string m_textPath;
	public GUIStyle[] friendshipStatusVMStyle;
	public GUIStyle[] statsVMStyle;
	public GUIStyle[] userProfileVMStyle;
	public GUIStyle[] profileScreenVMStyle;
	public GUIStyle[] profileVMStyle;
	public GUIStyle[] myTrophiesVMStyle;
	public GUIStyle[] popUpVMStyle;
	public Texture2D[] filePickerTextures;
	public GUISkin fileBrowserSkin;
	private bool isTutorialLaunched;
	private GameObject tutorial;
	private ProfileCheckPasswordPopUpView checkPasswordView;
	private ProfileChangePasswordPopUpView changePasswordView;
	private ProfileErrorPopUpView errorView;
	
	void Start () {
		
		instance = this;
		this.view = Camera.main.gameObject.AddComponent <ProfileView>();
		this.model = new ProfileModel ();
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		StartCoroutine (this.initialization ());
	}
	private IEnumerator initialization(){
		yield return StartCoroutine (model.getProfile ());
		ApplicationModel.profileChosen = "";
		this.picturesInitialization ();
		view.profileScreenVM = new ProfileScreenViewModel (this.filePickerTextures [0],
		                                                   this.filePickerTextures [1],
		                                                   this.fileBrowserSkin);
		view.myTrophiesVM.trophies = model.Trophies;
		view.userProfileVM.Profile = model.Profile;
		view.statsVM = new StatsViewModel (model.Profile.TotalNbWins,
		                                   model.Profile.TotalNbLooses,
		                                   model.Profile.Division,
		                                   model.Profile.Ranking,
		                                   model.Profile.RankingPoints,
		                                   model.Profile.CollectionPoints,
		                                   model.Profile.CollectionRanking);
		if(model.Profile.Username!=ApplicationModel.username)
		{
			view.profileVM.isMyProfile=false;
			view.profileScreenVM.displayTopRightBlock=true;
			view.myFriendsVM.title="Les amis de "+model.Profile.Username;
			view.userProfileVM.title="Ses informations";
		}
		this.initStyles ();
		this.loadData ();
		this.pictures ();
		view.setCanDisplay (true);
		if(!model.Player.MyProfileTutorial && view.profileVM.isMyProfile)
		{
			this.tutorial = Instantiate(this.TutorialObject) as GameObject;
			MenuObject.GetComponent<MenuController>().setTutorialLaunched(true);
			this.tutorial.AddComponent<MyProfileTutorialController>();
			this.tutorial.GetComponent<MyProfileTutorialController>().launchSequence(0);
			this.isTutorialLaunched=true;
		}
		else if(!model.Player.ProfileTutorial && !view.profileVM.isMyProfile)
		{
			this.tutorial = Instantiate(this.TutorialObject) as GameObject;
			MenuObject.GetComponent<MenuController>().setTutorialLaunched(true);
			this.tutorial.AddComponent<ProfileTutorialController>();
			this.tutorial.GetComponent<ProfileTutorialController>().launchSequence(0);
			this.isTutorialLaunched=true;
		}
	}
	private void loadData()
	{
		this.computeConnections ();
		this.computeLabelsNo ();
		if(model.Profile.Username!=ApplicationModel.username)
		{
			this.computeFriendshipStatus();
		}
		this.resize ();
	}
	private void computeConnections(){

		view.myFriendsVM.contacts = new List<User> ();
		view.myFriendsVM.contactsDisplayed = new List<int> ();
		view.invitationsReceivedVM.contacts = new List<User> ();
		view.invitationsReceivedVM.contactsDisplayed = new List<int> ();
		view.invitationsSentVM.contacts = new List<User> ();
		view.invitationsSentVM.contactsDisplayed = new List<int> ();

		for (int i =0; i<model.Connections.Count;i++)
		{
			if (model.Connections[i].IsAccepted)
			{
				view.myFriendsVM.contacts.Add (model.Contacts[i]);
				view.myFriendsVM.contactsDisplayed.Add (i);
			}
			else if(model.Profile.Username==ApplicationModel.username)
			{
				if(model.Connections[i].IdUser1==model.Player.Id)
				{
					view.invitationsSentVM.contacts.Add (model.Contacts[i]);
					view.invitationsSentVM.contactsDisplayed.Add (i);
				}
				else if(model.Connections[i].IdUser2==model.Player.Id) 
				{
					view.invitationsReceivedVM.contacts.Add (model.Contacts[i]);
					view.invitationsReceivedVM.contactsDisplayed.Add (i);
				}
			}
		}
	}
	private void computeLabelsNo ()
	{
		if(model.Profile.Username==ApplicationModel.username)
		{
			if(view.myFriendsVM.contacts.Count==0)
			{
				view.myFriendsVM.labelNo="Vous n'avez pas encore d'amis";
			}
			else
			{
				view.myFriendsVM.labelNo="";
			}
			if(view.invitationsReceivedVM.contacts.Count==0)
			{
				view.invitationsReceivedVM.labelNo="Vous n'avez pas d'invitations en attente";
			}
			else
			{
				view.invitationsReceivedVM.labelNo="";
			}
			if(view.invitationsSentVM.contacts.Count==0)
			{
				view.invitationsSentVM.labelNo="Vous n'avez pas envoye d'invitations";
			}
			else
			{
				view.invitationsSentVM.labelNo="";
			}
			if(view.myTrophiesVM.trophies.Count==0)
			{
				view.myTrophiesVM.labelNo="Vous n'avez pas encore remporté de trophée";
			}
			else
			{
				view.myTrophiesVM.labelNo="";
			}
		}
		else
		{
			if(view.myFriendsVM.contacts.Count==0)
			{
				view.myFriendsVM.labelNo=model.Profile.Username+" n'a pas encore d'amis";
			}
			else
			{
				view.myFriendsVM.labelNo="";
			}
			if(view.myTrophiesVM.trophies.Count==0)
			{
				view.myTrophiesVM.labelNo=model.Profile.Username+" n'a pas encore de trophée";
			}
			else
			{
				view.myTrophiesVM.labelNo="";
			}
		}
	}
	public void returnPressed()
	{
		if(this.errorView!=null)
		{
			this.hideErrorPopUp();
		}
		if(changePasswordView!=null)
		{
			this.editPasswordHandler();
		}
		if(checkPasswordView!=null)
		{
			this.checkPasswordHandler(checkPasswordView.checkPasswordPopUpVM.tempOldPassword);
		}
		if(view.userProfileVM.isEditing)
		{
			this.updateUserInformationsHandler();
		}
	}
	public void escapePressed()
	{
		if(view.userProfileVM.isEditing)
		{
			view.userProfileVM.isEditing=false;
		}
		if(this.changePasswordView!=null)
		{
			this.hideChangePasswordPopUp();
		}
		if(this.checkPasswordView!=null)
		{
			this.hideCheckPasswordPopUp();
		}
		if(this.errorView!=null)
		{
			this.hideErrorPopUp();
		}
		if(view.m_fileBrowser!=null)
		{
			view.m_fileBrowser=null;
		}
	}
	private void setGUI(bool value)
	{
		view.profileVM.guiEnabled = value;
		this.setButtonsGui (value);
	}
	public void setButtonsGui(bool value)
	{
		view.profileVM.buttonsEnabled = value;
	}
	public void displayChangePasswordPopUp()
	{
		this.setGUI (false);
		this.changePasswordView = Camera.main.gameObject.AddComponent <ProfileChangePasswordPopUpView>();
		changePasswordView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
		for(int i=0;i<this.popUpVMStyle.Length;i++)
		{
			changePasswordView.popUpVM.styles[i]=this.popUpVMStyle[i];
		}
		changePasswordView.popUpVM.initStyles();
		this.resizeChangePasswordPopUp();
	}
	public void displayCheckPasswordPopUp()
	{
		this.setGUI (false);
		this.checkPasswordView = Camera.main.gameObject.AddComponent <ProfileCheckPasswordPopUpView>();
		checkPasswordView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
		for(int i=0;i<this.popUpVMStyle.Length;i++)
		{
			checkPasswordView.popUpVM.styles[i]=this.popUpVMStyle[i];
		}
		checkPasswordView.popUpVM.initStyles();
		this.resizeCheckPasswordPopUp();
	}
	public void displayErrorPopUp()
	{
		this.setGUI (false);
		this.errorView = Camera.main.gameObject.AddComponent <ProfileErrorPopUpView>();
		errorView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
		for(int i=0;i<this.popUpVMStyle.Length;i++)
		{
			errorView.popUpVM.styles[i]=this.popUpVMStyle[i];
		}
		errorView.popUpVM.initStyles();
		this.resizeErrorPopUp();
	}
	public void hideChangePasswordPopUp()
	{
		this.setGUI (true);
		Destroy (this.changePasswordView);
	}
	public void hideCheckPasswordPopUp()
	{
		this.setGUI (true);
		Destroy (this.checkPasswordView);
	}
	public void hideErrorPopUp()
	{
		this.setGUI (true);
		Destroy (this.errorView);
	}
	public void resizeChangePasswordPopUp()
	{
		changePasswordView.popUpVM.centralWindow = view.profileScreenVM.centralWindow;
		changePasswordView.popUpVM.resize ();
	}
	public void resizeCheckPasswordPopUp()
	{
		checkPasswordView.popUpVM.centralWindow = view.profileScreenVM.centralWindow;
		checkPasswordView.popUpVM.resize ();
	}
	public void resizeErrorPopUp()
	{
		errorView.popUpVM.centralWindow = view.profileScreenVM.centralWindow;
		errorView.popUpVM.resize ();
	}
	private void picturesInitialization()
	{
		view.userProfileVM.ProfilePicture = model.Profile.texture;
		view.profileVM.contactsPicturesButtonStyle=new List<GUIStyle>();
		view.myTrophiesVM.trophiesPicturesButtonStyle=new List<GUIStyle>();
		
		for(int i =0;i<model.Contacts.Count;i++)
		{
			view.profileVM.contactsPicturesButtonStyle.Add(new GUIStyle());
			view.profileVM.contactsPicturesButtonStyle[i].normal.background=model.Contacts[i].texture;
		}
		for(int i =0;i<model.Trophies.Count;i++)
		{
			view.myTrophiesVM.trophiesPicturesButtonStyle.Add(new GUIStyle());
			view.myTrophiesVM.trophiesPicturesButtonStyle[i].normal.background=model.Trophies[i].texture;
		}
	}
	public void pictures(){
		
		StartCoroutine(model.Profile.setProfilePicture());
		for(int i =0;i<model.Contacts.Count;i++)
		{
			StartCoroutine(model.Contacts[i].setProfilePicture());
		}
		for(int i =0;i<model.Trophies.Count;i++)
		{
			StartCoroutine(model.Trophies[i].setPicture());
		}
	}
	private void initializePagination()
	{
		view.myFriendsVM.nbPages = Mathf.CeilToInt((view.myFriendsVM.contacts.Count-1) / (3*view.myFriendsVM.elementPerRow))+1;
		view.myFriendsVM.pageDebut = 0 ;
		if (view.myFriendsVM.nbPages>10){
			view.myFriendsVM.pageFin = 10 ;
		}
		else{
			view.myFriendsVM.pageFin = view.myFriendsVM.nbPages;
		}
		view.myFriendsVM.paginatorGuiStyle = new GUIStyle[view.myFriendsVM.nbPages];
		for (int i = 0; i < view.myFriendsVM.nbPages; i++) { 
			if (i==0){
				view.myFriendsVM.paginatorGuiStyle[i]=view.profileVM.paginationActivatedStyle;
			}
			else{
				view.myFriendsVM.paginatorGuiStyle[i]=view.profileVM.paginationStyle;
			}
		}
		view.myTrophiesVM.nbPages = Mathf.CeilToInt((view.myTrophiesVM.trophies.Count-1) / (3*view.myTrophiesVM.elementPerRow))+1;
		view.myTrophiesVM.pageDebut = 0 ;
		if (view.myTrophiesVM.nbPages>3){
			view.myTrophiesVM.pageFin = 3 ;
		}
		else{
			view.myTrophiesVM.pageFin = view.myTrophiesVM.nbPages;
		}
		view.myTrophiesVM.paginatorGuiStyle = new GUIStyle[view.myTrophiesVM.nbPages];
		for (int i = 0; i < view.myTrophiesVM.nbPages; i++) { 
			if (i==0){
				view.myTrophiesVM.paginatorGuiStyle[i]=view.profileVM.paginationActivatedStyle;
			}
			else{
				view.myTrophiesVM.paginatorGuiStyle[i]=view.profileVM.paginationStyle;
			}
		}
		if(model.Profile.Username==ApplicationModel.username)
		{
			view.invitationsReceivedVM.nbPages = Mathf.CeilToInt((view.invitationsReceivedVM.contacts.Count-1) / (3*view.invitationsReceivedVM.elementPerRow))+1;
			view.invitationsReceivedVM.pageDebut = 0 ;
			if (view.invitationsReceivedVM.nbPages>10){
				view.invitationsReceivedVM.pageFin = 10 ;
			}
			else{
				view.invitationsReceivedVM.pageFin = view.invitationsReceivedVM.nbPages;
			}
			view.invitationsReceivedVM.paginatorGuiStyle = new GUIStyle[view.invitationsReceivedVM.nbPages];
			for (int i = 0; i < view.invitationsReceivedVM.nbPages; i++) { 
				if (i==0){
					view.invitationsReceivedVM.paginatorGuiStyle[i]=view.profileVM.paginationActivatedStyle;
				}
				else{
					view.invitationsReceivedVM.paginatorGuiStyle[i]=view.profileVM.paginationStyle;
				}
			}
			view.invitationsSentVM.nbPages = Mathf.CeilToInt((view.invitationsSentVM.contacts.Count-1) / (3*view.invitationsSentVM.elementPerRow))+1;
			view.invitationsSentVM.pageDebut = 0 ;
			if (view.invitationsSentVM.nbPages>10){
				view.invitationsSentVM.pageFin = 10 ;
			}
			else{
				view.invitationsSentVM.pageFin = view.invitationsSentVM.nbPages;
			}
			view.invitationsSentVM.paginatorGuiStyle = new GUIStyle[view.invitationsSentVM.nbPages];
			for (int i = 0; i < view.invitationsSentVM.nbPages; i++) { 
				if (i==0){
					view.invitationsSentVM.paginatorGuiStyle[i]=view.profileVM.paginationActivatedStyle;
				}
				else{
					view.invitationsSentVM.paginatorGuiStyle[i]=view.profileVM.paginationStyle;
				}
			}
		}
	}
	private void computeFriendshipStatus()
	{
		view.friendshipStatusVM.username = model.Profile.Username;
		view.friendshipStatusVM.status=3;
		for(int i=0;i<model.Connections.Count;i++)
		{
			if(model.Connections[i].IdUser1==model.Player.Id && model.Connections[i].IsAccepted)
			{
				view.friendshipStatusVM.status=1;
				view.friendshipStatusVM.indexConnection=i;
				break;
			}
			else if(model.Connections[i].IdUser1==model.Player.Id && !model.Connections[i].IsAccepted)
			{
				view.friendshipStatusVM.status=2;
				view.friendshipStatusVM.indexConnection=i;
				break;
			}
			else if(model.Connections[i].IdUser2==model.Player.Id && model.Connections[i].IsAccepted)
			{
				view.friendshipStatusVM.status=1;
				view.friendshipStatusVM.indexConnection=i;
				break;
			}
			else if(model.Connections[i].IdUser2==model.Player.Id && !model.Connections[i].IsAccepted)
			{
				view.friendshipStatusVM.status=0;
				view.friendshipStatusVM.indexConnection=i;
				break;
			}
		}
	}
	private void initStyles()
	{
		view.profileScreenVM.styles=new GUIStyle[this.profileScreenVMStyle.Length];
		for(int i=0;i<this.profileScreenVMStyle.Length;i++)
		{
			view.profileScreenVM.styles[i]=this.profileScreenVMStyle[i];
		}
		view.profileScreenVM.initStyles();
		view.userProfileVM.styles=new GUIStyle[this.userProfileVMStyle.Length];
		for(int i=0;i<this.userProfileVMStyle.Length;i++)
		{
			view.userProfileVM.styles[i]=this.userProfileVMStyle[i];
		}
		view.userProfileVM.initStyles();
		view.statsVM.styles=new GUIStyle[this.statsVMStyle.Length];
		for(int i=0;i<this.statsVMStyle.Length;i++)
		{
			view.statsVM.styles[i]=this.statsVMStyle[i];
		}
		view.statsVM.initStyles();
		view.profileVM.styles=new GUIStyle[this.profileVMStyle.Length];
		for(int i=0;i<this.profileVMStyle.Length;i++)
		{
			view.profileVM.styles[i]=this.profileVMStyle[i];
		}
		view.profileVM.initStyles();
		view.friendshipStatusVM.styles=new GUIStyle[this.friendshipStatusVMStyle.Length];
		for(int i=0;i<this.friendshipStatusVMStyle.Length;i++)
		{
			view.friendshipStatusVM.styles[i]=this.friendshipStatusVMStyle[i];
		}
		view.friendshipStatusVM.initStyles();
		view.myTrophiesVM.styles=new GUIStyle[this.myTrophiesVMStyle.Length];
		for(int i=0;i<this.myTrophiesVMStyle.Length;i++)
		{
			view.myTrophiesVM.styles[i]=this.myTrophiesVMStyle[i];
		}
		view.myTrophiesVM.initStyles();
	}
	public void resize()
	{
		view.profileScreenVM.resize ();
		view.friendshipStatusVM.resize (view.profileScreenVM.heightScreen);
		view.invitationsReceivedVM.resize (view.profileScreenVM.heightScreen);
		view.invitationsSentVM.resize (view.profileScreenVM.heightScreen);
		view.statsVM.resize (view.profileScreenVM.heightScreen);
		view.userProfileVM.resize (view.profileScreenVM.heightScreen);
		view.myFriendsVM.resize (view.profileScreenVM.heightScreen);
		view.profileVM.resize (view.profileScreenVM.heightScreen);

		view.userProfileVM.profilePictureWidth = (int)view.profileScreenVM.blockLeftWidth-2*(int)view.profileScreenVM.gapBetweenblocks;
		view.userProfileVM.profilePictureHeight = view.userProfileVM.profilePictureWidth;

		view.userProfileVM.profilePictureRect = new Rect (view.profileScreenVM.blockLeft.xMin + (view.profileScreenVM.blockLeftWidth-view.userProfileVM.profilePictureWidth)/2,
		                                                  view.profileScreenVM.blockLeft.yMin + 0.05f*view.profileScreenVM.blockLeftHeight,
		                                                  view.userProfileVM.profilePictureWidth,
		                                                  view.userProfileVM.profilePictureHeight);

		view.myFriendsVM.elementPerRow = 4 + 2*Mathf.FloorToInt(((float)view.profileScreenVM.blockTopCenterWidth/(float)view.profileScreenVM.blockTopCenterHeight - 1.5f));
		view.invitationsReceivedVM.elementPerRow=Mathf.FloorToInt(view.myFriendsVM.elementPerRow/2f);
		view.invitationsSentVM.elementPerRow=view.invitationsReceivedVM.elementPerRow;
		view.myTrophiesVM.elementPerRow = 1;

		view.myFriendsVM.blocksWidth = view.profileScreenVM.blockTopCenterWidth / view.myFriendsVM.elementPerRow;
		view.myFriendsVM.blocksHeight = 0.8f*(view.profileScreenVM.blockTopCenterHeight / 3);
		view.myFriendsVM.blocks=new Rect[3*view.myFriendsVM.elementPerRow];
		for(int i=0;i<view.myFriendsVM.blocks.Length;i++)
		{
			view.myFriendsVM.blocks[i]=new Rect(view.profileScreenVM.blockTopCenter.xMin+(i%view.myFriendsVM.elementPerRow)*view.myFriendsVM.blocksWidth,
			                                    view.profileScreenVM.blockTopCenter.yMin+0.1f*view.profileScreenVM.blockTopCenterHeight+Mathf.FloorToInt(i/view.myFriendsVM.elementPerRow)*view.myFriendsVM.blocksHeight,
			                                    view.myFriendsVM.blocksWidth,
			                                    view.myFriendsVM.blocksHeight);
		}

		view.myTrophiesVM.blocksWidth = view.profileScreenVM.blockBottomRightWidth / view.myTrophiesVM.elementPerRow;
		view.myTrophiesVM.blocksHeight = 0.8f*(view.profileScreenVM.blockBottomRightHeight / 3);
		view.myTrophiesVM.blocks=new Rect[3*view.myTrophiesVM.elementPerRow];
		for(int i=0;i<view.myTrophiesVM.blocks.Length;i++)
		{
			view.myTrophiesVM.blocks[i]=new Rect(view.profileScreenVM.blockBottomRight.xMin+(i%view.myTrophiesVM.elementPerRow)*view.myTrophiesVM.blocksWidth,
			                                     view.profileScreenVM.blockBottomRight.yMin+0.1f*view.profileScreenVM.blockBottomRightHeight+Mathf.FloorToInt(i/view.myTrophiesVM.elementPerRow)*view.myTrophiesVM.blocksHeight,
			                                     view.myTrophiesVM.blocksWidth,
			                                     view.myTrophiesVM.blocksHeight);
		}

		if(model.Profile.Username==ApplicationModel.username)
		{
			view.userProfileVM.updateProfilePictureButtonHeight = view.userProfileVM.profilePictureHeight*10/100;
			view.userProfileVM.updateProfilePictureButtonWidth = view.userProfileVM.profilePictureWidth;

			view.userProfileVM.updateProfilePictureButtonRect = new Rect (view.userProfileVM.profilePictureRect.xMin,
			                                                              view.userProfileVM.profilePictureRect.yMax,
			                                                              view.userProfileVM.updateProfilePictureButtonWidth,
			                                                              view.userProfileVM.updateProfilePictureButtonHeight);

			view.invitationsSentVM.blocksWidth = view.profileScreenVM.blockBottomCenterLeftWidth / view.invitationsSentVM.elementPerRow;
			view.invitationsSentVM.blocksHeight = 0.8f*(view.profileScreenVM.blockBottomCenterLeftHeight / 3);
			view.invitationsSentVM.blocks=new Rect[3*view.invitationsSentVM.elementPerRow];
			for(int i=0;i<view.invitationsSentVM.blocks.Length;i++)
			{
				view.invitationsSentVM.blocks[i]=new Rect(view.profileScreenVM.blockBottomCenterLeft.xMin+(i%view.invitationsSentVM.elementPerRow)*view.invitationsSentVM.blocksWidth,
				                                          view.profileScreenVM.blockBottomCenterLeft.yMin+0.1f*view.profileScreenVM.blockBottomCenterLeftHeight+Mathf.FloorToInt(i/view.invitationsSentVM.elementPerRow)*view.invitationsSentVM.blocksHeight,
				                                    view.invitationsSentVM.blocksWidth,
				                                    view.myFriendsVM.blocksHeight);
			}

			view.invitationsReceivedVM.blocksWidth = view.profileScreenVM.blockBottomCenterRightWidth / view.invitationsReceivedVM.elementPerRow;
			view.invitationsReceivedVM.blocksHeight = 0.8f*(view.profileScreenVM.blockBottomCenterRightHeight / 3);
			view.invitationsReceivedVM.blocks=new Rect[3*view.invitationsReceivedVM.elementPerRow];
			for(int i=0;i<view.invitationsReceivedVM.blocks.Length;i++)
			{
				view.invitationsReceivedVM.blocks[i]=new Rect(view.profileScreenVM.blockBottomCenterRight.xMin+(i%view.invitationsReceivedVM.elementPerRow)*view.invitationsReceivedVM.blocksWidth,
				                                          view.profileScreenVM.blockBottomCenterRight.yMin+0.1f*view.profileScreenVM.blockBottomCenterRightHeight+Mathf.FloorToInt(i/view.invitationsReceivedVM.elementPerRow)*view.invitationsReceivedVM.blocksHeight,
				                                          view.invitationsReceivedVM.blocksWidth,
				                                          view.myFriendsVM.blocksHeight);
			}
			view.invitationsReceivedVM.chosenPage = 0;
			view.invitationsSentVM.chosenPage = 0;
		}
		view.myFriendsVM.chosenPage = 0;
		view.myTrophiesVM.chosenPage = 0;
		this.initializePagination();
		view.myTrophiesVM.displayPage ();
		view.myFriendsVM.displayPage ();
		if(model.Profile.Username==ApplicationModel.username)
		{
			view.invitationsReceivedVM.displayPage ();
			view.invitationsSentVM.displayPage ();
		}
		if(errorView!=null)
		{
			this.resizeErrorPopUp();
		}
		if(changePasswordView!=null)
		{
			this.resizeChangePasswordPopUp();
		}
		if(checkPasswordView!=null)
		{
			this.resizeCheckPasswordPopUp();
		}
		if(isTutorialLaunched)
		{
			this.tutorial.GetComponent<TutorialObjectController>().resize();
		}
	}
	public IEnumerator addConnection()
	{
		model.Connections.Add (new Connection (model.Player.Id, model.Profile.Id,false));
		model.Contacts.Add (model.Player);
		view.profileVM.contactsPicturesButtonStyle.Add(new GUIStyle());
		view.profileVM.contactsPicturesButtonStyle[view.profileVM.contactsPicturesButtonStyle.Count-1].normal.background=model.Contacts[model.Contacts.Count-1].texture;
		StartCoroutine(model.Player.setProfilePicture());
		yield return StartCoroutine(model.Connections [model.Connections.Count - 1].add ());
		if(model.Connections [model.Connections.Count - 1].Error=="")
		{
			this.loadData ();
			Notification tempNotification = new Notification(model.Profile.Id,model.Player.Id,false,4);
			StartCoroutine(tempNotification.add ());
		}
		else
		{
			this.displayErrorPopUp();
			errorView.errorPopUpVM.error=model.Connections[model.Connections.Count - 1].Error;
		}
	}
	public void removeConnection(int indexConnection)
	{
		Notification tempNotification = new Notification ();
		News tempNews1 = new News ();
		News tempNews2 = new News ();
		if(model.Connections[indexConnection].IsAccepted)
		{
			tempNotification = new Notification(model.Connections[indexConnection].IdUser1,model.Connections[indexConnection].IdUser2,false,3);
			tempNews1=new News(model.Connections[indexConnection].IdUser1,1,model.Connections[indexConnection].IdUser2.ToString());
			tempNews2=new News(model.Connections[indexConnection].IdUser2,1,model.Connections[indexConnection].IdUser1.ToString());
			StartCoroutine(tempNews1.remove ());
			StartCoroutine(tempNews2.remove ());
		}
		else
		{
			tempNotification = new Notification(model.Connections[indexConnection].IdUser2,model.Connections[indexConnection].IdUser1,false,4);
		}
		StartCoroutine(tempNotification.remove ());
		StartCoroutine(model.Connections [indexConnection].remove ());
		model.Connections.RemoveAt (indexConnection);
		model.Contacts.RemoveAt (indexConnection);
		view.profileVM.contactsPicturesButtonStyle.RemoveAt (indexConnection);
		this.loadData ();
	}
	public IEnumerator confirmConnection(int indexConnection)
	{
		model.Connections[indexConnection].IsAccepted=true;
		yield return StartCoroutine(model.Connections [indexConnection].confirm ());
		if(model.Connections[indexConnection].Error=="")
		{
			this.loadData ();
			Notification tempNotification1 = new Notification(model.Connections[indexConnection].IdUser1,model.Connections[indexConnection].IdUser2,false,3);
			StartCoroutine(tempNotification1.add ());
			Notification tempNotification2 = new Notification(model.Connections[indexConnection].IdUser2,model.Connections[indexConnection].IdUser1,false,4);
			StartCoroutine(tempNotification2.remove ());
			News tempNews1=new News(model.Connections[indexConnection].IdUser1, 1,model.Connections[indexConnection].IdUser2.ToString());
			StartCoroutine(tempNews1.add ());
			News tempNews2=new News(model.Connections[indexConnection].IdUser2, 1,model.Connections[indexConnection].IdUser1.ToString());
			StartCoroutine(tempNews2.add ());
		}
		else
		{
			this.displayErrorPopUp();
			errorView.errorPopUpVM.error=model.Connections[indexConnection].Error;
		}
	}
	public void reloadPage()
	{
		if(model.Profile.Username!=ApplicationModel.username)
		{
			ApplicationModel.profileChosen=model.Profile.Username;
		}
		Application.LoadLevel("Profile");
	}
	public void checkPasswordHandler(string password)
	{
		StartCoroutine (checkPassword (password));
	}
	private IEnumerator checkPassword(string password)
	{
		checkPasswordView.checkPasswordPopUpVM.error = this.checkPasswordComplexity (password);
		if(checkPasswordView.checkPasswordPopUpVM.error=="")
		{
			checkPasswordView.popUpVM.guiEnabled = false;
			yield return StartCoroutine(ApplicationModel.checkPassword(password));
			if(ApplicationModel.error=="")
			{
				this.hideCheckPasswordPopUp();
				this.displayChangePasswordPopUp();
			}
			else
			{
				checkPasswordView.checkPasswordPopUpVM.error=ApplicationModel.error;
				ApplicationModel.error="";
			}
			checkPasswordView.popUpVM.guiEnabled = true;
		}
	}
	public void editPasswordHandler()
	{
		changePasswordView.changePasswordPopUpVM.passwordsCheck = this.checkPasswordEgality (changePasswordView.changePasswordPopUpVM.tempNewPassword, changePasswordView.changePasswordPopUpVM.tempNewPassword2);
		if(changePasswordView.changePasswordPopUpVM.passwordsCheck=="")
		{
			changePasswordView.changePasswordPopUpVM.passwordsCheck=this.checkPasswordComplexity(changePasswordView.changePasswordPopUpVM.tempNewPassword);
		}
		if(changePasswordView.changePasswordPopUpVM.passwordsCheck=="")
		{
			StartCoroutine(this.editPassword(changePasswordView.changePasswordPopUpVM.tempNewPassword));
			changePasswordView.changePasswordPopUpVM.tempNewPassword="";
			changePasswordView.changePasswordPopUpVM.tempNewPassword2="";
		}
	}
	private IEnumerator editPassword(string password)
	{
		changePasswordView.popUpVM.guiEnabled = false;
		yield return StartCoroutine(ApplicationModel.editPassword(password));
		changePasswordView.popUpVM.guiEnabled = true;
		this.hideChangePasswordPopUp ();
	}
	public void pagination(int section, int scenario, int chosenPage=0)
	{
		switch(section)
		{
		case 0:
			switch(scenario)
			{
			case 0:
				view.myFriendsVM.pageDebut = view.myFriendsVM.pageDebut-10;
				view.myFriendsVM.pageFin = view.myFriendsVM.pageDebut+10;
				break;
			case 1:
				view.myFriendsVM.paginatorGuiStyle[view.myFriendsVM.chosenPage]=view.profileVM.paginationStyle;
				view.myFriendsVM.chosenPage=chosenPage;
				view.myFriendsVM.paginatorGuiStyle[chosenPage]=view.profileVM.paginationActivatedStyle;
				view.myFriendsVM.displayPage();
				break;
			case 2:
				view.myFriendsVM.pageDebut = view.myFriendsVM.pageDebut+10;
				view.myFriendsVM.pageFin= Mathf.Min(view.myFriendsVM.pageFin+10, view.myFriendsVM.nbPages);
				break;
			}
			break;
		case 1:
			switch(scenario)
			{
			case 0:
				view.invitationsSentVM.pageDebut = view.invitationsSentVM.pageDebut-10;
				view.invitationsSentVM.pageFin = view.invitationsSentVM.pageDebut+10;
				break;
			case 1:
				view.invitationsSentVM.paginatorGuiStyle[view.invitationsSentVM.chosenPage]=view.profileVM.paginationStyle;
				view.invitationsSentVM.chosenPage=chosenPage;
				view.invitationsSentVM.paginatorGuiStyle[chosenPage]=view.profileVM.paginationActivatedStyle;
				view.invitationsSentVM.displayPage();
				break;
			case 2:
				view.invitationsSentVM.pageDebut = view.invitationsSentVM.pageDebut+10;
				view.invitationsSentVM.pageFin= Mathf.Min(view.invitationsSentVM.pageFin+10, view.invitationsSentVM.nbPages);
				break;
			}
			break;
		case 2:
			switch(scenario)
			{
			case 0:
				view.invitationsReceivedVM.pageDebut = view.invitationsReceivedVM.pageDebut-10;
				view.invitationsReceivedVM.pageFin = view.invitationsReceivedVM.pageDebut+10;
				break;
			case 1:
				view.invitationsReceivedVM.paginatorGuiStyle[view.invitationsReceivedVM.chosenPage]=view.profileVM.paginationStyle;
				view.invitationsReceivedVM.chosenPage=chosenPage;
				view.invitationsReceivedVM.paginatorGuiStyle[chosenPage]=view.profileVM.paginationActivatedStyle;
				view.invitationsReceivedVM.displayPage();
				break;
			case 2:
				view.invitationsReceivedVM.pageDebut = view.invitationsReceivedVM.pageDebut+10;
				view.invitationsReceivedVM.pageFin= Mathf.Min(view.invitationsReceivedVM.pageFin+10, view.invitationsSentVM.nbPages);
				break;
			}
			break;
		case 3:
			switch(scenario)
			{
			case 0:
				view.myTrophiesVM.pageDebut = view.myTrophiesVM.pageDebut-3;
				view.myTrophiesVM.pageFin = view.myTrophiesVM.pageDebut+3;
				break;
			case 1:
				view.myTrophiesVM.paginatorGuiStyle[view.myTrophiesVM.chosenPage]=view.profileVM.paginationStyle;
				view.myTrophiesVM.chosenPage=chosenPage;
				view.myTrophiesVM.paginatorGuiStyle[chosenPage]=view.profileVM.paginationActivatedStyle;
				view.myTrophiesVM.displayPage();
				break;
			case 2:
				view.myTrophiesVM.pageDebut = view.myTrophiesVM.pageDebut+3;
				view.myTrophiesVM.pageFin= Mathf.Min(view.myTrophiesVM.pageFin+3, view.myTrophiesVM.nbPages);
				break;
			}
			break;
		}
	}
	public void editProfileInformations(bool value)
	{
		view.userProfileVM.error = "";
		view.userProfileVM.isEditing = value;
		view.userProfileVM.tempSurname = model.Player.Surname;
		view.userProfileVM.tempFirstName = model.Player.FirstName;
		view.userProfileVM.tempMail = model.Player.Mail;
	}
	public void updateUserInformationsHandler()
	{
		view.userProfileVM.error = this.checkname (view.userProfileVM.tempSurname);
		if(view.userProfileVM.error=="")
		{
			view.userProfileVM.error = this.checkname (view.userProfileVM.tempFirstName);
		}
		if(view.userProfileVM.error=="")
		{
			view.userProfileVM.error = this.checkEmail (view.userProfileVM.tempMail);
		}
		if(view.userProfileVM.error=="")
		{
			StartCoroutine(updateUserInformations(view.userProfileVM.tempFirstName,view.userProfileVM.tempSurname,view.userProfileVM.tempMail));
		}
	}
	public string checkname(string name)
	{
		if(!Regex.IsMatch(name, @"^[a-zA-Z0-9_]+$"))
		{
			return "Vous ne pouvez pas utiliser de caractères spéciaux";
		}   
		return "";
	}
	public string checkPasswordEgality (string password1, string password2)
	{
		if(password1=="")
		{
			return "Veuillez saisir un mot de passe";
		}
		else if(password2=="")
		{
			return "Veuillez confirmer votre mot de passe";
		}
		else if(password1!=password2)
		{
			return "Les deux mots de passes doivent être identiques";
		}
		return "";
	}
	public string checkPasswordComplexity(string password)
	{
		if(password.Length<5)
		{
			return "Le mot de passe doit comporter au moins 5 caractères";
		}
		else if(!Regex.IsMatch(password, @"^[a-zA-Z0-9_.@]+$"))
		{
			return "Le mot de passe ne peut comporter de caractères spéciaux hormis @ _ et .";
		} 
		return "";
	}
	public string checkEmail(string email)
	{
		if(!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
		{
			return "Veuillez saisir une adresse email valide";
		}
		return "";
	}
	private IEnumerator updateUserInformations(string firstname, string surname, string mail)
	{
		this.setGUI (false);
		model.Profile.FirstName = firstname;
		model.Profile.Surname = surname;
		model.Profile.Mail = mail;
		yield return StartCoroutine (model.Profile.updateInformations ());
		this.setGUI (true);
		view.userProfileVM.isEditing = false;
	}
	public void updateProfilePictureHandler(string path)
	{
		StartCoroutine (updateProfilePicture (path));
	}
	private IEnumerator updateProfilePicture(string path)
	{
		this.setGUI (false);
		File tempFile = new File ();
		yield return StartCoroutine (tempFile.createProfilePicture (path));
		if(tempFile.Error!="")
		{
			this.displayErrorPopUp();
			this.errorView.errorPopUpVM.error=tempFile.Error;
		}
		else
		{
			yield return StartCoroutine(model.Profile.updateProfilePicture(tempFile));
			if(model.Profile.Error!="")
			{
				this.displayErrorPopUp();
				this.errorView.errorPopUpVM.error=model.Profile.Error;
				model.Profile.Error="";
			}
			else
			{
				StartCoroutine (model.Profile.setProfilePicture());
				this.setGUI(true);
			}
		}
	}
	public IEnumerator endTutorial()
	{
		if(view.profileVM.isMyProfile)
		{
			yield return StartCoroutine (model.Player.setMyProfileTutorial(true));
		}
		else
		{
			yield return StartCoroutine (model.Player.setProfileTutorial(true));
		}
		MenuController.instance.setButtonsGui (true);
		Destroy (this.tutorial);
		this.isTutorialLaunched = false;
		MenuController.instance.isTutorialLaunched = false;
		this.setGUI (true);
	}
}

