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
	private NewStoreModel model;

	public GameObject cardObject;
	public GameObject packObject;
	public GameObject blockObject;
	public GUISkin popUpSkin;

	private GameObject menu;
	private GameObject tutorial;

	private GameObject packsBlock;
	private GameObject packsBlockTitle;
	private GameObject[] packs;
	private GameObject[] packsTitle;
	private GameObject[] packsPicture;
	private GameObject[] packsButton;
	private GameObject[] separationLines;
	private GameObject packsPaginationButtons;
	private GameObject packsPaginationLine;
	private GameObject packsNumberTitle;
	private GameObject packsScrollLine;

	private GameObject storeBlock;
	private GameObject storeBlockTitle;
	private GameObject storeSubtitle;

	private GameObject buyCreditsBlock;
	private GameObject buyCreditsBlockTitle;
	private GameObject buyCreditsButton;
	private GameObject buyCreditsSubtitle;
	
	private GameObject[] randomCards;
	private GameObject focusedCard;
	private GameObject backButton;

	private GameObject mainCamera;
	private GameObject packsCamera;
	private GameObject menuCamera;
	private GameObject buyCreditsCamera;
	private GameObject tutorialCamera;
	private GameObject backgroundCamera;

	private IList<int> packsDisplayed;
	
	private NewStoreAddCreditsPopUpView addCreditsView;
	private bool isAddCreditsViewDisplayed;
	private NewStoreSelectCardTypePopUpView selectCardTypeView;
	private bool isSelectCardTypeViewDisplayed;

	private Pagination packsPagination;

	private Rect centralWindow;
	private Rect selectCardTypeWindow;

	private int selectedPackIndex;
	private int selectedCardType;
	private int clickedCardId;

	private bool[] toRotate;
	private bool drawFace;
	private bool startRotation;
	private float speed;
	private float angle;
	private Quaternion target;
	private bool[] randomCardsDisplayed;
	private bool areRandomCardsGenerated;

	private bool isCardFocusedDisplayed;

	private bool isSceneLoaded;

	private bool toUpdatePackPrices;
	private bool isScrolling;
	private float scrollIntersection;
	private float packsCameraIntermediatePosition;

	private bool toSlideRight;
	private bool toSlideLeft;
	private bool storeDisplayed;
	private bool mainContentDisplayed;
	
	private float storePositionX;
	private float mainContentPositionX;
	
	void Update () 
	{
		if (Input.touchCount == 1 && this.isSceneLoaded) 
		{
			if(Input.touches[0].deltaPosition.x<-15f && Mathf.Abs(Input.touches[0].deltaPosition.y)<Mathf.Abs(Input.touches[0].deltaPosition.x))
			{
				if(this.storeDisplayed || this.toSlideLeft)
				{
					this.toSlideRight=true;
					this.toSlideLeft=false;
					this.storeDisplayed=false;
				}
			}
			else if(Input.touches[0].deltaPosition.x>15f && Mathf.Abs(Input.touches[0].deltaPosition.y)<Mathf.Abs(Input.touches[0].deltaPosition.x))
			{
				if(this.mainContentDisplayed || this.toSlideRight)
				{
					if(this.mainContentDisplayed)
					{
						this.packsCamera.GetComponent<ScrollingController>().reset();
					}
					this.toSlideLeft=true;
					this.toSlideRight=false;
					this.mainContentDisplayed=false;
				}
			}
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
							TutorialObjectController.instance.tutorialTrackPoint();
						}
					}
					this.target = Quaternion.Euler(0, this.angle, 0);
					this.randomCards[i].transform.rotation = target;
				}
			}
		}
		if(toSlideRight || toSlideLeft)
		{
			Vector3 mainCameraPosition = this.mainCamera.transform.position;
			Vector3 packsCameraPosition = this.packsCamera.transform.position;
			Vector3 buyCameraPosition = this.buyCreditsCamera.transform.position;
			float camerasXPosition = mainCameraPosition.x;
			if(toSlideRight)
			{
				camerasXPosition=camerasXPosition+Time.deltaTime*40f;
				if(camerasXPosition>this.mainContentPositionX)
				{
					camerasXPosition=this.mainContentPositionX;
					this.toSlideRight=false;
					this.mainContentDisplayed=true;
				}
			}
			else if(toSlideLeft)
			{
				camerasXPosition=camerasXPosition-Time.deltaTime*40f;
				if(camerasXPosition<this.storePositionX)
				{
					camerasXPosition=this.storePositionX;
					this.toSlideLeft=false;
					this.storeDisplayed=true;
				}
			}
			buyCameraPosition.x=camerasXPosition;
			mainCameraPosition.x=camerasXPosition;
			packsCameraPosition.x=camerasXPosition;
			this.mainCamera.transform.position=mainCameraPosition;
			this.packsCamera.transform.position=packsCameraPosition;
			this.buyCreditsCamera.transform.position=buyCameraPosition;
		}
		if(ApplicationDesignRules.isMobileScreen && this.isSceneLoaded && this.mainContentDisplayed)
		{
			isScrolling = this.packsCamera.GetComponent<ScrollingController>().ScrollController();
		}
	}
	void Awake()
	{
		instance = this;
		this.model = new NewStoreModel ();
		this.speed = 300.0f;
		this.scrollIntersection = 1.4f;
		this.mainContentDisplayed = true;
		this.initializeScene ();
		this.startMenuInitialization ();
	}
	public void paginationHandler()
	{
		this.drawPaginationNumber ();
		this.drawPacks ();
	}
	private void startMenuInitialization()
	{
		this.menu = GameObject.Find ("Menu");
		this.menu.AddComponent<StoreMenuController> ();
	}
	public void endMenuInitialization()
	{
		this.startTutorialInitialization ();
	}
	private void startTutorialInitialization()
	{
		this.tutorial = GameObject.Find ("Tutorial");
		this.tutorial.AddComponent<StoreTutorialController>();
	}
	public void endTutorialInitialization()
	{
		StartCoroutine (this.initialization ());
	}
	private void initializeScene()
	{
		this.packsBlock = Instantiate (this.blockObject) as GameObject;
		this.packsBlockTitle = GameObject.Find ("PacksBlockTitle");
		this.packsBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.packsBlockTitle.GetComponent<TextMeshPro> ().text = "Acheter";
		this.packsNumberTitle = GameObject.Find ("PacksNumberTitle");
		this.packsNumberTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;

		this.packs = new GameObject[0];
		this.packsButton = new GameObject[0];
		this.packsTitle = new GameObject[0];
		this.packsPicture=new GameObject[0];
		this.separationLines = new GameObject[0];

		this.packsPaginationButtons = GameObject.Find("Pagination");
		this.packsPaginationButtons.AddComponent<NewStorePaginationController> ();
		this.packsPaginationButtons.GetComponent<NewStorePaginationController> ().initialize ();
		this.packsPaginationLine = GameObject.Find ("PacksPaginationLine");
		this.packsPaginationLine.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.packsScrollLine = GameObject.Find ("PacksScrollLine");
		this.packsScrollLine.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;

		this.backButton = GameObject.Find ("BackButton");
		this.backButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "Retour à la boutique".ToUpper();
		this.backButton.AddComponent<NewStoreBackButtonController> ();
		this.backButton.SetActive (false);

		this.storeBlock = Instantiate (this.blockObject) as GameObject;
		this.storeBlockTitle = GameObject.Find ("StoreBlockTitle");
		this.storeBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.storeBlockTitle.GetComponent<TextMeshPro> ().text = "La boutique";
		this.storeSubtitle = GameObject.Find ("StoreSubtitle");
		this.storeSubtitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.storeSubtitle.GetComponent<TextMeshPro> ().text = "Faites l'acquisition de nouvelles recrues en sélectionnant le pack qui vous intéresse.";

		this.buyCreditsBlock = Instantiate (this.blockObject) as GameObject;
		this.buyCreditsSubtitle = GameObject.Find ("BuyCreditsSubtitle");
		this.buyCreditsSubtitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.buyCreditsSubtitle.GetComponent<TextMeshPro> ().text = "Le meilleur moyen d'accéder aux cartes rares";
		this.buyCreditsBlockTitle = GameObject.Find ("BuyCreditsBlockTitle");
		this.buyCreditsBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.buyCreditsBlockTitle.GetComponent<TextMeshPro> ().text = "Ajouter des crédits";
		this.buyCreditsButton = GameObject.Find ("BuyCreditsButton");
		this.buyCreditsButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "J'achète des crédits".ToUpper();
		this.buyCreditsButton.AddComponent<NewStoreBuyCreditsButtonController> ();

		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardStoreController> ();
		this.focusedCard.SetActive (false);
		this.mainCamera = gameObject;
		this.menuCamera = GameObject.Find ("MenuCamera");
		this.tutorialCamera = GameObject.Find ("TutorialCamera");
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
		this.packsCamera = GameObject.Find ("PacksCamera");
		this.packsCamera.AddComponent<ScrollingController> ();
		this.buyCreditsCamera = GameObject.Find ("BuyCreditsCamera");
	}
	private IEnumerator initialization()
	{
		this.resize ();
		MenuController.instance.displayLoadingScreen ();
		yield return(StartCoroutine(this.model.initializeStore()));
		this.initializePacks ();
		this.initializeBuyCreditsButton ();
		MenuController.instance.hideLoadingScreen ();
		this.isSceneLoaded = true;
		if(ApplicationModel.packToBuy!=-1)
		{
			this.buyPackHandler(ApplicationModel.packToBuy,true);
			ApplicationModel.packToBuy=-1;
		}
		else if(model.player.TutorialStep!=-1)
		{
			TutorialObjectController.instance.startTutorial(model.player.TutorialStep,model.player.displayTutorial);
		}
	}
	public void initializePacks()
	{
		this.packsPagination.chosenPage = 0;
		this.packsPagination.totalElements = model.packList.Count;
		this.packsPaginationButtons.GetComponent<NewStorePaginationController> ().p = packsPagination;
		this.packsPaginationButtons.GetComponent<NewStorePaginationController> ().setPagination ();
		this.drawPaginationNumber ();
		this.drawPacks ();
	}
	private void initializeBuyCreditsButton()
	{
		if(!ApplicationModel.isAdmin)
		{
			this.buyCreditsButton.GetComponent<NewStoreBuyCreditsButtonController>().setIsActive(false);
		}
		else
		{
			this.buyCreditsButton.GetComponent<NewStoreBuyCreditsButtonController>().setIsActive(true);
		}
	}
	public void drawPaginationNumber()
	{
		if(packsPagination.totalElements>0)
		{
			this.packsNumberTitle.GetComponent<TextMeshPro>().text=("pack " +this.packsPagination.elementDebut+" à "+this.packsPagination.elementFin+" sur "+this.packsPagination.totalElements ).ToUpper();
		}
		else
		{
			this.packsNumberTitle.GetComponent<TextMeshPro>().text="aucun pack à afficher".ToUpper();
		}
	}
	public void resize()
	{
		this.menuCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.tutorialCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.backgroundCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.backgroundCameraSize;

		float packsBlockLeftMargin;
		float packsBlockUpMargin;
		float packsBlockHeight;
		
		float storeBlockLeftMargin;
		float storeBlockUpMargin;
		float storeBlockHeight;
		
		float buyCreditsBlockLeftMargin;
		float buyCreditsBlockUpMargin;
		float buyCreditsBlockHeight;
		
		storeBlockHeight=ApplicationDesignRules.mediumBlockHeight;
		buyCreditsBlockHeight=ApplicationDesignRules.smallBlockHeight;

		this.packsPagination = new Pagination ();
		
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.packsScrollLine.SetActive(true);
			this.packsPagination.nbElementsPerPage = 4;

			packsBlockHeight=3f+this.packsPagination.nbElementsPerPage*(ApplicationDesignRules.cardWorldSize.y);

			storeBlockLeftMargin=-ApplicationDesignRules.worldWidth;
			storeBlockUpMargin=0f;
			
			packsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			packsBlockUpMargin=0f;
			
			buyCreditsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			buyCreditsBlockUpMargin=packsBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+packsBlockHeight;
		}
		else
		{
			this.packsScrollLine.SetActive(false);
			this.packsPagination.nbElementsPerPage = 2;

			this.packsCamera.SetActive(false);
			this.buyCreditsCamera.SetActive(false);
			this.mainCamera.GetComponent<Camera>().rect=new Rect(0f,0f,1f,1f);
			this.mainCamera.transform.position=ApplicationDesignRules.mainCameraStartPosition;
			this.mainCamera.GetComponent<Camera>().orthographicSize=ApplicationDesignRules.cameraSize;

			packsBlockHeight=ApplicationDesignRules.largeBlockHeight;

			storeBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			storeBlockUpMargin=ApplicationDesignRules.upMargin;
			
			buyCreditsBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			buyCreditsBlockUpMargin=storeBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+storeBlockHeight;
			
			packsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			packsBlockUpMargin=ApplicationDesignRules.upMargin;
		}

		this.centralWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen, ApplicationDesignRules.widthScreen * 0.50f, 0.40f * ApplicationDesignRules.heightScreen);
		this.selectCardTypeWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen, ApplicationDesignRules.widthScreen * 0.50f, 0.50f * ApplicationDesignRules.heightScreen);
		
		this.packsBlock.GetComponent<NewBlockController> ().resize(packsBlockLeftMargin,packsBlockUpMargin,ApplicationDesignRules.blockWidth,packsBlockHeight);
		Vector3 packsBlockUpperLeftPosition = this.packsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 packsBlockLowerLeftPosition = this.packsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector3 packsBlockUpperRightPosition = this.packsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 packsBlockSize = this.packsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector3 packsBlockOrigin = this.packsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.packsBlockTitle.transform.position = new Vector3 (packsBlockUpperLeftPosition.x + 0.3f, packsBlockUpperLeftPosition.y - 0.2f, 0f);
		this.packsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		this.packsNumberTitle.transform.position = new Vector3 (packsBlockUpperLeftPosition.x + 0.3f, packsBlockUpperLeftPosition.y - 1.2f, 0f);
		this.packsNumberTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		this.packs = new GameObject[packsPagination.nbElementsPerPage];
		this.packsButton = new GameObject[packsPagination.nbElementsPerPage];
		this.packsPicture = new GameObject[packsPagination.nbElementsPerPage];
		this.packsTitle = new GameObject[packsPagination.nbElementsPerPage];
		this.separationLines = new GameObject[packsPagination.nbElementsPerPage];

		float upperMargin = 1.6f;
		float lowerMargin = 0.6f;
		Vector2 packBlockSize = new Vector2 (packsBlockSize.x - 0.6f, (packsBlockSize.y - lowerMargin-upperMargin)/this.packsPagination.nbElementsPerPage);
		float lineScale = ApplicationDesignRules.getLineScale (packsBlockSize.x - 0.6f);

		float packPictureWidth = 375f;
		float packPictureHeight = 200f;
		float packPictureScale = 1.3f * ApplicationDesignRules.reductionRatio;
		float packPictureWorldWidth = packPictureScale * (packPictureWidth / ApplicationDesignRules.pixelPerUnit);
		float packPictureWorldHeight = packPictureWorldWidth * (packPictureHeight / packPictureWidth);

		for(int i=0;i<this.packsPagination.nbElementsPerPage;i++)
		{
			this.packs[i]=Instantiate (this.packObject) as GameObject;
			this.packsButton[i]=this.packs[i].transform.FindChild("Button").gameObject;
			this.packsTitle[i]=this.packs[i].transform.FindChild("Name").gameObject;
			this.packsPicture[i]=this.packs[i].transform.FindChild("Picture").gameObject;
			this.separationLines[i]=this.packs[i].transform.FindChild("SeparationLine").gameObject;
			this.packs[i].transform.FindChild("Button").gameObject.AddComponent<NewStoreBuyPackButtonController>();
			this.packs[i].transform.FindChild("Button").GetComponent<NewStoreBuyPackButtonController>().setId(i);
			this.separationLines[i].transform.localScale=new Vector3(lineScale,1f,1f);
			this.separationLines[i].transform.position=new Vector3(packsBlockUpperLeftPosition.x+packsBlockSize.x/2f,packsBlockUpperLeftPosition.y-upperMargin-i*packBlockSize.y,0f);
			this.packsTitle[i].transform.localScale=ApplicationDesignRules.subMainTitleScale;
			this.packsTitle[i].transform.position=new Vector3(packsBlockUpperLeftPosition.x+0.3f,packsBlockUpperLeftPosition.y-upperMargin-0.2f-i*(packBlockSize.y),0f);
			this.packsButton[i].transform.localScale=ApplicationDesignRules.button62Scale;
			this.packsButton[i].transform.position=new Vector3(packsBlockUpperLeftPosition.x+0.3f+ApplicationDesignRules.button62WorldSize.x/2f,packsBlockUpperLeftPosition.y-upperMargin-packBlockSize.y+ApplicationDesignRules.button62WorldSize.y/2f+0.1f-i*(packBlockSize.y),0f);
			this.packsPicture[i].transform.localScale=new Vector3(packPictureScale,packPictureScale,packPictureScale);
			this.packsPicture[i].transform.position=new Vector3(packsBlockUpperRightPosition.x-0.3f-packPictureWorldWidth/2f,packsBlockUpperRightPosition.y-upperMargin-packBlockSize.y+packPictureWorldHeight/2f+0.05f-i*(packBlockSize.y),0f);
		}

		this.packsPaginationButtons.transform.localPosition=new Vector3(packsBlockLowerLeftPosition.x+packsBlockSize.x/2f, packsBlockLowerLeftPosition.y + 0.3f, 0f);
		this.packsPaginationButtons.transform.GetComponent<NewStorePaginationController> ().resize ();

		this.packsPaginationLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.packsPaginationLine.transform.position = new Vector3 (packsBlockLowerLeftPosition.x + packsBlockSize.x / 2, packsBlockLowerLeftPosition.y + 0.6f, 0f);

		this.focusedCard.transform.localScale = ApplicationDesignRules.cardFocusedScale;
		this.focusedCard.transform.position = new Vector3 (0f, -ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.downMargin+ApplicationDesignRules.cardFocusedWorldSize.y/2f-0.22f, 0f);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setCentralWindow (this.centralWindow);
		
		this.storeBlock.GetComponent<NewBlockController> ().resize(storeBlockLeftMargin,storeBlockUpMargin,ApplicationDesignRules.blockWidth,storeBlockHeight);
		Vector3 storeBlockUpperLeftPosition = this.storeBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 storeBlockUpperRightPosition = this.storeBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 storeBlockSize = this.storeBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 storeBlockOrigin = this.storeBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.storeBlockTitle.transform.position = new Vector3 (storeBlockUpperLeftPosition.x + 0.3f, storeBlockUpperLeftPosition.y - 0.2f, 0f);
		this.storeBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.storeSubtitle.transform.position = new Vector3 (storeBlockUpperLeftPosition.x + 0.3f, storeBlockUpperLeftPosition.y - 1.2f, 0f);
		this.storeSubtitle.transform.GetComponent<TextContainer>().width=storeBlockSize.x-0.6f;
		this.storeSubtitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		
		this.buyCreditsBlock.GetComponent<NewBlockController> ().resize(buyCreditsBlockLeftMargin,buyCreditsBlockUpMargin,ApplicationDesignRules.blockWidth,buyCreditsBlockHeight);
		Vector3 buyCreditsBlockUpperLeftPosition = this.buyCreditsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector2 buyCreditsBlockSize = this.buyCreditsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector3 buyCreditsOrigin = this.buyCreditsBlock.GetComponent<NewBlockController> ().getOriginPosition ();

		this.buyCreditsBlockTitle.transform.position = new Vector3 (buyCreditsBlockUpperLeftPosition.x + 0.3f, buyCreditsBlockUpperLeftPosition.y - 0.2f, 0f);
		this.buyCreditsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.buyCreditsSubtitle.transform.position = new Vector3 (buyCreditsBlockUpperLeftPosition.x + 0.3f, buyCreditsBlockUpperLeftPosition.y - 1.2f, 0f);
		this.buyCreditsSubtitle.transform.GetComponent<TextContainer>().width=buyCreditsBlockSize.x-0.6f;
		this.buyCreditsSubtitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		this.buyCreditsButton.transform.position = new Vector3(buyCreditsOrigin.x,buyCreditsOrigin.y-0.5f,buyCreditsOrigin.z);
		this.buyCreditsButton.transform.localScale = ApplicationDesignRules.button62Scale;

		this.backButton.transform.position = new Vector3 (0, -3f, 0f);
		this.backButton.transform.localScale = ApplicationDesignRules.button62Scale;

		this.packsScrollLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.packsScrollLine.transform.position = new Vector3 (buyCreditsBlockUpperLeftPosition.x + buyCreditsBlockSize.x / 2, buyCreditsBlockUpperLeftPosition.y-0.03f, 0f);

		if(ApplicationDesignRules.isMobileScreen)
		{
			this.mainCamera.GetComponent<Camera> ().rect = new Rect (0f,(ApplicationDesignRules.worldHeight-ApplicationDesignRules.upMargin-this.scrollIntersection)/ApplicationDesignRules.worldHeight,1f,(this.scrollIntersection)/ApplicationDesignRules.worldHeight);
			this.mainCamera.GetComponent<Camera> ().orthographicSize = this.scrollIntersection/2f;
			this.mainCamera.transform.position = new Vector3 (0f, packsBlockUpperLeftPosition.y-(this.scrollIntersection/2f), -10f);
			
			this.packsCamera.SetActive(true);
			this.packsCamera.GetComponent<Camera> ().rect = new Rect (0f,(ApplicationDesignRules.downMargin+buyCreditsBlockHeight)/ApplicationDesignRules.worldHeight,1f,(ApplicationDesignRules.viewHeight-this.scrollIntersection-buyCreditsBlockHeight)/ApplicationDesignRules.worldHeight);
			this.packsCamera.GetComponent<Camera> ().orthographicSize = (ApplicationDesignRules.viewHeight-this.scrollIntersection-buyCreditsBlockHeight)/2f;
			this.packsCamera.GetComponent<ScrollingController> ().setViewHeight(ApplicationDesignRules.viewHeight-this.scrollIntersection-buyCreditsBlockHeight);
			this.packsCamera.GetComponent<ScrollingController> ().setContentHeight(packsBlockHeight-this.scrollIntersection-0.1f);
			this.packsCamera.transform.position = new Vector3 (0f, packsBlockUpperLeftPosition.y-(this.scrollIntersection/2f)-this.scrollIntersection/2f-(ApplicationDesignRules.viewHeight-this.scrollIntersection-buyCreditsBlockHeight)/2f, -10f);
			this.packsCamera.GetComponent<ScrollingController> ().setStartPositionY (this.packsCamera.transform.position.y);

			this.buyCreditsCamera.SetActive(true);
			this.buyCreditsCamera.GetComponent<Camera>().rect=new Rect(0f,(ApplicationDesignRules.downMargin)/ApplicationDesignRules.worldHeight,1f,(buyCreditsBlockHeight)/ApplicationDesignRules.worldHeight);
			this.buyCreditsCamera.GetComponent<Camera> ().orthographicSize = (buyCreditsBlockHeight)/2f;
			this.buyCreditsCamera.transform.position=new Vector3(0f,buyCreditsOrigin.y,-10f);

			this.mainContentPositionX = packsBlockOrigin.x;
			this.storePositionX=storeBlockOrigin.x;
		}

		if(areRandomCardsGenerated)
		{
			this.resizeRandomCards();
		}
		if(isSelectCardTypeViewDisplayed)
		{
			this.selectCardPopUpResize();
		}
		if(isAddCreditsViewDisplayed)
		{
			this.addCreditsPopUpResize();
		}
		TutorialObjectController.instance.resize();
	}
	public void createRandomCards()
	{
		this.areRandomCardsGenerated=true;
		for (int i = 0; i <model.packList [this.selectedPackIndex].NbCards; i++)
		{
			this.randomCards[i] = Instantiate(this.cardObject) as GameObject;
			this.randomCards[i].AddComponent<NewCardStoreController>();
			this.randomCards[i].GetComponent<NewCardStoreController>().c=model.packList[this.selectedPackIndex].Cards.getCard(i);
			this.randomCards[i].GetComponent<NewCardStoreController>().show();
			this.randomCards[i].GetComponent<NewCardStoreController>().setId(i);
			this.randomCards[i].GetComponent<NewCardStoreController>().setBackFace(true);
			this.randomCards[i].name="Card"+i;
			this.randomCardsDisplayed[i]=true;
		}
	}
	public void resizeRandomCards()
	{
		float width = ApplicationDesignRules.worldWidth-ApplicationDesignRules.leftMargin-ApplicationDesignRules.rightMargin;
		float cardWorldWidth = width/(model.packList [this.selectedPackIndex].NbCards+1);
		float scaleCard = cardWorldWidth / (ApplicationDesignRules.getCardOriginalSize().x/ApplicationDesignRules.pixelPerUnit);
		float cardMaxWorldHeight = ApplicationDesignRules.cardFocusedWorldSize.y;
		float cardWorldHeight = cardWorldWidth * (ApplicationDesignRules.getCardOriginalSize().x / ApplicationDesignRules.getCardOriginalSize().y);
		if(cardWorldHeight>cardMaxWorldHeight)
		{
			cardWorldWidth=cardMaxWorldHeight*(ApplicationDesignRules.getCardOriginalSize().x/ApplicationDesignRules.getCardOriginalSize().y);
			scaleCard=(cardMaxWorldHeight*(ApplicationDesignRules.getCardOriginalSize().x/ApplicationDesignRules.getCardOriginalSize().y)*ApplicationDesignRules.pixelPerUnit)/ApplicationDesignRules.getCardOriginalSize().x;
		}
		Vector3 scale = new Vector3 (scaleCard, scaleCard, scaleCard);
		Vector3 position=new Vector3(0,0,0);
		float gapBetweenCards = cardWorldWidth / (model.packList [this.selectedPackIndex].NbCards + 1);
		for (int i = 0; i <model.packList [this.selectedPackIndex].NbCards; i++)
		{
			scale = new Vector3(scaleCard,scaleCard,scaleCard);
			position = new Vector3((ApplicationDesignRules.leftMargin-ApplicationDesignRules.rightMargin-width)/2f+gapBetweenCards+cardWorldWidth/2f+(float)i*(gapBetweenCards+cardWorldWidth), 0f, 0f); 
			this.randomCards[i].transform.position=position;
			this.randomCards[i].transform.localScale=scale;
		}
	}
	public void rotateRandomCards()
	{	
		for (int i = 0; i <model.packList [this.selectedPackIndex].NbCards; i++)
		{
			this.toRotate[i]=false;
			this.randomCards[i].transform.rotation=Quaternion.Euler(0, 180, 0);
			this.randomCardsDisplayed[i]=true;
		}
		this.startRotation = true;
		this.toRotate [0] = true;
		this.angle = 180;
	}
	public void createSingleCard()
	{
		this.randomCards[0]=this.focusedCard;
		this.randomCardsDisplayed[0]=true;
		this.focusedCard.SetActive(true);
		this.isCardFocusedDisplayed = true;
		this.focusedCard.GetComponent<NewFocusedCardStoreController>().displayFocusFeatures(false);
		this.focusedCard.GetComponent<NewFocusedCardStoreController>().c=model.packList[this.selectedPackIndex].Cards.getCard(0);
		this.focusedCard.GetComponent<NewFocusedCardStoreController>().show ();
		this.focusedCard.GetComponent<NewFocusedCardStoreController>().setBackFace(true);
		this.focusedCard.GetComponent<NewFocusedCardStoreController>().transform.rotation=Quaternion.Euler(0, 180, 0);
		this.isCardFocusedDisplayed=true;
	}
	public void rotateSingleCard()
	{
		this.startRotation = true;
		this.toRotate [0] = true;
		this.angle = 180;
	}
	public void cleanCards()
	{
		this.areRandomCardsGenerated = false;
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
	public void backToPacksHandler()
	{
		this.displayBackUI (true);
	}
	public void displayBackUI(bool value)
	{
		for(int i=0;i<this.packs.Length;i++)
		{
			if(i<this.packsDisplayed.Count && value)
			{
				this.packs[i].SetActive(true);
				this.separationLines[i].SetActive(true);
			}
			else
			{
				this.packs[i].SetActive(false);
				this.separationLines[i].SetActive(false);
			}
		}
		this.packsBlock.SetActive (value);
		this.packsBlockTitle.SetActive (value);
		this.packsNumberTitle.SetActive (value);
		if(value)
		{
			this.packsPaginationButtons.GetComponent<NewStorePaginationController>().setPagination();
		}
		else
		{
			this.packsPaginationButtons.GetComponent<NewStorePaginationController>().setVisible(false);
		}
		this.packsPaginationLine.SetActive (value);
		this.storeBlock.SetActive (value);
		this.storeBlockTitle.SetActive (value);
		this.storeSubtitle.SetActive (value);
		this.buyCreditsSubtitle.SetActive (value);
		this.buyCreditsBlock.SetActive (value);
		this.buyCreditsButton.SetActive (value);
		this.buyCreditsBlockTitle.SetActive (value);

		if(value && toUpdatePackPrices)
		{
			this.updatePackPrices();
		}
		if(!value && areRandomCardsGenerated && !TutorialObjectController.instance.getIsTutorialLaunched())
		{
			this.backButton.SetActive (true);
			this.backButton.GetComponent<NewStoreBackButtonController>().reset();
		}
		else
		{
			this.backButton.SetActive(false);
		}
		if(value && areRandomCardsGenerated)
		{
			this.cleanCards();
		}
	}
	public void drawPacks()
	{
		this.packsDisplayed = new List<int> ();
		
		for(int i=0;i<this.packsPagination.nbElementsPerPage;i++)
		{
			if(this.packsPagination.chosenPage*(this.packsPagination.nbElementsPerPage)+i<model.packList.Count)
			{
				this.packsDisplayed.Add (this.packsPagination.chosenPage*(this.packsPagination.nbElementsPerPage)+i);
				this.packs[i].transform.FindChild("Button").FindChild("Title").GetComponent<TextMeshPro>().text=model.packList[this.packsDisplayed[i]].Price.ToString();
				this.packs[i].transform.FindChild("Name").GetComponent<TextMeshPro>().text=model.packList[this.packsDisplayed[i]].Name;
				this.packs[i].transform.FindChild("Picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnPackPicture (model.packList[this.packsDisplayed[i]].IdPicture);
				this.separationLines[i].SetActive(true);
				this.packs[i].SetActive(true);
			}
			else
			{
				this.separationLines[i].SetActive(false);
				this.packs[i].SetActive(false);
			}
		}
		this.updatePackPrices ();
	}
	public void cleanPacks()
	{
		for(int i=0;i<this.packs.Length;i++)
		{
			Destroy(this.packs[i]);
		}
	}
	public void buyPackHandler(int id, bool fromHome=false)
	{
		if(fromHome)
		{
			for(int i=0;i<model.packList.Count;i++)
			{
				if(model.packList[i].Id==id)
				{
					this.selectedPackIndex=i;
					break;
				}

			}
		}
		else
		{
			this.selectedPackIndex = this.packsDisplayed [id];
		}
		this.selectedCardType = model.packList [this.selectedPackIndex].CardType;
		if(this.selectedCardType==-2)
		{
			this.displaySelectCardTypePopUp();
		}
		else
		{
			StartCoroutine (this.buyPack ());
		}
	}
	public void buyPackWidthCardTypeHandler()
	{
		if(isCardTypeSelected())
		{
			selectCardTypeView.selectCardTypePopUpVM.guiEnabled=false;
			int cardType;
			this.selectedCardType=this.getCardTypeId(selectCardTypeView.selectCardTypePopUpVM.cardTypeSelected);
			this.hideSelectCardPopUp();
			StartCoroutine(this.buyPack());	
		}
	}
	public IEnumerator buyPack()
	{
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.buyPack (this.selectedPackIndex, this.selectedCardType, TutorialObjectController.instance.getIsTutorialLaunched()));
		MenuController.instance.hideLoadingScreen ();
		if(model.Error=="")
		{
			this.displayBackUI(false);
			this.randomCardsDisplayed = new bool[model.packList [this.selectedPackIndex].NbCards];
			this.randomCards = new GameObject[model.packList [this.selectedPackIndex].NbCards];
			this.toRotate=new bool[model.packList [this.selectedPackIndex].NbCards];
			if(model.packList[this.selectedPackIndex].NbCards>1)
			{
				this.backButton.SetActive(true);
				this.createRandomCards();
				this.resizeRandomCards();
				this.rotateRandomCards();
				if(this.model.CollectionPointsEarned>0)
				{
					MenuController.instance.displayCollectionPointsPopUp(model.CollectionPointsEarned,model.CollectionPointsRanking);
				}
				if(this.model.NewSkills.Count>0)
				{
					MenuController.instance.displayNewSkillsPopUp(model.NewSkills);
				}
				TutorialObjectController.instance.tutorialTrackPoint ();
			}
			else
			{
				this.createSingleCard();
				this.rotateSingleCard();
			}
		}
		else
		{
			MenuController.instance.displayErrorPopUp(model.Error);
		}
	}
	public void hideCardFocused()
	{
		this.isCardFocusedDisplayed = false;
		this.focusedCard.SetActive (false);
		if(this.areRandomCardsGenerated)
		{
			this.displayRandomCards(true);
			this.backButton.SetActive(true);
			if(this.randomCardsDisplayed[this.clickedCardId])
			{
				this.randomCards[this.clickedCardId].GetComponent<NewCardStoreController>().show();
			}
		}
		else
		{
			this.displayBackUI(true);
		}
	}
	public void leftClickedHandler(int id)
	{
		this.clickedCardId = id;
		this.focusedCard.SetActive (true);
		this.isCardFocusedDisplayed = true;
		this.displayRandomCards (false);
		this.buyCreditsButton.SetActive (false);
		this.focusedCard.GetComponent<NewFocusedCardStoreController> ().displayFocusFeatures (true);
		this.focusedCard.GetComponent<NewFocusedCardStoreController> ().c = model.packList [this.selectedPackIndex].Cards.getCard (this.clickedCardId);
		this.focusedCard.GetComponent<NewFocusedCardController> ().show ();
	}
	public void displayRandomCards(bool value)
	{
		for(int i=0;i<this.randomCards.Length;i++)
		{
			if(this.randomCardsDisplayed[i] && value)
			{
				this.randomCards[i].SetActive(true);
			}
			else
			{
				this.randomCards[i].SetActive(false);
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
			StartCoroutine(MenuController.instance.getUserData ());
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
				this.displayBackUI(true);
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
		else if(this.isAddCreditsViewDisplayed)
		{
			this.addCreditsHandler();
		}
	}
	public void escapePressed()
	{
		if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardStoreController>().escapePressed();
		}
		else if(this.areRandomCardsGenerated)
		{
			this.displayBackUI(true);
		}
		else if(this.isAddCreditsViewDisplayed)
		{
			this.hideAddCreditsPopUp();
		}
		else if(this.isSelectCardTypeViewDisplayed)
		{
			this.hideSelectCardPopUp();
		}
		else
		{
			MenuController.instance.leaveGame();
		}
	}
	public void closeAllPopUp()
	{
		if(this.isAddCreditsViewDisplayed)
		{
			this.hideAddCreditsPopUp();
		}
		else if(this.isSelectCardTypeViewDisplayed)
		{
			this.hideSelectCardPopUp();
		}
	}
	public void updatePackPrices()
	{
		this.toUpdatePackPrices = false;
		for(int i=0;i<this.packsDisplayed.Count;i++)
		{
			if(ApplicationModel.credits<model.packList[this.packsDisplayed[i]].Price)
			{
				this.packs[i].transform.FindChild("Button").GetComponent<NewStoreBuyPackButtonController>().setIsActive(false);
			}
			else
			{
				this.packs[i].transform.FindChild("Button").GetComponent<NewStoreBuyPackButtonController>().setIsActive(true);
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
	public void displaySelectCardTypePopUp()
	{
		this.selectCardTypeView = gameObject.AddComponent<NewStoreSelectCardTypePopUpView> ();
		this.isSelectCardTypeViewDisplayed = true;
		List<string> cardTypesAllowed = this.getCardTypesAllowed ();
		selectCardTypeView.selectCardTypePopUpVM.cardTypes=new string[cardTypesAllowed.Count];
		for(int i =0;i<cardTypesAllowed.Count;i++)
		{
			selectCardTypeView.selectCardTypePopUpVM.cardTypes[i]=cardTypesAllowed[i];
		}
		selectCardTypeView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.window);
		selectCardTypeView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
		selectCardTypeView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
		selectCardTypeView.popUpVM.centralWindowSelGridStyle = new GUIStyle (this.popUpSkin.toggle);
		selectCardTypeView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
		selectCardTypeView.popUpVM.centralWindowErrorStyle = new GUIStyle (this.popUpSkin.customStyles [1]);
		this.selectCardPopUpResize ();
	}
	private bool isCardTypeSelected()
	{
		if(selectCardTypeView.selectCardTypePopUpVM.cardTypeSelected!=-1)
		{
			return true;
		}
		else
		{
			selectCardTypeView.selectCardTypePopUpVM.error="Veuillez sélectionner une classe";
			return false;
		}
	}
	public void addCreditsPopUpResize()
	{
		addCreditsView.popUpVM.centralWindow = this.centralWindow;
		addCreditsView.popUpVM.resize ();
	}
	private void selectCardPopUpResize()
	{
		selectCardTypeView.popUpVM.centralWindow = this.centralWindow;
		selectCardTypeView.popUpVM.resize ();
	}
	public void hideSelectCardPopUp()
	{
		Destroy (this.selectCardTypeView);
		this.isSelectCardTypeViewDisplayed = false;
	}
	public void hideAddCreditsPopUp()
	{
		Destroy (this.addCreditsView);
		this.isAddCreditsViewDisplayed = false;
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
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (this.model.player.addMoney (value));
		StartCoroutine(MenuController.instance.getUserData ());
		MenuController.instance.hideLoadingScreen ();
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
	public void moneyUpdate()
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
	}

	#region TUTORIAL FUNCTIONS

	public Vector3 returnBuyPackButtonPosition(int id)
	{
		return this.packsButton [id].transform.position;
	}
	public bool getIsCardFocusedDisplayed()
	{
		return isCardFocusedDisplayed;
	}
	public bool getAreRandomCardsDisplayed()
	{
		return areRandomCardsGenerated;
	}
	public Vector3 getBuyCreditsBlockOrigin()
	{
		return this.buyCreditsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getBuyCreditsBlockSize()
	{
		return this.buyCreditsBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public Vector3 getPacksBlockOrigin()
	{
		return this.packsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getPacksBlockSize()
	{
		return this.packsBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public Vector3 getStoreBlockOrigin()
	{
		return this.storeBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public GameObject returnCardFocused()
	{
		return this.focusedCard;
	}

	#endregion
}
