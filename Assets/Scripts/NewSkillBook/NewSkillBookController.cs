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
	public GameObject paginationButtonObject;
	public GameObject skillObject;
	public Sprite[] cardTypesPictos;
	public Sprite[] starsPictos;

	private GameObject menu;
	private GameObject starsBlock;
	private GameObject mainBlock;
	private GameObject cardTypesBlock;
	private GameObject[] skills;
	private GameObject[] stars;
	private GameObject[] paginationButtons;
	private GameObject selectedCardType;
	private GameObject cardTypes;
	private GameObject collectionLevel;

	private int widthScreen;
	private int heightScreen;
	private float worldWidth;
	private float worldHeight;
	private float pixelPerUnit;
	
	private IList<int> skillsDisplayed;
	private IList<int> skillsToBeDisplayed;
	private int skillsPerLine;
	private int nbLines;

	private int nbPages;
	private int nbPaginationButtonsLimit;
	private int elementsPerPage;
	private int chosenPage;
	private int pageDebut;
	private int activePaginationButtonId;
	
	private int selectedCardTypeId;

	private int[] skillsPercentages;
	private int[] skillsNbCards;
	private int[] cardTypesNbSkillsOwn;
	private int[] cardTypesNbSkills;
	private int[] cardTypesNbCards;
	private int globalPercentage;
	
	void Update()
	{	
		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			this.resize();
			this.drawSkills();
			this.drawPagination();
		}
		if(Input.GetKeyDown(KeyCode.Escape)) 
		{
			this.escapePressed();
		}
	}
	void Awake()
	{
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.pixelPerUnit = 108f;
		this.elementsPerPage= 5;
		this.selectedCardTypeId = 0;
		this.initializeScene ();
	}
	void Start()
	{
		instance = this;
		this.model = new NewSkillBookModel ();
		this.resize ();
		StartCoroutine (this.initialization ());
	}
	private IEnumerator initialization()
	{
		yield return StartCoroutine (model.getSkillBookData ());
		this.computeIndicators ();
		this.drawCollectionLevel ();
		this.loadSkills ();
	}
	public void initializeScene()
	{
		menu = GameObject.Find ("newMenu");
		menu.GetComponent<newMenuController> ().setCurrentPage (0);
		this.mainBlock = Instantiate(this.blockObject) as GameObject;
		this.cardTypesBlock = Instantiate(this.blockObject) as GameObject;
		this.starsBlock = Instantiate(this.blockObject) as GameObject;
		selectedCardType = GameObject.Find ("SelectedCardType");
		cardTypes = GameObject.Find ("CardTypes");
		this.skills=new GameObject[0];
		this.paginationButtons=new GameObject[0];
		this.collectionLevel = GameObject.Find ("CollectionLevel");
		this.collectionLevel.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Niveau de la collection";
	}
	public void resize()
	{
		this.cleanSkills ();
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		float screenRatio = (float)this.widthScreen / (float)this.heightScreen;
		menu.GetComponent<newMenuController> ().resizeMeunObject (worldHeight,worldWidth);
		
		float mainBlockLeftMargin =3f;
		float mainBlockRightMargin = 2f;
		float mainBlockUpMargin = 0.2f;
		float mainBlockDownMargin = 1.95f;
		
		float mainBlockHeight = worldHeight - mainBlockUpMargin-mainBlockDownMargin;
		float mainBlockWidth = worldWidth-mainBlockLeftMargin-mainBlockRightMargin;
		Vector2 mainBlockOrigin = new Vector3 (-worldWidth/2f+mainBlockLeftMargin+mainBlockWidth/2f, -worldHeight / 2f + mainBlockDownMargin + mainBlockHeight / 2,0f);
		
		this.mainBlock.GetComponent<BlockController> ().resize(new Rect(mainBlockOrigin.x,mainBlockOrigin.y,mainBlockWidth,mainBlockHeight));

		this.selectedCardType.transform.position = new Vector3 (mainBlockOrigin.x, worldHeight/2f-mainBlockUpMargin-0.75f, 0f);

		float cardTypesBlockLeftMargin = this.worldWidth-1.8f;
		float cardTypesBlockRightMargin = 0f;
		float cardTypesBlockUpMargin = 0.6f;
		float cardTypesBlockDownMargin = 0.2f;
		
		float cardTypesBlockHeight = worldHeight - cardTypesBlockUpMargin-cardTypesBlockDownMargin;
		float cardTypesBlockWidth = worldWidth-cardTypesBlockLeftMargin-cardTypesBlockRightMargin;
		Vector2 cardTypesBlockOrigin = new Vector3 (-worldWidth/2f+cardTypesBlockLeftMargin+cardTypesBlockWidth/2f, -worldHeight / 2f + cardTypesBlockDownMargin + cardTypesBlockHeight / 2,0f);
		
		this.cardTypesBlock.GetComponent<BlockController> ().resize(new Rect(cardTypesBlockOrigin.x,cardTypesBlockOrigin.y, cardTypesBlockWidth, cardTypesBlockHeight));

		this.cardTypes.transform.position = new Vector3 (cardTypesBlockOrigin.x, 0f, 0f);

		float starsBlockLeftMargin =3f;
		float starsBlockRightMargin = 2f;
		float starsBlockUpMargin = 8.25f;
		float starsBlockDownMargin = 0.2f;
		
		float starsBlockHeight = worldHeight - starsBlockUpMargin-starsBlockDownMargin;
		float starsBlockWidth = worldWidth-starsBlockLeftMargin-starsBlockRightMargin;
		Vector2 starsBlockOrigin = new Vector3 (-worldWidth/2f+starsBlockLeftMargin+starsBlockWidth/2f, -worldHeight / 2f + starsBlockDownMargin + starsBlockHeight / 2,0f);
		
		this.starsBlock.GetComponent<BlockController> ().resize(new Rect(starsBlockOrigin.x,starsBlockOrigin.y,starsBlockWidth,starsBlockHeight));

		this.collectionLevel.transform.position = new Vector3 (starsBlockOrigin.x, starsBlockOrigin.y, 0f);
		if(screenRatio<(1.6f))
		{
			float collectionLevelScale = 1-1.5f*((1.6f-screenRatio)/1.6f);
			this.collectionLevel.transform.localScale=new Vector3(collectionLevelScale,collectionLevelScale,collectionLevelScale);
		}

		float skillsWidth = 600f;
		float skillsHeight = 200f;
		float skillsScale = 0.9f;
		float skillsWorldWidth = (skillsWidth / pixelPerUnit) * skillsScale;
		float skillsWorldHeight = (skillsHeight / pixelPerUnit) * skillsScale;
		
		this.skillsPerLine = Mathf.FloorToInt ((mainBlockWidth-0.5f) / skillsWorldWidth);
		this.nbLines = Mathf.FloorToInt ((mainBlockHeight-1.5f) / skillsWorldHeight);
		
		float gapWidth = (mainBlockWidth - (this.skillsPerLine * skillsWorldWidth)) / (this.skillsPerLine + 1);
		float gapHeight = (mainBlockHeight-1.5f - 0.45f - (this.nbLines * skillsWorldHeight)) / (this.nbLines + 1);
		float skillsStartX = mainBlockOrigin.x - mainBlockWidth / 2f-skillsWorldWidth/2f;
		float skillsStartY = mainBlockOrigin.y + mainBlockHeight / 2f-1.5f+skillsWorldHeight/2f;
		
		this.skills=new GameObject[this.skillsPerLine*this.nbLines];

		for(int j=0;j<this.nbLines;j++)
		{
			for(int i =0;i<this.skillsPerLine;i++)
			{
				this.skills[j*(skillsPerLine)+i] = Instantiate(this.skillObject) as GameObject;
				this.skills[j*(skillsPerLine)+i].transform.localScale= new Vector3(0.9f,0.9f,0.9f);
				this.skills[j*(skillsPerLine)+i].transform.position=new Vector3(skillsStartX+(i+1)*(gapWidth+skillsWorldWidth),skillsStartY-(j+1)*(gapHeight+skillsWorldHeight),0f);
				this.skills[j*(skillsPerLine)+i].transform.name="Skill"+(j*(this.skillsPerLine)+i);
				this.skills[j*(skillsPerLine)+i].SetActive(false);
			}
		}
	}
	public void loadSkills()
	{
		this.chosenPage = 0;
		this.selectedCardType.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().sprite = this.cardTypesPictos [this.selectedCardTypeId];
		this.selectedCardType.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = model.cardTypesList [this.selectedCardTypeId];

		bool display=new bool();
		if(this.cardTypesNbCards[this.selectedCardTypeId]>0)
		{
			display=true;
			this.selectedCardType.transform.FindChild("Description").GetComponent<TextMeshPro>().text=this.cardTypesNbSkillsOwn[this.selectedCardTypeId]+"/"+this.cardTypesNbSkills[this.selectedCardTypeId]+"compétences acquises";
			float percentage = (float)this.cardTypesNbSkillsOwn[this.selectedCardTypeId]/(float)this.cardTypesNbSkills[this.selectedCardTypeId];
			//print (percentage);
			if(this.cardTypesNbCards[this.selectedCardTypeId]>1)
			{
				this.selectedCardType.transform.FindChild("NbCards").GetComponent<TextMeshPro>().text=this.cardTypesNbCards[this.selectedCardTypeId]+" cartes";
			}
			else
			{
				this.selectedCardType.transform.FindChild("NbCards").GetComponent<TextMeshPro>().text=this.cardTypesNbCards[this.selectedCardTypeId]+" carte";
			}
			Vector3 tempPosition = this.selectedCardType.transform.FindChild ("Jauge").transform.localPosition;
			tempPosition.x = -1.3f + percentage * 1.16f;
			this.selectedCardType.transform.FindChild ("Jauge").transform.localPosition = tempPosition;
			Vector3 tempScale = this.selectedCardType.transform.FindChild ("Jauge").transform.localScale;
			tempScale.x = percentage * 2.48f;
			this.selectedCardType.transform.FindChild ("Jauge").transform.localScale = tempScale;

			Color color = new Color ();
			if(percentage<0.6)
			{
				color=new Color(170f/255f,124f/255f,80f/255f);
			}
			else if(percentage<0.8)
			{
				color=new Color(168f/255f,168f/255f,168f/255f);
			}
			else
			{
				color=new Color(255f/255f,232f/255f,99f/255f);
			}
			this.selectedCardType.transform.FindChild ("Jauge").GetComponent<SpriteRenderer> ().color = color;
		}
		else
		{
			display=false;
		}

		this.selectedCardType.transform.FindChild ("NbCards").gameObject.SetActive (display);
		this.selectedCardType.transform.FindChild ("Jauge").gameObject.SetActive (display);
		this.selectedCardType.transform.FindChild ("JaugeBackground").gameObject.SetActive (display);
		this.selectedCardType.transform.FindChild ("buttonBorder").gameObject.SetActive (display);
		this.selectedCardType.transform.FindChild ("Description").gameObject.SetActive (display);

		this.initializeSkills ();
		this.drawSkills ();
		this.drawPagination ();
	}
	public void initializeSkills()
	{
		this.skillsToBeDisplayed = new List<int> ();
		for(int i=0;i<model.skillsList.Count;i++)
		{
			if(model.skillsList[i].CardType==selectedCardTypeId)
			{
				this.skillsToBeDisplayed.Add (i);
			}
		}
	}
	public void selectCardTypeHandler(int id)
	{
		this.cardTypes.transform.FindChild("Picto"+this.selectedCardTypeId).GetComponent<SpriteRenderer>().color=new Color(189f/255f,189f/255f,189f/255f);
		this.cardTypes.transform.FindChild ("Picto" + this.selectedCardTypeId).GetComponent<NewSkillBookCardTypeButtonController> ().setActive (false);
		this.selectedCardTypeId = id;
		this.cardTypes.transform.FindChild("Picto"+id).GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		this.cardTypes.transform.FindChild ("Picto" + id).GetComponent<NewSkillBookCardTypeButtonController> ().setActive (true);
		this.loadSkills ();
	}
	public void drawSkills()
	{
		this.skillsDisplayed = new List<int> ();
		
		for(int j=0;j<nbLines;j++)
		{
			for(int i =0;i<skillsPerLine;i++)
			{
				if(this.chosenPage*(this.nbLines*this.skillsPerLine)+j*(skillsPerLine)+i<this.skillsToBeDisplayed.Count)
				{
					this.skillsDisplayed.Add (this.skillsToBeDisplayed[this.chosenPage*(this.nbLines*this.skillsPerLine)+j*(skillsPerLine)+i]);
					this.skills[j*(skillsPerLine)+i].GetComponent<NewSkillBookSkillController>().setId(j*skillsPerLine+i);
					this.skills[j*(skillsPerLine)+i].GetComponent<NewSkillBookSkillController>().s=model.skillsList[this.skillsDisplayed[j*(skillsPerLine)+i]];
					this.skills[j*(skillsPerLine)+i].GetComponent<NewSkillBookSkillController>().show ();
					this.skills[j*(skillsPerLine)+i].GetComponent<NewSkillBookSkillController>().setNbCards(this.skillsNbCards[this.skillsDisplayed[j*(skillsPerLine)+i]]);
					this.skills[j*(skillsPerLine)+i].GetComponent<NewSkillBookSkillController>().setPercentage(this.skillsPercentages[this.skillsDisplayed[j*(skillsPerLine)+i]]);
					this.skills[j*(skillsPerLine)+i].SetActive(true);
				}
				else
				{
					this.skills[j*(skillsPerLine)+i].SetActive(false);
				}
			}
		}
	}
	public void cleanSkills()
	{
		for (int i=0;i<this.skills.Length;i++)
		{
			Destroy (this.skills[i]);
		}
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
		this.nbPages = Mathf.CeilToInt((float)this.skillsToBeDisplayed.Count / ((float)this.nbLines*(float)this.skillsPerLine));
		if(this.nbPages>1)
		{
			this.nbPaginationButtonsLimit = Mathf.CeilToInt((this.worldWidth-5f)/(paginationButtonWidth+gapBetweenPaginationButton));
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
				this.paginationButtons[i].AddComponent<SkillBookPaginationController>();
				this.paginationButtons[i].transform.position=new Vector3(0.5f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-2.75f,0f);
				this.paginationButtons[i].name="Pagination"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtons[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebut+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtons[i].GetComponent<SkillBookPaginationController>().setId(i);
				if(this.pageDebut+i-System.Convert.ToInt32(drawBackButton)==this.chosenPage)
				{
					this.paginationButtons[i].GetComponent<SkillBookPaginationController>().setActive(true);
					this.activePaginationButtonId=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtons[0].GetComponent<SkillBookPaginationController>().setId(-2);
				this.paginationButtons[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtons[nbButtonsToDraw-1].GetComponent<SkillBookPaginationController>().setId(-1);
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
				this.paginationButtons[this.activePaginationButtonId].GetComponent<SkillBookPaginationController>().setActive(false);
			}
			this.activePaginationButtonId=id;
			this.chosenPage=this.pageDebut-System.Convert.ToInt32(this.pageDebut!=0)+id;
			this.drawSkills();
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
						this.cardTypesNbCards[model.skillsList[i].CardType]++;
						idCards.Add (model.cardIdsList[j]);
					}
					this.skillsNbCards[i]++;
					if(this.skillsPercentages[i]<model.ownSkillsList[j].Power)
					{
						this.skillsPercentages[i]=model.ownSkillsList[j].Power;
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
				if(model.skillsList[j].CardType==i)
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
	public void displayCardTypesCardsHandler()
	{
		ApplicationModel.cardTypeChosen = this.selectedCardTypeId;
		Application.LoadLevel ("newMyGame");
	}
	public void displaySkillCardsHandler(int id)
	{
		ApplicationModel.skillChosen = model.skillsList[this.skillsDisplayed[id]].Name;
		Application.LoadLevel ("newMyGame");
	}
	public void drawCollectionLevel()
	{
		Color starsColor = new Color ();
		if(this.globalPercentage>90)
		{
			this.collectionLevel.transform.FindChild("Star4").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star3").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star2").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star1").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star0").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			starsColor=new Color(255f/255f,232f/255f,99f/255f);
		}
		else if(this.globalPercentage>80)
		{
			this.collectionLevel.transform.FindChild("Star4").GetComponent<SpriteRenderer>().sprite=this.starsPictos[1];
			this.collectionLevel.transform.FindChild("Star3").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star2").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star1").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star0").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			starsColor=new Color(255f/255f,232f/255f,99f/255f);
		}
		else if(this.globalPercentage>70)
		{
			this.collectionLevel.transform.FindChild("Star4").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star3").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star2").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star1").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star0").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			starsColor=new Color(168f/255f,168f/255f,168f/255f);
		}
		else if(this.globalPercentage>60)
		{
			this.collectionLevel.transform.FindChild("Star4").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star3").GetComponent<SpriteRenderer>().sprite=this.starsPictos[1];
			this.collectionLevel.transform.FindChild("Star2").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star1").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star0").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			starsColor=new Color(168f/255f,168f/255f,168f/255f);
		}
		else if(this.globalPercentage>50)
		{
			this.collectionLevel.transform.FindChild("Star4").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star3").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star2").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star1").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star0").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			starsColor= new Color(170f/255f,124f/255f,80f/255f);
		}
		else if(this.globalPercentage>40)
		{
			this.collectionLevel.transform.FindChild("Star4").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star3").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star2").GetComponent<SpriteRenderer>().sprite=this.starsPictos[1];
			this.collectionLevel.transform.FindChild("Star1").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star0").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			starsColor= new Color(170f/255f,124f/255f,80f/255f);
		}
		else if(this.globalPercentage>30)
		{
			this.collectionLevel.transform.FindChild("Star4").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star3").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star2").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star1").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			this.collectionLevel.transform.FindChild("Star0").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			starsColor= new Color(170f/255f,124f/255f,80f/255f);
		}
		else if(this.globalPercentage>20)
		{
			this.collectionLevel.transform.FindChild("Star4").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star3").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star2").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star1").GetComponent<SpriteRenderer>().sprite=this.starsPictos[1];
			this.collectionLevel.transform.FindChild("Star0").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			starsColor= new Color(170f/255f,124f/255f,80f/255f);
		}
		else if(this.globalPercentage>10)
		{
			this.collectionLevel.transform.FindChild("Star4").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star3").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star2").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star1").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star0").GetComponent<SpriteRenderer>().sprite=this.starsPictos[2];
			starsColor= new Color(170f/255f,124f/255f,80f/255f);
		}
		else
		{
			this.collectionLevel.transform.FindChild("Star4").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star3").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star2").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star1").GetComponent<SpriteRenderer>().sprite=this.starsPictos[0];
			this.collectionLevel.transform.FindChild("Star0").GetComponent<SpriteRenderer>().sprite=this.starsPictos[1];
			starsColor= new Color(170f/255f,124f/255f,80f/255f);
		}
		this.collectionLevel.transform.FindChild ("Star0").GetComponent<SpriteRenderer> ().color = starsColor;
		this.collectionLevel.transform.FindChild ("Star1").GetComponent<SpriteRenderer> ().color = starsColor;
		this.collectionLevel.transform.FindChild ("Star2").GetComponent<SpriteRenderer> ().color = starsColor;
		this.collectionLevel.transform.FindChild ("Star3").GetComponent<SpriteRenderer> ().color = starsColor;
		this.collectionLevel.transform.FindChild ("Star4").GetComponent<SpriteRenderer> ().color = starsColor;
	}
	public void escapePressed()
	{
		if(newMenuController.instance.isAPopUpDisplayed())
		{
			newMenuController.instance.hideAllPopUp();
		}
	}
}