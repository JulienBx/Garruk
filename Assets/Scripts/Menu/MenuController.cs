using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MenuController : MonoBehaviour {

	public GUIStyle[] notificationsReminderVMStyle;
	public GUIStyle[] userDataVMStyle;
	public GUIStyle[] menuVMStyle;
	public int totalNbResultLimit;
	public int refreshInterval;
	public bool isTutorialLaunched;

	private MenuView view;
	public static MenuController instance;
	private MenuModel model;

	private float timer;

	void Start () {
		
		instance = this;
		this.view = Camera.main.gameObject.AddComponent <MenuView>();
		this.view.notificationsReminderVM.nbNotificationsNonRead = ApplicationModel.nbNotificationsNonRead;
		this.view.userDataVM.username = ApplicationModel.username;
		this.view.userDataVM.credits = ApplicationModel.credits;
		this.model = new MenuModel ();
		this.initStyles ();
		this.resize ();
		StartCoroutine (this.getUserData ());
	}
	void Update(){
		
		timer += Time.deltaTime;
		
		if (timer > refreshInterval) {
			timer=timer-refreshInterval;
			StartCoroutine (this.getUserData());
		}
	}
	public void setNbNotificationsNonRead(int value)
	{
		view.notificationsReminderVM.nbNotificationsNonRead = value;
		ApplicationModel.nbNotificationsNonRead = value;
	}
	public IEnumerator getUserData()
	{
		yield return StartCoroutine (model.loadUserData (this.totalNbResultLimit));
		if(Application.loadedLevelName!="HomePage")
		{
			view.notificationsReminderVM.nbNotificationsNonRead = model.nbNotificationsNonRead;
			ApplicationModel.nbNotificationsNonRead = model.nbNotificationsNonRead;
		}
		view.userDataVM.credits = model.credits;
		ApplicationModel.credits = model.credits;
	}
	private void initStyles()
	{
		view.menuVM.styles=new GUIStyle[this.menuVMStyle.Length];
		for(int i=0;i<this.menuVMStyle.Length;i++)
		{
			view.menuVM.styles[i]=this.menuVMStyle[i];
		}
		view.menuVM.initStyles();
		view.notificationsReminderVM.styles=new GUIStyle[this.notificationsReminderVMStyle.Length];
		for(int i=0;i<this.notificationsReminderVMStyle.Length;i++)
		{
			view.notificationsReminderVM.styles[i]=this.notificationsReminderVMStyle[i];
		}
		view.notificationsReminderVM.initStyles();
		view.userDataVM.styles=new GUIStyle[this.userDataVMStyle.Length];
		for(int i=0;i<this.userDataVMStyle.Length;i++)
		{
			view.userDataVM.styles[i]=this.userDataVMStyle[i];
		}
		view.userDataVM.initStyles();
	}
	public void resize()
	{
		view.menuScreenVM.resize ();
		view.menuVM.resize (view.menuScreenVM.heightScreen,view.menuScreenVM.widthScreen);
		view.notificationsReminderVM.resize (view.menuScreenVM.heightScreen);
		view.userDataVM.resize (view.menuScreenVM.heightScreen,view.menuScreenVM.widthScreen);

		view.notificationsReminderVM.flexibleSpaceSize = (view.menuScreenVM.widthScreen 
		                                                  - view.menuVM.titleStyle.fixedWidth 
		                                                  - 5f * view.menuVM.buttonStyle.fixedWidth 
		                                                  - view.userDataVM.welcomeStyle.fixedWidth 
		                                                  - view.notificationsReminderVM.nonReadNotificationsButtonStyle.fixedWidth) / 13;
		view.notificationsReminderVM.distanceToNonReadNotificationsCounter = 3f * view.notificationsReminderVM.flexibleSpaceSize 
			+ view.menuVM.titleStyle.fixedWidth 
				+ view.menuVM.buttonStyle.fixedWidth 
				+ 0.75f * view.notificationsReminderVM.nonReadNotificationsButtonStyle.fixedWidth;

		view.notificationsReminderVM.nonReadNotificationsCounter=new Rect(view.notificationsReminderVM.distanceToNonReadNotificationsCounter,
		                                                                  0.01f*view.menuScreenVM.heightScreen,
		                                                                  view.notificationsReminderVM.nonReadNotificationsButtonStyle.fixedHeight*0.5f,
		                                                                  view.notificationsReminderVM.nonReadNotificationsButtonStyle.fixedHeight*0.5f);
	}
	public void homePageLink()
	{
		if(Application.loadedLevelName=="Lobby")
		{
			PhotonNetwork.Disconnect();
		}
		Application.LoadLevel("HomePage");
	}
	public void myGameLink()
	{
		if(Application.loadedLevelName=="Lobby")
		{
			PhotonNetwork.Disconnect();
		}
		if(this.isTutorialLaunched)
		{
			TutorialObjectController.instance.actionIsDone();
		}
		else
		{
			Application.LoadLevel("MyGame");
		}
	}
	public void shopLink()
	{
		if(Application.loadedLevelName=="Lobby"){
			PhotonNetwork.Disconnect();
		}
		if(this.isTutorialLaunched)
		{
			TutorialObjectController.instance.actionIsDone();
		}
		else
		{
			Application.LoadLevel("Store");
		}
	}
	public void marketLink()
	{
		if(Application.loadedLevelName=="Lobby"){
			PhotonNetwork.Disconnect();
		}
		Application.LoadLevel("Market");
	}
	
	public void logOutLink() 
	{
		ApplicationModel.username = "";
		ApplicationModel.toDeconnect = true;
		if(Application.loadedLevelName=="Lobby"){
			PhotonNetwork.Disconnect();
		}
		Application.LoadLevel("Authentication");
	}
	public void profileLink() 
	{
		if(Application.loadedLevelName=="Lobby"){
			PhotonNetwork.Disconnect();
		}
		Application.LoadLevel("Profile");
	}
	public void lobbyLink()
	{
		if(Application.loadedLevelName=="Lobby"){
			PhotonNetwork.Disconnect();
		}
		if(this.isTutorialLaunched)
		{
			TutorialObjectController.instance.actionIsDone();
		}
		else
		{
			Application.LoadLevel("Lobby");
		}
	}
	public void setButtonsGui(bool value)
	{
		for(int i=0;i<view.menuVM.buttonsEnabled.Length;i++)
		{
			view.menuVM.buttonsEnabled[i]=value;
		}
	}
	public void setButtonGui(int index, bool value)
	{
		view.menuVM.buttonsEnabled[index]=value;
	}
	public void setTutorialLaunched(bool value)
	{
		this.isTutorialLaunched = value;
	}
}

