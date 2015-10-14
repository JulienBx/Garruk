using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class newMenuController : MonoBehaviour 
{
	public static newMenuController instance;
	private newMenuModel model;
	private newMenuRessources ressources;
	private newMenuPhotonController photon;
	private int currentPage;
	private int pageHovered;
	private float timer;
	private bool toMoveButtons;
	private bool toMoveBackButtons;
	private Vector3 currentButtonPosition;
	private GameObject playPopUp;
	private GameObject invitationPopUp;
	private GameObject transparentBackground;
	private bool isPlayPopUpDisplayed;
	private bool isInvitationPopUpDisplayed;
	private Rect centralWindow;
	private GameObject loadingScreen;

	private bool isDisconnectedViewDisplayed;
	private NewMenuDisconnectedPopUpView disconnectedView;

	private bool isLoadingScreenDisplayed;
	public bool isTutorialLaunched;

	private bool isInviting;

	private newMenuErrorPopUpView errorView;
	private bool errorViewDisplayed;

	void Awake()
	{
		instance = this;
		this.toMoveButtons = false;
		this.model = new newMenuModel ();
		this.ressources = this.gameObject.GetComponent<newMenuRessources> ();
		this.photon = this.gameObject.GetComponent<newMenuPhotonController> ();
	}
	public virtual void Start () 
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
		if(toMoveButtons)
		{
			this.toMoveButtons=false;
			for(int i=0;i<6;i++)
			{
				this.currentButtonPosition= gameObject.transform.Find("Button"+i).localPosition;
				if(i!=this.pageHovered && this.currentButtonPosition.x!=this.ressources.startButtonPosition)
				{
					this.currentButtonPosition.x-= this.ressources.speed * Time.deltaTime;
					if(this.currentButtonPosition.x<this.ressources.startButtonPosition)
					{
						this.currentButtonPosition.x=this.ressources.startButtonPosition;

					}
					else
					{
						this.toMoveButtons=true;
					}
					gameObject.transform.Find("Button"+i).localPosition=this.currentButtonPosition;
				}
				else if(i==this.pageHovered && this.currentButtonPosition.x!=this.ressources.endButtonPosition)
				{
					this.currentButtonPosition.x+= this.ressources.speed * Time.deltaTime;
					if(this.currentButtonPosition.x>this.ressources.endButtonPosition)
					{
						this.currentButtonPosition.x=this.ressources.endButtonPosition;
					}
					else
					{
						this.toMoveButtons=true;
					}
					gameObject.transform.Find("Button"+i).localPosition=this.currentButtonPosition;
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
		this.transparentBackground=Instantiate(this.ressources.transparentBackgroundObject) as GameObject;
		this.transparentBackground.transform.position = new Vector3 (0, 0, -1f);
		this.playPopUp=Instantiate(this.ressources.playPopUpObject) as GameObject;
		this.playPopUp.transform.position = new Vector3 (0f, 0f, -2f);
		this.setCurrentPage (5);
		this.isPlayPopUpDisplayed = true;
	}
	public void displayInvitationPopUp()
	{
		this.transparentBackground=Instantiate(this.ressources.transparentBackgroundObject) as GameObject;
		this.transparentBackground.transform.position = new Vector3 (0, 0, -1f);
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
		Destroy (this.transparentBackground);
		this.isInvitationPopUpDisplayed = false;
	}
	public void hidePlayPopUp()
	{
		Destroy (this.playPopUp);
		Destroy (this.transparentBackground);
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
	public void moveButton(int value)
	{
		this.toMoveButtons = true;
		this.pageHovered = value;
		if(this.pageHovered!=this.currentPage)
		{
			this.toMoveBackButtons=true;
		}
	}
	public void setCurrentPage(int i)
	{
		this.currentPage = i;
		Vector3 tempPosition;
		tempPosition= gameObject.transform.Find("Button"+i).localPosition;
		tempPosition.x = this.ressources.endButtonPosition;
		gameObject.transform.Find("Button"+i).localPosition=tempPosition;
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
		ApplicationModel.credits = model.player.Money;
		this.refreshMenuObject ();
		if(model.isInvited && !this.isInvitationPopUpDisplayed && !this.isInviting)
		{
			if(this.isPlayPopUpDisplayed)
			{
				this.hidePlayPopUp();
			}
			this.displayInvitationPopUp();
		}
		if(this.isInviting && model.invitationError!="")
		{
			newMenuController.instance.hideLoadingScreen ();
			photon.leaveRoom();
			this.isInviting=false;
			this.displayErrorPopUp(model.invitationError);
		}
	}
	public IEnumerator initialization()
	{
		yield return StartCoroutine (model.loadUserData (this.ressources.totalNbResultLimit));
		if(Application.loadedLevelName!="NewHomePage")
		{
			ApplicationModel.nbNotificationsNonRead = model.player.nonReadNotifications;
		}
		ApplicationModel.credits = model.player.Money;

		this.initializeMenuObject ();
		this.refreshMenuObject ();
		gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.returnThumbPicture (model.player.idProfilePicture);
	}
	public void initializeMenuObject()
	{
		if(ApplicationModel.isAdmin)
		{
			this.gameObject.transform.Find("Toolbar").FindChild("AdminButton").gameObject.SetActive(true);
		}
		
		for (int i=0;i<6;i++)
		{
			this.gameObject.transform.FindChild("Button"+i).FindChild("Text").GetComponent<TextMeshPro>().text=model.buttonsLabels[i];
		}
		
		this.gameObject.transform.FindChild("User").FindChild("Username").GetComponent<TextMeshPro>().text=ApplicationModel.username;
	}
	public void resizeMeunObject(float worldHeight, float worldWidth)
	{
		this.gameObject.transform.position =new Vector3(-worldWidth / 2f + 1.5625f, 0, 0);
		this.gameObject.transform.Find ("Toolbar").position = new Vector3 (worldWidth / 2f - 0.3665f - 0.2f * (worldHeight / worldWidth), worldHeight / 2f - 0.2f - 0.1805f, 0f);
		this.centralWindow = new Rect (Screen.width * 0.25f, 0.12f * Screen.height,Screen.width * 0.50f, 0.40f * Screen.height);
	}
	public void refreshMenuObject()
	{
		this.gameObject.transform.FindChild("User").FindChild("Credits").GetComponent<TextMeshPro>().text=ApplicationModel.credits.ToString();
		if(ApplicationModel.nbNotificationsNonRead>0)
		{
			this.gameObject.transform.FindChild("Notifications").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("Notifications").FindChild("Text").GetComponent<TextMeshPro>().text=ApplicationModel.nbNotificationsNonRead.ToString();
		}
		else
		{
			this.gameObject.transform.FindChild("Notifications").gameObject.SetActive(false);
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
	}
	public bool getIsTutorialLaunched()
	{
		return isTutorialLaunched;
	}
	public bool isAPopUpDisplayed()
	{
		if(isPlayPopUpDisplayed)
		{
			return true;
		}
		if(isDisconnectedViewDisplayed)
		{
			return true;
		}
		if(isInvitationPopUpDisplayed)
		{
			return true;
		}
		if(errorViewDisplayed)
		{
			return true;
		}
		return false;
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
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (invitation.add ());
		newMenuController.instance.changeLoadingScreenLabel("En attente de la réponse de votre ami ...");
		newMenuController.instance.displayLoadingScreenButton (true);
		ApplicationModel.gameType = 2+invitation.Id;
		photon.CreateNewRoom();
		this.isInviting = true;
	}
	public void leaveRandomRoomHandler()
	{
		newMenuController.instance.hideLoadingScreen ();
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
}

