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
	
	public GameObject tutorialObject;
	public GameObject blockObject;
	public GameObject paginationButtonObject;
	public GameObject popUpObject;
	public GUISkin popUpSkin;
	public Sprite[] profilePictures;
	public GameObject selectPicturePopUpObject;
	public GameObject searchUsersPopUpObject;
	public Sprite[] gameTypesPicto;
	
	private GameObject searchBlock;
	private GameObject friendsBlock;
	private GameObject friendsRequestsBlock;
	private GameObject confrontationsBlock;
	private GameObject trophiesBlock;
	private GameObject profileBlock;
	private GameObject challengesRecordsBlock;
	private GameObject statsBlock;

	private GameObject menu;
	private GameObject tutorial;

	private GameObject popUp;

	private GameObject searchTitle;
	private GameObject friendsTitle;
	private GameObject friendsRequestsTitle;
	private GameObject trophiesTitle;
	private GameObject profileTitle;
	private GameObject challengesRecordsTitle;
	private GameObject statsTitle;
	private GameObject confrontationsTitle;
	
	private GameObject stats;
	private GameObject profile;
	private GameObject search;
	private GameObject friendshipState;
	private GameObject collectionButton;
	private GameObject cleanCardsButton;

	private GameObject[] friends;
	private GameObject[] friendsRequests;
	private GameObject[] trophies;
	private GameObject[] challengesRecords;
	private GameObject[] confrontations;
	
	private GameObject[] paginationButtonsFriends;
	private GameObject[] paginationButtonsFriendsRequests;
	private GameObject[] paginationButtonsChallengesRecords;
	private GameObject[] paginationButtonsTrophies;
	private GameObject[] paginationButtonsConfrontations;

	private GameObject selectPicturePopUp;
	private GameObject searchUsersPopUp;

	private bool isMyProfile;
	private string profileChosen;

	private int widthScreen;
	private int heightScreen;
	private float worldWidth;
	private float worldHeight;
	private float pixelPerUnit;
	private Rect centralWindow;
	private Rect centralWindowEditInformations;
	
	private IList<int> friendsRequestsDisplayed;
	private IList<int> challengesRecordsDisplayed;
	private IList<int> trophiesDisplayed;
	private IList<int> friendsDisplayed;
	private IList<int> friendsToBeDisplayed;
	private IList<int> confrontationsDisplayed;
	
	private IList<int> friendsOnline;
	
	private int nbPagesFriendsRequests;
	private int nbPaginationButtonsLimitFriendsRequests;
	private int elementsPerPageFriendsRequests;
	private int chosenPageFriendsRequests;
	private int pageDebutFriendsRequests;
	private int activePaginationButtonIdFriendsRequests;

	private int nbPagesChallengesRecords;
	private int nbPaginationButtonsLimitChallengesRecords;
	private int elementsPerPageChallengesRecords;
	private int chosenPageChallengesRecords;
	private int pageDebutChallengesRecords;
	private int activePaginationButtonIdChallengesRecords;

	private int nbPagesTrophies;
	private int nbPaginationButtonsLimitTrophies;
	private int elementsPerPageTrophies;
	private int chosenPageTrophies;
	private int pageDebutTrophies;
	private int activePaginationButtonIdTrophies;

	private int nbPagesFriends;
	private int nbPaginationButtonsLimitFriends;
	private int elementsPerPageFriends;
	private int chosenPageFriends;
	private int pageDebutFriends;
	private int activePaginationButtonIdFriends;

	private int nbPagesConfrontations;
	private int nbPaginationButtonsLimitConfrontations;
	private int elementsPerPageConfrontations;
	private int chosenPageConfrontations;
	private int pageDebutConfrontations;
	private int activePaginationButtonIdConfrontations;
	
	private bool isSceneLoaded;
	
	private bool isHoveringFriendsRequest;
	private bool isHoveringFriend;
	private bool isHoveringChallengesRecord;
	private bool isHoveringPopUp;
	private bool isPopUpDisplayed;
	private int idFriendsRequestHovered;
	private int idFriendHovered;
	private int idChallengesRecordHovered;

	private bool toDestroyPopUp;
	private float popUpDestroyInterval;

	private bool areTrophiesPicturesLoading;
	
	private bool isTutorialLaunched;

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

	void Update()
	{	
		this.friendsCheckTimer += Time.deltaTime;
		
		if (this.isMyProfile && friendsCheckTimer > friendsRefreshInterval && this.isSceneLoaded) 
		{
			this.checkFriendsOnlineStatus();
		}
		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			this.resize();
			if(this.isMyProfile)
			{
				this.drawPaginationChallengesRecords();
				this.drawPaginationFriendsRequests();
			}
			else
			{

			}
			this.drawPaginationFriends();
			this.drawPaginationTrophies();
		}
		if(toDestroyPopUp)
		{
			this.popUpDestroyInterval=this.popUpDestroyInterval+Time.deltaTime;
			if(this.popUpDestroyInterval>0.5f)
			{
				this.toDestroyPopUp=false;
				this.hidePopUp();
			}
		}
		if(areTrophiesPicturesLoading)
		{
			bool allPicturesLoaded=true;
			for(int i=0;i<trophiesDisplayed.Count;i++)
			{
				if(!model.trophies[this.trophiesDisplayed[i]].competition.isTextureLoaded)
				{
					allPicturesLoaded=false;
					break;
				}
			}
			if(allPicturesLoaded)
			{
				this.areTrophiesPicturesLoading=false;
				for(int i=0;i<this.trophiesDisplayed.Count;i++)
				{
					this.trophies[i].GetComponent<TrophyController>().setPicture(model.trophies[this.trophiesDisplayed[i]].competition.texture);
				}
			}
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
						this.search.transform.FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text = this.searchValue;
						if(this.searchValue.Length==0)
						{
							this.isSearchingUsers=false;
							this.search.transform.FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text ="Rechercher";
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
							this.search.transform.FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text = this.searchValue;
						}
					}
				}
			}
			if((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))&& !this.isMouseOnSearchBar)
			{
				this.isSearchingUsers=false;
				if(this.searchValue=="")
				{
					this.search.transform.FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text ="Rechercher";
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
			this.elementsPerPageFriendsRequests = 2;
			this.elementsPerPageChallengesRecords = 3;
			this.elementsPerPageFriends = 7;
		}
		else
		{
			this.isMyProfile=false;
			this.profileChosen=ApplicationModel.profileChosen;
			ApplicationModel.profileChosen="";
			this.elementsPerPageFriends = 10;
			this.elementsPerPageConfrontations=3;
		}
		this.elementsPerPageTrophies = 3;
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.pixelPerUnit = 108f;
		this.searchValue = "";
		this.initializeScene ();
		this.resize ();
	}
	public IEnumerator initialization()
	{
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.getData (this.isMyProfile, this.profileChosen));
		if(this.isMyProfile)
		{
			this.initializeFriendsRequests ();
			this.initializeChallengesRecords ();
		}
		else
		{
			this.initializeConfrontations();
			this.initializeFriendshipState();
		}
		this.initializeFriends ();
		this.initializeTrophies ();
		this.initializeStats ();
		this.initializeProfile ();
		this.checkFriendsOnlineStatus ();
		newMenuController.instance.hideLoadingScreen ();
		this.isSceneLoaded = true;
	}
	private void initializeFriendsRequests()
	{
		this.chosenPageFriendsRequests = 0;
		this.pageDebutFriendsRequests = 0 ;
		this.drawPaginationFriendsRequests();
		this.drawFriendsRequests();
	}
	private void initializeChallengesRecords()
	{
		this.chosenPageChallengesRecords = 0;
		this.pageDebutChallengesRecords = 0 ;
		this.drawPaginationChallengesRecords();
		this.drawChallengesRecords ();
	}
	private void initializeFriends()
	{
		this.chosenPageFriends = 0;
		this.pageDebutFriends = 0 ;
		this.sortFriendsList ();
		this.drawPaginationFriends();
		this.drawFriends ();
	}
	private void initializeTrophies()
	{
		this.chosenPageTrophies = 0;
		this.pageDebutTrophies = 0 ;
		this.drawPaginationTrophies();
		this.drawTrophies ();
	}
	private void initializeConfrontations()
	{
		this.chosenPageConfrontations = 0;
		this.pageDebutConfrontations = 0 ;
		this.drawPaginationConfrontations();
		this.drawConfrontations ();
	}
	private void initializeStats()
	{
		this.drawStats ();
	}
	private void initializeProfile()
	{
		this.drawProfile ();
	}
	private void initializeFriendshipState()
	{
		this.drawFriendshipState ();
	}
	public void initializeScene()
	{
		menu = GameObject.Find ("newMenu");
		menu.AddComponent<newProfileMenuController> ();
		menu.GetComponent<newMenuController> ().setCurrentPage (0);
		this.profileBlock = Instantiate(this.blockObject) as GameObject;
		this.statsBlock = Instantiate(this.blockObject) as GameObject;
		this.trophiesBlock = Instantiate(this.blockObject) as GameObject;
		this.friendsBlock = Instantiate (this.blockObject) as GameObject;
		this.searchBlock = Instantiate(this.blockObject) as GameObject;
		this.search = GameObject.Find ("Search");
		this.search.transform.FindChild ("SearchBar").transform.FindChild ("Text").GetComponent<TextMeshPro> ().text = "Rechercher";
		this.search.transform.FindChild ("SearchButton").transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "OK";
		this.profile = GameObject.Find ("Profile");
		this.profileTitle = GameObject.Find ("ProfileTitle");
		this.profileTitle.SetActive (false);
		//this.profileTitle.GetComponent<TextMeshPro> ().text = "Mon profil";
		this.statsTitle = GameObject.Find ("StatsTitle");
		this.statsTitle.GetComponent<TextMeshPro> ().text = "Statistiques";
		this.friendsTitle = GameObject.Find ("FriendsTitle");
		this.friendsTitle.GetComponent<TextMeshPro> ().text = "Amis";
		this.friendsRequestsTitle = GameObject.Find ("FriendsRequestsTitle");
		this.challengesRecordsTitle = GameObject.Find ("ChallengesRecordsTitle");
		this.confrontationsTitle = GameObject.Find ("ConfrontationsTitle");
		this.searchTitle = GameObject.Find ("SearchTitle");
		this.searchTitle.GetComponent<TextMeshPro> ().text = "Trouver un ami";
		this.trophiesTitle = GameObject.Find ("TrophiesTitle");
		this.trophiesTitle.GetComponent<TextMeshPro> ().text = "Trophées";
		this.stats = GameObject.Find ("Stats");
		this.paginationButtonsFriends = new GameObject[0];
		this.paginationButtonsTrophies = new GameObject[0];
		this.friends=new GameObject[10];
		for(int i=0;i<this.friends.Length;i++)
		{
			this.friends[i]=GameObject.Find ("Friend"+i);
			this.friends[i].GetComponent<OnlineFriendController>().setId(i);
			this.friends[i].SetActive(false);
		}
		this.trophies=new GameObject[3];
		for(int i=0;i<this.trophies.Length;i++)
		{
			this.trophies[i]=GameObject.Find ("Trophy"+i);
			this.trophies[i].GetComponent<TrophyController>().setId(i);
			this.trophies[i].SetActive(false);
		}
		this.collectionButton = GameObject.Find ("CollectionButton");
		this.collectionButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "Cristalopedia";

		this.challengesRecords=new GameObject[3];
		for(int i=0;i<this.challengesRecords.Length;i++)
		{
			this.challengesRecords[i]=GameObject.Find ("ChallengesRecord"+i);
			this.challengesRecords[i].GetComponent<ChallengesRecordController>().setId(i);
			this.challengesRecords[i].SetActive(false);
		}
		this.friendsRequests=new GameObject[2];
		for(int i=0;i<this.friendsRequests.Length;i++)
		{
			this.friendsRequests[i]=GameObject.Find ("FriendsRequest"+i);
			this.friendsRequests[i].GetComponent<FriendsRequestController>().setId(i);
			this.friendsRequests[i].SetActive(false);
		}
		this.confrontations=new GameObject[3];
		for(int i=0;i<this.confrontations.Length;i++)
		{
			this.confrontations[i]=GameObject.Find ("Confrontation"+i);
			this.confrontations[i].GetComponent<ConfrontationController>().setId(i);
			this.confrontations[i].SetActive(false);
		}
		this.cleanCardsButton = GameObject.Find ("CleanCardsButton");
		this.friendshipState = GameObject.Find ("FriendshipState");

		if(this.isMyProfile)
		{
			this.friendsOnline = new List<int> ();
			this.friendsRequestsBlock = Instantiate(this.blockObject) as GameObject;
			this.challengesRecordsBlock = Instantiate(this.blockObject) as GameObject;
			this.friendsRequestsTitle.GetComponent<TextMeshPro> ().text = "Mes invitations";
			this.challengesRecordsTitle.GetComponent<TextMeshPro> ().text = "Amis défiés";
			this.paginationButtonsFriendsRequests = new GameObject[0];
			this.paginationButtonsChallengesRecords = new GameObject[0];
			this.profile.transform.FindChild("EditInformationsButton").gameObject.SetActive(true);
			this.profile.transform.FindChild("EditPasswordButton").gameObject.SetActive(true);
			this.profile.transform.FindChild("EditInformationsButton").FindChild("Title").GetComponent<TextMeshPro>().text="Modifier mes informations";
			this.profile.transform.FindChild("EditPasswordButton").FindChild("Title").GetComponent<TextMeshPro>().text="Modifier mon mot de passe";
			this.confrontationsTitle.SetActive (false);
			if(ApplicationModel.isAdmin)
			{
				this.cleanCardsButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "Vider";
			}
			else
			{
				this.cleanCardsButton.SetActive(false);
			}
			this.friendshipState.SetActive(false);
		}
		else
		{
			this.confrontationsBlock=Instantiate(this.blockObject) as GameObject;
			this.paginationButtonsConfrontations = new GameObject[0];
			this.confrontationsTitle.GetComponent<TextMeshPro> ().text = "Vos rencontres";
			this.profile.transform.FindChild("EditInformationsButton").gameObject.SetActive(false);
			this.profile.transform.FindChild("EditPasswordButton").gameObject.SetActive(false);	
			this.friendsRequestsTitle.SetActive (false);
			this.challengesRecordsTitle.SetActive(false);
			this.cleanCardsButton.SetActive(false);
		}
	}
	public void resize()
	{
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.centralWindowEditInformations = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.35f * this.heightScreen);
		this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		float screenRatio = (float)this.widthScreen / (float)this.heightScreen;
		menu.GetComponent<newMenuController> ().resizeMeunObject (worldHeight,worldWidth);

		float trophiesBlockLeftMargin = (this.worldWidth - 3f - 3f) / 2f + 3.1f;
		float trophiesBlockRightMargin = 3f;
		float trophiesBlockUpMargin = 6.1f;
		float trophiesBlockDownMargin = 0.2f;
		
		float trophiesBlockHeight = worldHeight - trophiesBlockUpMargin-trophiesBlockDownMargin;
		float trophiesBlockWidth = worldWidth-trophiesBlockLeftMargin-trophiesBlockRightMargin;
		Vector2 trophiesBlockOrigin = new Vector3 (-worldWidth/2f+trophiesBlockLeftMargin+trophiesBlockWidth/2f, -worldHeight / 2f + trophiesBlockDownMargin + trophiesBlockHeight / 2,0f);
		
		this.trophiesBlock.GetComponent<BlockController> ().resize(new Rect(trophiesBlockOrigin.x,trophiesBlockOrigin.y,trophiesBlockWidth,trophiesBlockHeight));
		this.trophiesTitle.transform.position = new Vector3 (trophiesBlockOrigin.x, trophiesBlockOrigin.y+trophiesBlockHeight/2f-0.3f, 0);
		
		float statsBlockLeftMargin = 3f;
		float statsBlockRightMargin = 3f;
		float statsBlockUpMargin = 3.6f;
		float statsBlockDownMargin = 4.1f;
		
		float statsBlockHeight = worldHeight - statsBlockUpMargin-statsBlockDownMargin;
		float statsBlockWidth = worldWidth-statsBlockLeftMargin-statsBlockRightMargin;
		Vector2 statsBlockOrigin = new Vector3 (-worldWidth/2f+statsBlockLeftMargin+statsBlockWidth/2f, -worldHeight / 2f + statsBlockDownMargin + statsBlockHeight / 2,0f);
		
		this.statsBlock.GetComponent<BlockController> ().resize(new Rect(statsBlockOrigin.x,statsBlockOrigin.y,statsBlockWidth,statsBlockHeight));
		this.statsTitle.transform.position = new Vector3 (statsBlockOrigin.x, statsBlockOrigin.y+statsBlockHeight/2f-0.3f, 0);
		
		this.stats.transform.position = new Vector3 (statsBlockOrigin.x, statsBlockOrigin.y, 0);
		this.stats.transform.FindChild ("nbWins").localPosition = new Vector3 (-1.5f * statsBlockWidth / 5f, 0f, 0);
		this.stats.transform.FindChild ("nbLooses").localPosition = new Vector3 (-0.5f * statsBlockWidth / 5f, 0f, 0);
		this.stats.transform.FindChild ("ranking").localPosition = new Vector3 (0.5f * statsBlockWidth / 5f, 0f, 0);
		this.stats.transform.FindChild ("collectionPoints").localPosition = new Vector3 (1.5f * statsBlockWidth / 5f, 0f, 0);
		this.collectionButton.transform.position = new Vector3 (1.5f * statsBlockWidth / 5f, statsBlockOrigin.y+statsBlockHeight/2f-0.3f, 0f);

		
		float profileBlockLeftMargin = 3f;
		float profileBlockRightMargin = 3f;
		float profileBlockUpMargin = 0.2f;
		float profileBlockDownMargin = 6.6f;
		
		float profileBlockHeight = worldHeight - profileBlockUpMargin-profileBlockDownMargin;
		float profileBlockWidth = worldWidth-profileBlockLeftMargin-profileBlockRightMargin;
		Vector2 profileBlockOrigin = new Vector3 (-worldWidth/2f+profileBlockLeftMargin+profileBlockWidth/2f, -worldHeight / 2f + profileBlockDownMargin + profileBlockHeight / 2,0f);
		
		this.profileBlock.GetComponent<BlockController> ().resize(new Rect(profileBlockOrigin.x,profileBlockOrigin.y,profileBlockWidth,profileBlockHeight));
		this.profileTitle.transform.position = new Vector3 (profileBlockOrigin.x, profileBlockOrigin.y+profileBlockHeight/2f-0.3f, 0);
		this.profile.transform.position = new Vector3 (1f, 3.12f, 0f);
		
		float friendsBlockLeftMargin = this.worldWidth-2.8f;
		float friendsBlockRightMargin = 0f;
		float friendsBlockUpMargin = 2f;
		float friendsBlockDownMargin;

		if(this.isMyProfile)
		{
			friendsBlockDownMargin = 2.6f;
		}
		else
		{
			friendsBlockDownMargin = 0.2f;
		}
		
		float friendsBlockHeight = worldHeight - friendsBlockUpMargin-friendsBlockDownMargin;
		float friendsBlockWidth = worldWidth-friendsBlockLeftMargin-friendsBlockRightMargin;
		Vector2 friendsBlockOrigin = new Vector3 (-worldWidth/2f+friendsBlockLeftMargin+friendsBlockWidth/2f, -worldHeight / 2f + friendsBlockDownMargin + friendsBlockHeight / 2,0f);
		
		this.friendsBlock.GetComponent<BlockController> ().resize(new Rect(friendsBlockOrigin.x,friendsBlockOrigin.y, friendsBlockWidth, friendsBlockHeight));
		this.friendsTitle.transform.position = new Vector3 (friendsBlockOrigin.x, friendsBlockOrigin.y+friendsBlockHeight/2f-0.3f, 0);
		
		float searchBlockLeftMargin = this.worldWidth-2.8f;
		float searchBlockRightMargin = 0f;
		float searchBlockUpMargin = 0.6f;
		float searchBlockDownMargin = 8.2f;
		
		float searchBlockHeight = worldHeight - searchBlockUpMargin-searchBlockDownMargin;
		float searchBlockWidth = worldWidth-searchBlockLeftMargin-searchBlockRightMargin;
		Vector2 searchBlockOrigin = new Vector3 (-worldWidth/2f+searchBlockLeftMargin+searchBlockWidth/2f, -worldHeight / 2f + searchBlockDownMargin + searchBlockHeight / 2,0f);
		
		this.searchBlock.GetComponent<BlockController> ().resize(new Rect(searchBlockOrigin.x,searchBlockOrigin.y,searchBlockWidth,searchBlockHeight));
		this.searchTitle.transform.position = new Vector3 (searchBlockOrigin.x, searchBlockOrigin.y+searchBlockHeight/2f-0.3f, 0);
		this.search.transform.position = new Vector3 (searchBlockOrigin.x, searchBlockOrigin.y - 0.25f, 0);

		for(int i=0;i<this.trophies.Length;i++)
		{
			this.trophies[i].transform.position=new Vector3(trophiesBlockOrigin.x-0.45f,trophiesBlockOrigin.y+trophiesBlockHeight/2f-1f-i*0.85f,0);
		}
		
		for(int i=0;i<this.friends.Length;i++)
		{
			this.friends[i].transform.position=new Vector3(friendsBlockOrigin.x-0.2f,friendsBlockOrigin.y+friendsBlockHeight/2f-0.75f-i*0.65f,0); 
		}

		if(this.isMyProfile)
		{
			float challengesRecordsBlockLeftMargin =3f;
			float challengesRecordsBlockRightMargin = (this.worldWidth - 3f - 3f) / 2f + 3.1f;
			float challengesRecordsBlockUpMargin = 6.1f;
			float challengesRecordsBlockDownMargin = 0.2f;
			
			float challengesRecordsBlockHeight = worldHeight - challengesRecordsBlockUpMargin-challengesRecordsBlockDownMargin;
			float challengesRecordsBlockWidth = worldWidth-challengesRecordsBlockLeftMargin-challengesRecordsBlockRightMargin;
			Vector2 challengesRecordsBlockOrigin = new Vector3 (-worldWidth/2f+challengesRecordsBlockLeftMargin+challengesRecordsBlockWidth/2f, -worldHeight / 2f + challengesRecordsBlockDownMargin + challengesRecordsBlockHeight / 2,0f);
			
			this.challengesRecordsBlock.GetComponent<BlockController> ().resize(new Rect(challengesRecordsBlockOrigin.x,challengesRecordsBlockOrigin.y,challengesRecordsBlockWidth,challengesRecordsBlockHeight));
			this.challengesRecordsTitle.transform.position = new Vector3 (challengesRecordsBlockOrigin.x, challengesRecordsBlockOrigin.y+challengesRecordsBlockHeight/2f-0.3f, 0);
			
			float friendsRequestsBlockLeftMargin = this.worldWidth-2.8f;
			float friendsRequestsBlockRightMargin = 0f;
			float friendsRequestsBlockUpMargin = 7.6f;
			float friendsRequestsBlockDownMargin = 0.2f;
			
			float friendsRequestsBlockHeight = worldHeight - friendsRequestsBlockUpMargin-friendsRequestsBlockDownMargin;
			float friendsRequestsBlockWidth = worldWidth-friendsRequestsBlockLeftMargin-friendsRequestsBlockRightMargin;
			Vector2 friendsRequestsBlockOrigin = new Vector3 (-worldWidth/2f+friendsRequestsBlockLeftMargin+friendsRequestsBlockWidth/2f, -worldHeight / 2f + friendsRequestsBlockDownMargin + friendsRequestsBlockHeight / 2,0f);
			
			this.friendsRequestsBlock.GetComponent<BlockController> ().resize(new Rect(friendsRequestsBlockOrigin.x,friendsRequestsBlockOrigin.y,friendsRequestsBlockWidth,friendsRequestsBlockHeight));
			this.friendsRequestsTitle.transform.position = new Vector3 (friendsRequestsBlockOrigin.x, friendsRequestsBlockOrigin.y+friendsRequestsBlockHeight/2f-0.3f, 0);
			
			for(int i=0;i<this.challengesRecords.Length;i++)
			{
				this.challengesRecords[i].transform.position=new Vector3(challengesRecordsBlockOrigin.x,challengesRecordsBlockOrigin.y+challengesRecordsBlockHeight/2f-1f-i*0.85f,0);
			}
			
			for(int i=0;i<this.friendsRequests.Length;i++)
			{
				this.friendsRequests[i].transform.position=new Vector3(friendsRequestsBlockOrigin.x-0.2f,friendsRequestsBlockOrigin.y+friendsRequestsBlockHeight/2f-0.85f-i*0.65f,0);
			}
			this.cleanCardsButton.transform.position = new Vector3 (1.5f * statsBlockWidth / 5f, statsBlockOrigin.y+statsBlockHeight/2f-0.58f, 0f);
		}
		else
		{
			float confrontationsBlockLeftMargin =3f;
			float confrontationsBlockRightMargin = (this.worldWidth - 3f - 3f) / 2f + 3.1f;
			float confrontationsBlockUpMargin = 6.1f;
			float confrontationsBlockDownMargin = 0.2f;
			
			float confrontationsBlockHeight = worldHeight - confrontationsBlockUpMargin-confrontationsBlockDownMargin;
			float confrontationsBlockWidth = worldWidth-confrontationsBlockLeftMargin-confrontationsBlockRightMargin;
			Vector2 confrontationsBlockOrigin = new Vector3 (-worldWidth/2f+confrontationsBlockLeftMargin+confrontationsBlockWidth/2f, -worldHeight / 2f + confrontationsBlockDownMargin + confrontationsBlockHeight / 2,0f);
			
			this.confrontationsBlock.GetComponent<BlockController> ().resize(new Rect(confrontationsBlockOrigin.x,confrontationsBlockOrigin.y,confrontationsBlockWidth,confrontationsBlockHeight));
			this.confrontationsTitle.transform.position = new Vector3 (confrontationsBlockOrigin.x, confrontationsBlockOrigin.y+confrontationsBlockHeight/2f-0.3f, 0);

			for(int i=0;i<this.confrontations.Length;i++)
			{
				this.confrontations[i].transform.position=new Vector3(confrontationsBlockOrigin.x-0.45f,confrontationsBlockOrigin.y+confrontationsBlockHeight/2f-1f-i*0.85f,0);
			}

		}
		if(this.isTutorialLaunched)
		{
			this.tutorial.GetComponent<TutorialObjectController>().resize();
		}
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
		for(int i =0;i<elementsPerPageChallengesRecords;i++)
		{
			if(this.chosenPageChallengesRecords*this.elementsPerPageChallengesRecords+i<model.challengesRecords.Count)
			{
				this.challengesRecordsDisplayed.Add (this.chosenPageChallengesRecords*this.elementsPerPageChallengesRecords+i);
				this.challengesRecords[i].GetComponent<ChallengesRecordController>().c=model.challengesRecords[this.chosenPageChallengesRecords*this.elementsPerPageChallengesRecords+i];
				this.challengesRecords[i].GetComponent<ChallengesRecordController>().show();
				this.challengesRecords[i].SetActive(true);
			}
			else
			{
				this.challengesRecords[i].SetActive(false);
			}
		}
	}
	public void drawFriendsRequests()
	{
		this.friendsRequestsDisplayed = new List<int> ();
		for(int i =0;i<elementsPerPageFriendsRequests;i++)
		{
			if(this.chosenPageFriendsRequests*this.elementsPerPageFriendsRequests+i<model.friendsRequests.Count)
			{
				this.friendsRequestsDisplayed.Add (this.chosenPageFriendsRequests*this.elementsPerPageFriendsRequests+i);
				this.friendsRequests[i].GetComponent<FriendsRequestController>().f=model.friendsRequests[this.chosenPageFriendsRequests*this.elementsPerPageFriendsRequests+i];
				this.friendsRequests[i].GetComponent<FriendsRequestController>().show();
				this.friendsRequests[i].SetActive(true);
			}
			else
			{
				this.friendsRequests[i].SetActive(false);
			}
		}
	}
	public void drawFriends()
	{
		this.friendsDisplayed = new List<int> ();
		for(int i =0;i<elementsPerPageFriends;i++)
		{
			if(this.chosenPageFriends*this.elementsPerPageFriends+i<this.friendsToBeDisplayed.Count)
			{
				this.friendsDisplayed.Add (this.chosenPageFriends*this.elementsPerPageFriends+i);
				this.friends[i].GetComponent<ProfileOnlineFriendController>().u=model.users[this.friendsToBeDisplayed[this.chosenPageFriends*this.elementsPerPageFriends+i]];
				this.friends[i].GetComponent<ProfileOnlineFriendController>().show();
				this.friends[i].SetActive(true);
			}
			else
			{
				this.friends[i].SetActive(false);
			}
		}
	}
	public void drawTrophies()
	{
		this.trophiesDisplayed = new List<int> ();
		bool allPicturesLoaded = true;
		for(int i =0;i<elementsPerPageTrophies;i++)
		{
			if(this.chosenPageTrophies*this.elementsPerPageTrophies+i<model.trophies.Count)
			{
				if(!model.trophies[this.chosenPageTrophies*this.elementsPerPageTrophies+i].competition.isTextureLoaded)
				{
					StartCoroutine(model.trophies[this.chosenPageTrophies*this.elementsPerPageTrophies+i].competition.setPicture());
					allPicturesLoaded=false;
				}
				this.trophiesDisplayed.Add (this.chosenPageTrophies*this.elementsPerPageTrophies+i);
				this.trophies[i].GetComponent<TrophyController>().t=model.trophies[this.chosenPageTrophies*this.elementsPerPageTrophies+i];
				this.trophies[i].GetComponent<TrophyController>().show();
				this.trophies[i].SetActive(true);
			}
			else
			{
				this.trophies[i].SetActive(false);
			}
		}
		if(!allPicturesLoaded)
		{
			this.areTrophiesPicturesLoading=true;
		}
	}
	public void drawConfrontations()
	{
		this.confrontationsDisplayed = new List<int> ();
		for(int i =0;i<elementsPerPageConfrontations;i++)
		{
			if(this.chosenPageConfrontations*this.elementsPerPageConfrontations+i<model.confrontations.Count)
			{
				this.confrontationsDisplayed.Add (this.chosenPageConfrontations*this.elementsPerPageConfrontations+i);
				this.confrontations[i].GetComponent<ConfrontationController>().r=model.confrontations[this.chosenPageConfrontations*this.elementsPerPageConfrontations+i];
				bool hasWon;
				if(model.confrontations[this.chosenPageConfrontations*this.elementsPerPageConfrontations+i].IdWinner==model.player.Id)
				{
					hasWon=true;
				}
				else
				{
					hasWon=false;
				}
				this.confrontations[i].GetComponent<ConfrontationController>().show(hasWon);
				this.confrontations[i].SetActive(true);
			}
			else
			{
				this.confrontations[i].SetActive(false);
			}
		}
	}
	public void drawStats()
	{
		this.stats.transform.FindChild ("nbWins").FindChild ("Value").GetComponent<TextMeshPro> ().text = model.player.TotalNbWins.ToString ();
		this.stats.transform.FindChild ("nbWins").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Victoires";
		this.stats.transform.FindChild ("nbLooses").FindChild ("Value").GetComponent<TextMeshPro> ().text = model.player.TotalNbLooses.ToString ();
		this.stats.transform.FindChild ("nbLooses").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Défaites";
		this.stats.transform.FindChild ("ranking").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Classement combattant";
		this.stats.transform.FindChild ("ranking").FindChild ("Value").GetComponent<TextMeshPro> ().text = model.player.Ranking.ToString ();
		this.stats.transform.FindChild ("ranking").FindChild ("Title2").GetComponent<TextMeshPro> ().text = "("+model.player.RankingPoints.ToString()+" pts)";
		this.stats.transform.FindChild ("collectionPoints").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Classement collectionneur";
		this.stats.transform.FindChild ("collectionPoints").FindChild ("Value").GetComponent<TextMeshPro> ().text = model.player.CollectionRanking.ToString ();
		this.stats.transform.FindChild ("collectionPoints").FindChild ("Title2").GetComponent<TextMeshPro> ().text = "("+model.player.CollectionPoints.ToString()+" pts)";
	}
	private void drawProfile()
	{
		this.profile.transform.FindChild ("Username").GetComponent<TextMeshPro> ().text = model.player.Username;
		if(this.isMyProfile)
		{
			this.drawPersonalInformations ();
		}
		this.drawProfilePicture ();
	}
	private void drawPersonalInformations()
	{
		this.profile.transform.FindChild ("Informations").GetComponent<TextMeshPro> ().text = "Prénom : " + model.player.FirstName + "\nNom : " + model.player.Surname + "\neMail : " + model.player.Mail;
	}
	private void drawProfilePicture()
	{
		this.profile.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.profilePictures[model.player.idProfilePicture];
	}
	private void drawPaginationChallengesRecords()
	{
		for(int i=0;i<this.paginationButtonsChallengesRecords.Length;i++)
		{
			Destroy (this.paginationButtonsChallengesRecords[i]);
		}
		this.paginationButtonsChallengesRecords = new GameObject[0];
		this.activePaginationButtonIdChallengesRecords = -1;
		float paginationButtonWidth = 0.34f;
		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
		this.nbPagesChallengesRecords = Mathf.CeilToInt((float)model.challengesRecords.Count / ((float)this.elementsPerPageChallengesRecords));
		if(this.nbPagesChallengesRecords>1)
		{
			this.nbPaginationButtonsLimitChallengesRecords = Mathf.CeilToInt((2.4f)/(paginationButtonWidth+gapBetweenPaginationButton));
			int nbButtonsToDraw=0;
			bool drawBackButton=false;
			if (this.pageDebutChallengesRecords !=0)
			{
				drawBackButton=true;
			}
			bool drawNextButton=false;
			if (this.pageDebutChallengesRecords+nbPaginationButtonsLimitChallengesRecords-System.Convert.ToInt32(drawBackButton)<this.nbPagesChallengesRecords-1)
			{
				drawNextButton=true;
				nbButtonsToDraw=nbPaginationButtonsLimitChallengesRecords;
			}
			else
			{
				nbButtonsToDraw=this.nbPagesChallengesRecords-this.pageDebutChallengesRecords;
				if(drawBackButton)
				{
					nbButtonsToDraw++;
				}
			}
			this.paginationButtonsChallengesRecords = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsChallengesRecords[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsChallengesRecords[i].AddComponent<ProfileChallengesRecordsPaginationController>();
				this.paginationButtonsChallengesRecords[i].transform.position=new Vector3(this.challengesRecordsBlock.transform.position.x+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
				this.paginationButtonsChallengesRecords[i].name="PaginationChallengesRecord"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtonsChallengesRecords[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutChallengesRecords+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtonsChallengesRecords[i].GetComponent<ProfileChallengesRecordsPaginationController>().setId(i);
				if(this.pageDebutChallengesRecords+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageChallengesRecords)
				{
					this.paginationButtonsChallengesRecords[i].GetComponent<ProfileChallengesRecordsPaginationController>().setActive(true);
					this.activePaginationButtonIdChallengesRecords=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtonsChallengesRecords[0].GetComponent<ProfileChallengesRecordsPaginationController>().setId(-2);
				this.paginationButtonsChallengesRecords[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtonsChallengesRecords[nbButtonsToDraw-1].GetComponent<ProfileChallengesRecordsPaginationController>().setId(-1);
				this.paginationButtonsChallengesRecords[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
		}
	}
	public void paginationHandlerChallengesRecords(int id)
	{
		if(id==-2)
		{
			this.pageDebutChallengesRecords=this.pageDebutChallengesRecords-this.nbPaginationButtonsLimitChallengesRecords+1+System.Convert.ToInt32(this.pageDebutChallengesRecords-this.nbPaginationButtonsLimitChallengesRecords+1!=0);
			this.drawPaginationChallengesRecords();
		}
		else if(id==-1)
		{
			this.pageDebutChallengesRecords=this.pageDebutChallengesRecords+this.nbPaginationButtonsLimitChallengesRecords-1-System.Convert.ToInt32(this.pageDebutChallengesRecords!=0);
			this.drawPaginationChallengesRecords();
		}
		else
		{
			if(activePaginationButtonIdChallengesRecords!=-1)
			{
				this.paginationButtonsChallengesRecords[this.activePaginationButtonIdChallengesRecords].GetComponent<ProfileChallengesRecordsPaginationController>().setActive(false);
			}
			this.activePaginationButtonIdChallengesRecords=id;
			this.chosenPageChallengesRecords=this.pageDebutChallengesRecords-System.Convert.ToInt32(this.pageDebutChallengesRecords!=0)+id;
			this.drawChallengesRecords();
		}
	}
	private void drawPaginationConfrontations()
	{
		for(int i=0;i<this.paginationButtonsConfrontations.Length;i++)
		{
			Destroy (this.paginationButtonsConfrontations[i]);
		}
		this.paginationButtonsConfrontations = new GameObject[0];
		this.activePaginationButtonIdConfrontations = -1;
		float paginationButtonWidth = 0.34f;
		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
		this.nbPagesConfrontations = Mathf.CeilToInt((float)model.confrontations.Count / ((float)this.elementsPerPageConfrontations));
		if(this.nbPagesConfrontations>1)
		{
			this.nbPaginationButtonsLimitConfrontations = Mathf.CeilToInt((2.4f)/(paginationButtonWidth+gapBetweenPaginationButton));
			int nbButtonsToDraw=0;
			bool drawBackButton=false;
			if (this.pageDebutConfrontations !=0)
			{
				drawBackButton=true;
			}
			bool drawNextButton=false;
			if (this.pageDebutConfrontations+nbPaginationButtonsLimitConfrontations-System.Convert.ToInt32(drawBackButton)<this.nbPagesConfrontations-1)
			{
				drawNextButton=true;
				nbButtonsToDraw=nbPaginationButtonsLimitConfrontations;
			}
			else
			{
				nbButtonsToDraw=this.nbPagesConfrontations-this.pageDebutConfrontations;
				if(drawBackButton)
				{
					nbButtonsToDraw++;
				}
			}
			this.paginationButtonsConfrontations = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsConfrontations[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsConfrontations[i].AddComponent<ProfileConfrontationsPaginationController>();
				this.paginationButtonsConfrontations[i].transform.position=new Vector3(this.confrontationsBlock.transform.position.x+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
				this.paginationButtonsConfrontations[i].name="PaginationChallengesRecord"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtonsConfrontations[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutConfrontations+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtonsConfrontations[i].GetComponent<ProfileConfrontationsPaginationController>().setId(i);
				if(this.pageDebutConfrontations+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageConfrontations)
				{
					this.paginationButtonsConfrontations[i].GetComponent<ProfileConfrontationsPaginationController>().setActive(true);
					this.activePaginationButtonIdConfrontations=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtonsConfrontations[0].GetComponent<ProfileConfrontationsPaginationController>().setId(-2);
				this.paginationButtonsConfrontations[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtonsConfrontations[nbButtonsToDraw-1].GetComponent<ProfileConfrontationsPaginationController>().setId(-1);
				this.paginationButtonsConfrontations[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
		}
	}
	public void paginationHandlerConfrontations(int id)
	{
		if(id==-2)
		{
			this.pageDebutConfrontations=this.pageDebutConfrontations-this.nbPaginationButtonsLimitConfrontations+1+System.Convert.ToInt32(this.pageDebutConfrontations-this.nbPaginationButtonsLimitConfrontations+1!=0);
			this.drawPaginationConfrontations();
		}
		else if(id==-1)
		{
			this.pageDebutConfrontations=this.pageDebutConfrontations+this.nbPaginationButtonsLimitConfrontations-1-System.Convert.ToInt32(this.pageDebutConfrontations!=0);
			this.drawPaginationConfrontations();
		}
		else
		{
			if(activePaginationButtonIdConfrontations!=-1)
			{
				this.paginationButtonsConfrontations[this.activePaginationButtonIdConfrontations].GetComponent<ProfileConfrontationsPaginationController>().setActive(false);
			}
			this.activePaginationButtonIdConfrontations=id;
			this.chosenPageConfrontations=this.pageDebutConfrontations-System.Convert.ToInt32(this.pageDebutConfrontations!=0)+id;
			this.drawConfrontations();
		}
	}
	private void drawPaginationFriendsRequests()
	{
		for(int i=0;i<this.paginationButtonsFriendsRequests.Length;i++)
		{
			Destroy (this.paginationButtonsFriendsRequests[i]);
		}
		this.paginationButtonsFriendsRequests = new GameObject[0];
		this.activePaginationButtonIdFriendsRequests = -1;
		float paginationButtonWidth = 0.34f;
		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
		this.nbPagesFriendsRequests = Mathf.CeilToInt((float)model.friendsRequests.Count / ((float)this.elementsPerPageFriendsRequests));
		if(this.nbPagesFriendsRequests>1)
		{
			this.nbPaginationButtonsLimitFriendsRequests = Mathf.CeilToInt((2.4f)/(paginationButtonWidth+gapBetweenPaginationButton));
			int nbButtonsToDraw=0;
			bool drawBackButton=false;
			if (this.pageDebutFriendsRequests !=0)
			{
				drawBackButton=true;
			}
			bool drawNextButton=false;
			if (this.pageDebutFriendsRequests+nbPaginationButtonsLimitFriendsRequests-System.Convert.ToInt32(drawBackButton)<this.nbPagesFriendsRequests-1)
			{
				drawNextButton=true;
				nbButtonsToDraw=nbPaginationButtonsLimitFriendsRequests;
			}
			else
			{
				nbButtonsToDraw=this.nbPagesFriendsRequests-this.pageDebutFriendsRequests;
				if(drawBackButton)
				{
					nbButtonsToDraw++;
				}
			}
			this.paginationButtonsFriendsRequests = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsFriendsRequests[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsFriendsRequests[i].AddComponent<ProfileFriendsRequestsPaginationController>();
				this.paginationButtonsFriendsRequests[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
				this.paginationButtonsFriendsRequests[i].name="PaginationFriends"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtonsFriendsRequests[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutFriendsRequests+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtonsFriendsRequests[i].GetComponent<ProfileFriendsRequestsPaginationController>().setId(i);
				if(this.pageDebutFriendsRequests+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageFriendsRequests)
				{
					this.paginationButtonsFriendsRequests[i].GetComponent<ProfileFriendsRequestsPaginationController>().setActive(true);
					this.activePaginationButtonIdFriendsRequests=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtonsFriendsRequests[0].GetComponent<ProfileFriendsRequestsPaginationController>().setId(-2);
				this.paginationButtonsFriendsRequests[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtonsFriendsRequests[nbButtonsToDraw-1].GetComponent<ProfileFriendsRequestsPaginationController>().setId(-1);
				this.paginationButtonsFriendsRequests[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
		}
	}
	public void paginationHandlerFriendsRequests(int id)
	{
		if(id==-2)
		{
			this.pageDebutFriendsRequests=this.pageDebutFriendsRequests-this.nbPaginationButtonsLimitFriendsRequests+1+System.Convert.ToInt32(this.pageDebutFriendsRequests-this.nbPaginationButtonsLimitFriendsRequests+1!=0);
			this.drawPaginationFriendsRequests();
		}
		else if(id==-1)
		{
			this.pageDebutFriendsRequests=this.pageDebutFriendsRequests+this.nbPaginationButtonsLimitFriendsRequests-1-System.Convert.ToInt32(this.pageDebutFriendsRequests!=0);
			this.drawPaginationFriendsRequests();
		}
		else
		{
			if(activePaginationButtonIdFriendsRequests!=-1)
			{
				this.paginationButtonsFriendsRequests[this.activePaginationButtonIdFriendsRequests].GetComponent<ProfileFriendsRequestsPaginationController>().setActive(false);
			}
			this.activePaginationButtonIdFriendsRequests=id;
			this.chosenPageFriendsRequests=this.pageDebutFriendsRequests-System.Convert.ToInt32(this.pageDebutFriendsRequests!=0)+id;
			this.drawFriendsRequests();
		}
	}
	private void drawPaginationTrophies()
	{
		for(int i=0;i<this.paginationButtonsTrophies.Length;i++)
		{
			Destroy (this.paginationButtonsTrophies[i]);
		}
		this.paginationButtonsTrophies = new GameObject[0];
		this.activePaginationButtonIdTrophies = -1;
		float paginationButtonWidth = 0.34f;
		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
		this.nbPagesTrophies = Mathf.CeilToInt((float)model.trophies.Count / ((float)this.elementsPerPageTrophies));
		if(this.nbPagesTrophies>1)
		{
			this.nbPaginationButtonsLimitTrophies = Mathf.CeilToInt((2.4f)/(paginationButtonWidth+gapBetweenPaginationButton));
			int nbButtonsToDraw=0;
			bool drawBackButton=false;
			if (this.pageDebutTrophies !=0)
			{
				drawBackButton=true;
			}
			bool drawNextButton=false;
			if (this.pageDebutTrophies+nbPaginationButtonsLimitTrophies-System.Convert.ToInt32(drawBackButton)<this.nbPagesTrophies-1)
			{
				drawNextButton=true;
				nbButtonsToDraw=nbPaginationButtonsLimitTrophies;
			}
			else
			{
				nbButtonsToDraw=this.nbPagesTrophies-this.pageDebutTrophies;
				if(drawBackButton)
				{
					nbButtonsToDraw++;
				}
			}
			this.paginationButtonsTrophies = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsTrophies[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsTrophies[i].AddComponent<ProfileTrophiesPaginationController>();
				this.paginationButtonsTrophies[i].transform.position=new Vector3(this.trophiesBlock.transform.position.x+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
				this.paginationButtonsTrophies[i].name="PaginationTrophies"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtonsTrophies[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutTrophies+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtonsTrophies[i].GetComponent<ProfileTrophiesPaginationController>().setId(i);
				if(this.pageDebutTrophies+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageTrophies)
				{
					this.paginationButtonsTrophies[i].GetComponent<ProfileTrophiesPaginationController>().setActive(true);
					this.activePaginationButtonIdTrophies=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtonsTrophies[0].GetComponent<ProfileTrophiesPaginationController>().setId(-2);
				this.paginationButtonsTrophies[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtonsTrophies[nbButtonsToDraw-1].GetComponent<ProfileTrophiesPaginationController>().setId(-1);
				this.paginationButtonsTrophies[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
		}
	}
	public void paginationHandlerTrophies(int id)
	{
		if(id==-2)
		{
			this.pageDebutTrophies=this.pageDebutTrophies-this.nbPaginationButtonsLimitTrophies+1+System.Convert.ToInt32(this.pageDebutTrophies-this.nbPaginationButtonsLimitTrophies+1!=0);
			this.drawPaginationTrophies();
		}
		else if(id==-1)
		{
			this.pageDebutTrophies=this.pageDebutTrophies+this.nbPaginationButtonsLimitTrophies-1-System.Convert.ToInt32(this.pageDebutTrophies!=0);
			this.drawPaginationTrophies();
		}
		else
		{
			if(activePaginationButtonIdTrophies!=-1)
			{
				this.paginationButtonsTrophies[this.activePaginationButtonIdTrophies].GetComponent<ProfileTrophiesPaginationController>().setActive(false);
			}
			this.activePaginationButtonIdTrophies=id;
			this.chosenPageTrophies=this.pageDebutTrophies-System.Convert.ToInt32(this.pageDebutTrophies!=0)+id;
			this.drawTrophies();
		}
	}
	private void drawPaginationFriends()
	{
		for(int i=0;i<this.paginationButtonsFriends.Length;i++)
		{
			Destroy (this.paginationButtonsFriends[i]);
		}
		this.paginationButtonsFriends = new GameObject[0];
		this.activePaginationButtonIdFriends = -1;
		float paginationButtonWidth = 0.34f;
		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
		this.nbPagesFriends = Mathf.CeilToInt((float)this.friendsToBeDisplayed.Count / ((float)this.elementsPerPageFriends));
		if(this.nbPagesFriends>1)
		{
			this.nbPaginationButtonsLimitFriends = Mathf.CeilToInt((2.4f)/(paginationButtonWidth+gapBetweenPaginationButton));
			int nbButtonsToDraw=0;
			bool drawBackButton=false;
			if (this.pageDebutFriends !=0)
			{
				drawBackButton=true;
			}
			bool drawNextButton=false;
			if (this.pageDebutFriends+nbPaginationButtonsLimitFriends-System.Convert.ToInt32(drawBackButton)<this.nbPagesFriends-1)
			{
				drawNextButton=true;
				nbButtonsToDraw=nbPaginationButtonsLimitFriends;
			}
			else
			{
				nbButtonsToDraw=this.nbPagesFriends-this.pageDebutFriends;
				if(drawBackButton)
				{
					nbButtonsToDraw++;
				}
			}
			this.paginationButtonsFriends = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsFriends[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsFriends[i].AddComponent<ProfileFriendsPaginationController>();
				if(this.isMyProfile)
				{
					this.paginationButtonsFriends[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-2.15f,0f);
				}
				else
				{
					this.paginationButtonsFriends[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
				}
				this.paginationButtonsFriends[i].name="PaginationFriends"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtonsFriends[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutFriends+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtonsFriends[i].GetComponent<ProfileFriendsPaginationController>().setId(i);
				if(this.pageDebutFriends+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageFriends)
				{
					this.paginationButtonsFriends[i].GetComponent<ProfileFriendsPaginationController>().setActive(true);
					this.activePaginationButtonIdFriends=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtonsFriends[0].GetComponent<ProfileFriendsPaginationController>().setId(-2);
				this.paginationButtonsFriends[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtonsFriends[nbButtonsToDraw-1].GetComponent<ProfileFriendsPaginationController>().setId(-1);
				this.paginationButtonsFriends[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
		}
	}
	public void paginationHandlerFriends(int id)
	{
		if(id==-2)
		{
			this.pageDebutFriends=this.pageDebutFriends-this.nbPaginationButtonsLimitFriends+1+System.Convert.ToInt32(this.pageDebutFriends-this.nbPaginationButtonsLimitFriends+1!=0);
			this.drawPaginationFriends();
		}
		else if(id==-1)
		{
			this.pageDebutFriends=this.pageDebutFriends+this.nbPaginationButtonsLimitFriends-1-System.Convert.ToInt32(this.pageDebutFriends!=0);
			this.drawPaginationFriends();
		}
		else
		{
			if(activePaginationButtonIdFriends!=-1)
			{
				this.paginationButtonsFriends[this.activePaginationButtonIdFriends].GetComponent<ProfileFriendsPaginationController>().setActive(false);
			}
			this.activePaginationButtonIdFriends=id;
			this.chosenPageFriends=this.pageDebutFriends-System.Convert.ToInt32(this.pageDebutFriends!=0)+id;
			this.drawFriends();
		}
	}
	public void startHoveringPopUp()
	{
		this.isHoveringPopUp = true;
		this.toDestroyPopUp = false;
		this.popUpDestroyInterval = 0f;
	}
	public void endHoveringPopUp()
	{
		this.isHoveringPopUp = false;
		this.toDestroyPopUp = true;
		this.popUpDestroyInterval = 0f;
	}
	public void startHoveringFriendsRequest (int id)
	{
		this.idFriendsRequestHovered=id;
		this.isHoveringFriendsRequest = true;
		if(this.isPopUpDisplayed && this.popUp.GetComponent<PopUpController>().getIsFriendsRequest())
		{
			if(this.popUp.GetComponent<PopUpFriendsRequestProfileController>().getId()!=this.idFriendsRequestHovered);
			{
				this.hidePopUp();
				this.showPopUpFriendsRequest();
			}
		}
		else
		{
			if(this.isPopUpDisplayed)
			{
				this.hidePopUp();
			}
			this.showPopUpFriendsRequest();
		}
	}
	public void startHoveringFriend (int id)
	{
		this.idFriendHovered=id;
		this.isHoveringFriend = true;
		if(this.isPopUpDisplayed && this.popUp.GetComponent<PopUpController>().getIsFriend())
		{
			if(this.popUp.GetComponent<PopUpFriendProfileController>().getId()!=this.idFriendHovered);
			{
				this.hidePopUp();
				this.showPopUpFriend();
			}
		}
		else
		{
			if(this.isPopUpDisplayed)
			{
				this.hidePopUp();
			}
			this.showPopUpFriend();
		}
	}
	public void startHoveringChallengesRecord (int id)
	{
		this.idChallengesRecordHovered=id;
		this.isHoveringChallengesRecord = true;
		if(this.isPopUpDisplayed && this.popUp.GetComponent<PopUpController>().getIsChallengesRecord())
		{
			if(this.popUp.GetComponent<PopUpChallengesRecordProfileController>().getId()!=this.idChallengesRecordHovered);
			{
				this.hidePopUp();
				this.showPopUpChallengesRecord();
			}
		}
		else
		{
			if(this.isPopUpDisplayed)
			{
				this.hidePopUp();
			}
			this.showPopUpChallengesRecord();
		}
	}
	public void endHoveringFriendsRequest ()
	{
		this.isHoveringFriendsRequest = false;
		this.toDestroyPopUp = true;
		this.popUpDestroyInterval = 0f;
	}
	public void endHoveringFriend ()
	{
		this.isHoveringFriend = false;
		this.toDestroyPopUp = true;
		this.popUpDestroyInterval = 0f;
	}
	public void endHoveringChallengesRecord ()
	{
		this.isHoveringChallengesRecord = false;
		this.toDestroyPopUp = true;
		this.popUpDestroyInterval = 0f;
	}
	public void showPopUpFriendsRequest()
	{
		this.popUp = Instantiate(this.popUpObject) as GameObject;
		this.popUp.transform.position=new Vector3(this.friendsRequests[this.idFriendsRequestHovered].transform.position.x-3.1f,this.friendsRequests[this.idFriendsRequestHovered].transform.position.y,-1f);
		this.popUp.AddComponent<PopUpFriendsRequestProfileController>();
		this.popUp.GetComponent<PopUpFriendsRequestProfileController> ().setIsFriendsRequest (true);
		this.popUp.GetComponent<PopUpFriendsRequestProfileController> ().setId (this.idFriendsRequestHovered);
		this.popUp.GetComponent<PopUpFriendsRequestProfileController> ().show (model.friendsRequests [this.friendsRequestsDisplayed [this.idFriendsRequestHovered]]);
		this.isPopUpDisplayed=true;
	}
	public void showPopUpFriend()
	{
		this.popUp = Instantiate(this.popUpObject) as GameObject;
		this.popUp.transform.position=new Vector3(this.friends[this.idFriendHovered].transform.position.x-3.1f,this.friends[this.idFriendHovered].transform.position.y,-1f);
		this.popUp.AddComponent<PopUpFriendProfileController>();
		this.popUp.GetComponent<PopUpFriendProfileController> ().setIsFriend (true);
		this.popUp.GetComponent<PopUpFriendProfileController> ().setId (this.idFriendHovered);
		if(this.isMyProfile)
		{
			this.popUp.GetComponent<PopUpFriendProfileController> ().show (model.users [this.friendsToBeDisplayed[this.friendsDisplayed[this.idFriendHovered]]]);
		}
		else
		{
			this.popUp.GetComponent<PopUpFriendProfileController> ().show2 (model.users [this.friendsToBeDisplayed[this.friendsDisplayed[this.idFriendHovered]]]);
		}
		this.isPopUpDisplayed=true;
	}
	public void showPopUpChallengesRecord()
	{
		this.popUp = Instantiate(this.popUpObject) as GameObject;
		this.popUp.transform.position=new Vector3(this.challengesRecords[this.idChallengesRecordHovered].transform.position.x-3.1f,this.challengesRecords[this.idChallengesRecordHovered].transform.position.y,-1f);
		this.popUp.AddComponent<PopUpChallengesRecordProfileController>();
		this.popUp.GetComponent<PopUpChallengesRecordProfileController> ().setIsChallengesRecord (true);
		this.popUp.GetComponent<PopUpChallengesRecordProfileController> ().setId (this.idChallengesRecordHovered);
		this.popUp.GetComponent<PopUpChallengesRecordProfileController> ().show (model.challengesRecords [this.challengesRecordsDisplayed [this.idChallengesRecordHovered]]);
		this.isPopUpDisplayed=true;
	}
	public void hidePopUp()
	{
		this.toDestroyPopUp = false;
		this.popUpDestroyInterval = 0f;
		Destroy (this.popUp);
		this.isPopUpDisplayed=false;
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
			if(this.chosenPageFriends == 0)
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
	public void sendInvitationHandler()
	{
		if(!model.hasDeck)
		{
			newMenuController.instance.displayErrorPopUp("Vous ne pouvez lancer de match sans avoir au préalable créé un deck");
		}
		else if(model.users [this.friendsToBeDisplayed[this.friendsDisplayed[this.idFriendHovered]]].OnlineStatus!=1)
		{
			newMenuController.instance.displayErrorPopUp("Votre adversaire n'est plus disponible");
		}
		else
		{
			StartCoroutine (this.sendInvitation ());
		}
	}
	public IEnumerator sendInvitation()
	{
		newMenuController.instance.displayLoadingScreen ();
		// yield return StartCoroutine (model.player.SetSelectedDeck (model.decks [this.deckDisplayed].Id));
		StartCoroutine (newMenuController.instance.sendInvitation (model.users [this.friendsToBeDisplayed[this.friendsDisplayed[this.idFriendHovered]]], model.player));
		yield break;
	}
	public void acceptFriendsRequestHandler()
	{
		this.hidePopUp ();
		StartCoroutine (this.confirmFriendRequest ());
	}
	public void declineFriendsRequestHandler()
	{
		this.hidePopUp ();
		StartCoroutine (this.removeFriendRequest ());
	}
	public void cancelFriendsRequestHandler()
	{
		this.hidePopUp ();
		StartCoroutine (this.removeFriendRequest ());
	}
	public void startHoveringProfilePicture()
	{
		this.isProfilePictureHovered = true;
		this.profile.transform.FindChild ("PictureButton").gameObject.SetActive (true);
	}
	public void endHoveringProfilePicture()
	{
		this.isProfilePictureHovered = false;
		this.profile.transform.FindChild ("PictureButton").gameObject.SetActive (false);
	}
	public void editProfilePictureHandler()
	{
		this.displaySelectPicturePopUp ();
	}
	private void displaySelectPicturePopUp()
	{
		newMenuController.instance.displayTransparentBackground ();
		this.selectPicturePopUp=Instantiate(this.selectPicturePopUpObject) as GameObject;
		this.selectPicturePopUp.transform.position = new Vector3 (0f, 0f, -2f);
		this.selectPicturePopUp.GetComponent<SelectPicturePopUpController> ().selectPicture (model.player.idProfilePicture);
		this.isSelectPicturePopUpDisplayed = true;
	}
	public void hideSelectPicturePopUp()
	{
		Destroy (this.selectPicturePopUp);
		newMenuController.instance.hideTransparentBackground ();
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
		newMenuController.instance.displayLoadingScreen();
		yield return StartCoroutine(model.player.setProfilePicture(id));
		this.drawProfilePicture ();
		newMenuController.instance.changeThumbPicture (id);
		newMenuController.instance.hideLoadingScreen();
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
			this.search.transform.FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text = this.searchValue;
		}
	}
	public void searchUsersHandler()
	{
		if(this.searchValue.Length>2)
		{
			this.isSearchingUsers = false;
			this.displaySearchUsersPopUp(this.searchValue);
			this.searchValue = "";
			this.search.transform.FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text ="Rechercher";
		}
	}
	private void displaySearchUsersPopUp(string searchValue)
	{
		newMenuController.instance.displayTransparentBackground ();
		this.searchUsersPopUp=Instantiate(this.searchUsersPopUpObject) as GameObject;
		this.searchUsersPopUp.transform.position = new Vector3 (0f, 0f, -2f);
		this.searchUsersPopUp.GetComponent<SearchUsersPopUpController> ().launch (searchValue);
		this.isSearchUsersPopUpDisplayed = true;
	}
	public void hideSearchUsersPopUp()
	{
		Destroy (this.searchUsersPopUp);
		newMenuController.instance.hideTransparentBackground ();
		this.isSearchUsersPopUpDisplayed = false;
	}
	public IEnumerator confirmFriendRequest()
	{
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.confirm ());
		if(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.Error=="")
		{
			Notification tempNotification1 = new Notification(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id,model.activePlayerId,false,3);
			StartCoroutine(tempNotification1.add ());
			Notification tempNotification2 = new Notification(model.activePlayerId,model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id,false,4);
			StartCoroutine(tempNotification2.remove ());
			News tempNews1=new News(model.activePlayerId, 1,model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id.ToString());
			StartCoroutine(tempNews1.add ());
			News tempNews2=new News(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id, 1,model.activePlayerId.ToString());
			StartCoroutine(tempNews2.add ());
			model.moveToFriend(this.friendsRequestsDisplayed[this.idFriendsRequestHovered]);
			this.initializeFriendsRequests();
			this.initializeFriends();
		}
		else
		{
			newMenuController.instance.displayErrorPopUp(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.Error);
			model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.Error="";
		}
		newMenuController.instance.hideLoadingScreen ();
	}
	public IEnumerator confirmConnection()
	{
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.connectionWithMe.confirm ());
		if(model.connectionWithMe.Error=="")
		{
			Notification tempNotification1 = new Notification(model.player.Id,model.activePlayerId,false,3);
			StartCoroutine(tempNotification1.add ());
			Notification tempNotification2 = new Notification(model.activePlayerId,model.player.Id,false,4);
			StartCoroutine(tempNotification2.remove ());
			News tempNews1=new News(model.activePlayerId, 1,model.player.Id.ToString());
			StartCoroutine(tempNews1.add ());
			News tempNews2=new News(model.player.Id, 1,model.activePlayerId.ToString());
			StartCoroutine(tempNews2.add ());
			this.initializeFriendshipState();
			this.initializeFriends();
		}
		else
		{
			newMenuController.instance.displayErrorPopUp(model.connectionWithMe.Error);
			model.connectionWithMe.Error="";
		}
		newMenuController.instance.hideLoadingScreen ();
	}
	public IEnumerator removeFriendRequest()
	{
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.remove ());
		if(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.Error=="")
		{
			Notification tempNotification = new Notification ();
			tempNotification = new Notification(model.friendsRequests[this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id,model.activePlayerId,false,4);
			StartCoroutine(tempNotification.remove ());
			Notification tempNotification2 = new Notification ();
			tempNotification2 = new Notification(model.activePlayerId,model.friendsRequests[this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id,false,4);
			Notification tempNotification3 = new Notification ();
			tempNotification3 = new Notification(model.friendsRequests[this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id,model.activePlayerId,false,3);
			StartCoroutine(tempNotification3.remove ());
			Notification tempNotification4 = new Notification ();
			tempNotification4 = new Notification(model.activePlayerId,model.friendsRequests[this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].User.Id,false,3);
			StartCoroutine(tempNotification4.remove ());
			model.friendsRequests.RemoveAt(this.friendsRequestsDisplayed[this.idFriendsRequestHovered]);
			this.initializeFriendsRequests();
		}
		else
		{
			newMenuController.instance.displayErrorPopUp(model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.Error);
			model.friendsRequests [this.friendsRequestsDisplayed[this.idFriendsRequestHovered]].Connection.Error="";
		}
		newMenuController.instance.hideLoadingScreen ();
	}
	public IEnumerator removeConnection()
	{
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.connectionWithMe.remove ());
		if(model.connectionWithMe.Error=="")
		{
			Notification tempNotification = new Notification ();
			tempNotification = new Notification(model.player.Id,model.activePlayerId,false,4);
			StartCoroutine(tempNotification.remove ());
			Notification tempNotification2 = new Notification ();
			tempNotification2 = new Notification(model.activePlayerId,model.player.Id,false,4);
			StartCoroutine(tempNotification2.remove ());
			Notification tempNotification3 = new Notification ();
			tempNotification3 = new Notification(model.player.Id,model.activePlayerId,false,3);
			StartCoroutine(tempNotification3.remove ());
			Notification tempNotification4 = new Notification ();
			tempNotification4 = new Notification(model.activePlayerId,model.player.Id,false,3);
			StartCoroutine(tempNotification4.remove ());
			model.isConnectedToMe=false;
			this.initializeFriends();
			this.initializeFriendshipState();
		}
		else
		{
			newMenuController.instance.displayErrorPopUp(model.connectionWithMe.Error);
			model.connectionWithMe.Error="";
		}
		newMenuController.instance.hideLoadingScreen ();
	}
	public IEnumerator addConnection()
	{
		newMenuController.instance.displayLoadingScreen ();
		Connection connection = new Connection ();
		connection.IdUser1 = model.activePlayerId;
		connection.IdUser2 = model.player.Id;
		connection.IsAccepted = false;

		yield return StartCoroutine(connection.add ());
		if(connection.Error=="")
		{
			Notification tempNotification = new Notification(model.player.Id,model.activePlayerId,false,4);
			StartCoroutine(tempNotification.add ());
			model.isConnectedToMe=true;
			model.connectionWithMe=connection;
			this.initializeFriendshipState();
		}
		else
		{
			newMenuController.instance.displayErrorPopUp(connection.Error);
			connection.Error="";
		}
		newMenuController.instance.hideLoadingScreen ();
	}
	private void drawFriendshipState()
	{
		if(model.isConnectedToMe)
		{
			if(model.connectionWithMe.IsAccepted)
			{
				this.friendshipState.transform.FindChild("Button0").gameObject.SetActive(true);
				this.friendshipState.transform.FindChild("Button0").FindChild("Title").GetComponent<TextMeshPro>().text="Retirer";
				this.friendshipState.transform.FindChild("Button1").gameObject.SetActive(false);
				this.friendshipState.transform.FindChild("Description").GetComponent<TextMeshPro>().text="Vous êtes amis";
			}
			else if(model.connectionWithMe.IdUser1==model.player.Id)
			{
				this.friendshipState.transform.FindChild("Button0").gameObject.SetActive(true);
				this.friendshipState.transform.FindChild("Button0").FindChild("Title").GetComponent<TextMeshPro>().text="Accepter";
				this.friendshipState.transform.FindChild("Button1").gameObject.SetActive(true);
				this.friendshipState.transform.FindChild("Button1").FindChild("Title").GetComponent<TextMeshPro>().text="Refuser";
				this.friendshipState.transform.FindChild("Description").GetComponent<TextMeshPro>().text="Souhaite faire parti de vos amis";
			}
			else if(model.connectionWithMe.IdUser1==model.activePlayerId)
			{
				this.friendshipState.transform.FindChild("Button0").gameObject.SetActive(true);
				this.friendshipState.transform.FindChild("Button0").FindChild("Title").GetComponent<TextMeshPro>().text="Annuler";
				this.friendshipState.transform.FindChild("Button1").gameObject.SetActive(false);
				this.friendshipState.transform.FindChild("Description").GetComponent<TextMeshPro>().text="n'a pas encore répondu à votre invitation";
			}
		}
		else
		{
			this.friendshipState.transform.FindChild("Button0").gameObject.SetActive(true);
			this.friendshipState.transform.FindChild("Button0").FindChild("Title").GetComponent<TextMeshPro>().text="Ajouer";
			this.friendshipState.transform.FindChild("Button1").gameObject.SetActive(false);
			this.friendshipState.transform.FindChild("Description").GetComponent<TextMeshPro>().text="ne fait pas partie de vos amis";
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
}