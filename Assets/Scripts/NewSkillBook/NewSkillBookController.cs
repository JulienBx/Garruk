using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Text;
using System.Globalization;

public class NewSkillBookController : MonoBehaviour
{
	public static NewSkillBookController instance;
	private NewSkillBookModel model;
	
	public GameObject blockObject;
	public GameObject skillObject;
	public GameObject contentObject;

	private GameObject backOfficeController;
	private GameObject menu;
	private GameObject tutorial;

	private GameObject skillsBlock;
	private GameObject skillsBlockTitle;
	private GameObject[] skills;
	private GameObject skillsNumberTitle;
	private GameObject skillsScrollLine;
	private GameObject skillsPaginationLine;
	private GameObject skillsPaginationButtons;
	
	private GameObject helpBlock;
	private GameObject helpLine;
	private GameObject helpSubtitle;
	private GameObject helpIndicatorsContent;
	private GameObject[] tabs;
	private GameObject[] contents;
	private GameObject helpPaginationButtons;
	private GameObject[] stats;
	private GameObject[] helpContents;

	private GameObject filtersBlock;
	private GameObject filtersBlockTitle;
	private GameObject[] cardsTypeFilters;
	private GameObject[] skillTypeFilters;
	private GameObject[] availableFilters;
	private GameObject cardTypeFilterTitle;
	private GameObject skillTypeFilterTitle;
	private GameObject availabilityFilterTitle;
	private GameObject skillSearchBarTitle;
	private GameObject skillSearchBar;
	private GameObject[] skillChoices;

	private GameObject mainCamera;
	private GameObject lowerScrollCamera;
	private GameObject upperScrollCamera;
	private GameObject sceneCamera;
	private GameObject tutorialCamera;
	private GameObject backgroundCamera;

	private GameObject informationButton;
	private GameObject filterButton;
	private GameObject slideRightButton;
	private GameObject slideLeftButton;

	private GameObject focusedSkill;
	private GameObject helpBlockTitle;
	
	private IList<int> skillsDisplayed;
	private IList<int> skillsToBeDisplayed;
	private IList<int> cardsTypesDisplayed;
	private IList<int> skillsTypesDisplayed;

	private Pagination skillsPagination;
	private Pagination helpPagination;

	private int activeTab;
	private int selectedCardTypeId;

	private IList<int> filtersCardType;
	private IList<int> filtersSkillType;
	private bool isOwnFilterOn;
	private bool isNotOwnFilterOn;
	private bool isSearchingSkill;
	private bool isSkillChosen;
	private string valueSkill;
	private IList<int> skillsChoiceDisplayed;

	private int globalPercentage;
	private int[] skillsPercentages;
	private int[] skillsNbCards;
	private int[] cardTypesNbSkillsOwn;
	private int[] cardTypesNbSkills;
	private int[] cardTypesNbCards;

	private bool isSceneLoaded;
	private bool isScrolling;
	private float scrollIntersection;

	private bool toSlideLeft;
	private bool toSlideRight;
	private bool filtersDisplayed;
	private bool mainContentDisplayed;
	private bool helpContentDisplayed;
	
	private float filtersPositionX;
	private float mainContentPositionX;
	private float helpContentPositionX;
	private float targetContentPositionX;

	private bool isFocusedSkillDisplayed;
	public bool isLeftClicked;

