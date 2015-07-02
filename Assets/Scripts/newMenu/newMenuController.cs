using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class newMenuController : MonoBehaviour {

	public int totalNbResultLimit;
	public int refreshInterval;
	public bool isTutorialLaunched;
	public GUISkin mainSkin;
	public GUISkin titlesSkin;
	public GUISkin menuSkin;

	
	private newMenuView view;
	public static newMenuController instance;
	private newMenuModel model;
	private int currentPage;
	private int pageHovered;
	private float timer;
	private bool toMoveButtons;
	private bool toMoveBackButtons;
	private float speed;
	private float startButtonPosition;
	private float endButtonPosition;
	
	void Start () 
	{	
		instance = this;
		this.speed = 1500;
		this.toMoveButtons = false;
		this.view = Camera.main.gameObject.AddComponent <newMenuView>();
		this.view.notificationsReminderVM.nbNotificationsNonRead = ApplicationModel.nbNotificationsNonRead;
		this.view.userDataVM.username = ApplicationModel.username;
		this.view.userDataVM.credits = ApplicationModel.credits;
		this.model = new newMenuModel ();
		this.initStyles ();
		this.resize ();
		StartCoroutine (this.getUserData ());
		if(ApplicationModel.isAdmin)
		{
			view.menuVM.displayAdmin=true;
		}
	}
	void Update()
	{
		timer += Time.deltaTime;
		
		if (timer > refreshInterval) {
			timer=timer-refreshInterval;
			StartCoroutine (this.getUserData());
		}
		if(toMoveButtons)
		{
			this.toMoveButtons=false;
			for(int i=0;i<view.menuScreenVM.buttonsArea.Length;i++)
			{
				if(i!=this.pageHovered && view.menuScreenVM.buttonsArea[i].x!=this.startButtonPosition)
				{
					view.menuScreenVM.buttonsArea[i].x=view.menuScreenVM.buttonsArea[i].x-Time.deltaTime*this.speed;
					if(view.menuScreenVM.buttonsArea[i].x<this.startButtonPosition)
					{
						view.menuScreenVM.buttonsArea[i].x=this.startButtonPosition;
					}
					else
					{
						this.toMoveButtons=true;
					}
				}
				else if(i==this.pageHovered && view.menuScreenVM.buttonsArea[i].x!=this.endButtonPosition)
				{
					view.menuScreenVM.buttonsArea[i].x=view.menuScreenVM.buttonsArea[i].x+Time.deltaTime*this.speed;
					if(view.menuScreenVM.buttonsArea[i].x>this.endButtonPosition)
					{
						view.menuScreenVM.buttonsArea[i].x=this.endButtonPosition;
					}
					else
					{
						this.toMoveButtons=true;
					}
				}
			}
		}
		else
		{
			if(this.toMoveBackButtons)
			{
				this.pageHovered=this.currentPage;
				this.toMoveButtons=true;
				this.toMoveBackButtons=false;
			}
		}
	}
	public void moveButton(int value)
	{
		this.toMoveButtons = true;
		this.pageHovered = value;
		if(this.pageHovered!=this.currentPage)
		{
			this.toMoveBackButtons=true;
		}
	}
	public void setCurrentPage(int value)
	{
		this.currentPage = value;
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
			view.notificationsReminderVM.nbNotificationsNonRead = model.player.nonReadNotifications;
			ApplicationModel.nbNotificationsNonRead = model.player.nonReadNotifications;
		}
		view.userDataVM.credits = model.player.Money;
		ApplicationModel.credits = model.player.Money;
		view.userDataVM.profilePictureStyle.normal.background = model.player.texture;
		view.userDataVM.profilePictureStyle.stretchHeight = true;
		StartCoroutine (model.player.setThumbProfilePicture ());
	}
	private void initStyles()
	{
		view.menuVM.buttonStyle = new GUIStyle(this.menuSkin.button);
		view.menuVM.logoStyle = new GUIStyle(this.menuSkin.customStyles [0]);
		view.userDataVM.creditsStyle = new GUIStyle(this.titlesSkin.label);
		view.userDataVM.usernameStyle = new GUIStyle(this.titlesSkin.label);
		view.userDataVM.creditPictureStyle = new GUIStyle(this.menuSkin.customStyles [1]);
		view.notificationsReminderVM.nonReadNotificationsButtonStyle = new GUIStyle(this.menuSkin.customStyles [2]);
		view.notificationsReminderVM.nonReadNotificationsCounterPoliceStyle = new GUIStyle(this.menuSkin.customStyles [3]);
		view.userDataVM.profilePictureBorderStyle = new GUIStyle (this.menuSkin.customStyles [4]);
	}
	public void resize()
	{
		view.menuScreenVM.resize ();
		this.startButtonPosition = -view.menuScreenVM.mainBlock.width*1f/3f;
		this.endButtonPosition = -view.menuScreenVM.mainBlock.width*1f/10f;
		view.menuScreenVM.buttonWidth = view.menuScreenVM.mainBlock.width;
		view.menuScreenVM.buttonsAreaHeight= (view.menuScreenVM.mainBlock.height*0.55f) / (view.menuScreenVM.buttonsArea.Length);
		view.menuScreenVM.buttonHeight = (view.menuScreenVM.mainBlock.height*0.55f) / (view.menuScreenVM.buttonsArea.Length+1);

		for(int i=0;i<view.menuScreenVM.buttonsArea.Length;i++)
		{
			float start;
			if(i!=this.currentPage)
			{
				start=this.startButtonPosition;
			}
			else
			{
				start=this.endButtonPosition;
			}
			view.menuScreenVM.buttonsArea[i]=new Rect(start,view.menuScreenVM.mainBlock.height*0.25f+i*view.menuScreenVM.buttonsAreaHeight,view.menuScreenVM.buttonWidth,view.menuScreenVM.buttonsAreaHeight);
		}
		view.menuVM.resize (view.menuScreenVM.buttonHeight, view.menuScreenVM.buttonWidth);


		view.notificationsReminderVM.nonReadNotificationsLogo = new Rect (view.menuScreenVM.mainBlock.width * (1f / 6f), view.menuScreenVM.mainBlock.height * 0.19f, view.menuScreenVM.mainBlock.height * 0.05f, view.menuScreenVM.mainBlock.height * 0.05f);
		view.notificationsReminderVM.nonReadNotificationsCounter = new Rect (view.notificationsReminderVM.nonReadNotificationsLogo.x + view.notificationsReminderVM.nonReadNotificationsLogo.width * (5f / 5f),
		                                                                     view.notificationsReminderVM.nonReadNotificationsLogo.y + view.notificationsReminderVM.nonReadNotificationsLogo.height * (4f / 5f),
		                                                                     view.notificationsReminderVM.nonReadNotificationsLogo.height * (5f / 5f),
		                                                                     view.notificationsReminderVM.nonReadNotificationsLogo.height * (2f / 5f));

		view.userDataVM.profilePictureHeight = view.menuScreenVM.mainBlock.height * 0.07f;
		view.userDataVM.creditsPictureHeight = view.menuScreenVM.mainBlock.height * 0.04f;

		view.userDataVM.profileRect = new Rect (view.menuScreenVM.mainBlock.width * 0.05f,
		                                        view.menuScreenVM.mainBlock.height -view.userDataVM.profilePictureHeight - view.userDataVM.creditsPictureHeight,
		                                        view.menuScreenVM.mainBlock.width * 0.95f,
		                                        view.userDataVM.profilePictureHeight);

		view.userDataVM.profilePictureBorderRect = new Rect (view.userDataVM.profileRect.x,
		                                                     view.userDataVM.profileRect.y,
		                                                     view.userDataVM.profileRect.height,
		                                                     view.userDataVM.profileRect.height);

		view.userDataVM.creditsRect = new Rect (view.userDataVM.profilePictureBorderRect.xMax,
		                                       view.userDataVM.profilePictureBorderRect.yMax-view.menuScreenVM.mainBlock.height*0.02f,
		                                       view.menuScreenVM.mainBlock.width * 0.870f,
		                                       view.userDataVM.creditsPictureHeight);

		view.userDataVM.resize ();
		view.notificationsReminderVM.resize (view.menuScreenVM.heightScreen);
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
	public void adminBoardLink() 
	{
		if(Application.loadedLevelName=="Lobby"){
			PhotonNetwork.Disconnect();
		}
		Application.LoadLevel("AdminBoard");
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

