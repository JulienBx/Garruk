using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class NewLobbyController : MonoBehaviour
{
	public static NewLobbyController instance;
	private NewLobbyModel model;
	
	public GameObject loadingScreenObject;
	public GameObject tutorialObject;
	public GameObject blockObject;
	public GameObject paginationButtonObject;
	
	private GameObject mainBlock;
	private GameObject mainBlockTitle;
	private GameObject competitionBlock;
	private GameObject competitionBlockTitle;
	private GameObject lastResultsBlock;
	private GameObject lastResultsBlockTitle;
	private GameObject statsBlock;
	private GameObject statsBlockTitle;
	private GameObject popUp;
	private GameObject profilePopUp;
	private GameObject loadingScreen;
	private GameObject menu;
	private GameObject tutorial;
	private GameObject[] results;
	private GameObject stats;
	private GameObject[] paginationButtons;
	private GameObject transparentBackground;
	private GameObject competition;
	
	private int widthScreen;
	private int heightScreen;
	private float worldWidth;
	private float worldHeight;
	private float pixelPerUnit;
	
	private IList<int> resultsDisplayed;
	
	private int nbPages;
	private int nbPaginationButtonsLimit;
	private int elementsPerPage;
	private int chosenPage;
	private int pageDebut;
	private int activePaginationButtonId;

	private bool isSceneLoaded;

	private bool isHoveringResult;
	private bool isHoveringPopUp;
	private bool isProfilePopUpDisplayed;
	private int idResultHovered;
	private bool toDestroyPopUp;
	private float popUpDestroyInterval;

	private bool areResultsPicturesLoading;
	private bool isCompetitionPictureLoading;

	private bool isTutorialLaunched;
	private bool isPopUpDisplayed;
	private bool isLoadingScreenDisplayed;

	private bool isDivisionLobby;
	private bool isEndGameLobby;
	private bool hasWonLastGame;
	
	void Update()
	{	

		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			this.resize();
		}
		if(toDestroyPopUp)
		{
			this.popUpDestroyInterval=this.popUpDestroyInterval+Time.deltaTime;
			if(this.popUpDestroyInterval>0.5f)
			{
				this.toDestroyPopUp=false;
				this.hideProfilePopUp();
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
		if(this.areResultsPicturesLoading)
		{
			bool allPicturesLoaded=true;
			for(int i=0;i<this.resultsDisplayed.Count;i++)
			{
				if(!model.lastResults[this.resultsDisplayed[i]].Opponent.isThumbPictureLoaded)
				{
					allPicturesLoaded=false;
					break;
				}
			}
			if(allPicturesLoaded)
			{
				this.areResultsPicturesLoading=false;
				for(int i=0;i<resultsDisplayed.Count;i++)
				{
					this.results[i].GetComponent<ResultController>().setPicture(model.lastResults[this.resultsDisplayed[i]].Opponent.texture);
				}
			}
		}
		if(this.isCompetitionPictureLoading)
		{
			if(this.isDivisionLobby)
			{
				if(!model.currentDivision.isTextureLoaded)
				{
					this.competition.GetComponent<CompetitionInfosController>().setPicture(model.currentDivision.texture);
				}
			}
			else
			{
				if(!model.currentCup.isTextureLoaded)
				{
					this.competition.GetComponent<CompetitionInfosController>().setPicture(model.currentCup.texture);
				}
			}

		}
	}
	void Awake()
	{
		this.displayLoadingScreen ();
		if(ApplicationModel.gameType==1)
		{
			this.isDivisionLobby=true;
		}
		if(ApplicationModel.launchEndGameSequence)
		{
			this.isEndGameLobby=true;
			ApplicationModel.launchEndGameSequence=false;
		}
		if(ApplicationModel.hasWonLastGame)
		{
			this.hasWonLastGame=true;
			ApplicationModel.hasWonLastGame=false;
		}
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.pixelPerUnit = 108f;
		this.elementsPerPage = 5;
		this.initializeScene ();
	}
	void Start()
	{
		instance = this;
		this.model = new NewLobbyModel ();
		this.resize ();
		StartCoroutine (this.initialization ());
	}
	private IEnumerator initialization()
	{
		yield return StartCoroutine(model.getLobbyData(this.isDivisionLobby,this.isEndGameLobby));
		this.initializeResults ();
		this.initializeCompetitions ();
		this.initializeStats ();
		this.isSceneLoaded = true;
		this.hideLoadingScreen ();
	}
	private void initializeResults()
	{
		this.chosenPage = 0;
		this.pageDebut = 0 ;
		this.drawPagination();
		this.drawResults ();
	}
	private void initializeCompetitions()
	{
		//this.drawCompetition ();
	}
	private void initializeStats()
	{
		this.drawStats ();
	}
	public void initializeScene()
	{
		menu = GameObject.Find ("newMenu");
		menu.GetComponent<newMenuController> ().setCurrentPage (0);
		this.mainBlock = Instantiate(this.blockObject) as GameObject;
		this.mainBlockTitle = GameObject.Find ("MainBlockTitle");
		this.statsBlock = Instantiate(this.blockObject) as GameObject;
		this.statsBlockTitle = GameObject.Find ("StatsBlockTitle");
		this.lastResultsBlock = Instantiate(this.blockObject) as GameObject;
		this.lastResultsBlockTitle = GameObject.Find ("LastResultsBlockTitle");
		this.competitionBlock = Instantiate(this.blockObject) as GameObject;
		this.competitionBlockTitle = GameObject.Find ("CompetitionBlockTitle");
		this.paginationButtons = new GameObject[0];
		this.results=new GameObject[5];
		for(int i=0;i<this.results.Length;i++)
		{
			this.results[i]=GameObject.Find ("Result"+i);
			this.results[i].GetComponent<ResultController>().setId(i);
			this.results[i].SetActive(false);
		}
		this.transparentBackground = GameObject.Find ("TransparentBackGround");
		this.transparentBackground.SetActive (false);
		this.popUp = GameObject.Find ("PopUp");
		this.popUp.SetActive (false);
		this.profilePopUp = GameObject.Find ("ProfilePopUp");
		this.profilePopUp.SetActive (false);
		this.stats = GameObject.Find ("Stats");
		
	}
	public void resize()
	{
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		float screenRatio = (float)this.widthScreen / (float)this.heightScreen;
		menu.GetComponent<newMenuController> ().resizeMeunObject (worldHeight,worldWidth);
		
		float mainBlockLeftMargin =3f;
		float mainBlockRightMargin = 3f;
		float mainBlockUpMargin = 0.2f;
		float mainBlockDownMargin = 4f;
		
		float mainBlockHeight = worldHeight - mainBlockUpMargin-mainBlockDownMargin;
		float mainBlockWidth = worldWidth-mainBlockLeftMargin-mainBlockRightMargin;
		Vector2 mainBlockOrigin = new Vector3 (-worldWidth/2f+mainBlockLeftMargin+mainBlockWidth/2f, -worldHeight / 2f + mainBlockDownMargin + mainBlockHeight / 2,0f);
		
		this.mainBlock.GetComponent<BlockController> ().resize(new Rect(mainBlockOrigin.x,mainBlockOrigin.y,mainBlockWidth,mainBlockHeight));
		this.mainBlockTitle.transform.position = new Vector3 (mainBlockOrigin.x, mainBlockOrigin.y+mainBlockHeight/2f-0.3f, 0);

		float competitionBlockLeftMargin =this.worldWidth-2.8f;
		float competitionBlockRightMargin = 0f;
		float competitionBlockUpMargin = 4.1f;
		float competitionBlockDownMargin = 0.2f;
		
		float competitionBlockHeight = worldHeight - competitionBlockUpMargin-competitionBlockDownMargin;
		float competitionBlockWidth = worldWidth-competitionBlockLeftMargin-competitionBlockRightMargin;
		Vector2 competitionBlockOrigin = new Vector3 (-worldWidth/2f+competitionBlockLeftMargin+competitionBlockWidth/2f, -worldHeight / 2f + competitionBlockDownMargin + competitionBlockHeight / 2,0f);
		
		this.competitionBlock.GetComponent<BlockController> ().resize(new Rect(competitionBlockOrigin.x,competitionBlockOrigin.y,competitionBlockWidth,competitionBlockHeight));
		this.competitionBlockTitle.transform.position = new Vector3 (competitionBlockOrigin.x, competitionBlockOrigin.y+competitionBlockHeight/2f-0.3f, 0);

		float statsBlockLeftMargin =3f;
		float statsBlockRightMargin = 3f;
		float statsBlockUpMargin = 6.2f;
		float statsBlockDownMargin = 0.2f;
		
		float statsBlockHeight = worldHeight - statsBlockUpMargin-statsBlockDownMargin;
		float statsBlockWidth = worldWidth-statsBlockLeftMargin-statsBlockRightMargin;
		Vector2 statsBlockOrigin = new Vector3 (-worldWidth/2f+statsBlockLeftMargin+statsBlockWidth/2f, -worldHeight / 2f + statsBlockDownMargin + statsBlockHeight / 2,0f);
		
		this.statsBlock.GetComponent<BlockController> ().resize(new Rect(statsBlockOrigin.x,statsBlockOrigin.y,statsBlockWidth,statsBlockHeight));
		this.statsBlockTitle.transform.position = new Vector3 (statsBlockOrigin.x, statsBlockOrigin.y+statsBlockHeight/2f-0.3f, 0);
		this.stats.transform.position = new Vector3 (statsBlockOrigin.x, statsBlockOrigin.y, 0);
		this.stats.transform.FindChild ("nbWins").localPosition = new Vector3 (-1.5f * statsBlockWidth / 5f, 0f, 0);
		this.stats.transform.FindChild ("nbLooses").localPosition = new Vector3 (-0.5f * statsBlockWidth / 5f, 0f, 0);
		this.stats.transform.FindChild ("ranking").localPosition = new Vector3 (0.5f * statsBlockWidth / 5f, 0f, 0);
		this.stats.transform.FindChild ("collectionPoints").localPosition = new Vector3 (1.5f * statsBlockWidth / 5f, 0f, 0);

		float lastResultsBlockLeftMargin =this.worldWidth-2.8f;
		float lastResultsBlockRightMargin = 0f;
		float lastResultsBlockUpMargin = 0.6f;
		float lastResultsBlockDownMargin = 6.1f;
		
		float lastResultsBlockHeight = worldHeight - lastResultsBlockUpMargin-lastResultsBlockDownMargin;
		float lastResultsBlockWidth = worldWidth-lastResultsBlockLeftMargin-lastResultsBlockRightMargin;
		Vector2 lastResultsBlockOrigin = new Vector3 (-worldWidth/2f+lastResultsBlockLeftMargin+lastResultsBlockWidth/2f, -worldHeight / 2f + lastResultsBlockDownMargin + lastResultsBlockHeight / 2,0f);
		
		this.lastResultsBlock.GetComponent<BlockController> ().resize(new Rect(lastResultsBlockOrigin.x,lastResultsBlockOrigin.y,lastResultsBlockWidth,lastResultsBlockHeight));
		this.mainBlockTitle.transform.position = new Vector3 (lastResultsBlockOrigin.x, lastResultsBlockOrigin.y+lastResultsBlockHeight/2f-0.3f, 0);

		
		if(this.isTutorialLaunched)
		{
			this.tutorial.GetComponent<TutorialObjectController>().resize();
		}
		
		this.transparentBackground.transform.position = new Vector3 (0, 0, -2f);
		this.popUp.transform.position = new Vector3 (0, 2f, -3f);
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
	public void drawResults()
	{
		this.resultsDisplayed = new List<int> ();
		int tempInt = this.elementsPerPage;
		if(this.chosenPage*(elementsPerPage)+elementsPerPage>this.model.lastResults.Count)
		{
			tempInt=model.lastResults.Count-this.chosenPage*(elementsPerPage);
		}
		bool allPicturesLoaded = true;
		for(int i =0;i<elementsPerPage;i++)
		{
			if(this.chosenPage*this.elementsPerPage+i<model.lastResults.Count)
			{
				if(!model.lastResults[this.chosenPage*this.elementsPerPage+i].Opponent.isThumbPictureLoaded)
				{
					StartCoroutine(model.lastResults[this.chosenPage*this.elementsPerPage+i].Opponent.setThumbProfilePicture());
					allPicturesLoaded=false;
				}
				this.resultsDisplayed.Add (this.chosenPage*this.elementsPerPage+i);
				this.results[i].GetComponent<ResultController>().setResult(model.lastResults[this.chosenPage*this.elementsPerPage+i]);
				this.results[i].GetComponent<ResultController>().show();
				this.results[i].SetActive(true);
			}
			else
			{
				this.results[i].SetActive(false);
			}
		}
		if(!allPicturesLoaded)
		{
			this.areResultsPicturesLoading=true;
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
		this.stats.transform.FindChild ("collectionPoints").FindChild ("Value").GetComponent<TextMeshPro> ().text = model.player.Ranking.ToString ();
		this.stats.transform.FindChild ("collectionPoints").FindChild ("Title2").GetComponent<TextMeshPro> ().text = "("+model.player.CollectionPoints.ToString()+" pts)";
	}
	private void drawPagination()
	{
		for(int i=0;i<this.paginationButtons.Length;i++)
		{
			Destroy (this.paginationButtons[i]);
		}
		this.paginationButtons = new GameObject[0];
		this.activePaginationButtonId = -1;
		float paginationButtonWidth = 0.34f;
		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
		this.nbPages = Mathf.CeilToInt((float)model.lastResults.Count / ((float)this.elementsPerPage));
		if(this.nbPages>1)
		{
			this.nbPaginationButtonsLimit = Mathf.CeilToInt((2.9f)/(paginationButtonWidth+gapBetweenPaginationButton));
			int nbButtonsToDraw=0;
			bool drawBackButton=false;
			if (this.pageDebut !=0)
			{
				drawBackButton=true;
			}
			bool drawNextButton=false;
			if (this.pageDebut+nbPaginationButtonsLimit-System.Convert.ToInt32(drawBackButton)<this.nbPages-1)
			{
				drawNextButton=true;
				nbButtonsToDraw=nbPaginationButtonsLimit;
			}
			else
			{
				nbButtonsToDraw=this.nbPages-this.pageDebut;
			}
			this.paginationButtons = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtons[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtons[i].AddComponent<LobbyPaginationController>();
				this.paginationButtons[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
				this.paginationButtons[i].name="Pagination"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtons[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebut+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtons[i].GetComponent<LobbyPaginationController>().setId(i);
				if(this.pageDebut+i-System.Convert.ToInt32(drawBackButton)==this.chosenPage)
				{
					this.paginationButtons[i].GetComponent<LobbyPaginationController>().setActive(true);
					this.activePaginationButtonId=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtons[0].GetComponent<LobbyPaginationController>().setId(-2);
				this.paginationButtons[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtons[nbButtonsToDraw-1].GetComponent<LobbyPaginationController>().setId(-1);
				this.paginationButtons[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
		}
	}
	public void paginationHandler(int id)
	{
		if(id==-2)
		{
			this.pageDebut=this.pageDebut-this.nbPaginationButtonsLimit+1+System.Convert.ToInt32(this.pageDebut-this.nbPaginationButtonsLimit+1!=0);
			this.drawPagination();
		}
		else if(id==-1)
		{
			this.pageDebut=this.pageDebut+this.nbPaginationButtonsLimit-1-System.Convert.ToInt32(this.pageDebut!=0);
			this.drawPagination();
		}
		else
		{
			if(activePaginationButtonId!=-1)
			{
				this.paginationButtons[this.activePaginationButtonId].GetComponent<LobbyPaginationController>().setActive(false);
			}
			this.activePaginationButtonId=id;
			this.chosenPage=this.pageDebut-System.Convert.ToInt32(this.pageDebut!=0)+id;
			this.drawResults();
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
	public void startHoveringResult (int id)
	{
		this.idResultHovered=id;
		this.isHoveringResult = true;
		if(this.isProfilePopUpDisplayed && this.profilePopUp.GetComponent<PopUpController>().getIsNotification())
		{
			if(this.profilePopUp.GetComponent<PopUpResultLobbyController>().getId()!=this.idResultHovered);
			{
				this.hideProfilePopUp();
				this.showPopUpResult();
			}
		}
		else
		{
			if(this.isProfilePopUpDisplayed)
			{
				this.hideProfilePopUp();
			}
			this.showPopUpResult();
		}
	}
	public void endHoveringResult ()
	{
		this.isHoveringResult = false;
		this.toDestroyPopUp = true;
		this.popUpDestroyInterval = 0f;
	}
	public void showPopUpResult()
	{
		this.profilePopUp.SetActive (true);
		this.profilePopUp.transform.position=new Vector3(this.results[this.idResultHovered].transform.position.x-3.1f,this.results[this.idResultHovered].transform.position.y,-1f);
		this.profilePopUp.AddComponent<PopUpResultLobbyController>();
		this.profilePopUp.GetComponent<PopUpResultLobbyController> ().setIsNotification (true);
		this.profilePopUp.GetComponent<PopUpResultLobbyController> ().setId (this.idResultHovered);
		this.profilePopUp.GetComponent<PopUpResultLobbyController> ().show (model.lastResults [this.resultsDisplayed [this.idResultHovered]]);
		this.isPopUpDisplayed=true;
	}
	public void hideProfilePopUp()
	{
		this.toDestroyPopUp = false;
		this.popUpDestroyInterval = 0f;
		this.profilePopUp.SetActive (false);
		this.isPopUpDisplayed=false;
	}
	private void displayPopUp()
	{
		this.isPopUpDisplayed = true;
		this.popUp.SetActive (true);
		this.transparentBackground.SetActive (true);
	}
	public void hidePopUp()
	{
		this.isPopUpDisplayed = false;
		this.popUp.SetActive (false);
		this.transparentBackground.SetActive (false);
	}
	public void displayLoadingScreen()
	{
		if(!isLoadingScreenDisplayed)
		{
			this.loadingScreen=Instantiate(this.loadingScreenObject) as GameObject;
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
	}
}