	void Update()
	{
		if (Input.touchCount == 1 && this.isSceneLoaded && !this.isFocusedSkillDisplayed && TutorialObjectController.instance.getCanSwipe() && BackOfficeController.instance.getCanSwipeAndScroll()) 
		{
			if(Mathf.Abs(Input.touches[0].deltaPosition.y)>1f && Mathf.Abs(Input.touches[0].deltaPosition.y)>Mathf.Abs(Input.touches[0].deltaPosition.x))
			{
				this.isLeftClicked=false;
			}
			else if(Input.touches[0].deltaPosition.x<-15f )
			{
				this.isLeftClicked=false;
				if(this.helpContentDisplayed || this.mainContentDisplayed || this.toSlideLeft)
				{
					slideRight();
				}
			}
			else if(Input.touches[0].deltaPosition.x>15f)
			{
				this.isLeftClicked=false;
				if(this.mainContentDisplayed || this.filtersDisplayed || this.toSlideRight)
				{
					slideLeft();
				}
			}
		}
		if(isSearchingSkill)
		{
			if(this.skillSearchBar.GetComponent<NewSkillBookSkillSearchBarController>().getInputText().ToLower()!=this.valueSkill.ToLower())
			{
				this.valueSkill=this.skillSearchBar.GetComponent<NewSkillBookSkillSearchBarController>().getInputText();
				this.setSkillAutocompletion();
			}
		}
		if(toSlideRight || toSlideLeft)
		{
			Vector3 mainCameraPosition = this.upperScrollCamera.transform.position;
			Vector3 cardsCameraPosition = this.lowerScrollCamera.transform.position;
			float camerasXPosition = mainCameraPosition.x;
			if(toSlideRight)
			{
				camerasXPosition=camerasXPosition+Time.deltaTime*40f;
				if(camerasXPosition>this.targetContentPositionX)
				{
					camerasXPosition=this.targetContentPositionX;
					this.toSlideRight=false;
					if(camerasXPosition==this.filtersPositionX)
					{
						this.filtersDisplayed=true;
					}
					else if(camerasXPosition==this.mainContentPositionX)
					{
						this.mainContentDisplayed=true;
					}
				}
			}
			else if(toSlideLeft)
			{
				camerasXPosition=camerasXPosition-Time.deltaTime*40f;
				if(camerasXPosition<this.targetContentPositionX)
				{
					camerasXPosition=this.targetContentPositionX;
					this.toSlideLeft=false;
					if(camerasXPosition==this.helpContentPositionX)
					{
						this.helpContentDisplayed=true;
					}
					else if(camerasXPosition==this.mainContentPositionX)
					{
						this.mainContentDisplayed=true;
					}
				}
			}
			mainCameraPosition.x=camerasXPosition;
			cardsCameraPosition.x=camerasXPosition;
			this.upperScrollCamera.transform.position=mainCameraPosition;
			this.lowerScrollCamera.transform.position=cardsCameraPosition;
		}
		if(ApplicationDesignRules.isMobileScreen && this.isSceneLoaded && this.mainContentDisplayed && !this.isFocusedSkillDisplayed && TutorialObjectController.instance.getCanScroll() && BackOfficeController.instance.getCanSwipeAndScroll())
		{
			isScrolling = this.lowerScrollCamera.GetComponent<ScrollingController>().ScrollController();
		}
	}
	void Awake()
	{
		instance = this;
		this.activeTab = 0;
		this.model = new NewSkillBookModel ();
		this.selectedCardTypeId = 0;
		this.scrollIntersection = 1.18f;
		this.mainContentDisplayed = true;
		this.initializeScene ();
		this.initializeBackOffice();
		this.initializeMenu();
		this.initializeTutorial();
		StartCoroutine (this.initialization ());
	}
	private void initializeTutorial()
	{
		this.tutorial = GameObject.Find ("Tutorial");
		this.tutorial.AddComponent<SkillBookTutorialController>();
		this.tutorial.GetComponent<SkillBookTutorialController>().initialize();
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
		this.backOfficeController.AddComponent<BackOfficeSkillBookController>();
		this.backOfficeController.GetComponent<BackOfficeSkillBookController>().initialize();
	}
	public IEnumerator initialization()
	{
		this.resize ();
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.getSkillBookData ());
		this.computeIndicators ();
		this.selectATab ();
		this.initializeFilters ();
		this.initializeSkills ();
		BackOfficeController.instance.hideLoadingScreen ();
		this.isSceneLoaded = true;
		if(ApplicationModel.player.TutorialStep!=-1)
		{
			TutorialObjectController.instance.startTutorial();
		}
		else if(ApplicationModel.player.DisplayTutorial&&!ApplicationModel.player.SkillBookTutorial)
		{
			TutorialObjectController.instance.startHelp();
		}
	}
	public void selectATabHandler(int idTab)
	{
		this.activeTab = idTab;
		this.selectATab ();
	}
	private void selectATab()
	{
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.hideActiveTab();
		}
		for(int i=0;i<this.tabs.Length;i++)
		{
			if(i==this.activeTab)
			{
				this.tabs[i].GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnTabPicture(1);
				this.tabs[i].GetComponent<NewSkillBookTabController>().setIsSelected(true);
				this.tabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			}
			else
			{
				this.tabs[i].GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnTabPicture(0);
				this.tabs[i].GetComponent<NewSkillBookTabController>().reset();
			}
		}
		this.initializeTabContents ();
	}
	public void initializeTabContents()
	{
		switch(this.activeTab)
		{
		case 0:
			this.initializeHelp();
			break;
		case 1:
			this.initializeIndicators();
			break;
		case 2:
			this.initializeSkillsTypes();
			break;
		case 3:
			this.initializeCardsTypes();
			break;
		}
	}
	public void paginationHelpHandler()
	{
		switch(this.activeTab)
		{
		case 2:
			this.drawSkillsTypes();
			break;
		case 3:
			this.drawCardsTypes();
			break;
		}
	}
	public void paginationSkillHandler()
	{
		this.drawSkillsPaginationNumber ();
		this.drawSkills ();
		if(ApplicationDesignRules.isMobileScreen)
		{
			Vector3 cardsCameraPosition = this.lowerScrollCamera.transform.position;
			cardsCameraPosition.y=this.lowerScrollCamera.GetComponent<ScrollingController>().getStartPositionY();
			this.lowerScrollCamera.transform.position=cardsCameraPosition;
		}
	}
	private void initializeHelp()
	{
		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i].SetActive(false);
		}
		for(int i=0;i<this.stats.Length;i++)
		{
			this.stats[i].SetActive(false);
		}
		this.helpPaginationButtons.SetActive (false);
		this.drawMainHelp ();
	}
	private void initializeIndicators()
	{
		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i].SetActive(false);
		}
		for(int i=0;i<this.helpContents.Length;i++)
		{
			this.helpContents[i].SetActive(false);
		}
		this.helpPaginationButtons.SetActive (false);
		this.drawIndicators ();
	}
	private void initializeSkillsTypes()
	{
		this.helpPaginationButtons.SetActive (true);
		this.helpPagination.chosenPage = 0;
		this.helpPagination.totalElements= model.skillTypesList.Count;
		this.helpPaginationButtons.GetComponent<NewSkillBookHelpPaginationController> ().p = this.helpPagination;
		this.helpPaginationButtons.GetComponent<NewSkillBookHelpPaginationController> ().setPagination ();
		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i].SetActive(true);
			this.contents[i].transform.FindChild("cardTypePicture").gameObject.SetActive(false);
			this.contents[i].transform.FindChild("skillTypePicture").gameObject.SetActive(true);
		}
		for(int i=0;i<this.stats.Length;i++)
		{
			this.stats[i].SetActive(false);
		}
		for(int i=0;i<this.helpContents.Length;i++)
		{
			this.helpContents[i].SetActive(false);
		}
		this.helpSubtitle.SetActive (false);
		this.helpLine.SetActive (false);
		this.drawSkillsTypes ();
	}
	private void initializeCardsTypes()
	{
		this.helpPaginationButtons.SetActive (true);
		this.helpPagination.chosenPage = 0;
		this.helpPagination.totalElements= model.cardTypesList.Count;
		this.helpPaginationButtons.GetComponent<NewSkillBookHelpPaginationController> ().p = this.helpPagination;
		this.helpPaginationButtons.GetComponent<NewSkillBookHelpPaginationController> ().setPagination ();
		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i].SetActive(true);
			this.contents[i].transform.FindChild("cardTypePicture").gameObject.SetActive(true);
			this.contents[i].transform.FindChild("skillTypePicture").gameObject.SetActive(false);
		}
		for(int i=0;i<this.stats.Length;i++)
		{
			this.stats[i].SetActive(false);
		}
		for(int i=0;i<this.helpContents.Length;i++)
		{
			this.helpContents[i].SetActive(false);
		}
		this.helpSubtitle.SetActive (false);
		this.helpLine.SetActive (false);
		this.drawCardsTypes ();
	}
	public void initializeSkills()
	{
		this.resetFiltersValue ();
		this.skillsPagination.chosenPage = 0;
		this.applyFilters ();
	}
	private void initializeFilters()
	{
		for(int i=0;i<this.skillTypeFilters.Length;i++)
		{
			this.skillTypeFilters[i].GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnSkillTypePicture(i);
		}
	}
	private void applyFilters()
	{
		this.computeFilters ();
		this.skillsPagination.totalElements= this.skillsToBeDisplayed.Count;
		this.skillsPaginationButtons.GetComponent<NewSkillBookSkillsPaginationController> ().p = skillsPagination;
		this.skillsPaginationButtons.GetComponent<NewSkillBookSkillsPaginationController> ().setPagination ();
		this.drawSkillsPaginationNumber ();
		this.drawSkills ();
	}
	public void drawSkillsPaginationNumber()
	{
		if(skillsPagination.totalElements>0)
		{
			this.skillsNumberTitle.GetComponent<TextMeshPro>().text=(WordingPagination.getReference(4) +this.skillsPagination.elementDebut+WordingPagination.getReference(1)+this.skillsPagination.elementFin+WordingPagination.getReference(2)+this.skillsPagination.totalElements ).ToUpper();
		}
		else
		{
			this.skillsNumberTitle.GetComponent<TextMeshPro>().text=WordingPagination.getReference(5).ToUpper();
		}
	}
	public void initializeScene()
	{
		this.skillsBlock = Instantiate (this.blockObject) as GameObject;
		this.skillsBlockTitle = GameObject.Find ("SkillsBlockTitle");
		this.skillsBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.skillsBlockTitle.GetComponent<TextMeshPro> ().text = WordingSkillBook.getReference(0);
		this.skills=new GameObject[0];
		this.skillsPaginationButtons = GameObject.Find ("SkillsPagination");
		this.skillsPaginationButtons.AddComponent<NewSkillBookSkillsPaginationController> ();
		this.skillsPaginationButtons.GetComponent<NewSkillBookSkillsPaginationController> ().initialize ();
		this.skillsPaginationLine = GameObject.Find ("SkillsPaginationLine");
		this.skillsPaginationLine.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.skillsNumberTitle = GameObject.Find ("SkillsNumberTitle");
		this.skillsNumberTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.skillsScrollLine = GameObject.Find ("SkillsScrollLine");
		this.skillsScrollLine.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;

		this.helpBlock = Instantiate (this.blockObject) as GameObject;
		this.helpLine = GameObject.Find ("HelpLine");
		this.tabs=new GameObject[4];
		for(int i=0;i<this.tabs.Length;i++)
		{
			this.tabs[i]=GameObject.Find ("Tab"+i);
			this.tabs[i].AddComponent<NewSkillBookTabController>();
			this.tabs[i].GetComponent<NewSkillBookTabController>().setId(i);
		}
		this.tabs[0].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = WordingSkillBook.getReference(1);
		this.tabs[1].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = WordingSkillBook.getReference(2);
		this.tabs[2].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = WordingSkillBook.getReference(3);
		this.tabs[3].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingSkillBook.getReference(4);
		this.helpBlockTitle = GameObject.Find ("HelpBlockTitle");
		this.helpBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.helpBlockTitle.GetComponent<TextMeshPro> ().text = "Compétences";
		this.helpSubtitle = GameObject.Find ("HelpSubtitle");
		this.helpSubtitle.transform.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.contents=new GameObject[0];
		this.helpContents=new GameObject[3];
		for(int i=0;i<this.helpContents.Length;i++)
		{
			this.helpContents[i]=GameObject.Find("HelpContent"+i);
			this.helpContents[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
		this.helpContents [0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text =  WordingSkillBook.getReference(5);
		this.helpContents [1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text =  WordingSkillBook.getReference(6);
		this.helpContents [2].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text =  WordingSkillBook.getReference(7);
		this.helpContents [0].transform.FindChild("Picture").FindChild("Title").GetComponent<TextMeshPro> ().text =  WordingSkills.getName(2);
		this.helpContents [1].transform.FindChild("Picture").FindChild("Title").GetComponent<TextMeshPro> ().text =  WordingSkills.getName(72);
		this.helpContents [2].transform.FindChild("Picture").FindChild("Title").GetComponent<TextMeshPro> ().text =  WordingSkills.getName(3);

		this.helpPaginationButtons = GameObject.Find ("HelpPagination");
		this.helpPaginationButtons.AddComponent<NewSkillBookHelpPaginationController> ();
		this.helpPaginationButtons.GetComponent<NewSkillBookHelpPaginationController> ().initialize ();

		this.stats = new GameObject[4];
		for(int i=0;i<this.stats.Length;i++)
		{
			this.stats[i]=GameObject.Find ("Stat"+i);
			this.stats[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			// A compléter !
		}
		this.stats[0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text=  WordingSkillBook.getReference(8);
		this.stats[1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text=  WordingSkillBook.getReference(9);
		this.stats[2].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text=  WordingSkillBook.getReference(10);
		this.stats[3].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text=  WordingSkillBook.getReference(11);

		this.filtersBlock = Instantiate (this.blockObject) as GameObject;
		this.filtersBlockTitle = GameObject.Find ("FiltersBlockTitle");
		this.filtersBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.filtersBlockTitle.GetComponent<TextMeshPro> ().text = WordingFilters.getReference(0);
		this.cardsTypeFilters = new GameObject[10];
		for(int i=0;i<this.cardsTypeFilters.Length;i++)
		{
			this.cardsTypeFilters[i]=GameObject.Find("CardTypeFilter"+i);
			this.cardsTypeFilters[i].AddComponent<NewSkillBookCardTypeFilterController>();
			this.cardsTypeFilters[i].GetComponent<NewSkillBookCardTypeFilterController>().setId(i);
		}
		this.skillTypeFilters = new GameObject[7];
		for(int i=0;i<this.skillTypeFilters.Length;i++)
		{
			this.skillTypeFilters[i]=GameObject.Find("SkillTypeFilter"+i);
			this.skillTypeFilters[i].AddComponent<NewSkillBookSkillTypeFilterController>();
			this.skillTypeFilters[i].GetComponent<NewSkillBookSkillTypeFilterController>().setId(i);
			this.skillTypeFilters[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSkillTypes.getLetter(i);
		}
		this.availableFilters = new GameObject[2];
		for (int i=0; i<this.availableFilters.Length; i++) 
		{
			this.availableFilters[i]=GameObject.Find("AvailableFilter"+i);
			this.availableFilters[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.availableFilters[i].AddComponent<NewSkillBookAvailabilityFilterController>();
			this.availableFilters[i].GetComponent<NewSkillBookAvailabilityFilterController>().setId(i);
		}
		this.availableFilters [0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text =  WordingFilters.getReference(9);
		this.availableFilters [1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text =  WordingFilters.getReference(10);
		this.skillSearchBarTitle = GameObject.Find ("SkillSearchTitle");
		this.skillSearchBarTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.skillSearchBarTitle.GetComponent<TextMeshPro> ().text =  WordingFilters.getReference(1).ToUpper ();
		this.skillSearchBar = GameObject.Find ("SkillSearchBar");
		this.skillSearchBar.GetComponent<NewSkillBookSkillSearchBarController> ().setButtonText ( WordingFilters.getReference(2));
		this.skillChoices=new GameObject[3];
		for(int i=0;i<this.skillChoices.Length;i++)
		{
			this.skillChoices[i]=GameObject.Find("SkillChoice"+i);
			this.skillChoices[i].AddComponent<NewSkillBookSkillChoiceController>();
			this.skillChoices[i].GetComponent<NewSkillBookSkillChoiceController>().setId(i);
			this.skillChoices[i].SetActive(false);
		}
		this.cardTypeFilterTitle = GameObject.Find ("CardTypeFilterTitle");
		this.cardTypeFilterTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.cardTypeFilterTitle.GetComponent<TextMeshPro> ().text =  WordingFilters.getReference(3).ToUpper ();
		this.skillTypeFilterTitle = GameObject.Find ("SkillTypeFilterTitle");
		this.skillTypeFilterTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.skillTypeFilterTitle.GetComponent<TextMeshPro> ().text = WordingFilters.getReference(11).ToUpper ();
		this.availabilityFilterTitle = GameObject.Find ("AvailabilityFilterTitle");
		this.availabilityFilterTitle.GetComponent<TextMeshPro> ().text = WordingFilters.getReference(12).ToUpper ();
		this.availabilityFilterTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.mainCamera = gameObject;
		this.sceneCamera = GameObject.Find ("sceneCamera");
		this.lowerScrollCamera = GameObject.Find ("LowerScrollCamera");
		this.lowerScrollCamera.AddComponent<ScrollingController> ();
		this.upperScrollCamera = GameObject.Find ("UpperScrollCamera");
		this.tutorialCamera = GameObject.Find ("TutorialCamera");
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
		this.filterButton = GameObject.Find ("FilterButton");
		this.filterButton.AddComponent<NewSkillBookFilterButtonController> ();
		this.informationButton = GameObject.Find ("InformationButton");
		this.informationButton.AddComponent<NewSkillBookInformationButtonController> ();
		this.slideLeftButton = GameObject.Find ("SlideLeftButton");
		this.slideLeftButton.AddComponent<NewSkillBookSlideLeftButtonController> ();
		this.slideRightButton = GameObject.Find ("SlideRightButton");
		this.slideRightButton.AddComponent<NewSkillBookSlideRightButtonController> ();
		this.focusedSkill = GameObject.Find ("FocusedSkill");
		this.focusedSkill.AddComponent<FocusedSkillControllerSkillBook> ();
	}
	public void resize()
	{
		float skillsBlockLeftMargin;
		float skillsBlockUpMargin;
		float skillsBlockHeight;
		
		float helpBlockLeftMargin;
		float helpBlockUpMargin;
		float helpBlockHeight;
		
		float filtersBlockLeftMargin;
		float filtersBlockUpMargin;
		float filtersBlockHeight;

		float firstLineSkills;
		float firstLineContents;
		float contentsHeight;
		float helpSubTitleFirstLine;
		float helpLinePositionY;
		float helpContentsFirstLine;
		float helpContentsHeight;

		this.mainCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.mainCamera.transform.position = ApplicationDesignRules.mainCameraPosition;
		this.sceneCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraStandardPosition;
		this.tutorialCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.tutorialCamera.transform.position = ApplicationDesignRules.tutorialCameraPositiion;
		this.backgroundCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.backgroundCameraSize;
		this.backgroundCamera.transform.position = ApplicationDesignRules.backgroundCameraPosition;
		this.backgroundCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		this.tutorialCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		this.sceneCamera.GetComponent<Camera> ().rect = new Rect (0f,0f,1f,1f);
		this.mainCamera.GetComponent<Camera>().rect= new Rect (0f,0f,1f,1f);

		this.skillsPagination = new Pagination ();
		this.skillsPagination.chosenPage = 0;
		this.helpPagination = new Pagination ();
		this.helpPagination.chosenPage = 0;
		
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.skillsPagination.nbElementsPerPage = 300;
			this.helpPagination.nbElementsPerPage = 6;
			skillsBlockHeight=2.1f+this.skillsPagination.nbElementsPerPage*(0.85f+ApplicationDesignRules.gapBetweenSkillsLine);

			firstLineContents=1f;
			contentsHeight=1f;
			helpSubTitleFirstLine=ApplicationDesignRules.subMainTitleVerticalSpacing;
			helpLinePositionY=2.2f;
			helpContentsFirstLine=2.2f;
			helpContentsHeight=1.5f;
			helpBlockHeight=ApplicationDesignRules.viewHeight-ApplicationDesignRules.tabWorldSize.y;
			helpBlockLeftMargin=-ApplicationDesignRules.worldWidth;
			helpBlockUpMargin=0f+ApplicationDesignRules.tabWorldSize.y;
			
			firstLineSkills=1.7f;
			skillsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			skillsBlockUpMargin=0f;
			
			filtersBlockHeight=ApplicationDesignRules.viewHeight;
			filtersBlockLeftMargin=ApplicationDesignRules.worldWidth+ApplicationDesignRules.leftMargin;
			filtersBlockUpMargin=0f;

			this.skillsScrollLine.SetActive(true);
			this.sceneCamera.SetActive(false);
			this.lowerScrollCamera.SetActive(true);
			this.upperScrollCamera.SetActive(true);
			this.toSlideLeft=false;
			this.toSlideRight=false;
			this.mainContentDisplayed=true;
			this.filterButton.SetActive(true);
			this.informationButton.SetActive(true);
			this.slideLeftButton.SetActive(true);
			this.slideRightButton.SetActive(true);
			this.skillsPaginationLine.SetActive(false);
			this.helpBlockTitle.SetActive(true);

			this.upperScrollCamera.GetComponent<Camera> ().rect = new Rect (0f,(ApplicationDesignRules.worldHeight-ApplicationDesignRules.upMargin-this.scrollIntersection)/ApplicationDesignRules.worldHeight,1f,(this.scrollIntersection)/ApplicationDesignRules.worldHeight);
			this.upperScrollCamera.GetComponent<Camera> ().orthographicSize = this.scrollIntersection/2f;
			this.upperScrollCamera.transform.position = new Vector3 (0f, ApplicationDesignRules.worldHeight/2f-this.scrollIntersection/2f, -10f);
			
			this.lowerScrollCamera.GetComponent<Camera> ().rect = new Rect (0f,(ApplicationDesignRules.downMargin)/ApplicationDesignRules.worldHeight,1f,(ApplicationDesignRules.viewHeight-this.scrollIntersection)/ApplicationDesignRules.worldHeight);
			this.lowerScrollCamera.GetComponent<Camera> ().orthographicSize = (ApplicationDesignRules.viewHeight-this.scrollIntersection)/2f;
			this.lowerScrollCamera.GetComponent<ScrollingController> ().setViewHeight(ApplicationDesignRules.viewHeight-this.scrollIntersection);
			this.lowerScrollCamera.transform.position = new Vector3 (0f, ApplicationDesignRules.worldHeight/2f-this.scrollIntersection-(ApplicationDesignRules.viewHeight-this.scrollIntersection)/2f, -10f);
			this.lowerScrollCamera.GetComponent<ScrollingController> ().setStartPositionY (this.lowerScrollCamera.transform.position.y);

			if(isFocusedSkillDisplayed)
			{
				this.lowerScrollCamera.SetActive(false);
				this.upperScrollCamera.SetActive(false);
				this.sceneCamera.SetActive(true);
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraFocusedCardPosition;
			}
			else
			{
				this.lowerScrollCamera.SetActive(true);
				this.upperScrollCamera.SetActive(true);
				this.sceneCamera.SetActive(false);
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraStandardPosition;
			}
		}
		else
		{
			firstLineContents=0.3f;
			contentsHeight=0.9f;
			helpLinePositionY=1.5f;
			helpContentsFirstLine=1.5f;
			helpContentsHeight=0.7f;
			helpSubTitleFirstLine=ApplicationDesignRules.mainTitleVerticalSpacing;
			helpBlockHeight=ApplicationDesignRules.mediumBlockHeight-ApplicationDesignRules.tabWorldSize.y;
			helpBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			helpBlockUpMargin=ApplicationDesignRules.upMargin+ApplicationDesignRules.tabWorldSize.y;
			
			filtersBlockHeight=ApplicationDesignRules.smallBlockHeight;
			filtersBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			filtersBlockUpMargin=helpBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+helpBlockHeight;

			firstLineSkills=2.1f;
			skillsBlockHeight=ApplicationDesignRules.largeBlockHeight;
			skillsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			skillsBlockUpMargin=ApplicationDesignRules.upMargin;

			this.skillsPagination.nbElementsPerPage = 5;
			this.skillsScrollLine.SetActive(false);
			this.helpPagination.nbElementsPerPage = 3;
			
			this.sceneCamera.SetActive(true);
			this.lowerScrollCamera.SetActive(false);
			this.upperScrollCamera.SetActive(false);
			this.skillsPaginationLine.SetActive(true);
			this.filterButton.SetActive(false);
			this.informationButton.SetActive(false);
			this.slideLeftButton.SetActive(false);
			this.slideRightButton.SetActive(false);
			this.helpBlockTitle.SetActive(false);

			if(isFocusedSkillDisplayed)
			{
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraFocusedCardPosition;
			}
			else
			{
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraStandardPosition;
			}
		}

		this.filtersBlock.GetComponent<NewBlockController> ().resize(filtersBlockLeftMargin,filtersBlockUpMargin,ApplicationDesignRules.blockWidth,filtersBlockHeight);
		Vector3 filtersBlockUpperLeftPosition = this.filtersBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 filtersBlockUpperRightPosition = this.filtersBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector3 filtersBlockLowerLeftPosition = this.filtersBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 filtersBlockSize = this.filtersBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 filtersBlockOrigin = this.filtersBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		
		float gapBetweenSubFiltersBlock = 0.05f;
		float filtersSubBlockSize = (filtersBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing - 2f * gapBetweenSubFiltersBlock) / 3f;
		
		this.filtersBlockTitle.transform.position = new Vector3 (filtersBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, filtersBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.filtersBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		
		this.cardTypeFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		this.availabilityFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		this.skillTypeFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.skillSearchBarTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.skillSearchBar.transform.localScale = ApplicationDesignRules.inputTextScale;

		this.skillsBlock.GetComponent<NewBlockController> ().resize(skillsBlockLeftMargin,skillsBlockUpMargin,ApplicationDesignRules.blockWidth,skillsBlockHeight);
		Vector3 skillsBlockUpperLeftPosition = this.skillsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 skillsBlockLowerLeftPosition = this.skillsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector3 skillsBlockUpperRightPosition = this.skillsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 skillsBlockSize = this.skillsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector3 skillsBlockOrigin = this.skillsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.skillsBlockTitle.transform.position = new Vector3 (skillsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, skillsBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.skillsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		this.skillsNumberTitle.transform.position = new Vector3 (skillsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, skillsBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
		this.skillsNumberTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		this.filterButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.filterButton.transform.position = new Vector3 (skillsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f, skillsBlockUpperRightPosition.y-ApplicationDesignRules.buttonVerticalSpacing-ApplicationDesignRules.roundButtonWorldSize.y/2f, 0f);
		
		this.informationButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.informationButton.transform.position = new Vector3 (skillsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 1.5f*ApplicationDesignRules.roundButtonWorldSize.x, skillsBlockUpperRightPosition.y-ApplicationDesignRules.buttonVerticalSpacing-ApplicationDesignRules.roundButtonWorldSize.y/2f, 0f);

		this.skills=new GameObject[this.skillsPagination.nbElementsPerPage];

		float skillWorldWidth = skillsBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing;
		float lineScale = ApplicationDesignRules.getLineScale (skillsBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing);

		for(int i=0;i<this.skills.Length;i++)
		{
			this.skills[i]=Instantiate (this.skillObject) as GameObject;
			this.skills[i].GetComponent<NewSkillBookSkillController>().initialize();
			this.skills[i].GetComponent<NewSkillBookSkillController>().setId(i);
			this.skills[i].transform.position=new Vector3(skillsBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+skillWorldWidth/2f,skillsBlockUpperLeftPosition.y-firstLineSkills-i*(0.85f+ApplicationDesignRules.gapBetweenSkillsLine),0f);
			this.skills[i].transform.GetComponent<NewSkillBookSkillController>().resize(skillWorldWidth);
		}

		this.skillsPaginationButtons.transform.GetComponent<NewSkillBookSkillsPaginationController> ().resize ();
		this.skillsPaginationLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.skillsPaginationLine.transform.position = new Vector3 (skillsBlockLowerLeftPosition.x + skillsBlockSize.x / 2, skillsBlockLowerLeftPosition.y + 0.6f, 0f);
		this.skillsScrollLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.skillsScrollLine.transform.position = new Vector3 (skillsBlockLowerLeftPosition.x + skillsBlockSize.x / 2, skillsBlockUpperLeftPosition.y-1.15f, 0f);

		this.helpBlock.GetComponent<NewBlockController> ().resize(helpBlockLeftMargin,helpBlockUpMargin, ApplicationDesignRules.blockWidth,helpBlockHeight);
		Vector3 helpBlockUpperLeftPosition = this.helpBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 helpBlockUpperRightPosition = this.helpBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 helpBlockLowerLeftPosition = this.helpBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 helpBlockLowerRightPosition = this.helpBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 helpBlockSize = this.helpBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 helpBlockOrigin = this.helpBlock.GetComponent<NewBlockController> ().getOriginPosition ();

		this.helpBlockTitle.transform.position = new Vector3 (helpBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, helpBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.helpBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		float gapBetweenSelectionsButtons = 0.02f;

		this.helpLine.transform.localPosition=new Vector3 (helpBlockUpperLeftPosition.x + helpBlockSize.x / 2, helpBlockUpperLeftPosition.y - helpLinePositionY, 0f);
		this.helpLine.transform.localScale = new Vector3 (lineScale,lineScale,lineScale);

		this.contents = new GameObject[this.helpPagination.nbElementsPerPage];
		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i]=Instantiate (this.contentObject) as GameObject;
			this.contents[i].transform.position=new Vector3(helpBlockUpperLeftPosition.x+helpBlockSize.x/2f,helpBlockUpperLeftPosition.y-firstLineContents-(i+1f)*contentsHeight,0f);
			this.contents[i].transform.FindChild("line").localScale=new Vector3(lineScale,1f,1f);
			this.contents[i].transform.FindChild("skillTypePicture").localScale=ApplicationDesignRules.skillTypeFilterScale;
			this.contents[i].transform.FindChild("skillTypePicture").localPosition=new Vector3(-helpBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(contentsHeight-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
			this.contents[i].transform.FindChild("cardTypePicture").localScale=ApplicationDesignRules.cardTypeFilterScale;
			this.contents[i].transform.FindChild("cardTypePicture").localPosition=new Vector3(-helpBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(contentsHeight-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
			this.contents[i].transform.FindChild("title").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.contents[i].transform.FindChild("title").GetComponent<TextMeshPro>().textContainer.width=(helpBlockSize.x/2f)-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.contents[i].transform.FindChild("title").localPosition=new Vector3(-helpBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,contentsHeight-(contentsHeight-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.contents[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=0.75f*helpBlockSize.x/2f-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.contents[i].transform.FindChild("description").localPosition=new Vector3(-helpBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,contentsHeight/2f,0f);
			this.contents[i].transform.FindChild("description").GetComponent<TextContainer>().width=(helpBlockSize.x-0.1f-ApplicationDesignRules.thumbWorldSize.x)*1f/ApplicationDesignRules.reductionRatio;
		}
		
		this.helpPaginationButtons.GetComponent<NewSkillBookHelpPaginationController> ().resize ();

		this.helpSubtitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.helpSubtitle.transform.position = new Vector3 (helpBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, helpBlockUpperLeftPosition.y - helpSubTitleFirstLine, 0f);
		this.helpSubtitle.GetComponent<TextContainer> ().width = (helpBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing)*1/ApplicationDesignRules.subMainTitleScale.x;
		this.helpSubtitle.GetComponent<TextContainer> ().height = (helpBlockSize.y - 2f*ApplicationDesignRules.blockHorizontalSpacing)*1/ApplicationDesignRules.subMainTitleScale.y;

		this.mainContentPositionX = skillsBlockOrigin.x;
		this.helpContentPositionX=helpBlockOrigin.x;
		this.filtersPositionX = filtersBlockOrigin.x;

		this.slideLeftButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.slideLeftButton.transform.position = new Vector3 (filtersBlockUpperRightPosition.x -ApplicationDesignRules.roundButtonWorldSize.x/2f-ApplicationDesignRules.blockHorizontalSpacing, filtersBlockUpperRightPosition.y - ApplicationDesignRules.roundButtonWorldSize.y / 2f- ApplicationDesignRules.buttonVerticalSpacing, 0f);
		
		this.slideRightButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.slideRightButton.transform.position = new Vector3 (helpBlockUpperRightPosition.x - ApplicationDesignRules.roundButtonWorldSize.x/2f-ApplicationDesignRules.blockHorizontalSpacing, helpBlockUpperRightPosition.y - ApplicationDesignRules.roundButtonWorldSize.y / 2f-ApplicationDesignRules.buttonVerticalSpacing, 0f);

		this.focusedSkill.transform.localScale = ApplicationDesignRules.focusedSkillScale;
		this.focusedSkill.transform.position = ApplicationDesignRules.focusedSkillPosition;

		if(ApplicationDesignRules.isMobileScreen)
		{
			for(int i=0;i<this.tabs.Length-1;i++)
			{
				this.tabs[i].transform.localScale = ApplicationDesignRules.tabScale;
				this.tabs[i].transform.position = new Vector3 (helpBlockUpperLeftPosition.x + ApplicationDesignRules.tabWorldSize.x / 2f+ i*(ApplicationDesignRules.tabWorldSize.x+gapBetweenSelectionsButtons), helpBlockUpperLeftPosition.y+ApplicationDesignRules.tabWorldSize.y/2f,0f);
			}
			this.hideActiveTab();
			this.skillsPaginationButtons.transform.localPosition=new Vector3 (skillsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 3f*ApplicationDesignRules.roundButtonWorldSize.x, skillsBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);
			this.skillsBlockTitle.transform.GetComponent<TextContainer>().width=ApplicationDesignRules.blockWidth-2f*ApplicationDesignRules.blockHorizontalSpacing-1.5f*ApplicationDesignRules.roundButtonWorldSize.x;
			this.skillSearchBarTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Left;
			this.skillSearchBarTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Left;
			this.skillSearchBarTitle.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x, filtersBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
			this.skillSearchBar.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x+ApplicationDesignRules.inputTextWorldSize.x/2f, filtersBlockUpperLeftPosition.y - 1.375f, 0f);
			for(int i=0;i<this.skillChoices.Length;i++)
			{
				this.skillChoices[i].transform.localScale=ApplicationDesignRules.listElementScale;
				this.skillChoices[i].transform.position=new Vector3(this.skillSearchBar.transform.position.x,this.skillSearchBar.transform.position.y-ApplicationDesignRules.inputTextWorldSize.y/2f-(i+0.5f)*ApplicationDesignRules.listElementWorldSize.y+i*0.02f,-1f);
			}
			this.cardTypeFilterTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Left;
			this.cardTypeFilterTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Left;
			this.cardTypeFilterTitle.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x, filtersBlockUpperLeftPosition.y - 1.9f, 0f);
			float gapBetweenCardTypesFilters = (filtersBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing-5f*ApplicationDesignRules.cardTypeFilterWorldSize.x)/4f;
			for(int i = 0;i<this.cardsTypeFilters.Length;i++)
			{
				Vector3 cardTypeFilterPosition=new Vector3();

				if(i<5)
				{
					cardTypeFilterPosition.y=filtersBlockUpperLeftPosition.y-2.65f;
					cardTypeFilterPosition.x=filtersBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+0.5f*ApplicationDesignRules.cardTypeFilterWorldSize.x+i*(ApplicationDesignRules.cardTypeFilterWorldSize.x+gapBetweenCardTypesFilters);

				}
				else
				{
					cardTypeFilterPosition.y=filtersBlockUpperLeftPosition.y-3.8f;
					cardTypeFilterPosition.x=filtersBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+0.5f*ApplicationDesignRules.cardTypeFilterWorldSize.x+(i-5)*(ApplicationDesignRules.cardTypeFilterWorldSize.x+gapBetweenCardTypesFilters);

				}
				this.cardsTypeFilters[i].transform.position=cardTypeFilterPosition;
				this.cardsTypeFilters[i].transform.localScale=ApplicationDesignRules.cardTypeFilterScale;
			}

			this.skillTypeFilterTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Left;
			this.skillTypeFilterTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Left;
			this.skillTypeFilterTitle.transform.position=new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x, filtersBlockUpperLeftPosition.y - 4.7f, 0f);

			float gapBetweenSkillTypeFilters = (ApplicationDesignRules.blockWidth-2f*ApplicationDesignRules.blockHorizontalSpacing-this.skillTypeFilters.Length*ApplicationDesignRules.skillTypeFilterWorldSize.x)/(this.skillTypeFilters.Length-1);

			for(int i=0;i<this.skillTypeFilters.Length;i++)
			{
				this.skillTypeFilters[i].transform.localScale=ApplicationDesignRules.skillTypeFilterScale;
				this.skillTypeFilters[i].transform.position=new Vector3(filtersBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+ApplicationDesignRules.skillTypeFilterWorldSize.x/2f+i*(ApplicationDesignRules.skillTypeFilterWorldSize.x+gapBetweenSkillTypeFilters),filtersBlockUpperLeftPosition.y - 5.4f,0f);
			}
			this.availabilityFilterTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Left;
			this.availabilityFilterTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Left;
			this.availabilityFilterTitle.transform.position=new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x, filtersBlockUpperLeftPosition.y - 6.1f, 0f);
			for(int i=0;i<this.availableFilters.Length;i++)
			{
				this.availableFilters[i].transform.localScale=ApplicationDesignRules.button61Scale;
				this.availableFilters[i].transform.position=new Vector3(filtersBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+ApplicationDesignRules.button61WorldSize.x/2f+i*(ApplicationDesignRules.button61WorldSize.x+0.2f), filtersBlockUpperLeftPosition.y-6.55f,0f);
			}
			this.helpPaginationButtons.transform.localPosition=new Vector3 (helpBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing-2.5f*ApplicationDesignRules.roundButtonWorldSize.x, helpBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing-ApplicationDesignRules.roundButtonWorldSize.y/2f, 0f);
			Vector3 statsScale = new Vector3 (1f, 1f, 1f);
				
			for(int i=0;i<this.stats.Length;i++)
			{
				this.stats[i].transform.position=new Vector3(helpBlockLowerLeftPosition.x+helpBlockSize.x/2f,helpBlockUpperLeftPosition.y-3.1f-i*1.2f);
				this.stats[i].transform.localScale= ApplicationDesignRules.reductionRatio*statsScale;
				this.stats[i].transform.FindChild("Title").GetComponent<TextContainer>().width=helpBlockSize.x;
			}

			float helpContentPictureHeight = helpContentsHeight - 0.8f;

			for(int i=0;i<this.helpContents.Length;i++)
			{
				this.helpContents[i].transform.FindChild("Title").GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Middle;
				this.helpContents[i].transform.FindChild("Title").GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;
				float scale = helpContentPictureHeight/(this.helpContents[i].transform.FindChild("Picture").GetComponent<SpriteRenderer>().bounds.size.y);
				this.helpContents[i].transform.FindChild("Picture").localScale=scale*this.helpContents[i].transform.FindChild("Picture").localScale;
				this.helpContents[i].transform.FindChild("Picture").position=new Vector3(helpBlockOrigin.x,helpBlockUpperLeftPosition.y-helpContentsFirstLine-helpContentsHeight/2f-i*(helpContentsHeight),0f);
				this.helpContents[i].transform.FindChild("Title").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
				this.helpContents[i].transform.FindChild("Title").position=new Vector3(helpBlockOrigin.x,helpBlockUpperLeftPosition.y-helpContentsFirstLine-1.5f-i*(helpContentsHeight),0f);
				this.helpContents[i].transform.FindChild("Title").GetComponent<TextContainer>().width=(helpBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing)*1/(ApplicationDesignRules.reductionRatio);
			}
		}
		else
		{
			for(int i=0;i<this.tabs.Length;i++)
			{
				this.tabs[i].transform.localScale = ApplicationDesignRules.tabScale;
				this.tabs[i].transform.position = new Vector3 (helpBlockUpperLeftPosition.x + ApplicationDesignRules.tabWorldSize.x / 2f+ i*(ApplicationDesignRules.tabWorldSize.x+gapBetweenSelectionsButtons), helpBlockUpperLeftPosition.y+ApplicationDesignRules.tabWorldSize.y/2f,0f);
				this.tabs[i].SetActive(true);
			}
			this.skillsPaginationButtons.transform.localPosition=new Vector3(skillsBlockLowerLeftPosition.x+skillsBlockSize.x/2f, skillsBlockLowerLeftPosition.y + 0.3f, 0f);
			this.skillsBlockTitle.transform.GetComponent<TextContainer>().width=ApplicationDesignRules.blockWidth-2f*ApplicationDesignRules.blockHorizontalSpacing;
			this.cardTypeFilterTitle.transform.position = new Vector3 (0.3f+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f, filtersBlockUpperLeftPosition.y - 1.2f, 0f);

			for(int i=0;i<this.cardsTypeFilters.Length;i++)
			{
				int column=0;
				int line=0;
				Vector3 position=new Vector3();
				if((i>=0 && i<3)||(i>=7))
				{
					if(i>=7)
					{
						column=i-7;
						line=2;
					}
					else
					{
						column=i;
						line=0;
					}
					position.x=filtersBlockUpperLeftPosition.x+0.3f+ApplicationDesignRules.cardTypeFilterWorldSize.x+column*(ApplicationDesignRules.cardTypeFilterWorldSize.x);
				}
				else if(i>=3&& i<7)
				{
					column=i-3;
					line=1;
					position.x=filtersBlockUpperLeftPosition.x+0.3f+ApplicationDesignRules.cardTypeFilterWorldSize.x/2f+column*(ApplicationDesignRules.cardTypeFilterWorldSize.x);
				}
				position.y=filtersBlockUpperLeftPosition.y-1.75f-line*(0.7f*ApplicationDesignRules.cardTypeFilterWorldSize.y);
				position.z=0;
				this.cardsTypeFilters[i].transform.localScale=ApplicationDesignRules.cardTypeFilterScale;
				this.cardsTypeFilters[i].transform.position=position;
			}
			this.skillSearchBarTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Middle;
			this.skillSearchBarTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;
			this.skillSearchBarTitle.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 2f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - 1.2f, 0f);
			this.skillSearchBar.transform.position = new Vector3 (this.skillSearchBarTitle.transform.position.x, filtersBlockUpperLeftPosition.y - 1.6f, 0f);
			for(int i=0;i<this.skillChoices.Length;i++)
			{
				this.skillChoices[i].transform.localScale=ApplicationDesignRules.listElementScale;
				this.skillChoices[i].transform.position=new Vector3(this.skillSearchBar.transform.position.x,this.skillSearchBar.transform.position.y-ApplicationDesignRules.inputTextWorldSize.y/2f-(i+0.5f)*ApplicationDesignRules.listElementWorldSize.y+i*0.02f,-1f);
			}
			this.skillTypeFilterTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Middle;
			this.skillTypeFilterTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;
			this.skillTypeFilterTitle.transform.position=new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 1f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);

			for(int i=0;i<this.skillTypeFilters.Length;i++)
			{
				int column=0;
				int line=0;
				Vector3 position=new Vector3();
				if((i>=0 && i<2)||(i>=5))
				{
					if(i>=5)
					{
						column=i-5;
						line=2;
					}
					else
					{
						column=i;
						line=0;
					}
					position.x=filtersBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+gapBetweenSubFiltersBlock+1.5f*filtersSubBlockSize-0.5f*ApplicationDesignRules.skillTypeFilterWorldSize.x-0.05f+column*(0.1f+ApplicationDesignRules.skillTypeFilterWorldSize.x);
				}
				else if(i>=2&& i<5)
				{
					column=i-2;
					line=1;
					position.x=filtersBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+gapBetweenSubFiltersBlock+1.5f*filtersSubBlockSize-1f*ApplicationDesignRules.skillTypeFilterWorldSize.x-0.1f+column*(0.1f+ApplicationDesignRules.skillTypeFilterWorldSize.x);
				}
				position.y=filtersBlockUpperLeftPosition.y-1.75f-line*(1.05f*ApplicationDesignRules.skillTypeFilterWorldSize.y);
				position.z=0;
				this.skillTypeFilters[i].transform.localScale=ApplicationDesignRules.skillTypeFilterScale;
				this.skillTypeFilters[i].transform.position=position;
			}
			this.availabilityFilterTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Middle;
			this.availabilityFilterTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;
			this.availabilityFilterTitle.transform.position=new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 2f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - 2.25f, 0f);
			for(int i=0;i<this.availableFilters.Length;i++)
			{
				this.availableFilters[i].transform.localScale=ApplicationDesignRules.button61Scale;
				this.availableFilters[i].transform.position=new Vector3(availabilityFilterTitle.transform.position.x, filtersBlockUpperLeftPosition.y-2.65f-i*(ApplicationDesignRules.button61WorldSize.y+0.05f),0f);
			}
			this.helpPaginationButtons.transform.localPosition=new Vector3 (helpBlockLowerLeftPosition.x + helpBlockSize.x / 2, helpBlockLowerLeftPosition.y + 0.3f, 0f);
			Vector3 statsScale = new Vector3 (1f, 1f, 1f);
			Vector2 statsBlockSize = new Vector2 ((helpBlockSize.x - 0.6f) / 4f, helpBlockSize.y - 2.6f);
			
			for(int i=0;i<this.stats.Length;i++)
			{
				this.stats[i].transform.position=new Vector3(helpBlockLowerLeftPosition.x+0.3f+statsBlockSize.x/2f+i*statsBlockSize.x,helpBlockUpperLeftPosition.y-2.2f-statsBlockSize.y/2f);
				this.stats[i].transform.localScale= ApplicationDesignRules.reductionRatio*statsScale;
				this.stats[i].transform.FindChild("Title").GetComponent<TextContainer>().width=statsBlockSize.x;
			}

			float helpContentPictureHeight = helpContentsHeight - 0.2f;

			for(int i=0;i<this.helpContents.Length;i++)
			{
				this.helpContents[i].transform.FindChild("Title").GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Left;
				this.helpContents[i].transform.FindChild("Title").GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Left;
				float scale = helpContentPictureHeight/(this.helpContents[i].transform.FindChild("Picture").GetComponent<SpriteRenderer>().bounds.size.y);
				this.helpContents[i].transform.FindChild("Picture").localScale=scale*this.helpContents[i].transform.FindChild("Picture").localScale;
				float helpContentPictureWidth = this.helpContents[i].transform.FindChild("Picture").GetComponent<SpriteRenderer>().bounds.size.x;
				this.helpContents[i].transform.FindChild("Picture").position=new Vector3(helpBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+helpContentPictureWidth/2f,helpBlockUpperLeftPosition.y-helpContentsFirstLine-helpContentsHeight/2f-i*(helpContentsHeight),0f);
				this.helpContents[i].transform.FindChild("Title").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
				this.helpContents[i].transform.FindChild("Title").position=new Vector3(helpBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+0.1f+helpContentPictureWidth,helpBlockUpperLeftPosition.y-helpContentsFirstLine-helpContentsHeight/2f-i*(helpContentsHeight),0f);
				this.helpContents[i].transform.FindChild("Title").GetComponent<TextContainer>().width=(helpBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing-0.1f-helpContentPictureWidth)*1/(ApplicationDesignRules.reductionRatio);
			}
		}
		this.skillSearchBar.GetComponent<NewSkillBookSkillSearchBarController>().resize();
		MenuController.instance.resize();
		MenuController.instance.setCurrentPage(4);
		MenuController.instance.refreshMenuObject();
		TutorialObjectController.instance.resize();
	}
	public void cardTypeFilterHandler(int id)
	{
		if(!ApplicationDesignRules.isMobileScreen || this.filtersDisplayed)
		{
			if(this.filtersCardType.Contains(id))
			{
				this.filtersCardType.Remove(id);
				this.cardsTypeFilters[id].GetComponent<NewSkillBookCardTypeFilterController>().reset();
			}
			else
			{
				this.filtersCardType.Add (id);
				this.cardsTypeFilters[id].GetComponent<NewSkillBookCardTypeFilterController>().setIsSelected(true);
				this.cardsTypeFilters[id].GetComponent<NewSkillBookCardTypeFilterController>().setHoveredState();
			}
			this.skillsPagination.chosenPage = 0;
			this.applyFilters ();
		}
	}
	public void skillTypeFilterHandler(int id)
	{
		if(!ApplicationDesignRules.isMobileScreen || this.filtersDisplayed)
		{
			if(this.filtersSkillType.Contains(id))
			{
				this.filtersSkillType.Remove(id);
				this.skillTypeFilters[id].GetComponent<NewSkillBookSkillTypeFilterController>().reset();
			}
			else
			{
				this.filtersSkillType.Add (id);
				this.skillTypeFilters[id].GetComponent<NewSkillBookSkillTypeFilterController>().setIsSelected(true);
				this.skillTypeFilters[id].GetComponent<NewSkillBookSkillTypeFilterController>().setHoveredState();
			}
			this.skillsPagination.chosenPage = 0;
			this.applyFilters ();
		}
	}
	public void drawSkills()
	{
		this.skillsDisplayed = new List<int> ();
		
		for(int i=0;i<skillsPagination.nbElementsPerPage;i++)
		{
			if(this.skillsPagination.chosenPage*this.skillsPagination.nbElementsPerPage+i<this.skillsToBeDisplayed.Count)
			{
				this.skillsDisplayed.Add (this.skillsToBeDisplayed[this.skillsPagination.chosenPage*this.skillsPagination.nbElementsPerPage+i]);
				this.skills[i].GetComponent<NewSkillBookSkillController>().s=model.skillsList[this.skillsToBeDisplayed[this.skillsPagination.chosenPage*this.skillsPagination.nbElementsPerPage+i]];
				this.skills[i].GetComponent<NewSkillBookSkillController>().show ();
				this.skills[i].SetActive(true);
			}
			else
			{
				this.skills[i].SetActive(false);
			}
		}
		if(ApplicationDesignRules.isMobileScreen)
		{
			int nbLinesToDisplay = this.skillsDisplayed.Count;
			float contentHeight = 0.1f+nbLinesToDisplay*(0.85f+ApplicationDesignRules.gapBetweenSkillsLine);
			if(this.lowerScrollCamera.GetComponent<ScrollingController>().getViewHeight()>contentHeight)
			{
				contentHeight=this.lowerScrollCamera.GetComponent<ScrollingController>().getViewHeight()+0.7f;
			}
			this.lowerScrollCamera.GetComponent<ScrollingController> ().setContentHeight(contentHeight);
			this.lowerScrollCamera.GetComponent<ScrollingController>().setEndPositionY();
		}
	}
	public void cleanSkills()
	{
		for(int i=0;i<this.skills.Length;i++)
		{
			Destroy(this.skills[i]);
		}
	}
	public void cleanContents()
	{
		for(int i=0;i<this.contents.Length;i++)
		{
			Destroy(this.contents[i]);
		}
	}
	public void drawCardsTypes()
	{
		this.cardsTypesDisplayed = new List<int> ();
		for(int i =0;i<this.helpPagination.nbElementsPerPage;i++)
		{
			if(this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i<model.cardTypesList.Count)
			{
				this.cardsTypesDisplayed.Add (this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i);
				this.contents[i].SetActive(true);
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=WordingCardTypes.getDescription(this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i);
				this.contents[i].transform.FindChild("cardTypePicture").GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(model.cardTypesList[this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i].getPictureId());
				this.contents[i].transform.FindChild("title").GetComponent<TextMeshPro>().text=WordingCardTypes.getName(this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i);
				
			}
			else
			{
				this.contents[i].SetActive(false);
			}
		}
	}
	public void drawSkillsTypes()
	{
		this.skillsTypesDisplayed = new List<int> ();
		for(int i =0;i<this.helpPagination.nbElementsPerPage;i++)
		{
			if(this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i<model.skillTypesList.Count)
			{
				this.skillsTypesDisplayed.Add (this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i);
				this.contents[i].SetActive(true);
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=WordingSkillTypes.getDescription(this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i);
				this.contents[i].transform.FindChild("skillTypePicture").GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnSkillTypePicture(model.skillTypesList[this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i].IdPicture);
				this.contents[i].transform.FindChild("skillTypePicture").FindChild("Title").GetComponent<TextMeshPro>().text=WordingSkillTypes.getLetter(this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i);
				this.contents[i].transform.FindChild("title").GetComponent<TextMeshPro>().text=WordingSkillTypes.getName(this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i);
			}
			else
			{
				this.contents[i].SetActive(false);
			}
		}
	}
	public void drawMainHelp()
	{
		this.helpSubtitle.SetActive (true);
		this.helpLine.SetActive (true);
		this.helpSubtitle.transform.GetComponent<TextMeshPro> ().text = WordingSkillBook.getReference(12);
		for(int i=0;i<this.helpContents.Length;i++)
		{
			this.helpContents[i].SetActive(true);
		}
	}
	public void drawIndicators()
	{
		for(int i=0;i<this.stats.Length;i++)
		{
			this.stats[i].SetActive(true);
		}
		this.helpSubtitle.SetActive (true);
		this.helpLine.SetActive (true);
		this.helpSubtitle.transform.GetComponent<TextMeshPro> ().text = WordingSkillBook.getReference(13);
		this.stats[0].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.ownSkillsIdList.Count.ToString();
		this.stats[1].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= this.globalPercentage.ToString()+ " %";
		this.stats[2].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= ApplicationModel.player.CollectionPoints.ToString ();
		this.stats[3].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= ApplicationModel.player.CollectionRanking.ToString ();
	}
	public void availabilityFilterHandler(int id)
	{
		if(!ApplicationDesignRules.isMobileScreen || this.filtersDisplayed)
		{
			if(id==0)
			{
				if(isNotOwnFilterOn)
				{
					isNotOwnFilterOn=false;
					this.availableFilters[0].GetComponent<NewSkillBookAvailabilityFilterController>().reset();
				}
				else
				{
					isNotOwnFilterOn=true;
					if(isOwnFilterOn)
					{
						isOwnFilterOn=false;
						this.availableFilters[1].GetComponent<NewSkillBookAvailabilityFilterController>().reset();
					}
					this.availableFilters[0].GetComponent<NewSkillBookAvailabilityFilterController>().setIsSelected(true);
					this.availableFilters[0].GetComponent<NewSkillBookAvailabilityFilterController>().setHoveredState();
				}
			}
			else if(id==1)
			{
				if(isOwnFilterOn)
				{
					isOwnFilterOn=false;
					this.availableFilters[1].GetComponent<NewSkillBookAvailabilityFilterController>().reset();
				}
				else
				{
					isOwnFilterOn=true;
					if(isNotOwnFilterOn)
					{
						isNotOwnFilterOn=false;
						this.availableFilters[0].GetComponent<NewSkillBookAvailabilityFilterController>().reset();
					}
					this.availableFilters[1].GetComponent<NewSkillBookAvailabilityFilterController>().setIsSelected(true);
					this.availableFilters[1].GetComponent<NewSkillBookAvailabilityFilterController>().setHoveredState();
				}
			}
			this.skillsPagination.chosenPage = 0;
			this.applyFilters ();
		}
	}
	private void resetFiltersValue()
	{
		this.filtersCardType = new List<int> ();
		for(int i=0;i<this.cardsTypeFilters.Length;i++)
		{
			this.cardsTypeFilters[i].GetComponent<NewSkillBookCardTypeFilterController>().reset();
		}
		this.filtersSkillType = new List<int> ();
		for(int i=0;i<this.skillTypeFilters.Length;i++)
		{
			this.skillTypeFilters[i].GetComponent<NewSkillBookSkillTypeFilterController>().reset();
		}
		this.isOwnFilterOn = false;
		this.isNotOwnFilterOn = false;
		for(int i=0;i<this.availableFilters.Length;i++)
		{
			this.availableFilters[i].GetComponent<NewSkillBookAvailabilityFilterController>().reset();
		}
		this.valueSkill = "";
		this.isSkillChosen = false;
		this.cleanSkillAutocompletion ();
		this.stopSearchingSkill();
	}
	private void computeFilters() 
	{
		this.skillsToBeDisplayed=new List<int>();
		int nbCardTypeFilters = this.filtersCardType.Count;
		int nbSkillTypeFilters = this.filtersSkillType.Count;
		int max = model.skillsList.Count;
		
		for(int i=0;i<max;i++)
		{
			if(this.isOwnFilterOn && model.ownSkillsIdList.Contains(model.skillsList[i].Id))
			{
				continue;
			}
			if(this.isNotOwnFilterOn && !model.ownSkillsIdList.Contains(model.skillsList[i].Id))
			{
				continue;
			}
			if(nbCardTypeFilters>0)
			{
				bool testCardTypes=false;
				for(int j=0;j<nbCardTypeFilters;j++)
				{
					if (model.skillsList[i].CardType.Id == this.filtersCardType [j])
					{
						testCardTypes=true;
						break;
					}
				}
				if(!testCardTypes)
				{
					continue;
				}
			}
			if(nbSkillTypeFilters>0)
			{
				bool testSkillTypes=false;
				for(int j=0;j<nbSkillTypeFilters;j++)
				{
					if (model.skillsList[i].IdSkillType == this.filtersSkillType [j])
					{
						testSkillTypes=true;
						break;
					}
				}
				if(!testSkillTypes)
				{
					continue;
				}
			}
			if(this.isSkillChosen)
			{
				string valueSkillLower=this.valueSkill.ToLower();
				if(WordingSkills.getName(model.skillsList[i].Id).ToLower()!=valueSkillLower)
				{
					continue;
				}
			}
			this.skillsToBeDisplayed.Add(i);
		}
	}
	private void computeIndicators()
	{
		this.skillsPercentages=new int[model.skillsList.Count];
		this.skillsNbCards=new int[model.skillsList.Count];
		this.cardTypesNbSkillsOwn=new int[model.cardTypesList.Count];
		this.cardTypesNbSkills=new int[model.cardTypesList.Count];
		this.cardTypesNbCards=new int[model.cardTypesList.Count];
		int globalSum=new int();
		IList<int> idCards = new List<int> ();
		for(int i=0;i<model.skillsList.Count;i++)
		{
			for(int j=0;j<model.ownSkillsList.Count;j++)
			{
				if(model.skillsList[i].Id==model.ownSkillsList[j].Id)
				{
					if(!idCards.Contains(model.cardIdsList[j]))
					{
						this.cardTypesNbCards[model.skillsList[i].CardType.Id]++;
						idCards.Add (model.cardIdsList[j]);
					}
					this.skillsNbCards[i]++;
					if(this.skillsPercentages[i]<model.ownSkillsList[j].Power*10)
					{
						this.skillsPercentages[i]=model.ownSkillsList[j].Power*10;
					}
				}
			}
			globalSum=globalSum+this.skillsPercentages[i];
		}
		if(this.model.skillsList.Count>0)
		{
			this.globalPercentage=globalSum/this.model.skillsList.Count;
		}
		for (int i=0;i<model.cardTypesList.Count;i++)
		{
			for(int j=0;j<model.skillsList.Count;j++)
			{
				if(model.skillsList[j].CardType.Id==i)
				{
					this.cardTypesNbSkills[i]++;
					if(this.skillsNbCards[j]>0)
					{
						this.cardTypesNbSkillsOwn[i]++;
					}
				}
			}
		}
	}
	public void returnPressed()
	{
	}
	public void escapePressed()
	{
		BackOfficeController.instance.leaveGame();
	}
	public void closeAllPopUp()
	{
	}
	public void searchingSkill()
	{
		if(isSkillChosen)
		{
			this.isSkillChosen=false;
			this.skillsPagination.chosenPage = 0;
			this.applyFilters();
		}
		this.cleanSkillAutocompletion();
		this.valueSkill = "";
		this.isSearchingSkill = true;
	}
	public void stopSearchingSkill()
	{
		this.isSearchingSkill=false;
		this.cleanSkillAutocompletion();
		this.skillSearchBar.GetComponent<NewSkillBookSkillSearchBarController>().resetSearchBar();
		this.skillSearchBar.GetComponent<NewSkillBookSkillSearchBarController>().setButtonText(WordingFilters.getReference(2));
		this.valueSkill="Rechercher";
	}
	public void filterASkill(int id)
	{
		this.isSearchingSkill = false;
		this.valueSkill = this.skillChoices[id].transform.FindChild("Title").GetComponent<TextMeshPro>().text;
		this.isSkillChosen = true;
		this.skillSearchBar.GetComponent<NewSkillBookSkillSearchBarController>().resetSearchBar();
		this.skillSearchBar.GetComponent<NewSkillBookSkillSearchBarController>().setButtonText(this.valueSkill);
		this.cleanSkillAutocompletion ();
		this.skillsPagination.chosenPage = 0;
		this.applyFilters ();
	}
	private void setSkillAutocompletion()
	{
		this.skillsDisplayed = new List<int> ();
		this.cleanSkillAutocompletion ();
		this.skillSearchBar.GetComponent<NewSkillBookSkillSearchBarController>().setButtonText(this.valueSkill);
		if(this.valueSkill.Length>0)
		{
			for (int i = 0; i < model.skillsList.Count; i++) 
			{  
				if(this.removeDiacritics(WordingSkills.getName(model.skillsList [i].Id).ToLower()).Contains(this.removeDiacritics(this.valueSkill).ToLower()))
				{
					this.skillsDisplayed.Add (i);
					this.skillChoices[this.skillsDisplayed.Count-1].SetActive(true);
					this.skillChoices[this.skillsDisplayed.Count-1].GetComponent<NewSkillBookSkillChoiceController>().reset();
					this.skillChoices[this.skillsDisplayed.Count-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text =WordingSkills.getName(model.skillsList [i].Id);
				}
				if(this.skillsDisplayed.Count==this.skillChoices.Length)
				{
					break;
				}
			}
		}
	}
	private void cleanSkillAutocompletion ()
	{
		for(int i=0;i<this.skillChoices.Length;i++)
		{
			this.skillChoices[i].SetActive(false);
		}
	}
	public void slideLeft()
	{
		if(this.mainContentDisplayed)
		{
			this.lowerScrollCamera.GetComponent<ScrollingController>().reset();
			this.mainContentDisplayed=false;
			this.targetContentPositionX=this.helpContentPositionX;
		}
		else if(this.filtersDisplayed)
		{
			this.filtersDisplayed=false;
			this.targetContentPositionX=this.mainContentPositionX;
		}
		else if(this.targetContentPositionX==mainContentPositionX)
		{
			this.targetContentPositionX=this.helpContentPositionX;
		}
		else if(this.targetContentPositionX==filtersPositionX)
		{
			this.targetContentPositionX=this.mainContentPositionX;
		}
		this.toSlideRight=false;
		this.toSlideLeft=true;
	}
	public void slideRight()
	{
		if(this.mainContentDisplayed)
		{
			this.lowerScrollCamera.GetComponent<ScrollingController>().reset();
			this.mainContentDisplayed=false;
			this.targetContentPositionX=this.filtersPositionX;
		}
		else if(this.helpContentDisplayed)
		{
			this.helpContentDisplayed=false;
			this.targetContentPositionX=this.mainContentPositionX;
		}
		else if(this.targetContentPositionX==mainContentPositionX)
		{
			this.targetContentPositionX=this.filtersPositionX;
		}
		else if(this.targetContentPositionX==helpContentPositionX)
		{
			this.targetContentPositionX=this.mainContentPositionX;
		}
		this.toSlideRight=true;
		this.toSlideLeft=false;
	}
	public void leftClickHandler()
	{
		this.isLeftClicked = true;
	}
	public void showFocusedSkill(int id)
	{
		this.isFocusedSkillDisplayed= true;
		this.displayBackUI (false);
		this.focusedSkill.SetActive (true);
		this.focusedSkill.GetComponent<FocusedSkillController> ().show (model.skillsList [this.skillsToBeDisplayed [this.skillsPagination.chosenPage * this.skillsPagination.nbElementsPerPage + id]]);
	}
	public void hideFocusedSkill()
	{
		this.isFocusedSkillDisplayed = false;
		this.focusedSkill.SetActive (false);
		this.displayBackUI (true);
	}
	public void displayBackUI(bool value)
	{
		if(value)
		{
			if(ApplicationDesignRules.isMobileScreen)
			{
				this.lowerScrollCamera.SetActive(true);
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
				this.upperScrollCamera.SetActive(false);
				this.sceneCamera.SetActive(true);
			}
			this.sceneCamera.transform.position=ApplicationDesignRules.sceneCameraFocusedCardPosition;
		}
	}
	public void hideActiveTab()
	{
		this.helpBlockTitle.GetComponent<TextMeshPro>().text=this.tabs[this.activeTab].transform.FindChild("Title").GetComponent<TextMeshPro>().text;
		switch(this.activeTab)
		{
		case 0:
			this.tabs[0].SetActive(false);
			this.tabs[1].SetActive(true);
			this.tabs[2].SetActive(true);
			this.tabs[3].transform.position=this.tabs[0].transform.position;
			this.tabs[3].SetActive(true);
			break;
		case 1:
			this.tabs[0].SetActive(true);
			this.tabs[1].SetActive(false);
			this.tabs[2].SetActive(true);
			this.tabs[3].transform.position=this.tabs[1].transform.position;
			this.tabs[3].SetActive(true);
			break;
		case 2:
			this.tabs[0].SetActive(true);
			this.tabs[1].SetActive(true);
			this.tabs[2].SetActive(false);
			this.tabs[3].transform.position=this.tabs[2].transform.position;
			this.tabs[3].SetActive(true);
			break;
		case 3:
			this.tabs[0].SetActive(true);
			this.tabs[1].SetActive(true);
			this.tabs[2].SetActive(true);
			this.tabs[3].SetActive(false);
			break;
		}
	}
	public void leftClickReleaseHandler(int id)
	{
		if(this.isLeftClicked)
		{
			this.showFocusedSkill(id);
			this.isLeftClicked=false;
		}
	}
	private string removeDiacritics(string text) 
	{
	    var normalizedString = text.Normalize(NormalizationForm.FormD);
	    var stringBuilder = new StringBuilder();

	    foreach (var c in normalizedString)
	    {
	        var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
	        if (unicodeCategory != UnicodeCategory.NonSpacingMark)
	        {
	            stringBuilder.Append(c);
	        }
	    }
	    return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
	}
	#region TUTORIAL FUNCTIONS
	
	public Vector3 getFiltersBlockOrigin()
	{
		return this.filtersBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getFiltersBlockSize()
	{
		return this.filtersBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public Vector3 getSkillsBlockOrigin()
	{
		return this.skillsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getSkillsBlockSize()
	{
		return this.skillsBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public Vector3 getHelpBlockOrigin()
	{
		return this.helpBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public IEnumerator endHelp()
	{
		if(!ApplicationModel.player.SkillBookTutorial)
		{
			BackOfficeController.instance.displayLoadingScreen();
			yield return StartCoroutine(ApplicationModel.player.setSkillBookTutorial(true));
			BackOfficeController.instance.hideLoadingScreen();
		}
		yield break;
	}
	public bool getAreFilersDisplayed()
	{
		return filtersDisplayed;
	}
	public bool getHelpDisplayed()
	{
		return helpContentDisplayed;
	}
	public void resetScrolling()
	{
		this.lowerScrollCamera.GetComponent<ScrollingController>().reset();
	}
	public bool getIsFocusedSkillDisplayed()
	{
		return this.isFocusedSkillDisplayed;
	}
	public Vector3 getFocusedSkillPosition()
	{
		return this.focusedSkill.transform.position;
	}
	#endregion
}