using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine.Purchasing;
using TMPro;
using Xsolla;

public class NewStoreController : MonoBehaviour, IStoreListener
{
	public static NewStoreController instance;
	private NewStoreModel model;

	private IStoreController m_StoreController;
	private IExtensionProvider m_StoreExtensionProvider;

	public GameObject cardObject;
	public GameObject packObject;
	public GameObject blockObject;
	public GUISkin popUpSkin;
	public Sprite[] productsIcons;

	private GameObject backOfficeController;
	private GameObject menu;
	private GameObject help;

	private GameObject packsBlock;
	private GameObject packsBlockTitle;
	private GameObject[] packs;
	private GameObject packsPaginationButtons;
	private GameObject packsPaginationLine;
	private GameObject packsNumberTitle;
	private GameObject bottomPacksScrollLine;
	private GameObject topPacksScrollLine;

	private GameObject[] products;
	private GameObject productsPaginationButtons;

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
	private GameObject helpCamera;
	private GameObject backgroundCamera;

	private Vector3 lowerScrollCameraStandardPosition;
	private Vector3 lowerScrollCameraStorePosition;

	private IList<int> packsDisplayed;
	private IList<int> productsDisplayed;
	
	private GameObject productsPopUp;
	private bool isProductsPopUpDisplayed;
	private GameObject selectCardTypePopUp;
	private bool isSelectCardTypePopUpDisplayed;

	private Pagination packsPagination;
	private Pagination productsPagination;

