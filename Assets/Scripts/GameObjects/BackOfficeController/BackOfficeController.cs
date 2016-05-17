using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;

public class BackOfficeController : MonoBehaviour 
{
	public static BackOfficeController instance;
	public BackOfficePhotonController photon;
	private BackOfficeRessources ressources;

	private GameObject loadingScreen;
	private bool isLoadingScreenDisplayed;
	private GameObject disconnectedPopUp;
	private bool isDisconnectedPopUpDisplayed;
	private GameObject errorPopUp;
	private bool isErrorPopUpDisplayed;
	private GameObject collectionPointsPopUp;
	private bool isCollectionPointsPopUpDisplayed;
	private GameObject[] newSkillsPopUps;
	private bool areNewSkillsPopUpsDisplayed;
	private GameObject playPopUp;
	private bool isPlayPopUpDisplayed;
	private GameObject invitationPopUp;
	private bool isInvitationPopUpDisplayed;
	private GameObject transparentBackground;
	private bool isTransparentBackgroundDisplayed;
	private GameObject toolTip;
	private bool isToolTipDisplayed;
	private bool toDisplayToolTip;
	private bool toReDrawToolTip;
	private float toolTipTimer;

	private bool isMenuLoaded;
	private bool isHelpLoaded;
	private bool isSwiping;
	private bool isScrolling;
	private bool isRefreshing;

	private float speed;
	private float timer;
	private float collectionPointsTimer;
	private float refreshInterval;

	#if (UNITY_EDITOR)
	private float checkOnlineTimer;
	private float checkOnlineRefreshInterval;
	#endif

    private AsyncOperation async;

