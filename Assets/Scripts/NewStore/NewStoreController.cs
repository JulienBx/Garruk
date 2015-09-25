using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;

public class NewStoreController : MonoBehaviour
{
	public static NewStoreController instance;

	public GameObject loadingScreenObject;
	public GameObject CardObject;
	public GameObject PackObject;
	public GameObject BlockObject;
	public GameObject paginationButtonObject;
	public GameObject tutorialObject;
	public GUISkin popUpSkin;

	private GameObject loadingScreen;
	private GameObject menu;
	private GameObject tutorial;
	private GameObject focusedCard;
	private GameObject[] paginationButtons;
	private GameObject[] randomCards;
	private bool[] randomCardsDisplayed;
	private bool isCardFocusedDisplayed;
	private bool areRandomCardsGenerated;
	private GameObject[] packs;
	private GameObject[] packsBlocks;
	private GameObject cardFocused;
	private GameObject addCreditsButton;
	private GameObject backButton;
	private GameObject buyCreditsButton;
	private GameObject homePageBoughtPack;

	private IList<int> packsDisplayed;

	//public GameObject TutorialObject;

	private NewStoreModel model;
	private NewStoreErrorPopUpView errorView;
	private bool isErrorViewDisplayed;
	private NewStoreAddCreditsPopUpView addCreditsView;
	private bool isAddCreditsViewDisplayed;

	//private bool isTutorialLaunched;
	//private GameObject tutorial;

	private int nbPages;
	private int nbPaginationButtonsLimit;
	private int chosenPage;
	private int pageDebut;
	private int activePaginationButtonId;

	private float worldHeight;
	private float worldWidth;
	private float packsBoardLeftMargin;
	private float packsBoardRightMargin;
	private int widthScreen;
	private int heightScreen;
	private int pixelPerUnit;

	private int packsPerLine;

	private Rect centralWindow;
	private Rect collectionPointsWindow;
	private Rect newSkillsWindow;
	private Rect newCardTypeWindow;
	private Rect selectCardTypeWindow;

	private bool arePacksPicturesLoading;

	private int selectedPackIndex;
	private int clickedCardId;

	private bool[] toRotate;
	private bool drawFace;
	private bool startRotation;
	private float speed;
	private float angle;
	private Quaternion target;

	private bool isSceneLoaded;
	private int money;

	private bool toUpdatePackPrices;
	private bool isTutorialLaunched;

	private bool toResizeBackUI;

	private bool isLoadingScreenDisplayed;
	
