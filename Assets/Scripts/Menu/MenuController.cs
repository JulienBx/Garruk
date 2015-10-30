using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class MenuController : MonoBehaviour 
{
	public static MenuController instance;
	private MenuModel model;
	private MenuRessources ressources;
	private MenuPhotonController photon;
	private int currentPage;
	private float timer;
	private GameObject playPopUp;
	private GameObject invitationPopUp;
	private GameObject transparentBackground;
	private bool isPlayPopUpDisplayed;
	private bool isInvitationPopUpDisplayed;
	private bool isTransparentBackgroundDisplayed;
	private Rect centralWindow;
	private GameObject loadingScreen;
	
	private bool isDisconnectedViewDisplayed;
	private NewMenuDisconnectedPopUpView disconnectedView;
	
	private bool isLoadingScreenDisplayed;
	public bool isTutorialLaunched;
	
	private bool isInviting;
	
	private newMenuErrorPopUpView errorView;
	private bool errorViewDisplayed;
	
	private bool isUserBusy;

	
	void Awake()
	{
		instance = this;
		this.model = new MenuModel ();
		this.ressources = this.gameObject.GetComponent<MenuRessources> ();
		this.photon = this.gameObject.GetComponent<MenuPhotonController> ();
		ApplicationDesignRules.widthScreen = Screen.width;
		ApplicationDesignRules.heightScreen = Screen.height;

	}
	void Start () 
	{	
		StartCoroutine (this.initialization());
	}
	void Update()
	{
		timer += Time.deltaTime;
		
		if (timer > this.ressources.refreshInterval) 
		{
			timer=timer-this.ressources.refreshInterval;
			StartCoroutine (this.getUserData());
		}
		if(Input.GetKeyDown(KeyCode.Return)) 
		{
			this.returnPressed();
		}
		if(Input.GetKeyDown(KeyCode.Escape) && !isUserBusy) 
		{
			this.escapePressed();
		}
		if (Screen.width != ApplicationDesignRules.widthScreen || Screen.height!=ApplicationDesignRules.heightScreen) 
		{
			this.resizeAll();
		}
	}
	public void displayErrorPopUp(string error)
	{
		this.errorViewDisplayed = true;
		this.errorView = Camera.main.gameObject.AddComponent <newMenuErrorPopUpView>();
		errorView.errorPopUpVM.error = error;
		errorView.popUpVM.centralWindowStyle = new GUIStyle(ressources.popUpSkin.customStyles[3]);
		errorView.popUpVM.centralWindowTitleStyle = new GUIStyle (ressources.popUpSkin.customStyles [0]);
		errorView.popUpVM.centralWindowButtonStyle = new GUIStyle (ressources.popUpSkin.button);
		errorView.popUpVM.transparentStyle = new GUIStyle (ressources.popUpSkin.customStyles [2]);
		this.errorPopUpResize ();
	}
	public void displayPlayPopUp()
	{
		this.displayTransparentBackground ();
		this.playPopUp=Instantiate(this.ressources.playPopUpObject) as GameObject;
		this.playPopUp.transform.position = new Vector3 (0f, 0f, -2f);
		this.setCurrentPage (5);
		this.isPlayPopUpDisplayed = true;
	}
	public void displayTransparentBackground()
	{
		if(!this.isTransparentBackgroundDisplayed)
		{
			this.isTransparentBackgroundDisplayed = true;
			this.transparentBackground=Instantiate(this.ressources.transparentBackgroundObject) as GameObject;
			this.transparentBackground.transform.position = new Vector3 (0, 0, -1f);
		}
	}
	public void displayInvitationPopUp()
	{
		this.closeAllPopUp ();
		this.displayTransparentBackground ();
		this.invitationPopUp=Instantiate(this.ressources.invitationPopUpObject) as GameObject;
		this.invitationPopUp.transform.position = new Vector3 (0f, 0f, -2f);
		this.isInvitationPopUpDisplayed = true;
	}
	public void errorPopUpResize()
	{
		errorView.popUpVM.centralWindow = this.centralWindow;
		errorView.popUpVM.resize ();
	}
	public void hideErrorPopUp()
	{
		Destroy (this.errorView);
		this.errorViewDisplayed = false;
	}
	public void hideInvitationPopUp()
	{
		Destroy (this.invitationPopUp);
		this.hideTransparentBackground ();
		this.isInvitationPopUpDisplayed = false;
	}
	public void hidePlayPopUp()
	{
		Destroy (this.playPopUp);
		this.hideTransparentBackground ();
		this.isPlayPopUpDisplayed = false;
		if(Application.loadedLevelName=="newMyGame")
		{
			this.setCurrentPage (1);
		}
		else if(Application.loadedLevelName=="newStore")
		{
			this.setCurrentPage (2);
		}
		else if(Application.loadedLevelName=="newMarket")
		{
			this.setCurrentPage (3);
		}
		else if(Application.loadedLevelName=="newSkillBook")
		{
			this.setCurrentPage (4);
		}
		else
		{
			this.setCurrentPage (0);
		}
		Vector3 tempPosition;
		tempPosition= gameObject.transform.Find("Button"+5).localPosition;
		tempPosition.x = this.ressources.startButtonPosition;
		gameObject.transform.Find("Button"+5).localPosition=tempPosition;
	}
	public void hideTransparentBackground()
	{
		if(this.isTransparentBackgroundDisplayed)
		{
			this.isTransparentBackgroundDisplayed = false;
			Destroy (this.transparentBackground);
		}
	}
	public void setCurrentPage(int i)
	{
		this.currentPage = i;
		gameObject.transform.Find ("Button" + i).GetComponent<MenuButtonController> ().setIsSelected(true);
		gameObject.transform.Find ("Button" + i).FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.blueColor;
	}
	public void setNbNotificationsNonRead(int value)
	{
		ApplicationModel.nbNotificationsNonRead = value;
		this.refreshMenuObject ();
	}
	public IEnumerator getUserData()
	{
		yield return StartCoroutine (model.refreshUserData (this.ressources.totalNbResultLimit, this.isInviting));
		if(Application.loadedLevelName!="NewHomePage")
		{
			ApplicationModel.nbNotificationsNonRead = model.player.nonReadNotifications;
		}
		if(model.player.Money!=ApplicationModel.credits)
		{
			ApplicationModel.credits = model.player.Money;
			this.moneyUpdate();
		}
		this.refreshMenuObject ();
		if(model.isInvited && !this.isInvitationPopUpDisplayed && !this.isInviting && !this.isUserBusy)
		{
			if(this.isPlayPopUpDisplayed)
			{
				this.hidePlayPopUp();
			}
			this.displayInvitationPopUp();
		}
		if(this.isInviting && model.invitationError!="")
		{
			MenuController.instance.hideLoadingScreen ();
			photon.leaveRoom();
			this.isInviting=false;
			this.displayErrorPopUp(model.invitationError);
		}
	}
	public IEnumerator initialization()
	{
		this.displayLoadingScreen ();
		this.initializeMenuObject ();
		this.resize ();
		this.initializeScene ();
		yield return StartCoroutine (model.loadUserData (this.ressources.totalNbResultLimit));
		if(Application.loadedLevelName!="NewHomePage")
		{
			ApplicationModel.nbNotificationsNonRead = model.player.nonReadNotifications;
		}
		ApplicationModel.credits = model.player.Money;
		this.refreshMenuObject ();
	}
	public virtual void initializeScene()
	{
	}
	public void initializeMenuObject()
	{
		this.gameObject.transform.Find ("LogoBlock").FindChild ("AdminButton").gameObject.AddComponent<MenuAdminController> ();
		this.gameObject.transform.Find ("LogoBlock").FindChild ("DisconnectButton").gameObject.AddComponent<MenuDisconnectController> ();
		this.gameObject.transform.FindChild ("UserBlock").FindChild ("Username").gameObject.AddComponent<MenuUserUsernameController> ();
		this.gameObject.transform.FindChild ("UserBlock").FindChild ("Picture").gameObject.AddComponent<MenuUserPictureController> ();
		this.gameObject.transform.FindChild ("UserBlock").FindChild ("Bell").gameObject.AddComponent<MenuNotificationsController> ();
		this.gameObject.transform.FindChild ("UserBlock").FindChild ("Credits").gameObject.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;

		if(ApplicationModel.isAdmin)
		{
			this.gameObject.transform.Find("LogoBlock").FindChild("AdminButton").gameObject.SetActive(true);
			this.gameObject.transform.Find("LogoBlock").FindChild("AdminButton").transform.localPosition=new Vector3(-2.73f,-0.3f,0f);
			this.gameObject.transform.Find("LogoBlock").FindChild("DisconnectButton").transform.localPosition=new Vector3(-2.73f,0.3f,0f);
		}
		else
		{
			this.gameObject.transform.Find("LogoBlock").FindChild("AdminButton").gameObject.SetActive(false);
			this.gameObject.transform.Find("LogoBlock").FindChild("DisconnectButton").transform.localPosition=new Vector3(-2.73f,0f,0f);
		}
		
		for (int i=0;i<6;i++)
		{
			this.gameObject.transform.FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().text=model.buttonsLabels[i].ToUpper();
			this.gameObject.transform.FindChild("Button"+i).gameObject.AddComponent<MenuButtonController>();
			this.gameObject.transform.FindChild("Button"+i).GetComponent<MenuButtonController>().setId(i);
		}
		
		this.gameObject.transform.FindChild("UserBlock").FindChild("Username").GetComponent<TextMeshPro>().text=ApplicationModel.username;
		gameObject.transform.FindChild("UserBlock").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.returnThumbPicture (model.player.idProfilePicture);
	}
	public virtual void resizeAll()
	{
	}
	public void resize()
	{
		ApplicationDesignRules.computeDesignRules ();

		this.centralWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen,ApplicationDesignRules.widthScreen * 0.50f, 0.40f * ApplicationDesignRules.heightScreen);

		float buttonsBorderWidth = 1500f;
		float buttonsWorldScaleX = (ApplicationDesignRules.worldWidth-ApplicationDesignRules.leftMargin-ApplicationDesignRules.rightMargin)/(buttonsBorderWidth / ApplicationDesignRules.pixelPerUnit);
		gameObject.transform.FindChild ("ButtonsBorder").localScale = new Vector3 (buttonsWorldScaleX, 0.63f, 1f);

		float buttonsTotalSize=0f;
		float buttonsGap=0f;
		float textRatio=1f;
		float fontSize = 2.5f;
		for(int i=0;i<6;i++)
		{
			this.gameObject.transform.FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().fontSize=textRatio*fontSize;
			buttonsTotalSize=buttonsTotalSize+this.gameObject.transform.FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().bounds.size.x;
		}
		if((ApplicationDesignRules.worldWidth-1.5f-ApplicationDesignRules.rightMargin-ApplicationDesignRules.leftMargin)<buttonsTotalSize)
		{
			textRatio=(ApplicationDesignRules.worldWidth-1.5f-ApplicationDesignRules.rightMargin-ApplicationDesignRules.leftMargin)/buttonsTotalSize;
		}
		buttonsTotalSize = 0f;
		for(int i=0;i<6;i++)
		{
			this.gameObject.transform.FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().fontSize=textRatio*fontSize;
			buttonsTotalSize=buttonsTotalSize+this.gameObject.transform.FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().bounds.size.x;
		}
		buttonsGap = (ApplicationDesignRules.worldWidth - ApplicationDesignRules.leftMargin-ApplicationDesignRules.rightMargin- buttonsTotalSize) / 6f;

		for(int i=0;i<6;i++)
		{
			float previousButtonsTotalSize=0f;
			for(int j=0;j<i;j++)
			{
				previousButtonsTotalSize=previousButtonsTotalSize+this.gameObject.transform.FindChild("Button"+j).FindChild("Title").GetComponent<TextMeshPro>().bounds.size.x;
			}
			Vector3 buttonPosition=gameObject.transform.FindChild("Button"+i).transform.position;
			buttonPosition.x=buttonsGap/2f+(-ApplicationDesignRules.worldWidth / 2f)+ApplicationDesignRules.leftMargin+(i)*(buttonsGap)+this.gameObject.transform.FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().bounds.size.x/2f+previousButtonsTotalSize;
			gameObject.transform.FindChild("Button"+i).transform.position=buttonPosition;
		}	                                                                 

		float logoBlockWidth = 693f;
		float logoBlockScale = 0.7f;
		float logoBlockWorldWidth = (logoBlockWidth / ApplicationDesignRules.pixelPerUnit)*logoBlockScale;
		Vector3 logoBlockPosition = gameObject.transform.FindChild ("LogoBlock").transform.position;
		logoBlockPosition.x = (-ApplicationDesignRules.worldWidth / 2f) + ApplicationDesignRules.leftMargin + logoBlockWorldWidth / 2f;
		gameObject.transform.FindChild ("LogoBlock").transform.position = logoBlockPosition;

		float userBlockWidth = 766f;
		float userBlockScale = 0.7f;
		float userBlockWorldWidth = userBlockWidth / ApplicationDesignRules.pixelPerUnit*logoBlockScale;
		Vector3 userBlockPosition = gameObject.transform.FindChild ("UserBlock").transform.position;
		userBlockPosition.x = (ApplicationDesignRules.worldWidth / 2f) - ApplicationDesignRules.rightMargin - userBlockWorldWidth / 2f;
		gameObject.transform.FindChild ("UserBlock").transform.position = userBlockPosition;

	}
	public void refreshMenuObject()
	{
		this.gameObject.transform.FindChild("UserBlock").FindChild("Credits").GetComponent<TextMeshPro>().text=ApplicationModel.credits.ToString();
		if(ApplicationModel.nbNotificationsNonRead>0)
		{
			this.gameObject.transform.FindChild("UserBlock").FindChild("Bell").GetComponent<MenuNotificationsController>().reset();
			this.gameObject.transform.FindChild("UserBlock").FindChild("Notifications").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("UserBlock").FindChild("Notifications").GetComponent<TextMeshPro>().text=ApplicationModel.nbNotificationsNonRead.ToString();
		}
		else
		{
			this.gameObject.transform.FindChild("UserBlock").FindChild("Bell").GetComponent<MenuNotificationsController>().setIsActive(false);
			this.gameObject.transform.FindChild("UserBlock").FindChild("Bell").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.greySpriteColor;
			this.gameObject.transform.FindChild("UserBlock").FindChild("Notifications").gameObject.SetActive(false);
		}
	}
	public void changePage(int i)
	{
		switch(i)
		{
		case 0:
			this.homePageLink();
			break;
		case 1:
			this.myGameLink();
			break;
		case 2:
			this.storeLink();
			break;
		case 3:
			this.marketLink();
			break;
		case 4:
			this.skillBookLink();
			break;
		case 5:
			this.playLink();
			break;
		}
	}
	public void logOutLink() 
	{
		if(isDisconnectedViewDisplayed)
		{
			this.hideDisconnectedPopUp();
		}
		ApplicationModel.username = "";
		ApplicationModel.toDeconnect = true;
		PhotonNetwork.Disconnect();
		Application.LoadLevel("Authentication");
	}
	public void homePageLink()
	{
		Application.LoadLevel("NewHomePage");
	}
	public void profileLink() 
	{
		Application.LoadLevel("NewProfile");
	}
	public void myGameLink() 
	{
		if(this.isTutorialLaunched)
		{
			TutorialObjectController.instance.actionIsDone();
		}
		else
		{
			Application.LoadLevel("newMyGame");
		}
	}
	public void marketLink() 
	{
		Application.LoadLevel("newMarket");
	}
	public void skillBookLink() 
	{
		Application.LoadLevel("NewSkillBook");
	}
	public void storeLink() 
	{
		if(this.isTutorialLaunched)
		{
			TutorialObjectController.instance.actionIsDone();
		}
		else
		{
			Application.LoadLevel("newStore");
		}
	}
	public void playLink() 
	{
		this.displayPlayPopUp();
	}
	public void adminBoardLink() 
	{
		Application.LoadLevel("AdminBoard");
	}
	public void setButtonsGui(bool value)
	{
		//		for(int i=0;i<view.menuVM.buttonsEnabled.Length;i++)
		//		{
		//			view.menuVM.buttonsEnabled[i]=value;
		//		}
	}
	public void setButtonGui(int index, bool value)
	{
		//		view.menuVM.buttonsEnabled[index]=value;
	}
	public void setTutorialLaunched(bool value)
	{
		this.isTutorialLaunched = value;
		this.setIsUserBusy (value);
	}
	public bool getIsTutorialLaunched()
	{
		return isTutorialLaunched;
	}
	public void returnPressed()
	{
		if(errorViewDisplayed)
		{
			this.hideErrorPopUp();
		}
		else if(isDisconnectedViewDisplayed)
		{
			this.logOutLink();
		}
		else if(isInvitationPopUpDisplayed)
		{
			InvitationPopUpController.instance.acceptInvitationHandler();
		}
		else
		{
			this.sceneReturnPressed();
		}
	}
	public virtual void sceneReturnPressed()
	{
	}
	public void escapePressed()
	{
		if(errorViewDisplayed)
		{
			this.hideErrorPopUp();
		}
		else if(isPlayPopUpDisplayed)
		{
			if(!isLoadingScreenDisplayed)
			{
				this.hidePlayPopUp();
			}
		}
		else if(isDisconnectedViewDisplayed)
		{
			this.hideDisconnectedPopUp();
		}
		else if(isInvitationPopUpDisplayed)
		{
			InvitationPopUpController.instance.declineInvitationHandler();
		}
		else
		{
			this.sceneEscapePressed();
		}
	}
	public virtual void sceneEscapePressed()
	{
	}
	public void closeAllPopUp()
	{
		if(errorViewDisplayed)
		{
			this.hideErrorPopUp();
		}
		if(isPlayPopUpDisplayed)
		{
			if(!isLoadingScreenDisplayed)
			{
				this.hidePlayPopUp();
			}
		}
		if(isDisconnectedViewDisplayed)
		{
			this.hideDisconnectedPopUp();
		}
		this.sceneCloseAllPopUp ();
	}
	public virtual void sceneCloseAllPopUp()
	{
	}
	public Vector3 getButtonPosition(int id)
	{
		return gameObject.transform.FindChild ("Button" + id).position;
	}
	public void displayDisconnectedPopUp()
	{
		this.isDisconnectedViewDisplayed = true;
		this.disconnectedView = Camera.main.gameObject.AddComponent <NewMenuDisconnectedPopUpView>();
		disconnectedView.popUpVM.centralWindowStyle = new GUIStyle(this.ressources.popUpSkin.window);
		disconnectedView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.ressources.popUpSkin.customStyles [0]);
		disconnectedView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.ressources.popUpSkin.button);
		disconnectedView.popUpVM.transparentStyle = new GUIStyle (this.ressources.popUpSkin.customStyles [2]);
		this.disconnectedPopUpResize ();
	}
	public void disconnectedPopUpResize()
	{
		disconnectedView.popUpVM.centralWindow = this.centralWindow;
		disconnectedView.popUpVM.resize ();
	}
	public void hideDisconnectedPopUp()
	{
		Destroy (this.disconnectedView);
		this.isDisconnectedViewDisplayed = false;
	}
	
	public void displayLoadingScreen()
	{
		if(!isLoadingScreenDisplayed)
		{
			this.loadingScreen=Instantiate(this.ressources.loadingScreenObject) as GameObject;
			this.isLoadingScreenDisplayed=true;
		}
	}
	public void hideLoadingScreen()
	{
		if(isLoadingScreenDisplayed)
		{
			Destroy (this.loadingScreen);
			this.isLoadingScreenDisplayed=false;
		}
		if(this.isInviting)
		{
			this.isInviting=false;
		}
	}
	public void changeLoadingScreenLabel(string label)
	{
		if(isLoadingScreenDisplayed)
		{
			this.loadingScreen.GetComponent<LoadingScreenController> ().changeLoadingScreenLabel (label);
		}
	}
	public void displayLoadingScreenButton(bool value)
	{
		if(isLoadingScreenDisplayed)
		{
			this.loadingScreen.GetComponent<LoadingScreenController> ().displayButton (value);
		}
	}
	public IEnumerator sendInvitation(User invitedUser, User sendingUser)
	{
		Invitation invitation = new Invitation ();
		invitation.InvitedUser = invitedUser;
		invitation.SendingUser = sendingUser;
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (invitation.add ());
		MenuController.instance.changeLoadingScreenLabel("En attente de la réponse de votre ami ...");
		MenuController.instance.displayLoadingScreenButton (true);
		ApplicationModel.gameType = 2+invitation.Id;
		photon.CreateNewRoom();
		this.isInviting = true;
	}
	public void leaveRandomRoomHandler()
	{
		MenuController.instance.hideLoadingScreen ();
		photon.leaveRoom ();
		if(ApplicationModel.gameType>2)
		{
			Invitation invitation = new Invitation ();
			invitation.Id = ApplicationModel.gameType-2;
			StartCoroutine(invitation.changeStatus(-1));
		}
	}
	public void joinRandomRoomHandler()
	{
		this.displayLoadingScreen ();
		if(ApplicationModel.gameType<=2 && !this.isTutorialLaunched)
		{
			this.displayLoadingScreenButton (true);
			this.changeLoadingScreenLabel ("En attente de joueurs ...");
		}
		photon.joinRandomRoom ();
	}
	public void joinInvitationRoomFailed()
	{
		this.hideLoadingScreen ();
		this.displayErrorPopUp ("Votre ami a annulé le défi");
		Invitation invitation = new Invitation ();
		invitation.Id = ApplicationModel.gameType-2;
		StartCoroutine(invitation.changeStatus(-1));
	}
	public Sprite returnThumbPicture(int id)
	{
		return ressources.profilePictures [id];
	}
	public Sprite returnPackPicture(int id)
	{
		return ressources.packPictures [id];
	}
	public Sprite returnTabPicture(int id)
	{
		return ressources.tabsPictures [id];
	}
	public void changeThumbPicture(int id)
	{
		model.player.idProfilePicture = id;
		gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.returnThumbPicture (model.player.idProfilePicture);
	}
	public void setIsUserBusy(bool value)
	{
		this.isUserBusy = value;
	}
	public virtual void moneyUpdate()
	{
	}
}

