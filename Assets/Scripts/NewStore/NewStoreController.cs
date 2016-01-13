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
	private GameObject packsPaginationButtons;
	private GameObject packsPaginationLine;
	private GameObject packsNumberTitle;
	private GameObject bottomPacksScrollLine;
	private GameObject topPacksScrollLine;

	private GameObject storeBlock;
	private GameObject storeBlockTitle;
	private GameObject storeSubtitle;

	private GameObject buyCreditsBlock;
	private GameObject buyCreditsBlockTitle;
	private GameObject buyCreditsButton;
	private GameObject buyCreditsSubtitle;

	private GameObject informationButton;
	private GameObject slideRightButton;
	
	private GameObject[] randomCards;
	private GameObject focusedCard;
	private GameObject backButton;

	private GameObject mainCamera;
	private GameObject upperScrollCamera;
	private GameObject mediumScrollCamera;
	private GameObject lowerScrollCamera;
	private GameObject sceneCamera;
	private GameObject menuCamera;
	private GameObject tutorialCamera;
	private GameObject backgroundCamera;

	private Vector3 lowerScrollCameraStandardPosition;
	private Vector3 lowerScrollCameraStorePosition;

	private IList<int> packsDisplayed;
	
	private NewStoreAddCreditsPopUpView addCreditsView;
	private bool isAddCreditsViewDisplayed;
	private GameObject selectCardTypePopUp;
	private bool isSelectCardTypePopUpDisplayed;

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
					this.slideRight();
				}
			}
			else if(Input.touches[0].deltaPosition.x>15f && Mathf.Abs(Input.touches[0].deltaPosition.y)<Mathf.Abs(Input.touches[0].deltaPosition.x))
			{
				if(this.mainContentDisplayed || this.toSlideRight)
				{
					this.slideLeft();
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
			Vector3 upperCameraPosition = this.upperScrollCamera.transform.position;
			Vector3 mediumCameraPosition = this.mediumScrollCamera.transform.position;
			Vector3 lowerCameraPosition = this.lowerScrollCamera.transform.position;
			float camerasXPosition = upperCameraPosition.x;
			float lowerCameraXPosition = upperCameraPosition.x;
			if(toSlideRight)
			{
				camerasXPosition=camerasXPosition+Time.deltaTime*40f;
				if(camerasXPosition>this.mainContentPositionX)
				{
					camerasXPosition=this.mainContentPositionX;
					this.toSlideRight=false;
					this.mainContentDisplayed=true;
					lowerCameraPosition.y=this.lowerScrollCameraStandardPosition.y;
					lowerCameraXPosition=this.lowerScrollCameraStandardPosition.x;
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
					lowerCameraXPosition=camerasXPosition;
				}
			}
			upperCameraPosition.x=camerasXPosition;
			mediumCameraPosition.x=camerasXPosition;
			lowerCameraPosition.x=lowerCameraXPosition;
			this.upperScrollCamera.transform.position=upperCameraPosition;
			this.mediumScrollCamera.transform.position=mediumCameraPosition;
			this.lowerScrollCamera.transform.position=lowerCameraPosition;
		}
		if(ApplicationDesignRules.isMobileScreen && this.isSceneLoaded && this.mainContentDisplayed)
		{
			isScrolling = this.mediumScrollCamera.GetComponent<ScrollingController>().ScrollController();
		}
	}
	void Awake()
	{
		instance = this;
		this.model = new NewStoreModel ();
		this.speed = 300.0f;
		this.scrollIntersection = 1.5f;
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

		this.packsPaginationButtons = GameObject.Find("Pagination");
		this.packsPaginationButtons.AddComponent<NewStorePaginationController> ();
		this.packsPaginationButtons.GetComponent<NewStorePaginationController> ().initialize ();
		this.packsPaginationLine = GameObject.Find ("PacksPaginationLine");
		this.packsPaginationLine.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.bottomPacksScrollLine = GameObject.Find ("BottomPacksScrollLine");
		this.topPacksScrollLine = GameObject.Find ("TopPacksScrollLine");
		this.bottomPacksScrollLine.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.topPacksScrollLine.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;

		this.informationButton = GameObject.Find ("InformationButton");
		this.informationButton.AddComponent<NewStoreInformationButtonController> ();
		this.slideRightButton = GameObject.Find ("SlideRightButton");
		this.slideRightButton.AddComponent<NewStoreSlideRightButtonController> ();

		this.backButton = GameObject.Find ("BackButton");
		this.backButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "Retour aux packs";
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
		this.buyCreditsButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "Acheter des crédits";
		this.buyCreditsButton.AddComponent<NewStoreBuyCreditsButtonController> ();

		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardStoreController> ();
		this.focusedCard.SetActive (false);
		this.mainCamera = gameObject;
		this.sceneCamera = GameObject.Find ("sceneCamera");
		this.tutorialCamera = GameObject.Find ("TutorialCamera");
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
		this.lowerScrollCamera = GameObject.Find ("LowerScrollCamera");
		this.mediumScrollCamera = GameObject.Find ("MediumScrollCamera");
		this.mediumScrollCamera.AddComponent<ScrollingController> ();
		this.upperScrollCamera = GameObject.Find ("UpperScrollCamera");
		this.selectCardTypePopUp = GameObject.Find ("SelectCardTypePopUp");
		this.selectCardTypePopUp.SetActive (false);
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
		float packsBlockLeftMargin;
		float packsBlockUpMargin;
		float packsBlockHeight;
		
		float storeBlockLeftMargin;
		float storeBlockUpMargin;
		float storeBlockHeight;
		
		float buyCreditsBlockLeftMargin;
		float buyCreditsBlockUpMargin;
		float buyCreditsBlockHeight;

		buyCreditsBlockHeight=ApplicationDesignRules.smallBlockHeight;

		this.packsPagination = new Pagination ();

		this.mainCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.mainCamera.transform.position = ApplicationDesignRules.mainCameraPosition;
		this.sceneCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.tutorialCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.tutorialCamera.transform.position = ApplicationDesignRules.tutorialCameraPositiion;
		this.backgroundCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.backgroundCameraSize;
		this.backgroundCamera.transform.position = ApplicationDesignRules.backgroundCameraPosition;
		
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.packsPagination.nbElementsPerPage = 4;
			packsBlockHeight=10f+this.packsPagination.nbElementsPerPage*(ApplicationDesignRules.packWorldSize.y+ApplicationDesignRules.gapBetweenPacksLine);

			storeBlockHeight=ApplicationDesignRules.viewHeight;
			storeBlockLeftMargin=-ApplicationDesignRules.worldWidth;
			storeBlockUpMargin=0f;

			buyCreditsBlockLeftMargin=ApplicationDesignRules.worldWidth+ApplicationDesignRules.leftMargin;
			buyCreditsBlockUpMargin=0f;
			
			packsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			packsBlockUpMargin=0f;

			this.upperScrollCamera.GetComponent<Camera> ().rect = new Rect (0f,(ApplicationDesignRules.worldHeight-ApplicationDesignRules.upMargin-this.scrollIntersection)/ApplicationDesignRules.worldHeight,1f,(this.scrollIntersection)/ApplicationDesignRules.worldHeight);
			this.upperScrollCamera.GetComponent<Camera> ().orthographicSize = this.scrollIntersection/2f;
			this.upperScrollCamera.transform.position = new Vector3 (0f, ApplicationDesignRules.worldHeight/2f-(this.scrollIntersection/2f), -10f);
			
			this.mediumScrollCamera.SetActive(true);
			this.mediumScrollCamera.GetComponent<Camera> ().rect = new Rect (0f,(ApplicationDesignRules.downMargin+buyCreditsBlockHeight)/ApplicationDesignRules.worldHeight,1f,(ApplicationDesignRules.viewHeight-this.scrollIntersection-buyCreditsBlockHeight)/ApplicationDesignRules.worldHeight);
			this.mediumScrollCamera.GetComponent<Camera> ().orthographicSize = (ApplicationDesignRules.viewHeight-this.scrollIntersection-buyCreditsBlockHeight)/2f;
			this.mediumScrollCamera.GetComponent<ScrollingController> ().setViewHeight(ApplicationDesignRules.viewHeight-this.scrollIntersection-buyCreditsBlockHeight);
			this.mediumScrollCamera.transform.position = new Vector3 (0f, ApplicationDesignRules.worldHeight/2f-this.scrollIntersection-(ApplicationDesignRules.viewHeight-this.scrollIntersection-buyCreditsBlockHeight)/2f, -10f);
			this.mediumScrollCamera.GetComponent<ScrollingController> ().setStartPositionY (this.mediumScrollCamera.transform.position.y);
			
			this.lowerScrollCamera.SetActive(true);
			this.lowerScrollCamera.GetComponent<Camera>().rect=new Rect(0f,(ApplicationDesignRules.downMargin)/ApplicationDesignRules.worldHeight,1f,(buyCreditsBlockHeight)/ApplicationDesignRules.worldHeight);
			this.lowerScrollCamera.GetComponent<Camera> ().orthographicSize = (buyCreditsBlockHeight)/2f;
			this.lowerScrollCamera.transform.position=new Vector3(ApplicationDesignRules.worldWidth,ApplicationDesignRules.worldHeight/2f-buyCreditsBlockUpMargin-buyCreditsBlockHeight/2f,-10f);
			this.lowerScrollCameraStandardPosition=this.lowerScrollCamera.transform.position;
			this.lowerScrollCameraStorePosition=new Vector3(-ApplicationDesignRules.worldWidth,-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.upMargin+ApplicationDesignRules.downMargin+buyCreditsBlockHeight/2f,-10f);

			this.packsPaginationLine.SetActive(false);
			this.topPacksScrollLine.SetActive(true);
			this.bottomPacksScrollLine.SetActive(true);
			this.slideRightButton.SetActive(true);
			this.informationButton.SetActive(true);
			this.toSlideLeft=false;
			this.toSlideRight=false;
			this.mainContentDisplayed=true;

			if(isCardFocusedDisplayed)
			{
				this.lowerScrollCamera.SetActive(false);
				this.mediumScrollCamera.SetActive(false);
				this.upperScrollCamera.SetActive(false);
				this.sceneCamera.SetActive(true);
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraFocusedCardPosition;
			}
			else if(areRandomCardsGenerated)
			{
				this.lowerScrollCamera.SetActive(false);
				this.mediumScrollCamera.SetActive(false);
				this.upperScrollCamera.SetActive(false);
				this.sceneCamera.SetActive(true);
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraRandomCardsPosition;
			}
			else
			{
				this.lowerScrollCamera.SetActive(true);
				this.mediumScrollCamera.SetActive(true);
				this.upperScrollCamera.SetActive(true);
				this.sceneCamera.SetActive(false);
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraStandardPosition;
			}
		}
		else
		{
			packsBlockHeight=ApplicationDesignRules.largeBlockHeight;

			storeBlockHeight=ApplicationDesignRules.mediumBlockHeight;
			storeBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			storeBlockUpMargin=ApplicationDesignRules.upMargin;
			
			buyCreditsBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			buyCreditsBlockUpMargin=storeBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+storeBlockHeight;
			
			packsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			packsBlockUpMargin=ApplicationDesignRules.upMargin;

			this.packsPaginationLine.SetActive(true);
			this.topPacksScrollLine.SetActive(false);
			this.bottomPacksScrollLine.SetActive(false);
			this.packsPagination.nbElementsPerPage = 2;
			this.lowerScrollCamera.SetActive(false);
			this.upperScrollCamera.SetActive(false);
			this.mediumScrollCamera.SetActive(false);
			this.sceneCamera.SetActive(true);
			this.slideRightButton.SetActive(false);
			this.informationButton.SetActive(false);
			
			if(isCardFocusedDisplayed)
			{
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraFocusedCardPosition;
			}
			else if(areRandomCardsGenerated)
			{
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraRandomCardsPosition;
			}
			else
			{
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraStandardPosition;
			}
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

		float upperMargin = 1.6f;
		float lowerMargin = 0.9f;
		float lineScale = ApplicationDesignRules.getLineScale (packsBlockSize.x - 0.6f);
		float gapBetweenPacks=ApplicationDesignRules.gapBetweenPacksLine;

		for(int i=0;i<this.packsPagination.nbElementsPerPage;i++)
		{
			this.packs[i]=Instantiate (this.packObject) as GameObject;
			this.packs[i].transform.position=new Vector3(packsBlockOrigin.x,packsBlockUpperLeftPosition.y-upperMargin-ApplicationDesignRules.packWorldSize.y/2f-i*(ApplicationDesignRules.packWorldSize.y+gapBetweenPacks),0f);
			this.packs[i].AddComponent<NewPackStoreController>();
			this.packs[i].GetComponent<NewPackStoreController>().setId(i);
			this.packs[i].GetComponent<NewPackStoreController>().resize();
		}

		this.packsPaginationButtons.transform.GetComponent<NewStorePaginationController> ().resize ();

		this.informationButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.informationButton.transform.position = new Vector3 (packsBlockUpperRightPosition.x - 0.3f - ApplicationDesignRules.roundButtonWorldSize.x / 2f, packsBlockUpperRightPosition.y - 0.3f - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

		this.storeBlock.GetComponent<NewBlockController> ().resize(storeBlockLeftMargin,storeBlockUpMargin,ApplicationDesignRules.blockWidth,storeBlockHeight);
		Vector3 storeBlockUpperLeftPosition = this.storeBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 storeBlockUpperRightPosition = this.storeBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector3 storeBlockLowerLeftPosition = this.storeBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 storeBlockSize = this.storeBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 storeBlockOrigin = this.storeBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.storeBlockTitle.transform.position = new Vector3 (storeBlockUpperLeftPosition.x + 0.3f, storeBlockUpperLeftPosition.y - 0.2f, 0f);
		this.storeBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.storeSubtitle.transform.position = new Vector3 (storeBlockUpperLeftPosition.x + 0.3f, storeBlockUpperLeftPosition.y - 1.2f, 0f);
		this.storeSubtitle.transform.GetComponent<TextContainer>().width=storeBlockSize.x-0.6f;
		this.storeSubtitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		this.slideRightButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.slideRightButton.transform.position = new Vector3 (storeBlockLowerLeftPosition.x + storeBlockSize.x / 2f, storeBlockLowerLeftPosition.y + 0.1f + ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

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

		this.backButton.transform.position = new Vector3 (ApplicationDesignRules.randomCardsPosition.x, ApplicationDesignRules.sceneCameraRandomCardsPosition.y-ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.downMargin+ApplicationDesignRules.button62WorldSize.y, 0f);
		this.backButton.transform.localScale = ApplicationDesignRules.button62Scale;

		this.bottomPacksScrollLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.topPacksScrollLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.bottomPacksScrollLine.transform.position = new Vector3 (buyCreditsBlockUpperLeftPosition.x + buyCreditsBlockSize.x / 2, buyCreditsBlockUpperLeftPosition.y-0.03f, 0f);
		this.topPacksScrollLine.transform.position = new Vector3 (packsBlockUpperLeftPosition.x + packsBlockSize.x / 2f, packsBlockUpperLeftPosition.y - this.scrollIntersection + 0.03f, 0f);

		this.mainContentPositionX = packsBlockOrigin.x;
		this.storePositionX=storeBlockOrigin.x;

		this.packsPaginationLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.packsPaginationLine.transform.position = new Vector3 (packsBlockLowerLeftPosition.x + packsBlockSize.x / 2, packsBlockLowerLeftPosition.y + 0.6f, 0f);

		this.focusedCard.transform.localScale = ApplicationDesignRules.cardFocusedScale;
		this.focusedCard.transform.position = ApplicationDesignRules.focusedCardPosition;
		this.focusedCard.GetComponent<NewFocusedCardStoreController> ().resize ();

		if(ApplicationDesignRules.isMobileScreen)
		{
			this.packsPaginationButtons.transform.localPosition= new Vector3 (packsBlockUpperRightPosition.x - 0.3f - 3.5f*ApplicationDesignRules.roundButtonWorldSize.x, packsBlockUpperRightPosition.y - 0.3f - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);
		}
		else
		{
			this.packsPaginationButtons.transform.localPosition=new Vector3(packsBlockLowerLeftPosition.x+packsBlockSize.x/2f, packsBlockLowerLeftPosition.y + 0.3f, 0f);
		}
		if(areRandomCardsGenerated)
		{
			this.resizeRandomCards();
		}
		if(isSelectCardTypePopUpDisplayed)
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
		float height = ApplicationDesignRules.worldHeight - ApplicationDesignRules.upMargin - ApplicationDesignRules.downMargin-2f*ApplicationDesignRules.button62WorldSize.y;
		float gapBetweenCards = 0.2f;
		float cardWorldWidth = 0;
		float cardWorldHeight = 0;
		float nbLines = 0;
		int elementPerLine = model.packList [this.selectedPackIndex].NbCards;

		while(cardWorldWidth<ApplicationDesignRules.cardWorldSize.x)
		{
			nbLines++;
			elementPerLine=Mathf.CeilToInt((float)model.packList [this.selectedPackIndex].NbCards/(float)nbLines);
			cardWorldWidth = (width-(((float)elementPerLine+1)*gapBetweenCards))/(float)elementPerLine;
		}

		cardWorldHeight = cardWorldWidth * (ApplicationDesignRules.getCardOriginalSize().y / ApplicationDesignRules.getCardOriginalSize().x);
		                               
		if((float)nbLines*cardWorldHeight+((float)nbLines+1)*gapBetweenCards>height)
		{
			cardWorldHeight=(height-((float)nbLines+1)*ApplicationDesignRules.gapBetweenMarketCardsLine)/(float)nbLines;
			cardWorldWidth=cardWorldHeight * (ApplicationDesignRules.getCardOriginalSize().x / ApplicationDesignRules.getCardOriginalSize().y);
		}

		float scaleCard = cardWorldWidth / (ApplicationDesignRules.getCardOriginalSize().x/ApplicationDesignRules.pixelPerUnit);
		Vector3 scale = new Vector3 (scaleCard, scaleCard, scaleCard);
		Vector3 position=new Vector3(0,0,0);
		int column;
		int line;
		int elementOnCurrentLine;

		for (int i = 0; i <model.packList [this.selectedPackIndex].NbCards; i++)
		{
			line = Mathf.FloorToInt(i/elementPerLine);
			column = i%elementPerLine;
			if(line+1<nbLines)
			{
				elementOnCurrentLine=elementPerLine;
			}
			else
			{
				elementOnCurrentLine=model.packList [this.selectedPackIndex].NbCards%elementPerLine;
				if(elementOnCurrentLine==0)
				{
					elementOnCurrentLine=elementPerLine;
				}
			}
			position.x=ApplicationDesignRules.randomCardsPosition.x-((float)elementOnCurrentLine*cardWorldWidth+((float)elementOnCurrentLine+1f)*gapBetweenCards)/2f+((float)column+0.5f)*cardWorldWidth+((float)column+1f)*gapBetweenCards;
			position.y=ApplicationDesignRules.randomCardsPosition.y+ApplicationDesignRules.button62WorldSize.y+((float)nbLines*cardWorldHeight+((float)nbLines+1)*gapBetweenCards)/2f-((float)line+0.5f)*cardWorldHeight-((float)line+1f)*gapBetweenCards;
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
		if(value)
		{
			if(ApplicationDesignRules.isMobileScreen)
			{
				this.lowerScrollCamera.SetActive(true);
				this.mediumScrollCamera.SetActive(true);
				this.upperScrollCamera.SetActive(true);
				this.sceneCamera.SetActive(false);
			}
			this.sceneCamera.transform.position=ApplicationDesignRules.sceneCameraStandardPosition;
		}
		else
		{
			if(ApplicationDesignRules.isMobileScreen)
			{
				this.lowerScrollCamera.SetActive(false);
				this.mediumScrollCamera.SetActive(false);
				this.upperScrollCamera.SetActive(false);
				this.sceneCamera.SetActive(true);
			}
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
				this.packs[i].GetComponent<NewPackStoreController>().show(model.packList[this.packsDisplayed[i]]);
				this.packs[i].SetActive(true);
			}
			else
			{
				this.packs[i].SetActive(false);
			}
		}
		if(ApplicationDesignRules.isMobileScreen)
		{
			int nbLinesToDisplay = this.packsDisplayed.Count;
			float contentHeight = nbLinesToDisplay*(ApplicationDesignRules.packWorldSize.y+ApplicationDesignRules.gapBetweenPacksLine);
			if(this.mediumScrollCamera.GetComponent<ScrollingController>().getViewHeight()>contentHeight)
			{
				contentHeight=this.mediumScrollCamera.GetComponent<ScrollingController>().getViewHeight()+0.7f;
			}
			this.mediumScrollCamera.GetComponent<ScrollingController> ().setContentHeight(contentHeight);
			this.mediumScrollCamera.GetComponent<ScrollingController>().setEndPositionY();
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
	public void buyPackHandler(int id)
	{
		bool fromHome=false;
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
	public void buyPackHandler(int id, bool fromHome)
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
	public void buyPackWidthCardTypeHandler(int id)
	{
		this.selectedCardType=id;
		this.hideSelectCardPopUp();
		StartCoroutine(this.buyPack());	
	}
	public IEnumerator buyPack()
	{
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.buyPack (this.selectedPackIndex, this.selectedCardType, TutorialObjectController.instance.getIsTutorialLaunched()));
		MenuController.instance.hideLoadingScreen ();
		if(model.Error=="")
		{
			this.randomCardsDisplayed = new bool[model.packList [this.selectedPackIndex].NbCards];
			this.randomCards = new GameObject[model.packList [this.selectedPackIndex].NbCards];
			this.toRotate=new bool[model.packList [this.selectedPackIndex].NbCards];
			if(model.packList[this.selectedPackIndex].NbCards>1)
			{
				this.backButton.SetActive(true);
				this.createRandomCards();
				this.resizeRandomCards();
				this.rotateRandomCards();
				this.displayBackUI(false);
				this.displayRandomCards();
				if(this.model.CollectionPointsEarned>0)
				{
					MenuController.instance.displayCollectionPointsPopUp(model.CollectionPointsEarned,model.CollectionPointsRanking);
				}
				if(this.model.NewSkills.Count>0)
				{
					MenuController.instance.displayNewSkillsPopUps(model.NewSkills);
				}
				TutorialObjectController.instance.tutorialTrackPoint ();
			}
			else
			{
				this.createSingleCard();
				this.rotateSingleCard();
				this.displayBackUI(false);
				this.displayCardFocused();
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
			this.displayRandomCards();
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
		this.focusedCard.GetComponent<NewFocusedCardStoreController> ().displayFocusFeatures (true);
		this.focusedCard.GetComponent<NewFocusedCardStoreController> ().c = model.packList [this.selectedPackIndex].Cards.getCard (this.clickedCardId);
		this.focusedCard.GetComponent<NewFocusedCardController> ().show ();
		this.displayCardFocused ();
	}
	public void displayCardFocused()
	{
		this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraFocusedCardPosition;
	}
	public void displayRandomCards()
	{
		this.sceneCamera.transform.position=ApplicationDesignRules.sceneCameraRandomCardsPosition;
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
			this.randomCards[this.clickedCardId].SetActive(false);
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
	public void backOfficeBackgroundClicked()
	{
		if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().escapePressed();
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
		else if(this.isSelectCardTypePopUpDisplayed)
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
		else if(this.isSelectCardTypePopUpDisplayed)
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
				this.packs[i].GetComponent<NewPackStoreController>().activeButton(false);
			}
			else
			{
				this.packs[i].GetComponent<NewPackStoreController>().activeButton(true);
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
		MenuController.instance.displayTransparentBackground ();
		this.selectCardTypePopUp.transform.GetComponent<SelectCardTypePopUpController> ().reset (model.player.CardTypesAllowed);
		this.isSelectCardTypePopUpDisplayed = true;
		this.selectCardTypePopUp.SetActive (true);
		this.selectCardPopUpResize ();
	}
	public void addCreditsPopUpResize()
	{
		addCreditsView.popUpVM.centralWindow = this.centralWindow;
		addCreditsView.popUpVM.resize ();
	}
	private void selectCardPopUpResize()
	{
		this.selectCardTypePopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.selectCardTypePopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.selectCardTypePopUp.GetComponent<SelectCardTypePopUpController> ().resize ();
	}
	public void hideSelectCardPopUp()
	{
		this.selectCardTypePopUp.SetActive (false);
		MenuController.instance.hideTransparentBackground();
		this.isSelectCardTypePopUpDisplayed = false;
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
	public void slideLeft()
	{
		if(this.mainContentDisplayed)
		{
			this.mediumScrollCamera.GetComponent<ScrollingController>().reset();
			this.lowerScrollCamera.transform.position=this.lowerScrollCameraStorePosition;
		}
		this.toSlideLeft=true;
		this.toSlideRight=false;
		this.mainContentDisplayed=false;
	}
	public void slideRight()
	{
		this.toSlideRight=true;
		this.toSlideLeft=false;
		this.storeDisplayed=false;
	}
	public Camera returnCurrentCamera()
	{
		return this.sceneCamera.GetComponent<Camera>();
	}
	#region TUTORIAL FUNCTIONS

	public Vector3 returnBuyPackButtonPosition(int id)
	{
		return this.packs [id].GetComponent<NewPackStoreController> ().getBuyButtonPosition ();
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
