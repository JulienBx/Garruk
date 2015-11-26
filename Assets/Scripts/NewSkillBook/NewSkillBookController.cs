using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class NewSkillBookController : MonoBehaviour
{
	public static NewSkillBookController instance;
	private NewSkillBookModel model;
	
	public GameObject blockObject;
	public GameObject skillObject;

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
	private GameObject skillsCamera;
	private GameObject menuCamera;
	private GameObject tutorialCamera;
	private GameObject backgroundCamera;
	
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
	private bool isMouseOnSearchBar;
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

	void Update()
	{
		if(isSearchingSkill)
		{
			if(!Input.GetKey(KeyCode.Delete))
			{
				foreach (char c in Input.inputString) 
				{
					if(c==(char)KeyCode.Backspace && this.valueSkill.Length>0)
					{
						this.valueSkill = this.valueSkill.Remove(this.valueSkill.Length - 1);
						this.skillSearchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text = this.valueSkill;
						this.setSkillAutocompletion();
						if(this.valueSkill.Length==0)
						{
							this.isSearchingSkill=false;
							this.skillSearchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text = "Rechercher";
							this.skillSearchBar.GetComponent<NewSkillBookSkillSearchBarController>().reset();
						}
					}
					else if (c == "\b"[0])
					{
						if (valueSkill.Length != 0)
						{
							valueSkill= valueSkill.Substring(0, valueSkill.Length - 1);
						}
					}
					else
					{
						if (c == "\n"[0] || c == "\r"[0])
						{
							
						}
						else if(this.valueSkill.Length<12)
						{
							this.valueSkill += c;
							this.valueSkill=this.valueSkill.ToLower();
							this.setSkillAutocompletion();
						}
					}
				}
			}
			if((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))&& !this.isMouseOnSearchBar)
			{
				this.isSearchingSkill=false;
				this.cleanSkillAutocompletion();
				this.skillSearchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text = "Rechercher";
				this.skillSearchBar.GetComponent<NewSkillBookSkillSearchBarController>().reset();
			}
		}
		if(ApplicationDesignRules.isMobileScreen && this.isSceneLoaded)
		{
			isScrolling = this.skillsCamera.GetComponent<ScrollingController>().ScrollController();
		}
	}
	void Awake()
	{
		instance = this;
		this.activeTab = 0;
		this.model = new NewSkillBookModel ();
		this.helpPagination = new Pagination ();
		this.helpPagination.chosenPage = 0;
		this.helpPagination.nbElementsPerPage = 3;
		this.selectedCardTypeId = 0;
		this.scrollIntersection = 1.5f;
		this.initializeScene ();
		this.startMenuInitialization ();
	}
	private void startMenuInitialization()
	{
		this.menu = GameObject.Find ("Menu");
		this.menu.AddComponent<SkillBookMenuController> ();
	}
	public void endMenuInitialization()
	{
		this.startTutorialInitialization ();
	}
	private void startTutorialInitialization()
	{
		this.tutorial = GameObject.Find ("Tutorial");
		this.tutorial.AddComponent<SkillBookTutorialController>();
	}
	public void endTutorialInitialization()
	{
		StartCoroutine(this.initialization ());
	}
	public IEnumerator initialization()
	{
		this.resize ();
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.getSkillBookData ());
		this.computeIndicators ();
		this.selectATab ();
		this.initializeFilters ();
		this.initializeSkills ();
		MenuController.instance.hideLoadingScreen ();
		this.isSceneLoaded = true;
		if(model.player.TutorialStep!=-1)
		{
			TutorialObjectController.instance.startTutorial(model.player.TutorialStep,model.player.displayTutorial);
		}
	}
	public void selectATabHandler(int idTab)
	{
		this.activeTab = idTab;
		this.selectATab ();
	}
	private void selectATab()
	{
		for(int i=0;i<this.tabs.Length;i++)
		{
			if(i==this.activeTab)
			{
				this.tabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(1);
				this.tabs[i].GetComponent<NewSkillBookTabController>().setIsSelected(true);
				this.tabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			}
			else
			{
				this.tabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(0);
				this.tabs[i].GetComponent<NewSkillBookTabController>().reset();
			}
		}
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
			Vector3 cardsCameraPosition = this.skillsCamera.transform.position;
			cardsCameraPosition.y=this.skillsCamera.GetComponent<ScrollingController>().getStartPositionY();
			this.skillsCamera.transform.position=cardsCameraPosition;
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
			this.skillTypeFilters[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=model.skillTypesList[i].Name.Substring (0, 1).ToUpper();
			this.skillTypeFilters[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnSkillTypePicture(i);
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
			this.skillsNumberTitle.GetComponent<TextMeshPro>().text=("compétence " +this.skillsPagination.elementDebut+" à "+this.skillsPagination.elementFin+" sur "+this.skillsPagination.totalElements ).ToUpper();
		}
		else
		{
			this.skillsNumberTitle.GetComponent<TextMeshPro>().text="aucune compétence à afficher".ToUpper();
		}
	}
	public void initializeScene()
	{
		this.skillsBlock = Instantiate (this.blockObject) as GameObject;
		this.skillsBlockTitle = GameObject.Find ("SkillsBlockTitle");
		this.skillsBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.skillsBlockTitle.GetComponent<TextMeshPro> ().text = "Compétences";
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
		this.tabs[0].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Cristalopedia");
		this.tabs[1].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Progression");
		this.tabs[2].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Disciplines");
		this.tabs[3].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Factions");
		this.contents = new GameObject[helpPagination.nbElementsPerPage];
		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i]=GameObject.Find("Content"+i);
		}
		this.helpSubtitle = GameObject.Find ("HelpSubtitle");
		this.helpSubtitle.transform.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.helpContents=new GameObject[4];
		for(int i=0;i<this.helpContents.Length;i++)
		{
			this.helpContents[i]=GameObject.Find("HelpContent"+i);
			this.helpContents[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
		this.helpContents [0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Discipline associé à la compétence.";
		this.helpContents [1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Faction associé à la compétence. ";
		this.helpContents [2].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Pourcentage de réussite lorsqu'une compétence est lancée.";
		this.helpContents [3].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Présente les niveaux de puissance de la compétence."; 
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
		this.stats[0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= "Compétences acquises";
		this.stats[1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= "Niveau de la collection";
		this.stats[2].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= "Points de collection";
		this.stats[3].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= "Classement de la collection";

		this.filtersBlock = Instantiate (this.blockObject) as GameObject;
		this.filtersBlockTitle = GameObject.Find ("FiltersBlockTitle");
		this.filtersBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.filtersBlockTitle.GetComponent<TextMeshPro> ().text = "Filtrer";
		this.cardsTypeFilters = new GameObject[10];
		for(int i=0;i<this.cardsTypeFilters.Length;i++)
		{
			this.cardsTypeFilters[i]=GameObject.Find("CardTypeFilter"+i);
			this.cardsTypeFilters[i].AddComponent<NewSkillBookCardTypeFilterController>();
			this.cardsTypeFilters[i].GetComponent<NewSkillBookCardTypeFilterController>().setId(i);
		}
		this.skillTypeFilters = new GameObject[6];
		for(int i=0;i<this.skillTypeFilters.Length;i++)
		{
			this.skillTypeFilters[i]=GameObject.Find("SkillTypeFilter"+i);
			this.skillTypeFilters[i].AddComponent<NewSkillBookSkillTypeFilterController>();
			this.skillTypeFilters[i].GetComponent<NewSkillBookSkillTypeFilterController>().setId(i);
		}
		this.availableFilters = new GameObject[2];
		for (int i=0; i<this.availableFilters.Length; i++) 
		{
			this.availableFilters[i]=GameObject.Find("AvailableFilter"+i);
			this.availableFilters[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.availableFilters[i].AddComponent<NewSkillBookAvailabilityFilterController>();
			this.availableFilters[i].GetComponent<NewSkillBookAvailabilityFilterController>().setId(i);
		}
		this.availableFilters [0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "acquises".ToUpper ();
		this.availableFilters [1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "manquantes".ToUpper();
		this.skillSearchBarTitle = GameObject.Find ("SkillSearchTitle");
		this.skillSearchBarTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.skillSearchBarTitle.GetComponent<TextMeshPro> ().text = "Compétence".ToUpper ();
		this.skillSearchBar = GameObject.Find ("SkillSearchBar");
		this.skillSearchBar.AddComponent<NewSkillBookSkillSearchBarController> ();
		this.skillSearchBar.GetComponent<NewSkillBookSkillSearchBarController> ().setText ("Rechercher");
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
		this.cardTypeFilterTitle.GetComponent<TextMeshPro> ().text = "Faction".ToUpper ();
		this.skillTypeFilterTitle = GameObject.Find ("SkillTypeFilterTitle");
		this.skillTypeFilterTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.skillTypeFilterTitle.GetComponent<TextMeshPro> ().text = "Discipline".ToUpper ();
		this.availabilityFilterTitle = GameObject.Find ("AvailabilityFilterTitle");
		this.availabilityFilterTitle.GetComponent<TextMeshPro> ().text = "Disponibilité".ToUpper ();
		this.availabilityFilterTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.mainCamera = gameObject;
		this.mainCamera.AddComponent<ScrollingController> ();
		this.menuCamera = GameObject.Find ("MenuCamera");
		this.tutorialCamera = GameObject.Find ("TutorialCamera");
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
		this.skillsCamera = GameObject.Find ("SkillsCamera");
		this.skillsCamera.AddComponent<ScrollingController> ();
	}
	public void resize()
	{
		this.menuCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.tutorialCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.backgroundCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.backgroundCameraSize;

		float skillsBlockLeftMargin;
		float skillsBlockUpMargin;
		float skillsBlockHeight;
		
		float helpBlockLeftMargin;
		float helpBlockUpMargin;
		float helpBlockHeight;
		
		float filtersBlockLeftMargin;
		float filtersBlockUpMargin;
		float filtersBlockHeight;

		float skillScale = 0.53f;
		float skillBackgroundHeight = 351f;
		float skillWorldHeight = skillScale*(skillBackgroundHeight / ApplicationDesignRules.pixelPerUnit);
		float gapBetweenSkills = 0.2f;
		
		helpBlockHeight=ApplicationDesignRules.mediumBlockHeight-ApplicationDesignRules.button62WorldSize.y;
		filtersBlockHeight=ApplicationDesignRules.smallBlockHeight;

		this.skillsPagination = new Pagination ();
		this.skillsPagination.chosenPage = 0;
		
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.skillsPagination.nbElementsPerPage = 50;
			this.skillsScrollLine.SetActive(true);
			skillsBlockHeight=2.1f+this.skillsPagination.nbElementsPerPage*(skillWorldHeight+gapBetweenSkills);

			helpBlockLeftMargin=ApplicationDesignRules.leftMargin;
			helpBlockUpMargin=0f+ApplicationDesignRules.button62WorldSize.y;
			
			skillsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			skillsBlockUpMargin=helpBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+helpBlockHeight;
			
			filtersBlockLeftMargin=ApplicationDesignRules.leftMargin;
			filtersBlockUpMargin=skillsBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+skillsBlockHeight;
		}
		else
		{
			this.skillsPagination.nbElementsPerPage = 3;
			this.skillsScrollLine.SetActive(false);
			skillsBlockHeight=ApplicationDesignRules.largeBlockHeight;

			this.skillsCamera.SetActive(false);
			this.mainCamera.GetComponent<Camera>().rect=new Rect(0f,0f,1f,1f);
			this.mainCamera.transform.position=ApplicationDesignRules.mainCameraStartPosition;
			this.mainCamera.GetComponent<Camera>().orthographicSize=ApplicationDesignRules.cameraSize;

			helpBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			helpBlockUpMargin=ApplicationDesignRules.upMargin+ApplicationDesignRules.button62WorldSize.y;
			
			filtersBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			filtersBlockUpMargin=helpBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+helpBlockHeight;
			
			skillsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			skillsBlockUpMargin=ApplicationDesignRules.upMargin;
		}

		this.mainCamera.GetComponent<ScrollingController> ().setViewHeight(ApplicationDesignRules.viewHeight);
		this.mainCamera.GetComponent<ScrollingController> ().setContentHeight(helpBlockHeight + skillsBlockHeight + filtersBlockHeight + 2f * ApplicationDesignRules.gapBetweenBlocks + ApplicationDesignRules.button62WorldSize.y);
		this.mainCamera.transform.position = ApplicationDesignRules.mainCameraStartPosition;
		this.mainCamera.GetComponent<ScrollingController> ().setStartPositionY (ApplicationDesignRules.mainCameraStartPosition.y);

		this.filtersBlock.GetComponent<NewBlockController> ().resize(filtersBlockLeftMargin,filtersBlockUpMargin,ApplicationDesignRules.blockWidth,filtersBlockHeight);
		Vector3 filtersBlockUpperLeftPosition = this.filtersBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 filtersBlockUpperRightPosition = this.filtersBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 filtersBlockSize = this.filtersBlock.GetComponent<NewBlockController> ().getSize ();
		
		float gapBetweenSubFiltersBlock = 0.05f;
		float filtersSubBlockSize = (filtersBlockSize.x - 0.6f - 2f * gapBetweenSubFiltersBlock) / 3f;
		
		this.filtersBlockTitle.transform.position = new Vector3 (filtersBlockUpperLeftPosition.x + 0.3f, filtersBlockUpperLeftPosition.y - 0.2f, 0f);
		this.filtersBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		
		this.cardTypeFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.cardTypeFilterTitle.transform.position = new Vector3 (0.3f+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f, filtersBlockUpperLeftPosition.y - 1.2f, 0f);
		
		float gapBetweenCardTypesFilters = (filtersSubBlockSize - 4f * ApplicationDesignRules.cardTypeFilterWorldSize.x) / 3f;
		float gapBetweenLines;
		if(gapBetweenCardTypesFilters>0.05f)
		{
			gapBetweenLines=0.05f;
		}
		else
		{
			gapBetweenLines=gapBetweenCardTypesFilters;
		}
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
				position.x=filtersBlockUpperLeftPosition.x+0.3f+ApplicationDesignRules.cardTypeFilterWorldSize.x+column*(gapBetweenCardTypesFilters+ApplicationDesignRules.cardTypeFilterWorldSize.x);
			}
			else if(i>=3&& i<7)
			{
				column=i-3;
				line=1;
				position.x=filtersBlockUpperLeftPosition.x+0.3f+ApplicationDesignRules.cardTypeFilterWorldSize.x/2f+column*(gapBetweenCardTypesFilters+ApplicationDesignRules.cardTypeFilterWorldSize.x);
			}
			position.y=filtersBlockUpperLeftPosition.y-1.7f-line*(ApplicationDesignRules.cardTypeFilterWorldSize.y+gapBetweenLines);
			position.z=0;
			this.cardsTypeFilters[i].transform.localScale=ApplicationDesignRules.cardTypeFilterScale;
			this.cardsTypeFilters[i].transform.position=position;
		}
		
		this.availabilityFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.availabilityFilterTitle.transform.position=new Vector3 (0.3f+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 2f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - 2.25f, 0f);
		
		this.skillTypeFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.skillTypeFilterTitle.transform.position=new Vector3 (0.3f+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 1f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - 1.2f, 0f);
		
		for(int i=0;i<this.skillTypeFilters.Length;i++)
		{
			this.skillTypeFilters[i].transform.localScale=ApplicationDesignRules.skillTypeFilterScale;
			if(i<3)
			{
				this.skillTypeFilters[i].transform.position=new Vector3(skillTypeFilterTitle.transform.position.x-0.025f-ApplicationDesignRules.skillTypeFilterWorldSize.x/2f,filtersBlockUpperLeftPosition.y - 1.7f-i*(ApplicationDesignRules.skillTypeFilterWorldSize.y+0.025f),0f);
			}
			else
			{
				this.skillTypeFilters[i].transform.position=new Vector3(skillTypeFilterTitle.transform.position.x+0.025f+ApplicationDesignRules.skillTypeFilterWorldSize.x/2f,filtersBlockUpperLeftPosition.y - 1.7f-(i-3)*(ApplicationDesignRules.skillTypeFilterWorldSize.y+0.025f),0f);
			}
		}

		this.skillSearchBarTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.skillSearchBarTitle.transform.position = new Vector3 (0.3f+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 2f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - 1.2f, 0f);
		
		this.skillSearchBar.transform.localScale = ApplicationDesignRules.inputTextScale;
		this.skillSearchBar.transform.position = new Vector3 (this.skillSearchBarTitle.transform.position.x, filtersBlockUpperLeftPosition.y - 1.6f, 0f);
		
		for(int i=0;i<this.skillChoices.Length;i++)
		{
			this.skillChoices[i].transform.localScale=ApplicationDesignRules.listElementScale;
			this.skillChoices[i].transform.position=new Vector3(this.skillSearchBar.transform.position.x,this.skillSearchBar.transform.position.y-ApplicationDesignRules.inputTextWorldSize.y/2f-(i+0.5f)*ApplicationDesignRules.listElementWorldSize.y+i*0.02f,-1f);
		}
		
		for(int i=0;i<this.availableFilters.Length;i++)
		{
			this.availableFilters[i].transform.localScale=ApplicationDesignRules.button61Scale;
			this.availableFilters[i].transform.position=new Vector3(availabilityFilterTitle.transform.position.x, filtersBlockUpperLeftPosition.y-2.65f-i*(ApplicationDesignRules.button61WorldSize.y+0.05f),0f);
		}

		this.skillsBlock.GetComponent<NewBlockController> ().resize(skillsBlockLeftMargin,skillsBlockUpMargin,ApplicationDesignRules.blockWidth,skillsBlockHeight);
		Vector3 skillsBlockUpperLeftPosition = this.skillsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 skillsBlockLowerLeftPosition = this.skillsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector3 skillsBlockUpperRightPosition = this.skillsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 skillsBlockSize = this.skillsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector3 skillsBlockOrigin = this.skillsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.skillsBlockTitle.transform.position = new Vector3 (skillsBlockUpperLeftPosition.x + 0.3f, skillsBlockUpperLeftPosition.y - 0.2f, 0f);
		this.skillsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		this.skillsNumberTitle.transform.position = new Vector3 (skillsBlockUpperLeftPosition.x + 0.3f, skillsBlockUpperLeftPosition.y - 1.2f, 0f);
		this.skillsNumberTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		this.skills=new GameObject[this.skillsPagination.nbElementsPerPage];

		float skillWorldWidth = skillsBlockSize.x - 0.6f;
		float lineScale = ApplicationDesignRules.getLineScale (skillsBlockSize.x - 0.6f);


		for(int i=0;i<this.skills.Length;i++)
		{
			this.skills[i]=Instantiate (this.skillObject) as GameObject;
			this.skills[i].GetComponent<NewSkillBookSkillController>().initialize();
			this.skills[i].transform.position=new Vector3(skillsBlockUpperLeftPosition.x+0.3f+skillWorldWidth/2f,skillsBlockUpperLeftPosition.y-2.35f-i*(skillWorldHeight+gapBetweenSkills),0f);
			this.skills[i].transform.GetComponent<NewSkillBookSkillController>().resize(skillWorldWidth);
		}

		this.skillsPaginationButtons.transform.localPosition=new Vector3(skillsBlockLowerLeftPosition.x+skillsBlockSize.x/2f, skillsBlockLowerLeftPosition.y + 0.3f, 0f);
		this.skillsPaginationButtons.transform.GetComponent<NewSkillBookSkillsPaginationController> ().resize ();

		this.skillsPaginationLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.skillsPaginationLine.transform.position = new Vector3 (skillsBlockLowerLeftPosition.x + skillsBlockSize.x / 2, skillsBlockLowerLeftPosition.y + 0.6f, 0f);
		this.skillsScrollLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.skillsScrollLine.transform.position = new Vector3 (skillsBlockLowerLeftPosition.x + skillsBlockSize.x / 2, skillsBlockUpperLeftPosition.y-this.scrollIntersection+0.03f+ApplicationDesignRules.gapBetweenBlocks, 0f);



		this.helpBlock.GetComponent<NewBlockController> ().resize(helpBlockLeftMargin,helpBlockUpMargin, ApplicationDesignRules.blockWidth,helpBlockHeight);
		Vector3 helpBlockUpperLeftPosition = this.helpBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 helpBlockUpperRightPosition = this.helpBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 helpBlockLowerLeftPosition = this.helpBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 helpBlockLowerRightPosition = this.helpBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 helpBlockSize = this.helpBlock.GetComponent<NewBlockController> ().getSize ();
		
		float gapBetweenSelectionsButtons = 0.02f;
		for(int i=0;i<this.tabs.Length;i++)
		{
			this.tabs[i].transform.localScale = ApplicationDesignRules.button62Scale;
			this.tabs[i].transform.position = new Vector3 (helpBlockUpperLeftPosition.x + ApplicationDesignRules.button62WorldSize.x / 2f+ i*(ApplicationDesignRules.button62WorldSize.x+gapBetweenSelectionsButtons), helpBlockUpperLeftPosition.y+ApplicationDesignRules.button62WorldSize.y/2f,0f);
		}
		
		Vector2 contentBlockSize = new Vector2 (helpBlockSize.x - 0.6f, (helpBlockSize.y - 0.3f - 0.6f)/this.contents.Length);
		float helpLineScale = ApplicationDesignRules.getLineScale (contentBlockSize.x);

		this.helpLine.transform.localPosition=new Vector3 (helpBlockUpperLeftPosition.x + helpBlockSize.x / 2, helpBlockUpperLeftPosition.y - 1.5f, 0f);
		this.helpLine.transform.localScale = new Vector3 (helpLineScale, 1f, 1f);
		
		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i].transform.position=new Vector3(helpBlockUpperLeftPosition.x+0.3f+contentBlockSize.x/2f,helpBlockUpperLeftPosition.y-0.3f-(i+1)*contentBlockSize.y,0f);
			this.contents[i].transform.FindChild("line").localScale=new Vector3(lineScale,1f,1f);
			this.contents[i].transform.FindChild("skillTypePicture").localScale=ApplicationDesignRules.skillTypeThumbScale;
			this.contents[i].transform.FindChild("skillTypePicture").localPosition=new Vector3(-contentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(contentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
			this.contents[i].transform.FindChild("cardTypePicture").localScale=ApplicationDesignRules.cardTypeThumbScale;
			this.contents[i].transform.FindChild("cardTypePicture").localPosition=new Vector3(-contentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(contentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
			this.contents[i].transform.FindChild("title").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.contents[i].transform.FindChild("title").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/2f)-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.contents[i].transform.FindChild("title").localPosition=new Vector3(-contentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,contentBlockSize.y-(contentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.contents[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=0.75f*contentBlockSize.x-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.contents[i].transform.FindChild("description").localPosition=new Vector3(-contentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,contentBlockSize.y/2f,0f);
			this.contents[i].transform.FindChild("description").GetComponent<TextContainer>().width=(contentBlockSize.x-0.1f-ApplicationDesignRules.thumbWorldSize.x)*1/ApplicationDesignRules.reductionRatio;
	}
		
		this.helpPaginationButtons.transform.localPosition=new Vector3 (helpBlockLowerLeftPosition.x + helpBlockSize.x / 2, helpBlockLowerLeftPosition.y + 0.3f, 0f);
		this.helpPaginationButtons.GetComponent<NewSkillBookHelpPaginationController> ().resize ();

		this.helpSubtitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.helpSubtitle.transform.position = new Vector3 (helpBlockUpperLeftPosition.x + 0.3f, helpBlockUpperLeftPosition.y - 0.3f, 0f);
		this.helpSubtitle.GetComponent<TextContainer> ().width = (helpBlockSize.x - 0.6f)*1/ApplicationDesignRules.subMainTitleScale.x;
		this.helpSubtitle.GetComponent<TextContainer> ().height = (helpBlockSize.y - 0.6f)*1/ApplicationDesignRules.subMainTitleScale.y;

		Vector2 helpContentBlockSize = new Vector2 (helpBlockSize.x - 0.6f, (helpBlockSize.y - 1.5f) / 4f);
		float helpContentPictureHeight = helpContentBlockSize.y - 0.1f;

		for(int i=0;i<this.helpContents.Length;i++)
		{
			float scale = helpContentPictureHeight/(this.helpContents[i].transform.FindChild("Picture").GetComponent<SpriteRenderer>().bounds.size.x);
			this.helpContents[i].transform.FindChild("Picture").localScale=scale*this.helpContents[i].transform.FindChild("Picture").localScale;
			this.helpContents[i].transform.FindChild("Picture").position=new Vector3(helpBlockUpperLeftPosition.x+0.3f+helpContentPictureHeight/2f,helpBlockUpperLeftPosition.y-1.5f-helpContentBlockSize.y/2f-i*(helpContentBlockSize.y),0f);
			this.helpContents[i].transform.FindChild("Title").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.helpContents[i].transform.FindChild("Title").position=new Vector3(helpBlockUpperLeftPosition.x+0.3f+0.1f+helpContentPictureHeight,helpBlockUpperLeftPosition.y-1.5f-helpContentBlockSize.y/2f-i*(helpContentBlockSize.y),0f);
			this.helpContents[i].transform.FindChild("Title").GetComponent<TextContainer>().width=(helpBlockSize.x-0.6f-0.1f-helpContentPictureHeight)*1/(ApplicationDesignRules.reductionRatio);
		}

		Vector3 statsScale = new Vector3 (1f, 1f, 1f);
		Vector2 statsBlockSize = new Vector2 ((helpBlockSize.x - 0.6f) / 4f, helpBlockSize.y - 2.6f);
		
		for(int i=0;i<this.stats.Length;i++)
		{
			this.stats[i].transform.position=new Vector3(helpBlockLowerLeftPosition.x+0.3f+statsBlockSize.x/2f+i*statsBlockSize.x,helpBlockUpperLeftPosition.y-2.2f-statsBlockSize.y/2f);
			this.stats[i].transform.localScale= ApplicationDesignRules.reductionRatio*statsScale;
			this.stats[i].transform.FindChild("Title").GetComponent<TextContainer>().width=statsBlockSize.x;
		}

		if(ApplicationDesignRules.isMobileScreen)
		{
			this.mainCamera.GetComponent<Camera> ().rect = new Rect (0f,(ApplicationDesignRules.worldHeight-ApplicationDesignRules.upMargin-this.scrollIntersection)/ApplicationDesignRules.worldHeight,1f,(this.scrollIntersection)/ApplicationDesignRules.worldHeight);
			this.mainCamera.GetComponent<Camera> ().orthographicSize = this.scrollIntersection/2f;
			this.mainCamera.transform.position = new Vector3 (0f, skillsBlockUpperLeftPosition.y-(this.scrollIntersection/2f) + ApplicationDesignRules.gapBetweenBlocks, -10f);
			
			this.skillsCamera.SetActive(true);
			this.skillsCamera.GetComponent<Camera> ().rect = new Rect (0f,(ApplicationDesignRules.downMargin)/ApplicationDesignRules.worldHeight,1f,(ApplicationDesignRules.viewHeight-this.scrollIntersection)/ApplicationDesignRules.worldHeight);
			this.skillsCamera.GetComponent<Camera> ().orthographicSize = (ApplicationDesignRules.viewHeight-this.scrollIntersection)/2f;
			this.skillsCamera.GetComponent<ScrollingController> ().setViewHeight(ApplicationDesignRules.viewHeight-this.scrollIntersection);
			this.skillsCamera.GetComponent<ScrollingController> ().setContentHeight(skillsBlockHeight-this.scrollIntersection+0.05f);
			this.skillsCamera.transform.position = new Vector3 (0f, skillsBlockUpperLeftPosition.y-(this.scrollIntersection/2f)+ApplicationDesignRules.gapBetweenBlocks-this.scrollIntersection/2f-(ApplicationDesignRules.viewHeight-this.scrollIntersection)/2f, -10f);
			this.skillsCamera.GetComponent<ScrollingController> ().setStartPositionY (this.skillsCamera.transform.position.y);
		}

		TutorialObjectController.instance.resize ();
	}
	public void cardTypeFilterHandler(int id)
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
	public void skillTypeFilterHandler(int id)
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
	}
	public void cleanSkills()
	{
		for(int i=0;i<this.skills.Length;i++)
		{
			Destroy(this.skills[i]);
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
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=model.cardTypesList[this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i].Description;
				this.contents[i].transform.FindChild("cardTypePicture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnCardTypePicture(model.cardTypesList[this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i].IdPicture);
				this.contents[i].transform.FindChild("title").GetComponent<TextMeshPro>().text=model.cardTypesList[this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i].Name;
				
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
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=model.skillTypesList[this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i].Description;
				this.contents[i].transform.FindChild("skillTypePicture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnSkillTypePicture(model.skillTypesList[this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i].IdPicture);
				this.contents[i].transform.FindChild("skillTypePicture").FindChild("Title").GetComponent<TextMeshPro>().text=model.skillTypesList[this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i].Name.Substring(0,1).ToUpper();
				this.contents[i].transform.FindChild("title").GetComponent<TextMeshPro>().text=model.skillTypesList[this.helpPagination.chosenPage*this.helpPagination.nbElementsPerPage+i].Name;
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
		this.helpSubtitle.transform.GetComponent<TextMeshPro> ().text = "Cristalopédia présente l'ensemble des compétences du jeu. Chaque compétence est caractérisée par un certain nombre d'informations";
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
		this.helpSubtitle.transform.GetComponent<TextMeshPro> ().text = "Ces indicateurs mesurent votre progression dans l'acquisition des compétences. Essayez de toute les collectionner !";
		this.stats[0].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.ownSkillsIdList.Count.ToString();
		this.stats[1].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= this.globalPercentage.ToString()+ " %";
		this.stats[2].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.player.CollectionPoints.ToString ();
		this.stats[3].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.player.CollectionRanking.ToString ();
	}
	public void availabilityFilterHandler(int id)
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
					if (model.skillsList[i].IdCardType == this.filtersCardType [j])
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
					if (model.skillsList[i].cible == this.filtersSkillType [j])
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
				if(model.skillsList[i].Name.ToLower()!=valueSkillLower)
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
		this.isSearchingSkill = true;
		this.valueSkill = "";
		this.skillSearchBar.transform.FindChild("Title").GetComponent<TextMeshPro>().text = this.valueSkill;
		this.skillSearchBar.transform.GetComponent<NewSkillBookSkillSearchBarController> ().setIsSelected (true);
		this.skillSearchBar.transform.GetComponent<NewSkillBookSkillSearchBarController> ().setInitialState ();
	}
	public void mouseOnSearchBar(bool value)
	{
		this.isMouseOnSearchBar = value;
	}
	public void filterASkill(int id)
	{
		this.isSearchingSkill = false;
		this.valueSkill = this.skillChoices[id].transform.FindChild("Title").GetComponent<TextMeshPro>().text.ToLower();
		this.isSkillChosen = true;
		this.skillSearchBar.transform.FindChild("Title").GetComponent<TextMeshPro>().text =valueSkill;
		this.cleanSkillAutocompletion ();
		this.skillsPagination.chosenPage = 0;
		this.applyFilters ();
	}
	private void setSkillAutocompletion()
	{
		this.skillsDisplayed = new List<int> ();
		this.cleanSkillAutocompletion ();
		this.skillSearchBar.transform.FindChild("Title").GetComponent<TextMeshPro>().text = this.valueSkill;
		if(this.valueSkill.Length>0)
		{
			for (int i = 0; i < model.skillsList.Count; i++) 
			{  
				if (model.skillsList [i].Name.ToLower ().Contains (this.valueSkill)) 
				{
					this.skillsDisplayed.Add (i);
					this.skillChoices[this.skillsDisplayed.Count-1].SetActive(true);
					this.skillChoices[this.skillsDisplayed.Count-1].GetComponent<NewSkillBookSkillChoiceController>().reset();
					this.skillChoices[this.skillsDisplayed.Count-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text = model.skillsList [i].Name;
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
	
	#endregion
}