using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class NewProfileController : MonoBehaviour
{
	public static NewProfileController instance;
	private NewProfileModel model;
	
	public GameObject blockObject;
	public GameObject friendsContentObject;
	public GameObject challengeButtonObject;
	public GameObject friendsStatusButtonObject;
	public GameObject resultObject;
	public Sprite[] languageSprites;

	private GameObject backOfficeController;
	private GameObject menu;
	private GameObject tutorial;

	private GameObject profileBlock;
	private GameObject profileBlockTitle;
	private GameObject cleanCardsButton;
	private GameObject friendshipStatus;
	private GameObject[] friendshipStatusButtons;
	private GameObject profilePicture;
	private GameObject profileDivisionIcon;
	private GameObject profileEditPictureButton;
	private GameObject profileEditInformationsButton;
	private GameObject profileEditPasswordButton;
	private GameObject profileChooseLanguageButton;
	private GameObject[] profileInformations;
	private GameObject profileLine;
	private GameObject[] profileStats;

	private GameObject searchBlock;
	private GameObject searchBlockTitle;
	private GameObject searchSubtitle;
	private GameObject searchBar;
	private GameObject searchButton;
	private GameObject searchError;

	private GameObject resultsBlock;
	private GameObject resultsBlockTitle;
	private GameObject[] resultsContents;
	private GameObject[] resultsTabs;
	private GameObject resultsPaginationButtons;

	private GameObject friendsBlock;
	private GameObject friendsBlockTitle;
	private GameObject[] friendsContents;
	private GameObject[] friendsTabs;
	private GameObject[] friendsStatusButtons;
	private GameObject[] challengesButtons;
	private GameObject friendsPaginationButtons;
	
	private GameObject selectPicturePopUp;
	private GameObject searchUsersPopUp;

	private GameObject mainCamera;
	private GameObject sceneCamera;
	private GameObject tutorialCamera;
	private GameObject backgroundCamera;

	private GameObject slideLeftButton;
	private GameObject slideRightButton;
	private GameObject friendsButton;
	private GameObject resultsButton;

	private bool isMyProfile;
	private string profileChosen;
	
	private IList<int> friendsRequestsDisplayed;
	private IList<int> challengesRecordsDisplayed;
	private IList<int> trophiesDisplayed;
	private IList<int> friendsDisplayed;
	private IList<int> friendsToBeDisplayed;
	private IList<int> confrontationsDisplayed;
	
	private IList<int> friendsOnline;

	private int activeResultsTab;
	private int activeFriendsTab;
	private Pagination resultsPagination;
	private Pagination friendsPagination;

	private bool isSceneLoaded;
	private bool isProfilePictureHovered;

	private bool isSelectPicturePopUpDisplayed;
	private bool isSearchUsersPopUpDisplayed;

	public int friendsRefreshInterval;
	private float friendsCheckTimer;

	private GameObject checkPasswordPopUp;
	private bool isCheckPasswordPopUpDisplayed;

	private GameObject changePasswordPopUp;
	private bool isChangePasswordPopUpDisplayed;

	private GameObject editInformationsPopUp;
	private bool isEditInformationsPopUpDisplayed;

	private GameObject chooseLanguagePopUp;
	private bool isChooseLanguagePopUpDisplayed;

	private GameObject messagePopUp;
	private bool isMessagePopUpDisplayed;

	private string searchValue;
	private bool isScrolling;

	private bool mainContentDisplayed;
	private bool resultsSliderDisplayed;
	private bool friendsSliderDisplayed;
	
	private bool toSlideLeft;
	private bool toSlideRight;
	
	private float mainContentPositionX;
	private float resultsPositionX;
	private float friendsPositionX;
	private float targetContentPositionX;

	void Update()
	{	
		this.friendsCheckTimer += Time.deltaTime;
		
		if (Input.touchCount == 1 && this.isSceneLoaded  && TutorialObjectController.instance.getCanSwipe() && BackOfficeController.instance.getCanSwipeAndScroll()) 
		{
			if(Input.touches[0].deltaPosition.x<-15f)
			{
				if(this.friendsSliderDisplayed || this.mainContentDisplayed || this.toSlideLeft)
				{
					this.slideRight();
					BackOfficeController.instance.setIsSwiping(true);
				}
			}
			if(Input.touches[0].deltaPosition.x>15f)
			{
				if(this.mainContentDisplayed || this.resultsSliderDisplayed || this.toSlideRight)
				{
					this.slideLeft();
					BackOfficeController.instance.setIsSwiping(true);
				}
			}
		}
		if(toSlideRight || toSlideLeft)
		{
			Vector3 sceneCameraPosition = this.sceneCamera.transform.position;
			float camerasXPosition = sceneCameraPosition.x;
			if(toSlideRight)
			{
				camerasXPosition=camerasXPosition+Time.deltaTime*40f;
				if(camerasXPosition>this.targetContentPositionX)
				{
					camerasXPosition=this.targetContentPositionX;
					this.toSlideRight=false;
					if(camerasXPosition==this.resultsPositionX)
					{
						this.resultsSliderDisplayed=true;
					}
					else if(camerasXPosition==this.mainContentPositionX)
					{
						this.mainContentDisplayed=true;
					}
					BackOfficeController.instance.setIsSwiping(false);
				}
			}
			else if(toSlideLeft)
			{
				camerasXPosition=camerasXPosition-Time.deltaTime*40f;
				if(camerasXPosition<this.targetContentPositionX)
				{
					camerasXPosition=this.targetContentPositionX;
					this.toSlideLeft=false;
					if(camerasXPosition==this.friendsPositionX)
					{
						this.friendsSliderDisplayed=true;
					}
					else if(camerasXPosition==this.mainContentPositionX)
					{
						this.mainContentDisplayed=true;
					}
					BackOfficeController.instance.setIsSwiping(false);
				}
			}
			sceneCameraPosition.x=camerasXPosition;
			this.sceneCamera.transform.position=sceneCameraPosition;
		}
		if (this.isMyProfile && friendsCheckTimer > friendsRefreshInterval && this.isSceneLoaded) 
		{
			this.friendsCheckTimer=0;
			this.checkFriendsOnlineStatus();
		}
	}
	void Awake()
	{
		instance = this;
		this.model = new NewProfileModel ();
		if(ApplicationModel.player.ProfileChosen==""|| ApplicationModel.player.ProfileChosen==ApplicationModel.player.Username)
		{
			this.isMyProfile=true;
			this.profileChosen=ApplicationModel.player.Username;
		}
		else
		{
			this.isMyProfile=false;
			this.profileChosen=ApplicationModel.player.ProfileChosen;
			ApplicationModel.player.ProfileChosen="";
		}
		this.activeResultsTab = 0;
		this.activeFriendsTab = 0;
		this.searchValue = "";
		this.friendsOnline = new List<int> ();
		this.initializeScene ();
		this.mainContentDisplayed = true;
		this.initializeBackOffice();
		this.initializeMenu();
		this.initializeTutorial();
		StartCoroutine (this.initialization ());
	}
	private void initializeTutorial()
	{
		this.tutorial = GameObject.Find ("Tutorial");
		this.tutorial.AddComponent<ProfileTutorialController>();
		this.tutorial.GetComponent<ProfileTutorialController>().initialize();
		BackOfficeController.instance.setIsTutorialLoaded(true);
	}
	private void initializeMenu()
	{
		this.menu = GameObject.Find ("Menu");
		this.menu.AddComponent<MenuController>();
		this.menu.GetComponent<MenuController>().initialize();
		BackOfficeController.instance.setIsMenuLoaded(true);
	}
	private void initializeBackOffice()
	{
		this.backOfficeController = GameObject.Find ("BackOfficeController");
		this.backOfficeController.AddComponent<BackOfficeProfileController>();
		this.backOfficeController.GetComponent<BackOfficeProfileController>().initialize();
	}
	public IEnumerator initialization()
	{
		this.resize ();
		yield return StartCoroutine (model.getData (this.isMyProfile, this.profileChosen));
		this.selectAFriendsTab ();
		this.selectAResultsTab ();
		this.initializeProfile ();
		if(!this.isMyProfile)
		{
			this.initializeFriendshipState();
		}
		this.checkFriendsOnlineStatus ();
		BackOfficeController.instance.hideLoadingScreen ();
		this.isSceneLoaded = true;
		if(ApplicationModel.player.TutorialStep!=-1)
		{
			TutorialObjectController.instance.startTutorial();
		}
		else if(ApplicationModel.player.DisplayTutorial&&!ApplicationModel.player.ProfileTutorial)
		{
			TutorialObjectController.instance.startHelp();
		}
	}
	private void initializeFriendsRequests()
	{
		this.friendsPagination.chosenPage = 0;
		this.friendsPagination.totalElements= model.friendsRequests.Count;
		this.friendsPaginationButtons.GetComponent<NewProfileFriendsPaginationController> ().p = this.friendsPagination;
		this.friendsPaginationButtons.GetComponent<NewProfileFriendsPaginationController> ().setPagination ();
		for(int i=0;i<this.friendsContents.Length;i++)
		{
			this.challengesButtons[i].SetActive(false);
			this.friendsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
		this.drawFriendsRequests();
	}
	private void initializeFriends()
	{
		this.sortFriendsList ();
		this.friendsPagination.chosenPage = 0;
		this.friendsPagination.totalElements= this.friendsToBeDisplayed.Count;
		this.friendsPaginationButtons.GetComponent<NewProfileFriendsPaginationController> ().p = this.friendsPagination;
		this.friendsPaginationButtons.GetComponent<NewProfileFriendsPaginationController> ().setPagination ();
		for(int i=0;i<this.friendsContents.Length;i++)
		{
			this.friendsStatusButtons[i*2].SetActive(false);
			this.friendsStatusButtons[i*2+1].SetActive(false);
		}
		this.drawFriends();
	}
	private void initializeChallengesRecords()
	{
		this.resultsPagination.chosenPage = 0;
		this.resultsPagination.totalElements= model.challengesRecords.Count;
		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().p = this.resultsPagination;
		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().setPagination ();
		for(int i=0;i<this.resultsContents.Length;i++)
		{
			this.resultsContents[i].transform.FindChild("title").GetComponent<NewProfileResultsContentUsernameController>().setIsActive(true);
			this.resultsContents[i].transform.FindChild("picture").GetComponent<NewProfileResultsContentPictureController>().setIsActive(true);
			this.resultsContents[i].transform.FindChild("divisionIcon").gameObject.SetActive(true);
			this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
		this.drawChallengesRecords ();
	}
	private void initializeTrophies()
	{
		this.resultsPagination.chosenPage = 0;
		this.resultsPagination.totalElements= model.trophies.Count;
		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().p = this.resultsPagination;
		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().setPagination ();
		for(int i=0;i<this.resultsContents.Length;i++)
		{
			this.resultsContents[i].transform.FindChild("title").GetComponent<NewProfileResultsContentUsernameController>().setIsActive(false);
			this.resultsContents[i].transform.FindChild("picture").GetComponent<NewProfileResultsContentPictureController>().setIsActive(false);
			this.resultsContents[i].transform.FindChild("divisionIcon").gameObject.SetActive(false);
			this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
		this.drawTrophies ();
	}
	private void initializeConfrontations()
	{
		this.resultsPagination.chosenPage = 0;
		this.resultsPagination.totalElements= model.confrontations.Count;
		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().p = this.resultsPagination;
		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().setPagination ();
		for(int i=0;i<this.resultsContents.Length;i++)
		{
			this.resultsContents[i].transform.FindChild("title").GetComponent<NewProfileResultsContentUsernameController>().setIsActive(false);
			this.resultsContents[i].transform.FindChild("picture").GetComponent<NewProfileResultsContentPictureController>().setIsActive(false);
			this.resultsContents[i].transform.FindChild("divisionIcon").gameObject.SetActive(false);
		}
		this.drawConfrontations ();
	}
	private void initializeProfile()
	{
		this.drawProfile ();
	}
	private void initializeFriendshipState()
	{
		this.drawFriendshipState ();
	}
	private void selectAResultsTab()
	{
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.hideResultsActiveTab();
		}
		for(int i=0;i<this.resultsTabs.Length;i++)
		{
			if(i==this.activeResultsTab)
			{
				this.resultsTabs[i].GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnTabPicture(1);
				this.resultsTabs[i].GetComponent<NewProfileResultsTabController>().setIsSelected(true);
				this.resultsTabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			}
			else
			{
				this.resultsTabs[i].GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnTabPicture(0);
				this.resultsTabs[i].GetComponent<NewProfileResultsTabController>().reset();
			}
		}
		this.initializeResultsTabContent ();
	}
	public void initializeResultsTabContent()
	{
		switch(this.activeResultsTab)
		{
		case 0:
			this.initializeTrophies();
			break;
		case 1:
			if(this.isMyProfile)
			{
				this.initializeChallengesRecords();
			}
			else
			{
				this.initializeConfrontations();
			}
			break;
		}
	}
	public void selectAResultsTabHandler(int idTab)
	{
		SoundController.instance.playSound(9);
		this.activeResultsTab = idTab;
		this.selectAResultsTab ();
	}
	public void paginationResultsHandler()
	{
		switch(this.activeResultsTab)
		{
		case 0:
			this.drawTrophies();
			break;
		case 1:
			if(this.isMyProfile)
			{
				this.drawChallengesRecords();
			}
			else
			{
				this.drawConfrontations();
			}
			break;
		}
	}
	private void selectAFriendsTab()
	{
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.hideFriendsActiveTab();
		}
		for(int i=0;i<this.friendsTabs.Length;i++)
		{
			if(i==this.activeFriendsTab)
			{
				this.friendsTabs[i].GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnTabPicture(1);
				this.friendsTabs[i].GetComponent<NewProfileFriendsTabController>().setIsSelected(true);
				this.friendsTabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			}
			else
			{
				this.friendsTabs[i].GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnTabPicture(0);
				this.friendsTabs[i].GetComponent<NewProfileFriendsTabController>().reset();
			}
		}
		this.initializeFriendsTabContent ();
	}
	public void initializeFriendsTabContent()
	{
		switch(this.activeFriendsTab)
		{
		case 0:
			this.initializeFriends();
			break;
		case 1:
			this.initializeFriendsRequests();
			break;
		}
	}
	public void selectAFriendsTabHandler(int idTab)
	{
		SoundController.instance.playSound(9);
		this.activeFriendsTab = idTab;
		this.selectAFriendsTab ();
	}
	public void paginationFriendsHandler()
	{
		switch(this.activeFriendsTab)
		{
		case 0:
			this.drawFriends();
			break;
		case 1:
			this.drawFriendsRequests();
			break;
		}
	}
	public void initializeScene()
	{
		this.profileBlock = Instantiate(this.blockObject) as GameObject;
		this.profileBlockTitle = GameObject.Find ("ProfileTitle");
		this.profileBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.profilePicture = GameObject.Find ("ProfilePicture");
		this.profilePicture.AddComponent<NewProfilePictureButtonController> ();
		this.profileDivisionIcon = GameObject.Find("ProfileDivisionIcon");
		this.profileEditPictureButton = GameObject.Find ("EditProfilePicture");
		this.profileEditPictureButton.AddComponent<NewProfileEditPictureButtonController> ();
		this.profileEditPictureButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingProfile.getReference(0);
		this.profileEditPictureButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.profileEditPictureButton.SetActive (false);
		this.cleanCardsButton = GameObject.Find ("CleanCardsButton");
		this.cleanCardsButton.AddComponent<NewProfileCleanCardsButtonController> ();
		this.cleanCardsButton.GetComponent<TextMeshPro> ().text = WordingProfile.getReference(1);
		if(isMyProfile&&ApplicationModel.player.IsAdmin)
		{
			this.cleanCardsButton.SetActive(true);
		}
		else
		{
			this.cleanCardsButton.SetActive(false);
		}
		this.friendshipStatus = GameObject.Find ("FriendshipStatus");
		this.friendshipStatus.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.friendshipStatus.SetActive (!this.isMyProfile);
		this.friendshipStatusButtons = new GameObject[2];
		for(int i=0;i<this.friendshipStatusButtons.Length;i++)
		{
			this.friendshipStatusButtons[i] = GameObject.Find ("FriendshipStatusButton"+i);
			this.friendshipStatusButtons[i].AddComponent<NewProfileFriendshipStatusButtonController> ();
			this.friendshipStatusButtons[i].GetComponent<NewProfileFriendshipStatusButtonController>().setId(i);
			this.friendshipStatusButtons[i].SetActive(!this.isMyProfile);
		}
		this.profileInformations=new GameObject[3];
		for(int i=0;i<this.profileInformations.Length;i++)
		{
			this.profileInformations[i]=GameObject.Find ("ProfileInformation"+i);
			this.profileInformations[i].GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.profileInformations[i].SetActive(this.isMyProfile);
		}
		this.profileEditInformationsButton = GameObject.Find ("ProfileEditInformationsButton");
		this.profileEditInformationsButton.AddComponent<NewProfileEditInformationsButtonController> ();
		this.profileEditInformationsButton.SetActive (this.isMyProfile);
		this.profileEditPasswordButton = GameObject.Find ("ProfileEditPasswordButton");
		this.profileEditPasswordButton.AddComponent<NewProfileEditPasswordButtonController> ();
		this.profileEditPasswordButton.SetActive (this.isMyProfile);
		this.profileChooseLanguageButton = GameObject.Find("ProfileChooseLanguageButton");
		this.profileChooseLanguageButton.AddComponent<NewProfileChooseLanguageButtonController>();
		this.profileChooseLanguageButton.GetComponent<SpriteRenderer>().sprite=this.languageSprites[ApplicationModel.player.IdLanguage];
		this.profileChooseLanguageButton.SetActive(this.isMyProfile);
		this.profileStats = new GameObject[4];
		for(int i=0;i<this.profileStats.Length;i++)
		{
			this.profileStats[i]=GameObject.Find ("ProfileStat"+i);
			this.profileStats[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			// A compléter !
		}
		this.profileStats[0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= WordingProfile.getReference(2);
		this.profileStats[0].transform.FindChild ("Subvalue").gameObject.SetActive (false);
		this.profileStats[1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= WordingProfile.getReference(3);
		this.profileStats[1].transform.FindChild ("Subvalue").gameObject.SetActive (false);
		this.profileStats[2].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= WordingProfile.getReference(4);
		this.profileStats[3].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= WordingProfile.getReference(5);
		this.profileLine = GameObject.Find ("ProfileLine");

		this.searchBlock = Instantiate(this.blockObject)as GameObject;
		this.searchBlockTitle = GameObject.Find ("SearchTitle");
		this.searchBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.searchBlockTitle.GetComponent<TextMeshPro>().text=WordingProfile.getReference(6);
		this.searchSubtitle=GameObject.Find ("SearchSubtitle");
		this.searchSubtitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.searchSubtitle.GetComponent<TextMeshPro>().text=WordingProfile.getReference(7).ToUpper();
		this.searchBar=GameObject.Find ("SearchBar");
		this.searchBar.GetComponent<NewProfileSearchBarController>().setButtonText(WordingProfile.getReference(8));
		this.searchButton = GameObject.Find ("SearchButton");
		this.searchButton.AddComponent<NewProfileSearchButtonController> ();
		this.searchButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingProfile.getReference(9);
		this.searchError = GameObject.Find("SearchError");
		this.searchError.GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;

		this.resultsBlock = Instantiate (this.blockObject) as GameObject;
		this.resultsBlockTitle = GameObject.Find ("ResultsBlockTitle");
		this.resultsBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.resultsTabs=new GameObject[2];
		for(int i=0;i<this.resultsTabs.Length;i++)
		{
			this.resultsTabs[i]=GameObject.Find ("ResultsTab"+i);
			this.resultsTabs[i].AddComponent<NewProfileResultsTabController>();
			this.resultsTabs[i].GetComponent<NewProfileResultsTabController>().setId(i);
		}
		this.resultsTabs[0].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = WordingProfile.getReference(10);
		if(this.isMyProfile)
		{
			this.resultsTabs[1].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = WordingProfile.getReference(11);
		}
		else
		{
			this.resultsTabs[1].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = WordingProfile.getReference(12);
		}
		this.resultsPaginationButtons = GameObject.Find("ResultsPagination");
		this.resultsPaginationButtons.AddComponent<NewProfileResultsPaginationController> ();
		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().initialize ();
		this.resultsContents = new GameObject[0];
		this.friendsBlock = Instantiate (this.blockObject) as GameObject;
		this.friendsBlockTitle = GameObject.Find ("FriendsBlockTitle");
		this.friendsBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.friendsTabs=new GameObject[2];
		for(int i=0;i<this.friendsTabs.Length;i++)
		{
			this.friendsTabs[i]=GameObject.Find("FriendsTab"+i);
			this.friendsTabs[i].AddComponent<NewProfileFriendsTabController>();
			this.friendsTabs[i].GetComponent<NewProfileFriendsTabController>().setId (i);
		}
		this.friendsTabs[0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingProfile.getReference(13);
		this.friendsTabs[1].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingProfile.getReference(14);
		this.friendsPaginationButtons = GameObject.Find("FriendsPagination");
		this.friendsPaginationButtons.AddComponent<NewProfileFriendsPaginationController> ();
		this.friendsPaginationButtons.GetComponent<NewProfileFriendsPaginationController> ().initialize ();
		this.friendsStatusButtons=new GameObject[0];
		this.challengesButtons=new GameObject[0];
		this.friendsContents=new GameObject[0];
		this.searchUsersPopUp = GameObject.Find ("searchPopUp");
		this.searchUsersPopUp.GetComponent<SearchUsersPopUpController> ().launch ();
		this.searchUsersPopUp.SetActive (false);
		this.selectPicturePopUp = GameObject.Find ("profilePicturesPopUp");
		this.selectPicturePopUp.SetActive (false);
		this.checkPasswordPopUp = GameObject.Find ("checkPasswordPopUp");
		this.checkPasswordPopUp.SetActive (false);
		this.changePasswordPopUp = GameObject.Find ("changePasswordPopUp");
		this.changePasswordPopUp.SetActive (false);
		this.editInformationsPopUp = GameObject.Find ("editInformationsPopUp");
		this.editInformationsPopUp.SetActive (false);
		this.chooseLanguagePopUp = GameObject.Find("chooseLanguagePopUp");
		this.chooseLanguagePopUp.SetActive(false);
		this.messagePopUp=GameObject.Find("profileMessagePopUp");
		this.messagePopUp.SetActive(false);
		this.mainCamera = gameObject;
		this.sceneCamera = GameObject.Find ("sceneCamera");
		this.tutorialCamera = GameObject.Find ("TutorialCamera");
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
		this.slideLeftButton = GameObject.Find ("SlideLeftButton");
		this.slideLeftButton.AddComponent<NewProfileSlideLeftButtonController> ();
		this.slideRightButton = GameObject.Find ("SlideRightButton");
		this.slideRightButton.AddComponent<NewProfileSlideRightButtonController> ();
		this.friendsButton = GameObject.Find ("FriendsButton");
		this.friendsButton.AddComponent<NewProfileFriendsButtonController> ();
		this.resultsButton = GameObject.Find ("ResultsButton");
		this.resultsButton.AddComponent<NewProfileResultsButtonController> ();
	}
	public void resize()
	{
		float profileBlockLeftMargin;
		float profileBlockUpMargin;
		float profileBlockHeight;
		float profileStatFirstLine;
		
		float searchBlockLeftMargin;
		float searchBlockUpMargin;
		float searchBlockHeight;
		float searchElementsLine;
		
		float friendsBlockLeftMargin;
		float friendsBlockUpMargin;
		float friendsBlockHeight;
		float friendsHeight;
		float friendsFirstLineY;
		
		float resultsBlockLeftMargin;
		float resultsBlockUpMargin;
		float resultsBlockHeight;
		float resultsHeight;
		float resultsFirstLineY;

		this.mainCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.mainCamera.transform.position = ApplicationDesignRules.mainCameraPosition;
		this.sceneCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraStandardPosition;
		this.tutorialCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.tutorialCamera.transform.position = ApplicationDesignRules.helpCameraPositiion;
		this.backgroundCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.backgroundCameraSize;
		this.backgroundCamera.transform.position = ApplicationDesignRules.backgroundCameraPosition;
		this.backgroundCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		this.tutorialCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		this.sceneCamera.GetComponent<Camera> ().rect = new Rect (0f,0f,1f,1f);
		this.mainCamera.GetComponent<Camera>().rect= new Rect (0f,0f,1f,1f);

		this.friendsPagination = new Pagination ();
		this.friendsPagination.chosenPage = 0;

		this.resultsPagination = new Pagination ();
		this.resultsPagination.chosenPage = 0;
		
		if(ApplicationDesignRules.isMobileScreen)
		{
			profileBlockHeight=5f;
			profileBlockLeftMargin=ApplicationDesignRules.leftMargin;
			profileBlockUpMargin=0f;
			profileStatFirstLine = 4f;

			searchBlockHeight=3f;
			searchBlockLeftMargin=ApplicationDesignRules.leftMargin;
			searchBlockUpMargin=profileBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+profileBlockHeight;
			searchElementsLine = 0.4f;

			if(this.isMyProfile)
			{
				friendsBlockHeight=ApplicationDesignRules.viewHeight-ApplicationDesignRules.tabWorldSize.y;
				friendsBlockUpMargin=ApplicationDesignRules.tabWorldSize.y;
			}
			else
			{
				friendsBlockHeight=ApplicationDesignRules.viewHeight;
				friendsBlockUpMargin=0f;
			}
			friendsBlockLeftMargin=-ApplicationDesignRules.worldWidth;
			friendsHeight=1f;
			friendsFirstLineY=1f;
			
			resultsBlockHeight=ApplicationDesignRules.viewHeight-ApplicationDesignRules.tabWorldSize.y;
			resultsBlockLeftMargin=ApplicationDesignRules.worldWidth+ApplicationDesignRules.leftMargin;
			resultsBlockUpMargin=ApplicationDesignRules.tabWorldSize.y;
			resultsHeight=1f;
			resultsFirstLineY=1f;

			this.slideLeftButton.SetActive(true);
			this.slideRightButton.SetActive(true);
			this.friendsButton.SetActive(true);
			this.resultsButton.SetActive(true);
			this.friendsBlockTitle.SetActive(true);
			this.resultsBlockTitle.SetActive(true);

			this.friendsPagination.nbElementsPerPage = 6;
			this.resultsPagination.nbElementsPerPage = 6;
		}
		else
		{
			profileBlockHeight=ApplicationDesignRules.mediumBlockHeight;
			profileBlockLeftMargin=ApplicationDesignRules.leftMargin;
			profileBlockUpMargin=ApplicationDesignRules.upMargin;
			profileStatFirstLine = 3.25f;
			
			searchBlockHeight=ApplicationDesignRules.smallBlockHeight;
			searchBlockLeftMargin=ApplicationDesignRules.leftMargin;
			searchBlockUpMargin=profileBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+profileBlockHeight;
			searchElementsLine = 0.7f;
			
			friendsBlockHeight=ApplicationDesignRules.mediumBlockHeight-ApplicationDesignRules.tabWorldSize.y;
			friendsBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			friendsBlockUpMargin=ApplicationDesignRules.upMargin+ApplicationDesignRules.tabWorldSize.y;
			friendsHeight=0.9f;
			friendsFirstLineY=0.3f;
			
			resultsBlockHeight=ApplicationDesignRules.smallBlockHeight-ApplicationDesignRules.tabWorldSize.y;
			resultsBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			resultsBlockUpMargin=friendsBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+friendsBlockHeight+ApplicationDesignRules.tabWorldSize.y;
			resultsHeight=0.9f;
			resultsFirstLineY=0.3f;

			this.slideLeftButton.SetActive(false);
			this.slideRightButton.SetActive(false);
			this.friendsButton.SetActive(false);
			this.resultsButton.SetActive(false);
			this.friendsBlockTitle.SetActive(false);
			this.resultsBlockTitle.SetActive(false);

			this.friendsPagination.nbElementsPerPage = 3;
			this.resultsPagination.nbElementsPerPage = 2;
		}

		this.profileBlock.GetComponent<NewBlockController> ().resize(profileBlockLeftMargin,profileBlockUpMargin,ApplicationDesignRules.blockWidth,profileBlockHeight);
		Vector3 profileBlockUpperLeftPosition = this.profileBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 profileBlockUpperRightPosition = this.profileBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 profileBlockLowerLeftPosition = this.profileBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 profileBlockLowerRightPosition = this.profileBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 profileBlockSize = this.profileBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 profileBlockOrigin = this.profileBlock.GetComponent<NewBlockController> ().getOriginPosition ();

		this.profilePicture.transform.position = new Vector3 (profileBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing + ApplicationDesignRules.profilePictureWorldSize.x / 2f, profileBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing - ApplicationDesignRules.profilePictureWorldSize.y / 2f, 0f);
		this.profilePicture.transform.localScale = ApplicationDesignRules.profilePictureScale;

		this.profileDivisionIcon.transform.position=this.profilePicture.transform.position+new Vector3(0.62f,-0.62f,0f);
		this.profileDivisionIcon.transform.localScale=new Vector3(0.3f,0.3f,0.3f);

		this.profileBlockTitle.transform.position = new Vector3 (profileBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing+ApplicationDesignRules.profilePictureWorldSize.x+0.3f, profileBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.profileBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		
		this.profileEditPictureButton.transform.position = new Vector3(this.profilePicture.transform.position.x,this.profilePicture.transform.position.y,-1f);
		this.profileEditPictureButton.transform.localScale = this.profilePicture.transform.localScale;

		this.profileEditInformationsButton.transform.localScale = ApplicationDesignRules.button62Scale;
		this.profileEditPasswordButton.transform.localScale = ApplicationDesignRules.button62Scale;

		this.friendshipStatus.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.friendshipStatus.transform.localPosition= new Vector3 (profileBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing + ApplicationDesignRules.profilePictureWorldSize.x + 0.3f, profileBlockUpperLeftPosition.y - 1.2f, 0f);
		
		for(int i=0;i<this.friendshipStatusButtons.Length;i++)
		{
			this.friendshipStatusButtons[i].transform.position = new Vector3 (profileBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing + ApplicationDesignRules.profilePictureWorldSize.x+0.3f+ApplicationDesignRules.button61WorldSize.x/2f+i*(ApplicationDesignRules.button61WorldSize.x+0.05f), profileBlockUpperLeftPosition.y - 0.2f - ApplicationDesignRules.profilePictureWorldSize.y + ApplicationDesignRules.button61WorldSize.y / 2f, 0f);
			this.friendshipStatusButtons[i].transform.localScale = ApplicationDesignRules.button62Scale;
		}

		float gapBetweenProfileInformations = (ApplicationDesignRules.profilePictureWorldSize.y - 0.7f) / 3f;
		Vector3 profileInformationScale = new Vector3 (1f, 1f, 1f);

		for(int i=0;i<this.profileInformations.Length;i++)
		{
			this.profileInformations[i].transform.position = new Vector3 (profileBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing + ApplicationDesignRules.profilePictureWorldSize.x + 0.3f, profileBlockUpperLeftPosition.y - 1f - i*gapBetweenProfileInformations, 0f);
			this.profileInformations[i].transform.localScale = ApplicationDesignRules.reductionRatio*profileInformationScale;
		}

		float profileLineScale = ApplicationDesignRules.getLineScale (profileBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing);
		this.profileLine.transform.localScale = new Vector3 (profileLineScale, 1f, 1f);

		float profileStatWidth = (profileBlockSize.x - 2f * ApplicationDesignRules.blockHorizontalSpacing) / 4f;

		for(int i=0;i<this.profileStats.Length;i++)
		{
			this.profileStats[i].transform.position=new Vector3(profileBlockLowerLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+profileStatWidth/2f+i*profileStatWidth,profileBlockUpperLeftPosition.y-profileStatFirstLine);
			this.profileStats[i].transform.localScale= ApplicationDesignRules.profileStatScale;
		}

		this.cleanCardsButton.transform.position = new Vector3 (this.profileStats[3].transform.position.x, profileBlockUpperLeftPosition.y -profileStatFirstLine-0.7f, 0f);
		this.cleanCardsButton.transform.localScale = ApplicationDesignRules.button51Scale;
		
		this.searchBlock.GetComponent<NewBlockController> ().resize(searchBlockLeftMargin,searchBlockUpMargin,ApplicationDesignRules.blockWidth,searchBlockHeight);
		Vector3 searchBlockUpperLeftPosition = this.searchBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 searchBlockUpperRightPosition = this.searchBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 searchBlockSize = this.searchBlock.GetComponent<NewBlockController> ().getSize ();
		Vector3 searchOrigin = this.searchBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.searchBlockTitle.transform.position = new Vector3 (searchBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, searchBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.searchBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		
		this.searchSubtitle.transform.position = new Vector3 (searchBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, searchBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
		this.searchSubtitle.transform.GetComponent<TextContainer>().width=searchBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing;
		this.searchSubtitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		float searchMargin = (searchBlockSize.x - ApplicationDesignRules.largeInputTextWorldSize.x -0.2f- ApplicationDesignRules.button61WorldSize.x*(ApplicationDesignRules.largeInputTextScale.x/ApplicationDesignRules.button61Scale.x)) / 2f;

		this.searchBar.transform.position = new Vector3(searchBlockUpperLeftPosition.x+searchMargin+ApplicationDesignRules.largeInputTextWorldSize.x/2f,searchOrigin.y-searchElementsLine,searchOrigin.z);
		this.searchBar.transform.localScale = ApplicationDesignRules.largeInputTextScale;

		this.searchButton.transform.position = new Vector3(searchBlockUpperRightPosition.x-searchMargin-ApplicationDesignRules.button61WorldSize.x/2f,searchOrigin.y-searchElementsLine,searchOrigin.z);
		this.searchButton.transform.localScale = ApplicationDesignRules.largeInputTextScale;

		this.searchError.transform.position=new Vector3(searchOrigin.x,searchOrigin.y-searchElementsLine-0.45f,0f);
		this.searchError.transform.localScale=ApplicationDesignRules.largeInputTextScale;

		this.friendsBlock.GetComponent<NewBlockController> ().resize(friendsBlockLeftMargin,friendsBlockUpMargin,ApplicationDesignRules.blockWidth,friendsBlockHeight);
		Vector3 friendsBlockUpperLeftPosition = this.friendsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 friendsBlockUpperRightPosition = this.friendsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 friendsBlockLowerLeftPosition = this.friendsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 friendsBlockLowerRightPosition = this.friendsBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 friendsBlockSize = this.friendsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 friendsBlockOrigin = this.friendsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.friendsBlockTitle.transform.position = new Vector3 (friendsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, friendsBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.friendsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		
		float gapBetweenFriendsTab = 0.02f;
		
		float friendsWidth = friendsBlockSize.x - 2f * ApplicationDesignRules.blockHorizontalSpacing;
		float friendsLineScale = ApplicationDesignRules.getLineScale (friendsWidth);

		friendsContents=new GameObject[friendsPagination.nbElementsPerPage];
		for(int i=0;i<this.friendsContents.Length;i++)
		{
			this.friendsContents[i]=Instantiate (this.friendsContentObject) as GameObject;
			this.friendsContents[i].transform.position=new Vector3(friendsBlockOrigin.x,friendsBlockUpperLeftPosition.y-friendsFirstLineY-(i+1)*friendsHeight,0f);
			this.friendsContents[i].transform.FindChild("line").localScale=new Vector3(friendsLineScale,1f,1f);
			this.friendsContents[i].transform.FindChild("picture").localScale=ApplicationDesignRules.thumbScale;
			this.friendsContents[i].transform.FindChild("picture").localPosition=new Vector3(-friendsWidth/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(friendsHeight-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
			this.friendsContents[i].transform.FindChild("username").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.friendsContents[i].transform.FindChild("username").GetComponent<TextMeshPro>().textContainer.width=(friendsWidth/2f)-0.2f-ApplicationDesignRules.thumbWorldSize.x;
			this.friendsContents[i].transform.FindChild("username").localPosition=new Vector3(-friendsWidth/2f+ApplicationDesignRules.thumbWorldSize.x+0.2f,friendsHeight-(friendsHeight-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.friendsContents[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.friendsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=0.75f*friendsWidth-0.2f-ApplicationDesignRules.thumbWorldSize.x;
			this.friendsContents[i].transform.FindChild("description").localPosition=new Vector3(-friendsWidth/2f+ApplicationDesignRules.thumbWorldSize.x+0.2f,friendsHeight-(friendsHeight-ApplicationDesignRules.thumbWorldSize.y)/2f-0.25f,0f);
			this.friendsContents[i].transform.FindChild("divisionIcon").localScale=ApplicationDesignRules.divisionIconScale;
			this.friendsContents[i].transform.FindChild("divisionIcon").localPosition=new Vector3(ApplicationDesignRules.divisionIconDistance.x-friendsWidth/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(friendsHeight-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f+ApplicationDesignRules.divisionIconDistance.y,0f);
			//this.friendsContents[i].transform.FindChild("date").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			//this.friendsContents[i].transform.FindChild("date").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
			//this.friendsContents[i].transform.FindChild("date").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y-(contentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			//this.friendsContents[i].transform.FindChild("new").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			//this.friendsContents[i].transform.FindChild("new").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
			//this.friendsContents[i].transform.FindChild("new").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y/2f,0f);
		}
		this.initializeFriendsContent ();

		friendsStatusButtons=new GameObject[2*friendsPagination.nbElementsPerPage];
		for(int i=0;i<this.friendsContents.Length;i++)
		{
			this.friendsStatusButtons[i*2]=Instantiate(this.friendsStatusButtonObject) as GameObject;
			this.friendsStatusButtons[i*2+1]=Instantiate(this.friendsStatusButtonObject) as GameObject;
			this.friendsStatusButtons[i*2].transform.localScale = ApplicationDesignRules.button31Scale;
			this.friendsStatusButtons[i*2+1].transform.localScale = ApplicationDesignRules.button31Scale;
			this.friendsStatusButtons[i*2].transform.position=new Vector3(friendsBlockUpperRightPosition.x-ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.button31WorldSize.x/2f,friendsBlockUpperRightPosition.y-friendsFirstLineY-(i+0.5f)*friendsHeight+ApplicationDesignRules.button31WorldSize.y/2f+0.025f,0f);
			this.friendsStatusButtons[i*2+1].transform.position=new Vector3(friendsBlockUpperRightPosition.x-ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.button31WorldSize.x/2f,friendsBlockUpperRightPosition.y-friendsFirstLineY-(i+0.5f)*friendsHeight-ApplicationDesignRules.button31WorldSize.y/2f-0.025f,0f);
		}
		this.initializeFriendsStatusButton ();

		challengesButtons=new GameObject[friendsPagination.nbElementsPerPage];
		for(int i=0;i<this.challengesButtons.Length;i++)
		{
			this.challengesButtons[i]=Instantiate(this.challengeButtonObject) as GameObject;
			this.challengesButtons[i].transform.localScale = ApplicationDesignRules.button62Scale;
			this.challengesButtons[i].transform.position=new Vector3(friendsBlockUpperRightPosition.x-ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.button62WorldSize.x/2f,friendsBlockUpperRightPosition.y-friendsFirstLineY-(i+0.5f)*friendsHeight,0f);
		}
		this.initializeChallengeButtons ();
		
		this.friendsPaginationButtons.GetComponent<NewProfileFriendsPaginationController> ().resize ();

		this.resultsBlock.GetComponent<NewBlockController> ().resize(resultsBlockLeftMargin,resultsBlockUpMargin,ApplicationDesignRules.blockWidth,resultsBlockHeight);
		Vector3 resultsBlockUpperLeftPosition = this.resultsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 resultsBlockUpperRightPosition = this.resultsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 resultsBlockLowerLeftPosition = this.resultsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 resultsBlockLowerRightPosition = this.resultsBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 resultsBlockSize = this.resultsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 resultsBlockOrigin = this.resultsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.resultsBlockTitle.transform.position = new Vector3 (resultsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, resultsBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.resultsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		
		float gapBetweenResultsTab = 0.02f;

		float resultsWidth = resultsBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing;
		float resultsLineScale = ApplicationDesignRules.getLineScale (resultsWidth);

		this.resultsContents=new GameObject[resultsPagination.nbElementsPerPage];
		for(int i=0;i<this.resultsContents.Length;i++)
		{
			this.resultsContents[i]=Instantiate(this.resultObject) as GameObject;
			this.resultsContents[i].transform.position=new Vector3(resultsBlockOrigin.x,resultsBlockUpperLeftPosition.y-resultsFirstLineY-(i+1)*resultsHeight,0f);
			this.resultsContents[i].transform.FindChild("line").localScale=new Vector3(resultsLineScale,1f,1f);
			this.resultsContents[i].transform.FindChild("picture").localScale=ApplicationDesignRules.thumbScale;
			this.resultsContents[i].transform.FindChild("picture").localPosition=new Vector3(-resultsWidth/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(resultsHeight-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
			this.resultsContents[i].transform.FindChild("title").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.resultsContents[i].transform.FindChild("title").GetComponent<TextMeshPro>().textContainer.width=(resultsWidth/2f)-0.2f-ApplicationDesignRules.thumbWorldSize.x;
			this.resultsContents[i].transform.FindChild("title").localPosition=new Vector3(-resultsWidth/2f+ApplicationDesignRules.thumbWorldSize.x+0.2f,resultsHeight-(resultsHeight-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.resultsContents[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=resultsWidth-0.2f-ApplicationDesignRules.thumbWorldSize.x;
			this.resultsContents[i].transform.FindChild("description").localPosition=new Vector3(-resultsWidth/2f+ApplicationDesignRules.thumbWorldSize.x+0.2f,resultsHeight-(resultsHeight-ApplicationDesignRules.thumbWorldSize.y)/2f-0.25f,0f);
			this.resultsContents[i].transform.FindChild("divisionIcon").localScale=ApplicationDesignRules.divisionIconScale;
			this.resultsContents[i].transform.FindChild("divisionIcon").localPosition=new Vector3(ApplicationDesignRules.divisionIconDistance.x-resultsWidth/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(resultsHeight-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f+ApplicationDesignRules.divisionIconDistance.y,0f);
			//this.resultsContents[i].transform.FindChild("date").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			//this.resultsContents[i].transform.FindChild("date").GetComponent<TextMeshPro>().textContainer.width=(resultsContentBlockSize.x/4f);
			//this.resultsContents[i].transform.FindChild("date").localPosition=new Vector3(resultsContentBlockSize.x/2f,resultsContentBlockSize.y-(resultsContentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			//this.resultsContents[i].transform.FindChild("new").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			//this.resultsContents[i].transform.FindChild("new").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
			//this.resultsContents[i].transform.FindChild("new").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y/2f,0f);
		}
		this.initializeResultsContent ();

		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().resize ();

		this.slideLeftButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.slideLeftButton.transform.position = new Vector3 (resultsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.roundButtonWorldSize.x/2f, resultsBlockUpperRightPosition.y -ApplicationDesignRules.buttonVerticalSpacing- ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);
		
		this.slideRightButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.slideRightButton.transform.position = new Vector3 (friendsBlockUpperRightPosition.x -ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.roundButtonWorldSize.x/2f, friendsBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing- ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);
		
		this.resultsButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.resultsButton.transform.position = new Vector3 (profileBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f,  profileBlockUpperLeftPosition.y - 2.5f, 0f);
		
		this.friendsButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.friendsButton.transform.position = new Vector3 (profileBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 1.5f*ApplicationDesignRules.roundButtonWorldSize.x,  profileBlockUpperLeftPosition.y - 2.5f, 0f);

		this.mainContentPositionX = profileBlockOrigin.x;
		this.resultsPositionX = resultsBlockOrigin.x;
		this.friendsPositionX = friendsBlockOrigin.x;

		this.searchUsersPopUp.GetComponent<SearchUsersPopUpController> ().resize ();
		this.searchUsersPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.searchUsersPopUp.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);

		if(ApplicationDesignRules.isMobileScreen)
		{
			if(!this.isMyProfile)
			{
				this.friendsTabs[0].SetActive(false);
				this.friendsTabs[1].SetActive(false);
			}
			else
			{
				for(int i=0;i<this.friendsTabs.Length;i++)
				{
					this.friendsTabs[i].transform.localScale = ApplicationDesignRules.tabScale;
					this.friendsTabs[i].transform.position = new Vector3 (friendsBlockUpperLeftPosition.x + ApplicationDesignRules.tabWorldSize.x / 2f, friendsBlockUpperLeftPosition.y+ApplicationDesignRules.tabWorldSize.y/2f,0f);
				}
				this.hideFriendsActiveTab();
			}
			this.friendsPaginationButtons.transform.localPosition=new Vector3 (friendsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 2.5f*ApplicationDesignRules.roundButtonWorldSize.x, friendsBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

			for(int i=0;i<this.resultsTabs.Length;i++)
			{
				this.resultsTabs[i].transform.localScale = ApplicationDesignRules.tabScale;
				this.resultsTabs[i].transform.position = new Vector3 (resultsBlockUpperLeftPosition.x + ApplicationDesignRules.tabWorldSize.x / 2f, resultsBlockUpperLeftPosition.y+ApplicationDesignRules.tabWorldSize.y/2f,0f);
			}
			this.hideResultsActiveTab();
			this.resultsPaginationButtons.transform.localPosition=new Vector3 (resultsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 2.5f*ApplicationDesignRules.roundButtonWorldSize.x, resultsBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

			//float gapBetweenProfileButtons = (profileBlockSize.x-2f*ApplicationDesignRules.button62WorldSize.x)/3f;

			this.profileChooseLanguageButton.transform.position=new Vector3(profileBlockUpperLeftPosition.x +ApplicationDesignRules.blockHorizontalSpacing + 0.5f*ApplicationDesignRules.roundButtonWorldSize.x,profileBlockUpperLeftPosition.y - 2.5f,0f);
			this.profileEditInformationsButton.transform.position = new Vector3 (profileBlockUpperLeftPosition.x +ApplicationDesignRules.blockHorizontalSpacing + 1.5f*ApplicationDesignRules.roundButtonWorldSize.x, profileBlockUpperLeftPosition.y - 2.5f, 0f);
			this.profileEditPasswordButton.transform.position  = new Vector3 (profileBlockUpperLeftPosition.x +ApplicationDesignRules.blockHorizontalSpacing + 2.5f*ApplicationDesignRules.roundButtonWorldSize.x, profileBlockUpperLeftPosition.y - 2.5f, 0f);
			this.profileLine.transform.position = new Vector3 (profileBlockLowerLeftPosition.x+profileBlockSize.x/2f, profileBlockUpperLeftPosition.y - 3f, 0f);

			for(int i=0;i<this.friendshipStatusButtons.Length;i++)
			{
				this.friendshipStatusButtons[i].transform.position = new Vector3 (profileBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing + ApplicationDesignRules.profilePictureWorldSize.x+0.3f+ApplicationDesignRules.button61WorldSize.x/2f, profileBlockUpperLeftPosition.y - 0.2f - ApplicationDesignRules.profilePictureWorldSize.y + ApplicationDesignRules.button61WorldSize.y / 2f-i*(ApplicationDesignRules.button61WorldSize.y), 0f);
				this.friendshipStatusButtons[i].transform.localScale = ApplicationDesignRules.button62Scale;
			}
			for(int i=0;i<this.profileInformations.Length;i++)
			{
				this.profileInformations[i].GetComponent<TextContainer>().width=profileBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.profilePictureWorldSize.x - 0.3f;
			}
		}
		else
		{
			for(int i=0;i<this.friendsTabs.Length;i++)
			{
				this.friendsTabs[i].transform.localScale = ApplicationDesignRules.tabScale;
				this.friendsTabs[i].transform.position = new Vector3 (friendsBlockUpperLeftPosition.x + ApplicationDesignRules.tabWorldSize.x / 2f+ i*(ApplicationDesignRules.tabWorldSize.x+gapBetweenFriendsTab), friendsBlockUpperLeftPosition.y+ApplicationDesignRules.tabWorldSize.y/2f,0f);
			}
			this.friendsTabs[0].SetActive(true);
			if(!this.isMyProfile)
			{
				this.friendsTabs[1].SetActive(false);
			}
			else
			{
				this.friendsTabs[1].SetActive(true);
			}
			this.friendsPaginationButtons.transform.position=new Vector3 (friendsBlockLowerLeftPosition.x + friendsBlockSize.x / 2, friendsBlockLowerLeftPosition.y + 0.3f, 0f);

			for(int i=0;i<this.resultsTabs.Length;i++)
			{
				this.resultsTabs[i].SetActive(true);
				this.resultsTabs[i].transform.localScale = ApplicationDesignRules.tabScale;
				this.resultsTabs[i].transform.position = new Vector3 (resultsBlockUpperLeftPosition.x + ApplicationDesignRules.tabWorldSize.x / 2f+ i*(ApplicationDesignRules.tabWorldSize.x+gapBetweenResultsTab), resultsBlockUpperLeftPosition.y+ApplicationDesignRules.tabWorldSize.y/2f,0f);
			}
			this.profileChooseLanguageButton.transform.position=new Vector3(profileBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f,profileBlockUpperLeftPosition.y - ApplicationDesignRules.buttonVerticalSpacing -ApplicationDesignRules.profilePictureWorldSize.y + ApplicationDesignRules.roundButtonWorldSize.y/2f+ApplicationDesignRules.roundButtonWorldSize.y,0f);
			this.profileEditInformationsButton.transform.position = new Vector3 (profileBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f, profileBlockUpperLeftPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.profilePictureWorldSize.y+ ApplicationDesignRules.roundButtonWorldSize.y/2f, 0f);
			this.profileEditPasswordButton.transform.position  = new Vector3 (profileBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 1.5f*ApplicationDesignRules.roundButtonWorldSize.x, profileBlockUpperLeftPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.profilePictureWorldSize.y+ ApplicationDesignRules.roundButtonWorldSize.y/2f, 0f);
			this.profileLine.transform.position = new Vector3 (profileBlockLowerLeftPosition.x+profileBlockSize.x/2f, profileBlockUpperLeftPosition.y - 0.5f - ApplicationDesignRules.profilePictureWorldSize.y, 0f);

			this.resultsPaginationButtons.transform.position=new Vector3 (resultsBlockLowerLeftPosition.x + resultsBlockSize.x / 2, resultsBlockLowerLeftPosition.y + 0.3f, 0f);

			for(int i=0;i<this.friendshipStatusButtons.Length;i++)
			{
				this.friendshipStatusButtons[i].transform.position = new Vector3 (profileBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing + ApplicationDesignRules.profilePictureWorldSize.x+0.3f+ApplicationDesignRules.button61WorldSize.x/2f+i*(ApplicationDesignRules.button61WorldSize.x+0.05f), profileBlockUpperLeftPosition.y - 0.2f - ApplicationDesignRules.profilePictureWorldSize.y + ApplicationDesignRules.button61WorldSize.y / 2f, 0f);
				this.friendshipStatusButtons[i].transform.localScale = ApplicationDesignRules.button62Scale;
			}
			for(int i=0;i<this.profileInformations.Length;i++)
			{
				this.profileInformations[i].GetComponent<TextContainer>().width=profileBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing-2f*ApplicationDesignRules.roundButtonWorldSize.x-ApplicationDesignRules.profilePictureWorldSize.x - 0.3f;
			}
		}
		this.searchBar.GetComponent<NewProfileSearchBarController>().resize();

		if(this.isCheckPasswordPopUpDisplayed)
		{
			this.checkPasswordPopUpResize();
		}
		else if(this.isChangePasswordPopUpDisplayed)
		{
			this.changePasswordPopUpResize();
		}
		else if(this.isEditInformationsPopUpDisplayed)
		{
			this.editInformationsPopUpResize();
		}
		else if(this.isSelectPicturePopUpDisplayed)
		{
			this.selectPicturePopUpResize();
		}
		else if(this.isChooseLanguagePopUpDisplayed)
		{
			this.chooseLanguagePopUpResize();

		}
		MenuController.instance.resize();
		MenuController.instance.refreshMenuObject();
		TutorialObjectController.instance.resize();
	}
	public void returnPressed()
	{
		if(this.isCheckPasswordPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.checkPasswordHandler();
		}
		else if(this.isChangePasswordPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.editPasswordHandler();
		}
		else if(this.isEditInformationsPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.updateUserInformationsHandler();
		}
	}
	public void escapePressed()
	{
		if(this.isSelectPicturePopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideSelectPicturePopUp();
		}
		else if(this.isCheckPasswordPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideCheckPasswordPopUp();
		}
		else if(this.isChangePasswordPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideChangePasswordPopUp();
		}
		else if(this.isEditInformationsPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideEditInformationsPopUp();
		}
		else if(this.isSearchUsersPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideSearchUsersPopUp();
		}
		else if(this.isChooseLanguagePopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideChooseLanguagePopUp();
		}
		else
		{
			BackOfficeController.instance.leaveGame();
		}
	}
	public void closeAllPopUp()
	{
		if(this.isSelectPicturePopUpDisplayed)
		{
			this.hideSelectPicturePopUp();
		}
		if(this.isCheckPasswordPopUpDisplayed)
		{
			this.hideCheckPasswordPopUp();
		}
		if(this.isChangePasswordPopUpDisplayed)
		{
			this.hideChangePasswordPopUp();
		}
		if(this.isEditInformationsPopUpDisplayed)
		{
			this.hideEditInformationsPopUp();
		}
		if(this.isSearchUsersPopUpDisplayed)
		{
			this.hideSearchUsersPopUp();
		}
		if(this.isChooseLanguagePopUpDisplayed)
		{
			this.hideChooseLanguagePopUp();
		}
	}
	public void drawChallengesRecords()
	{
		this.challengesRecordsDisplayed = new List<int> ();
		for(int i =0;i<this.resultsPagination.nbElementsPerPage;i++)
		{
			if(this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i<model.challengesRecords.Count)
			{
				this.challengesRecordsDisplayed.Add (this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i);
				this.resultsContents[i].transform.FindChild("title").GetComponent<TextMeshPro>().text=model.challengesRecords[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].Friend.Username;
				this.resultsContents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnThumbPicture(model.challengesRecords[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].Friend.IdProfilePicture);
				this.resultsContents[i].transform.FindChild("divisionIcon").GetComponent<DivisionIconController>().setDivision(model.challengesRecords[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].Friend.Division);

				int nbWins = model.challengesRecords[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].NbWins;
				int nbLooses = model.challengesRecords[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].NbLooses;
				if(nbWins>nbLooses)
				{
					this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro> ().text = WordingProfile.getReference(15)+nbWins+WordingProfile.getReference(16)+nbLooses+WordingProfile.getReference(17);
				}
				else if(nbLooses > nbWins)
				{
					this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro> ().text = WordingProfile.getReference(18)+nbLooses+WordingProfile.getReference(19)+nbWins+WordingProfile.getReference(20);
				}
				else
				{
					this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro> ().text = WordingProfile.getReference(21)+nbLooses+WordingProfile.getReference(22);
				}

				this.resultsContents[i].SetActive(true);
			}
			else
			{
				this.resultsContents[i].SetActive(false);
			}
		}
	}
	public void drawFriendsRequests()
	{
		this.friendsRequestsDisplayed = new List<int> ();
		Vector3 friendsStatusButtonPosition = new Vector3 ();
		for(int i =0;i<this.friendsPagination.nbElementsPerPage;i++)
		{
			if(this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i<this.model.friendsRequests.Count)
			{
				this.friendsRequestsDisplayed.Add (this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i);
				if(this.model.friendsRequests[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i].IsInvitingPlayer)
				{
					this.friendsContents[i].transform.FindChild ("description").GetComponent<TextMeshPro> ().text = WordingSocial.getReference(8);
					friendsStatusButtonPosition=this.friendsStatusButtons[2*i].transform.position;
					friendsStatusButtonPosition.y=0.2f+this.challengesButtons[i].transform.position.y;
					this.friendsStatusButtons[2*i].SetActive(true);
					this.friendsStatusButtons[2*i].transform.position=friendsStatusButtonPosition;
					this.friendsStatusButtons[2*i].GetComponent<NewProfileFriendsStatusButtonController>().setId(i);
					this.friendsStatusButtons[2*i].GetComponent<NewProfileFriendsStatusButtonController>().setToAcceptInvitation();
					this.friendsStatusButtons[2*i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSocial.getReference(1);
					friendsStatusButtonPosition=this.friendsStatusButtons[2*i+1].transform.position;
					friendsStatusButtonPosition.y=-0.2f+this.challengesButtons[i].transform.position.y;
					this.friendsStatusButtons[2*i+1].transform.position=friendsStatusButtonPosition;
					this.friendsStatusButtons[2*i+1].SetActive(true);
					this.friendsStatusButtons[2*i+1].GetComponent<NewProfileFriendsStatusButtonController>().setId(i);
					this.friendsStatusButtons[2*i+1].GetComponent<NewProfileFriendsStatusButtonController>().setToDeclineInvitation();
					this.friendsStatusButtons[2*i+1].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSocial.getReference(2);
				}
				else
				{
					this.friendsContents[i].transform.FindChild ("description").GetComponent<TextMeshPro> ().text =  WordingSocial.getReference(9);
					friendsStatusButtonPosition=this.friendsStatusButtons[2*i].transform.position;
					friendsStatusButtonPosition.y=0f+this.challengesButtons[i].transform.position.y;
					this.friendsStatusButtons[2*i].transform.position=friendsStatusButtonPosition;
					this.friendsStatusButtons[2*i].SetActive(true);
					this.friendsStatusButtons[2*i].GetComponent<NewProfileFriendsStatusButtonController>().setId(i);
					this.friendsStatusButtons[2*i].GetComponent<NewProfileFriendsStatusButtonController>().setToCancelInvitation();
					this.friendsStatusButtons[2*i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSocial.getReference(1);
					this.friendsStatusButtons[2*i+1].SetActive(false);
				}
				this.friendsContents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnThumbPicture(model.friendsRequests[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i].User.IdProfilePicture);
				this.friendsContents[i].transform.FindChild("username").GetComponent<TextMeshPro>().text=model.friendsRequests[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i].User.Username;
				this.friendsContents[i].transform.FindChild("divisionIcon").GetComponent<DivisionIconController>().setDivision(model.friendsRequests[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i].User.Division);
				this.friendsContents[i].SetActive(true);
			}
			else
			{
				this.friendsContents[i].SetActive(false);
				this.friendsStatusButtons[2*i].SetActive(false);
				this.friendsStatusButtons[2*i+1].SetActive(false);
			}
		}
	}
	public void drawFriends()
	{
		this.friendsDisplayed = new List<int> ();
		for(int i =0;i<this.friendsPagination.nbElementsPerPage;i++)
		{
			if(this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i<this.friendsToBeDisplayed.Count)
			{
				this.friendsDisplayed.Add (this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i);
				this.friendsContents[i].transform.FindChild("picture").GetComponent<NewProfileFriendsContentPictureController>().reset();
				this.friendsContents[i].transform.FindChild("username").GetComponent<NewProfileFriendsContentUsernameController>().reset();
				this.friendsContents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnThumbPicture(model.users[this.friendsToBeDisplayed[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i]].IdProfilePicture);
				this.friendsContents[i].transform.FindChild("username").GetComponent<TextMeshPro>().text=model.users[this.friendsToBeDisplayed[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i]].Username;
				this.friendsContents[i].transform.FindChild("divisionIcon").GetComponent<DivisionIconController>().setDivision(model.users[this.friendsToBeDisplayed[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i]].Division);
				this.friendsContents[i].SetActive(true);
				string connectionState="";
				Color connectionStateColor=new Color();
				switch(model.users[this.friendsToBeDisplayed[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i]].OnlineStatus)
				{
				case 0:
					connectionState = WordingSocial.getReference(3);
					connectionStateColor=ApplicationDesignRules.whiteTextColor;
					this.challengesButtons[i].SetActive(false);
					break;
				case 1:
					connectionState =WordingSocial.getReference(4);
					connectionStateColor=ApplicationDesignRules.blueColor;
					this.challengesButtons[i].SetActive(true);
					break;
				case 2:
					connectionState = WordingSocial.getReference(5);
					connectionStateColor=ApplicationDesignRules.redColor;
					this.challengesButtons[i].SetActive(false);
					break;
				}
				this.friendsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=connectionState;
				this.friendsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=connectionStateColor;
			}
			else
			{
				this.friendsContents[i].SetActive(false);
				this.challengesButtons[i].SetActive(false);
			}
		}
	}
	public void drawTrophies()
	{
		this.trophiesDisplayed = new List<int> ();
		for(int i =0;i<this.resultsPagination.nbElementsPerPage;i++)
		{
			if(this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i<model.trophies.Count)
			{
				this.trophiesDisplayed.Add (this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i);
				this.resultsContents[i].transform.FindChild("title").GetComponent<TextMeshPro>().text=model.trophies[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].competition.Name;
				this.resultsContents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCompetitionPicture(model.trophies[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].competition.getPictureId());
				this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=WordingProfile.getReference(23)+model.trophies[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].Date.ToString(WordingDates.getDateFormat());
				this.resultsContents[i].SetActive(true);
			}
			else
			{
				this.resultsContents[i].SetActive(false);
			}
		}
	}
	public void drawConfrontations()
	{
		this.confrontationsDisplayed = new List<int> ();
		for(int i =0;i<this.resultsPagination.nbElementsPerPage;i++)
		{
			if(this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i<model.confrontations.Count)
			{
				this.confrontationsDisplayed.Add (this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i);
				string gameTypeName="";
				int idCompetitionPicture;
				switch(model.confrontations[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].GameType)
				{
				case 1:
					gameTypeName=WordingGameModes.getReference(8);
					idCompetitionPicture=1;
					break;
//				case 2:
//					gameTypeName="Match de coupe";
//					idCompetitionPicture=2;
//					break;
				default:
					gameTypeName=WordingGameModes.getReference(9);
					idCompetitionPicture=0;
					break;
				}
				this.resultsContents[i].transform.FindChild("title").GetComponent<TextMeshPro>().text=gameTypeName;
				this.resultsContents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCompetitionPicture(idCompetitionPicture);
				string description="";
				Color textColor=new Color();
				if(model.confrontations[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].IdWinner==ApplicationModel.player.Id)
				{
					description="Victoire le "+model.confrontations[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].Date.ToString(WordingDates.getDateFormat());
					textColor=ApplicationDesignRules.blueColor;
				}
				else
				{
					description="Défaite le "+model.confrontations[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].Date.ToString(WordingDates.getDateFormat());
					textColor=ApplicationDesignRules.redColor;
				}
				this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=description;
				this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=textColor;
				this.resultsContents[i].SetActive(true);
			}
			else
			{
				this.resultsContents[i].SetActive(false);
			}
		}
	}
	private void drawProfile()
	{

		this.profileStats[0].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text=model.displayedUser.TotalNbWins.ToString ();
		this.profileStats[1].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.displayedUser.TotalNbLooses.ToString ();
		this.profileStats[2].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.displayedUser.Ranking.ToString ();
		this.profileStats[2].transform.FindChild ("Subvalue").GetComponent<TextMeshPro> ().text= WordingProfile.getReference(24)+model.displayedUser.RankingPoints.ToString()+WordingProfile.getReference(25);
		this.profileStats[3].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.displayedUser.CollectionRanking.ToString ();
		this.profileStats[3].transform.FindChild ("Subvalue").GetComponent<TextMeshPro> ().text= WordingProfile.getReference(24)+model.displayedUser.CollectionPoints.ToString()+WordingProfile.getReference(25);
		this.profileBlockTitle.GetComponent<TextMeshPro> ().text = model.displayedUser.Username;
		if(this.isMyProfile)
		{
			this.drawPersonalInformations ();
		}
		this.drawProfilePicture ();
		this.profileDivisionIcon.transform.GetComponent<DivisionIconController>().setDivision(model.displayedUser.Division);
	}
	private void drawPersonalInformations()
	{
		this.profileInformations [0].GetComponent<TextMeshPro> ().text = WordingProfile.getReference(26)+ApplicationModel.player.FirstName;
		this.profileInformations [1].GetComponent<TextMeshPro> ().text = WordingProfile.getReference(27)+ApplicationModel.player.Surname;
		this.profileInformations [2].GetComponent<TextMeshPro> ().text = WordingProfile.getReference(28)+ApplicationModel.player.Mail;
	}
	private void drawProfilePicture()
	{
		this.profilePicture.GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnLargeProfilePicture(model.displayedUser.IdProfilePicture);
	}
	public void checkFriendsOnlineStatus()
	{
		if(PhotonNetwork.insideLobby)
		{
			PhotonNetwork.FindFriends (model.usernameList);
		}
	}
	public void OnUpdatedFriendList()
	{
		if(this.isMyProfile)
		{
			for(int i=0;i<PhotonNetwork.Friends.Count;i++)
			{
				for(int j=0;j<model.users.Count;j++)
				{
					if(model.users[j].Username==PhotonNetwork.Friends[i].Name)
					{
						if(PhotonNetwork.Friends[i].IsInRoom)
						{
							if(model.friends.Contains(j))
							{
								if(!this.friendsOnline.Contains(j))
								{
									this.friendsOnline.Insert(0,j);
									model.users[j].OnlineStatus=2;
								}
								else if(model.users[j].OnlineStatus!=2)
								{
									this.friendsOnline.Remove(j);
									this.friendsOnline.Insert(0,j);
									model.users[j].OnlineStatus=2;
								}
							}
						}
						else if(PhotonNetwork.Friends[i].IsOnline)
						{
							if(model.friends.Contains(j))
							{
								if(!this.friendsOnline.Contains(j))
								{
									this.friendsOnline.Insert(0,j);
									model.users[j].OnlineStatus=1;
								}
								else if(model.users[j].OnlineStatus!=1)
								{
									this.friendsOnline.Remove(j);
									this.friendsOnline.Insert(0,j);
									model.users[j].OnlineStatus=1;
								}
							}
						}
						else
						{
							model.users[j].OnlineStatus=0;
						}
						break;
					}
				}
			}
			if(this.activeFriendsTab == 0 && this.friendsPagination.chosenPage==0)
			{
				this.initializeFriends();
			}
		}
	}
	public void sortFriendsList()
	{
		this.friendsToBeDisplayed = new List<int> ();
		if(this.isMyProfile)
		{
			for(int i=0;i<this.friendsOnline.Count;i++)
			{
				this.friendsToBeDisplayed.Add (this.friendsOnline[i]);
			}
		}
		for(int i=0;i<model.friends.Count;i++)
		{
			if(!this.friendsToBeDisplayed.Contains(model.friends[i]))
			{
				this.friendsToBeDisplayed.Add(model.friends[i]);
			}
		}
	}
	public void sendInvitationHandler(int id)
	{
		SoundController.instance.playSound(9);
		if(!ApplicationModel.player.HasDeck)
		{
			BackOfficeController.instance.displayErrorPopUp(WordingGameModes.getReference(5));
		}
		else if(model.users [this.friendsToBeDisplayed[this.friendsDisplayed[id]]].OnlineStatus!=1)
		{
			BackOfficeController.instance.displayErrorPopUp(WordingGameModes.getReference(6));
		}
		else
		{
			StartCoroutine (this.sendInvitation (id));
		}
	}
	public IEnumerator sendInvitation(int id)
	{
		BackOfficeController.instance.displayLoadingScreen ();
		StartCoroutine (BackOfficeController.instance.sendInvitation (model.users [this.friendsToBeDisplayed[this.friendsDisplayed[id]]], ApplicationModel.player));
		yield break;
	}
	public void acceptFriendsRequestHandler(int id)
	{
		SoundController.instance.playSound(9);
		StartCoroutine (this.confirmFriendRequest (id));
	}
	public void declineFriendsRequestHandler(int id)
	{
		SoundController.instance.playSound(9);
		StartCoroutine (this.removeFriendRequest (id));
	}
	public void cancelFriendsRequestHandler(int id)
	{
		SoundController.instance.playSound(9);
		StartCoroutine (this.removeFriendRequest (id));
	}
	public void startHoveringProfilePicture()
	{
		if(this.isMyProfile)
		{
			this.isProfilePictureHovered = true;
			if(!ApplicationDesignRules.isMobileScreen)
			{
				this.profileEditPictureButton.SetActive (true);
			}
		}	
	}
	public void endHoveringProfilePicture()
	{
		if(this.isMyProfile)
		{
			this.isProfilePictureHovered = false;
			if(!ApplicationDesignRules.isMobileScreen)
			{
				this.profileEditPictureButton.SetActive (false);
			}
		}
	}
	public void editProfilePictureHandler()
	{
		if(this.isMyProfile)
		{
			SoundController.instance.playSound(9);
			this.displaySelectPicturePopUp ();
		}
	}
	private void displaySelectPicturePopUp()
	{
		BackOfficeController.instance.displayTransparentBackground ();
		this.selectPicturePopUp.SetActive (true);
		this.selectPicturePopUp.GetComponent<SelectPicturePopUpController> ().selectPicture (ApplicationModel.player.IdProfilePicture);
		this.selectPicturePopUpResize ();
		this.isSelectPicturePopUpDisplayed = true;
	}
	private void selectPicturePopUpResize()
	{
		this.selectPicturePopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.selectPicturePopUp.transform.localScale = ApplicationDesignRules.popUpScale;
	}
	public void hideSelectPicturePopUp()
	{
		this.selectPicturePopUp.SetActive(false);
		BackOfficeController.instance.hideTransparentBackground ();
		this.isSelectPicturePopUpDisplayed = false;
	}
	public void changeUserPictureHandler(int id)
	{
		this.hideSelectPicturePopUp ();
		if(id!=ApplicationModel.player.IdProfilePicture)
		{
			StartCoroutine(this.changeUserPicture(id));
		}
	}
	public IEnumerator changeUserPicture(int id)
	{
		BackOfficeController.instance.displayLoadingScreen();
		yield return StartCoroutine(ApplicationModel.player.setProfilePicture(id));
		model.displayedUser.IdProfilePicture=ApplicationModel.player.IdProfilePicture;
		this.drawProfilePicture ();
		MenuController.instance.changeThumbPicture ();
		BackOfficeController.instance.hideLoadingScreen();
	}
	public void displayChooseLanguagePopUp()
	{
		SoundController.instance.playSound(9);
		BackOfficeController.instance.displayTransparentBackground ();
		this.chooseLanguagePopUp.SetActive (true);
		this.chooseLanguagePopUp.GetComponent<ChooseLanguagePopUpController> ().reset();
		this.chooseLanguagePopUpResize ();
		this.isChooseLanguagePopUpDisplayed = true;
	}
	private void chooseLanguagePopUpResize()
	{
		this.chooseLanguagePopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.chooseLanguagePopUp.transform.localScale = ApplicationDesignRules.popUpScale;
	}
	public void hideChooseLanguagePopUp()
	{
		this.chooseLanguagePopUp.SetActive(false);
		BackOfficeController.instance.hideTransparentBackground ();
		this.isChooseLanguagePopUpDisplayed = false;
	}
	public void chooseLanguageHandler(int id)
	{
		this.hideChooseLanguagePopUp ();
		if(id!=ApplicationModel.player.IdLanguage)
		{
			StartCoroutine(this.chooseLanguage(id));
		}
	}
	public IEnumerator chooseLanguage(int id)
	{
		BackOfficeController.instance.displayLoadingScreen();
		yield return StartCoroutine(ApplicationModel.player.chooseLanguage(id));
		if(ApplicationModel.player.Error=="")
		{
			MenuController.instance.profileLink();
		}
		else
		{
			BackOfficeController.instance.displayErrorPopUp(ApplicationModel.player.Error);
			ApplicationModel.player.Error="";
			BackOfficeController.instance.hideLoadingScreen();
		}
	}
	public void displayCheckPasswordPopUp()
	{
		SoundController.instance.playSound(9);
		BackOfficeController.instance.displayTransparentBackground ();
		this.checkPasswordPopUp.transform.GetComponent<CheckPasswordPopUpController> ().reset ();
		this.isCheckPasswordPopUpDisplayed = true;
		this.checkPasswordPopUp.SetActive (true);
		this.checkPasswordPopUpResize ();
	}
	public void displayChangePasswordPopUp()
	{
		BackOfficeController.instance.displayTransparentBackground ();
		this.changePasswordPopUp.transform.GetComponent<ChangePasswordPopUpController> ().reset ();
		this.isChangePasswordPopUpDisplayed = true;
		this.changePasswordPopUp.SetActive (true);
		this.changePasswordPopUpResize ();
	}
	public void displayEditInformationsPopUp()
	{
		SoundController.instance.playSound(9);
		BackOfficeController.instance.displayTransparentBackground ();
		this.editInformationsPopUp.transform.GetComponent<EditInformationsPopUpController> ().reset (ApplicationModel.player.FirstName,ApplicationModel.player.Surname,ApplicationModel.player.Mail);
		this.isEditInformationsPopUpDisplayed = true;
		this.editInformationsPopUp.SetActive (true);
		this.editInformationsPopUpResize ();
	}
	public void displayMessagePopUp(int messageId)
	{
		BackOfficeController.instance.displayTransparentBackground ();
		this.messagePopUp.transform.GetComponent<ProfileMessagePopUpController> ().reset (messageId);
		this.isMessagePopUpDisplayed = true;
		this.messagePopUp.SetActive (true);
		this.messagePopUpResize ();
	}
	public void checkPasswordPopUpResize()
	{
		this.checkPasswordPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.checkPasswordPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.checkPasswordPopUp.GetComponent<CheckPasswordPopUpController> ().resize ();
	}
	public void changePasswordPopUpResize()
	{
		this.changePasswordPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.changePasswordPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.changePasswordPopUp.GetComponent<ChangePasswordPopUpController> ().resize ();
	}
	public void editInformationsPopUpResize()
	{
		this.editInformationsPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.editInformationsPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.editInformationsPopUp.GetComponent<EditInformationsPopUpController> ().resize ();
	}
	public void messagePopUpResize()
	{
		this.messagePopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.messagePopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.messagePopUp.GetComponent<ProfileMessagePopUpController> ().resize ();
	}
	public void hideCheckPasswordPopUp()
	{
		this.checkPasswordPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isCheckPasswordPopUpDisplayed = false;
	}
	public void hideChangePasswordPopUp()
	{
		this.changePasswordPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isChangePasswordPopUpDisplayed = false;
	}
	public void hideEditInformationsPopUp()
	{
		this.editInformationsPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isEditInformationsPopUpDisplayed = false;
	}
	public void hideMessagePopUp()
	{
		this.messagePopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isMessagePopUpDisplayed = false;
	}
	public void checkPasswordHandler()
	{
		string password = this.checkPasswordPopUp.GetComponent<CheckPasswordPopUpController>().getPassword();
		StartCoroutine (checkPassword (password));
	}
	private IEnumerator checkPassword(string password)
	{
		string checkPassword = this.checkPasswordComplexity (password);
		if(checkPassword=="")
		{
			BackOfficeController.instance.displayLoadingScreen();
			this.checkPasswordPopUp.SetActive(false);
			yield return StartCoroutine(ApplicationModel.player.checkPassword(password));
			BackOfficeController.instance.hideLoadingScreen();
			this.checkPasswordPopUp.SetActive(true);
			if(ApplicationModel.player.Error=="")
			{
				this.hideCheckPasswordPopUp();
				this.displayChangePasswordPopUp();
			}
			else
			{
				SoundController.instance.playSound(13);
				this.checkPasswordPopUp.GetComponent<CheckPasswordPopUpController>().setError(ApplicationModel.player.Error);
				ApplicationModel.player.Error="";
			}
		}
		else
		{
			SoundController.instance.playSound(13);
			this.checkPasswordPopUp.GetComponent<CheckPasswordPopUpController>().setError(checkPassword);
		}
	}
	public string checkPasswordComplexity(string password)
	{
		if(password.Length<5)
		{
			return WordingProfile.getReference(29);
		}
		else if(!Regex.IsMatch(password, @"^[a-zA-Z0-9_.@]+$"))
		{
			return WordingProfile.getReference(30);
		} 
		return "";
	}
	public void editPasswordHandler()
	{
		string firstPassword = this.changePasswordPopUp.transform.GetComponent<ChangePasswordPopUpController>().getFirstPassword();
		string secondPassword = this.changePasswordPopUp.transform.GetComponent<ChangePasswordPopUpController> ().getSecondPassword ();
		string passwordCheck = this.checkPasswordEgality (firstPassword,secondPassword);
		if(passwordCheck=="")
		{
			passwordCheck=this.checkPasswordComplexity(firstPassword);
			if(passwordCheck=="")
			{
				ApplicationModel.player.Password=firstPassword;
				StartCoroutine(this.editPassword());
			}
		}
		if(passwordCheck!="")
		{
			SoundController.instance.playSound(13);
		}
		this.changePasswordPopUp.transform.GetComponent<ChangePasswordPopUpController> ().setError (passwordCheck);

	}
	private IEnumerator editPassword()
	{
		this.hideChangePasswordPopUp ();
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine(ApplicationModel.player.editPassword());
		if(ApplicationModel.player.Error=="")
		{
			this.hideChangePasswordPopUp();
		}
		else
		{
			SoundController.instance.playSound(13);
			this.changePasswordPopUp.GetComponent<ChangePasswordPopUpController>().setError(ApplicationModel.player.Error);
			ApplicationModel.player.Error="";
		}
		BackOfficeController.instance.hideLoadingScreen ();
	}
	public string checkPasswordEgality (string password1, string password2)
	{
		if(password1=="")
		{
			return WordingProfile.getReference(31);
		}
		else if(password2=="")
		{
			return WordingProfile.getReference(32);
		}
		else if(password1!=password2)
		{
			return WordingProfile.getReference(33);
		}
		return "";
	}
	public void updateUserInformationsHandler()
	{
		string firstname = this.editInformationsPopUp.transform.GetComponent<EditInformationsPopUpController> ().getFirstInput ();
		string surname = this.editInformationsPopUp.transform.GetComponent<EditInformationsPopUpController> ().getSecondInput ();
		string mail = this.editInformationsPopUp.transform.GetComponent<EditInformationsPopUpController> ().getThirdInput ();
		string error = this.checkname (firstname);

		if(error=="")
		{
			error = this.checkname (surname);
			if(error=="")
			{
				error = this.checkEmail (mail);
				if(error=="")
				{
					bool isNewEmail=false;
					if(mail!=ApplicationModel.player.Mail)
					{
						isNewEmail=true;
					}
					StartCoroutine(updateUserInformations(firstname,surname,mail,isNewEmail));
				}
			}
		}
		if(error!="")
		{
			SoundController.instance.playSound(13);
		}
		this.editInformationsPopUp.transform.GetComponent<EditInformationsPopUpController> ().setError (error);
	}
	private IEnumerator updateUserInformations(string firstname, string surname, string mail, bool isNewEmail)
	{
		BackOfficeController.instance.displayLoadingScreen ();
		this.editInformationsPopUp.SetActive(false);
		yield return StartCoroutine (ApplicationModel.player.updateInformations (firstname,surname,mail,isNewEmail));
		this.editInformationsPopUp.SetActive(true);
		if(ApplicationModel.player.Error=="")
		{
			this.hideEditInformationsPopUp ();
			if(isNewEmail)
			{
				this.displayMessagePopUp(1);
			}
			this.drawPersonalInformations ();
		}
		else
		{
			this.editInformationsPopUp.SetActive(true);
			this.editInformationsPopUp.transform.GetComponent<EditInformationsPopUpController> ().setError (ApplicationModel.player.Error);
			ApplicationModel.player.Error="";
			SoundController.instance.playSound(13);
		}
		BackOfficeController.instance.hideLoadingScreen ();
	}
	public string checkname(string name)
	{
		if(name.Length>20)
		{
			return WordingProfile.getReference(37);
		}
		else if(!Regex.IsMatch(name, @"^[a-zA-Z0-9_]+$"))
		{
			return WordingProfile.getReference(34);
		}   
		return "";
	}
	public string checkEmail(string email)
	{
		if(name.Length>40)
		{
			return WordingProfile.getReference(38);
		}
		else if(!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
		{
			return WordingProfile.getReference(35);
		}
		return "";
	}
	public void searchUsersHandler()
	{
		SoundController.instance.playSound(9);
		this.searchValue = this.searchBar.GetComponent<NewProfileSearchBarController>().getInputText();
		if(this.searchValue.Length>2)
		{
			this.displaySearchUsersPopUp(this.searchValue);
			this.searchError.GetComponent<TextMeshPro>().text="";
		}
		else
		{
			this.searchError.GetComponent<TextMeshPro>().text=WordingProfile.getReference(36);
		}
	}
	private void displaySearchUsersPopUp(string searchValue)
	{
		BackOfficeController.instance.displayTransparentBackground ();
		this.searchUsersPopUp.SetActive (true);
		StartCoroutine(this.searchUsersPopUp.GetComponent<SearchUsersPopUpController> ().initialization (searchValue));
		this.isSearchUsersPopUpDisplayed = true;
	}
	public void hideSearchUsersPopUp()
	{
		this.searchUsersPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground ();
		this.isSearchUsersPopUpDisplayed = false;
	}
	public IEnumerator confirmFriendRequest(int id)
	{
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.confirm ());
		if(model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.Error=="")
		{
			model.moveToFriend(this.friendsRequestsDisplayed[id]);
			this.initializeFriendsRequests();
		}
		else
		{
			BackOfficeController.instance.displayErrorPopUp(model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.Error);
			model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.Error="";
		}
		BackOfficeController.instance.hideLoadingScreen ();
	}
	public IEnumerator confirmConnection()
	{
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.displayedUser.ConnectionWithPlayer.confirm ());
		if(model.displayedUser.ConnectionWithPlayer.Error=="")
		{
			this.initializeFriendshipState();
			this.initializeFriends();
		}
		else
		{
			BackOfficeController.instance.displayErrorPopUp(model.displayedUser.ConnectionWithPlayer.Error);
			model.displayedUser.ConnectionWithPlayer.Error="";
		}
		BackOfficeController.instance.hideLoadingScreen ();
	}
	public IEnumerator removeFriendRequest(int id)
	{
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.remove ());
		if(model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.Error=="")
		{
			model.friendsRequests.RemoveAt(this.friendsRequestsDisplayed[id]);
			this.initializeFriendsRequests();
		}
		else
		{
			BackOfficeController.instance.displayErrorPopUp(model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.Error);
			model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.Error="";
		}
		BackOfficeController.instance.hideLoadingScreen ();
	}
	public IEnumerator removeConnection()
	{
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.displayedUser.ConnectionWithPlayer.remove ());
		if(model.displayedUser.ConnectionWithPlayer.Error=="")
		{
			model.displayedUser.IsConnectedToPlayer=false;
			this.initializeFriends();
			this.initializeFriendshipState();
		}
		else
		{
			BackOfficeController.instance.displayErrorPopUp(model.displayedUser.ConnectionWithPlayer.Error);
			model.displayedUser.ConnectionWithPlayer.Error="";
		}
		BackOfficeController.instance.hideLoadingScreen ();
	}
	public IEnumerator addConnection()
	{
		BackOfficeController.instance.displayLoadingScreen ();
		Connection connection = new Connection ();
		connection.IdUser1 = ApplicationModel.player.Id;
		connection.IdUser2 = model.displayedUser.Id;
		connection.IsAccepted = false;

		yield return StartCoroutine(connection.add ());
		if(connection.Error=="")
		{
			model.displayedUser.IsConnectedToPlayer=true;
			model.displayedUser.ConnectionWithPlayer=connection;
			this.initializeFriendshipState();
		}
		else
		{
			BackOfficeController.instance.displayErrorPopUp(connection.Error);
			connection.Error="";
		}
		BackOfficeController.instance.hideLoadingScreen ();
	}
	private void drawFriendshipState()
	{
		if(model.displayedUser.IsConnectedToPlayer)
		{
			if(model.displayedUser.ConnectionWithPlayer.IsAccepted)
			{
				this.friendshipStatusButtons[0].SetActive(true);
				this.friendshipStatusButtons[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSocial.getReference(10);
				this.friendshipStatusButtons[1].SetActive(false);
				this.friendshipStatus.GetComponent<TextMeshPro>().text=WordingSocial.getReference(11);
			}
			else if(model.displayedUser.ConnectionWithPlayer.IdUser1==model.displayedUser.Id)
			{
				this.friendshipStatusButtons[0].SetActive(true);
				this.friendshipStatusButtons[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSocial.getReference(12);
				this.friendshipStatusButtons[1].SetActive(true);
				this.friendshipStatusButtons[1].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSocial.getReference(13);
				this.friendshipStatus.GetComponent<TextMeshPro>().text=WordingSocial.getReference(14);
			}
			else if(model.displayedUser.ConnectionWithPlayer.IdUser1==ApplicationModel.player.Id)
			{
				this.friendshipStatusButtons[0].SetActive(true);
				this.friendshipStatusButtons[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSocial.getReference(15);
				this.friendshipStatusButtons[1].SetActive(false);
				this.friendshipStatus.GetComponent<TextMeshPro>().text=WordingSocial.getReference(16);
			}
		}
		else
		{
			this.friendshipStatusButtons[0].SetActive(true);
			this.friendshipStatusButtons[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSocial.getReference(17);
			this.friendshipStatusButtons[1].SetActive(false);
			this.friendshipStatus.GetComponent<TextMeshPro>().text=WordingSocial.getReference(18);
		}
	}
	public void friendshipStateHandler(int buttonId)
	{
		SoundController.instance.playSound(9);
		if(model.displayedUser.IsConnectedToPlayer)
		{
			if(model.displayedUser.ConnectionWithPlayer.IsAccepted)
			{
				StartCoroutine(this.removeConnection());
			}
			else if(model.displayedUser.ConnectionWithPlayer.IdUser1==model.displayedUser.Id)
			{
				if(buttonId==0)
				{
					StartCoroutine(this.confirmConnection());
				}
				else
				{
					StartCoroutine(this.removeConnection());
				}
			}
			else if(model.displayedUser.ConnectionWithPlayer.IdUser1==ApplicationModel.player.Id)
			{
				StartCoroutine(this.removeConnection());
			}
		}
		else
		{
			StartCoroutine(this.addConnection());
		}
	}
	public void cleanCardsHandler()
	{
		SoundController.instance.playSound(9);
		StartCoroutine (this.cleanCards ());
	}
	public IEnumerator cleanCards()
	{
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine(ApplicationModel.player.cleanCards ());
		BackOfficeController.instance.hideLoadingScreen ();
	}
	public void clickOnFriendsContentProfile(int id)
	{
		SoundController.instance.playSound(9);
		ApplicationModel.player.ProfileChosen = this.friendsContents [id].transform.FindChild ("username").GetComponent<TextMeshPro> ().text;
		SceneManager.LoadScene("NewProfile");
	}
	public void clickOnResultsContentProfile(int id)
	{
		SoundController.instance.playSound(9);
		ApplicationModel.player.ProfileChosen = this.resultsContents [id].transform.FindChild ("title").GetComponent<TextMeshPro> ().text;
		SceneManager.LoadScene("NewProfile");
	}
	public void slideRight()
	{
		SoundController.instance.playSound(16);
		if(this.mainContentDisplayed)
		{
			this.mainContentDisplayed=false;
			this.targetContentPositionX=this.resultsPositionX;
		}
		else if(this.friendsSliderDisplayed)
		{
			this.friendsSliderDisplayed=false;
			this.targetContentPositionX=this.mainContentPositionX;
		}
		else if(this.targetContentPositionX==mainContentPositionX)
		{
			this.targetContentPositionX=this.resultsPositionX;
		}
		else if(this.targetContentPositionX==resultsPositionX)
		{
			this.targetContentPositionX=this.mainContentPositionX;
		}
		this.toSlideRight=true;
		this.toSlideLeft=false;
	}
	public void slideLeft()
	{
		SoundController.instance.playSound(16);
		if(this.mainContentDisplayed)
		{
			this.mainContentDisplayed=false;
			this.targetContentPositionX=this.friendsPositionX;
		}
		else if(this.resultsSliderDisplayed)
		{
			this.resultsSliderDisplayed=false;
			this.targetContentPositionX=this.mainContentPositionX;
		}
		else if(this.targetContentPositionX==mainContentPositionX)
		{
			this.targetContentPositionX=this.friendsPositionX;
		}
		else if(this.targetContentPositionX==resultsPositionX)
		{
			this.targetContentPositionX=this.mainContentPositionX;
		}
		this.toSlideRight=false;
		this.toSlideLeft=true;
	}
	public void hideFriendsActiveTab()
	{
		this.friendsBlockTitle.GetComponent<TextMeshPro>().text=this.friendsTabs[this.activeFriendsTab].transform.FindChild("Title").GetComponent<TextMeshPro>().text;
		if(this.isMyProfile)
		{
			switch(this.activeFriendsTab)
			{
			case 0:
				this.friendsTabs[0].SetActive(false);
				this.friendsTabs[1].SetActive(true);
				break;
			case 1:
				this.friendsTabs[0].SetActive(true);
				this.friendsTabs[1].SetActive(false);
				break;
			}
		}
	}
	public void hideResultsActiveTab()
	{
		this.resultsBlockTitle.GetComponent<TextMeshPro>().text=this.resultsTabs[this.activeResultsTab].transform.FindChild("Title").GetComponent<TextMeshPro>().text;
		switch(this.activeResultsTab)
		{
		case 0:
			this.resultsTabs[0].SetActive(false);
			this.resultsTabs[1].SetActive(true);
			break;
		case 1:
			this.resultsTabs[0].SetActive(true);
			this.resultsTabs[1].SetActive(false);
			break;
		}
	}
	public void cleanResultsContents()
	{
		for(int i =0;i<this.resultsContents.Length;i++)
		{
			Destroy(this.resultsContents[i]);
		}
	}
	public void cleanFriendsContents()
	{
		for(int i=0;i<this.friendsContents.Length;i++)
		{
			Destroy(this.friendsContents[i]);
		}
	}
	public void cleanChallengeButtons()
	{
		for(int i=0;i<this.challengesButtons.Length;i++)
		{
			Destroy(this.challengesButtons[i]);
		}
	}
	public void cleanFriendsStatusButtons()
	{
		for(int i=0;i<this.friendsStatusButtons.Length;i++)
		{
			Destroy(this.friendsStatusButtons[i]);
		}
	}
	public void initializeFriendsStatusButton()
	{
		for(int i=0;i<this.friendsStatusButtons.Length;i++)
		{
			this.friendsStatusButtons[i].AddComponent<NewProfileFriendsStatusButtonController>();
		}
	}
	public void initializeFriendsContent()
	{
		for(int i=0;i<this.friendsContents.Length;i++)
		{
			this.friendsContents[i].transform.FindChild("picture").gameObject.AddComponent<NewProfileFriendsContentPictureController>();
			this.friendsContents[i].transform.FindChild("picture").GetComponent<NewProfileFriendsContentPictureController>().setId(i);
			this.friendsContents[i].transform.FindChild("username").gameObject.AddComponent<NewProfileFriendsContentUsernameController>();
			this.friendsContents[i].transform.FindChild("username").GetComponent<NewProfileFriendsContentUsernameController>().setId(i);
		}
	}
	public void initializeChallengeButtons()
	{
		for(int i=0;i<this.challengesButtons.Length;i++)
		{
			this.challengesButtons[i].AddComponent<NewProfileChallengeButtonController>();
			this.challengesButtons[i].GetComponent<NewProfileChallengeButtonController>().setId(i);
			this.challengesButtons[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSocial.getReference(0);
		}
	}
	public void initializeResultsContent()
	{
		for(int i=0;i<this.resultsContents.Length;i++)
		{
			this.resultsContents[i].transform.FindChild("picture").gameObject.AddComponent<NewProfileResultsContentPictureController>();
			this.resultsContents[i].transform.FindChild("picture").GetComponent<NewProfileResultsContentPictureController>().setId(i);
			this.resultsContents[i].transform.FindChild("title").gameObject.AddComponent<NewProfileResultsContentUsernameController>();
			this.resultsContents[i].transform.FindChild("title").GetComponent<NewProfileResultsContentUsernameController>().setId(i); 
		}
	}
	#region TUTORIAL FUNCTIONS
	
	public bool getIsMyProfile()
	{
		return this.isMyProfile;
	}
	public Vector3 getProfileBlockOrigin()
	{
		return this.profileBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getProfileBlockSize()
	{
		return this.profileBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public Vector3 getResultsBlockOrigin()
	{
		Vector3 blockOrigin = this.resultsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		blockOrigin.y = blockOrigin.y + ApplicationDesignRules.tabWorldSize.y / 2f;
		return blockOrigin;
	}
	public Vector2 getResultsBlockSize()
	{
		Vector2 blockSize=this.resultsBlock.GetComponent<NewBlockController> ().getSize ();
		blockSize.y = blockSize.y + ApplicationDesignRules.tabWorldSize.y;
		return blockSize;
	}
	public Vector3 getFriendsBlockOrigin()
	{
		Vector3 blockOrigin = this.friendsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		blockOrigin.y = blockOrigin.y + ApplicationDesignRules.tabWorldSize.y / 2f;
		return blockOrigin;
	}
	public Vector2 getFriendsBlockSize()
	{
		Vector2 blockSize=this.friendsBlock.GetComponent<NewBlockController> ().getSize ();
		blockSize.y = blockSize.y + ApplicationDesignRules.tabWorldSize.y;
		return blockSize;
	}
	public Vector3 getSearchBlockOrigin()
	{
		return this.searchBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getSearchBlockSize()
	{
		return this.searchBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public IEnumerator endHelp()
	{
		if(!ApplicationModel.player.ProfileTutorial)
		{
			BackOfficeController.instance.displayLoadingScreen();
			yield return StartCoroutine(ApplicationModel.player.setProfileTutorial(true));
			BackOfficeController.instance.hideLoadingScreen();
		}
	}
	public bool getIsFriendsSliderDisplayed()
	{
		return this.friendsSliderDisplayed;
	}
	public bool getIsResultsSliderDisplayed()
	{
		return this.resultsSliderDisplayed;
	}
	#endregion
}