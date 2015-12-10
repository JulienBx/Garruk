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
	public GUISkin popUpSkin;

	private GameObject menu;
	private GameObject tutorial;

	private GameObject profileBlock;
	private GameObject profileBlockTitle;
	private GameObject collectionButton;
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
	private GameObject[] resultsContents;
	private GameObject[] resultsTabs;
	private GameObject resultsPaginationButtons;

	private GameObject friendsBlock;
	private GameObject[] friendsContents;
	private GameObject[] friendsTabs;
	private GameObject[] friendsStatusButtons;
	private GameObject[] challengesButtons;
	private GameObject friendsPaginationButtons;
	
	private GameObject selectPicturePopUp;
	private GameObject searchUsersPopUp;

	private GameObject mainCamera;
	private GameObject menuCamera;
	private GameObject tutorialCamera;
	private GameObject backgroundCamera;

	private bool isMyProfile;
	private string profileChosen;
	
	private Rect centralWindow;
	private Rect centralWindowEditInformations;
	
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

	private ProfileCheckPasswordPopUpView checkPasswordView;
	private bool isCheckPasswordViewDisplayed;

	private ProfileChangePasswordPopUpView changePasswordView;
	private bool isChangePasswordViewDisplayed;

	private ProfileEditInformationsPopUpView editInformationsView;
	private bool isEditInformationsViewDisplayed;

	private bool isSearchingUsers;
	private string searchValue;
	private bool isMouseOnSearchBar;
	private bool isScrolling;

	void Update()
	{	
		this.friendsCheckTimer += Time.deltaTime;
		
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
		if(ApplicationDesignRules.isMobileScreen && this.isSceneLoaded)
		{
			isScrolling = this.mainCamera.GetComponent<ScrollingController>().ScrollController();
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
		this.resultsPagination = new Pagination ();
		this.resultsPagination.chosenPage = 0;
		this.resultsPagination.nbElementsPerPage = 2;
		this.activeResultsTab = 0;
		this.friendsPagination = new Pagination ();
		this.friendsPagination.chosenPage = 0;
		this.friendsPagination.nbElementsPerPage = 3;
		this.activeFriendsTab = 0;
		this.searchValue = "";
		this.friendsOnline = new List<int> ();
		this.initializeScene ();
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
		this.collectionButton = GameObject.Find ("CollectionButton");
		this.collectionButton.AddComponent<NewProfileCollectionButtonController> ();
		this.collectionButton.GetComponent<TextMeshPro> ().text = "- Détails -";
		this.collectionButton.SetActive (this.isMyProfile);
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
		this.profileEditInformationsButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Modifier mon profil".ToUpper ();
		this.profileEditInformationsButton.SetActive (this.isMyProfile);
		this.profileEditPasswordButton = GameObject.Find ("ProfileEditPasswordButton");
		this.profileEditPasswordButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "reset password".ToUpper ();
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
		this.resultsContents = new GameObject[this.resultsPagination.nbElementsPerPage];
		for(int i=0;i<this.resultsContents.Length;i++)
		{
			this.resultsContents[i]=GameObject.Find("ResultsContent"+i);
			this.resultsContents[i].transform.FindChild("picture").gameObject.AddComponent<NewProfileResultsContentPictureController>();
			this.resultsContents[i].transform.FindChild("picture").GetComponent<NewProfileResultsContentPictureController>().setId(i);
			this.resultsContents[i].transform.FindChild("title").gameObject.AddComponent<NewProfileResultsContentUsernameController>();
			this.resultsContents[i].transform.FindChild("title").GetComponent<NewProfileResultsContentUsernameController>().setId(i); 
		}

		this.friendsBlock = Instantiate (this.blockObject) as GameObject;
		this.friendsTabs=new GameObject[2];
		for(int i=0;i<this.friendsTabs.Length;i++)
		{
			this.friendsTabs[i]=GameObject.Find("FriendsTab"+i);
			this.friendsTabs[i].AddComponent<NewProfileFriendsTabController>();
			this.friendsTabs[i].GetComponent<NewProfileFriendsTabController>().setId (i);
		}
		this.friendsTabs [0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = ("Amis");
		if(this.isMyProfile)
		{
			this.friendsTabs[1].transform.FindChild("Title").GetComponent<TextMeshPro>().text=("Invitations");
		}
		else
		{
			this.friendsTabs[1].SetActive(false);
		}
		this.friendsPaginationButtons = GameObject.Find("FriendsPagination");
		this.friendsPaginationButtons.AddComponent<NewProfileFriendsPaginationController> ();
		this.friendsPaginationButtons.GetComponent<NewProfileFriendsPaginationController> ().initialize ();
		this.friendsStatusButtons=new GameObject[this.friendsPagination.nbElementsPerPage*2];
		for(int i=0;i<this.friendsStatusButtons.Length;i++)
		{
			this.friendsStatusButtons[i]=GameObject.Find ("FriendsStatusButton"+i);
			this.friendsStatusButtons[i].AddComponent<NewProfileFriendsStatusButtonController>();
			this.friendsStatusButtons[i].SetActive(false);
		}
		this.challengesButtons=new GameObject[this.friendsPagination.nbElementsPerPage];
		for(int i=0;i<this.challengesButtons.Length;i++)
		{
			this.challengesButtons[i]=GameObject.Find ("ChallengeButton"+i);
			this.challengesButtons[i].AddComponent<NewProfileChallengeButtonController>();
			this.challengesButtons[i].GetComponent<NewProfileChallengeButtonController>().setId(i);
			this.challengesButtons[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text="Défier";
			this.challengesButtons[i].SetActive(false);
		}
		this.friendsContents=new GameObject[this.friendsPagination.nbElementsPerPage];
		for(int i=0;i<this.friendsContents.Length;i++)
		{
			this.friendsContents[i]=GameObject.Find ("FriendsContent"+i);
			this.friendsContents[i].transform.FindChild("picture").gameObject.AddComponent<NewProfileFriendsContentPictureController>();
			this.friendsContents[i].transform.FindChild("picture").GetComponent<NewProfileFriendsContentPictureController>().setId(i);
			this.friendsContents[i].transform.FindChild("username").gameObject.AddComponent<NewProfileFriendsContentUsernameController>();
			this.friendsContents[i].transform.FindChild("username").GetComponent<NewProfileFriendsContentUsernameController>().setId(i);
		}
		this.searchUsersPopUp = GameObject.Find ("searchPopUp");
		this.searchUsersPopUp.GetComponent<SearchUsersPopUpController> ().launch ();
		this.searchUsersPopUp.SetActive (false);
		this.selectPicturePopUp = GameObject.Find ("profilePicturesPopUp");
		this.selectPicturePopUp.SetActive (false);
		this.mainCamera = gameObject;
		this.mainCamera.AddComponent<ScrollingController> ();
		this.menuCamera = GameObject.Find ("MenuCamera");
		this.tutorialCamera = GameObject.Find ("TutorialCamera");
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
	}
	public void resize()
	{
		this.mainCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.menuCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.tutorialCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.backgroundCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.backgroundCameraSize;

		float profileBlockLeftMargin;
		float profileBlockUpMargin;
		float profileBlockHeight;
		
		float searchBlockLeftMargin;
		float searchBlockUpMargin;
		float searchBlockHeight;
		
		float friendsBlockLeftMargin;
		float friendsBlockUpMargin;
		float friendsBlockHeight;
		
		float resultsBlockLeftMargin;
		float resultsBlockUpMargin;
		float resultsBlockHeight;
		
		profileBlockHeight=ApplicationDesignRules.mediumBlockHeight;
		searchBlockHeight=ApplicationDesignRules.smallBlockHeight;
		friendsBlockHeight=ApplicationDesignRules.mediumBlockHeight-ApplicationDesignRules.button62WorldSize.y;
		resultsBlockHeight=ApplicationDesignRules.smallBlockHeight-ApplicationDesignRules.button62WorldSize.y;
		
		if(ApplicationDesignRules.isMobileScreen)
		{
			profileBlockLeftMargin=ApplicationDesignRules.leftMargin;
			profileBlockUpMargin=0f;
			
			searchBlockLeftMargin=ApplicationDesignRules.leftMargin;
			searchBlockUpMargin=profileBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+profileBlockHeight;
			
			friendsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			friendsBlockUpMargin=searchBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+searchBlockHeight+ApplicationDesignRules.button62WorldSize.y;
			
			resultsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			resultsBlockUpMargin=friendsBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+friendsBlockHeight+ApplicationDesignRules.button62WorldSize.y;
		}
		else
		{
			profileBlockLeftMargin=ApplicationDesignRules.leftMargin;
			profileBlockUpMargin=ApplicationDesignRules.upMargin;
			
			searchBlockLeftMargin=ApplicationDesignRules.leftMargin;
			searchBlockUpMargin=profileBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+profileBlockHeight;
			
			friendsBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			friendsBlockUpMargin=ApplicationDesignRules.upMargin+ApplicationDesignRules.button62WorldSize.y;
			
			resultsBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			resultsBlockUpMargin=friendsBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+friendsBlockHeight+ApplicationDesignRules.button62WorldSize.y;
		}

		this.mainCamera.GetComponent<ScrollingController> ().setViewHeight(ApplicationDesignRules.viewHeight);
		this.mainCamera.GetComponent<ScrollingController> ().setContentHeight(profileBlockHeight + searchBlockHeight + friendsBlockHeight + resultsBlockHeight + 3f * ApplicationDesignRules.gapBetweenBlocks + 2f*ApplicationDesignRules.button62WorldSize.y);
		this.mainCamera.transform.position = ApplicationDesignRules.mainCameraStartPosition;
		this.mainCamera.GetComponent<ScrollingController> ().setStartPositionY (ApplicationDesignRules.mainCameraStartPosition.y);

		this.centralWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen,ApplicationDesignRules.widthScreen * 0.50f, 0.40f * ApplicationDesignRules.heightScreen);
		this.centralWindowEditInformations=new Rect(ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen,ApplicationDesignRules.widthScreen * 0.50f, 0.50f * ApplicationDesignRules.heightScreen);

		this.profileBlock.GetComponent<NewBlockController> ().resize(profileBlockLeftMargin,profileBlockUpMargin,ApplicationDesignRules.blockWidth,profileBlockHeight);
		Vector3 profileBlockUpperLeftPosition = this.profileBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 profileBlockUpperRightPosition = this.profileBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 profileBlockLowerLeftPosition = this.profileBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 profileBlockLowerRightPosition = this.profileBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 profileBlockSize = this.profileBlock.GetComponent<NewBlockController> ().getSize ();

		this.profilePicture.transform.position = new Vector3 (profileBlockUpperLeftPosition.x + 0.3f + ApplicationDesignRules.profilePictureWorldSize.x / 2f, profileBlockUpperLeftPosition.y - 0.2f - ApplicationDesignRules.profilePictureWorldSize.y / 2f, 0f);
		this.profilePicture.transform.localScale = ApplicationDesignRules.profilePictureScale;

		this.profileBlockTitle.transform.position = new Vector3 (profileBlockUpperLeftPosition.x + 0.3f+ApplicationDesignRules.profilePictureWorldSize.x+0.3f, profileBlockUpperLeftPosition.y - 0.2f, 0f);
		this.profileBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		
		this.profileEditPictureButton.transform.position = new Vector3(this.profilePicture.transform.position.x,this.profilePicture.transform.position.y,-1f);
		this.profileEditPictureButton.transform.localScale = this.profilePicture.transform.localScale;

		this.profileEditInformationsButton.transform.position = new Vector3 (profileBlockUpperRightPosition.x - 0.3f - ApplicationDesignRules.button61WorldSize.x / 2f, profileBlockUpperLeftPosition.y - 0.2f -ApplicationDesignRules.profilePictureWorldSize.y + ApplicationDesignRules.button62WorldSize.y/2f, 0f);
		this.profileEditInformationsButton.transform.localScale = ApplicationDesignRules.button61Scale;
		
		this.profileEditPasswordButton.transform.position  = new Vector3 (profileBlockUpperRightPosition.x - 0.3f - ApplicationDesignRules.button61WorldSize.x / 2f, profileBlockUpperLeftPosition.y - 0.2f - ApplicationDesignRules.profilePictureWorldSize.y+ ApplicationDesignRules.button62WorldSize.y/2f+1f*(ApplicationDesignRules.button62WorldSize.y+0.2f), 0f);
		this.profileEditPasswordButton.transform.localScale = ApplicationDesignRules.button62Scale;

		this.friendshipStatus.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.friendshipStatus.transform.localPosition= new Vector3 (profileBlockUpperLeftPosition.x + 0.3f + ApplicationDesignRules.profilePictureWorldSize.x + 0.3f, profileBlockUpperLeftPosition.y - 1.2f, 0f);

		
		for(int i=0;i<this.friendshipStatusButtons.Length;i++)
		{
			this.friendshipStatusButtons[i].transform.position = new Vector3 (profileBlockUpperLeftPosition.x + 0.3f + ApplicationDesignRules.profilePictureWorldSize.x+0.3f+ApplicationDesignRules.button61WorldSize.x/2f+i*(ApplicationDesignRules.button61WorldSize.x+0.05f), profileBlockUpperLeftPosition.y - 0.2f - ApplicationDesignRules.profilePictureWorldSize.y + ApplicationDesignRules.button61WorldSize.y / 2f, 0f);
			this.friendshipStatusButtons[i].transform.localScale = ApplicationDesignRules.button62Scale;
		}

		float gapBetweenProfileInformations = (ApplicationDesignRules.profilePictureWorldSize.y - 0.7f) / 3f;
		Vector3 profileInformationScale = new Vector3 (1f, 1f, 1f);

		for(int i=0;i<this.profileInformations.Length;i++)
		{
			this.profileInformations[i].transform.position = new Vector3 (profileBlockUpperLeftPosition.x + 0.3f + ApplicationDesignRules.profilePictureWorldSize.x + 0.3f, profileBlockUpperLeftPosition.y - 1f - i*gapBetweenProfileInformations, 0f);
			this.profileInformations[i].transform.localScale = ApplicationDesignRules.reductionRatio*profileInformationScale;
		}

		float profileLineScale = ApplicationDesignRules.getLineScale (profileBlockSize.x-0.6f);
		this.profileLine.transform.localScale = new Vector3 (profileLineScale, 1f, 1f);
		this.profileLine.transform.position = new Vector3 (profileBlockLowerLeftPosition.x+profileBlockSize.x/2f, profileBlockUpperLeftPosition.y - 0.5f - ApplicationDesignRules.profilePictureWorldSize.y, 0f);

		Vector3 profileStatsScale = new Vector3 (1f, 1f, 1f);
		Vector2 profileStatsBlockSize = new Vector2 ((profileBlockSize.x - 0.6f) / 4f, profileBlockSize.y - 0.6f-ApplicationDesignRules.profilePictureWorldSize.y);

		for(int i=0;i<this.profileStats.Length;i++)
		{
			this.profileStats[i].transform.position=new Vector3(profileBlockLowerLeftPosition.x+0.3f+profileStatsBlockSize.x/2f+i*profileStatsBlockSize.x,profileBlockLowerLeftPosition.y+0.2f+profileStatsBlockSize.y/2f);
			this.profileStats[i].transform.localScale= ApplicationDesignRules.reductionRatio*profileStatsScale;
		}

		this.cleanCardsButton.transform.position = new Vector3 (this.profileStats[3].transform.position.x, profileBlockLowerRightPosition.y + ApplicationDesignRules.button61WorldSize.y/2f +0.1f, 0f);
		this.cleanCardsButton.transform.localScale = ApplicationDesignRules.button51Scale;
		
		this.collectionButton.transform.position =  new Vector3 (this.profileStats[3].transform.position.x, profileBlockLowerRightPosition.y + ApplicationDesignRules.button61WorldSize.y/2f+0.3f, 0f);
		this.collectionButton.transform.localScale = ApplicationDesignRules.button51Scale;

		this.searchBlock.GetComponent<NewBlockController> ().resize(searchBlockLeftMargin,searchBlockUpMargin,ApplicationDesignRules.blockWidth,searchBlockHeight);
		Vector3 searchBlockUpperLeftPosition = this.searchBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 searchBlockUpperRightPosition = this.searchBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 searchBlockSize = this.searchBlock.GetComponent<NewBlockController> ().getSize ();
		Vector3 searchOrigin = this.searchBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.searchBlockTitle.transform.position = new Vector3 (searchBlockUpperLeftPosition.x + 0.3f, searchBlockUpperLeftPosition.y - 0.2f, 0f);
		this.searchBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		
		this.searchSubtitle.transform.position = new Vector3 (searchBlockUpperLeftPosition.x + 0.3f, searchBlockUpperLeftPosition.y - 1.2f, 0f);
		this.searchSubtitle.transform.GetComponent<TextContainer>().width=searchBlockSize.x-0.6f;
		this.searchSubtitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		float searchMargin = (searchBlockSize.x - ApplicationDesignRules.largeInputTextWorldSize.x - ApplicationDesignRules.button61WorldSize.x-0.1f) / 2f;

		this.searchBar.transform.position = new Vector3(searchBlockUpperLeftPosition.x+searchMargin+ApplicationDesignRules.largeInputTextWorldSize.x/2f,searchOrigin.y-0.7f,searchOrigin.z);
		this.searchBar.transform.localScale = ApplicationDesignRules.largeInputTextScale;

		this.searchButton.transform.position = new Vector3(searchBlockUpperRightPosition.x-searchMargin-ApplicationDesignRules.button62WorldSize.x/2f,searchOrigin.y-0.7f,searchOrigin.z);
		this.searchButton.transform.localScale = ApplicationDesignRules.largeInputTextScale;

		this.friendsBlock.GetComponent<NewBlockController> ().resize(friendsBlockLeftMargin,friendsBlockUpMargin,ApplicationDesignRules.blockWidth,friendsBlockHeight);
		Vector3 friendsBlockUpperLeftPosition = this.friendsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 friendsBlockUpperRightPosition = this.friendsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 friendsBlockLowerLeftPosition = this.friendsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 friendsBlockLowerRightPosition = this.friendsBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 friendsBlockSize = this.friendsBlock.GetComponent<NewBlockController> ().getSize ();
		
		float gapBetweenFriendsTab = 0.02f;
		for(int i=0;i<this.friendsTabs.Length;i++)
		{
			this.friendsTabs[i].transform.localScale = ApplicationDesignRules.button62Scale;
			this.friendsTabs[i].transform.position = new Vector3 (friendsBlockUpperLeftPosition.x + ApplicationDesignRules.button62WorldSize.x / 2f+ i*(ApplicationDesignRules.button62WorldSize.x+gapBetweenFriendsTab), friendsBlockUpperLeftPosition.y+ApplicationDesignRules.button62WorldSize.y/2f,0f);
		}
		
		Vector2 friendsContentBlockSize = new Vector2 (friendsBlockSize.x - 0.6f, (friendsBlockSize.y - 0.3f - 0.6f)/this.friendsContents.Length);
		float friendsLineScale = ApplicationDesignRules.getLineScale (friendsContentBlockSize.x);
		
		for(int i=0;i<this.friendsContents.Length;i++)
		{
			this.friendsContents[i].transform.position=new Vector3(friendsBlockUpperLeftPosition.x+0.3f+friendsContentBlockSize.x/2f,friendsBlockUpperLeftPosition.y-0.3f-(i+1)*friendsContentBlockSize.y,0f);
			this.friendsContents[i].transform.FindChild("line").localScale=new Vector3(friendsLineScale,1f,1f);
			this.friendsContents[i].transform.FindChild("picture").localScale=ApplicationDesignRules.thumbScale;
			this.friendsContents[i].transform.FindChild("picture").localPosition=new Vector3(-friendsContentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(friendsContentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
			this.friendsContents[i].transform.FindChild("username").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.friendsContents[i].transform.FindChild("username").GetComponent<TextMeshPro>().textContainer.width=(friendsContentBlockSize.x/2f)-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.friendsContents[i].transform.FindChild("username").localPosition=new Vector3(-friendsContentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,friendsContentBlockSize.y-(friendsContentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.friendsContents[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.friendsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=0.75f*friendsContentBlockSize.x-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.friendsContents[i].transform.FindChild("description").localPosition=new Vector3(-friendsContentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,friendsContentBlockSize.y/2f,0f);
			//this.friendsContents[i].transform.FindChild("date").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			//this.friendsContents[i].transform.FindChild("date").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
			//this.friendsContents[i].transform.FindChild("date").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y-(contentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			//this.friendsContents[i].transform.FindChild("new").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			//this.friendsContents[i].transform.FindChild("new").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
			//this.friendsContents[i].transform.FindChild("new").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y/2f,0f);
		}
		
		for(int i=0;i<this.friendsContents.Length;i++)
		{
			this.friendsStatusButtons[i*2].transform.localScale = ApplicationDesignRules.button31Scale;
			this.friendsStatusButtons[i*2+1].transform.localScale = ApplicationDesignRules.button31Scale;
			this.friendsStatusButtons[i*2].transform.position=new Vector3(friendsBlockUpperRightPosition.x-0.3f-ApplicationDesignRules.button31WorldSize.x/2f,friendsBlockUpperRightPosition.y-0.3f-(i+0.5f)*friendsContentBlockSize.y+ApplicationDesignRules.button31WorldSize.y/2f+0.025f,0f);
			this.friendsStatusButtons[i*2+1].transform.position=new Vector3(friendsBlockUpperRightPosition.x-0.3f-ApplicationDesignRules.button31WorldSize.x/2f,friendsBlockUpperRightPosition.y-0.3f-(i+0.5f)*friendsContentBlockSize.y-ApplicationDesignRules.button31WorldSize.y/2f-0.025f,0f);
		}

		for(int i=0;i<this.challengesButtons.Length;i++)
		{
			this.challengesButtons[i].transform.localScale = ApplicationDesignRules.button62Scale;
			this.challengesButtons[i].transform.position=new Vector3(friendsBlockUpperRightPosition.x-0.3f-ApplicationDesignRules.button62WorldSize.x/2f,friendsBlockUpperRightPosition.y-0.3f-(i+0.5f)*friendsContentBlockSize.y,0f);
		}
		
		this.friendsPaginationButtons.transform.position=new Vector3 (friendsBlockLowerLeftPosition.x + friendsBlockSize.x / 2, friendsBlockLowerLeftPosition.y + 0.3f, 0f);
		this.friendsPaginationButtons.GetComponent<NewProfileFriendsPaginationController> ().resize ();

		this.resultsBlock.GetComponent<NewBlockController> ().resize(resultsBlockLeftMargin,resultsBlockUpMargin,ApplicationDesignRules.blockWidth,resultsBlockHeight);
		Vector3 resultsBlockUpperLeftPosition = this.resultsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 resultsBlockUpperRightPosition = this.resultsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 resultsBlockLowerLeftPosition = this.resultsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 resultsBlockLowerRightPosition = this.resultsBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 resultsBlockSize = this.resultsBlock.GetComponent<NewBlockController> ().getSize ();
		
		float gapBetweenResultsTab = 0.02f;
		for(int i=0;i<this.resultsTabs.Length;i++)
		{
			this.resultsTabs[i].transform.localScale = ApplicationDesignRules.button62Scale;
			this.resultsTabs[i].transform.position = new Vector3 (resultsBlockUpperLeftPosition.x + ApplicationDesignRules.button62WorldSize.x / 2f+ i*(ApplicationDesignRules.button62WorldSize.x+gapBetweenResultsTab), resultsBlockUpperLeftPosition.y+ApplicationDesignRules.button62WorldSize.y/2f,0f);
		}
		
		Vector2 resultsContentBlockSize = new Vector2 (resultsBlockSize.x - 0.6f, (resultsBlockSize.y - 0.3f - 0.6f)/this.resultsContents.Length);
		float resultsLineScale = ApplicationDesignRules.getLineScale (resultsContentBlockSize.x);
		
		for(int i=0;i<this.resultsContents.Length;i++)
		{
			this.resultsContents[i].transform.position=new Vector3(resultsBlockUpperLeftPosition.x+0.3f+resultsContentBlockSize.x/2f,resultsBlockUpperLeftPosition.y-0.3f-(i+1)*resultsContentBlockSize.y,0f);
			this.resultsContents[i].transform.FindChild("line").localScale=new Vector3(resultsLineScale,1f,1f);
			this.resultsContents[i].transform.FindChild("picture").localScale=ApplicationDesignRules.thumbScale;
			this.resultsContents[i].transform.FindChild("picture").localPosition=new Vector3(-resultsContentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(resultsContentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
			this.resultsContents[i].transform.FindChild("title").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.resultsContents[i].transform.FindChild("title").GetComponent<TextMeshPro>().textContainer.width=(resultsContentBlockSize.x/2f)-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.resultsContents[i].transform.FindChild("title").localPosition=new Vector3(-resultsContentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,resultsContentBlockSize.y-(resultsContentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.resultsContents[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=resultsContentBlockSize.x-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.resultsContents[i].transform.FindChild("description").localPosition=new Vector3(-resultsContentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,resultsContentBlockSize.y/2f,0f);
			//this.resultsContents[i].transform.FindChild("date").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			//this.resultsContents[i].transform.FindChild("date").GetComponent<TextMeshPro>().textContainer.width=(resultsContentBlockSize.x/4f);
			//this.resultsContents[i].transform.FindChild("date").localPosition=new Vector3(resultsContentBlockSize.x/2f,resultsContentBlockSize.y-(resultsContentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			//this.resultsContents[i].transform.FindChild("new").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			//this.resultsContents[i].transform.FindChild("new").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
			//this.resultsContents[i].transform.FindChild("new").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y/2f,0f);
		}

		this.resultsPaginationButtons.transform.position=new Vector3 (resultsBlockLowerLeftPosition.x + resultsBlockSize.x / 2, resultsBlockLowerLeftPosition.y + 0.3f, 0f);
		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().resize ();

		TutorialObjectController.instance.resize ();

		if(this.isCheckPasswordViewDisplayed)
		{
			this.checkPasswordViewResize();
		}
		else if(this.isChangePasswordViewDisplayed)
		{
			this.changePasswordViewResize();
		}
		else if(this.isEditInformationsViewDisplayed)
		{
			this.editInformationsViewResize();
		}
	}
	public void returnPressed()
	{
		if(this.isCheckPasswordViewDisplayed)
		{
			this.checkPasswordHandler(checkPasswordView.checkPasswordPopUpVM.tempOldPassword);
		}
		else if(this.isChangePasswordViewDisplayed)
		{
			this.editPasswordHandler();
		}
		else if(this.isEditInformationsViewDisplayed)
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
		else if(this.isCheckPasswordViewDisplayed)
		{
			this.hideCheckPasswordPopUp();
		}
		else if(this.isChangePasswordViewDisplayed)
		{
			this.hideChangePasswordPopUp();
		}
		else if(this.isEditInformationsViewDisplayed)
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
		if(this.isCheckPasswordViewDisplayed)
		{
			this.hideCheckPasswordPopUp();
		}
		if(this.isChangePasswordViewDisplayed)
		{
			this.hideChangePasswordPopUp();
		}
		if(this.isEditInformationsViewDisplayed)
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
		this.selectPicturePopUp.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.selectPicturePopUp.GetComponent<SelectPicturePopUpController> ().selectPicture (model.player.idProfilePicture);
		this.isSelectPicturePopUpDisplayed = true;
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
		this.checkPasswordView = gameObject.AddComponent<ProfileCheckPasswordPopUpView> ();
		this.isCheckPasswordViewDisplayed = true;
		checkPasswordView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.window);
		checkPasswordView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
		checkPasswordView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
		checkPasswordView.popUpVM.centralWindowTextfieldStyle = new GUIStyle (this.popUpSkin.textField);
		checkPasswordView.popUpVM.centralWindowErrorStyle = new GUIStyle (this.popUpSkin.customStyles [1]);
		checkPasswordView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
		this.checkPasswordViewResize ();
	}
	public void displayChangePasswordPopUp()
	{
		this.changePasswordView = gameObject.AddComponent<ProfileChangePasswordPopUpView> ();
		this.isChangePasswordViewDisplayed = true;
		changePasswordView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.window);
		changePasswordView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
		changePasswordView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
		changePasswordView.popUpVM.centralWindowTextfieldStyle = new GUIStyle (this.popUpSkin.textField);
		changePasswordView.popUpVM.centralWindowErrorStyle = new GUIStyle (this.popUpSkin.customStyles [1]);
		changePasswordView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
		this.changePasswordViewResize ();
	}
	public void displayEditInformationsPopUp()
	{
		this.editInformationsView = gameObject.AddComponent<ProfileEditInformationsPopUpView> ();
		this.isEditInformationsViewDisplayed = true;
		editInformationsView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.window);
		editInformationsView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
		editInformationsView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
		editInformationsView.popUpVM.centralWindowTextfieldStyle = new GUIStyle (this.popUpSkin.textField);
		editInformationsView.popUpVM.centralWindowErrorStyle = new GUIStyle (this.popUpSkin.customStyles [1]);
		editInformationsView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
		editInformationsView.editInformationsPopUpVM.tempFirstName = model.player.FirstName;
		editInformationsView.editInformationsPopUpVM.tempSurname = model.player.Surname;
		editInformationsView.editInformationsPopUpVM.tempMail = model.player.Mail;
		this.editInformationsViewResize ();
	}
	public void checkPasswordViewResize()
	{
		checkPasswordView.popUpVM.centralWindow = this.centralWindow;
		checkPasswordView.popUpVM.resize ();
	}
	public void changePasswordViewResize()
	{
		changePasswordView.popUpVM.centralWindow = this.centralWindow;
		changePasswordView.popUpVM.resize ();
	}
	public void editInformationsViewResize()
	{
		editInformationsView.popUpVM.centralWindow = this.centralWindowEditInformations;
		editInformationsView.popUpVM.resize ();
	}
	public void hideCheckPasswordPopUp()
	{
		Destroy (this.checkPasswordView);
		this.isCheckPasswordViewDisplayed = false;
	}
	public void hideChangePasswordPopUp()
	{
		Destroy (this.changePasswordView);
		this.isChangePasswordViewDisplayed = false;
	}
	public void hideEditInformationsPopUp()
	{
		Destroy (this.editInformationsView);
		this.isEditInformationsViewDisplayed = false;
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
		editInformationsView.editInformationsPopUpVM.error = this.checkname (editInformationsView.editInformationsPopUpVM.tempSurname);
		if(editInformationsView.editInformationsPopUpVM.error=="")
		{
			editInformationsView.editInformationsPopUpVM.error = this.checkname (editInformationsView.editInformationsPopUpVM.tempFirstName);
		}
		if(editInformationsView.editInformationsPopUpVM.error=="")
		{
			editInformationsView.editInformationsPopUpVM.error = this.checkEmail (editInformationsView.editInformationsPopUpVM.tempMail);
		}
		if(editInformationsView.editInformationsPopUpVM.error=="")
		{
			StartCoroutine(updateUserInformations(editInformationsView.editInformationsPopUpVM.tempFirstName,editInformationsView.editInformationsPopUpVM.tempSurname,editInformationsView.editInformationsPopUpVM.tempMail));
		}
	}
	private IEnumerator updateUserInformations(string firstname, string surname, string mail)
	{
		editInformationsView.popUpVM.guiEnabled = false;
		model.player.FirstName = firstname;
		model.player.Surname = surname;
		model.player.Mail = mail;
		yield return StartCoroutine (model.player.updateInformations ());
		this.drawPersonalInformations ();
		editInformationsView.popUpVM.guiEnabled = true;
		this.hideEditInformationsPopUp ();
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
		this.searchUsersPopUp.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
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
	public void collectionButtonHandler()
	{
		Application.LoadLevel("NewSkillBook");
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
		blockOrigin.y = blockOrigin.y + ApplicationDesignRules.button62WorldSize.y / 2f;
		return blockOrigin;
	}
	public Vector2 getResultsBlockSize()
	{
		Vector2 blockSize=this.resultsBlock.GetComponent<NewBlockController> ().getSize ();
		blockSize.y = blockSize.y + ApplicationDesignRules.button62WorldSize.y;
		return blockSize;
	}
	public Vector3 getFriendsBlockOrigin()
	{
		Vector3 blockOrigin = this.friendsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		blockOrigin.y = blockOrigin.y + ApplicationDesignRules.button62WorldSize.y / 2f;
		return blockOrigin;
	}
	public Vector2 getFriendsBlockSize()
	{
		Vector2 blockSize=this.friendsBlock.GetComponent<NewBlockController> ().getSize ();
		blockSize.y = blockSize.y + ApplicationDesignRules.button62WorldSize.y;
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