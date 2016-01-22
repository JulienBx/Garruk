using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class NewProfileController : MonoBehaviour
{
	public static NewProfileController instance;
	private NewProfileModel model;
	
	public GameObject blockObject;
	public GameObject friendsContentObject;
	public GameObject challengeButtonObject;
	public GameObject friendsStatusButtonObject;
	public GameObject resultObject;

	private GameObject menu;
	private GameObject tutorial;

	private GameObject profileBlock;
	private GameObject profileBlockTitle;
	private GameObject cleanCardsButton;
	private GameObject friendshipStatus;
	private GameObject[] friendshipStatusButtons;
	private GameObject profilePicture;
	private GameObject profileEditPictureButton;
	private GameObject profileEditInformationsButton;
	private GameObject profileEditPasswordButton;
	private GameObject[] profileInformations;
	private GameObject profileLine;
	private GameObject[] profileStats;

	private GameObject searchBlock;
	private GameObject searchBlockTitle;
	private GameObject searchSubtitle;
	private GameObject searchBar;
	private GameObject searchButton;

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

	private bool isSearchingUsers;
	private string searchValue;
	private bool isMouseOnSearchBar;
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
		
		if (Input.touchCount == 1 && this.isSceneLoaded) 
		{
			if(Input.touches[0].deltaPosition.x<-15f)
			{
				if(this.friendsSliderDisplayed || this.mainContentDisplayed || this.toSlideLeft)
				{
					this.slideRight();
				}
			}
			if(Input.touches[0].deltaPosition.x>15f)
			{
				if(this.mainContentDisplayed || this.resultsSliderDisplayed || this.toSlideRight)
				{
					this.slideLeft();
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
				}
			}
			sceneCameraPosition.x=camerasXPosition;
			this.sceneCamera.transform.position=sceneCameraPosition;
		}
		if (this.isMyProfile && friendsCheckTimer > friendsRefreshInterval && this.isSceneLoaded) 
		{
			this.checkFriendsOnlineStatus();
		}
		if(isSearchingUsers)
		{
			if(!Input.GetKey(KeyCode.Delete))
			{
				foreach (char c in Input.inputString) 
				{
					if(c==(char)KeyCode.Backspace && this.searchValue.Length>0)
					{
						this.searchValue = this.searchValue.Remove(this.searchValue.Length - 1);
						this.searchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text = this.searchValue;
						if(this.searchValue.Length==0)
						{
							this.isSearchingUsers=false;
							this.searchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text ="Entrez un pseudo";
						}
					}
					else if (c == "\b"[0])
					{
						if (searchValue.Length != 0)
						{
							searchValue= searchValue.Substring(0, searchValue.Length - 1);
						}
					}
					else
					{
						if (c == "\n"[0] || c == "\r"[0])
						{
							this.searchUsersHandler();	
						}
						else if(this.searchValue.Length<12)
						{
							this.searchValue += c;
							this.searchValue=this.searchValue.ToLower();
							this.searchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text = this.searchValue;
						}
					}
				}
			}
			if((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))&& !this.isMouseOnSearchBar)
			{
				this.isSearchingUsers=false;
				if(this.searchValue=="")
				{
					this.searchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text ="Entrez un pseudo";
				}
			}
		}
	}
	void Awake()
	{
		instance = this;
		this.model = new NewProfileModel ();
		if(ApplicationModel.profileChosen==""|| ApplicationModel.profileChosen==ApplicationModel.username)
		{
			this.isMyProfile=true;
			this.profileChosen=ApplicationModel.username;
		}
		else
		{
			this.isMyProfile=false;
			this.profileChosen=ApplicationModel.profileChosen;
			ApplicationModel.profileChosen="";
		}
		this.activeResultsTab = 0;
		this.activeFriendsTab = 0;
		this.searchValue = "";
		this.friendsOnline = new List<int> ();
		this.initializeScene ();
		this.mainContentDisplayed = true;
		this.startMenuInitialization ();
	}
	private void startMenuInitialization()
	{
		this.menu = GameObject.Find ("Menu");
		this.menu.AddComponent<ProfileMenuController> ();
	}
	public void endMenuInitialization()
	{
		this.startTutorialInitialization ();
	}
	private void startTutorialInitialization()
	{
		this.tutorial = GameObject.Find ("Tutorial");
		this.tutorial.AddComponent<ProfileTutorialController>();
	}
	public void endTutorialInitialization()
	{
		StartCoroutine(this.initialization ());
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
		MenuController.instance.hideLoadingScreen ();
		this.isSceneLoaded = true;
		if(model.tutorialStep!=-1)
		{
			TutorialObjectController.instance.startTutorial(model.tutorialStep,model.displayTutorial);
		}
		else if(model.displayTutorial&&!model.profileTutorial)
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
				this.resultsTabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(1);
				this.resultsTabs[i].GetComponent<NewProfileResultsTabController>().setIsSelected(true);
				this.resultsTabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			}
			else
			{
				this.resultsTabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(0);
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
				this.friendsTabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(1);
				this.friendsTabs[i].GetComponent<NewProfileFriendsTabController>().setIsSelected(true);
				this.friendsTabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			}
			else
			{
				this.friendsTabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(0);
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
		this.profileEditPictureButton = GameObject.Find ("EditProfilePicture");
		this.profileEditPictureButton.AddComponent<NewProfileEditPictureButtonController> ();
		this.profileEditPictureButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Modifier";
		this.profileEditPictureButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.profileEditPictureButton.SetActive (false);
		this.cleanCardsButton = GameObject.Find ("CleanCardsButton");
		this.cleanCardsButton.AddComponent<NewProfileCleanCardsButtonController> ();
		this.cleanCardsButton.GetComponent<TextMeshPro> ().text = "- Vider -";
		if(isMyProfile&&ApplicationModel.isAdmin)
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
		this.profileStats = new GameObject[4];
		for(int i=0;i<this.profileStats.Length;i++)
		{
			this.profileStats[i]=GameObject.Find ("ProfileStat"+i);
			this.profileStats[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			// A compléter !
		}
		this.profileStats[0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= "Victoires";
		this.profileStats[0].transform.FindChild ("Subvalue").gameObject.SetActive (false);
		this.profileStats[1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= "Défaites";
		this.profileStats[1].transform.FindChild ("Subvalue").gameObject.SetActive (false);
		this.profileStats[2].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= "Classement combattant";
		this.profileStats[3].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= "Classement collectionneur";
		this.profileLine = GameObject.Find ("ProfileLine");

		this.searchBlock = Instantiate(this.blockObject)as GameObject;
		this.searchBlockTitle = GameObject.Find ("SearchTitle");
		this.searchBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.searchBlockTitle.GetComponent<TextMeshPro>().text="Rechercher";
		this.searchSubtitle=GameObject.Find ("SearchSubtitle");
		this.searchSubtitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.searchSubtitle.GetComponent<TextMeshPro>().text="Trouver un ami à l'aide de son pseudo".ToUpper();
		this.searchBar=GameObject.Find ("SearchBar");
		this.searchBar.AddComponent<NewProfileSearchBarController>();
		this.searchBar.GetComponent<NewProfileSearchBarController>().setText("Entrez un pseudo");
		this.searchButton = GameObject.Find ("SearchButton");
		this.searchButton.AddComponent<NewProfileSearchButtonController> ();
		this.searchButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Ok";


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
		this.resultsTabs[0].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Conquêtes");
		if(this.isMyProfile)
		{
			this.resultsTabs[1].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Défis");
		}
		else
		{
			this.resultsTabs[1].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Confrontations");
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
		this.friendsTabs[0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = ("Amis");
		this.friendsTabs[1].transform.FindChild("Title").GetComponent<TextMeshPro>().text=("Invitations");
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
		this.tutorialCamera.transform.position = ApplicationDesignRules.tutorialCameraPositiion;
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
			this.friendsContents[i].transform.FindChild("username").GetComponent<TextMeshPro>().textContainer.width=(friendsWidth/2f)-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.friendsContents[i].transform.FindChild("username").localPosition=new Vector3(-friendsWidth/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,friendsHeight-(friendsHeight-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.friendsContents[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.friendsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=0.75f*friendsWidth-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.friendsContents[i].transform.FindChild("description").localPosition=new Vector3(-friendsWidth/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,friendsHeight/2f,0f);
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
			this.resultsContents[i].transform.FindChild("title").GetComponent<TextMeshPro>().textContainer.width=(resultsWidth/2f)-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.resultsContents[i].transform.FindChild("title").localPosition=new Vector3(-resultsWidth/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,resultsHeight-(resultsHeight-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.resultsContents[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=resultsWidth-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.resultsContents[i].transform.FindChild("description").localPosition=new Vector3(-resultsWidth/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,resultsHeight/2f,0f);
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

			this.profileEditInformationsButton.transform.position = new Vector3 (profileBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing + ApplicationDesignRules.roundButtonWorldSize.x / 2f, profileBlockUpperLeftPosition.y - 2.5f, 0f);
			this.profileEditPasswordButton.transform.position  = new Vector3 (profileBlockUpperLeftPosition.x +ApplicationDesignRules.blockHorizontalSpacing + 1.5f*ApplicationDesignRules.roundButtonWorldSize.x, profileBlockUpperLeftPosition.y - 2.5f, 0f);
			this.profileLine.transform.position = new Vector3 (profileBlockLowerLeftPosition.x+profileBlockSize.x/2f, profileBlockUpperLeftPosition.y - 3f, 0f);

			for(int i=0;i<this.friendshipStatusButtons.Length;i++)
			{
				this.friendshipStatusButtons[i].transform.position = new Vector3 (profileBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing + ApplicationDesignRules.profilePictureWorldSize.x+0.3f+ApplicationDesignRules.button61WorldSize.x/2f, profileBlockUpperLeftPosition.y - 0.2f - ApplicationDesignRules.profilePictureWorldSize.y + ApplicationDesignRules.button61WorldSize.y / 2f-i*(ApplicationDesignRules.button61WorldSize.y), 0f);
				this.friendshipStatusButtons[i].transform.localScale = ApplicationDesignRules.button62Scale;
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

			this.profileEditInformationsButton.transform.position = new Vector3 (profileBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f, profileBlockUpperLeftPosition.y - ApplicationDesignRules.buttonVerticalSpacing -ApplicationDesignRules.profilePictureWorldSize.y + ApplicationDesignRules.roundButtonWorldSize.y/2f+ApplicationDesignRules.roundButtonWorldSize.y, 0f);
			this.profileEditPasswordButton.transform.position  = new Vector3 (profileBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f, profileBlockUpperLeftPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.profilePictureWorldSize.y+ ApplicationDesignRules.roundButtonWorldSize.y/2f, 0f);
			this.profileLine.transform.position = new Vector3 (profileBlockLowerLeftPosition.x+profileBlockSize.x/2f, profileBlockUpperLeftPosition.y - 0.5f - ApplicationDesignRules.profilePictureWorldSize.y, 0f);

			this.resultsPaginationButtons.transform.position=new Vector3 (resultsBlockLowerLeftPosition.x + resultsBlockSize.x / 2, resultsBlockLowerLeftPosition.y + 0.3f, 0f);

			for(int i=0;i<this.friendshipStatusButtons.Length;i++)
			{
				this.friendshipStatusButtons[i].transform.position = new Vector3 (profileBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing + ApplicationDesignRules.profilePictureWorldSize.x+0.3f+ApplicationDesignRules.button61WorldSize.x/2f+i*(ApplicationDesignRules.button61WorldSize.x+0.05f), profileBlockUpperLeftPosition.y - 0.2f - ApplicationDesignRules.profilePictureWorldSize.y + ApplicationDesignRules.button61WorldSize.y / 2f, 0f);
				this.friendshipStatusButtons[i].transform.localScale = ApplicationDesignRules.button62Scale;
			}
		}

		TutorialObjectController.instance.resize ();

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
	}
	public void returnPressed()
	{
		if(this.isCheckPasswordPopUpDisplayed)
		{
			this.checkPasswordPopUp.transform.GetComponent<CheckPasswordPopUpController>().checkPasswordHandler();
		}
		else if(this.isChangePasswordPopUpDisplayed)
		{
			this.editPasswordHandler();
		}
		else if(this.isEditInformationsPopUpDisplayed)
		{
			this.updateUserInformationsHandler();
		}
	}
	public void escapePressed()
	{
		if(this.isSelectPicturePopUpDisplayed)
		{
			this.hideSelectPicturePopUp();
		}
		else if(this.isCheckPasswordPopUpDisplayed)
		{
			this.hideCheckPasswordPopUp();
		}
		else if(this.isChangePasswordPopUpDisplayed)
		{
			this.hideChangePasswordPopUp();
		}
		else if(this.isEditInformationsPopUpDisplayed)
		{
			this.hideEditInformationsPopUp();
		}
		else if(this.isSearchUsersPopUpDisplayed)
		{
			this.hideSearchUsersPopUp();
		}
		else
		{
			MenuController.instance.leaveGame();
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
				this.resultsContents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnThumbPicture(model.challengesRecords[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].Friend.idProfilePicture);
				int nbWins = model.challengesRecords[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].NbWins;
				int nbLooses = model.challengesRecords[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].NbLooses;
				if(nbWins>nbLooses)
				{
					this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro> ().text = "Votre ami mène avec "+nbWins+" Victoire(s) contre "+nbLooses+" défaite(s)";
				}
				else if(nbLooses > nbWins)
				{
					this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro> ().text = "Vous menez avec "+nbLooses+" Victoire(s) contre "+nbWins+" défaite(s)";
				}
				else
				{
					this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro> ().text = "Ex eaquo ! vous avez gagné chacun "+nbLooses+" victoire(s)";
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
					this.friendsContents[i].transform.FindChild ("description").GetComponent<TextMeshPro> ().text = "vous a invité à devenir son ami, accepter ?";
					friendsStatusButtonPosition=this.friendsStatusButtons[2*i].transform.position;
					friendsStatusButtonPosition.y=0.2f+this.challengesButtons[i].transform.position.y;
					this.friendsStatusButtons[2*i].SetActive(true);
					this.friendsStatusButtons[2*i].transform.position=friendsStatusButtonPosition;
					this.friendsStatusButtons[2*i].GetComponent<NewProfileFriendsStatusButtonController>().setId(i);
					this.friendsStatusButtons[2*i].GetComponent<NewProfileFriendsStatusButtonController>().setToAcceptInvitation();
					this.friendsStatusButtons[2*i].transform.FindChild("Title").GetComponent<TextMeshPro>().text="Oui";
					friendsStatusButtonPosition=this.friendsStatusButtons[2*i+1].transform.position;
					friendsStatusButtonPosition.y=-0.2f+this.challengesButtons[i].transform.position.y;
					this.friendsStatusButtons[2*i+1].transform.position=friendsStatusButtonPosition;
					this.friendsStatusButtons[2*i+1].SetActive(true);
					this.friendsStatusButtons[2*i+1].GetComponent<NewProfileFriendsStatusButtonController>().setId(i);
					this.friendsStatusButtons[2*i+1].GetComponent<NewProfileFriendsStatusButtonController>().setToDeclineInvitation();
					this.friendsStatusButtons[2*i+1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="Non";
				}
				else
				{
					this.friendsContents[i].transform.FindChild ("description").GetComponent<TextMeshPro> ().text = "n'a pas encore répondu à votre invitation, annuler ?";
					friendsStatusButtonPosition=this.friendsStatusButtons[2*i].transform.position;
					friendsStatusButtonPosition.y=0f+this.challengesButtons[i].transform.position.y;
					this.friendsStatusButtons[2*i].transform.position=friendsStatusButtonPosition;
					this.friendsStatusButtons[2*i].SetActive(true);
					this.friendsStatusButtons[2*i].GetComponent<NewProfileFriendsStatusButtonController>().setId(i);
					this.friendsStatusButtons[2*i].GetComponent<NewProfileFriendsStatusButtonController>().setToCancelInvitation();
					this.friendsStatusButtons[2*i].transform.FindChild("Title").GetComponent<TextMeshPro>().text="Oui";
					this.friendsStatusButtons[2*i+1].SetActive(false);
				}
				this.friendsContents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnThumbPicture(model.friendsRequests[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i].User.idProfilePicture);
				this.friendsContents[i].transform.FindChild("username").GetComponent<TextMeshPro>().text=model.friendsRequests[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i].User.Username;
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
				this.friendsContents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnThumbPicture(model.users[this.friendsToBeDisplayed[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i]].idProfilePicture);
				this.friendsContents[i].transform.FindChild("username").GetComponent<TextMeshPro>().text=model.users[this.friendsToBeDisplayed[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i]].Username;
				this.friendsContents[i].SetActive(true);
				string connectionState="";
				Color connectionStateColor=new Color();
				switch(model.users[this.friendsToBeDisplayed[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i]].OnlineStatus)
				{
				case 0:
					connectionState = "n'est pas en ligne";
					connectionStateColor=ApplicationDesignRules.whiteTextColor;
					this.challengesButtons[i].SetActive(false);
					break;
				case 1:
					connectionState = "est disponible pour un défi !";
					connectionStateColor=ApplicationDesignRules.blueColor;
					this.challengesButtons[i].SetActive(true);
					break;
				case 2:
					connectionState = "est entrain de jouer";
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
				this.resultsContents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnCompetitionPicture(model.trophies[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].competition.IdPicture);
				this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text="Hégémonie atteinte le "+model.trophies[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].Date.ToString("dd/MM/yyyy");
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
					gameTypeName="Match de division";
					idCompetitionPicture=1;
					break;
				case 2:
					gameTypeName="Match de coupe";
					idCompetitionPicture=2;
					break;
				default:
					gameTypeName="Match amical";
					idCompetitionPicture=0;
					break;
				}
				this.resultsContents[i].transform.FindChild("title").GetComponent<TextMeshPro>().text=gameTypeName;
				print (model.confrontations[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].GameType);
				this.resultsContents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnCompetitionPicture(idCompetitionPicture);
				string description="";
				Color textColor=new Color();
				if(model.confrontations[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].IdWinner==model.activePlayerId)
				{
					description="Victoire le "+model.confrontations[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].Date.ToString("dd/MM/yyyy");
					textColor=ApplicationDesignRules.blueColor;
				}
				else
				{
					description="Défaite le "+model.confrontations[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].Date.ToString("dd/MM/yyyy");
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

		this.profileStats[0].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.player.TotalNbWins.ToString ();
		this.profileStats[1].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.player.TotalNbLooses.ToString ();
		this.profileStats[2].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.player.Ranking.ToString ();
		this.profileStats[2].transform.FindChild ("Subvalue").GetComponent<TextMeshPro> ().text= "("+model.player.RankingPoints.ToString()+" pts)";
		this.profileStats[3].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.player.CollectionRanking.ToString ();
		this.profileStats[3].transform.FindChild ("Subvalue").GetComponent<TextMeshPro> ().text= "("+model.player.CollectionPoints.ToString()+" pts)";
		this.profileBlockTitle.GetComponent<TextMeshPro> ().text = model.player.Username;
		if(this.isMyProfile)
		{
			this.drawPersonalInformations ();
		}
		this.drawProfilePicture ();
	}
	private void drawPersonalInformations()
	{
		this.profileInformations [0].GetComponent<TextMeshPro> ().text = "prénom : "+model.player.FirstName;
		this.profileInformations [1].GetComponent<TextMeshPro> ().text = "nom : "+model.player.Surname;
		this.profileInformations [2].GetComponent<TextMeshPro> ().text = "mail : "+model.player.Mail;
	}
	private void drawProfilePicture()
	{
		this.profilePicture.GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnLargeProfilePicture(model.player.idProfilePicture);
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
		if(!model.hasDeck)
		{
			MenuController.instance.displayErrorPopUp("Vous ne pouvez lancer de match sans avoir au préalable créé un deck");
		}
		else if(model.users [this.friendsToBeDisplayed[this.friendsDisplayed[id]]].OnlineStatus!=1)
		{
			MenuController.instance.displayErrorPopUp("Votre adversaire n'est plus disponible");
		}
		else
		{
			StartCoroutine (this.sendInvitation (id));
		}
	}
	public IEnumerator sendInvitation(int id)
	{
		MenuController.instance.displayLoadingScreen ();
		// yield return StartCoroutine (model.player.SetSelectedDeck (model.decks [this.deckDisplayed].Id));
		StartCoroutine (MenuController.instance.sendInvitation (model.users [this.friendsToBeDisplayed[this.friendsDisplayed[id]]], model.player));
		yield break;
	}
	public void acceptFriendsRequestHandler(int id)
	{
		StartCoroutine (this.confirmFriendRequest (id));
	}
	public void declineFriendsRequestHandler(int id)
	{
		StartCoroutine (this.removeFriendRequest (id));
	}
	public void cancelFriendsRequestHandler(int id)
	{
		StartCoroutine (this.removeFriendRequest (id));
	}
	public void startHoveringProfilePicture()
	{
		if(this.isMyProfile)
		{
			this.isProfilePictureHovered = true;
			this.profileEditPictureButton.SetActive (true);
		}	
	}
	public void endHoveringProfilePicture()
	{
		if(this.isMyProfile)
		{
			this.isProfilePictureHovered = false;
			this.profileEditPictureButton.SetActive (false);
		}
	}
	public void editProfilePictureHandler()
	{
		this.displaySelectPicturePopUp ();
	}
	private void displaySelectPicturePopUp()
	{
		MenuController.instance.displayTransparentBackground ();
		this.selectPicturePopUp.SetActive (true);
		this.selectPicturePopUp.GetComponent<SelectPicturePopUpController> ().selectPicture (model.player.idProfilePicture);
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
		MenuController.instance.hideTransparentBackground ();
		this.isSelectPicturePopUpDisplayed = false;
	}
	public void changeUserPictureHandler(int id)
	{
		this.hideSelectPicturePopUp ();
		if(id!=model.player.idProfilePicture)
		{
			StartCoroutine(this.changeUserPicture(id));
		}
	}
	public IEnumerator changeUserPicture(int id)
	{
		MenuController.instance.displayLoadingScreen();
		yield return StartCoroutine(model.player.setProfilePicture(id));
		this.drawProfilePicture ();
		MenuController.instance.changeThumbPicture (id);
		MenuController.instance.hideLoadingScreen();
	}
	public void displayCheckPasswordPopUp()
	{
		MenuController.instance.displayTransparentBackground ();
		this.checkPasswordPopUp.transform.GetComponent<CheckPasswordPopUpController> ().reset ();
		this.isCheckPasswordPopUpDisplayed = true;
		this.checkPasswordPopUp.SetActive (true);
		this.checkPasswordPopUpResize ();
	}
	public void displayChangePasswordPopUp()
	{
		MenuController.instance.displayTransparentBackground ();
		this.changePasswordPopUp.transform.GetComponent<ChangePasswordPopUpController> ().reset ();
		this.isChangePasswordPopUpDisplayed = true;
		this.changePasswordPopUp.SetActive (true);
		this.changePasswordPopUpResize ();
	}
	public void displayEditInformationsPopUp()
	{
		MenuController.instance.displayTransparentBackground ();
		this.editInformationsPopUp.transform.GetComponent<EditInformationsPopUpController> ().reset (model.player.FirstName,model.player.Surname,model.player.Mail);
		this.isEditInformationsPopUpDisplayed = true;
		this.editInformationsPopUp.SetActive (true);
		this.editInformationsPopUpResize ();
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
	public void hideCheckPasswordPopUp()
	{
		this.checkPasswordPopUp.SetActive (false);
		MenuController.instance.hideTransparentBackground();
		this.isCheckPasswordPopUpDisplayed = false;
	}
	public void hideChangePasswordPopUp()
	{
		this.changePasswordPopUp.SetActive (false);
		MenuController.instance.hideTransparentBackground();
		this.isChangePasswordPopUpDisplayed = false;
	}
	public void hideEditInformationsPopUp()
	{
		this.editInformationsPopUp.SetActive (false);
		MenuController.instance.hideTransparentBackground();
		this.isEditInformationsPopUpDisplayed = false;
	}
	public void checkPasswordHandler(string password)
	{
		StartCoroutine (checkPassword (password));
	}
	private IEnumerator checkPassword(string password)
	{
		string checkPassword = this.checkPasswordComplexity (password);
		if(checkPassword=="")
		{
			MenuController.instance.displayLoadingScreen();
			this.checkPasswordPopUp.SetActive(false);
			yield return StartCoroutine(ApplicationModel.checkPassword(password));
			MenuController.instance.hideLoadingScreen();
			this.checkPasswordPopUp.SetActive(true);
			if(ApplicationModel.error=="")
			{
				this.hideCheckPasswordPopUp();
				this.displayChangePasswordPopUp();
			}
			else
			{
				this.checkPasswordPopUp.GetComponent<CheckPasswordPopUpController>().setError(ApplicationModel.error);
				ApplicationModel.error="";
			}
		}
		else
		{
			this.checkPasswordPopUp.GetComponent<CheckPasswordPopUpController>().setError(checkPassword);
		}
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
				StartCoroutine(this.editPassword(firstPassword));
			}
		}
		this.changePasswordPopUp.transform.GetComponent<ChangePasswordPopUpController> ().setError (passwordCheck);

	}
	private IEnumerator editPassword(string password)
	{
		this.hideChangePasswordPopUp ();
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(ApplicationModel.editPassword(password));
		MenuController.instance.hideLoadingScreen ();
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
					StartCoroutine(updateUserInformations(firstname,surname,mail));
				}
			}
		}
		this.editInformationsPopUp.transform.GetComponent<EditInformationsPopUpController> ().setError (error);
	}
	private IEnumerator updateUserInformations(string firstname, string surname, string mail)
	{
		model.player.FirstName = firstname;
		model.player.Surname = surname;
		model.player.Mail = mail;
		this.hideEditInformationsPopUp ();
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.player.updateInformations ());
		MenuController.instance.hideLoadingScreen ();
		this.drawPersonalInformations ();
	}
	public string checkname(string name)
	{
		if(!Regex.IsMatch(name, @"^[a-zA-Z0-9_]+$"))
		{
			return "Vous ne pouvez pas utiliser de caractères spéciaux";
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
	public void mouseOnSearchBar(bool value)
	{
		this.isMouseOnSearchBar = value;
	}
	public void searchingUsers()
	{
		if(this.searchValue=="")
		{
			this.isSearchingUsers = true;
			this.searchBar.transform.FindChild("Title").GetComponent<TextMeshPro>().text = this.searchValue;
		}
	}
	public void searchUsersHandler()
	{
		if(this.searchValue.Length>2)
		{
			this.isSearchingUsers = false;
			this.displaySearchUsersPopUp(this.searchValue);
			this.searchValue = "";
			this.searchBar.transform.FindChild("Title").GetComponent<TextMeshPro>().text ="Entrez un pseudo";
		}
	}
	private void displaySearchUsersPopUp(string searchValue)
	{
		MenuController.instance.displayTransparentBackground ();
		this.searchUsersPopUp.SetActive (true);
		StartCoroutine(this.searchUsersPopUp.GetComponent<SearchUsersPopUpController> ().initialization (searchValue));
		this.isSearchUsersPopUpDisplayed = true;
	}
	public void hideSearchUsersPopUp()
	{
		this.searchUsersPopUp.SetActive (false);
		MenuController.instance.hideTransparentBackground ();
		this.isSearchUsersPopUpDisplayed = false;
	}
	public IEnumerator confirmFriendRequest(int id)
	{
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.confirm ());
		if(model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.Error=="")
		{
			model.moveToFriend(this.friendsRequestsDisplayed[id]);
			this.initializeFriendsRequests();
		}
		else
		{
			MenuController.instance.displayErrorPopUp(model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.Error);
			model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.Error="";
		}
		MenuController.instance.hideLoadingScreen ();
	}
	public IEnumerator confirmConnection()
	{
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.connectionWithMe.confirm ());
		if(model.connectionWithMe.Error=="")
		{
			this.initializeFriendshipState();
			this.initializeFriends();
		}
		else
		{
			MenuController.instance.displayErrorPopUp(model.connectionWithMe.Error);
			model.connectionWithMe.Error="";
		}
		MenuController.instance.hideLoadingScreen ();
	}
	public IEnumerator removeFriendRequest(int id)
	{
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.remove ());
		if(model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.Error=="")
		{
			model.friendsRequests.RemoveAt(this.friendsRequestsDisplayed[id]);
			this.initializeFriendsRequests();
		}
		else
		{
			MenuController.instance.displayErrorPopUp(model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.Error);
			model.friendsRequests [this.friendsRequestsDisplayed[id]].Connection.Error="";
		}
		MenuController.instance.hideLoadingScreen ();
	}
	public IEnumerator removeConnection()
	{
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.connectionWithMe.remove ());
		if(model.connectionWithMe.Error=="")
		{
			model.isConnectedToMe=false;
			this.initializeFriends();
			this.initializeFriendshipState();
		}
		else
		{
			MenuController.instance.displayErrorPopUp(model.connectionWithMe.Error);
			model.connectionWithMe.Error="";
		}
		MenuController.instance.hideLoadingScreen ();
	}
	public IEnumerator addConnection()
	{
		MenuController.instance.displayLoadingScreen ();
		Connection connection = new Connection ();
		connection.IdUser1 = model.activePlayerId;
		connection.IdUser2 = model.player.Id;
		connection.IsAccepted = false;

		yield return StartCoroutine(connection.add ());
		if(connection.Error=="")
		{
			model.isConnectedToMe=true;
			model.connectionWithMe=connection;
			this.initializeFriendshipState();
		}
		else
		{
			MenuController.instance.displayErrorPopUp(connection.Error);
			connection.Error="";
		}
		MenuController.instance.hideLoadingScreen ();
	}
	private void drawFriendshipState()
	{
		if(model.isConnectedToMe)
		{
			if(model.connectionWithMe.IsAccepted)
			{
				this.friendshipStatusButtons[0].SetActive(true);
				this.friendshipStatusButtons[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="Retirer";
				this.friendshipStatusButtons[1].SetActive(false);
				this.friendshipStatus.GetComponent<TextMeshPro>().text="Vous êtes amis";
			}
			else if(model.connectionWithMe.IdUser1==model.player.Id)
			{
				this.friendshipStatusButtons[0].SetActive(true);
				this.friendshipStatusButtons[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="Accepter";
				this.friendshipStatusButtons[1].SetActive(true);
				this.friendshipStatusButtons[1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="Refuser";
				this.friendshipStatus.GetComponent<TextMeshPro>().text="Souhaite faire parti de vos amis";
			}
			else if(model.connectionWithMe.IdUser1==model.activePlayerId)
			{
				this.friendshipStatusButtons[0].SetActive(true);
				this.friendshipStatusButtons[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="Annuler";
				this.friendshipStatusButtons[1].SetActive(false);
				this.friendshipStatus.GetComponent<TextMeshPro>().text="n'a pas encore répondu à votre invitation";
			}
		}
		else
		{
			this.friendshipStatusButtons[0].SetActive(true);
			this.friendshipStatusButtons[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="Ajouter";
			this.friendshipStatusButtons[1].SetActive(false);
			this.friendshipStatus.GetComponent<TextMeshPro>().text="ne fait pas partie de vos amis";
		}
	}
	public void friendshipStateHandler(int buttonId)
	{
		if(model.isConnectedToMe)
		{
			if(model.connectionWithMe.IsAccepted)
			{
				StartCoroutine(this.removeConnection());
			}
			else if(model.connectionWithMe.IdUser1==model.player.Id)
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
			else if(model.connectionWithMe.IdUser1==model.activePlayerId)
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
		StartCoroutine (this.cleanCards ());
	}
	public IEnumerator cleanCards()
	{
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.player.cleanCards ());
		MenuController.instance.hideLoadingScreen ();
	}
	public void clickOnFriendsContentProfile(int id)
	{
		ApplicationModel.profileChosen = this.friendsContents [id].transform.FindChild ("username").GetComponent<TextMeshPro> ().text;
		Application.LoadLevel("NewProfile");
	}
	public void clickOnResultsContentProfile(int id)
	{
		ApplicationModel.profileChosen = this.resultsContents [id].transform.FindChild ("title").GetComponent<TextMeshPro> ().text;
		Application.LoadLevel("NewProfile");
	}
	public void slideRight()
	{
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
		if(this.mainContentDisplayed)
		{
			this.mainContentDisplayed=false;
			this.targetContentPositionX=this.friendsPositionX;
		}
		else if(this.resultsSliderDisplayed)
		{
			this.friendsSliderDisplayed=false;
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
			this.challengesButtons[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text="Défier";
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
		if(!model.profileTutorial)
		{
			MenuController.instance.displayLoadingScreen();
			yield return StartCoroutine(model.player.setProfileTutorial(true));
			MenuController.instance.hideLoadingScreen();
		}
	}
	#endregion
}