	public NewStoreController ()
	{
	}
	void Awake()
	{
		this.displayLoadingScreen ();
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.pixelPerUnit = 108;
		this.speed = 300.0f;
		this.packsBoardLeftMargin = 2.9f;
		this.packsBoardRightMargin = 0.5f;
		this.initializeScene ();
	}
	void Start()
	{
		instance = this;
		this.model = new NewStoreModel ();
		this.resize ();
		StartCoroutine (this.initialization ());
	}
	void Update () 
	{
		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			if(toResizeBackUI)
			{
				this.toResizeBackUI=false;
			}
			this.resize();
			if(!toResizeBackUI)
			{
				this.createPacks();
			}
		}
		if(money!=ApplicationModel.credits)
		{
			if(isSceneLoaded)
			{
				if(isCardFocusedDisplayed)
				{
					this.focusedCard.GetComponent<NewFocusedCardStoreController>().updateFocusFeatures();
					this.toUpdatePackPrices=true;
				}
				else
				{
					this.updatePackPrices();
				}
			}
			this.money=ApplicationModel.credits;
		}
		if(Input.GetKeyDown(KeyCode.Return)) 
		{
			this.returnPressed();
		}
		if(Input.GetKeyDown(KeyCode.Escape) && !isTutorialLaunched) 
		{
			this.escapePressed();
		}
		if (this.startRotation)
		{
			for(int i=0;i<this.randomCards.Length;i++)
			{
				if(toRotate[i])
				{
					if(this.angle==360)
					{
						this.angle=180;
					}
					
					this.angle = this.angle + this.speed * Time.deltaTime;
					if(this.angle>270 && !this.drawFace)
					{
						this.drawFace=true;
						this.randomCards[i].GetComponent<NewFocusedCardController>().setBackFace(false);
					}
					if(this.angle>=360)
					{
						this.angle=360;
						this.toRotate[i]=false;
						this.drawFace=false;
						if(i<model.packList[this.selectedPackIndex].NbCards-1)
						{
							this.toRotate[i+1]=true;
						}
						else
						{
							this.startRotation=false;
							if(model.packList[this.selectedPackIndex].NbCards==1)
							{
								this.randomCards[i].GetComponent<NewFocusedCardStoreController>().displayFocusFeatures(true);
							}
							if(this.isTutorialLaunched)
							{
								TutorialObjectController.instance.actionIsDone();
							}
						}
					}
					this.target = Quaternion.Euler(0, this.angle, 0);
					this.randomCards[i].transform.rotation = target;
				}
			}
		}
		if(arePacksPicturesLoading)
		{
			bool allPicturesLoaded=true;
			for(int i=0;i<packsDisplayed.Count;i++)
			{
				if(!model.packList[this.packsDisplayed[i]].isTextureLoaded)
				{
					allPicturesLoaded=false;
					break;
				}
			}
			if(allPicturesLoaded)
			{
				this.arePacksPicturesLoading=false;
				for(int i=0;i<packsDisplayed.Count;i++)
				{
					this.packs[i].GetComponent<NewPackController>().setPackPicture(model.packList[this.packsDisplayed[i]].texture);
				}
			}
		}
	}
	private IEnumerator initialization()
	{
		yield return(StartCoroutine(this.model.initializeStore()));
		this.money = ApplicationModel.credits;
		this.createPacks ();
		this.hideLoadingScreen ();
		this.isSceneLoaded = true;
		if(ApplicationModel.packToBuy!=-1)
		{
			int tempId=0;
			for(int i=0;i<model.packList.Count;i++)
			{
				if(model.packList[i].Id==ApplicationModel.packToBuy)
				{
					tempId=i;
					break;
				}
			}
			ApplicationModel.packToBuy = -1;
			this.homePageBoughtPack.GetComponent<NewPackStoreController>().p=model.packList[tempId];
			this.homePageBoughtPack.GetComponent<NewPackStoreController>().setId(tempId);
			this.homePageBoughtPack.GetComponent<NewPackStoreController>().OnMouseDown();
		}
		if(model.player.TutorialStep==5)
		{
			this.tutorial = Instantiate(this.tutorialObject) as GameObject;
			this.tutorial.AddComponent<StoreTutorialController>();
			this.menu.GetComponent<newMenuController>().setTutorialLaunched(true);
			this.tutorial.GetComponent<StoreTutorialController>().launchSequence(0);
			this.isTutorialLaunched=true;
		}
	}
	public void initializeScene()
	{
		menu = GameObject.Find ("newMenu");
		menu.GetComponent<newMenuController> ().setCurrentPage (2);
		this.paginationButtons = new GameObject[0];
		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardStoreController> ();
		this.focusedCard.SetActive (false);
		this.addCreditsButton = GameObject.Find ("AddCreditsButton");
		this.packs=new GameObject[0];
		this.packsBlocks = new GameObject[0];
		this.backButton = GameObject.Find ("BackButton");
		this.backButton.SetActive (false);
		this.buyCreditsButton = GameObject.Find ("BuyCreditsButton");
		this.buyCreditsButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "Acheter des cristaux";
		this.buyCreditsButton.SetActive (true);
		this.homePageBoughtPack = GameObject.Find ("HomePageBoughtPack");
	}
	public void resize()
	{
		if(!toResizeBackUI)
		{
			this.resizeMainParameters();
			this.resizeRandomCardsUI();
			this.resizeFocusedCard();
		}
		if(this.isCardFocusedDisplayed || this.areRandomCardsGenerated)
		{
			toResizeBackUI=true;
			if(this.isCardFocusedDisplayed)
			{
				this.focusedCard.GetComponent<NewFocusedCardController>().resize();
			}
			if(this.areRandomCardsGenerated)
			{
				this.resizeRandomCards();
			}
		}
		else 
		{
			this.resizeBackUI();
		}
		if(this.isTutorialLaunched)
		{
			this.tutorial.GetComponent<TutorialObjectController>().resize();
		}
	}
	public void resizeMainParameters()
	{
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.40f * this.heightScreen);
		this.selectCardTypeWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.50f * this.heightScreen);
		this.collectionPointsWindow=new Rect(this.widthScreen - this.widthScreen * 0.17f-5,0.1f * this.heightScreen+5,this.widthScreen * 0.17f,this.heightScreen * 0.1f);
		this.newSkillsWindow = new Rect (this.collectionPointsWindow.xMin, this.collectionPointsWindow.yMax + 5,this.collectionPointsWindow.width,this.heightScreen - 0.1f * this.heightScreen - 2 * 5 - this.collectionPointsWindow.height);
		this.newCardTypeWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		
		menu.GetComponent<newMenuController> ().resizeMeunObject (worldHeight,worldWidth);
	}
	public void resizeFocusedCard()
	{
		float focusedCardScale = 3.648985f;
		float focusedCardWidth = 194f;
		float focusedCardHeight = 271f;
		float focusedCardRightMargin = 0.5f;
		float focusedCardLeftMargin = 2.8f;
		float emptyWidth = this.worldWidth - focusedCardRightMargin - focusedCardLeftMargin;
		
		this.focusedCard.transform.position = new Vector3 (focusedCardLeftMargin+emptyWidth/2f-this.worldWidth/2f, -0.25f, 0f);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setCentralWindow (this.centralWindow);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setCollectionPointsWindow (this.collectionPointsWindow);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setNewSkillsWindow (this.newSkillsWindow);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setNewCardTypeWindow (this.newCardTypeWindow);
		//this.focusedCard.SetActive (false);
	}
	public void resizeRandomCardsUI()
	{
		float backButtonLeftMargin = 2.8f;
		float backButtonRightMargin = 0.5f;
		this.backButton.transform.position = new Vector3 ((backButtonLeftMargin - backButtonRightMargin) / 2f, -4f, 0);
	}
	public void resizeBackUI()
	{
		this.cleanPacks ();
		float buyCreditsButtonLeftMargin = 2.8f;
		float buyCreditsButtonRightMargin = 0.5f;
		this.buyCreditsButton.transform.position = new Vector3 ((buyCreditsButtonLeftMargin - buyCreditsButtonRightMargin) / 2f, -3.5f, 0);
		
		if(isErrorViewDisplayed)
		{
			this.errorPopUpResize();
		}
		else if(isAddCreditsViewDisplayed)
		{
			this.addCreditsPopUpResize();
		}
	}
	public void resizeRandomCards()
	{
		int nbCards = model.packList [this.selectedPackIndex].NbCards;
		float cardWidth = 720f;
		float cardHeight = 1004f;
		Vector3 scale;
		Vector3 position=new Vector3(0,0,0);
		float width = this.worldWidth-this.packsBoardLeftMargin-this.packsBoardRightMargin;
		float cardWorldWidth = width/(nbCards+1);
		float scaleCard = cardWorldWidth / (cardWidth/this.pixelPerUnit);
		float cardMaxWorldHeight = 6f;
		float cardWorldHeight = cardWorldWidth * (cardWidth / cardHeight);
		if(cardWorldHeight>cardMaxWorldHeight)
		{
			cardWorldWidth=cardMaxWorldHeight*(cardWidth/cardHeight);
			scaleCard=(cardMaxWorldHeight*(cardWidth/cardHeight)*this.pixelPerUnit)/cardWidth;
		}
		float gapBetweenCards = cardWorldWidth / (nbCards + 1);
		for (int i = 0; i <nbCards; i++)
		{
			scale = new Vector3(scaleCard,scaleCard,scaleCard);
			position = new Vector3((this.packsBoardLeftMargin-this.packsBoardRightMargin-width)/2f+gapBetweenCards+cardWorldWidth/2f+(float)i*(gapBetweenCards+cardWorldWidth), 0f, 0f); 
			this.randomCards[i].transform.position=position;
			this.randomCards[i].transform.localScale=scale;
		}
	}
	public void createPacks()
	{
		float packScale = 1.2f;
		float packsBoardUpMargin = 0.5f;
		float packsBoardDownMargin = 2f;
		float packsBoardWidth = worldWidth-packsBoardLeftMargin-packsBoardRightMargin;
		float packsBoardHeight = worldHeight - packsBoardDownMargin - packsBoardUpMargin;
		
		Vector2 packsBoardOrigin = new Vector3 (-worldWidth/2f+packsBoardLeftMargin+packsBoardWidth/2f, -worldHeight / 2f + packsBoardDownMargin + packsBoardHeight / 2,0f);

		float packBlockWorldWidth=3.25f;
		float packBlockWorldHeight = 4.5f;
		
		this.packsPerLine = Mathf.FloorToInt ((packsBoardWidth-0.5f) / packBlockWorldWidth);
		if(this.packsPerLine>model.packList.Count)
		{
			this.packsPerLine=model.packList.Count;
		}
		
		float gapWidth = (packsBoardWidth - (this.packsPerLine * packBlockWorldWidth)) / (this.packsPerLine + 1);
		float cardBoardStartX = packsBoardOrigin.x - packsBoardWidth / 2f-packBlockWorldWidth/2f;
		
		this.packs=new GameObject[this.packsPerLine];
		this.packsBlocks = new GameObject[this.packsPerLine];
		
		for(int i =0;i<this.packsPerLine;i++)
		{
			this.packs[i] = Instantiate(this.PackObject) as GameObject;
			this.packsBlocks[i]=Instantiate(this.BlockObject) as GameObject;
			this.packs[i].transform.localScale= new Vector3(1f,1f,1f);
			this.packs[i].transform.position=new Vector3(cardBoardStartX+(i+1)*(gapWidth+packBlockWorldWidth),packsBoardOrigin.y,0f);
			this.packsBlocks[i].GetComponent<BlockController>().resize(new Rect(cardBoardStartX+(i+1)*(gapWidth+packBlockWorldWidth),packsBoardOrigin.y+0.5f,packBlockWorldWidth,packBlockWorldHeight));
			this.packs[i].transform.name="Pack"+i;
			this.packs[i].AddComponent<NewPackStoreController>();
			this.packs[i].transform.GetComponent<NewPackStoreController>().setId(i);
			this.packs[i].transform.GetComponent<NewPackStoreController>().setCentralWindow (this.selectCardTypeWindow);
			this.packs[i].transform.GetComponent<NewPackStoreController>().setCollectionPointsWindow (this.collectionPointsWindow);
			this.packs[i].transform.GetComponent<NewPackStoreController>().setNewSkillsWindow (this.newSkillsWindow);
			this.packs[i].SetActive(false);
			this.packsBlocks[i].GetComponent<BlockController>().display(false);
		}
		this.homePageBoughtPack.transform.GetComponent<NewPackStoreController>().setCentralWindow (this.selectCardTypeWindow);
		this.homePageBoughtPack.transform.GetComponent<NewPackStoreController>().setCollectionPointsWindow (this.collectionPointsWindow);
		this.homePageBoughtPack.transform.GetComponent<NewPackStoreController>().setNewSkillsWindow (this.newSkillsWindow);
		this.drawPacks ();
		this.updatePackPrices ();
		this.drawPagination ();
	}
	public void cleanPacks()
	{
		for (int i=0;i<this.packs.Length;i++)
		{
			Destroy (this.packs[i]);
		}
		for (int i=0;i<this.packsBlocks.Length;i++)
		{
			this.packsBlocks[i].GetComponent<BlockController>().destroyShadow();
			Destroy (this.packsBlocks[i]);
		}
	}
	public void displayPacks(bool value)
	{
		for (int i=0;i<this.packs.Length;i++)
		{
			this.packs[i].GetComponent<NewPackController>().displayPack(value);
			this.packs[i].GetComponent<BoxCollider2D>().enabled=value;
			this.packsBlocks[i].GetComponent<BlockController>().display(value);
		}
		for(int i=0;i<this.paginationButtons.Length;i++)
		{
			this.paginationButtons[i].SetActive(value);
		}
	}
	public void cleanCards()
	{
		if(this.startRotation)
		{
			this.startRotation=false;
			this.drawFace=false;
		}
		for(int i=0;i<this.randomCards.Length;i++)
		{
			Destroy (this.randomCards[i]);
		}
	}
	public void backToPacks()
	{
		this.areRandomCardsGenerated = false;
		if(this.toResizeBackUI)
		{
			this.resize();
			this.toResizeBackUI=false;
			this.createPacks();
		}
		this.displayPacks (true);
		this.backButton.SetActive (false);
		this.buyCreditsButton.SetActive (true);
		this.cleanCards ();
		if(this.toUpdatePackPrices)
		{
			this.updatePackPrices();
		}
	}
	public void drawPacks()
	{
		this.packsDisplayed = new List<int> ();
		int tempInt = this.packsPerLine;
		if(this.chosenPage*(packsPerLine)+packsPerLine>this.model.packList.Count)
		{
			tempInt=model.packList.Count-this.chosenPage*(packsPerLine);
		}
		bool allPicturesLoaded = true;
		for(int i=0;i<packsPerLine;i++)
		{
			if(this.chosenPage*(packsPerLine)+i<this.model.packList.Count)
			{
				if(!model.packList[this.chosenPage*(packsPerLine)+i].isTextureLoaded)
				{
					StartCoroutine(model.packList[this.chosenPage*(packsPerLine)+i].setPicture());
					allPicturesLoaded=false;
				}
				this.packsDisplayed.Add (this.chosenPage*(packsPerLine)+i);
				this.packs[i].SetActive(true);
				this.packsBlocks[i].GetComponent<BlockController>().display(true);
				this.packs[i].transform.GetComponent<NewPackStoreController>().p=model.packList[this.chosenPage*(packsPerLine)+i];
				this.packs[i].transform.GetComponent<NewPackStoreController>().show();
				this.packs[i].transform.GetComponent<NewPackStoreController>().setId(i);

			}
			else
			{
				this.packs[i].SetActive(false);
				this.packsBlocks[i].GetComponent<BlockController>().display(false);
			}
		}
		if(!allPicturesLoaded)
		{
			this.arePacksPicturesLoading=true;
		}
	}
	public void drawRandomCards(int id)
	{
		if(this.isTutorialLaunched)
		{
			TutorialObjectController.instance.actionIsDone();
		}
		this.displayPacks (false);
		this.selectedPackIndex = id;
		int nbCards = model.packList [this.selectedPackIndex].NbCards;
		this.randomCards = new GameObject[nbCards];
		this.randomCardsDisplayed=new bool[nbCards];
		this.toRotate= new bool[nbCards];
		if(nbCards>1)
		{
			float cardWidth = 720f;
			float cardHeight = 1004f;
			string name;
			Vector3 scale;
			Vector3 position=new Vector3(0,0,0);
			Quaternion rotation;
			float width = this.worldWidth-this.packsBoardLeftMargin-this.packsBoardRightMargin;
			float cardWorldWidth = width/(nbCards+1);
			float scaleCard = cardWorldWidth / (cardWidth/this.pixelPerUnit);
			float cardMaxWorldHeight = 6f;
			float cardWorldHeight = cardWorldWidth * (cardWidth / cardHeight);
			if(cardWorldHeight>cardMaxWorldHeight)
			{
				cardWorldWidth=cardMaxWorldHeight*(cardWidth/cardHeight);
				scaleCard=(cardMaxWorldHeight*(cardWidth/cardHeight)*this.pixelPerUnit)/cardWidth;
			}
			float gapBetweenCards = cardWorldWidth / (nbCards + 1);
			for (int i = 0; i <nbCards; i++)
			{
				name="Card" + i;
				scale = new Vector3(scaleCard,scaleCard,scaleCard);
				position = new Vector3((this.packsBoardLeftMargin-this.packsBoardRightMargin-width)/2f+gapBetweenCards+cardWorldWidth/2f+(float)i*(gapBetweenCards+cardWorldWidth), 0f, 0f); 
				this.toRotate[i]=false;
				this.randomCards [i] = Instantiate(this.CardObject) as GameObject;
				this.randomCards [i].AddComponent<NewCardStoreController>();
				this.randomCards [i].name=name;
				this.randomCards[i].GetComponent<NewCardStoreController>().setId(i);
				this.randomCards[i].GetComponent<NewCardStoreController>().c=model.packList[this.selectedPackIndex+chosenPage*nbPages].Cards[i];
				this.randomCards[i].GetComponent<NewCardStoreController>().show();
				this.randomCards[i].GetComponent<NewCardStoreController>().setBackFace(true);
				this.randomCards[i].transform.rotation=Quaternion.Euler(0, 180, 0);
				this.randomCards[i].transform.position=position;
				this.randomCards[i].transform.localScale=scale;
				this.randomCardsDisplayed[i]=true;
			}
			this.backButton.SetActive(true);
			this.areRandomCardsGenerated=true;
		}
		else
		{
			this.randomCards[0]=this.focusedCard;
			this.randomCardsDisplayed[0]=true;
			this.focusedCard.SetActive(true);
			this.focusedCard.GetComponent<NewFocusedCardStoreController>().displayFocusFeatures(false);
			this.focusedCard.GetComponent<NewFocusedCardStoreController>().c=model.packList[this.selectedPackIndex+chosenPage*nbPages].Cards[0];
			this.focusedCard.GetComponent<NewFocusedCardStoreController>().show ();
			this.focusedCard.GetComponent<NewFocusedCardStoreController>().setBackFace(true);
			this.focusedCard.GetComponent<NewFocusedCardStoreController>().transform.rotation=Quaternion.Euler(0, 180, 0);
			this.isCardFocusedDisplayed=true;
		}
		this.buyCreditsButton.SetActive(false);
		this.startRotation = true;
		this.toRotate [0] = true;
		this.angle = 180;
	}
	public void hideCardFocused()
	{
		this.focusedCard.SetActive (false);
		if(this.areRandomCardsGenerated)
		{
			this.displayRandomCards(true);
			if(this.randomCardsDisplayed[this.clickedCardId])
			{
				this.randomCards[this.clickedCardId].GetComponent<NewCardStoreController>().show();
			}
		}
		else
		{
			if(this.toResizeBackUI)
			{
				this.resize();
				this.toResizeBackUI=false;
				this.createPacks();
			}
			this.displayPacks(true);
			this.buyCreditsButton.SetActive(true);
		}
		this.isCardFocusedDisplayed = false;
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
		this.nbPages = Mathf.CeilToInt((float)model.packList.Count / (float)this.packsPerLine);
		if(this.nbPages>1)
		{
			this.nbPaginationButtonsLimit = Mathf.CeilToInt((this.worldWidth-this.packsBoardLeftMargin-this.packsBoardRightMargin-1f)/(paginationButtonWidth+gapBetweenPaginationButton));
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
				this.paginationButtons[i].AddComponent<StorePaginationController>();
				this.paginationButtons[i].transform.position=new Vector3((1f+this.packsBoardLeftMargin-this.packsBoardRightMargin+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-1.4f,0f);
				this.paginationButtons[i].name="Pagination"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtons[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebut+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtons[i].GetComponent<StorePaginationController>().setId(i);
				if(this.pageDebut+i-System.Convert.ToInt32(drawBackButton)==this.chosenPage)
				{
					this.paginationButtons[i].GetComponent<StorePaginationController>().setActive(true);
					this.activePaginationButtonId=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtons[0].GetComponent<StorePaginationController>().setId(-2);
				this.paginationButtons[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtons[nbButtonsToDraw-1].GetComponent<StorePaginationController>().setId(-1);
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
				this.paginationButtons[this.activePaginationButtonId].GetComponent<StorePaginationController>().setActive(false);
			}
			this.activePaginationButtonId=id;
			this.chosenPage=this.pageDebut-System.Convert.ToInt32(this.pageDebut!=0)+id;
			this.drawPacks();
		}
	}
	public void refreshCredits()
	{
		StartCoroutine(this.menu.GetComponent<newMenuController> ().getUserData ());
	}
	public void rightClickedHandler(int id)
	{
		this.clickedCardId = id;
		this.focusedCard.SetActive (true);
		this.isCardFocusedDisplayed = true;
		this.displayRandomCards (false);
		this.buyCreditsButton.SetActive (false);
		this.focusedCard.GetComponent<NewFocusedCardStoreController> ().c = model.packList [this.selectedPackIndex].Cards [this.clickedCardId];
		this.focusedCard.GetComponent<NewFocusedCardController> ().show ();
	}
	public void displayRandomCards(bool value)
	{
		for(int i=0;i<this.randomCards.Length;i++)
		{
			if(this.randomCardsDisplayed[i])
			{
				this.randomCards[i].SetActive(value);
			}
		}
	}
	public List<string> getCardTypesAllowed()
	{
		List<string> cardTypesAllowed = new List<string> ();
		for(int i=0;i<model.player.CardTypesAllowed.Count;i++)
		{
			cardTypesAllowed.Add(model.cardTypeList[model.player.CardTypesAllowed[i]]);
		}
		return cardTypesAllowed;
	}
	public int getCardTypeId(int value)
	{
		return model.player.CardTypesAllowed [value];
	}
	public void deleteCard()
	{
		if(this.areRandomCardsGenerated)
		{
			this.randomCardsDisplayed[this.clickedCardId]=false;
			this.hideCardFocused();
			bool stillCards=false;
			for(int i=0;i<this.randomCardsDisplayed.Length;i++)
			{
				if(this.randomCardsDisplayed[i])
				{
					stillCards=true;
					break;
				}
			}
			if(!stillCards)
			{
				this.backToPacks();
			}
		}
		else
		{
			this.randomCardsDisplayed[0]=false;
			this.hideCardFocused();
		}
	}
	public void returnPressed()
	{
		if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardStoreController>().returnPressed();
		}
		else if(this.isErrorViewDisplayed)
		{
			this.hideErrorPopUp();
		}
		else if(this.isAddCreditsViewDisplayed)
		{
			this.addCreditsHandler();
		}
	}
	public void escapePressed()
	{
		if(newMenuController.instance.isAPopUpDisplayed())
		{
			newMenuController.instance.hideAllPopUp();
		}
		else if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardStoreController>().escapePressed();
		}
		else if(this.areRandomCardsGenerated)
		{
			this.backToPacks();
		}
		else if(this.isErrorViewDisplayed)
		{
			this.hideErrorPopUp();
		}
		else if(this.isAddCreditsViewDisplayed)
		{
			this.hideAddCreditsPopUp();
		}
	}
	public void updatePackPrices()
	{
		this.toUpdatePackPrices = false;
		for(int i=0;i<this.packsDisplayed.Count;i++)
		{
			if(ApplicationModel.credits<model.packList[this.packsDisplayed[i]].Price)
			{
				this.packs[i].GetComponent<NewPackStoreController>().setClickable(false);
			}
			else
			{
				this.packs[i].GetComponent<NewPackStoreController>().setClickable(true);
			}
		}
	}
	public void displayAddCreditsPopUp()
	{
		this.addCreditsView = gameObject.AddComponent<NewStoreAddCreditsPopUpView> ();
		this.isAddCreditsViewDisplayed = true;
		addCreditsView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.window);
		addCreditsView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
		addCreditsView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
		addCreditsView.popUpVM.centralWindowTextfieldStyle = new GUIStyle (this.popUpSkin.textField);
		addCreditsView.popUpVM.centralWindowErrorStyle = new GUIStyle (this.popUpSkin.customStyles [1]);
		addCreditsView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
		this.addCreditsPopUpResize ();
	}
	public void displayErrorPopUp(string error)
	{
		this.isErrorViewDisplayed = true;
		this.errorView = Camera.main.gameObject.AddComponent <NewStoreErrorPopUpView>();
		errorView.errorPopUpVM.error = error;
		errorView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.customStyles[3]);
		errorView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
		errorView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
		errorView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
		this.errorPopUpResize ();
	}
	public void addCreditsPopUpResize()
	{
		addCreditsView.popUpVM.centralWindow = this.centralWindow;
		addCreditsView.popUpVM.resize ();
	}
	public void errorPopUpResize()
	{
		errorView.popUpVM.centralWindow = this.centralWindow;
		errorView.popUpVM.resize ();
	}
	public void hideAddCreditsPopUp()
	{
		Destroy (this.addCreditsView);
		this.isAddCreditsViewDisplayed = false;
	}
	public void hideErrorPopUp()
	{
		Destroy (this.errorView);
		this.isErrorViewDisplayed = false;
	}
	public void addCreditsHandler()
	{
		int tempInt = addCreditsSyntaxCheck ();
		if(tempInt!=-1)
		{
			StartCoroutine (addCredits (tempInt));
		}
	}
	public IEnumerator addCredits(int value)
	{
		this.hideAddCreditsPopUp ();
		this.displayLoadingScreen ();
		yield return StartCoroutine (this.model.player.addMoney (value));
		this.refreshCredits ();
		this.hideLoadingScreen ();
	}
	public int addCreditsSyntaxCheck()
	{
		int n;
		bool isNumeric = int.TryParse(addCreditsView.addCreditsPopUpVM.credits, out n);
		if(addCreditsView.addCreditsPopUpVM.credits!="" && isNumeric)
		{
			if(System.Convert.ToInt32(addCreditsView.addCreditsPopUpVM.credits)>0)
			{
				return System.Convert.ToInt32(addCreditsView.addCreditsPopUpVM.credits);
			}
		}
		addCreditsView.addCreditsPopUpVM.error="Merci de bien vouloir saisir une valeur";
		return -1;
	}
	public Vector3 getFirstPackPosition()
	{
		return this.packs[0].transform.FindChild("PackPicture").position;
	}
	public Vector3 getFocusedCardFeaturePosition(int id)
	{
		return this.focusedCard.transform.FindChild ("FocusFeature"+id).position;
	}
	public Vector3 getBuyCreditsButtonPosition()
	{
		return this.buyCreditsButton.transform.position;
	}
	public Vector3 getFocusedCardPosition()
	{
		return this.focusedCard.transform.FindChild("Face").position;
	}
	public void endTutorial()
	{
		newMenuController.instance.setTutorialLaunched (false);
		Application.LoadLevel ("NewHomePage");
	}
	public void setTutorialStep()
	{
		StartCoroutine (model.player.setTutorialStep (-1));
	}
	public bool getIsTutorialLaunched()
	{
		return isTutorialLaunched;
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