	private int selectedPackIndex;
	private int selectedCardType;
	private int nbCards;
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
		if (Input.touchCount == 1 && this.isSceneLoaded  && HelpController.instance.getCanSwipe() && BackOfficeController.instance.getCanSwipeAndScroll()) 
		{
			if(Input.touches[0].deltaPosition.x<-15f && Mathf.Abs(Input.touches[0].deltaPosition.y)<Mathf.Abs(Input.touches[0].deltaPosition.x))
			{
				if(this.storeDisplayed || this.toSlideLeft)
				{
					this.slideRight();
					BackOfficeController.instance.setIsSwiping(true);
				}
			}
			else if(Input.touches[0].deltaPosition.x>15f && Mathf.Abs(Input.touches[0].deltaPosition.y)<Mathf.Abs(Input.touches[0].deltaPosition.x))
			{
				if(this.mainContentDisplayed || this.toSlideRight)
				{
					this.slideLeft();
					BackOfficeController.instance.setIsSwiping(true);
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
						if(i<this.nbCards-1)
						{
							this.toRotate[i+1]=true;
							if(!isCardFocusedDisplayed)
							{
								SoundController.instance.playSound(7);
							}
						}
						else
						{
							this.startRotation=false;
							if(this.nbCards==1)
							{
								this.randomCards[i].GetComponent<NewFocusedCardStoreController>().displayFocusFeatures(true);
							}
							HelpController.instance.tutorialTrackPoint();
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
					BackOfficeController.instance.setIsSwiping(false);
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
					BackOfficeController.instance.setIsSwiping(false);
				}
			}
			upperCameraPosition.x=camerasXPosition;
			mediumCameraPosition.x=camerasXPosition;
			lowerCameraPosition.x=lowerCameraXPosition;
			this.upperScrollCamera.transform.position=upperCameraPosition;
			this.mediumScrollCamera.transform.position=mediumCameraPosition;
			this.lowerScrollCamera.transform.position=lowerCameraPosition;
		}
		if(ApplicationDesignRules.isMobileScreen && this.isSceneLoaded && this.mainContentDisplayed && HelpController.instance.getCanScroll() && BackOfficeController.instance.getCanSwipeAndScroll())
		{
			isScrolling = this.mediumScrollCamera.GetComponent<ScrollingController>().ScrollController();
		}
	}
	void Awake()
	{
		instance = this;
		this.model = new NewStoreModel ();
		this.speed = 300.0f;
		this.scrollIntersection = 1.2f;
		this.mainContentDisplayed = true;
		this.initializeScene ();
		this.initializeBackOffice();
		this.initializeMenu();
		this.initializeHelp();
		StartCoroutine (this.initialization ());
	}
	private void initializeHelp()
	{
		this.help = GameObject.Find ("HelpController");
		this.help.AddComponent<StoreHelpController>();
		this.help.GetComponent<StoreHelpController>().initialize();
		BackOfficeController.instance.setIsHelpLoaded(true);
	}
	private void initializeMenu()
	{
		this.menu = GameObject.Find ("Menu");
		this.menu.AddComponent<MenuController>();
		this.menu.GetComponent<MenuController>().initialize();
		BackOfficeController.instance.setIsMenuLoaded(true);
	}
	private void initializeBackOffice()
	{
		this.backOfficeController = GameObject.Find ("BackOfficeController");
		this.backOfficeController.AddComponent<BackOfficeStoreController>();
		this.backOfficeController.GetComponent<BackOfficeStoreController>().initialize();
	}
	private void initializeScene()
	{
		this.packsBlock = Instantiate (this.blockObject) as GameObject;
		this.packsBlockTitle = GameObject.Find ("PacksBlockTitle");
		this.packsBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.packsBlockTitle.GetComponent<TextMeshPro> ().text = WordingStore.getReference(0);
		this.packsNumberTitle = GameObject.Find ("PacksNumberTitle");
		this.packsNumberTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;

		this.packs = new GameObject[0];
		this.products = new GameObject[4];

		for(int i=0;i<this.products.Length;i++)
		{
			this.products[i]=GameObject.Find("product"+i);
		}

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
		this.backButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = WordingStore.getReference(1);
		this.backButton.AddComponent<NewStoreBackButtonController> ();
		this.backButton.SetActive (false);

		this.storeBlock = Instantiate (this.blockObject) as GameObject;
		this.storeBlockTitle = GameObject.Find ("StoreBlockTitle");
		this.storeBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.storeBlockTitle.GetComponent<TextMeshPro> ().text = WordingStore.getReference(2);
		this.storeSubtitle = GameObject.Find ("StoreSubtitle");
		this.storeSubtitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.storeSubtitle.GetComponent<TextMeshPro> ().text = WordingStore.getReference(3);

		this.buyCreditsBlock = Instantiate (this.blockObject) as GameObject;
		this.buyCreditsSubtitle = GameObject.Find ("BuyCreditsSubtitle");
		this.buyCreditsSubtitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.buyCreditsSubtitle.GetComponent<TextMeshPro> ().text = WordingStore.getReference(4);
		this.buyCreditsBlockTitle = GameObject.Find ("BuyCreditsBlockTitle");
		this.buyCreditsBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.buyCreditsBlockTitle.GetComponent<TextMeshPro> ().text = WordingStore.getReference(5);
		this.buyCreditsButton = GameObject.Find ("BuyCreditsButton");
		this.buyCreditsButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = WordingStore.getReference(6);
		this.buyCreditsButton.AddComponent<NewStoreBuyCreditsButtonController> ();

		this.productsPaginationButtons = GameObject.Find("ProductsPagination");
		this.productsPaginationButtons.AddComponent<NewStoreProductsPaginationController> ();
		this.productsPaginationButtons.GetComponent<NewStoreProductsPaginationController> ().initialize ();

		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardStoreController> ();
		this.focusedCard.SetActive (false);
		this.mainCamera = gameObject;
		this.sceneCamera = GameObject.Find ("sceneCamera");
		this.helpCamera = GameObject.Find ("HelpCamera");
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
		this.lowerScrollCamera = GameObject.Find ("LowerScrollCamera");
		this.mediumScrollCamera = GameObject.Find ("MediumScrollCamera");
		this.mediumScrollCamera.AddComponent<ScrollingController> ();
		this.upperScrollCamera = GameObject.Find ("UpperScrollCamera");
		this.selectCardTypePopUp = GameObject.Find ("SelectCardTypePopUp");
		this.selectCardTypePopUp.SetActive (false);
		this.productsPopUp = GameObject.Find ("ProductsPopUp");
		this.productsPopUp.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingProducts.getReferences(1);
		this.productsPopUp.SetActive (false);
	}
	private IEnumerator initialization()
	{
		this.resize ();
		BackOfficeController.instance.displayLoadingScreen ();
		yield return(StartCoroutine(this.model.initializeStore()));
		if(ApplicationDesignRules.isMobileDevice)
		{
			if (m_StoreController == null)
	       	{
	       		InitializeMobilePurchasing();
	        }
		}
		else
		{
			this.initializeDesktopPurchasing();
		}
		this.initializePacks ();
		BackOfficeController.instance.hideLoadingScreen ();
		this.isSceneLoaded = true;
		if(ApplicationModel.player.HasToBuyTrainingPack)
		{
			this.buyPackHandler(-1,false,true);
			ApplicationModel.player.HasToBuyTrainingPack=false;
		}
		else if(ApplicationModel.player.PackToBuy!=-1)
		{
			this.buyPackHandler(ApplicationModel.player.PackToBuy,true,false);
			ApplicationModel.player.PackToBuy=-1;
		}
		else if(ApplicationModel.player.TutorialStep==4)
		{
			HelpController.instance.startTutorial();
		}
	}
	public void paginationHandler()
	{
		SoundController.instance.playSound(9);
		this.drawPaginationNumber();
		this.drawPacks();
	}
	public void paginationProductsHandler()
	{
		SoundController.instance.playSound(9);
		this.drawProducts();
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
	public void initializeProducts()
	{
		this.productsPagination.chosenPage = 0;
		this.productsPagination.totalElements = model.productList.Count;
		this.productsPaginationButtons.GetComponent<NewStoreProductsPaginationController> ().p = productsPagination;
		this.productsPaginationButtons.GetComponent<NewStoreProductsPaginationController> ().setPagination ();
		this.drawProducts ();
	}
	public void drawPaginationNumber()
	{
		if(packsPagination.totalElements>0)
		{
			this.packsNumberTitle.GetComponent<TextMeshPro>().text=(WordingPagination.getReference(6) +this.packsPagination.elementDebut+WordingPagination.getReference(1)+this.packsPagination.elementFin+WordingPagination.getReference(2)+this.packsPagination.totalElements ).ToUpper();
		}
		else
		{
			this.packsNumberTitle.GetComponent<TextMeshPro>().text=WordingPagination.getReference(7).ToUpper();
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

		this.packsPagination = new Pagination ();
		this.productsPagination = new Pagination();
		this.productsPagination.nbElementsPerPage=4;
		this.mainCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.mainCamera.transform.position = ApplicationDesignRules.mainCameraPosition;
		this.sceneCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.helpCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.helpCamera.transform.position = ApplicationDesignRules.helpCameraPositiion;
		this.backgroundCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.backgroundCameraSize;
		this.backgroundCamera.transform.position = ApplicationDesignRules.backgroundCameraPosition;
		this.backgroundCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		this.helpCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		this.sceneCamera.GetComponent<Camera> ().rect = new Rect (0f,0f,1f,1f);
		this.mainCamera.GetComponent<Camera>().rect= new Rect (0f,0f,1f,1f);
		
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.packsPagination.nbElementsPerPage = 4;
			packsBlockHeight=10f+this.packsPagination.nbElementsPerPage*(ApplicationDesignRules.packWorldSize.y+ApplicationDesignRules.gapBetweenPacksLine);

			storeBlockHeight=ApplicationDesignRules.viewHeight;
			storeBlockLeftMargin=-ApplicationDesignRules.worldWidth;
			storeBlockUpMargin=0f;

			buyCreditsBlockHeight=2.5f;
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

			buyCreditsBlockHeight=ApplicationDesignRules.smallBlockHeight;
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

		this.packsBlock.GetComponent<NewBlockController> ().resize(packsBlockLeftMargin,packsBlockUpMargin,ApplicationDesignRules.blockWidth,packsBlockHeight);
		Vector3 packsBlockUpperLeftPosition = this.packsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 packsBlockLowerLeftPosition = this.packsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector3 packsBlockUpperRightPosition = this.packsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 packsBlockSize = this.packsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector3 packsBlockOrigin = this.packsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.packsBlockTitle.transform.position = new Vector3 (packsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, packsBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.packsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		this.packsNumberTitle.transform.position = new Vector3 (packsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, packsBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
		this.packsNumberTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		this.packs = new GameObject[packsPagination.nbElementsPerPage];

		float lineScale = ApplicationDesignRules.getLineScale (packsBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing);
		float gapBetweenPacks=ApplicationDesignRules.gapBetweenPacksLine;

		for(int i=0;i<this.packsPagination.nbElementsPerPage;i++)
		{
			this.packs[i]=Instantiate (this.packObject) as GameObject;
			this.packs[i].transform.position=new Vector3(packsBlockOrigin.x,packsBlockUpperLeftPosition.y-ApplicationDesignRules.subMainTitleVerticalSpacing-0.4f-ApplicationDesignRules.packWorldSize.y/2f-i*(ApplicationDesignRules.packWorldSize.y+gapBetweenPacks),0f);
			this.packs[i].AddComponent<NewPackStoreController>();
			this.packs[i].GetComponent<NewPackStoreController>().setId(i);
			this.packs[i].GetComponent<NewPackStoreController>().resize();
		}

		this.packsPaginationButtons.transform.GetComponent<NewStorePaginationController> ().resize ();

		this.informationButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.informationButton.transform.position = new Vector3 (packsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f, packsBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

		this.storeBlock.GetComponent<NewBlockController> ().resize(storeBlockLeftMargin,storeBlockUpMargin,ApplicationDesignRules.blockWidth,storeBlockHeight);
		Vector3 storeBlockUpperLeftPosition = this.storeBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 storeBlockUpperRightPosition = this.storeBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 storeBlockSize = this.storeBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 storeBlockOrigin = this.storeBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.storeBlockTitle.transform.position = new Vector3 (storeBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, storeBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.storeBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.storeSubtitle.transform.position = new Vector3 (storeBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, storeBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
		this.storeSubtitle.transform.GetComponent<TextContainer>().width=storeBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing;
		this.storeSubtitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		this.slideRightButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.slideRightButton.transform.position = new Vector3 (storeBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x/2f, storeBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

		this.buyCreditsBlock.GetComponent<NewBlockController> ().resize(buyCreditsBlockLeftMargin,buyCreditsBlockUpMargin,ApplicationDesignRules.blockWidth,buyCreditsBlockHeight);
		Vector3 buyCreditsBlockUpperLeftPosition = this.buyCreditsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector2 buyCreditsBlockSize = this.buyCreditsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector3 buyCreditsOrigin = this.buyCreditsBlock.GetComponent<NewBlockController> ().getOriginPosition ();

		this.buyCreditsBlockTitle.transform.position = new Vector3 (buyCreditsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, buyCreditsBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.buyCreditsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.buyCreditsSubtitle.transform.position = new Vector3 (buyCreditsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, buyCreditsBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
		this.buyCreditsSubtitle.transform.GetComponent<TextContainer>().width=buyCreditsBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing;
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
			this.packsPaginationButtons.transform.localPosition=new Vector3 (packsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 2f*ApplicationDesignRules.roundButtonWorldSize.x, packsBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);
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
		if(isProductsPopUpDisplayed)
		{
			this.productsPopUpResize();
		}
		MenuController.instance.resize();
		MenuController.instance.setCurrentPage(2);
		MenuController.instance.refreshMenuObject();
		HelpController.instance.resize();
	}
	public void createRandomCards()
	{
		this.areRandomCardsGenerated=true;
		for (int i = 0; i <this.nbCards; i++)
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
		float gapBetweenCards = 0.1f;
		float cardWorldWidth = 0f;
		float cardWorldHeight = 0f;
		float nbLines = 0f;
		int elementPerLine = this.nbCards;

		while(cardWorldWidth<ApplicationDesignRules.cardWorldSize.x)
		{
			nbLines++;
			elementPerLine=Mathf.CeilToInt((float)this.nbCards/(float)nbLines);
			cardWorldWidth = (width-(((float)elementPerLine+1f)*gapBetweenCards))/(float)elementPerLine;
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

		for (int i = 0; i <this.nbCards; i++)
		{
			line = Mathf.FloorToInt(i/elementPerLine);
			column = i%elementPerLine;
			if(line+1<nbLines)
			{
				elementOnCurrentLine=elementPerLine;
			}
			else
			{
				elementOnCurrentLine=this.nbCards%elementPerLine;
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
		for (int i = 0; i <this.nbCards; i++)
		{
			this.toRotate[i]=false;
			this.randomCards[i].transform.rotation=Quaternion.Euler(0, 180, 0);
			this.randomCardsDisplayed[i]=true;
		}
		this.startRotation = true;
		this.toRotate [0] = true;
		this.angle = 180;
		SoundController.instance.playSound(7);
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
		SoundController.instance.playSound(7);
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
		SoundController.instance.stopPlayingSound();
		SoundController.instance.playSound(9);
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
	public void drawProducts()
	{
		this.productsDisplayed = new List<int> ();
		
		for(int i=0;i<this.productsPagination.nbElementsPerPage;i++)
		{
			if(this.productsPagination.chosenPage*(this.productsPagination.nbElementsPerPage)+i<model.productList.Count)
			{
				this.productsDisplayed.Add (this.productsPagination.chosenPage*(this.productsPagination.nbElementsPerPage)+i);
				this.products[i].GetComponent<ProductsPopUpProductController>().show(model.productList[this.productsDisplayed[i]]);
				this.products[i].SetActive(true);
			}
			else
			{
				this.products[i].SetActive(false);
			}
		}
	}
	public void cleanPacks()
	{
		for(int i=0;i<this.packs.Length;i++)
		{
			Destroy(this.packs[i]);
		}
	}
	public void buyPackHandler(int id, bool fromHome, bool trainingPack)
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
			this.selectedCardType = model.packList [this.selectedPackIndex].CardType;
		}
		if(!trainingPack)
		{
			if(!fromHome)
			{
				this.selectedPackIndex = this.packsDisplayed [id];
			}
			this.selectedCardType = model.packList [this.selectedPackIndex].CardType;
			this.nbCards=model.packList [this.selectedPackIndex].NbCards;
		}
		if(trainingPack)
		{
			this.selectedCardType=ApplicationModel.player.TrainingAllowedCardType;
			this.nbCards=ApplicationModel.nbCardsByDeck;
			StartCoroutine (this.buyPack ());
		}
		else if(this.selectedCardType==-2)
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
		SoundController.instance.playSound(11);
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.buyPack (this.selectedPackIndex, this.selectedCardType, HelpController.instance.getIsTutorialLaunched(),ApplicationModel.player.HasToBuyTrainingPack));
		BackOfficeController.instance.hideLoadingScreen ();
		if(model.Error=="")
		{
			this.randomCardsDisplayed = new bool[this.nbCards];
			this.randomCards = new GameObject[this.nbCards];
			this.toRotate=new bool[this.nbCards];
			if(this.nbCards>1)
			{
				this.backButton.SetActive(true);
				this.createRandomCards();
				this.resizeRandomCards();
				this.rotateRandomCards();
				this.displayBackUI(false);
				this.displayRandomCards();
				HelpController.instance.tutorialTrackPoint ();
			}
			else
			{
				this.createSingleCard();
				this.rotateSingleCard();
				this.displayBackUI(false);
				this.displayCardFocused();
			}
			if(!HelpController.instance.getIsTutorialLaunched())
			{
				if(this.model.CollectionPointsEarned>0)
				{
					BackOfficeController.instance.displayCollectionPointsPopUp(model.CollectionPointsEarned,model.CollectionPointsRanking);
				}
				if(this.model.NewSkills.Count>0)
				{
					BackOfficeController.instance.displayNewSkillsPopUps(model.NewSkills);
				}
			}
			StartCoroutine(BackOfficeController.instance.getUserData ());
		}
		else
		{
			BackOfficeController.instance.displayErrorPopUp(model.Error);
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
		HelpController.instance.tutorialTrackPoint();
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
		SoundController.instance.stopPlayingSound();
		SoundController.instance.playSound(4);
	}
	public void displayCardFocused()
	{
		this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraFocusedCardPosition;
		HelpController.instance.tutorialTrackPoint();
	}
	public void displayRandomCards()
	{
		this.sceneCamera.transform.position=ApplicationDesignRules.sceneCameraRandomCardsPosition;
	}
	public int getCardTypeId(int value)
	{
		return ApplicationModel.player.CardTypesAllowed [value];
	}
	public void deleteCard()
	{
		if(this.areRandomCardsGenerated)
		{
			StartCoroutine(BackOfficeController.instance.getUserData ());
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
		else if(this.isProductsPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideProductsPopUp();
		}
		else if(this.isSelectCardTypePopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideSelectCardPopUp();
		}
		else
		{
			BackOfficeController.instance.leaveGame();
		}
	}
	public void closeAllPopUp()
	{
		if(this.isProductsPopUpDisplayed)
		{
			this.hideProductsPopUp();
		}
		else if(this.isSelectCardTypePopUpDisplayed)
		{
			this.hideSelectCardPopUp();
		}
	}
	public void updatePackPrices()
	{
		for(int i=0;i<this.packsDisplayed.Count;i++)
		{
			if(ApplicationModel.player.Money<model.packList[this.packsDisplayed[i]].Price)
			{
				this.packs[i].GetComponent<NewPackStoreController>().activeButton(false);
			}
			else
			{
				this.packs[i].GetComponent<NewPackStoreController>().activeButton(true);
			}
		}
	}
	public void displayProductsPopUp()
	{
		SoundController.instance.playSound(9);
		BackOfficeController.instance.displayTransparentBackground ();
		this.isProductsPopUpDisplayed = true;
		this.productsPopUp.SetActive (true);
		this.productsPopUpResize ();
		this.productsPopUp.transform.FindChild("closebutton").GetComponent<ProductsPopUpCloseButtonController>().reset();
		this.productsPopUp.transform.FindChild("ProductsPagination").GetComponent<NewStoreProductsPaginationController>().reset();
		for(int i=0;i<productsPagination.nbElementsPerPage;i++)
		{
			this.productsPopUp.transform.FindChild("product"+i).GetComponent<ProductsPopUpProductController>().reset();
		}
		if(this.productsPagination.chosenPage!=0)
		{
			this.initializeProducts();	
		}
	}
	public void displaySelectCardTypePopUp()
	{
		SoundController.instance.playSound(9);
		BackOfficeController.instance.displayTransparentBackground ();
		this.isSelectCardTypePopUpDisplayed = true;
		this.selectCardTypePopUp.SetActive (true);
		this.selectCardTypePopUp.transform.GetComponent<SelectCardTypePopUpController> ().reset (ApplicationModel.player.CardTypesAllowed);
		this.selectCardPopUpResize ();
	}
	public void productsPopUpResize()
	{
		this.productsPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.productsPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.productsPopUp.transform.FindChild("ProductsPagination").GetComponent<NewStoreProductsPaginationController>().resize();
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
		BackOfficeController.instance.hideTransparentBackground();
		this.isSelectCardTypePopUpDisplayed = false;
	}
	public void hideProductsPopUp()
	{
		this.productsPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isProductsPopUpDisplayed = false;
	}
//	public void addCreditsHandler()
//	{
//		int tempInt = addCreditsSyntaxCheck ();
//		if(tempInt!=-1)
//		{
//			StartCoroutine (addCredits (tempInt));
//		}
//	}
//	public IEnumerator addCredits(int value)
//	{
//		this.hideAddCreditsPopUp ();
//		BackOfficeController.instance.displayLoadingScreen ();
//		yield return StartCoroutine (ApplicationModel.player.addMoney (value));
//		StartCoroutine(BackOfficeController.instance.getUserData ());
//		BackOfficeController.instance.hideLoadingScreen ();
//	}
//	public int addCreditsSyntaxCheck()
//	{
//		int n;
//		string credits = this.addCreditsPopUp.transform.GetComponent<AddCreditsPopUpController> ().getFirstInput ();
//		bool isNumeric = int.TryParse(credits, out n);
//		if(credits!="" && isNumeric)
//		{
//			if(System.Convert.ToInt32(credits)>0)
//			{
//				return System.Convert.ToInt32(credits);
//			}
//		}
//		this.addCreditsPopUp.transform.GetComponent<AddCreditsPopUpController>().setError(WordingStore.getReference(7));
//		return -1;
//	}
	public void moneyUpdate()
	{
		if(isSceneLoaded)
		{
			if(isCardFocusedDisplayed)
			{
				this.focusedCard.GetComponent<NewFocusedCardStoreController>().updateFocusFeatures();
			}
			else
			{
				this.updatePackPrices();
			}
		}
	}
	public void slideLeft()
	{
		SoundController.instance.playSound(16);
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
		SoundController.instance.playSound(16);
		this.toSlideRight=true;
		this.toSlideLeft=false;
		this.storeDisplayed=false;
	}
	public Camera returnCurrentCamera()
	{
		return this.sceneCamera.GetComponent<Camera>();
	}
	public void InitializeMobilePurchasing() 
	{
	    if (IsInitialized())
	    {
	        return;
	    }
	    
	   	var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

	    for(int i=0;i<model.productList.Count;i++)
	    {
			builder.AddProduct(model.productList[i].ProductID, ProductType.Consumable, new IDs(){{ model.productList[i].ProductNameApple,AppleAppStore.Name },{ model.productList[i].ProductNameGooglePlay,GooglePlay.Name },});
	    }
	    UnityPurchasing.Initialize(this, builder);
	}
	private bool IsInitialized()
	{
   		return m_StoreController != null && m_StoreExtensionProvider != null;
	}
	public string getProductsPrice(int id)
	{
		Product product = m_StoreController.products.WithID(model.productList[this.productsDisplayed[id]].ProductID);
		if(product!=null)
		{
			return product.metadata.localizedPriceString;
		}
		else
		{
			return WordingProducts.getReferences(0);
		}
	}
	public void buyProductHandler(int id)
	{
		BackOfficeController.instance.displayLoadingScreen();
		this.BuyProductID(model.productList[this.productsDisplayed[id]].ProductID);
		this.hideProductsPopUp();
	}
	void BuyProductID(string productId)
	{
	    // If the stores throw an unexpected exception, use try..catch to protect my logic here.
	    try
	    {
	        // If Purchasing has been initialized ...
	        if (IsInitialized())
	        {
	            // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
	            Product product = m_StoreController.products.WithID(productId);

	            // If the look up found a product for this device's store and that product is ready to be sold ... 
	            if (product != null && product.availableToPurchase)
	            {
	                Debug.Log (string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
	                m_StoreController.InitiatePurchase(product);
	            }
	            // Otherwise ...
	            else
	            {
	                // ... report the product look-up failure situation  
	                Debug.Log ("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
	            }
	        }
	        // Otherwise ...
	        else
	        {
	            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
	            Debug.Log("BuyProductID FAIL. Not initialized.");
	        }
	    }
	    // Complete the unexpected exception handling ...
	    catch (Exception e)
	    {
	        // ... by reporting any unexpected exception for later diagnosis.
	        Debug.Log ("BuyProductID: FAIL. Exception during purchase. " + e);
	    }
	}
	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
	    // Purchasing has succeeded initializing. Collect our Purchasing references.
	    Debug.Log("OnInitialized: PASS");
	    
	    // Overall Purchasing system, configured with products for this application.
	    m_StoreController = controller;
	    // Store specific subsystem, for accessing device-specific store features.
	    m_StoreExtensionProvider = extensions;
		this.initializeProducts();
	}
	public void OnInitializeFailed(InitializationFailureReason error)
	{
	    // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
	    Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
		this.initializeProducts();
	}
	public void runDesktopPurchasing()
	{
		XsollaSDK sdk = this.gameObject.GetComponent<XsollaSDK>();
		if(sdk!=null)
		{
			//XsollaJsonGenerator jsonGenerator = new XsollaJsonGenerator (ApplicationModel.player.Id.ToString(),17443);
			//jsonGenerator.settings.mode="sandbox";
			//jsonGenerator.settings.secretKey="m1WHb5qGb55B6eES";
			BackOfficeController.instance.displayTransparentBackground();
			sdk.CreatePaymentForm(ApplicationModel.player.DesktopPurchasingToken,Success,Failure);
		}
	}
	void Success (XsollaResult result)
	{
		Debug.Log("Sucess");
	}
	void Failure (XsollaError error)
	{
		Debug.Log("Failure");
	}
	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
	{
	    // A consumable product has been purchased by this user.

	    for(int i=0;i<model.productList.Count;i++)
	    {
			if (String.Equals(args.purchasedProduct.definition.id, model.productList[i].ProductID, StringComparison.Ordinal))
		    {
		        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));//If the consumable item has been successfully purchased, add 100 coins to the player's in-game score.
		        StartCoroutine	(ApplicationModel.player.addMoney((int)model.productList[i].Crystals));
    		}
	    }
	    BackOfficeController.instance.hideLoadingScreen();
	    return PurchaseProcessingResult.Complete;
	} 
	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
	    // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
	    Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",product.definition.storeSpecificId, failureReason));
	    BackOfficeController.instance.hideLoadingScreen();
	}
	public void initializeDesktopPurchasing()
	{
		StartCoroutine(getPurchasingToken());
	}
	public IEnumerator getPurchasingToken()
	{
		yield return StartCoroutine(ApplicationModel.player.getPurchasingToken());
		print(ApplicationModel.player.DesktopPurchasingToken);
	}
	  
	#region TUTORIAL FUNCTIONS

	public GameObject returnStoreBlock()
	{
		return this.storeBlock;
	}
	public GameObject returnBuyCreditsBlock()
	{
		return this.buyCreditsBlock;
	}
	public GameObject returnPacksBlock()
	{
		return this.packsBlock;
	}
	public Vector3 returnBuyPackButtonPosition(int id)
	{
		return this.packs [id].GetComponent<NewPackStoreController> ().getBuyButtonPosition ();
	}
	public Vector3 getCardsPosition(int id)
	{
		return new Vector3(-ApplicationDesignRules.randomCardsPosition.x+this.randomCards[id].transform.position.x,-ApplicationDesignRules.randomCardsPosition.y+this.randomCards[id].transform.position.y,this.randomCards[id].transform.position.z);
	}
	public Vector3 getFocusedCardPosition()
	{
		return new Vector3(-ApplicationDesignRules.focusedCardPosition.x+this.focusedCard.transform.FindChild("Card").position.x,-ApplicationDesignRules.focusedCardPosition.y+this.focusedCard.transform.FindChild("Card").position.y,this.focusedCard.transform.FindChild("Card").position.z); 
	}
	public Vector2 getCardsSize(int id)
	{
		return new Vector2((ApplicationDesignRules.getCardOriginalSize().x/ApplicationDesignRules.pixelPerUnit)*this.randomCards[id].transform.localScale.x,(ApplicationDesignRules.getCardOriginalSize().y/ApplicationDesignRules.pixelPerUnit)*this.randomCards[id].transform.localScale.y);
	}
	public GameObject returnCard(int id)
	{
		return this.randomCards[id];
	}
	public bool getIsCardFocusedDisplayed()
	{
		return isCardFocusedDisplayed;
	}
	public bool getAreRandomCardsDisplayed()
	{
		return areRandomCardsGenerated;
	}
	public GameObject returnCardFocused()
	{
		return this.focusedCard;
	}
	public Vector3 getMediumScrollCameraPosition()
	{
		return this.mediumScrollCamera.transform.position;
	}
	public bool getIsMainContentDisplayed()
	{
		return this.mainContentDisplayed;
	}
	public void resetScrolling()
	{
		this.mediumScrollCamera.GetComponent<ScrollingController>().reset();	
	}
	public bool getIsScrolling()
	{
		return this.isScrolling;
	}
	#endregion
}