	public virtual void Update()
	{
		if(Input.GetKeyDown(KeyCode.Return)) 
		{
			this.returnPressed();
		}
		if(Input.GetKeyDown(KeyCode.Escape) && !ApplicationModel.player.IsBusy) 
		{
			this.escapePressed();
		}
		if(Screen.width != ApplicationDesignRules.widthScreen || Screen.height!=ApplicationDesignRules.heightScreen) 
		{
			this.resizeAll();
		}
		if(isCollectionPointsPopUpDisplayed)
		{
			collectionPointsTimer =collectionPointsTimer + speed * Time.deltaTime;
			if(collectionPointsTimer>15f)
			{
				collectionPointsTimer=0f;
				this.hideCollectionPointsPopUp();
				if(areNewSkillsPopUpsDisplayed)
				{
					this.hideNewSkillsPopUps();
				}
			}
		}
		timer = timer+speed*Time.deltaTime;
		if (!isRefreshing && timer > this.refreshInterval && this.isMenuLoaded) 
		{
			timer=timer-this.refreshInterval;
			StartCoroutine (this.getUserData());
		}
		if(this.toDisplayToolTip)
		{
			toolTipTimer=toolTipTimer+Time.deltaTime;
			if(toolTipTimer>0.1f && this.isScrolling)
			{
				this.hideToolTip();
			}
			else if(toolTipTimer>0.3f)
			{
				this.toolTip.SetActive(true);
				this.toDisplayToolTip=false;
				this.isToolTipDisplayed=true;
				if(ApplicationDesignRules.isMobileScreen)
				{
					this.drawToolTip();
				}
				else
				{
					this.toReDrawToolTip=true;
				}
			}
		}
		if(this.toReDrawToolTip)
		{
			this.drawToolTip();
		}

		#if (UNITY_EDITOR)
		if(ApplicationModel.player.IsAdmin)
		{
			checkOnlineTimer=checkOnlineTimer+Time.deltaTime;
			if(checkOnlineTimer>refreshInterval)
			{
				this.checkOnlineTimer=0;
				this.checkOnlineStatus();
			}
		}
		#endif

	}
	public virtual void initialize()
	{
		instance = this;
		if(!ApplicationDesignRules.initialized)
		{
			ApplicationDesignRules.computeDesignRules();
		}
		this.ressources = this.gameObject.GetComponent<BackOfficeRessources> ();
		this.photon = this.gameObject.GetComponent<BackOfficePhotonController> ();
		this.isMenuLoaded=false;
		this.timer=0f;
		this.speed=5f;
		#if (UNITY_EDITOR)
		this.checkOnlineTimer=0f;
		this.checkOnlineRefreshInterval=5f;
		#endif
		this.refreshInterval=5f;
		this.isRefreshing=false;
		this.toolTip=this.gameObject.transform.FindChild("toolTip").gameObject;
		this.disconnectedPopUp=this.gameObject.transform.FindChild("disconnectPopUp").gameObject;
		this.errorPopUp = this.gameObject.transform.FindChild ("errorPopUp").gameObject;
		this.collectionPointsPopUp = this.gameObject.transform.FindChild ("collectionPointsPopUp").gameObject;
		this.loadingScreen=this.gameObject.transform.FindChild("loadingScreen").gameObject;
		this.loadingScreen.transform.FindChild("background2").GetComponent<SpriteRenderer>().sprite=ressources.loadingScreenBackgrounds[this.getLoadingScreenBackground()];
		this.resize();
		this.displayLoadingScreen();
		this.gameObject.GetComponent<PhotonView>().viewID = 1 ;
	}
	public void setIsMenuLoaded(bool value)
	{
		this.isMenuLoaded=value;
	}
	public void setIsHelpLoaded(bool value)
	{
		this.isHelpLoaded=value;
	}
	public virtual int getLoadingScreenBackground()
	{
		return 0;
	}
	public void displayErrorPopUp(string error)
	{
		SoundController.instance.playSound(3);
		this.displayTransparentBackground ();
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
		this.playPopUpResize ();
	}
	public void displayCollectionPointsPopUp(int collectionPoints, int collectionPointsRanking)
	{
		if(this.isCollectionPointsPopUpDisplayed)
		{
			this.hideCollectionPointsPopUp();
		}
		this.collectionPointsPopUp.SetActive (true);
		this.isCollectionPointsPopUpDisplayed = true;
		this.collectionPointsPopUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingCollectionPointsPopUp.getReference(0) + collectionPoints.ToString () + WordingCollectionPointsPopUp.getReference(1) + collectionPointsRanking.ToString ();
		this.timer = 0f;
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
			this.newSkillsPopUps[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSkills.getName(newSkills[i].Id);
			this.newSkillsPopUps[i].transform.FindChild("Picto").GetComponent<SpriteRenderer>().sprite=returnSkillPicto(newSkills[i].getPictureId());
		}
		this.newSkillsPopUpsResize ();
	}
	public void displayTransparentBackground()
	{
		if(!this.isTransparentBackgroundDisplayed)
		{
			this.hideToolTip();
			this.isTransparentBackgroundDisplayed = true;
			this.transparentBackground=Instantiate(this.ressources.transparentBackgroundObject) as GameObject;
			this.transparentBackgroundResize();
			if(this.isHelpLoaded)
			{
				HelpController.instance.freeze();
			}
		}
	}
	public void displayInvitationPopUp()
	{
		SoundController.instance.playSound(3);
		this.closeAllPopUp ();
		this.displayTransparentBackground ();
		this.invitationPopUp=Instantiate(this.ressources.invitationPopUpObject) as GameObject;
		this.invitationPopUp.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.isInvitationPopUpDisplayed = true;
		this.invitationPopUpResize ();
	}
	public void displayToolTip(string titleLabel, string descriptionLabel)
	{
		if(!this.toDisplayToolTip && !this.isToolTipDisplayed)
		{
			this.toolTip.transform.FindChild("title").GetComponent<TextMeshPro>().text=titleLabel;
			this.toolTip.transform.FindChild("description").GetComponent<TextMeshPro>().text=descriptionLabel;
			this.toolTipTimer=0f;
			this.toDisplayToolTip=true;	
		}
	}
	private void drawToolTip()
	{
		Vector3 toolTipPosition = Camera.main.GetComponent<Camera>().ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		if(toolTipPosition.x+ApplicationDesignRules.toolTipWorldSize.x/2f-ApplicationDesignRules.menuPosition.x>ApplicationDesignRules.worldWidth/2f)
		{
			toolTipPosition.x=ApplicationDesignRules.menuPosition.x+ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.toolTipWorldSize.x/2f;
		}
		else if(toolTipPosition.x-ApplicationDesignRules.toolTipWorldSize.x/2f-ApplicationDesignRules.menuPosition.x<-ApplicationDesignRules.worldWidth/2f)
		{
			toolTipPosition.x=ApplicationDesignRules.menuPosition.x-ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.toolTipWorldSize.x/2f;
		}
		if(toolTipPosition.y-ApplicationDesignRules.menuPosition.y+ApplicationDesignRules.toolTipWorldSize.y/2f+0.1f>ApplicationDesignRules.worldHeight/2f)
		{
			toolTipPosition.y=ApplicationDesignRules.menuPosition.y+ApplicationDesignRules.worldHeight/2f-ApplicationDesignRules.toolTipWorldSize.y/2f;
		}
		else 
		{
			toolTipPosition.y=toolTipPosition.y+0.1f+ApplicationDesignRules.toolTipWorldSize.y/2f;
		}
		toolTipPosition.z=-2f;
		this.toolTip.transform.position=toolTipPosition;
	}
	public void errorPopUpResize()
	{
		this.errorPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.errorPopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1f/this.gameObject.transform.localScale.x);
		this.errorPopUp.GetComponent<ErrorPopUpController> ().resize ();
	}
	public void playPopUpResize()
	{
		this.playPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.playPopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1f/this.gameObject.transform.localScale.x);
	}
	public void invitationPopUpResize()
	{
		this.invitationPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.invitationPopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1f/this.gameObject.transform.localScale.x);
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
	public void hideToolTip()
	{
		this.toolTip.SetActive(false);
		this.isToolTipDisplayed=false;
		this.toDisplayToolTip=false;
		this.toReDrawToolTip=false;
		this.toolTipTimer=0f;
	}
	public void hideCollectionPointsPopUp()
	{
		this.collectionPointsPopUp.SetActive (false);
		this.isCollectionPointsPopUpDisplayed = false;
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
		this.hideTransparentBackground();
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
	}
	public void failPlayPopUp()
	{
		this.hidePlayPopUp();
		this.displayErrorPopUp(WordingDeck.getReference(13));
	}
	public void hideTransparentBackground()
	{
		if(this.isTransparentBackgroundDisplayed)
		{
			this.isTransparentBackgroundDisplayed = false;
			Destroy (this.transparentBackground);
			if(this.isHelpLoaded)
			{
				HelpController.instance.reload();
			}
		}
	}
	public virtual void resizeAll()
	{
	}
	public void resize()
	{
		ApplicationDesignRules.computeDesignRules();
		this.loadingScreen.GetComponent<LoadingScreenController>().resize();
		this.toolTip.transform.localScale=ApplicationDesignRules.toolTipScale;
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
		if(this.isPlayPopUpDisplayed)
		{
			this.playPopUpResize();
		}
		if(this.isInvitationPopUpDisplayed)
		{
			this.invitationPopUpResize();
		}
	}

	public void returnPressed()
	{
		if(isErrorPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideErrorPopUp();
		}
		else if(isDisconnectedPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.toDisconnect();
		}
		else if(isInvitationPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
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
		if(isToolTipDisplayed)
		{
			this.hideToolTip();
		}
		if(isErrorPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideErrorPopUp();
		}
		else if(isPlayPopUpDisplayed)
		{
			if(!isLoadingScreenDisplayed)
			{
				SoundController.instance.playSound(8);
				this.hidePlayPopUp();
			}
		}
		else if(isDisconnectedPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideDisconnectedPopUp();
		}
		else if(isInvitationPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
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
		SoundController.instance.playSound(9);
		this.displayTransparentBackground ();
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
		this.hideTransparentBackground();
		this.isDisconnectedPopUpDisplayed = false;
	}
	public void displayLoadingScreen()
	{
		if(!isLoadingScreenDisplayed)
		{
			this.hideToolTip();
			this.loadingScreen.SetActive(true);
			this.isLoadingScreenDisplayed=true;
			this.changeLoadingScreenLabel(WordingLoadingScreen.getReference(0));
			this.loadingScreen.transform.FindChild("button").GetComponent<LoadingScreenButtonController>().reset();
			if(this.isHelpLoaded)
			{
				HelpController.instance.freeze();
			}
		}
	}
	public void hideLoadingScreen()
	{
		if(isLoadingScreenDisplayed)
		{
			this.displayLoadingScreenButton (false);
			this.loadingScreen.SetActive(false);
			this.isLoadingScreenDisplayed=false;
			if(this.isHelpLoaded)
			{
				HelpController.instance.reload();
			}
		}
		if(ApplicationModel.player.IsInviting)
		{
			ApplicationModel.player.IsInviting=false;
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
	public void launchPreMatchLoadingScreen()
	{
		if(isLoadingScreenDisplayed)
		{
			this.loadingScreen.GetComponent<LoadingScreenController> ().launchPreMatchLoadingScreen();
		}
	}
	public void toDisconnect() 
	{
		SoundController.instance.stopPlayingMusic();
		if(isDisconnectedPopUpDisplayed)
		{
			this.hideDisconnectedPopUp();
		}
		if(PhotonNetwork.connectionState==ConnectionState.Disconnected)
		{
			if(SceneManager.GetActiveScene().name!="Authentication")
			{
				ApplicationModel.player.ToDeconnect = true;
				if(!ApplicationModel.player.ToDeconnect)
				{
					ApplicationModel.player.HasLostConnection=true;
				}
		       	this.loadScene("Authentication");
			}
		}
		else
		{
			ApplicationModel.player.ToDeconnect = true;
			PhotonNetwork.Disconnect();
		}
	}
	public IEnumerator sendInvitation(User invitedUser, User sendingUser)
	{
		Invitation invitation = new Invitation ();
		invitation.InvitedUser = invitedUser;
		invitation.SendingUser = sendingUser;
		this.displayLoadingScreen ();
		yield return StartCoroutine (invitation.add ());
		if(invitation.Error!="")
		{
			this.hideLoadingScreen();
			this.displayErrorPopUp(invitation.Error);
			invitation.Error="";
		}
		else
		{
			this.changeLoadingScreenLabel(WordingSocial.getReference(6));
			ApplicationModel.player.ChosenGameType = 20+invitation.Id;
            photon.joinRandomRoom();
			ApplicationModel.player.IsInviting = true;	
		}
	}

	public void leaveRandomRoomHandler()
	{
		this.hideLoadingScreen ();
		photon.leaveRoom ();

		if(ApplicationModel.player.ChosenGameType>20)
		{
			Invitation invitation = new Invitation ();
			invitation.Id = ApplicationModel.player.ChosenGameType-20;
			StartCoroutine(invitation.changeStatus(-1));
		}
	}
	public void joinRandomRoomHandler()
	{
		this.displayLoadingScreen ();
		//ApplicationModel.player.ToLaunchGameTutorial=TutorialObjectController.instance.launchTutorialGame();
		if(ApplicationModel.player.ChosenGameType<=20 && !ApplicationModel.player.ToLaunchGameTutorial)
		{
			this.changeLoadingScreenLabel (WordingGameModes.getReference(7));
		}
		photon.joinRandomRoom ();
	}
	public void joinInvitationRoomFailed()
	{
		this.hideLoadingScreen ();
		this.displayErrorPopUp (WordingSocial.getReference(7));
		Invitation invitation = new Invitation ();
		invitation.Id = ApplicationModel.player.ChosenGameType-20;
		StartCoroutine(invitation.changeStatus(-1));
	}
	public IEnumerator joinTutorialGame()
	{
		BackOfficeController.instance.displayLoadingScreen();
		yield return StartCoroutine(ApplicationModel.player.setTutorialStep (1));
		ApplicationModel.player.ToLaunchGameTutorial=true;
		ApplicationModel.player.ChosenGameType=-1;
		BackOfficeController.instance.joinRandomRoomHandler();
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
	public Sprite returnSkillPicto(int id)
	{
		return ressources.skillsPictos[id];
	}
	public Sprite returnCardTypePicto(int id)
	{
		return ressources.cardTypesPictos[id];
	}
	public Sprite returnSmallCardsCaracter(int id)
	{
		return ressources.smallCardsCaracters[id];
	}
	public Sprite returnLargeCardsCaracter(int id)
	{
		return ressources.largeCardsCaracters[id];
	}
	public Sprite returnCaracterAvatar(int id)
	{
		return ressources.caractersAvatars[id];
	}
	public Sprite returnPreMatchScreenAvatar(int id)
	{
		return ressources.preMatchScreenAvatars[id];
	}
	public void leaveGame()
	{
		this.displayDisconnectedPopUp ();
	}
	public virtual void clickOnBackOfficeBackground()
	{
	}
	public bool getCanSwipeAndScroll()
	{
		if(this.isTransparentBackgroundDisplayed || this.isToolTipDisplayed)
		{
			return false;
		}
		return true;
	}
	public virtual void moneyUpdate()
	{
	}
	public IEnumerator getUserData()
	{
		this.isRefreshing=true;
		int money=ApplicationModel.player.Money;
		int nbNotificationsNonReads=ApplicationModel.player.NbNotificationsNonRead;
		yield return StartCoroutine (ApplicationModel.player.refreshUserData ());
		if(money!=ApplicationModel.player.Money)
		{
			this.moneyUpdate();
			MenuController.instance.refreshMenuObject();
		}
		if(nbNotificationsNonReads!=ApplicationModel.player.NbNotificationsNonRead)
		{
			if(SceneManager.GetActiveScene().name=="NewHomePage")
			{
				MenuController.instance.setNbNotificationsNonRead(NewHomePageController.instance.getNonReadNotificationsOnCurrentPage()+ApplicationModel.player.NbNotificationsNonRead);
			}
			else
			{
				MenuController.instance.setNbNotificationsNonRead(ApplicationModel.player.NbNotificationsNonRead);
			}
			MenuController.instance.refreshMenuObject();
		}
		if(ApplicationModel.player.IsInvited && !this.isInvitationPopUpDisplayed && !ApplicationModel.player.IsInviting && !ApplicationModel.player.IsBusy)
		{
			if(this.isPlayPopUpDisplayed)
			{
				this.hidePlayPopUp();
			}
			this.displayInvitationPopUp();

			//Invitation invitation = new Invitation ();
			//invitation.Id = ApplicationModel.player.ChosenGameType-20;
			//StartCoroutine(invitation.changeStatus(-1));
		}
		if(ApplicationModel.player.IsInviting && ApplicationModel.player.Error!="")
		{
			this.hideLoadingScreen ();
			photon.leaveRoom();
			ApplicationModel.player.IsInviting=false;
			this.displayErrorPopUp(ApplicationModel.player.Error);
			ApplicationModel.player.Error="";
		}
		this.isRefreshing=false;
	}
	public bool getIsLoadingScreenDisplayed()
	{
		return this.isLoadingScreenDisplayed;
	}
	public void setIsSwiping(bool value)
	{
		this.isSwiping=value;
	}
	public bool getIsSwiping()
	{
		return this.isSwiping;
	}
	public void setIsScrolling(bool value)
	{	
		this.isScrolling=value;
	}
	public bool getIsScrolling()
	{
		return this.isScrolling;
	}
	public bool getIsToolTipDisplayed()
	{
		return isToolTipDisplayed;
	}
    public void loadScene(string sceneName)
    {
        StartCoroutine(this.preLoadScene(sceneName));
        this.displayLoadingScreen();
    }
    private IEnumerator preLoadScene(string sceneName) 
    {
    	if(photon.async==null)
    	{
			async = Application.LoadLevelAsync(sceneName);
         	async.allowSceneActivation = true;
         	yield return async;
    	}
    	else
    	{
    		SceneManager.LoadScene(sceneName);
    		yield break;
    	}
         
     }
	#region TUTORIAL FUNCTIONS

	public bool getIsPlayPopUpDisplayed()
	{
		return this.isPlayPopUpDisplayed;
	}

	#endregion

	#if (UNITY_EDITOR)
	private void checkOnlineStatus()
	{
		if(PhotonNetwork.insideLobby)
		{
			PhotonNetwork.FindFriends (ApplicationModel.onlineCheck);
		}
	}

	public void OnUpdatedFriendList()
	{
		for(int i=0;i<PhotonNetwork.Friends.Count;i++)
		{
			for(int j=0;j<ApplicationModel.onlineCheck.Length;j++)
			{
				if(ApplicationModel.onlineCheck[j]==PhotonNetwork.Friends[i].Name)
				{
					if(PhotonNetwork.Friends[i].IsInRoom)
					{
						if(ApplicationModel.onlineStatus[j]!=2)
						{
							ApplicationModel.onlineStatus[j]=2;
							SoundController.instance.playSound(11);
							print(ApplicationModel.onlineCheck[j] + " a rejoint un match");
						}
					}
					else if(PhotonNetwork.Friends[i].IsOnline)
					{
						if(ApplicationModel.onlineStatus[j]!=1)
						{
							ApplicationModel.onlineStatus[j]=1;
							SoundController.instance.playSound(11);
							print(ApplicationModel.onlineCheck[j] + " a rejoint le lobby");
						}
					}
					else if(!PhotonNetwork.Friends[i].IsOnline && !PhotonNetwork.Friends[i].IsInRoom)
					{
						if(ApplicationModel.onlineStatus[j]!=0)
						{
							ApplicationModel.onlineStatus[j]=0;
							SoundController.instance.playSound(11);
							print(ApplicationModel.onlineCheck[j] + " a quitté le jeu");
						}
					}
				}
				break;
			}
		}
	}

	#endif

}

