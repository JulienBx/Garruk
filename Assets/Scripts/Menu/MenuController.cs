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
	private Rect collectionPointsWindow;
	private Rect newSkillsWindow;
	private Rect newCardTypeWindow;
	private GameObject loadingScreen;
	private GameObject tutorial;
	
	private bool isLoadingScreenDisplayed;
	
	private bool isInviting;
	
	private GameObject disconnectedPopUp;
	private bool isDisconnectedPopUpDisplayed;
	private GameObject errorPopUp;
	private bool isErrorPopUpDisplayed;
	private GameObject collectionPointsPopUp;
	private bool isCollectionPointsPopUpDisplayed;
	private GameObject[] newSkillsPopUps;
	private bool areNewSkillsPopUpsDisplayed;
	private NewCardTypePopUpView newCardTypeView;
	private bool isNewCardTypeViewDisplayed;

	private float speed;
	private float timerCollectionPoints;
	
	private bool isUserBusy;

	private float helpSpeed;
	private float helpTimer;
	private bool isHelpFlashing;
	
	void Awake()
	{
		instance = this;
		this.model = new MenuModel ();
		this.ressources = this.gameObject.GetComponent<MenuRessources> ();
		this.photon = this.gameObject.GetComponent<MenuPhotonController> ();
		this.speed = 5f;
		this.helpSpeed = 1f;
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
		if(isHelpFlashing)
		{
			this.helpTimer += Time.deltaTime;
			if(this.helpTimer>0.5f)
			{
				this.helpTimer=0f;
				gameObject.transform.FindChild("UserBlock").FindChild("Help").GetComponent<MenuHelpController>().changeColor();
			}
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
		if(isCollectionPointsPopUpDisplayed)
		{
			timerCollectionPoints = timerCollectionPoints + speed * Time.deltaTime;
			if(timerCollectionPoints>15f)
			{
				timerCollectionPoints=0f;
				this.hideCollectionPointsPopUp();
				if(areNewSkillsPopUpsDisplayed)
				{
					this.hideNewSkillsPopUps();
				}
			}
		}
	}
	public void displayErrorPopUp(string error)
	{
		MenuController.instance.displayTransparentBackground ();
		this.errorPopUp.transform.GetComponent<ErrorPopUpController> ().reset (error);
		this.isErrorPopUpDisplayed = true;
		this.errorPopUp.SetActive (true);
		this.errorPopUpResize();
	}
	public void displayPlayPopUp()
	{
		this.displayTransparentBackground ();
		this.playPopUp=Instantiate(this.ressources.playPopUpObject) as GameObject;
		this.playPopUp.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.isPlayPopUpDisplayed = true;
	}
	public void displayCollectionPointsPopUp(int collectionPoints, int collectionPointsRanking)
	{
		if(this.isCollectionPointsPopUpDisplayed)
		{
			this.hideCollectionPointsPopUp();
		}
		this.collectionPointsPopUp.SetActive (true);
		this.isCollectionPointsPopUpDisplayed = true;
		this.collectionPointsPopUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Collections Points : + " + collectionPoints.ToString () + "\nClassement : " + collectionPointsRanking.ToString ();
		this.timerCollectionPoints = 0f;
		this.collectionPointsPopUpResize ();
	}
	public void displayNewSkillsPopUps(IList<Skill> newSkills)
	{
		if(this.areNewSkillsPopUpsDisplayed)
		{
			this.hideNewSkillsPopUps();
		}
		this.areNewSkillsPopUpsDisplayed = true;
		this.newSkillsPopUps=new GameObject[newSkills.Count];
		for(int i=0;i<newSkills.Count;i++)
		{
			this.newSkillsPopUps[i]=Instantiate(this.ressources.newSkillPopUpObject) as GameObject;
			this.newSkillsPopUps[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=newSkills[i].Name;
			this.newSkillsPopUps[i].transform.FindChild("Picto").GetComponent<SpriteRenderer>().sprite=returnSkillPicture(newSkills[i].IdPicture);
		}
		this.newSkillsPopUpsResize ();
	}
	public void displayNewCardTypePopUp(string titleCardTypeUnlocked)
	{
		newCardTypeView = gameObject.AddComponent<NewCardTypePopUpView>();
		this.isNewCardTypeViewDisplayed = true;
		newCardTypeView.popUpVM.centralWindow = this.newCardTypeWindow;
		newCardTypeView.cardNewCardTypePopUpVM.newCardType = titleCardTypeUnlocked;
		newCardTypeView.popUpVM.centralWindowStyle = new GUIStyle(ressources.popUpSkin.window);
		newCardTypeView.popUpVM.centralWindowTitleStyle = new GUIStyle (ressources.popUpSkin.customStyles [0]);
		newCardTypeView.popUpVM.centralWindowButtonStyle = new GUIStyle (ressources.popUpSkin.button);
		this.newCardTypePopUpResize ();
	}
	public void displayTransparentBackground()
	{
		if(!this.isTransparentBackgroundDisplayed)
		{
			this.isTransparentBackgroundDisplayed = true;
			this.transparentBackground=Instantiate(this.ressources.transparentBackgroundObject) as GameObject;
			this.transparentBackgroundResize();
		}
	}
	public void displayInvitationPopUp()
	{
		this.closeAllPopUp ();
		this.displayTransparentBackground ();
		this.invitationPopUp=Instantiate(this.ressources.invitationPopUpObject) as GameObject;
		this.invitationPopUp.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.isInvitationPopUpDisplayed = true;
	}
	public void errorPopUpResize()
	{
		this.errorPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.errorPopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1f/this.gameObject.transform.localScale.x);
		this.errorPopUp.GetComponent<ErrorPopUpController> ().resize ();
	}
	public void transparentBackgroundResize()
	{
		this.transparentBackground.transform.localScale=ApplicationDesignRules.transparentBackgroundScale;
		this.transparentBackground.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -1f);
	}
	private void collectionPointsPopUpResize()
	{
		this.collectionPointsPopUp.transform.position = ApplicationDesignRules.collectionPopUpPosition;
		this.collectionPointsPopUp.transform.localScale = ApplicationDesignRules.collectionPopUpScale*(1f/this.gameObject.transform.localScale.x);
	}
	private void newSkillsPopUpsResize()
	{
		Vector3 newSkillPopUpPosition=ApplicationDesignRules.collectionPopUpPosition;
		for(int i=0;i<this.newSkillsPopUps.Length;i++)
		{
			this.newSkillsPopUps[i].transform.localScale=ApplicationDesignRules.newSkillsPopUpScale;
			newSkillPopUpPosition.y=ApplicationDesignRules.collectionPopUpPosition.y-ApplicationDesignRules.collectionPopUpWorldSize.y/2f-0.025f-ApplicationDesignRules.newSkillsPopUpWorldSize.y/2f-i*(ApplicationDesignRules.newSkillsPopUpWorldSize.y+0.025f);
			this.newSkillsPopUps[i].transform.position=newSkillPopUpPosition;
		}
	}
	private void newCardTypePopUpResize()
	{
		newCardTypeView.popUpVM.centralWindow = this.newCardTypeWindow;
		newCardTypeView.popUpVM.resize ();
	}
	public void hideCollectionPointsPopUp()
	{
		this.collectionPointsPopUp.SetActive (false);
		this.isCollectionPointsPopUpDisplayed = false;
	}
	public void hideNewCardTypePopUp()
	{
		Destroy (this.newCardTypeView);
		this.isNewCardTypeViewDisplayed = false;
	}
	public void hideNewSkillsPopUps()
	{
		for(int i=0;i<this.newSkillsPopUps.Length;i++)
		{
			Destroy (this.newSkillsPopUps[i]);
		}
		this.areNewSkillsPopUpsDisplayed = false;
	}
	public void hideErrorPopUp()
	{
		this.errorPopUp.SetActive (false);
		MenuController.instance.hideTransparentBackground();
		this.isErrorPopUpDisplayed = false;
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
		TutorialObjectController.instance.tutorialTrackPoint ();
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
		if(!ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.Find ("Button" + i).GetComponent<MenuButtonController> ().setIsSelected(true);
			gameObject.transform.Find ("Button" + i).GetComponent<MenuButtonController> ().setHoveredState();
		}
		else
		{
			gameObject.transform.FindChild("BottomBar").FindChild ("Button" + i).GetComponent<MenuButtonController> ().setIsSelected(true);
			gameObject.transform.FindChild("BottomBar").FindChild ("Button" + i).GetComponent<MenuButtonController> ().setHoveredState();
		}
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
		this.resize ();
		this.displayLoadingScreen ();
		this.initializeMenuObject ();
		this.initializeScene ();
		yield return StartCoroutine (model.loadUserData (this.ressources.totalNbResultLimit));
		gameObject.transform.FindChild("UserBlock").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.returnThumbPicture (model.player.idProfilePicture);
		if(Application.loadedLevelName!="NewHomePage")
		{
			ApplicationModel.nbNotificationsNonRead = model.player.nonReadNotifications;
		}
		this.refreshMenuObject ();
	}
	public virtual void initializeScene()
	{
	}
	public void initializeMenuObject()
	{
		this.tutorial = GameObject.Find("Tutorial");
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
			this.gameObject.transform.FindChild("BottomBar").FindChild("Button"+i).FindChild("Title").GetComponent<TextMeshPro>().text=model.buttonsLabels[i];
			this.gameObject.transform.FindChild("Button"+i).gameObject.AddComponent<MenuButtonController>();
			this.gameObject.transform.FindChild("BottomBar").FindChild("Button"+i).gameObject.AddComponent<MenuButtonController>();
			this.gameObject.transform.FindChild("Button"+i).GetComponent<MenuButtonController>().setId(i);
			this.gameObject.transform.FindChild("BottomBar").FindChild("Button"+i).GetComponent<MenuButtonController>().setId(i);
		}
		this.gameObject.transform.FindChild("UserBlock").FindChild("Username").GetComponent<TextMeshPro>().text=ApplicationModel.username;
		this.disconnectedPopUp=this.gameObject.transform.FindChild("disconnectPopUp").gameObject;
		this.errorPopUp = this.gameObject.transform.FindChild ("errorPopUp").gameObject;
		this.collectionPointsPopUp = this.gameObject.transform.FindChild ("collectionPointsPopUp").gameObject;
	}
	public virtual void resizeAll()
	{
	}
	public void resize()
	{
		ApplicationDesignRules.computeDesignRules ();

		gameObject.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y + ApplicationDesignRules.worldHeight / 2f - 5f, 0f);

		this.centralWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen,ApplicationDesignRules.widthScreen * 0.50f, 0.40f * ApplicationDesignRules.heightScreen);
		this.collectionPointsWindow=new Rect(ApplicationDesignRules.widthScreen - ApplicationDesignRules.widthScreen * 0.17f-5,0.1f * ApplicationDesignRules.heightScreen+5,ApplicationDesignRules.widthScreen * 0.17f,ApplicationDesignRules.heightScreen * 0.1f);
		this.newSkillsWindow = new Rect (this.collectionPointsWindow.xMin, this.collectionPointsWindow.yMax + 5,this.collectionPointsWindow.width,ApplicationDesignRules.heightScreen - 0.1f * ApplicationDesignRules.heightScreen - 2 * 5 - this.collectionPointsWindow.height);
		this.newCardTypeWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen, ApplicationDesignRules.widthScreen * 0.50f, 0.25f * ApplicationDesignRules.heightScreen);


		if(!ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.FindChild("LogoBlock").GetComponent<SpriteRenderer>().sprite=ressources.logoBackground;
			gameObject.transform.FindChild("UserBlock").GetComponent<SpriteRenderer>().sprite=ressources.userBackground;
			gameObject.transform.FindChild("TopBar").gameObject.SetActive(false);
			gameObject.transform.FindChild("BottomBar").gameObject.SetActive(false);

			float buttonsBorderWidth = 1500f;
			float buttonsWorldScaleX = (ApplicationDesignRules.worldWidth-ApplicationDesignRules.leftMargin-ApplicationDesignRules.rightMargin)/(buttonsBorderWidth / ApplicationDesignRules.pixelPerUnit);
			gameObject.transform.FindChild ("ButtonsBorder").localScale = new Vector3 (buttonsWorldScaleX, 0.63f, 1f);
			
			float buttonsTotalSize=0f;
			float buttonsGap=0f;
			float textRatio=1f;
			float fontSize = 2.5f;
			gameObject.transform.FindChild("ButtonsBorder").gameObject.SetActive(true);
			for(int i=0;i<6;i++)
			{
				this.gameObject.transform.FindChild("Button"+i).gameObject.SetActive(true);
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


		}
		else
		{
			gameObject.transform.FindChild("LogoBlock").GetComponent<SpriteRenderer>().sprite=null;
			gameObject.transform.FindChild("UserBlock").GetComponent<SpriteRenderer>().sprite=null;
			gameObject.transform.FindChild("TopBar").gameObject.SetActive(true);
			gameObject.transform.FindChild("TopBar").position=new Vector3(ApplicationDesignRules.menuPosition.x,ApplicationDesignRules.menuPosition.y+ApplicationDesignRules.worldHeight/2f-ApplicationDesignRules.topBarWorldSize.y/2f+0.05f,0f);
			gameObject.transform.FindChild("BottomBar").gameObject.SetActive(true);
			gameObject.transform.FindChild("BottomBar").position=new Vector3(ApplicationDesignRules.menuPosition.x,ApplicationDesignRules.menuPosition.y-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.bottomBarWorldSize.y/2f-0.05f,0f);
			gameObject.transform.FindChild("ButtonsBorder").gameObject.SetActive(false);
			for(int i=0;i<6;i++)
			{
				this.gameObject.transform.FindChild("Button"+i).gameObject.SetActive(false);
			}
		}

		float logoBlockWidth = 693f;
		float logoBlockScale = 0.7f;
		float logoBlockWorldWidth = (logoBlockWidth / ApplicationDesignRules.pixelPerUnit)*logoBlockScale;
		Vector3 logoBlockPosition = gameObject.transform.FindChild ("LogoBlock").transform.position;
		logoBlockPosition.x = (-ApplicationDesignRules.worldWidth / 2f) + ApplicationDesignRules.leftMargin + logoBlockWorldWidth / 2f;
		if(ApplicationDesignRules.isMobileScreen)
		{
			logoBlockPosition.y = gameObject.transform.FindChild("TopBar").position.y+0.05f;
		}
		gameObject.transform.FindChild ("LogoBlock").transform.position = logoBlockPosition;
		
		float userBlockWidth = 766f;
		float userBlockScale = 0.7f;
		float userBlockWorldWidth = userBlockWidth / ApplicationDesignRules.pixelPerUnit*logoBlockScale;
		Vector3 userBlockPosition = gameObject.transform.FindChild ("UserBlock").transform.position;
		userBlockPosition.x = (ApplicationDesignRules.worldWidth / 2f) - ApplicationDesignRules.rightMargin - userBlockWorldWidth / 2f;
		if(ApplicationDesignRules.isMobileScreen)
		{
			userBlockPosition.y = gameObject.transform.FindChild("TopBar").position.y+0.05f;
		}
		gameObject.transform.FindChild ("UserBlock").transform.position = userBlockPosition;

		if(this.isCollectionPointsPopUpDisplayed)
		{
			this.collectionPointsPopUpResize();
		}
		if(this.areNewSkillsPopUpsDisplayed)
		{
			this.newSkillsPopUpsResize();
		}
		if(this.isTransparentBackgroundDisplayed)
		{
			this.transparentBackgroundResize();
		}
		if(this.isDisconnectedPopUpDisplayed)
		{
			this.disconnectedPopUpResize();
		}
		if(this.isErrorPopUpDisplayed)
		{
			this.errorPopUpResize();
		}
	}
	public void refreshMenuObject()
	{
		this.gameObject.transform.FindChild("UserBlock").FindChild("Credits").GetComponent<TextMeshPro>().text=ApplicationModel.credits.ToString();
		if(ApplicationModel.nbNotificationsNonRead>0)
		{
			this.gameObject.transform.FindChild("UserBlock").FindChild("Bell").GetComponent<MenuNotificationsController>().reset();
			this.gameObject.transform.FindChild("UserBlock").FindChild("Notifications").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("UserBlock").FindChild("Bell").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
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
		if(isDisconnectedPopUpDisplayed)
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
		Application.LoadLevel("newMyGame");
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
		Application.LoadLevel("newStore");
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
	public void returnPressed()
	{
		if(isErrorPopUpDisplayed)
		{
			this.hideErrorPopUp();
		}
		else if(isDisconnectedPopUpDisplayed)
		{
			this.logOutLink();
		}
		else if(isNewCardTypeViewDisplayed)
		{
			this.hideNewCardTypePopUp();
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
		if(isErrorPopUpDisplayed)
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
		else if(isDisconnectedPopUpDisplayed)
		{
			this.hideDisconnectedPopUp();
		}
		else if(isNewCardTypeViewDisplayed)
		{
			this.hideNewCardTypePopUp();
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
		if(isErrorPopUpDisplayed)
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
		if(isNewCardTypeViewDisplayed)
		{
			this.hideNewCardTypePopUp();
		}
		if(isDisconnectedPopUpDisplayed)
		{
			this.hideDisconnectedPopUp();
		}
		this.sceneCloseAllPopUp ();
	}
	public virtual void sceneCloseAllPopUp()
	{
	}
	public void displayDisconnectedPopUp()
	{
		MenuController.instance.displayTransparentBackground ();
		this.disconnectedPopUp.transform.GetComponent<DisconnectPopUpController> ().reset ();
		this.isDisconnectedPopUpDisplayed = true;
		this.disconnectedPopUp.SetActive (true);
		this.disconnectedPopUpResize();
	}
	public void disconnectedPopUpResize()
	{
		this.disconnectedPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.disconnectedPopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1f/this.gameObject.transform.localScale.x);
		this.disconnectedPopUp.GetComponent<DisconnectPopUpController> ().resize ();
	}
	public void hideDisconnectedPopUp()
	{
		this.disconnectedPopUp.SetActive (false);
		MenuController.instance.hideTransparentBackground();
		this.isDisconnectedPopUpDisplayed = false;
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
		ApplicationModel.launchGameTutorial=TutorialObjectController.instance.launchTutorialGame();
		if(ApplicationModel.gameType<=2 && !ApplicationModel.launchGameTutorial)
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
	public Sprite returnLargeProfilePicture(int id)
	{
		return ressources.largeProfilePictures [id];
	}
	public Sprite returnCompetitionPicture(int id)
	{
		return ressources.competitionsPictures [id];
	}
	public Sprite returnLargeCompetitionPicture(int id)
	{
		return ressources.largeCompetitionsPictures [id];
	}
	public Sprite returnSkillTypePicture(int id)
	{
		return ressources.skillsTypePictures [id];
	}
	public Sprite returnCardTypePicture(int id)
	{
		return ressources.cardsTypePictures [id];
	}
	public Sprite returnSkillPicture(int id)
	{
		return ressources.skillsPictures [id];
	}
	public void changeThumbPicture(int id)
	{
		model.player.idProfilePicture = id;
		gameObject.transform.FindChild("UserBlock").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.returnThumbPicture (model.player.idProfilePicture);
	}
	public void setIsUserBusy(bool value)
	{
		this.isUserBusy = value;
	}
	public virtual void moneyUpdate()
	{
	}
	public void leaveGame()
	{
		this.displayDisconnectedPopUp ();
	}
	public virtual void clickOnBackOfficeBackground()
	{
	}
	#region TUTORIAL FUNCTIONS

	public void helpHandler()
	{
		TutorialObjectController.instance.helpClicked ();
	}
	public void setFlashingHelp(bool value)
	{
		this.isHelpFlashing=value;
		if(!value)
		{
			gameObject.transform.FindChild("UserBlock").FindChild("Help").GetComponent<MenuHelpController>().reset();
		}
	}
	public Vector3 returnButtonPosition(int id)
	{
		return gameObject.transform.FindChild("Button"+id).transform.position;
	}
	public bool getIsPlayPopUpDisplayed()
	{
		return this.isPlayPopUpDisplayed;
	}
	public Vector3 getButtonPosition(int id)
	{
		Vector3 buttonPosition = gameObject.transform.FindChild ("Button" + id).position;
		buttonPosition.x = buttonPosition.x - ApplicationDesignRules.menuPosition.x;
		buttonPosition.y = buttonPosition.y - ApplicationDesignRules.menuPosition.y;
		return buttonPosition;
	}
	public Vector3 getHelpButtonPosition()
	{
		Vector3 helpButtonPosition = gameObject.transform.FindChild ("UserBlock").FindChild ("Help").position;
		helpButtonPosition.x = helpButtonPosition.x - ApplicationDesignRules.menuPosition.x;
		helpButtonPosition.y = helpButtonPosition.y - ApplicationDesignRules.menuPosition.y;
		return helpButtonPosition;
	}
	#endregion
}

