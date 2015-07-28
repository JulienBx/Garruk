using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class NewHomePageController : MonoBehaviour
{
	public static NewHomePageController instance;
	private NewHomePageModel model;

	public GameObject paginationButtonObject;
	public GameObject packObject;
	public GameObject competitionObject;
	public GUISkin popUpSkin;
	public int refreshInterval;
	public int sliderRefreshInterval;
	public int totalNbResultLimit;
	
	private GameObject menu;
	private GameObject storeBoard;
	private GameObject storeAssetsTitle;
	private GameObject newsBoard;
	private GameObject newsTitle;
	private GameObject notificationsBoard;
	private GameObject notificationsTitle;
	private GameObject competitionsBoard;
	private GameObject competitionsTitle;
	private GameObject[] news;
	private GameObject[] notifications;
	private GameObject[] packs;
	private GameObject[] competitions;
	private GameObject[] paginationButtonsNotifications;
	private GameObject[] paginationButtonsNews;
	
	private int widthScreen;
	private int heightScreen;
	private float worldWidth;
	private float worldHeight;
	private float pixelPerUnit;
	private Rect centralWindow;
	
	private IList<int> newsDisplayed;
	private IList<int> notificationsDisplayed;
	private IList<int> packsDisplayed;
	private IList<int> competitionsDisplayed;
	
	private int nbPagesNotifications;
	private int nbPaginationButtonsLimitNotifications;
	private int elementsPerPageNotifications;
	private int chosenPageNotifications;
	private int pageDebutNotifications;
	private int activePaginationButtonIdNotifications;

	private int nbPagesNews;
	private int nbPaginationButtonsLimitNews;
	private int elementsPerPageNews;
	private int chosenPageNews;
	private int pageDebutNews;
	private int activePaginationButtonIdNews;

	private int nbPagesPacks;
	private int chosenPagePacks;

	private int nbPagesCompetitions;
	private int chosenPageCompetitions;
	
	private float timer;
	private bool isSceneLoaded;
	
	private int money;

	private bool areNotificationsPicturesLoading;
	private Texture2D[] notificationsPictures;
	private Rect[] atlasNotificationsPicturesRects;
	private Texture2D atlasNotificationsPictures;

	private bool areNewsPicturesLoading;
	private Texture2D[] newsPictures;
	private Rect[] atlasNewsPicturesRects;
	private Texture2D atlasNewsPictures;

	private bool arePacksPicturesLoading;
	private Texture2D[] packsPictures;
	private Rect[] atlasPacksPicturesRects;
	private Texture2D atlasPacksPictures;

	private bool areCompetitionsPicturesLoading;
	private Texture2D[] competitionsPictures;
	private Rect[] atlasCompetitionsPicturesRects;
	private Texture2D atlasCompetitionsPictures;

	private int packsPerLine;
	private int competitionsPerLine;
	
	void Update()
	{	
		this.timer += Time.deltaTime;

		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			this.resize();
			this.drawPaginationNews();
			this.drawPaginationNotifications();
			this.initializePacks();
		}
		if(this.timer>this.sliderRefreshInterval)
		{
			this.timer=0;
			if(this.isSceneLoaded)
			{
				if(nbPagesPacks-1>this.chosenPagePacks)
				{
					this.chosenPagePacks++;
					this.drawPacks();
				}
				else if(this.chosenPagePacks!=0)
				{
					this.chosenPagePacks=0;
					this.drawPacks();
				}
			}
		}
		if(areNotificationsPicturesLoading)
		{
			bool createAtlas=true;
			for(int i =0;i<this.notificationsDisplayed.Count;i++)
			{
				if(!model.notifications[this.notificationsDisplayed[i]].SendingUser.isThumbPictureLoaded)
				{
					createAtlas=false;
				}
			}
			if(createAtlas)
			{
				this.areNotificationsPicturesLoading=false;
				this.atlasNotificationsPictures = new Texture2D(8192, 8192);
				this.atlasNotificationsPicturesRects = atlasNotificationsPictures.PackTextures(notificationsPictures, 2, 8192);
				
				for(int i=0;i<this.notificationsDisplayed.Count;i++)
				{
					this.notifications [i].GetComponent<NotificationController> ().setPicture(Sprite.Create(this.atlasNotificationsPictures,new Rect(this.atlasNotificationsPicturesRects[i].x*atlasNotificationsPictures.width,this.atlasNotificationsPicturesRects[i].y*atlasNotificationsPictures.height,this.atlasNotificationsPicturesRects[i].width*atlasNotificationsPictures.width,this.atlasNotificationsPicturesRects[i].height*atlasNotificationsPictures.height),new Vector2(0.5f,0.5f)));
				}
			}
		}
		if(areNewsPicturesLoading)
		{
			bool createAtlas=true;
			for(int i =0;i<this.newsDisplayed.Count;i++)
			{
				if(!model.news[this.newsDisplayed[i]].User.isThumbPictureLoaded)
				{
					createAtlas=false;
				}
			}
			if(createAtlas)
			{
				this.areNewsPicturesLoading=false;
				this.atlasNewsPictures = new Texture2D(8192, 8192);
				this.atlasNewsPicturesRects = atlasNewsPictures.PackTextures(newsPictures, 2, 8192);
				
				for(int i=0;i<this.newsDisplayed.Count;i++)
				{
					this.news [i].GetComponent<NewsController> ().setPicture(Sprite.Create(this.atlasNewsPictures,new Rect(this.atlasNewsPicturesRects[i].x*atlasNewsPictures.width,this.atlasNewsPicturesRects[i].y*atlasNewsPictures.height,this.atlasNewsPicturesRects[i].width*atlasNewsPictures.width,this.atlasNewsPicturesRects[i].height*atlasNewsPictures.height),new Vector2(0.5f,0.5f)));
				}
			}
		}
		if(arePacksPicturesLoading)
		{
			bool createAtlas=true;
			for(int i =0;i<this.packsDisplayed.Count;i++)
			{
				if(!model.packs[this.packsDisplayed[i]].isTextureLoaded)
				{
					createAtlas=false;
				}
			}
			if(createAtlas)
			{
				this.arePacksPicturesLoading=false;
				this.atlasPacksPictures = new Texture2D(8192, 8192);
				this.atlasPacksPicturesRects = atlasPacksPictures.PackTextures(packsPictures, 2, 8192);
				
				for(int i=0;i<this.packsDisplayed.Count;i++)
				{
					this.packs [i].GetComponent<NewPackController> ().setPackPicture(Sprite.Create(this.atlasPacksPictures,new Rect(this.atlasPacksPicturesRects[i].x*atlasPacksPictures.width,this.atlasPacksPicturesRects[i].y*atlasPacksPictures.height,this.atlasPacksPicturesRects[i].width*atlasPacksPictures.width,this.atlasPacksPicturesRects[i].height*atlasPacksPictures.height),new Vector2(0.5f,0.5f)));
				}
			}
		}
		if(areCompetitionsPicturesLoading)
		{
			bool createAtlas=true;
			if(!model.currentDivision.isTextureLoaded)
			{
				createAtlas=false;
			}
			if(competitionsDisplayed.Count>1 && !model.currentCup.isTextureLoaded)
			{
				createAtlas=false;
			}
			if(createAtlas)
			{
				this.areCompetitionsPicturesLoading=false;
				this.atlasCompetitionsPictures = new Texture2D(8192, 8192);
				this.atlasCompetitionsPicturesRects = atlasCompetitionsPictures.PackTextures(competitionsPictures, 2, 8192);
				
				for(int i=0;i<this.competitionsDisplayed.Count;i++)
				{
					this.competitions [i].GetComponent<CompetitionController> ().setPicture(Sprite.Create(this.atlasCompetitionsPictures,new Rect(this.atlasCompetitionsPicturesRects[i].x*atlasCompetitionsPictures.width,this.atlasCompetitionsPicturesRects[i].y*atlasCompetitionsPictures.height,this.atlasCompetitionsPicturesRects[i].width*atlasCompetitionsPictures.width,this.atlasCompetitionsPicturesRects[i].height*atlasCompetitionsPictures.height),new Vector2(0.5f,0.5f)));
				}
			}
		}
	}
	void Awake()
	{
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.pixelPerUnit = 108f;
		this.elementsPerPageNotifications = 3;
		this.elementsPerPageNews = 6;
		this.initializeScene ();
	}
	void Start()
	{
		instance = this;
		this.model = new NewHomePageModel ();
		this.resize ();
		StartCoroutine (this.initialization ());
	}
	private IEnumerator initialization()
	{
		yield return StartCoroutine (model.getData (this.totalNbResultLimit));
		this.initializeNotifications ();
		this.initializeNews ();
		this.initializePacks ();
		this.initializeCompetitions ();
		this.isSceneLoaded = true;
	}
	private void initializeNotifications()
	{
		this.chosenPageNotifications = 0;
		this.pageDebutNotifications = 0 ;
		this.drawPaginationNotifications();
		this.drawNotifications ();
	}
	private void initializeNews()
	{
		this.chosenPageNews = 0;
		this.pageDebutNews = 0 ;
		this.drawPaginationNews();
		this.drawNews ();
	}
	private void initializePacks()
	{
		this.chosenPagePacks = 0;
		this.drawPacks ();
	}
	private void initializeCompetitions()
	{
		this.chosenPageCompetitions = 0;
		this.drawCompetitions ();
	}
	public void initializeScene()
	{
		menu = GameObject.Find ("newMenu");
		menu.GetComponent<newMenuController> ().setCurrentPage (0);
		this.storeBoard = GameObject.Find ("StoreBoard");
		this.newsBoard = GameObject.Find ("NewsBoard");
		this.notificationsBoard = GameObject.Find ("NotificationsBoard");
		this.competitionsBoard = GameObject.Find ("CompetitionsBoard");
		this.storeAssetsTitle = GameObject.Find ("StoreAssetsTitle");
		this.storeAssetsTitle.GetComponent<TextMeshPro> ().text = "Nouveautés";
		this.newsTitle = GameObject.Find ("NewsTitle");
		this.newsTitle.GetComponent<TextMeshPro> ().text = "Fil d'actualités";
		this.notificationsTitle = GameObject.Find ("NotificationsTitle");
		this.notificationsTitle.GetComponent<TextMeshPro> ().text = "Notifications";
		this.competitionsTitle = GameObject.Find ("CompetitionsTitle");
		this.competitionsTitle.GetComponent<TextMeshPro> ().text = "Mes compétitions";
		this.paginationButtonsNotifications = new GameObject[0];
		this.paginationButtonsNews = new GameObject[0];
		this.packs=new GameObject[0];
		this.competitions=new GameObject[0];
		this.notifications=new GameObject[3];
		for(int i=0;i<this.notifications.Length;i++)
		{
			this.notifications[i]=GameObject.Find ("Notification"+i);
			this.notifications[i].SetActive(false);
		}
		this.news=new GameObject[6];
		for(int i=0;i<this.news.Length;i++)
		{
			this.news[i]=GameObject.Find ("News"+i);
			this.news[i].SetActive(false);
		}
	}
	public void resize()
	{
		this.cleanPacks ();
		this.cleanCompetitions ();
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		float screenRatio = (float)this.widthScreen / (float)this.heightScreen;
		menu.GetComponent<newMenuController> ().resizeMeunObject (worldHeight,worldWidth);

		float storeBoardLeftMargin = 2.9f;
		float storeBoardRightMargin = (this.worldWidth - 2.9f - 3.25f) / 2f + 3.25f;
		float storeBoardUpMargin = 5f;
		float storeBoardDownMargin = 0.25f;

		float storeBoardHeight = worldHeight - storeBoardUpMargin-storeBoardDownMargin;
		float storeBoardWidth = worldWidth-storeBoardLeftMargin-storeBoardRightMargin;
		Vector2 storeBoardOrigin = new Vector3 (-worldWidth/2f+storeBoardLeftMargin+storeBoardWidth/2f, -worldHeight / 2f + storeBoardDownMargin + storeBoardHeight / 2,0f);
		
		this.storeBoard.GetComponent<BoardController> ().resize(storeBoardWidth,storeBoardHeight,storeBoardOrigin);
		this.storeAssetsTitle.transform.position = new Vector3 (storeBoardOrigin.x, storeBoardOrigin.y+storeBoardHeight/2f-0.3f, 0);

		float packScale = 0.84f;
		
		float packWidth = 250f;
		float packWorldWidth = (packWidth / pixelPerUnit) * packScale;
		
		this.packsPerLine = Mathf.FloorToInt ((storeBoardWidth-0.5f) / packWorldWidth);

		float packsGapWidth = (storeBoardWidth - (this.packsPerLine * packWorldWidth)) / (this.packsPerLine + 1);
		float packsBoardStartX = storeBoardOrigin.x - storeBoardWidth / 2f-packWorldWidth/2f;

		this.packs=new GameObject[this.packsPerLine];
		
		for(int i =0;i<this.packsPerLine;i++)
		{
			this.packs[i] = Instantiate(this.packObject) as GameObject;
			this.packs[i].transform.localScale= new Vector3(0.7f,0.7f,0.7f);
			this.packs[i].transform.position=new Vector3(packsBoardStartX+(i+1)*(packsGapWidth+packWorldWidth),storeBoardOrigin.y-0.5f,0f);
			this.packs[i].transform.name="Pack"+i;
			this.packs[i].AddComponent<NewPackHomePageController>();
			this.packs[i].transform.GetComponent<NewPackHomePageController>().setId(i);
			this.packs[i].SetActive(false);
		}

		float competitionsBoardLeftMargin = (this.worldWidth - 2.9f - 3.25f) / 2f + 2.9f;
		float competitionsBoardRightMargin = 3.25f;
		float competitionsBoardUpMargin = 5f;
		float competitionsBoardDownMargin = 0.25f;
		
		float competitionsBoardHeight = worldHeight - competitionsBoardUpMargin-competitionsBoardDownMargin;
		float competitionsBoardWidth = worldWidth-competitionsBoardLeftMargin-competitionsBoardRightMargin;
		Vector2 competitionsBoardOrigin = new Vector3 (-worldWidth/2f+competitionsBoardLeftMargin+competitionsBoardWidth/2f, -worldHeight / 2f + competitionsBoardDownMargin + competitionsBoardHeight / 2,0f);
		
		this.competitionsBoard.GetComponent<BoardController> ().resize(competitionsBoardWidth,competitionsBoardHeight,competitionsBoardOrigin);
		this.competitionsTitle.transform.position = new Vector3 (competitionsBoardOrigin.x, competitionsBoardOrigin.y+competitionsBoardHeight/2f-0.3f, 0);

		float competitionsScale = 0.84f;
		
		float competitionsWidth = 250f;
		float competitionsWorldWidth = (competitionsWidth / pixelPerUnit) * competitionsScale;
		
		this.competitionsPerLine = Mathf.FloorToInt ((competitionsBoardWidth-0.5f) / competitionsWorldWidth);
		
		float competitionsGapWidth = (competitionsBoardWidth - (this.competitionsPerLine * competitionsWorldWidth)) / (this.competitionsPerLine + 1);
		float competitionsBoardStartX = competitionsBoardOrigin.x - competitionsBoardWidth / 2f-competitionsWorldWidth/2f;
		
		this.competitions=new GameObject[this.competitionsPerLine];
		
		for(int i =0;i<this.competitionsPerLine;i++)
		{
			this.competitions[i] = Instantiate(this.competitionObject) as GameObject;
			this.competitions[i].transform.localScale= new Vector3(0.7f,0.7f,0.7f);
			this.competitions[i].transform.position=new Vector3(competitionsBoardStartX+(i+1)*(competitionsGapWidth+packWorldWidth),competitionsBoardOrigin.y-0.5f,0f);
			this.competitions[i].transform.name="Competition"+i;
			this.competitions[i].transform.GetComponent<CompetitionController>().setId(i);
			this.competitions[i].SetActive(false);
		}

		float newsBoardLeftMargin = this.worldWidth-3.25f;
		float newsBoardRightMargin = 0f;
		float newsBoardUpMargin = 4f;
		float newsBoardDownMargin = 0.25f;
		
		float newsBoardHeight = worldHeight - newsBoardUpMargin-newsBoardDownMargin;
		float newsBoardWidth = worldWidth-newsBoardLeftMargin-newsBoardRightMargin;
		Vector2 newsBoardOrigin = new Vector3 (-worldWidth/2f+newsBoardLeftMargin+newsBoardWidth/2f, -worldHeight / 2f + newsBoardDownMargin + newsBoardHeight / 2,0f);
		
		this.newsBoard.GetComponent<BoardController> ().resize(newsBoardWidth,newsBoardHeight,newsBoardOrigin);
		this.newsTitle.transform.position = new Vector3 (newsBoardOrigin.x, newsBoardOrigin.y+newsBoardHeight/2f-0.3f, 0);

		float notificationsBoardLeftMargin = this.worldWidth-3.25f;
		float notificationsBoardRightMargin = 0f;
		float notificationsBoardUpMargin = 0.6f;
		float notificationsBoardDownMargin = 6f;
		
		float notificationsBoardHeight = worldHeight - notificationsBoardUpMargin-notificationsBoardDownMargin;
		float notificationsBoardWidth = worldWidth-notificationsBoardLeftMargin-notificationsBoardRightMargin;
		Vector2 notificationsBoardOrigin = new Vector3 (-worldWidth/2f+notificationsBoardLeftMargin+notificationsBoardWidth/2f, -worldHeight / 2f + notificationsBoardDownMargin + notificationsBoardHeight / 2,0f);
		
		this.notificationsBoard.GetComponent<BoardController> ().resize(notificationsBoardWidth,notificationsBoardHeight,notificationsBoardOrigin);
		this.notificationsTitle.transform.position = new Vector3 (notificationsBoardOrigin.x, notificationsBoardOrigin.y+notificationsBoardHeight/2f-0.3f, 0);

		for(int i=0;i<this.notifications.Length;i++)
		{
			this.notifications[i].transform.position=new Vector3(notificationsBoardOrigin.x-0.25f,notificationsBoardOrigin.y+notificationsBoardHeight/2f-0.85f-i*0.77f,0);
		}

		for(int i=0;i<this.news.Length;i++)
		{
			this.news[i].transform.position=new Vector3(newsBoardOrigin.x-0.25f,newsBoardOrigin.y+newsBoardHeight/2f-0.85f-i*0.77f,0);
		}
	}

	public void drawNotifications()
	{
		this.notificationsDisplayed = new List<int> ();
		int tempInt = this.elementsPerPageNotifications;
		if(this.chosenPageNotifications*(elementsPerPageNotifications)+elementsPerPageNotifications>this.model.notifications.Count)
		{
			tempInt=model.notifications.Count-this.chosenPageNotifications*(elementsPerPageNotifications);
		}
		this.notificationsPictures = new Texture2D[tempInt];
		for(int i =0;i<elementsPerPageNotifications;i++)
		{
			if(this.chosenPageNotifications*this.elementsPerPageNotifications+i<model.notifications.Count)
			{
				this.notificationsDisplayed.Add (this.chosenPageNotifications*this.elementsPerPageNotifications+i);
				this.notifications[i].GetComponent<NotificationController>().setNotification(model.notifications[this.chosenPageNotifications*this.elementsPerPageNotifications+i]);
				this.notifications[i].GetComponent<NotificationController>().show();
				this.notifications[i].SetActive(true);
				if(!model.notifications[this.chosenPageNotifications*this.elementsPerPageNotifications+i].SendingUser.isThumbPictureLoaded)
				{
					StartCoroutine(model.notifications[this.chosenPageNotifications*this.elementsPerPageNotifications+i].SendingUser.setThumbProfilePicture());
				}
				this.notificationsPictures[i]=model.notifications[this.chosenPageNotifications*this.elementsPerPageNotifications+i].SendingUser.texture;
			}
			else
			{
				this.notifications[i].SetActive(false);
			}
		}
		this.areNotificationsPicturesLoading = true;
	}
	public void drawNews()
	{
		this.newsDisplayed = new List<int> ();
		int tempInt = this.elementsPerPageNews;
		if(this.chosenPageNews*(elementsPerPageNews)+elementsPerPageNews>this.model.news.Count)
		{
			tempInt=model.news.Count-this.chosenPageNews*(elementsPerPageNews);
		}
		this.newsPictures = new Texture2D[tempInt];
		for(int i =0;i<elementsPerPageNews;i++)
		{
			if(this.chosenPageNews*this.elementsPerPageNews+i<model.news.Count)
			{
				this.newsDisplayed.Add (this.chosenPageNews*this.elementsPerPageNews+i);
				this.news[i].GetComponent<NewsController>().setNews(model.news[this.chosenPageNews*this.elementsPerPageNews+i]);
				this.news[i].GetComponent<NewsController>().show();
				this.news[i].SetActive(true);
				if(!model.news[this.chosenPageNews*this.elementsPerPageNews+i].User.isThumbPictureLoaded)
				{
					StartCoroutine(model.news[this.chosenPageNews*this.elementsPerPageNews+i].User.setThumbProfilePicture());
				}
				this.newsPictures[i]=model.news[this.chosenPageNews*this.elementsPerPageNews+i].User.texture;
			}
			else
			{
				this.news[i].SetActive(false);
			}
		}
		this.areNewsPicturesLoading = true;
	}
	public void drawPacks()
	{
		this.arePacksPicturesLoading = false;
		this.packsDisplayed = new List<int> ();
		int tempInt = this.packsPerLine;
		if(this.chosenPagePacks*(packsPerLine)+packsPerLine>this.model.packs.Count)
		{
			tempInt=model.packs.Count-this.chosenPagePacks*(packsPerLine);
		}
		this.packsPictures = new Texture2D[tempInt];
		
		for(int i=0;i<packsPerLine;i++)
		{
			if(this.chosenPagePacks*(packsPerLine)+i<this.model.packs.Count)
			{
				this.packsDisplayed.Add (this.chosenPagePacks*(packsPerLine)+i);
				this.packs[i].SetActive(true);
				this.packs[i].transform.GetComponent<NewPackHomePageController>().p=model.packs[this.chosenPagePacks*(packsPerLine)+i];
				this.packs[i].transform.GetComponent<NewPackHomePageController>().show();
				this.packs[i].transform.GetComponent<NewPackHomePageController>().setId(i);
				if(!model.packs[this.chosenPagePacks*(packsPerLine)+i].isTextureLoaded)
				{
					StartCoroutine(model.packs[this.chosenPagePacks*(packsPerLine)+i].setPicture());
				}
				this.packsPictures[i]=model.packs[this.chosenPagePacks*(packsPerLine)+i].texture;
			}
			else
			{
				this.packs[i].SetActive(false);
			}
		}
		this.arePacksPicturesLoading = true;
		this.nbPagesPacks = Mathf.CeilToInt((float)model.packs.Count / (float)this.packsPerLine);
	}
	public void drawCompetitions()
	{
		this.areCompetitionsPicturesLoading = false;
		this.competitionsDisplayed = new List<int> ();
		int tempInt = this.competitionsPerLine;
		int tempNbCompetition=1;
		if(model.player.NbGamesCup>0)
		{
			tempNbCompetition=2;
		}
		if(this.chosenPagePacks*(packsPerLine)+packsPerLine>tempNbCompetition)
		{
			tempInt=tempNbCompetition-this.chosenPagePacks*(packsPerLine);
		}
		this.competitionsPictures = new Texture2D[tempInt];
		
		for(int i=0;i<competitionsPerLine;i++)
		{
			if(this.chosenPagePacks*(packsPerLine)+i<tempNbCompetition)
			{
				this.competitionsDisplayed.Add (this.chosenPagePacks*(packsPerLine)+i);
				this.competitions[i].SetActive(true);
				this.competitions[i].transform.GetComponent<CompetitionController>().setId(i);
				if(this.chosenPagePacks*(packsPerLine)==0)
				{
					this.competitions[i].transform.GetComponent<CompetitionController>().show(model.currentDivision.Name);
					if(!model.currentDivision.isTextureLoaded)
					{
						StartCoroutine(model.currentDivision.setPicture());
					}
					this.competitionsPictures[i]=model.currentDivision.texture;
				}
				else if(this.chosenPagePacks*(packsPerLine)==1)
				{
					this.competitions[i].transform.GetComponent<CompetitionController>().show(model.currentCup.Name);
					if(!model.currentCup.isTextureLoaded)
					{
						StartCoroutine(model.currentCup.setPicture());
					}
					this.competitionsPictures[i]=model.currentCup.texture;
				}
			}
			else
			{
				this.competitions[i].SetActive(false);
			}
		}
		this.areCompetitionsPicturesLoading = true;
		this.nbPagesPacks = Mathf.CeilToInt((float)tempNbCompetition / (float)this.competitionsPerLine);
	}
	public void cleanCompetitions()
	{
		for (int i=0;i<this.competitions.Length;i++)
		{
			Destroy (this.competitions[i]);
		}
	}
	public void cleanPacks()
	{
		for (int i=0;i<this.packs.Length;i++)
		{
			Destroy (this.packs[i]);
		}
	}
	private void drawPaginationNotifications()
	{
		for(int i=0;i<this.paginationButtonsNotifications.Length;i++)
		{
			Destroy (this.paginationButtonsNotifications[i]);
		}
		this.paginationButtonsNotifications = new GameObject[0];
		this.activePaginationButtonIdNotifications = -1;
		float paginationButtonWidth = 0.34f;
		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
		this.nbPagesNotifications = Mathf.CeilToInt((float)model.notifications.Count / ((float)this.elementsPerPageNotifications));
		if(this.nbPagesNotifications>1)
		{
			this.nbPaginationButtonsLimitNotifications = Mathf.CeilToInt((3.25f)/(paginationButtonWidth+gapBetweenPaginationButton));
			int nbButtonsToDraw=0;
			bool drawBackButton=false;
			if (this.pageDebutNotifications !=0)
			{
				drawBackButton=true;
			}
			bool drawNextButton=false;
			if (this.pageDebutNotifications+nbPaginationButtonsLimitNotifications-System.Convert.ToInt32(drawBackButton)<this.nbPagesNotifications-1)
			{
				drawNextButton=true;
				nbButtonsToDraw=nbPaginationButtonsLimitNotifications;
			}
			else
			{
				nbButtonsToDraw=this.nbPagesNotifications-this.pageDebutNotifications;
			}
			this.paginationButtonsNotifications = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsNotifications[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsNotifications[i].AddComponent<HomePageNotificationsPaginationController>();
				this.paginationButtonsNotifications[i].transform.position=new Vector3(this.worldWidth/2f-3.25f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),1.3f,0f);
				this.paginationButtonsNotifications[i].name="PaginationNotification"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtonsNotifications[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutNotifications+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtonsNotifications[i].GetComponent<HomePageNotificationsPaginationController>().setId(i);
				if(this.pageDebutNotifications+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageNotifications)
				{
					this.paginationButtonsNotifications[i].GetComponent<HomePageNotificationsPaginationController>().setActive(true);
					this.activePaginationButtonIdNotifications=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtonsNotifications[0].GetComponent<HomePageNotificationsPaginationController>().setId(-2);
				this.paginationButtonsNotifications[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtonsNotifications[nbButtonsToDraw-1].GetComponent<HomePageNotificationsPaginationController>().setId(-1);
				this.paginationButtonsNotifications[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
		}
	}
	public void paginationHandlerNotifications(int id)
	{
		if(id==-2)
		{
			this.pageDebutNotifications=this.pageDebutNotifications-this.nbPaginationButtonsLimitNotifications+1+System.Convert.ToInt32(this.pageDebutNotifications-this.nbPaginationButtonsLimitNotifications+1!=0);
			this.drawPaginationNotifications();
		}
		else if(id==-1)
		{
			this.pageDebutNotifications=this.pageDebutNotifications+this.nbPaginationButtonsLimitNotifications-1-System.Convert.ToInt32(this.pageDebutNotifications!=0);
			this.drawPaginationNotifications();
		}
		else
		{
			if(activePaginationButtonIdNotifications!=-1)
			{
				this.paginationButtonsNotifications[this.activePaginationButtonIdNotifications].GetComponent<HomePageNotificationsPaginationController>().setActive(false);
			}
			this.activePaginationButtonIdNotifications=id;
			this.chosenPageNotifications=this.pageDebutNotifications-System.Convert.ToInt32(this.pageDebutNotifications!=0)+id;
			this.drawNotifications();
		}
	}
	private void drawPaginationNews()
	{
		for(int i=0;i<this.paginationButtonsNews.Length;i++)
		{
			Destroy (this.paginationButtonsNews[i]);
		}
		this.paginationButtonsNews = new GameObject[0];
		this.activePaginationButtonIdNews = -1;
		float paginationButtonWidth = 0.34f;
		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
		this.nbPagesNews = Mathf.CeilToInt((float)model.news.Count / ((float)this.elementsPerPageNews));
		if(this.nbPagesNews>1)
		{
			this.nbPaginationButtonsLimitNews = Mathf.CeilToInt((3.25f)/(paginationButtonWidth+gapBetweenPaginationButton));
			int nbButtonsToDraw=0;
			bool drawBackButton=false;
			if (this.pageDebutNews !=0)
			{
				drawBackButton=true;
			}
			bool drawNextButton=false;
			if (this.pageDebutNews+nbPaginationButtonsLimitNews-System.Convert.ToInt32(drawBackButton)<this.nbPagesNews-1)
			{
				drawNextButton=true;
				nbButtonsToDraw=nbPaginationButtonsLimitNews;
			}
			else
			{
				nbButtonsToDraw=this.nbPagesNews-this.pageDebutNews;
			}
			this.paginationButtonsNews = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsNews[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsNews[i].AddComponent<HomePageNewsPaginationController>();
				this.paginationButtonsNews[i].transform.position=new Vector3(this.worldWidth/2f-3.25f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),0.55f,0f);
				this.paginationButtonsNews[i].name="PaginationNews"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtonsNews[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutNews+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtonsNews[i].GetComponent<HomePageNewsPaginationController>().setId(i);
				if(this.pageDebutNews+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageNews)
				{
					this.paginationButtonsNews[i].GetComponent<HomePageNewsPaginationController>().setActive(true);
					this.activePaginationButtonIdNews=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtonsNews[0].GetComponent<HomePageNewsPaginationController>().setId(-2);
				this.paginationButtonsNews[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtonsNews[nbButtonsToDraw-1].GetComponent<HomePageNewsPaginationController>().setId(-1);
				this.paginationButtonsNews[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
		}
	}
	public void paginationHandlerNews(int id)
	{
		if(id==-2)
		{
			this.pageDebutNews=this.pageDebutNews-this.nbPaginationButtonsLimitNews+1+System.Convert.ToInt32(this.pageDebutNews-this.nbPaginationButtonsLimitNews+1!=0);
			this.drawPaginationNews();
		}
		else if(id==-1)
		{
			this.pageDebutNews=this.pageDebutNews+this.nbPaginationButtonsLimitNews-1-System.Convert.ToInt32(this.pageDebutNews!=0);
			this.drawPaginationNews();
		}
		else
		{
			if(activePaginationButtonIdNews!=-1)
			{
				this.paginationButtonsNews[this.activePaginationButtonIdNews].GetComponent<HomePageNewsPaginationController>().setActive(false);
			}
			this.activePaginationButtonIdNews=id;
			this.chosenPageNews=this.pageDebutNews-System.Convert.ToInt32(this.pageDebutNews!=0)+id;
			this.drawNews();
		}
	}
	public void buyPackHandler(int id)
	{
		ApplicationModel.packToBuy = model.packs [this.packsDisplayed[id]].Id;
		Application.LoadLevel ("NewStore");
	}
}