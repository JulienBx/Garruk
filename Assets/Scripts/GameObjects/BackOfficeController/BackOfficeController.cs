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
	private BackOfficePhotonController photon;
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
	private GameObject newCardTypePopUp;
	private bool isNewCardTypePopUpDisplayed;
	private GameObject playPopUp;
	private bool isPlayPopUpDisplayed;
	private GameObject invitationPopUp;
	private bool isInvitationPopUpDisplayed;
	private GameObject transparentBackground;
	private bool isTransparentBackgroundDisplayed;

	private bool isMenuLoaded;

	private float speed;
	private float timer;
	private float collectionPointsTimer;
	private float refreshInterval;

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
		if (timer > this.refreshInterval && this.isMenuLoaded) 
		{
			timer=timer-this.refreshInterval;
			StartCoroutine (this.getUserData());
		}
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
		this.displayLoadingScreen();
		this.timer=0f;
		this.speed=5f;
		this.refreshInterval=5f;
		this.disconnectedPopUp=this.gameObject.transform.FindChild("disconnectPopUp").gameObject;
		this.errorPopUp = this.gameObject.transform.FindChild ("errorPopUp").gameObject;
		this.collectionPointsPopUp = this.gameObject.transform.FindChild ("collectionPointsPopUp").gameObject;
		this.newCardTypePopUp = this.gameObject.transform.FindChild ("newCardTypePopUp").gameObject;
	}
	public void setIsMenuLoaded(bool value)
	{
		this.isMenuLoaded=value;
	}
	public void displayErrorPopUp(string error)
	{
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
	public void displayNewCardTypePopUp(string titleCardTypeUnlocked)
	{
		this.displayTransparentBackground ();
		this.newCardTypePopUp.transform.GetComponent<NewCardTypePopUpController> ().reset (titleCardTypeUnlocked);
		this.isNewCardTypePopUpDisplayed = true;
		this.newCardTypePopUp.SetActive (true);
		this.newCardTypePopUpResize();
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
		this.invitationPopUpResize ();
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
	private void newCardTypePopUpResize()
	{
		this.newCardTypePopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.newCardTypePopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1f/this.gameObject.transform.localScale.x);
		this.newCardTypePopUp.GetComponent<NewCardTypePopUpController> ().resize ();
	}
	public void hideCollectionPointsPopUp()
	{
		this.collectionPointsPopUp.SetActive (false);
		this.isCollectionPointsPopUpDisplayed = false;
	}
	public void hideNewCardTypePopUp()
	{
		this.newCardTypePopUp.SetActive (false);
		this.hideTransparentBackground();
		this.isNewCardTypePopUpDisplayed = false;
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
	public virtual void resizeAll()
	{
	}
	public void resize()
	{
		ApplicationDesignRules.computeDesignRules();

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
		if(this.isNewCardTypePopUpDisplayed)
		{
			this.newCardTypePopUpResize();
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
			this.hideErrorPopUp();
		}
		else if(isDisconnectedPopUpDisplayed)
		{
			this.toDisconnect();
		}
		else if(isNewCardTypePopUpDisplayed)
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
		else if(isNewCardTypePopUpDisplayed)
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
		if(isNewCardTypePopUpDisplayed)
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
			this.loadingScreen=Instantiate(this.ressources.loadingScreenObject) as GameObject;
			this.isLoadingScreenDisplayed=true;
			this.changeLoadingScreenLabel(WordingLoadingScreen.getReference(0));
		}
	}
	public void hideLoadingScreen()
	{
		if(isLoadingScreenDisplayed)
		{
			Destroy (this.loadingScreen);
			this.isLoadingScreenDisplayed=false;
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
	public void toDisconnect() 
	{
		if(isDisconnectedPopUpDisplayed)
		{
			this.hideDisconnectedPopUp();
		}
		ApplicationModel.player.ToDeconnect = true;
		PhotonNetwork.Disconnect();
		SceneManager.LoadScene("Authentication");
	}
	public IEnumerator sendInvitation(User invitedUser, User sendingUser)
	{
		Invitation invitation = new Invitation ();
		invitation.InvitedUser = invitedUser;
		invitation.SendingUser = sendingUser;
		this.displayLoadingScreen ();
		yield return StartCoroutine (invitation.add ());
		this.changeLoadingScreenLabel(WordingSocial.getReference(6));
		this.displayLoadingScreenButton (true);
		ApplicationModel.player.ChosenGameType = 2+invitation.Id;
		photon.CreateNewRoom();
		ApplicationModel.player.IsInviting = true;
	}
	public void leaveRandomRoomHandler()
	{
		this.hideLoadingScreen ();
		photon.leaveRoom ();
		if(ApplicationModel.player.ChosenGameType>2)
		{
			Invitation invitation = new Invitation ();
			invitation.Id = ApplicationModel.player.ChosenGameType-2;
			StartCoroutine(invitation.changeStatus(-1));
		}
	}
	public void joinRandomRoomHandler()
	{
		this.displayLoadingScreen ();
		ApplicationModel.player.ToLaunchGameTutorial=TutorialObjectController.instance.launchTutorialGame();
		if(ApplicationModel.player.ChosenGameType<=2 && !ApplicationModel.player.ToLaunchGameTutorial)
		{
			this.displayLoadingScreenButton (true);
			this.changeLoadingScreenLabel (WordingGameModes.getReference(7));
		}
		photon.joinRandomRoom ();
	}
	public void joinInvitationRoomFailed()
	{
		this.hideLoadingScreen ();
		this.displayErrorPopUp (WordingSocial.getReference(7));
		Invitation invitation = new Invitation ();
		invitation.Id = ApplicationModel.player.ChosenGameType-2;
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
	public void leaveGame()
	{
		this.displayDisconnectedPopUp ();
	}
	public virtual void clickOnBackOfficeBackground()
	{
	}
	public bool getCanSwipeAndScroll()
	{
		return !this.isTransparentBackgroundDisplayed;
	}
	public virtual void moneyUpdate()
	{
	}
	public IEnumerator getUserData()
	{
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
			if(Application.loadedLevelName=="NewHomePage")
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
		}
		if(ApplicationModel.player.IsInviting && ApplicationModel.player.Error!="")
		{
			this.hideLoadingScreen ();
			photon.leaveRoom();
			ApplicationModel.player.IsInviting=false;
			this.displayErrorPopUp(ApplicationModel.player.Error);
		}
	}
	#region TUTORIAL FUNCTIONS

	public bool getIsPlayPopUpDisplayed()
	{
		return this.isPlayPopUpDisplayed;
	}

	#endregion
}

