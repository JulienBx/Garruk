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
	
	private GameObject searchBlock;
	private GameObject friendsBlock;
	private GameObject friendsRequestsBlock;
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
	
	private GameObject stats;
	private GameObject profile;
	private GameObject search;
	private GameObject collectionButton;
	private GameObject cleanCardsButton;

	private GameObject[] friends;
	private GameObject[] friendsRequests;
	private GameObject[] trophies;
	private GameObject[] challengesRecords;
	
	private GameObject[] paginationButtonsFriends;
	private GameObject[] paginationButtonsFriendsRequests;
	private GameObject[] paginationButtonsChallengesRecords;
	private GameObject[] paginationButtonsTrophies;

	private int widthScreen;
	private int heightScreen;
	private float worldWidth;
	private float worldHeight;
	private float pixelPerUnit;
	private Rect centralWindow;
	
	private IList<int> friendsRequestsDisplayed;
	private IList<int> challengesRecordsDisplayed;
	private IList<int> trophiesDisplayed;
	private IList<int> friendsDisplayed;
	private IList<int> friendsToBeDisplayed;
	
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
	
	private bool isSceneLoaded;
	
	private bool isSearchingDeck;
	private bool isMouseOnSelectDeckButton;
	
	private bool isHoveringFriendsRequest;
	private bool isHoveringFriend;
	private bool isHoveringPopUp;
	private bool isPopUpDisplayed;
	private int idFriendsRequestHovered;
	private int idFriendHovered;

	private bool toDestroyPopUp;
	private float popUpDestroyInterval;
	
	private bool areFriendsRequestsPicturesLoading;
	private bool areFriendsPicturesLoading;
	private bool areTrophiesPicturesLoading;
	private bool areChallengesRecordsPicturesLoading;
	
	private bool isTutorialLaunched;

	public int friendsRefreshInterval;
	private float friendsCheckTimer;
	
	void Update()
	{	
		this.friendsCheckTimer += Time.deltaTime;
		
		if (friendsCheckTimer > friendsRefreshInterval && this.isSceneLoaded) 
		{
			this.checkFriendsOnlineStatus();
		}
		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			this.resize();
			this.drawPaginationChallengesRecords();
			this.drawPaginationFriendsRequests();
			this.drawPaginationFriends();
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
		if(Input.GetKeyDown(KeyCode.Return)) 
		{
			this.returnPressed();
		}
		if(Input.GetKeyDown(KeyCode.Escape) && !isTutorialLaunched) 
		{
			this.escapePressed();
		}
		if(areFriendsRequestsPicturesLoading)
		{
			bool allPicturesLoaded=true;
			for(int i=0;i<friendsRequestsDisplayed.Count;i++)
			{
				if(!model.friendsRequests[this.friendsRequestsDisplayed[i]].User.isThumbPictureLoaded)
				{
					allPicturesLoaded=false;
					break;
				}
			}
			if(allPicturesLoaded)
			{
				this.areFriendsRequestsPicturesLoading=false;
				for(int i=0;i<friendsRequestsDisplayed.Count;i++)
				{
					this.friendsRequests[i].GetComponent<FriendsRequestController>().setPicture(model.friendsRequests[this.friendsRequestsDisplayed[i]].User.texture);
				}
			}
		}
		if(areFriendsPicturesLoading)
		{
			bool allPicturesLoaded=true;
			for(int i=0;i<friendsDisplayed.Count;i++)
			{
				if(!model.users[this.friendsToBeDisplayed[this.friendsDisplayed[i]]].isThumbPictureLoaded)
				{
					allPicturesLoaded=false;
					break;
				}
			}
			if(allPicturesLoaded)
			{
				this.areFriendsPicturesLoading=false;
				for(int i=0;i<friendsDisplayed.Count;i++)
				{
					this.friends[i].GetComponent<ProfileOnlineFriendController>().setPicture(model.users[this.friendsToBeDisplayed[this.friendsDisplayed[i]]].texture);
				}
			}
		}
		if(areChallengesRecordsPicturesLoading)
		{
			bool allPicturesLoaded=true;
			for(int i=0;i<challengesRecordsDisplayed.Count;i++)
			{
				if(!model.challengesRecords[this.challengesRecordsDisplayed[i]].Friend.isThumbPictureLoaded)
				{
					allPicturesLoaded=false;
					break;
				}
			}
			if(allPicturesLoaded)
			{
				this.areChallengesRecordsPicturesLoading=false;
				for(int i=0;i<challengesRecordsDisplayed.Count;i++)
				{
					this.challengesRecords[i].GetComponent<ChallengesRecordController>().setPicture(model.challengesRecords[this.challengesRecordsDisplayed[i]].Friend.texture);
				}
			}
		}
		if(areTrophiesPicturesLoading)
		{
			bool allPicturesLoaded=true;
			for(int i=0;i<trophiesDisplayed.Count;i++)
			{
				if(!model.trophies[i].competition.isTextureLoaded)
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
					this.trophies[i+1].GetComponent<TrophyController>().setPicture(model.trophies[i].competition.texture);
				}
			}
		}
	}
	void Awake()
	{
		instance = this;
		this.model = new NewProfileModel ();
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.pixelPerUnit = 108f;
		this.elementsPerPageFriends = 5;
		this.elementsPerPageFriendsRequests = 2;
		this.elementsPerPageTrophies = 4;
		this.elementsPerPageChallengesRecords = 4;
		this.initializeScene ();
		this.resize ();
	}
	public IEnumerator initialization()
	{
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.getData ());
		this.initializeFriends ();
		this.initializeFriendsRequests ();
		this.initializeChallengesRecords ();
		this.initializeTrophies ();
		this.initializeStats ();
		this.checkFriendsOnlineStatus ();
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
		this.sortFriendsList ();
		this.drawPaginationTrophies();
		this.drawTrophies ();
	}
	private void initializeStats()
	{
		this.drawStats ();
	}
	public void initializeScene()
	{
		menu = GameObject.Find ("newMenu");
		menu.AddComponent<newProfileMenuController> ();
		menu.GetComponent<newMenuController> ().setCurrentPage (0);
		this.friendsOnline = new List<int> ();
		this.profileBlock = Instantiate(this.blockObject) as GameObject;
		this.statsBlock = Instantiate(this.blockObject) as GameObject;
		this.trophiesBlock = Instantiate(this.blockObject) as GameObject;
		this.friendsRequestsBlock = Instantiate(this.blockObject) as GameObject;
		this.friendsBlock = Instantiate (this.blockObject) as GameObject;
		this.searchBlock = Instantiate(this.blockObject) as GameObject;
		this.challengesRecordsBlock = Instantiate(this.blockObject) as GameObject;
		this.profileTitle = GameObject.Find ("ProfileTitle");
		this.profileTitle.GetComponent<TextMeshPro> ().text = "Mon profil";
		this.friendsRequestsTitle = GameObject.Find ("FriendsRequestsTitle");
		this.friendsRequestsTitle.GetComponent<TextMeshPro> ().text = "Mes invitations";
		this.challengesRecordsTitle = GameObject.Find ("challengesRecordsTitle");
		this.challengesRecordsTitle.GetComponent<TextMeshPro> ().text = "Amis défiés";
		this.statsTitle = GameObject.Find ("StatsTitle");
		this.statsTitle.GetComponent<TextMeshPro> ().text = "Statistiques";
		this.friendsTitle = GameObject.Find ("FriendsTitle");
		this.friendsTitle.GetComponent<TextMeshPro> ().text = "Amis en ligne";
		this.searchTitle = GameObject.Find ("SearchTitle");
		this.searchTitle.GetComponent<TextMeshPro> ().text = "Recherche un ami";
		this.trophiesTitle = GameObject.Find ("TrophiesTitle");
		this.trophiesTitle.GetComponent<TextMeshPro> ().text = "Mes trophées";
		this.stats = GameObject.Find ("Stats");
		this.paginationButtonsFriendsRequests = new GameObject[0];
		this.paginationButtonsChallengesRecords = new GameObject[0];
		this.paginationButtonsFriends = new GameObject[0];
		this.paginationButtonsTrophies = new GameObject[0];
		this.challengesRecords=new GameObject[2];
		for(int i=0;i<this.challengesRecords.Length;i++)
		{
			this.challengesRecords[i]=GameObject.Find ("ChallengesRecord"+i);
			this.challengesRecords[i].GetComponent<ChallengesRecordController>().setId(i);
			this.challengesRecords[i].SetActive(false);
		}
		this.friends=new GameObject[2];
		for(int i=0;i<this.friends.Length;i++)
		{
			this.friends[i]=GameObject.Find ("Friend"+i);
			this.friends[i].GetComponent<OnlineFriendController>().setId(i);
			this.friends[i].SetActive(false);
		}
		this.friendsRequests=new GameObject[2];
		for(int i=0;i<this.friendsRequests.Length;i++)
		{
			this.friendsRequests[i]=GameObject.Find ("FriendsRequest"+i);
			this.friendsRequests[i].GetComponent<FriendsRequestController>().setId(i);
			this.friendsRequests[i].SetActive(false);
		}
		this.trophies=new GameObject[2];
		for(int i=0;i<this.trophies.Length;i++)
		{
			this.trophies[i]=GameObject.Find ("Trophy"+i);
			this.trophies[i].GetComponent<TrophyController>().setId(i);
			this.trophies[i].SetActive(false);
		}

		this.collectionButton = GameObject.Find ("CollectionButton");
		this.collectionButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "Cristalopedia";
		this.cleanCardsButton = GameObject.Find ("CleanCardsButton");
		this.cleanCardsButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "Vider";
		if(!ApplicationModel.isAdmin)
		{
			this.cleanCardsButton.SetActive(false);
		}
		
	}
	public void resize()
	{
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		float screenRatio = (float)this.widthScreen / (float)this.heightScreen;
		menu.GetComponent<newMenuController> ().resizeMeunObject (worldHeight,worldWidth);
		
		float challengesRecordsBlockLeftMargin =3f;
		float challengesRecordsBlockRightMargin = (this.worldWidth - 3f - 3f) / 2f + 3.1f;
		float challengesRecordsBlockUpMargin = 6.1f;
		float challengesRecordsBlockDownMargin = 0.2f;
		
		float challengesRecordsBlockHeight = worldHeight - challengesRecordsBlockUpMargin-challengesRecordsBlockDownMargin;
		float challengesRecordsBlockWidth = worldWidth-challengesRecordsBlockLeftMargin-challengesRecordsBlockRightMargin;
		Vector2 challengesRecordsBlockOrigin = new Vector3 (-worldWidth/2f+challengesRecordsBlockLeftMargin+challengesRecordsBlockWidth/2f, -worldHeight / 2f + challengesRecordsBlockDownMargin + challengesRecordsBlockHeight / 2,0f);
		
		this.challengesRecordsBlock.GetComponent<BlockController> ().resize(new Rect(challengesRecordsBlockOrigin.x,challengesRecordsBlockOrigin.y,challengesRecordsBlockWidth,challengesRecordsBlockHeight));
		this.challengesRecordsTitle.transform.position = new Vector3 (challengesRecordsBlockOrigin.x, challengesRecordsBlockOrigin.y+challengesRecordsBlockHeight/2f-0.3f, 0);
		
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
		this.cleanCardsButton.transform.position = new Vector3 (1.5f * statsBlockWidth / 5f, statsBlockOrigin.y+statsBlockHeight/2f-0.58f, 0f);
		
		float profileBlockLeftMargin = 3f;
		float profileBlockRightMargin = 3f;
		float profileBlockUpMargin = 0.2f;
		float profileBlockDownMargin = 6.6f;
		
		float profileBlockHeight = worldHeight - profileBlockUpMargin-profileBlockDownMargin;
		float profileBlockWidth = worldWidth-profileBlockLeftMargin-profileBlockRightMargin;
		Vector2 profileBlockOrigin = new Vector3 (-worldWidth/2f+profileBlockLeftMargin+profileBlockWidth/2f, -worldHeight / 2f + profileBlockDownMargin + profileBlockHeight / 2,0f);
		
		this.profileBlock.GetComponent<BlockController> ().resize(new Rect(profileBlockOrigin.x,profileBlockOrigin.y,profileBlockWidth,profileBlockHeight));
		this.profileTitle.transform.position = new Vector3 (statsBlockOrigin.x, statsBlockOrigin.y+statsBlockHeight/2f-0.3f, 0);
		
		float friendsBlockLeftMargin = this.worldWidth-2.8f;
		float friendsBlockRightMargin = 0f;
		float friendsBlockUpMargin = 4.1f;
		float friendsBlockDownMargin = 2.6f;
		
		float friendsBlockHeight = worldHeight - friendsBlockUpMargin-friendsBlockDownMargin;
		float friendsBlockWidth = worldWidth-friendsBlockLeftMargin-friendsBlockRightMargin;
		Vector2 friendsBlockOrigin = new Vector3 (-worldWidth/2f+friendsBlockLeftMargin+friendsBlockWidth/2f, -worldHeight / 2f + friendsBlockDownMargin + friendsBlockHeight / 2,0f);
		
		this.friendsBlock.GetComponent<BlockController> ().resize(new Rect(friendsBlockOrigin.x,friendsBlockOrigin.y, friendsBlockWidth, friendsBlockHeight));
		this.friendsTitle.transform.position = new Vector3 (friendsBlockOrigin.x, friendsBlockOrigin.y+friendsBlockHeight/2f-0.3f, 0);
		
		float searchBlockLeftMargin = this.worldWidth-2.8f;
		float searchBlockRightMargin = 0f;
		float searchBlockUpMargin = 0.6f;
		float searchBlockDownMargin = 6.1f;
		
		float searchBlockHeight = worldHeight - searchBlockUpMargin-searchBlockDownMargin;
		float searchBlockWidth = worldWidth-searchBlockLeftMargin-searchBlockRightMargin;
		Vector2 searchBlockOrigin = new Vector3 (-worldWidth/2f+searchBlockLeftMargin+searchBlockWidth/2f, -worldHeight / 2f + searchBlockDownMargin + searchBlockHeight / 2,0f);
		
		this.searchBlock.GetComponent<BlockController> ().resize(new Rect(searchBlockOrigin.x,searchBlockOrigin.y,searchBlockWidth,searchBlockHeight));
		this.searchTitle.transform.position = new Vector3 (searchBlockOrigin.x, searchBlockOrigin.y+searchBlockHeight/2f-0.3f, 0);
		
		float friendsRequestsBlockLeftMargin = this.worldWidth-2.8f;
		float friendsRequestsBlockRightMargin = 0f;
		float friendsRequestsBlockUpMargin = 7.6f;
		float friendsRequestsBlockDownMargin = 0.2f;
		
		float friendsRequestsBlockHeight = worldHeight - friendsRequestsBlockUpMargin-friendsRequestsBlockDownMargin;
		float friendsRequestsBlockWidth = worldWidth-friendsRequestsBlockLeftMargin-friendsRequestsBlockRightMargin;
		Vector2 friendsRequestsBlockOrigin = new Vector3 (-worldWidth/2f+friendsRequestsBlockLeftMargin+friendsRequestsBlockWidth/2f, -worldHeight / 2f + friendsRequestsBlockDownMargin + friendsRequestsBlockHeight / 2,0f);
		
		this.friendsRequestsBlock.GetComponent<BlockController> ().resize(new Rect(friendsRequestsBlockOrigin.x,friendsRequestsBlockOrigin.y,friendsRequestsBlockWidth,friendsRequestsBlockHeight));
		this.friendsRequestsTitle.transform.position = new Vector3 (friendsRequestsBlockOrigin.x, friendsRequestsBlockOrigin.y+friendsRequestsBlockHeight/2f-0.3f, 0);
		
		for(int i=0;i<this.trophies.Length;i++)
		{
			this.trophies[i].transform.position=new Vector3(trophiesBlockOrigin.x-0.2f,trophiesBlockOrigin.y+trophiesBlockHeight/2f-0.85f-i*0.77f,0);
		}

		for(int i=0;i<this.challengesRecords.Length;i++)
		{
			this.challengesRecords[i].transform.position=new Vector3(challengesRecordsBlockOrigin.x-0.2f,challengesRecordsBlockOrigin.y+challengesRecordsBlockHeight/2f-0.85f-i*0.77f,0);
		}

		for(int i=0;i<this.friendsRequests.Length;i++)
		{
			this.friendsRequests[i].transform.position=new Vector3(friendsRequestsBlockOrigin.x-0.2f,friendsRequestsBlockOrigin.y+friendsRequestsBlockHeight/2f-0.85f-i*0.77f,0);
		}
		
		for(int i=0;i<this.friends.Length;i++)
		{
			this.friends[i].transform.position=new Vector3(friendsBlockOrigin.x-0.2f,friendsBlockOrigin.y+friendsBlockHeight/2f-0.75f-i*0.65f,0); 
		}
		
		if(this.isTutorialLaunched)
		{
			this.tutorial.GetComponent<TutorialObjectController>().resize();
		}
	}
	

	public void returnPressed()
	{
		if(newMenuController.instance.isAPopUpDisplayed())
		{
			newMenuController.instance.returnPressed();
		}
	}
	public void escapePressed()
	{
		if(newMenuController.instance.isAPopUpDisplayed())
		{
			newMenuController.instance.escapePressed();
		}
	}
	
	public void drawChallengesRecords()
	{
		this.challengesRecordsDisplayed = new List<int> ();
		bool allPicturesLoaded = true;
		for(int i =0;i<elementsPerPageChallengesRecords;i++)
		{
			if(this.chosenPageChallengesRecords*this.elementsPerPageChallengesRecords+i<model.challengesRecords.Count)
			{
				if(!model.challengesRecords[this.chosenPageChallengesRecords*this.elementsPerPageChallengesRecords+i].Friend.isThumbPictureLoaded)
				{
					if(!model.challengesRecords[this.chosenPageChallengesRecords*this.elementsPerPageChallengesRecords+i].Friend.isThumbPictureLoading)
					{
						StartCoroutine(model.challengesRecords[this.chosenPageChallengesRecords*this.elementsPerPageChallengesRecords+i].Friend.setThumbProfilePicture());
					}
					allPicturesLoaded=false;
				}
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
		if(!allPicturesLoaded)
		{
			this.areChallengesRecordsPicturesLoading=true;
		}
	}
	public void drawFriendsRequests()
	{
		this.friendsRequestsDisplayed = new List<int> ();
		bool allPicturesLoaded = true;
		for(int i =0;i<elementsPerPageFriendsRequests;i++)
		{
			if(this.chosenPageFriendsRequests*this.elementsPerPageFriendsRequests+i<model.friendsRequests.Count)
			{
				if(!model.friendsRequests[this.chosenPageFriendsRequests*this.elementsPerPageFriendsRequests+i].User.isThumbPictureLoaded)
				{
					if(!model.friendsRequests[this.chosenPageFriendsRequests*this.elementsPerPageFriendsRequests+i].User.isThumbPictureLoading)
					{
						StartCoroutine(model.friendsRequests[this.chosenPageFriendsRequests*this.elementsPerPageFriendsRequests+i].User.setThumbProfilePicture());
					}
					allPicturesLoaded=false;
				}
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
		if(!allPicturesLoaded)
		{
			this.areFriendsRequestsPicturesLoading=true;
		}
	}
	public void drawFriends()
	{
		this.friendsDisplayed = new List<int> ();
		bool allPicturesLoaded = true;
		for(int i =0;i<elementsPerPageFriends;i++)
		{
			if(this.chosenPageFriends*this.elementsPerPageFriends+i<this.friendsToBeDisplayed.Count)
			{
				if(!model.users[this.friendsToBeDisplayed[this.chosenPageFriends*this.elementsPerPageFriends+i]].isThumbPictureLoaded)
				{
					if(!model.users[this.friendsToBeDisplayed[this.chosenPageFriends*this.elementsPerPageFriends+i]].isThumbPictureLoading)
					{
						StartCoroutine(model.users[this.friendsToBeDisplayed[this.chosenPageFriends*this.elementsPerPageFriends+i]].setThumbProfilePicture());
					}
					allPicturesLoaded=false;
				}
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
		if(!allPicturesLoaded)
		{
			this.areFriendsPicturesLoading=true;
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
				if(!model.trophies[this.chosenPageTrophies*this.elementsPerPageTrophies+i].User.isThumbPictureLoaded)
				{
					if(!model.trophies[this.chosenPageTrophies*this.elementsPerPageTrophies+i].User.isThumbPictureLoading)
					{
						StartCoroutine(model.trophies[this.chosenPageTrophies*this.elementsPerPageTrophies+i].User.setThumbProfilePicture());
					}
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
			}
			this.paginationButtonsChallengesRecords = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsChallengesRecords[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsChallengesRecords[i].AddComponent<ProfileChallengesRecordsPaginationController>();
				this.paginationButtonsChallengesRecords[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),1.35f,0f);
				this.paginationButtonsChallengesRecords[i].name="PaginationNotification"+i.ToString();
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
			}
			this.paginationButtonsFriendsRequests = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsFriendsRequests[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsFriendsRequests[i].AddComponent<ProfileFriendsRequestsPaginationController>();
				this.paginationButtonsFriendsRequests[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),1.35f,0f);
				this.paginationButtonsFriendsRequests[i].name="PaginationNotification"+i.ToString();
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
			}
			this.paginationButtonsTrophies = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsTrophies[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsTrophies[i].AddComponent<ProfileTrophiesPaginationController>();
				this.paginationButtonsTrophies[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),1.35f,0f);
				this.paginationButtonsTrophies[i].name="PaginationNotification"+i.ToString();
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
			}
			this.paginationButtonsFriends = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsFriends[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsFriends[i].AddComponent<HomePageFriendsPaginationController>();
				this.paginationButtonsFriends[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
				this.paginationButtonsFriends[i].name="PaginationFriends"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtonsFriends[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutFriends+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtonsFriends[i].GetComponent<HomePageFriendsPaginationController>().setId(i);
				if(this.pageDebutFriends+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageFriends)
				{
					this.paginationButtonsFriends[i].GetComponent<HomePageFriendsPaginationController>().setActive(true);
					this.activePaginationButtonIdFriends=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtonsFriends[0].GetComponent<HomePageFriendsPaginationController>().setId(-2);
				this.paginationButtonsFriends[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtonsFriends[nbButtonsToDraw-1].GetComponent<HomePageFriendsPaginationController>().setId(-1);
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
				this.paginationButtonsFriends[this.activePaginationButtonIdFriends].GetComponent<HomePageFriendsPaginationController>().setActive(false);
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
			if(this.popUp.GetComponent<PopUpFriendHomePageController>().getId()!=this.idFriendHovered);
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
		this.popUp.GetComponent<PopUpFriendProfileController> ().setIsNews (true);
		this.popUp.GetComponent<PopUpFriendProfileController> ().setId (this.idFriendHovered);
		this.popUp.GetComponent<PopUpFriendProfileController> ().show (model.users [this.friendsDisplayed [this.idFriendHovered]]);
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
	public void sortFriendsList()
	{
		this.friendsToBeDisplayed = new List<int> ();
		for(int i=0;i<this.friendsOnline.Count;i++)
		{
			this.friendsToBeDisplayed.Add (this.friendsOnline[i]);
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
}