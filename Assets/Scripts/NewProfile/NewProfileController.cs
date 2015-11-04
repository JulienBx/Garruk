//using UnityEngine;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;
//using TMPro;
//using System.Linq;
//
//public class NewProfileController : MonoBehaviour
//{
//	public static NewProfileController instance;
//	private NewProfileModel model;
//	
//	public GameObject tutorialObject;
//	public GameObject blockObject;
//	public GUISkin popUpSkin;
//
//	private GameObject menu;
//	private GameObject tutorial;
//
//	private GameObject profileBlock;
//	private GameObject profileBlockTitle;
//	private GameObject collectionButton;
//	private GameObject cleanCardsButton;
//	private GameObject friendshipStatusButton;
//	private GameObject profilePicture;
//	private GameObject profileEditInformationsButton;
//	private GameObject profileEditPasswordButton;
//	private GameObject[] profileInformations;
//	private GameObject[] profileStats;
//
//	private GameObject searchBlock;
//	private GameObject searchBlockTitle;
//	private GameObject searchSubtitle;
//	private GameObject searchBar;
//
//	private GameObject resultsBlock;
//	private GameObject[] resultsContents;
//	private GameObject[] resultsTabs;
//	private GameObject resultsPaginationButtons;
//
//	private GameObject friendsBlock;
//	private GameObject[] friendsContents;
//	private GameObject[] friendsTabs;
//	private GameObject[] friendsStatusButtons;
//	private GameObject[] challengesButtons;
//	private GameObject friendsPaginationButtons;
//	
//	private GameObject selectPicturePopUp;
//	private GameObject searchUsersPopUp;
//
//	private bool isMyProfile;
//	private string profileChosen;
//	
//	private Rect centralWindow;
//	private Rect centralWindowEditInformations;
//	
//	private IList<int> friendsRequestsDisplayed;
//	private IList<int> challengesRecordsDisplayed;
//	private IList<int> trophiesDisplayed;
//	private IList<int> friendsDisplayed;
//	private IList<int> friendsToBeDisplayed;
//	private IList<int> confrontationsDisplayed;
//	
//	private IList<int> friendsOnline;
//
//	private int activeResultsTab;
//	private int activeFriendsTab;
//	private Pagination resultsPagination;
//	private Pagination friendsPagination;
//
//	private bool isTutorialLaunched;
//	private bool isSceneLoaded;
//	private bool isProfilePictureHovered;
//
//	private bool isSelectPicturePopUpDisplayed;
//	private bool isSearchUsersPopUpDisplayed;
//
//	public int friendsRefreshInterval;
//	private float friendsCheckTimer;
//
//	private ProfileCheckPasswordPopUpView checkPasswordView;
//	private bool isCheckPasswordViewDisplayed;
//
//	private ProfileChangePasswordPopUpView changePasswordView;
//	private bool isChangePasswordViewDisplayed;
//
//	private ProfileEditInformationsPopUpView editInformationsView;
//	private bool isEditInformationsViewDisplayed;
//
//	private bool isSearchingUsers;
//	private string searchValue;
//	private bool isMouseOnSearchBar;
//
//	void Update()
//	{	
//		this.friendsCheckTimer += Time.deltaTime;
//		
//		if (this.isMyProfile && friendsCheckTimer > friendsRefreshInterval && this.isSceneLoaded) 
//		{
//			this.checkFriendsOnlineStatus();
//		}
//		if(isSearchingUsers)
//		{
//			if(!Input.GetKey(KeyCode.Delete))
//			{
//				foreach (char c in Input.inputString) 
//				{
//					if(c==(char)KeyCode.Backspace && this.searchValue.Length>0)
//					{
//						this.searchValue = this.searchValue.Remove(this.searchValue.Length - 1);
//						this.searchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text = this.searchValue;
//						if(this.searchValue.Length==0)
//						{
//							this.isSearchingUsers=false;
//							this.searchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text ="Rechercher";
//						}
//					}
//					else if (c == "\b"[0])
//					{
//						if (searchValue.Length != 0)
//						{
//							searchValue= searchValue.Substring(0, searchValue.Length - 1);
//						}
//					}
//					else
//					{
//						if (c == "\n"[0] || c == "\r"[0])
//						{
//							this.searchUsersHandler();	
//						}
//						else if(this.searchValue.Length<12)
//						{
//							this.searchValue += c;
//							this.searchValue=this.searchValue.ToLower();
//							this.searchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text = this.searchValue;
//						}
//					}
//				}
//			}
//			if((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))&& !this.isMouseOnSearchBar)
//			{
//				this.isSearchingUsers=false;
//				if(this.searchValue=="")
//				{
//					this.searchBar.transform.FindChild ("Title").FindChild("Title").GetComponent<TextMeshPro>().text ="Rechercher";
//				}
//			}
//		}
//	}
//	void Awake()
//	{
//		instance = this;
//		this.model = new NewProfileModel ();
//		if(ApplicationModel.profileChosen==""|| ApplicationModel.profileChosen==ApplicationModel.username)
//		{
//			this.isMyProfile=true;
//			this.profileChosen=ApplicationModel.username;
//		}
//		else
//		{
//			this.isMyProfile=false;
//			this.profileChosen=ApplicationModel.profileChosen;
//			ApplicationModel.profileChosen="";
//		}
//		this.resultsPagination = new Pagination ();
//		this.resultsPagination.chosenPage = 0;
//		this.resultsPagination.nbElementsPerPage = 2;
//		this.activeResultsTab = 0;
//		this.friendsPagination = new Pagination ();
//		this.friendsPagination.chosenPage = 0;
//		this.friendsPagination.nbElementsPerPage = 2;
//		this.activeFriendsTab = 0;
//		this.initializeScene ();
//		this.searchValue = "";
//		this.initializeScene ();
//	}
//	public IEnumerator initialization()
//	{
//		this.resize ();
//		yield return StartCoroutine (model.getData (this.isMyProfile, this.profileChosen));
//		this.selectAFriendsTab ();
//		this.selectAResultsTab ();
//		this.initializeProfile ();
//		this.checkFriendsOnlineStatus ();
//		newMenuController.instance.hideLoadingScreen ();
//		this.isSceneLoaded = true;
//	}
//	private void initializeFriendsRequests()
//	{
//		this.friendsPagination.chosenPage = 0;
//		this.friendsPagination.totalElements= model.friendsRequests.Count;
//		this.friendsPaginationButtons.GetComponent<NewProfileFriendsPaginationController> ().p = this.friendsPagination;
//		this.friendsPaginationButtons.GetComponent<NewProfileFriendsPaginationController> ().setPagination ();
//		this.drawFriendsRequests();
//	}
//	private void initializeFriends()
//	{
//		this.sortFriendsList ();
//		this.friendsPagination.chosenPage = 0;
//		this.friendsPagination.totalElements= this.friendsToBeDisplayed.Count;
//		this.friendsPaginationButtons.GetComponent<NewProfileFriendsPaginationController> ().p = this.friendsPagination;
//		this.friendsPaginationButtons.GetComponent<NewProfileFriendsPaginationController> ().setPagination ();
//		this.drawFriendsRequests();
//	}
//	private void initializeChallengesRecords()
//	{
//		this.resultsPagination.chosenPage = 0;
//		this.resultsPagination.totalElements= model.challengesRecords.Count;
//		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().p = this.resultsPagination;
//		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().setPagination ();
//		this.drawChallengesRecords ();
//	}
//	private void initializeTrophies()
//	{
//		this.resultsPagination.chosenPage = 0;
//		this.resultsPagination.totalElements= model.trophies.Count;
//		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().p = this.resultsPagination;
//		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().setPagination ();
//		this.drawChallengesRecords ();
//	}
//	private void initializeConfrontations()
//	{
//		this.resultsPagination.chosenPage = 0;
//		this.resultsPagination.totalElements= model.confrontations.Count;
//		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().p = this.resultsPagination;
//		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().setPagination ();
//		this.drawConfrontations ();
//	}
//	private void initializeProfile()
//	{
//		this.drawProfile ();
//	}
//	private void selectAResultsTab()
//	{
//		for(int i=0;i<this.resultsTabs.Length;i++)
//		{
//			if(i==this.activeResultsTab)
//			{
//				this.resultsTabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(1);
//				this.resultsTabs[i].GetComponent<NewProfileResultsTabController>().setIsSelected(true);
//				this.resultsTabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
//			}
//			else
//			{
//				this.resultsTabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(0);
//				this.resultsTabs[i].GetComponent<NewProfileResultsTabController>().reset();
//			}
//		}
//		switch(this.activeResultsTab)
//		{
//		case 0:
//			this.initializeTrophies();
//			break;
//		case 1:
//			if(this.isMyProfile)
//			{
//				this.initializeChallengesRecords();
//			}
//			else
//			{
//				this.initializeConfrontations();
//			}
//			break;
//		}
//	}
//	public void selectAResultsTabHandler(int idTab)
//	{
//		this.activeResultsTab = idTab;
//		this.selectAResultsTab ();
//	}
//	public void paginationResultsHandler()
//	{
//		switch(this.activeResultsTab)
//		{
//		case 0:
//			this.drawTrophies();
//			break;
//		case 1:
//			if(this.isMyProfile)
//			{
//				this.drawChallengesRecords();
//			}
//			else
//			{
//				this.drawConfrontations();
//			}
//			break;
//		}
//	}
//	private void selectAFriendsTab()
//	{
//		for(int i=0;i<this.friendsTabs.Length;i++)
//		{
//			if(i==this.activeFriendsTab)
//			{
//				this.friendsTabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(1);
//				this.friendsTabs[i].GetComponent<NewProfileFriendsTabController>().setIsSelected(true);
//				this.friendsTabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
//			}
//			else
//			{
//				this.friendsTabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(0);
//				this.friendsTabs[i].GetComponent<NewProfileFriendsTabController>().reset();
//			}
//		}
//		switch(this.activeResultsTab)
//		{
//		case 0:
//			this.initializeFriends();
//			break;
//		case 1:
//			this.initializeFriendsRequests();
//			break;
//		}
//	}
//	public void selectAFriendsTabHandler(int idTab)
//	{
//		this.activeFriendsTab = idTab;
//		this.selectAFriendsTab ();
//	}
//	public void paginationFriendsHandler()
//	{
//		switch(this.activeFriendsTab)
//		{
//		case 0:
//			this.drawFriends();
//			break;
//		case 1:
//			this.drawFriendsRequests();
//			break;
//		}
//	}
//	public void initializeScene()
//	{
//		menu = GameObject.Find ("newMenu");
//		menu.AddComponent<newProfileMenuController> ();
//		this.profileBlock = Instantiate(this.blockObject) as GameObject;
//		this.profileBlockTitle = GameObject.Find ("ProfileTitle");
//		this.profileBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
//		this.profilePicture = GameObject.Find ("ProfilePicture");
//		this.profilePicture.AddComponent<NewProfilePictureButtonController> ();
//		this.collectionButton = GameObject.Find ("CollectionButton");
//		this.collectionButton.AddComponent<NewProfileCollectionButtonController> ();
//		this.cleanCardsButton = GameObject.Find ("CleanCardsButton");
//		this.cleanCardsButton.AddComponent<NewProfileCleanCardsButtonController> ();
//		this.friendshipStatusButton = GameObject.Find ("FriendshipStatusButton");
//		this.friendshipStatusButton.AddComponent<NewProfileFriendshipStatusButtonController> ();
//		this.profileInformations=new GameObject[3];
//		for(int i=0;i<this.profileInformations.Length;i++)
//		{
//			this.profileInformations[i]=GameObject.Find ("ProfileInformation"+i);
//			this.profileInformations[i].GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
//			// A compléter !
//		}
//		this.profileEditInformationsButton = GameObject.Find ("ProfileEditInformationsButton");
//		this.profileEditInformationsButton.AddComponent<NewProfileEditInformationsButtonController> ();
//		this.profileEditPasswordButton = GameObject.Find ("ProfileEditPasswordButton");
//		this.profileEditPasswordButton.AddComponent<NewProfileEditPasswordButtonController> ();
//		this.profileStats = new GameObject[4];
//		for(int i=0;i<this.profileStats.Length;i++)
//		{
//			this.profileStats[i]=GameObject.Find ("ProfileStats"+i);
//			this.profileStats[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
//			// A compléter !
//		}
//
//		this.searchBlock = Instantiate(this.blockObject)as GameObject;
//		this.searchBlockTitle = GameObject.Find ("SearchTitle");
//		this.searchBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
//		this.searchBlockTitle.GetComponent<TextMeshPro>().text="Rechercher";
//		this.searchSubtitle=GameObject.Find ("SearchSubtitile");
//		this.searchSubtitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
//		this.searchSubtitle.GetComponent<TextMeshPro>().text="Trouver un ami".ToUpper();
//		this.searchBar=GameObject.Find ("SearchBar");
//		this.searchBar.AddComponent<NewProfileSearchBarController>();
//		this.searchBar.GetComponent<NewProfileSearchBarController>().setText("Rechercher");
//
//
//		this.resultsBlock = Instantiate (this.blockObject) as GameObject;
//		this.resultsTabs=new GameObject[2];
//		for(int i=0;i<this.resultsTabs.Length;i++)
//		{
//			this.resultsTabs[i]=GameObject.Find ("ResultsTab"+i);
//			this.resultsTabs[i].AddComponent<NewProfileResultsTabController>();
//			this.resultsTabs[i].GetComponent<NewProfileResultsTabController>().setId(i);
//		}
//		this.resultsTabs[0].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Trophés");
//		if(this.isMyProfile)
//		{
//			this.resultsTabs[1].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Confrontations");
//		}
//		else
//		{
//			this.resultsTabs[1].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Défis");
//		}
//		this.resultsPaginationButtons = GameObject.Find("ResultsPagination");
//		this.resultsPaginationButtons.AddComponent<NewProfileResultsPaginationController> ();
//		this.resultsPaginationButtons.GetComponent<NewProfileResultsPaginationController> ().initialize ();
//		this.resultsContents = new GameObject[this.resultsPagination.nbElementsPerPage];
//		for(int i=0;i<this.resultsContents.Length;i++)
//		{
//			this.resultsContents[i]=gameObject.Find("ResultsContent"+i);
//			// A compléter 
//		}
//
//		this.friendsBlock = Instantiate (this.blockObject) as GameObject;
//		this.friendsTabs=new GameObject[2];
//		for(int i=0;i<this.friendsTabs.Length;i++)
//		{
//			this.friendsTabs[i]=gameObject.Find("FriendsTab"+i);
//			this.friendsTabs[i].AddComponent<NewProfileFriendsTabController>();
//			this.friendsTabs[i].GetComponent<NewProfileFriendsTabController>().setId (i);
//		}
//		this.friendsTabs [0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = ("Amis");
//		if(this.isMyProfile)
//		{
//			this.friendsTabs[1].transform.FindChild("Title").GetComponent<TextMeshPro>().text=("Invitations");
//		}
//		else
//		{
//			this.friendsTabs[1].SetActive(false);
//		}
//		this.friendsPaginationButtons = GameObject.Find("ResultsPagination");
//		this.friendsPaginationButtons.AddComponent<NewProfileFriendsPaginationController> ();
//		this.friendsPaginationButtons.GetComponent<NewProfileFriendsPaginationController> ().initialize ();
//		this.friendsStatusButtons=new GameObject[this.friendsPagination.nbElementsPerPage];
//		for(int i=0;i<this.friendsStatusButtons.Length;i++)
//		{
//			this.friendsStatusButtons[i]=GameObject.Find ("FriendsStatusButton"+i);
//			this.friendsStatusButtons[i].AddComponent<NewProfileChallengeButtonController>();
//			this.friendsStatusButtons[i].GetComponent<NewProfileChallengeButtonController>().setId(i);
//		}
//		this.challengesButtons=new GameObject[this.friendsPagination.nbElementsPerPage];
//		for(int i=0;i<this.challengesButtons.Length;i++)
//		{
//			this.challengesButtons[i]=GameObject.Find ("FriendsStatusButton"+i);
//			this.challengesButtons[i].AddComponent<NewProfileChallengeButtonController>();
//			this.challengesButtons[i].GetComponent<NewProfileChallengeButtonController>().setId(i);
//		}
//		this.friendsContents=new GameObject[this.friendsPagination.nbElementsPerPage];
//		for(int i=0;i<this.friendsStatusButtons.Length;i++)
//		{
//			this.friendsContents[i]=GameObject.Find ("FriendsContent"+i);
//			this.friendsContents[i].AddComponent<NewProfileFriendsStatusButtonController>();
//			this.friendsContents[i].GetComponent<NewProfileFriendsStatusButtonController>().setId (i);
//		}
//	}
//	public void resize()
//	{
//		float profileBlockLeftMargin = ApplicationDesignRules.leftMargin;
//		float profileBlockRightMargin =ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.rightMargin+(ApplicationDesignRules.worldWidth-ApplicationDesignRules.leftMargin-ApplicationDesignRules.rightMargin-ApplicationDesignRules.gapBetweenBlocks)/2f;
//		float profileBlockUpMargin = ApplicationDesignRules.upMargin;
//		float profileBlockDownMargin = ApplicationDesignRules.worldHeight-6.45f+ApplicationDesignRules.gapBetweenBlocks;
//		
//		this.profileBlock.GetComponent<NewBlockController> ().resize(profileBlockLeftMargin,profileBlockRightMargin,profileBlockUpMargin,profileBlockDownMargin);
//		Vector3 profileBlockUpperLeftPosition = this.profileBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
//		Vector3 profileBlockUpperRightPosition = this.profileBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
//		Vector2 profileBlockSize = this.profileBlock.GetComponent<NewBlockController> ().getSize ();
//		this.profileBlockTitle.transform.position = new Vector3 (profileBlockUpperLeftPosition.x + 0.3f, profileBlockUpperLeftPosition.y - 0.2f, 0f);
//		this.profileBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
//
//		this.profilePicture.transform.position = new Vector3 (profileBlockUpperLeftPosition.x + 0.3f + ApplicationDesignRules.profilePictureWorldSize.x / 2f, profileBlockUpperLeftPosition.y - 1.2f - ApplicationDesignRules.profilePictureWorldSize.y / 2f, 0f);
//		this.profilePicture.transform.localScale = ApplicationDesignRules.profilePictureScale;
//
//		this.collectionButton.transform.position = new Vector3 (profileBlockUpperRightPosition.x - 0.3f - ApplicationDesignRules.button61WorldSize.x / 2f, profileBlockUpperRightPosition.y - 1.2f - ApplicationDesignRules.button61WorldSize.y / 2f, 0f);
//		this.collectionButton.transform.localScale = ApplicationDesignRules.button61Scale;
//
//		this.cleanCardsButton.transform.position = new Vector3 (profileBlockUpperRightPosition.x - 0.3f - ApplicationDesignRules.button61WorldSize.x / 2f, profileBlockUpperRightPosition.y - 2f - ApplicationDesignRules.button61WorldSize.y / 2f, 0f);
//		this.cleanCardsButton.transform.localScale = ApplicationDesignRules.button61Scale;
//
//		this.friendshipStatusButton.transform.position = new Vector3 (profileBlockUpperLeftPosition.x + 0.3f + ApplicationDesignRules.profilePictureWorldSize.x / 2f, profileBlockUpperLeftPosition.y - 1.2f - ApplicationDesignRules.profilePictureWorldSize.y - 0.1f - ApplicationDesignRules.button62WorldSize.y / 2f, 0f);
//		this.friendshipStatusButton.transform.localScale = ApplicationDesignRules.button62Scale;
//
//		for(int i=0;i<this.profileInformations.Length;i++)
//		{
//			this.profileInformations[i].transform.position = new Vector3 (profileBlockUpperLeftPosition.x + 0.3f + ApplicationDesignRules.profilePictureWorldSize.x + 0.5f, profileBlockUpperLeftPosition.y - 1.2f - ApplicationDesignRules.profilePictureWorldSize.y / (0.25f+i*0.25f), 0f);
//			this.profileInformations[i].transform.localScale = ApplicationDesignRules.subMainTitleScale;
//		}
//
//		this.profileEditInformationsButton.transform.position = new Vector3 (profileBlockUpperLeftPosition.x + 0.3f + ApplicationDesignRules.profilePictureWorldSize.x + 0.5f + ApplicationDesignRules.button62WorldSize.x / 2f, profileBlockUpperLeftPosition.y - 1.2f - ApplicationDesignRules.profilePictureWorldSize.y - 1f, 0f);
//		this.profileEditInformationsButton.transform.localScale = ApplicationDesignRules.button62Scale;
//
//		this.profileEditPasswordButton.transform.position = new Vector3 (profileBlockUpperLeftPosition.x + 0.3f + ApplicationDesignRules.profilePictureWorldSize.x + 0.5f + ApplicationDesignRules.button62WorldSize.x / 2f, profileBlockUpperLeftPosition.y - 1.2f - ApplicationDesignRules.profilePictureWorldSize.y - 2f, 0f);
//		this.profileEditPasswordButton.transform.localScale = ApplicationDesignRules.button62Scale;
//
//		float profileStatsWorldWidth = profileBlockSize.x - ApplicationDesignRules.profilePictureWorldSize.x - 0.6f - 0.5f;
//
//		for(int i=0;i<this.profileStats.Length;i++)
//		{
//			this.profileStats[i].transform.position=new Vector3(profileBlockUpperRightPosition.x-0.3f-profileStatsWorldWidth/2f,profileBlockUpperRightPosition.y-2f-i*0.5f);
//			// A compléter
//		}
//
//		float searchBlockLeftMargin =  ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.leftMargin+(ApplicationDesignRules.worldWidth-ApplicationDesignRules.rightMargin-ApplicationDesignRules.leftMargin-ApplicationDesignRules.gapBetweenBlocks)/2f;
//		float searchBlockRightMargin = ApplicationDesignRules.rightMargin;
//		float searchBlockUpMargin = 6.45f;
//		float searchBlockDownMargin = ApplicationDesignRules.downMargin;
//		
//		this.searchBlock.GetComponent<NewBlockController> ().resize(searchBlockLeftMargin,searchBlockRightMargin,searchBlockUpMargin,searchBlockDownMargin);
//		Vector3 searchBlockUpperLeftPosition = this.searchBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
//		Vector3 searchBlockUpperRightPosition = this.searchBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
//		Vector2 searchBlockSize = this.searchBlock.GetComponent<NewBlockController> ().getSize ();
//		Vector3 searchOrigin = this.searchBlock.GetComponent<NewBlockController> ().getOriginPosition ();
//		this.searchBlockTitle.transform.position = new Vector3 (searchBlockUpperLeftPosition.x + 0.3f, searchBlockUpperLeftPosition.y - 0.2f, 0f);
//		this.searchBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
//		
//		this.searchSubtitle.transform.position = new Vector3 (searchBlockUpperLeftPosition.x + 0.3f, searchBlockUpperLeftPosition.y - 1.2f, 0f);
//		this.searchSubtitle.transform.GetComponent<TextContainer>().width=searchBlockSize.x-0.6f;
//		this.searchSubtitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
//		
//		this.searchBar.transform.position = new Vector3(searchOrigin.x,searchOrigin.y-0.5f,searchOrigin.z);
//		this.searchBar.transform.localScale = ApplicationDesignRules.inputTextScale;
//
//		float friendsBlockLeftMargin = ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.leftMargin+(ApplicationDesignRules.worldWidth-ApplicationDesignRules.rightMargin-ApplicationDesignRules.leftMargin-ApplicationDesignRules.gapBetweenBlocks)/2f;;
//		float friendsBlockRightMargin = ApplicationDesignRules.rightMargin;
//		float friendsBlockUpMargin = ApplicationDesignRules.upMargin;
//		float friendsBlockDownMargin = ApplicationDesignRules.worldHeight-6.45f+ApplicationDesignRules.gapBetweenBlocks;
//
//		this.friendsBlock.GetComponent<NewBlockController> ().resize(friendsBlockLeftMargin,friendsBlockRightMargin,friendsBlockUpMargin,friendsBlockDownMargin);
//		Vector3 friendsBlockUpperLeftPosition = this.friendsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
//		Vector3 friendsBlockUpperRightPosition = this.friendsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
//		Vector2 friendsBlockLowerLeftPosition = this.friendsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
//		Vector2 friendsBlockLowerRightPosition = this.friendsBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
//		Vector2 friendsBlockSize = this.friendsBlock.GetComponent<NewBlockController> ().getSize ();
//		
//		float gapBetweenFriendsTab = 0.02f;
//		for(int i=0;i<this.friendsTabs.Length;i++)
//		{
//			this.friendsTabs[i].transform.localScale = ApplicationDesignRules.button62Scale;
//			this.friendsTabs[i].transform.position = new Vector3 (friendsBlockUpperLeftPosition.x + ApplicationDesignRules.button62WorldSize.x / 2f+ i*(ApplicationDesignRules.button62WorldSize.x+gapBetweenFriendsTab), friendsBlockUpperLeftPosition.y+ApplicationDesignRules.button62WorldSize.y/2f,0f);
//		}
//		
//		Vector2 friendsContentBlockSize = new Vector2 (friendsBlockSize.x - 0.6f, (friendsBlockSize.y - 0.3f - 0.6f)/this.friendsContents.Length);
//		float friendsLineScale = ApplicationDesignRules.getLineScale (friendsContentBlockSize.x);
//		
//		for(int i=0;i<this.friendsContents.Length;i++)
//		{
//			this.friendsContents[i].transform.position=new Vector3(friendsBlockUpperLeftPosition.x+0.3f+friendsContentBlockSize.x/2f,friendsBlockUpperLeftPosition.y-0.3f-(i+1)*friendsContentBlockSize.y,0f);
//			this.friendsContents[i].transform.FindChild("line").localScale=new Vector3(friendsLineScale,1f,1f);
//			this.friendsContents[i].transform.FindChild("picture").localScale=ApplicationDesignRules.thumbScale;
//			this.friendsContents[i].transform.FindChild("picture").localPosition=new Vector3(-friendsContentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(friendsContentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
//			this.friendsContents[i].transform.FindChild("username").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
//			this.friendsContents[i].transform.FindChild("username").GetComponent<TextMeshPro>().textContainer.width=(friendsContentBlockSize.x/2f)-0.1f-ApplicationDesignRules.thumbWorldSize.x;
//			this.friendsContents[i].transform.FindChild("username").localPosition=new Vector3(-friendsContentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,friendsContentBlockSize.y-(friendsContentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
//			this.friendsContents[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
//			this.friendsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=0.75f*friendsContentBlockSize.x-0.1f-ApplicationDesignRules.thumbWorldSize.x;
//			this.friendsContents[i].transform.FindChild("description").localPosition=new Vector3(-friendsContentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,friendsContentBlockSize.y/2f,0f);
//			//this.friendsContents[i].transform.FindChild("date").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
//			//this.friendsContents[i].transform.FindChild("date").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
//			//this.friendsContents[i].transform.FindChild("date").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y-(contentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
//			//this.friendsContents[i].transform.FindChild("new").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
//			//this.friendsContents[i].transform.FindChild("new").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
//			//this.friendsContents[i].transform.FindChild("new").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y/2f,0f);
//		}
//		
//		for(int i=0;i<this.friendsStatusButtons.Length;i++)
//		{
//			this.friendsStatusButtons[i].transform.localScale = ApplicationDesignRules.button62Scale;
//			this.friendsStatusButtons[i].transform.position=new Vector3(friendsBlockUpperRightPosition.x-0.3f-ApplicationDesignRules.button62WorldSize.x/2f,friendsBlockUpperRightPosition.y-0.3f-(i+0.5f)*contentBlockSize.y,0f);
//		}
//
//		for(int i=0;i<this.challengesButtons.Length;i++)
//		{
//			this.challengesButtons[i].transform.localScale = ApplicationDesignRules.button62Scale;
//			this.challengesButtons[i].transform.position=new Vector3(friendsBlockUpperRightPosition.x-0.3f-ApplicationDesignRules.button62WorldSize.x/2f,friendsBlockUpperRightPosition.y-0.3f-(i+0.5f)*contentBlockSize.y,0f);
//		}
//		
//		this.friendsPaginationButtons.transform.localPosition=new Vector3 (friendsBlockLowerLeftPosition.x + friendsBlockSize.x / 2, friendsBlockLowerLeftPosition.y + 0.3f, 0f);
//		this.friendsPaginationButtons.GetComponent<NewHomePagePaginationController> ().resize ();
//
//		float resultsBlockLeftMargin = ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.leftMargin+(ApplicationDesignRules.worldWidth-ApplicationDesignRules.rightMargin-ApplicationDesignRules.leftMargin-ApplicationDesignRules.gapBetweenBlocks)/2f;;
//		float resultsBlockRightMargin = ApplicationDesignRules.rightMargin;
//		float resultsBlockUpMargin = ApplicationDesignRules.upMargin;
//		float resultsBlockDownMargin = ApplicationDesignRules.worldHeight-6.45f+ApplicationDesignRules.gapBetweenBlocks;
//		
//		this.resultsBlock.GetComponent<NewBlockController> ().resize(resultsBlockLeftMargin,resultsBlockRightMargin,resultsBlockUpMargin,resultsBlockDownMargin);
//		Vector3 resultsBlockUpperLeftPosition = this.resultsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
//		Vector3 resultsBlockUpperRightPosition = this.resultsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
//		Vector2 resultsBlockLowerLeftPosition = this.resultsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
//		Vector2 resultsBlockLowerRightPosition = this.resultsBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
//		Vector2 resultsBlockSize = this.resultsBlock.GetComponent<NewBlockController> ().getSize ();
//		
//		float gapBetweenResultsTab = 0.02f;
//		for(int i=0;i<this.resultsTabs.Length;i++)
//		{
//			this.resultsTabs[i].transform.localScale = ApplicationDesignRules.button62Scale;
//			this.resultsTabs[i].transform.position = new Vector3 (resultsBlockUpperLeftPosition.x + ApplicationDesignRules.button62WorldSize.x / 2f+ i*(ApplicationDesignRules.button62WorldSize.x+gapBetweenResultsTab), resultsBlockUpperLeftPosition.y+ApplicationDesignRules.button62WorldSize.y/2f,0f);
//		}
//		
//		Vector2 resultsContentBlockSize = new Vector2 (resultsBlockSize.x - 0.6f, (resultsBlockSize.y - 0.3f - 0.6f)/this.resultsContents.Length);
//		float resultsLineScale = ApplicationDesignRules.getLineScale (resultsContentBlockSize.x);
//		
//		for(int i=0;i<this.resultsContents.Length;i++)
//		{
//			this.resultsContents[i].transform.position=new Vector3(resultsBlockUpperLeftPosition.x+0.3f+resultsContentBlockSize.x/2f,resultsBlockUpperLeftPosition.y-0.3f-(i+1)*friendsContentBlockSize.y,0f);
//			this.resultsContents[i].transform.FindChild("line").localScale=new Vector3(resultsLineScale,1f,1f);
//			this.resultsContents[i].transform.FindChild("picture").localScale=ApplicationDesignRules.thumbScale;
//			this.resultsContents[i].transform.FindChild("picture").localPosition=new Vector3(-friendsContentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(friendsContentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
//			this.resultsContents[i].transform.FindChild("username").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
//			this.resultsContents[i].transform.FindChild("username").GetComponent<TextMeshPro>().textContainer.width=(friendsContentBlockSize.x/2f)-0.1f-ApplicationDesignRules.thumbWorldSize.x;
//			this.resultsContents[i].transform.FindChild("username").localPosition=new Vector3(-friendsContentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,friendsContentBlockSize.y-(friendsContentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
//			this.resultsContents[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
//			this.resultsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=0.75f*friendsContentBlockSize.x-0.1f-ApplicationDesignRules.thumbWorldSize.x;
//			this.resultsContents[i].transform.FindChild("description").localPosition=new Vector3(-friendsContentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,friendsContentBlockSize.y/2f,0f);
//			//this.resultsContents[i].transform.FindChild("date").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
//			//this.resultsContents[i].transform.FindChild("date").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
//			//this.resultsContents[i].transform.FindChild("date").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y-(contentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
//			//this.resultsContents[i].transform.FindChild("new").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
//			//this.resultsContents[i].transform.FindChild("new").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
//			//this.resultsContents[i].transform.FindChild("new").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y/2f,0f);
//		}
//
//		if(this.isTutorialLaunched)
//		{
//			this.tutorial.GetComponent<TutorialObjectController>().resize();
//		}
//		if(this.isCheckPasswordViewDisplayed)
//		{
//			this.checkPasswordViewResize();
//		}
//		else if(this.isChangePasswordViewDisplayed)
//		{
//			this.changePasswordViewResize();
//		}
//		else if(this.isEditInformationsViewDisplayed)
//		{
//			this.editInformationsViewResize();
//		}
//	}
//	public void returnPressed()
//	{
//		if(this.isCheckPasswordViewDisplayed)
//		{
//			this.checkPasswordHandler(checkPasswordView.checkPasswordPopUpVM.tempOldPassword);
//		}
//		else if(this.isChangePasswordViewDisplayed)
//		{
//			this.editPasswordHandler();
//		}
//		else if(this.isEditInformationsViewDisplayed)
//		{
//			this.updateUserInformationsHandler();
//		}
//	}
//	public void escapePressed()
//	{
//		if(this.isSelectPicturePopUpDisplayed)
//		{
//			this.hideSelectPicturePopUp();
//		}
//		else if(this.isCheckPasswordViewDisplayed)
//		{
//			this.hideCheckPasswordPopUp();
//		}
//		else if(this.isChangePasswordViewDisplayed)
//		{
//			this.hideChangePasswordPopUp();
//		}
//		else if(this.isEditInformationsViewDisplayed)
//		{
//			this.hideEditInformationsPopUp();
//		}
//		else if(this.isSearchUsersPopUpDisplayed)
//		{
//			this.hideSearchUsersPopUp();
//		}
//	}
//	public void closeAllPopUp()
//	{
//		if(this.isSelectPicturePopUpDisplayed)
//		{
//			this.hideSelectPicturePopUp();
//		}
//		if(this.isCheckPasswordViewDisplayed)
//		{
//			this.hideCheckPasswordPopUp();
//		}
//		if(this.isChangePasswordViewDisplayed)
//		{
//			this.hideChangePasswordPopUp();
//		}
//		if(this.isEditInformationsViewDisplayed)
//		{
//			this.hideEditInformationsPopUp();
//		}
//		if(this.isSearchUsersPopUpDisplayed)
//		{
//			this.hideSearchUsersPopUp();
//		}
//	}
//	public void drawChallengesRecords()
//	{
//		this.challengesRecordsDisplayed = new List<int> ();
//		for(int i =0;i<this.resultsPagination.nbElementsPerPage;i++)
//		{
//			if(this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i<model.challengesRecords.Count)
//			{
//				this.challengesRecordsDisplayed.Add (this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i);
//				this.resultsContents[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=model.challengesRecords[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].Friend.Username;
//				this.resultsContents[i].transform.FindChild("Picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnThumbPicture(model.challengesRecords[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].Friend.idProfilePicture);
//				this.resultsContents[i].SetActive(true);
//			}
//			else
//			{
//				this.resultsContents[i].SetActive(false);
//			}
//		}
//	}
//	public void drawFriendsRequests()
//	{
//		this.friendsRequestsDisplayed = new List<int> ();
//		for(int i =0;i<elementsPerPageFriendsRequests;i++)
//		{
//			if(this.chosenPageFriendsRequests*this.elementsPerPageFriendsRequests+i<model.friendsRequests.Count)
//			{
//				this.friendsRequestsDisplayed.Add (this.chosenPageFriendsRequests*this.elementsPerPageFriendsRequests+i);
//				this.friendsRequests[i].GetComponent<FriendsRequestController>().f=model.friendsRequests[this.chosenPageFriendsRequests*this.elementsPerPageFriendsRequests+i];
//				this.friendsRequests[i].GetComponent<FriendsRequestController>().show();
//				this.friendsRequests[i].SetActive(true);
//			}
//			else
//			{
//				this.friendsRequests[i].SetActive(false);
//			}
//		}
//	}
//	public void drawFriends()
//	{
//		this.friendsDisplayed = new List<int> ();
//		for(int i =0;i<this.friendsPagination.nbElementsPerPage;i++)
//		{
//			if(this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i<this.friendsToBeDisplayed.Count)
//			{
//				this.friendsDisplayed.Add (this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i);
//				this.friendsContents[i].SetActive(true);
//				string connectionState="";
//				Color connectionStateColor=new Color();
//				switch(model.users[this.friendsToBeDisplayed[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i]].OnlineStatus)
//				{
//				case 0:
//					connectionState = "n'est pas en ligne";
//					connectionStateColor=ApplicationDesignRules.whiteTextColor;
//					break;
//				case 1:
//					connectionState = "est disponible pour un défi !";
//					connectionStateColor=ApplicationDesignRules.blueColor;
//					break;
//				case 2:
//					connectionState = "est entrain de jouer";
//					connectionStateColor=ApplicationDesignRules.redColor;
//					break;
//				}
//				this.friendsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=connectionState;
//				this.friendsContents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=connectionStateColor;
//				this.friendsContents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnThumbPicture(model.users[this.friendsToBeDisplayed[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i]].idProfilePicture);
//				this.friendsContents[i].transform.FindChild("username").GetComponent<TextMeshPro>().text=model.users[this.friendsToBeDisplayed[this.friendsPagination.chosenPage*this.friendsPagination.nbElementsPerPage+i]].Username;
//			}
//			else
//			{
//				this.friendsContents[i].SetActive(false);
//			}
//		}
//	}
//	public void drawTrophies()
//	{
//		this.trophiesDisplayed = new List<int> ();
//		for(int i =0;i<this.resultsPagination.nbElementsPerPage;i++)
//		{
//			if(this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i<model.trophies.Count)
//			{
//				this.trophiesDisplayed.Add (this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i);
//				this.resultsContents[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=model.trophies[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].;
//				this.resultsContents[i].transform.FindChild("Picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnThumbPicture(model.challengesRecords[this.resultsPagination.chosenPage*this.resultsPagination.nbElementsPerPage+i].Friend.idProfilePicture);
//				this.resultsContents[i].SetActive(true);
//			}
//			else
//			{
//				this.resultsContents[i].SetActive(false);
//			}
//		}
//	}
//	public void drawConfrontations()
//	{
//		this.confrontationsDisplayed = new List<int> ();
//		for(int i =0;i<elementsPerPageConfrontations;i++)
//		{
//			if(this.chosenPageConfrontations*this.elementsPerPageConfrontations+i<model.confrontations.Count)
//			{
//				this.confrontationsDisplayed.Add (this.chosenPageConfrontations*this.elementsPerPageConfrontations+i);
//				this.confrontations[i].GetComponent<ConfrontationController>().r=model.confrontations[this.chosenPageConfrontations*this.elementsPerPageConfrontations+i];
//				bool hasWon;
//				if(model.confrontations[this.chosenPageConfrontations*this.elementsPerPageConfrontations+i].IdWinner==model.player.Id)
//				{
//					hasWon=true;
//				}
//				else
//				{
//					hasWon=false;
//				}
//				this.confrontations[i].GetComponent<ConfrontationController>().show(hasWon);
//				this.confrontations[i].SetActive(true);
//			}
//			else
//			{
//				this.confrontations[i].SetActive(false);
//			}
//		}
//	}
//	public void drawStats()
//	{
//		this.stats.transform.FindChild ("nbWins").FindChild ("Value").GetComponent<TextMeshPro> ().text = model.player.TotalNbWins.ToString ();
//		this.stats.transform.FindChild ("nbWins").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Victoires";
//		this.stats.transform.FindChild ("nbLooses").FindChild ("Value").GetComponent<TextMeshPro> ().text = model.player.TotalNbLooses.ToString ();
//		this.stats.transform.FindChild ("nbLooses").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Défaites";
//		this.stats.transform.FindChild ("ranking").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Classement combattant";
//		this.stats.transform.FindChild ("ranking").FindChild ("Value").GetComponent<TextMeshPro> ().text = model.player.Ranking.ToString ();
//		this.stats.transform.FindChild ("ranking").FindChild ("Title2").GetComponent<TextMeshPro> ().text = "("+model.player.RankingPoints.ToString()+" pts)";
//		this.stats.transform.FindChild ("collectionPoints").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Classement collectionneur";
//		this.stats.transform.FindChild ("collectionPoints").FindChild ("Value").GetComponent<TextMeshPro> ().text = model.player.CollectionRanking.ToString ();
//		this.stats.transform.FindChild ("collectionPoints").FindChild ("Title2").GetComponent<TextMeshPro> ().text = "("+model.player.CollectionPoints.ToString()+" pts)";
//	}
//	private void drawProfile()
//	{
//		this.profile.transform.FindChild ("Username").GetComponent<TextMeshPro> ().text = model.player.Username;
//		if(this.isMyProfile)
//		{
//			this.drawPersonalInformations ();
//		}
//		this.drawProfilePicture ();
//	}
//	private void drawPersonalInformations()
//	{
//		this.profile.transform.FindChild ("Informations").GetComponent<TextMeshPro> ().text = "Prénom : " + model.player.FirstName + "\nNom : " + model.player.Surname + "\neMail : " + model.player.Mail;
//	}
//	private void drawProfilePicture()
//	{
//		this.profile.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.profilePictures[model.player.idProfilePicture];
//	}
//	private void drawPaginationChallengesRecords()
//	{
//		for(int i=0;i<this.paginationButtonsChallengesRecords.Length;i++)
//		{
//			Destroy (this.paginationButtonsChallengesRecords[i]);
//		}
//		this.paginationButtonsChallengesRecords = new GameObject[0];
//		this.activePaginationButtonIdChallengesRecords = -1;
//		float paginationButtonWidth = 0.34f;
//		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
//		this.nbPagesChallengesRecords = Mathf.CeilToInt((float)model.challengesRecords.Count / ((float)this.elementsPerPageChallengesRecords));
//		if(this.nbPagesChallengesRecords>1)
//		{
//			this.nbPaginationButtonsLimitChallengesRecords = Mathf.CeilToInt((2.4f)/(paginationButtonWidth+gapBetweenPaginationButton));
//			int nbButtonsToDraw=0;
//			bool drawBackButton=false;
//			if (this.pageDebutChallengesRecords !=0)
//			{
//				drawBackButton=true;
//			}
//			bool drawNextButton=false;
//			if (this.pageDebutChallengesRecords+nbPaginationButtonsLimitChallengesRecords-System.Convert.ToInt32(drawBackButton)<this.nbPagesChallengesRecords-1)
//			{
//				drawNextButton=true;
//				nbButtonsToDraw=nbPaginationButtonsLimitChallengesRecords;
//			}
//			else
//			{
//				nbButtonsToDraw=this.nbPagesChallengesRecords-this.pageDebutChallengesRecords;
//				if(drawBackButton)
//				{
//					nbButtonsToDraw++;
//				}
//			}
//			this.paginationButtonsChallengesRecords = new GameObject[nbButtonsToDraw];
//			for(int i =0;i<nbButtonsToDraw;i++)
//			{
//				this.paginationButtonsChallengesRecords[i] = Instantiate(this.paginationButtonObject) as GameObject;
//				this.paginationButtonsChallengesRecords[i].AddComponent<ProfileChallengesRecordsPaginationController>();
//				this.paginationButtonsChallengesRecords[i].transform.position=new Vector3(this.challengesRecordsBlock.transform.position.x+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
//				this.paginationButtonsChallengesRecords[i].name="PaginationChallengesRecord"+i.ToString();
//			}
//			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
//			{
//				this.paginationButtonsChallengesRecords[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutChallengesRecords+i-System.Convert.ToInt32(drawBackButton)).ToString();
//				this.paginationButtonsChallengesRecords[i].GetComponent<ProfileChallengesRecordsPaginationController>().setId(i);
//				if(this.pageDebutChallengesRecords+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageChallengesRecords)
//				{
//					this.paginationButtonsChallengesRecords[i].GetComponent<ProfileChallengesRecordsPaginationController>().setActive(true);
//					this.activePaginationButtonIdChallengesRecords=i;
//				}
//			}
//			if(drawBackButton)
//			{
//				this.paginationButtonsChallengesRecords[0].GetComponent<ProfileChallengesRecordsPaginationController>().setId(-2);
//				this.paginationButtonsChallengesRecords[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
//			}
//			if(drawNextButton)
//			{
//				this.paginationButtonsChallengesRecords[nbButtonsToDraw-1].GetComponent<ProfileChallengesRecordsPaginationController>().setId(-1);
//				this.paginationButtonsChallengesRecords[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
//			}
//		}
//	}
//	public void paginationHandlerChallengesRecords(int id)
//	{
//		if(id==-2)
//		{
//			this.pageDebutChallengesRecords=this.pageDebutChallengesRecords-this.nbPaginationButtonsLimitChallengesRecords+1+System.Convert.ToInt32(this.pageDebutChallengesRecords-this.nbPaginationButtonsLimitChallengesRecords+1!=0);
//			this.drawPaginationChallengesRecords();
//		}
//		else if(id==-1)
//		{
//			this.pageDebutChallengesRecords=this.pageDebutChallengesRecords+this.nbPaginationButtonsLimitChallengesRecords-1-System.Convert.ToInt32(this.pageDebutChallengesRecords!=0);
//			this.drawPaginationChallengesRecords();
//		}
//		else
//		{
//			if(activePaginationButtonIdChallengesRecords!=-1)
//			{
//				this.paginationButtonsChallengesRecords[this.activePaginationButtonIdChallengesRecords].GetComponent<ProfileChallengesRecordsPaginationController>().setActive(false);
//			}
//			this.activePaginationButtonIdChallengesRecords=id;
//			this.chosenPageChallengesRecords=this.pageDebutChallengesRecords-System.Convert.ToInt32(this.pageDebutChallengesRecords!=0)+id;
//			this.drawChallengesRecords();
//		}
//	}
//	private void drawPaginationConfrontations()
//	{
//		for(int i=0;i<this.paginationButtonsConfrontations.Length;i++)
//		{
//			Destroy (this.paginationButtonsConfrontations[i]);
//		}
//		this.paginationButtonsConfrontations = new GameObject[0];
//		this.activePaginationButtonIdConfrontations = -1;
//		float paginationButtonWidth = 0.34f;
//		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
//		this.nbPagesConfrontations = Mathf.CeilToInt((float)model.confrontations.Count / ((float)this.elementsPerPageConfrontations));
//		if(this.nbPagesConfrontations>1)
//		{
//			this.nbPaginationButtonsLimitConfrontations = Mathf.CeilToInt((2.4f)/(paginationButtonWidth+gapBetweenPaginationButton));
//			int nbButtonsToDraw=0;
//			bool drawBackButton=false;
//			if (this.pageDebutConfrontations !=0)
//			{
//				drawBackButton=true;
//			}
//			bool drawNextButton=false;
//			if (this.pageDebutConfrontations+nbPaginationButtonsLimitConfrontations-System.Convert.ToInt32(drawBackButton)<this.nbPagesConfrontations-1)
//			{
//				drawNextButton=true;
//				nbButtonsToDraw=nbPaginationButtonsLimitConfrontations;
//			}
//			else
//			{
//				nbButtonsToDraw=this.nbPagesConfrontations-this.pageDebutConfrontations;
//				if(drawBackButton)
//				{
//					nbButtonsToDraw++;
//				}
//			}
//			this.paginationButtonsConfrontations = new GameObject[nbButtonsToDraw];
//			for(int i =0;i<nbButtonsToDraw;i++)
//			{
//				this.paginationButtonsConfrontations[i] = Instantiate(this.paginationButtonObject) as GameObject;
//				this.paginationButtonsConfrontations[i].AddComponent<ProfileConfrontationsPaginationController>();
//				this.paginationButtonsConfrontations[i].transform.position=new Vector3(this.confrontationsBlock.transform.position.x+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
//				this.paginationButtonsConfrontations[i].name="PaginationChallengesRecord"+i.ToString();
//			}
//			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
//			{
//				this.paginationButtonsConfrontations[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutConfrontations+i-System.Convert.ToInt32(drawBackButton)).ToString();
//				this.paginationButtonsConfrontations[i].GetComponent<ProfileConfrontationsPaginationController>().setId(i);
//				if(this.pageDebutConfrontations+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageConfrontations)
//				{
//					this.paginationButtonsConfrontations[i].GetComponent<ProfileConfrontationsPaginationController>().setActive(true);
//					this.activePaginationButtonIdConfrontations=i;
//				}
//			}
//			if(drawBackButton)
//			{
//				this.paginationButtonsConfrontations[0].GetComponent<ProfileConfrontationsPaginationController>().setId(-2);
//				this.paginationButtonsConfrontations[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
//			}
//			if(drawNextButton)
//			{
//				this.paginationButtonsConfrontations[nbButtonsToDraw-1].GetComponent<ProfileConfrontationsPaginationController>().setId(-1);
//				this.paginationButtonsConfrontations[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
//			}
//		}
//	}
//	public void paginationHandlerConfrontations(int id)
//	{
//		if(id==-2)
//		{
//			this.pageDebutConfrontations=this.pageDebutConfrontations-this.nbPaginationButtonsLimitConfrontations+1+System.Convert.ToInt32(this.pageDebutConfrontations-this.nbPaginationButtonsLimitConfrontations+1!=0);
//			this.drawPaginationConfrontations();
//		}
//		else if(id==-1)
//		{
//			this.pageDebutConfrontations=this.pageDebutConfrontations+this.nbPaginationButtonsLimitConfrontations-1-System.Convert.ToInt32(this.pageDebutConfrontations!=0);
//			this.drawPaginationConfrontations();
//		}
//		else
//		{
//			if(activePaginationButtonIdConfrontations!=-1)
//			{
//				this.paginationButtonsConfrontations[this.activePaginationButtonIdConfrontations].GetComponent<ProfileConfrontationsPaginationController>().setActive(false);
//			}
//			this.activePaginationButtonIdConfrontations=id;
//			this.chosenPageConfrontations=this.pageDebutConfrontations-System.Convert.ToInt32(this.pageDebutConfrontations!=0)+id;
//			this.drawConfrontations();
//		}
//	}
//	private void drawPaginationFriendsRequests()
//	{
//		for(int i=0;i<this.paginationButtonsFriendsRequests.Length;i++)
//		{
//			Destroy (this.paginationButtonsFriendsRequests[i]);
//		}
//		this.paginationButtonsFriendsRequests = new GameObject[0];
//		this.activePaginationButtonIdFriendsRequests = -1;
//		float paginationButtonWidth = 0.34f;
//		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
//		this.nbPagesFriendsRequests = Mathf.CeilToInt((float)model.friendsRequests.Count / ((float)this.elementsPerPageFriendsRequests));
//		if(this.nbPagesFriendsRequests>1)
//		{
//			this.nbPaginationButtonsLimitFriendsRequests = Mathf.CeilToInt((2.4f)/(paginationButtonWidth+gapBetweenPaginationButton));
//			int nbButtonsToDraw=0;
//			bool drawBackButton=false;
//			if (this.pageDebutFriendsRequests !=0)
//			{
//				drawBackButton=true;
//			}
//			bool drawNextButton=false;
//			if (this.pageDebutFriendsRequests+nbPaginationButtonsLimitFriendsRequests-System.Convert.ToInt32(drawBackButton)<this.nbPagesFriendsRequests-1)
//			{
//				drawNextButton=true;
//				nbButtonsToDraw=nbPaginationButtonsLimitFriendsRequests;
//			}
//			else
//			{
//				nbButtonsToDraw=this.nbPagesFriendsRequests-this.pageDebutFriendsRequests;
//				if(drawBackButton)
//				{
//					nbButtonsToDraw++;
//				}
//			}
//			this.paginationButtonsFriendsRequests = new GameObject[nbButtonsToDraw];
//			for(int i =0;i<nbButtonsToDraw;i++)
//			{
//				this.paginationButtonsFriendsRequests[i] = Instantiate(this.paginationButtonObject) as GameObject;
//				this.paginationButtonsFriendsRequests[i].AddComponent<ProfileFriendsRequestsPaginationController>();
//				this.paginationButtonsFriendsRequests[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
//				this.paginationButtonsFriendsRequests[i].name="PaginationFriends"+i.ToString();
//			}
//			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
//			{
//				this.paginationButtonsFriendsRequests[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutFriendsRequests+i-System.Convert.ToInt32(drawBackButton)).ToString();
//				this.paginationButtonsFriendsRequests[i].GetComponent<ProfileFriendsRequestsPaginationController>().setId(i);
//				if(this.pageDebutFriendsRequests+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageFriendsRequests)
//				{
//					this.paginationButtonsFriendsRequests[i].GetComponent<ProfileFriendsRequestsPaginationController>().setActive(true);
//					this.activePaginationButtonIdFriendsRequests=i;
//				}
//			}
//			if(drawBackButton)
//			{
//				this.paginationButtonsFriendsRequests[0].GetComponent<ProfileFriendsRequestsPaginationController>().setId(-2);
//				this.paginationButtonsFriendsRequests[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
//			}
//			if(drawNextButton)
//			{
//				this.paginationButtonsFriendsRequests[nbButtonsToDraw-1].GetComponent<ProfileFriendsRequestsPaginationController>().setId(-1);
//				this.paginationButtonsFriendsRequests[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
//			}
//		}
//	}
//	public void paginationHandlerFriendsRequests(int id)
//	{
//		if(id==-2)
//		{
//			this.pageDebutFriendsRequests=this.pageDebutFriendsRequests-this.nbPaginationButtonsLimitFriendsRequests+1+System.Convert.ToInt32(this.pageDebutFriendsRequests-this.nbPaginationButtonsLimitFriendsRequests+1!=0);
//			this.drawPaginationFriendsRequests();
//		}
//		else if(id==-1)
//		{
//			this.pageDebutFriendsRequests=this.pageDebutFriendsRequests+this.nbPaginationButtonsLimitFriendsRequests-1-System.Convert.ToInt32(this.pageDebutFriendsRequests!=0);
//			this.drawPaginationFriendsRequests();
//		}
//		else
//		{
//			if(activePaginationButtonIdFriendsRequests!=-1)
//			{
//				this.paginationButtonsFriendsRequests[this.activePaginationButtonIdFriendsRequests].GetComponent<ProfileFriendsRequestsPaginationController>().setActive(false);
//			}
//			this.activePaginationButtonIdFriendsRequests=id;
//			this.chosenPageFriendsRequests=this.pageDebutFriendsRequests-System.Convert.ToInt32(this.pageDebutFriendsRequests!=0)+id;
//			this.drawFriendsRequests();
//		}
//	}
//	private void drawPaginationTrophies()
//	{
//		for(int i=0;i<this.paginationButtonsTrophies.Length;i++)
//		{
//			Destroy (this.paginationButtonsTrophies[i]);
//		}
//		this.paginationButtonsTrophies = new GameObject[0];
//		this.activePaginationButtonIdTrophies = -1;
//		float paginationButtonWidth = 0.34f;
//		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
//		this.nbPagesTrophies = Mathf.CeilToInt((float)model.trophies.Count / ((float)this.elementsPerPageTrophies));
//		if(this.nbPagesTrophies>1)
//		{
//			this.nbPaginationButtonsLimitTrophies = Mathf.CeilToInt((2.4f)/(paginationButtonWidth+gapBetweenPaginationButton));
//			int nbButtonsToDraw=0;
//			bool drawBackButton=false;
//			if (this.pageDebutTrophies !=0)
//			{
//				drawBackButton=true;
//			}
//			bool drawNextButton=false;
//			if (this.pageDebutTrophies+nbPaginationButtonsLimitTrophies-System.Convert.ToInt32(drawBackButton)<this.nbPagesTrophies-1)
//			{
//				drawNextButton=true;
//				nbButtonsToDraw=nbPaginationButtonsLimitTrophies;
//			}
//			else
//			{
//				nbButtonsToDraw=this.nbPagesTrophies-this.pageDebutTrophies;
//				if(drawBackButton)
//				{
//					nbButtonsToDraw++;
//				}
//			}
//			this.paginationButtonsTrophies = new GameObject[nbButtonsToDraw];
//			for(int i =0;i<nbButtonsToDraw;i++)
//			{
//				this.paginationButtonsTrophies[i] = Instantiate(this.paginationButtonObject) as GameObject;
//				this.paginationButtonsTrophies[i].AddComponent<ProfileTrophiesPaginationController>();
//				this.paginationButtonsTrophies[i].transform.position=new Vector3(this.trophiesBlock.transform.position.x+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
//				this.paginationButtonsTrophies[i].name="PaginationTrophies"+i.ToString();
//			}
//			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
//			{
//				this.paginationButtonsTrophies[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutTrophies+i-System.Convert.ToInt32(drawBackButton)).ToString();
//				this.paginationButtonsTrophies[i].GetComponent<ProfileTrophiesPaginationController>().setId(i);
//				if(this.pageDebutTrophies+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageTrophies)
//				{
//					this.paginationButtonsTrophies[i].GetComponent<ProfileTrophiesPaginationController>().setActive(true);
//					this.activePaginationButtonIdTrophies=i;
//				}
//			}
//			if(drawBackButton)
//			{
//				this.paginationButtonsTrophies[0].GetComponent<ProfileTrophiesPaginationController>().setId(-2);
//				this.paginationButtonsTrophies[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
//			}
//			if(drawNextButton)
//			{
//				this.paginationButtonsTrophies[nbButtonsToDraw-1].GetComponent<ProfileTrophiesPaginationController>().setId(-1);
//				this.paginationButtonsTrophies[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
//			}
//		}
//	}
//	public void paginationHandlerTrophies(int id)
//	{
//		if(id==-2)
//		{
//			this.pageDebutTrophies=this.pageDebutTrophies-this.nbPaginationButtonsLimitTrophies+1+System.Convert.ToInt32(this.pageDebutTrophies-this.nbPaginationButtonsLimitTrophies+1!=0);
//			this.drawPaginationTrophies();
//		}
//		else if(id==-1)
//		{
//			this.pageDebutTrophies=this.pageDebutTrophies+this.nbPaginationButtonsLimitTrophies-1-System.Convert.ToInt32(this.pageDebutTrophies!=0);
//			this.drawPaginationTrophies();
//		}
//		else
//		{
//			if(activePaginationButtonIdTrophies!=-1)
//			{
//				this.paginationButtonsTrophies[this.activePaginationButtonIdTrophies].GetComponent<ProfileTrophiesPaginationController>().setActive(false);
//			}
//			this.activePaginationButtonIdTrophies=id;
//			this.chosenPageTrophies=this.pageDebutTrophies-System.Convert.ToInt32(this.pageDebutTrophies!=0)+id;
//			this.drawTrophies();
//		}
//	}
//	private void drawPaginationFriends()
//	{
//		for(int i=0;i<this.paginationButtonsFriends.Length;i++)
//		{
//			Destroy (this.paginationButtonsFriends[i]);
//		}
//		this.paginationButtonsFriends = new GameObject[0];
//		this.activePaginationButtonIdFriends = -1;
//		float paginationButtonWidth = 0.34f;
//		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
//		this.nbPagesFriends = Mathf.CeilToInt((float)this.friendsToBeDisplayed.Count / ((float)this.elementsPerPageFriends));
//		if(this.nbPagesFriends>1)
//		{
//			this.nbPaginationButtonsLimitFriends = Mathf.CeilToInt((2.4f)/(paginationButtonWidth+gapBetweenPaginationButton));
//			int nbButtonsToDraw=0;
//			bool drawBackButton=false;
//			if (this.pageDebutFriends !=0)
//			{
//				drawBackButton=true;
//			}
//			bool drawNextButton=false;
//			if (this.pageDebutFriends+nbPaginationButtonsLimitFriends-System.Convert.ToInt32(drawBackButton)<this.nbPagesFriends-1)
//			{
//				drawNextButton=true;
//				nbButtonsToDraw=nbPaginationButtonsLimitFriends;
//			}
//			else
//			{
//				nbButtonsToDraw=this.nbPagesFriends-this.pageDebutFriends;
//				if(drawBackButton)
//				{
//					nbButtonsToDraw++;
//				}
//			}
//			this.paginationButtonsFriends = new GameObject[nbButtonsToDraw];
//			for(int i =0;i<nbButtonsToDraw;i++)
//			{
//				this.paginationButtonsFriends[i] = Instantiate(this.paginationButtonObject) as GameObject;
//				this.paginationButtonsFriends[i].AddComponent<ProfileFriendsPaginationController>();
//				if(this.isMyProfile)
//				{
//					this.paginationButtonsFriends[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-2.15f,0f);
//				}
//				else
//				{
//					this.paginationButtonsFriends[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
//				}
//				this.paginationButtonsFriends[i].name="PaginationFriends"+i.ToString();
//			}
//			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
//			{
//				this.paginationButtonsFriends[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutFriends+i-System.Convert.ToInt32(drawBackButton)).ToString();
//				this.paginationButtonsFriends[i].GetComponent<ProfileFriendsPaginationController>().setId(i);
//				if(this.pageDebutFriends+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageFriends)
//				{
//					this.paginationButtonsFriends[i].GetComponent<ProfileFriendsPaginationController>().setActive(true);
//					this.activePaginationButtonIdFriends=i;
//				}
//			}
//			if(drawBackButton)
//			{
//				this.paginationButtonsFriends[0].GetComponent<ProfileFriendsPaginationController>().setId(-2);
//				this.paginationButtonsFriends[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
//			}
//			if(drawNextButton)
//			{
//				this.paginationButtonsFriends[nbButtonsToDraw-1].GetComponent<ProfileFriendsPaginationController>().setId(-1);
//				this.paginationButtonsFriends[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
//			}
//		}
//	}
//	public void paginationHandlerFriends(int id)
//	{
//		if(id==-2)
//		{
//			this.pageDebutFriends=this.pageDebutFriends-this.nbPaginationButtonsLimitFriends+1+System.Convert.ToInt32(this.pageDebutFriends-this.nbPaginationButtonsLimitFriends+1!=0);
//			this.drawPaginationFriends();
//		}
//		else if(id==-1)
//		{
//			this.pageDebutFriends=this.pageDebutFriends+this.nbPaginationButtonsLimitFriends-1-System.Convert.ToInt32(this.pageDebutFriends!=0);
//			this.drawPaginationFriends();
//		}
//		else
//		{
//			if(activePaginationButtonIdFriends!=-1)
//			{
//				this.paginationButtonsFriends[this.activePaginationButtonIdFriends].GetComponent<ProfileFriendsPaginationController>().setActive(false);
//			}
//			this.activePaginationButtonIdFriends=id;
//			this.chosenPageFriends=this.pageDebutFriends-System.Convert.ToInt32(this.pageDebutFriends!=0)+id;
//			this.drawFriends();
//		}
//	}
//	public void startHoveringPopUp()
//	{
//		this.isHoveringPopUp = true;
//		this.toDestroyPopUp = false;
//		this.popUpDestroyInterval = 0f;
//	}
//	public void endHoveringPopUp()
//	{
//		this.isHoveringPopUp = false;
//		this.toDestroyPopUp = true;
//		this.popUpDestroyInterval = 0f;
//	}
//	public void startHoveringFriendsRequest (int id)
//	{
//		this.idFriendsRequestHovered=id;
//		this.isHoveringFriendsRequest = true;
//		if(this.isPopUpDisplayed && this.popUp.GetComponent<PopUpController>().getIsFriendsRequest())
//		{
//			if(this.popUp.GetComponent<PopUpFriendsRequestProfileController>().getId()!=this.idFriendsRequestHovered);
//			{
//				this.hidePopUp();
//				this.showPopUpFriendsRequest();
//			}
//		}
//		else
//		{
//			if(this.isPopUpDisplayed)
//			{
//				this.hidePopUp();
//			}
//			this.showPopUpFriendsRequest();
//		}
//	}
//	public void startHoveringFriend (int id)
//	{
//		this.idFriendHovered=id;
//		this.isHoveringFriend = true;
//		if(this.isPopUpDisplayed && this.popUp.GetComponent<PopUpController>().getIsFriend())
//		{
//			if(this.popUp.GetComponent<PopUpFriendProfileController>().getId()!=this.idFriendHovered);
//			{
//				this.hidePopUp();
//				this.showPopUpFriend();
//			}
//		}
//		else
//		{
//			if(this.isPopUpDisplayed)
//			{
//				this.hidePopUp();
//			}
//			this.showPopUpFriend();
//		}
//	}
//	public void startHoveringChallengesRecord (int id)
//	{
//		this.idChallengesRecordHovered=id;
//		this.isHoveringChallengesRecord = true;
//		if(this.isPopUpDisplayed && this.popUp.GetComponent<PopUpController>().getIsChallengesRecord())
//		{
//			if(this.popUp.GetComponent<PopUpChallengesRecordProfileController>().getId()!=this.idChallengesRecordHovered);
//			{
//				this.hidePopUp();
//				this.showPopUpChallengesRecord();
//			}
//		}
//		else
//		{
//			if(this.isPopUpDisplayed)
//			{
//				this.hidePopUp();
//			}
//			this.showPopUpChallengesRecord();
//		}
//	}
//	public void endHoveringFriendsRequest ()
//	{
//		this.isHoveringFriendsRequest = false;
//		this.toDestroyPopUp = true;
//		this.popUpDestroyInterval = 0f;
//	}
//	public void endHoveringFriend ()
//	{
//		this.isHoveringFriend = false;
//		this.toDestroyPopUp = true;
//		this.popUpDestroyInterval = 0f;
//	}
//	public void endHoveringChallengesRecord ()
//	{
//		this.isHoveringChallengesRecord = false;
//		this.toDestroyPopUp = true;
//		this.popUpDestroyInterval = 0f;
//	}
//	public void showPopUpFriendsRequest()
//	{
//		this.popUp = Instantiate(this.popUpObject) as GameObject;
//		this.popUp.transform.position=new Vector3(this.friendsRequests[this.idFriendsRequestHovered].transform.position.x-3.1f,this.friendsRequests[this.idFriendsRequestHovered].transform.position.y,-1f);
//		this.popUp.AddComponent<PopUpFriendsRequestProfileController>();
//		this.popUp.GetComponent<PopUpFriendsRequestProfileController> ().setIsFriendsRequest (true);
//		this.popUp.GetComponent<PopUpFriendsRequestProfileController> ().setId (this.idFriendsRequestHovered);
//		this.popUp.GetComponent<PopUpFriendsRequestProfileController> ().show (model.friendsRequests [this.friendsRequestsDisplayed [this.idFriendsRequestHovered]]);
//		this.isPopUpDisplayed=true;
//	}
//	public void showPopUpFriend()
//	{
//		this.popUp = Instantiate(this.popUpObject) as GameObject;
//		this.popUp.transform.position=new Vector3(this.friends[this.idFriendHovered].transform.position.x-3.1f,this.friends[this.idFriendHovered].transform.position.y,-1f);
//		this.popUp.AddComponent<PopUpFriendProfileController>();
//		this.popUp.GetComponent<PopUpFriendProfileController> ().setIsFriend (true);
//		this.popUp.GetComponent<PopUpFriendProfileController> ().setId (this.idFriendHovered);
//		if(this.isMyProfile)
//		{
//			this.popUp.GetComponent<PopUpFriendProfileController> ().show (model.users [this.friendsToBeDisplayed[this.friendsDisplayed[this.idFriendHovered]]]);
//		}
//		else
//		{
//			this.popUp.GetComponent<PopUpFriendProfileController> ().show2 (model.users [this.friendsToBeDisplayed[this.friendsDisplayed[this.idFriendHovered]]]);
//		}
//		this.isPopUpDisplayed=true;
//	}
//	public void showPopUpChallengesRecord()
//	{
//		this.popUp = Instantiate(this.popUpObject) as GameObject;
//		this.popUp.transform.position=new Vector3(this.challengesRecords[this.idChallengesRecordHovered].transform.position.x-3.1f,this.challengesRecords[this.idChallengesRecordHovered].transform.position.y,-1f);
//		this.popUp.AddComponent<PopUpChallengesRecordProfileController>();
//		this.popUp.GetComponent<PopUpChallengesRecordProfileController> ().setIsChallengesRecord (true);
//		this.popUp.GetComponent<PopUpChallengesRecordProfileController> ().setId (this.idChallengesRecordHovered);
//		this.popUp.GetComponent<PopUpChallengesRecordProfileController> ().show (model.challengesRecords [this.challengesRecordsDisplayed [this.idChallengesRecordHovered]]);
//		this.isPopUpDisplayed=true;
//	}
//	public void hidePopUp()
//	{
//		this.toDestroyPopUp = false;
//		this.popUpDestroyInterval = 0f;
//		Destroy (this.popUp);
//		this.isPopUpDisplayed=false;
//	}
//	public void checkFriendsOnlineStatus()
//	{
//		if(PhotonNetwork.insideLobby)
//		{
//			PhotonNetwork.FindFriends (model.usernameList);
//		}
//	}
//	public void OnUpdatedFriendList()
//	{
//		if(this.isMyProfile)
//		{
//			for(int i=0;i<PhotonNetwork.Friends.Count;i++)
//			{
//				for(int j=0;j<model.users.Count;j++)
//				{
//					if(model.users[j].Username==PhotonNetwork.Friends[i].Name)
//					{
//						if(PhotonNetwork.Friends[i].IsInRoom)
//						{
//							if(model.friends.Contains(j))
//							{
//								if(!this.friendsOnline.Contains(j))
//								{
//									this.friendsOnline.Insert(0,j);
//									model.users[j].OnlineStatus=2;
//								}
//								else if(model.users[j].OnlineStatus!=2)
//								{
//									this.friendsOnline.Remove(j);
//									this.friendsOnline.Insert(0,j);
//									model.users[j].OnlineStatus=2;
//								}
//							}
//						}
//						else if(PhotonNetwork.Friends[i].IsOnline)
//						{
//							if(model.friends.Contains(j))
//							{
//								if(!this.friendsOnline.Contains(j))
//								{
//									this.friendsOnline.Insert(0,j);
//									model.users[j].OnlineStatus=1;
//								}
//								else if(model.users[j].OnlineStatus!=1)
//								{
//									this.friendsOnline.Remove(j);
//									this.friendsOnline.Insert(0,j);
//									model.users[j].OnlineStatus=1;
//								}
//							}
//						}
//						else
//						{
//							model.users[j].OnlineStatus=0;
//						}
//						break;
//					}
//				}
//			}
//			if(this.chosenPageFriends == 0)
//			{
//				this.initializeFriends();
//			}
//		}
//	}
//	public void sortFriendsList()
//	{
//		this.friendsToBeDisplayed = new List<int> ();
//		if(this.isMyProfile)
//		{
//			for(int i=0;i<this.friendsOnline.Count;i++)
//			{
//				this.friendsToBeDisplayed.Add (this.friendsOnline[i]);
//			}
//		}
//		for(int i=0;i<model.friends.Count;i++)
//		{
//			if(!this.friendsToBeDisplayed.Contains(model.friends[i]))
//			{
//				this.friendsToBeDisplayed.Add(model.friends[i]);
//			}
//		}
//	}
//	public void sendInvitationHandler()
//	{
//		if(!model.hasDeck)
//		{
//			newMenuController.instance.displayErrorPopUp("Vous ne pouvez lancer de match sans avoir au préalable créé un deck");
//		}
//		else if(model.users [this.friendsToBeDisplayed[this.friendsDisplayed[this.idFriendHovered]]].OnlineStatus!=1)
//		{
//			newMenuController.instance.displayErrorPopUp("Votre adversaire n'est plus disponible");
//		}
//		else
//		{
//			StartCoroutine (this.sendInvitation ());
//		}
//	}
//	public IEnumerator sendInvitation()
//	{
//		newMenuController.instance.displayLoadingScreen ();
//		// yield return StartCoroutine (model.player.SetSelectedDeck (model.decks [this.deckDisplayed].Id));
//		StartCoroutine (newMenuController.instance.sendInvitation (model.users [this.friendsToBeDisplayed[this.friendsDisplayed[this.idFriendHovered]]], model.player));
//		yield break;
//	}
//	public void acceptFriendsRequestHandler()
//	{
//		this.hidePopUp ();
//		StartCoroutine (this.confirmFriendRequest ());
//	}
//	public void declineFriendsRequestHandler()
//	{
//		this.hidePopUp ();
//		StartCoroutine (this.removeFriendRequest ());
//	}
//	public void cancelFriendsRequestHandler()
//	{
//		this.hidePopUp ();
//		StartCoroutine (this.removeFriendRequest ());
//	}
//	public void startHoveringProfilePicture()
//	{
//		this.isProfilePictureHovered = true;
//		this.profile.transform.FindChild ("PictureButton").gameObject.SetActive (true);
//	}
//	public void endHoveringProfilePicture()
//	{
//		this.isProfilePictureHovered = false;
//		this.profile.transform.FindChild ("PictureButton").gameObject.SetActive (false);
//	}
//	public void editProfilePictureHandler()
//	{
//		this.displaySelectPicturePopUp ();
//	}
//	private void displaySelectPicturePopUp()
//	{
//		newMenuController.instance.displayTransparentBackground ();
//		this.selectPicturePopUp=Instantiate(this.selectPicturePopUpObject) as GameObject;
//		this.selectPicturePopUp.transform.position = new Vector3 (0f, 0f, -2f);
//		this.selectPicturePopUp.GetComponent<SelectPicturePopUpController> ().selectPicture (model.player.idProfilePicture);
//		this.isSelectPicturePopUpDisplayed = true;
//	}
//	public void hideSelectPicturePopUp()
//	{
//		Destroy (this.selectPicturePopUp);
//		newMenuController.instance.hideTransparentBackground ();
//		this.isSelectPicturePopUpDisplayed = false;
//	}
//	public void changeUserPictureHandler(int id)
//	{
//		this.hideSelectPicturePopUp ();
//		if(id!=model.player.idProfilePicture)
//		{
//			StartCoroutine(this.changeUserPicture(id));
//		}
//	}
//	public IEnumerator changeUserPicture(int id)
//	{
//		newMenuController.instance.displayLoadingScreen();
//		yield return StartCoroutine(model.player.setProfilePicture(id));
//		this.drawProfilePicture ();
//		newMenuController.instance.changeThumbPicture (id);
//		newMenuController.instance.hideLoadingScreen();
//	}
//	public void displayCheckPasswordPopUp()
//	{
//		this.checkPasswordView = gameObject.AddComponent<ProfileCheckPasswordPopUpView> ();
//		this.isCheckPasswordViewDisplayed = true;
//		checkPasswordView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.window);
//		checkPasswordView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
//		checkPasswordView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
//		checkPasswordView.popUpVM.centralWindowTextfieldStyle = new GUIStyle (this.popUpSkin.textField);
//		checkPasswordView.popUpVM.centralWindowErrorStyle = new GUIStyle (this.popUpSkin.customStyles [1]);
//		checkPasswordView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
//		this.checkPasswordViewResize ();
//	}
//	public void displayChangePasswordPopUp()
//	{
//		this.changePasswordView = gameObject.AddComponent<ProfileChangePasswordPopUpView> ();
//		this.isChangePasswordViewDisplayed = true;
//		changePasswordView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.window);
//		changePasswordView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
//		changePasswordView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
//		changePasswordView.popUpVM.centralWindowTextfieldStyle = new GUIStyle (this.popUpSkin.textField);
//		changePasswordView.popUpVM.centralWindowErrorStyle = new GUIStyle (this.popUpSkin.customStyles [1]);
//		changePasswordView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
//		this.changePasswordViewResize ();
//	}
//	public void displayEditInformationsPopUp()
//	{
//		this.editInformationsView = gameObject.AddComponent<ProfileEditInformationsPopUpView> ();
//		this.isEditInformationsViewDisplayed = true;
//		editInformationsView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.window);
//		editInformationsView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
//		editInformationsView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
//		editInformationsView.popUpVM.centralWindowTextfieldStyle = new GUIStyle (this.popUpSkin.textField);
//		editInformationsView.popUpVM.centralWindowErrorStyle = new GUIStyle (this.popUpSkin.customStyles [1]);
//		editInformationsView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
//		editInformationsView.editInformationsPopUpVM.tempFirstName = model.player.FirstName;
//		editInformationsView.editInformationsPopUpVM.tempSurname = model.player.Surname;
//		editInformationsView.editInformationsPopUpVM.tempMail = model.player.Mail;
//		this.editInformationsViewResize ();
//	}
//	public void checkPasswordViewResize()
//	{
//		checkPasswordView.popUpVM.centralWindow = this.centralWindow;
//		checkPasswordView.popUpVM.resize ();
//	}
//	public void changePasswordViewResize()
//	{
//		changePasswordView.popUpVM.centralWindow = this.centralWindow;
//		changePasswordView.popUpVM.resize ();
//	}
//	public void editInformationsViewResize()
//	{
//		editInformationsView.popUpVM.centralWindow = this.centralWindowEditInformations;
//		editInformationsView.popUpVM.resize ();
//	}
//	public void hideCheckPasswordPopUp()
//	{
//		Destroy (this.checkPasswordView);
//		this.isCheckPasswordViewDisplayed = false;
//	}
//	public void hideChangePasswordPopUp()
//	{
//		Destroy (this.changePasswordView);
//		this.isChangePasswordViewDisplayed = false;
//	}
//	public void hideEditInformationsPopUp()
//	{
//		Destroy (this.editInformationsView);
//		this.isEditInformationsViewDisplayed = false;
//	}
//	public void checkPasswordHandler(string password)
//	{
//		StartCoroutine (checkPassword (password));
//	}
//	private IEnumerator checkPassword(string password)
//	{
//		checkPasswordView.checkPasswordPopUpVM.error = this.checkPasswordComplexity (password);
//		if(checkPasswordView.checkPasswordPopUpVM.error=="")
//		{
//			checkPasswordView.popUpVM.guiEnabled = false;
//			yield return StartCoroutine(ApplicationModel.checkPassword(password));
//			if(ApplicationModel.error=="")
//			{
//				this.hideCheckPasswordPopUp();
//				this.displayChangePasswordPopUp();
//			}
//			else
//			{
//				checkPasswordView.checkPasswordPopUpVM.error=ApplicationModel.error;
//				ApplicationModel.error="";
//			}
//			checkPasswordView.popUpVM.guiEnabled = true;
//		}
//	}
//	public string checkPasswordComplexity(string password)
//	{
//		if(password.Length<5)
//		{
//			return "Le mot de passe doit comporter au moins 5 caractères";
//		}
//		else if(!Regex.IsMatch(password, @"^[a-zA-Z0-9_.@]+$"))
//		{
//			return "Le mot de passe ne peut comporter de caractères spéciaux hormis @ _ et .";
//		} 
//		return "";
//	}
//	public void editPasswordHandler()
//	{
//		changePasswordView.changePasswordPopUpVM.passwordsCheck = this.checkPasswordEgality (changePasswordView.changePasswordPopUpVM.tempNewPassword, changePasswordView.changePasswordPopUpVM.tempNewPassword2);
//		if(changePasswordView.changePasswordPopUpVM.passwordsCheck=="")
//		{
//			changePasswordView.changePasswordPopUpVM.passwordsCheck=this.checkPasswordComplexity(changePasswordView.changePasswordPopUpVM.tempNewPassword);
//		}
//		if(changePasswordView.changePasswordPopUpVM.passwordsCheck=="")
//		{
//			StartCoroutine(this.editPassword(changePasswordView.changePasswordPopUpVM.tempNewPassword));
//			changePasswordView.changePasswordPopUpVM.tempNewPassword="";
//			changePasswordView.changePasswordPopUpVM.tempNewPassword2="";
//		}
//	}
//	private IEnumerator editPassword(string password)
//	{
//		changePasswordView.popUpVM.guiEnabled = false;
//		yield return StartCoroutine(ApplicationModel.editPassword(password));
//		changePasswordView.popUpVM.guiEnabled = true;
//		this.hideChangePasswordPopUp ();
//	}
//	public string checkPasswordEgality (string password1, string password2)
//	{
//		if(password1=="")
//		{
//			return "Veuillez saisir un mot de passe";
//		}
//		else if(password2=="")
//		{
//			return "Veuillez confirmer votre mot de passe";
//		}
//		else if(password1!=password2)
//		{
//			return "Les deux mots de passes doivent être identiques";
//		}
//		return "";
//	}
//	public void updateUserInformationsHandler()
//	{
//		editInformationsView.editInformationsPopUpVM.error = this.checkname (editInformationsView.editInformationsPopUpVM.tempSurname);
//		if(editInformationsView.editInformationsPopUpVM.error=="")
//		{
//			editInformationsView.editInformationsPopUpVM.error = this.checkname (editInformationsView.editInformationsPopUpVM.tempFirstName);
//		}
//		if(editInformationsView.editInformationsPopUpVM.error=="")
//		{
//			editInformationsView.editInformationsPopUpVM.error = this.checkEmail (editInformationsView.editInformationsPopUpVM.tempMail);
//		}
//		if(editInformationsView.editInformationsPopUpVM.error=="")
//		{
//			StartCoroutine(updateUserInformations(editInformationsView.editInformationsPopUpVM.tempFirstName,editInformationsView.editInformationsPopUpVM.tempSurname,editInformationsView.editInformationsPopUpVM.tempMail));
//		}
//	}
//	private IEnumerator updateUserInformations(string firstname, string surname, string mail)
//	{
//		editInformationsView.popUpVM.guiEnabled = false;
//		model.player.FirstName = firstname;
//		model.player.Surname = surname;
//		model.player.Mail = mail;
//		yield return StartCoroutine (model.player.updateInformations ());
//		this.drawPersonalInformations ();
//		editInformationsView.popUpVM.guiEnabled = true;
//		this.hideEditInformationsPopUp ();
//	}
//	public string checkname(string name)
//	{
//		if(!Regex.IsMatch(name, @"^[a-zA-Z0-9_]+$"))
//		{
//			return "Vous ne pouvez pas utiliser de caractères spéciaux";
//		}   
//		return "";
//	}
//	public string checkEmail(string email)
//	{
//		if(!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
//		{
//			return "Veuillez saisir une adresse email valide";
//		}
//		return "";
//	}
//	public void mouseOnSearchBar(bool value)
//	{
//		this.isMouseOnSearchBar = value;
//	}
//	public void searchingUsers()
//	{
//		if(this.searchValue=="")
//		{
//			this.isSearchingUsers = true;
//			this.search.transform.FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text = this.searchValue;
//		}
//	}
//	public void searchUsersHandler()
//	{
//		if(this.searchValue.Length>2)
//		{
//			this.isSearchingUsers = false;
//			this.displaySearchUsersPopUp(this.searchValue);
//			this.searchValue = "";
//			this.search.transform.FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text ="Rechercher";
//		}
//	}
//	private void displaySearchUsersPopUp(string searchValue)
//	{
//		newMenuController.instance.displayTransparentBackground ();
//		this.searchUsersPopUp=Instantiate(this.searchUsersPopUpObject) as GameObject;
//		this.searchUsersPopUp.transform.position = new Vector3 (0f, 0f, -2f);
//		this.searchUsersPopUp.GetComponent<SearchUsersPopUpController> ().launch (searchValue);
//		this.isSearchUsersPopUpDisplayed = true;
//	}
//	public void hideSearchUsersPopUp()
//	{
//		Destroy (this.searchUsersPopUp);
//		newMenuController.instance.hideTransparentBackground ();
//		this.isSearchUsersPopUpDisplayed = false;
//	}
//	public IEnumerator confirmFriendRequest()
//	{
//		newMenuController.instance.displayLoadingScreen ();
//		yield return StartCoroutine(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.confirm ());
//		if(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.Error=="")
//		{
//			Notification tempNotification1 = new Notification(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id,model.activePlayerId,false,3);
//			StartCoroutine(tempNotification1.add ());
//			Notification tempNotification2 = new Notification(model.activePlayerId,model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id,false,4);
//			StartCoroutine(tempNotification2.remove ());
//			News tempNews1=new News(model.activePlayerId, 1,model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id.ToString());
//			StartCoroutine(tempNews1.add ());
//			News tempNews2=new News(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id, 1,model.activePlayerId.ToString());
//			StartCoroutine(tempNews2.add ());
//			model.moveToFriend(this.friendsRequestsDisplayed[this.idFriendsRequestHovered]);
//			this.initializeFriendsRequests();
//			this.initializeFriends();
//		}
//		else
//		{
//			newMenuController.instance.displayErrorPopUp(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.Error);
//			model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.Error="";
//		}
//		newMenuController.instance.hideLoadingScreen ();
//	}
//	public IEnumerator confirmConnection()
//	{
//		newMenuController.instance.displayLoadingScreen ();
//		yield return StartCoroutine(model.connectionWithMe.confirm ());
//		if(model.connectionWithMe.Error=="")
//		{
//			Notification tempNotification1 = new Notification(model.player.Id,model.activePlayerId,false,3);
//			StartCoroutine(tempNotification1.add ());
//			Notification tempNotification2 = new Notification(model.activePlayerId,model.player.Id,false,4);
//			StartCoroutine(tempNotification2.remove ());
//			News tempNews1=new News(model.activePlayerId, 1,model.player.Id.ToString());
//			StartCoroutine(tempNews1.add ());
//			News tempNews2=new News(model.player.Id, 1,model.activePlayerId.ToString());
//			StartCoroutine(tempNews2.add ());
//			this.initializeFriendshipState();
//			this.initializeFriends();
//		}
//		else
//		{
//			newMenuController.instance.displayErrorPopUp(model.connectionWithMe.Error);
//			model.connectionWithMe.Error="";
//		}
//		newMenuController.instance.hideLoadingScreen ();
//	}
//	public IEnumerator removeFriendRequest()
//	{
//		newMenuController.instance.displayLoadingScreen ();
//		yield return StartCoroutine(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.remove ());
//		if(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.Error=="")
//		{
//			Notification tempNotification = new Notification ();
//			tempNotification = new Notification(model.friendsRequests[this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id,model.activePlayerId,false,4);
//			StartCoroutine(tempNotification.remove ());
//			Notification tempNotification2 = new Notification ();
//			tempNotification2 = new Notification(model.activePlayerId,model.friendsRequests[this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id,false,4);
//			Notification tempNotification3 = new Notification ();
//			tempNotification3 = new Notification(model.friendsRequests[this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id,model.activePlayerId,false,3);
//			StartCoroutine(tempNotification3.remove ());
//			Notification tempNotification4 = new Notification ();
//			tempNotification4 = new Notification(model.activePlayerId,model.friendsRequests[this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id,false,3);
//			StartCoroutine(tempNotification4.remove ());
//			model.friendsRequests.RemoveAt(this.friendsRequestsDisplayed[this.idFriendsRequestHovered]);
//			this.initializeFriendsRequests();
//		}
//		else
//		{
//			newMenuController.instance.displayErrorPopUp(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.Error);
//			model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.Error="";
//		}
//		newMenuController.instance.hideLoadingScreen ();
//	}
//	public IEnumerator removeConnection()
//	{
//		newMenuController.instance.displayLoadingScreen ();
//		yield return StartCoroutine(model.connectionWithMe.remove ());
//		if(model.connectionWithMe.Error=="")
//		{
//			Notification tempNotification = new Notification ();
//			tempNotification = new Notification(model.player.Id,model.activePlayerId,false,4);
//			StartCoroutine(tempNotification.remove ());
//			Notification tempNotification2 = new Notification ();
//			tempNotification2 = new Notification(model.activePlayerId,model.player.Id,false,4);
//			StartCoroutine(tempNotification2.remove ());
//			Notification tempNotification3 = new Notification ();
//			tempNotification3 = new Notification(model.player.Id,model.activePlayerId,false,3);
//			StartCoroutine(tempNotification3.remove ());
//			Notification tempNotification4 = new Notification ();
//			tempNotification4 = new Notification(model.activePlayerId,model.player.Id,false,3);
//			StartCoroutine(tempNotification4.remove ());
//			model.isConnectedToMe=false;
//			this.initializeFriends();
//			this.initializeFriendshipState();
//		}
//		else
//		{
//			newMenuController.instance.displayErrorPopUp(model.connectionWithMe.Error);
//			model.connectionWithMe.Error="";
//		}
//		newMenuController.instance.hideLoadingScreen ();
//	}
//	public IEnumerator addConnection()
//	{
//		newMenuController.instance.displayLoadingScreen ();
//		Connection connection = new Connection ();
//		connection.IdUser1 = model.activePlayerId;
//		connection.IdUser2 = model.player.Id;
//		connection.IsAccepted = false;
//
//		yield return StartCoroutine(connection.add ());
//		if(connection.Error=="")
//		{
//			Notification tempNotification = new Notification(model.player.Id,model.activePlayerId,false,4);
//			StartCoroutine(tempNotification.add ());
//			model.isConnectedToMe=true;
//			model.connectionWithMe=connection;
//			this.initializeFriendshipState();
//		}
//		else
//		{
//			newMenuController.instance.displayErrorPopUp(connection.Error);
//			connection.Error="";
//		}
//		newMenuController.instance.hideLoadingScreen ();
//	}
//	private void drawFriendshipState()
//	{
//		if(model.isConnectedToMe)
//		{
//			if(model.connectionWithMe.IsAccepted)
//			{
//				this.friendshipState.transform.FindChild("Button0").gameObject.SetActive(true);
//				this.friendshipState.transform.FindChild("Button0").FindChild("Title").GetComponent<TextMeshPro>().text="Retirer";
//				this.friendshipState.transform.FindChild("Button1").gameObject.SetActive(false);
//				this.friendshipState.transform.FindChild("Description").GetComponent<TextMeshPro>().text="Vous êtes amis";
//			}
//			else if(model.connectionWithMe.IdUser1==model.player.Id)
//			{
//				this.friendshipState.transform.FindChild("Button0").gameObject.SetActive(true);
//				this.friendshipState.transform.FindChild("Button0").FindChild("Title").GetComponent<TextMeshPro>().text="Accepter";
//				this.friendshipState.transform.FindChild("Button1").gameObject.SetActive(true);
//				this.friendshipState.transform.FindChild("Button1").FindChild("Title").GetComponent<TextMeshPro>().text="Refuser";
//				this.friendshipState.transform.FindChild("Description").GetComponent<TextMeshPro>().text="Souhaite faire parti de vos amis";
//			}
//			else if(model.connectionWithMe.IdUser1==model.activePlayerId)
//			{
//				this.friendshipState.transform.FindChild("Button0").gameObject.SetActive(true);
//				this.friendshipState.transform.FindChild("Button0").FindChild("Title").GetComponent<TextMeshPro>().text="Annuler";
//				this.friendshipState.transform.FindChild("Button1").gameObject.SetActive(false);
//				this.friendshipState.transform.FindChild("Description").GetComponent<TextMeshPro>().text="n'a pas encore répondu à votre invitation";
//			}
//		}
//		else
//		{
//			this.friendshipState.transform.FindChild("Button0").gameObject.SetActive(true);
//			this.friendshipState.transform.FindChild("Button0").FindChild("Title").GetComponent<TextMeshPro>().text="Ajouer";
//			this.friendshipState.transform.FindChild("Button1").gameObject.SetActive(false);
//			this.friendshipState.transform.FindChild("Description").GetComponent<TextMeshPro>().text="ne fait pas partie de vos amis";
//		}
//	}
//	public void friendshipStateHandler(int buttonId)
//	{
//		if(model.isConnectedToMe)
//		{
//			if(model.connectionWithMe.IsAccepted)
//			{
//				StartCoroutine(this.removeConnection());
//			}
//			else if(model.connectionWithMe.IdUser1==model.player.Id)
//			{
//				if(buttonId==0)
//				{
//					StartCoroutine(this.confirmConnection());
//				}
//				else
//				{
//					StartCoroutine(this.removeConnection());
//				}
//			}
//			else if(model.connectionWithMe.IdUser1==model.activePlayerId)
//			{
//				StartCoroutine(this.removeConnection());
//			}
//		}
//		else
//		{
//			StartCoroutine(this.addConnection());
//		}
//	}
//}