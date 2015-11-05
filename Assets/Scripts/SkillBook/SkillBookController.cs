//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Linq;
//
//public class SkillBookController : Photon.MonoBehaviour
//{
//	
//	public static SkillBookController instance;
//	private SkillBookModel model;
//	private SkillBookView view;
//	public GameObject MenuObject;
//	public GameObject TutorialObject;
//	public GUIStyle[] skillsVMStyle;
//	public GUIStyle[] bookVMStyle;
//	public GUIStyle[] statsVMStyle;
//	public GUIStyle[] cTypesVMStyle;
//	public GUIStyle[] cTypeSelectedVMStyle;
//	public Texture2D[] cardTypesTab;
//	public Texture2D[] cardTypesSelectedTab;
//	public Texture2D[] openBookBackgrounds;
//	public Texture2D[] cardTypePictos;
//	public Texture2D[] gaugesLevels;
//	public Texture2D[] starsPictos;
//	private int[] skillsPercentages;
//	private int[] skillsNbCards;
//	private int[] cardTypesNbSkillsOwn;
//	private int[] cardTypesNbSkills;
//	private int[] cardTypesNbCards;
//	private int globalPercentage;
//	private bool isTutorialLaunched;
//	private GameObject tutorial;
//	
//	void Start()
//	{
//		instance = this;
//		this.model = new SkillBookModel ();
//		this.view = Camera.main.gameObject.AddComponent <SkillBookView>();
//		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
//		StartCoroutine (this.initialization());
//	}
//	private IEnumerator initialization ()
//	{
//		yield return StartCoroutine (model.getSkillBookData ());
//		this.initStyles ();
//		this.resize ();
//		this.computeIndicators ();
//		this.initVM ();
//		this.displayCTypeSelected ();
//		this.initSkillsPagination ();
//		this.displaySkillsPage ();
//		if(!model.player.SkillBookTutorial)
//		{
//			this.tutorial = Instantiate(this.TutorialObject) as GameObject;
//			MenuObject.GetComponent<MenuController>().setTutorialLaunched(true);
//			this.tutorial.AddComponent<SkillBookTutorialController>();
//			this.tutorial.GetComponent<SkillBookTutorialController>().launchSequence(0);
//			this.isTutorialLaunched=true;
//		}
//	}
//	public void loadAll()
//	{
//		this.resize ();
//		this.initSkillsPagination();
//		this.displaySkillsPage ();
//		if(isTutorialLaunched)
//		{
//			this.tutorial.GetComponent<SkillBookTutorialController>().resize();
//		}
//	}
//	private void computeIndicators()
//	{
//		this.skillsPercentages=new int[model.skillsList.Count];
//		this.skillsNbCards=new int[model.skillsList.Count];
//		this.cardTypesNbSkillsOwn=new int[model.cardTypesList.Count];
//		this.cardTypesNbSkills=new int[model.cardTypesList.Count];
//		this.cardTypesNbCards=new int[model.cardTypesList.Count];
//		int globalSum=new int();
//		IList<int> idCards = new List<int> ();
//		for(int i=0;i<model.skillsList.Count;i++)
//		{
//			for(int j=0;j<model.ownSkillsList.Count;j++)
//			{
//				if(model.skillsList[i].Id==model.ownSkillsList[j].Id)
//				{
//					if(!idCards.Contains(model.cardIdsList[j]))
//					{
//						this.cardTypesNbCards[model.skillsList[i].CardType]++;
//						idCards.Add (model.cardIdsList[j]);
//					}
//					this.skillsNbCards[i]++;
//					if(this.skillsPercentages[i]<model.ownSkillsList[j].Power)
//					{
//						this.skillsPercentages[i]=model.ownSkillsList[j].Power;
//					}
//				}
//			}
//			globalSum=globalSum+this.skillsPercentages[i];
//		}
//		if(this.model.skillsList.Count>0)
//		{
//			this.globalPercentage=globalSum/this.model.skillsList.Count;
//		}
//		for (int i=0;i<model.cardTypesList.Count;i++)
//		{
//			for(int j=0;j<model.skillsList.Count;j++)
//			{
//				if(model.skillsList[j].CardType==i)
//				{
//					this.cardTypesNbSkills[i]++;
//					if(this.skillsNbCards[j]>0)
//					{
//						this.cardTypesNbSkillsOwn[i]++;
//					}
//				}
//			}
//		}
//	}
//	private void initVM()
//	{
//		for(int i =0;i<model.cardTypesList.Count-1;i++)
//		{
//			view.cTypesVM.tabButtonsStyles.Add (new GUIStyle());
//			if(i==0)
//			{
//				view.cTypesVM.tabButtonsStyles[i].normal.background=this.cardTypesSelectedTab[i];
//			}
//			else
//			{
//				view.cTypesVM.tabButtonsStyles[i].normal.background=this.cardTypesTab[i];
//			}
//			view.cTypesVM.tabButtonsStyles[i].hover.background=this.cardTypesSelectedTab[i];
//		}
//		if(this.globalPercentage>90)
//		{
//			view.statsVM.stars[4].normal.background=this.starsPictos[6];
//			view.statsVM.stars[3].normal.background=this.starsPictos[6];
//			view.statsVM.stars[2].normal.background=this.starsPictos[6];
//			view.statsVM.stars[1].normal.background=this.starsPictos[6];
//			view.statsVM.stars[0].normal.background=this.starsPictos[6];
//		}
//		else if(this.globalPercentage>80)
//		{
//			view.statsVM.stars[4].normal.background=this.starsPictos[5];
//			view.statsVM.stars[3].normal.background=this.starsPictos[6];
//			view.statsVM.stars[2].normal.background=this.starsPictos[6];
//			view.statsVM.stars[1].normal.background=this.starsPictos[6];
//			view.statsVM.stars[0].normal.background=this.starsPictos[6];
//		}
//		else if(this.globalPercentage>70)
//		{
//			view.statsVM.stars[4].normal.background=this.starsPictos[0];
//			view.statsVM.stars[3].normal.background=this.starsPictos[6];
//			view.statsVM.stars[2].normal.background=this.starsPictos[6];
//			view.statsVM.stars[1].normal.background=this.starsPictos[6];
//			view.statsVM.stars[0].normal.background=this.starsPictos[6];
//		}
//		else if(this.globalPercentage>60)
//		{
//			view.statsVM.stars[4].normal.background=this.starsPictos[0];
//			view.statsVM.stars[3].normal.background=this.starsPictos[3];
//			view.statsVM.stars[2].normal.background=this.starsPictos[4];
//			view.statsVM.stars[1].normal.background=this.starsPictos[4];
//			view.statsVM.stars[0].normal.background=this.starsPictos[4];
//		}
//		else if(this.globalPercentage>50)
//		{
//			view.statsVM.stars[4].normal.background=this.starsPictos[0];
//			view.statsVM.stars[3].normal.background=this.starsPictos[0];
//			view.statsVM.stars[2].normal.background=this.starsPictos[4];
//			view.statsVM.stars[1].normal.background=this.starsPictos[4];
//			view.statsVM.stars[0].normal.background=this.starsPictos[4];
//		}
//		else if(this.globalPercentage>40)
//		{
//			view.statsVM.stars[4].normal.background=this.starsPictos[0];
//			view.statsVM.stars[3].normal.background=this.starsPictos[0];
//			view.statsVM.stars[2].normal.background=this.starsPictos[3];
//			view.statsVM.stars[1].normal.background=this.starsPictos[4];
//			view.statsVM.stars[0].normal.background=this.starsPictos[4];
//		}
//		else if(this.globalPercentage>30)
//		{
//			view.statsVM.stars[4].normal.background=this.starsPictos[0];
//			view.statsVM.stars[3].normal.background=this.starsPictos[0];
//			view.statsVM.stars[2].normal.background=this.starsPictos[0];
//			view.statsVM.stars[1].normal.background=this.starsPictos[2];
//			view.statsVM.stars[0].normal.background=this.starsPictos[2];
//		}
//		else if(this.globalPercentage>20)
//		{
//			view.statsVM.stars[4].normal.background=this.starsPictos[0];
//			view.statsVM.stars[3].normal.background=this.starsPictos[0];
//			view.statsVM.stars[2].normal.background=this.starsPictos[0];
//			view.statsVM.stars[1].normal.background=this.starsPictos[1];
//			view.statsVM.stars[0].normal.background=this.starsPictos[2];
//		}
//		else if(this.globalPercentage>10)
//		{
//			view.statsVM.stars[4].normal.background=this.starsPictos[0];
//			view.statsVM.stars[3].normal.background=this.starsPictos[0];
//			view.statsVM.stars[2].normal.background=this.starsPictos[0];
//			view.statsVM.stars[1].normal.background=this.starsPictos[0];
//			view.statsVM.stars[0].normal.background=this.starsPictos[2];
//		}
//		else
//		{
//			view.statsVM.stars[4].normal.background=this.starsPictos[0];
//			view.statsVM.stars[3].normal.background=this.starsPictos[0];
//			view.statsVM.stars[2].normal.background=this.starsPictos[0];
//			view.statsVM.stars[1].normal.background=this.starsPictos[0];
//			view.statsVM.stars[0].normal.background=this.starsPictos[1];
//		}
//	}
//	private void initStyles()
//	{
//		view.skillsVM.styles=new GUIStyle[this.skillsVMStyle.Length];
//		for(int i=0;i<this.skillsVMStyle.Length;i++)
//		{
//			view.skillsVM.styles[i]=this.skillsVMStyle[i];
//		}
//		view.skillsVM.initStyles();
//		view.bookVM.styles=new GUIStyle[this.bookVMStyle.Length];
//		for(int i=0;i<this.bookVMStyle.Length;i++)
//		{
//			view.bookVM.styles[i]=this.bookVMStyle[i];
//		}
//		view.bookVM.initStyles();
//		view.cTypesVM.styles=new GUIStyle[this.cTypesVMStyle.Length];
//		for(int i=0;i<this.cTypesVMStyle.Length;i++)
//		{
//			view.cTypesVM.styles[i]=this.cTypesVMStyle[i];
//		}
//		view.cTypesVM.initStyles();
//		view.cTypeSelectedVM.styles=new GUIStyle[this.cTypeSelectedVMStyle.Length];
//		for(int i=0;i<this.cTypeSelectedVMStyle.Length;i++)
//		{
//			view.cTypeSelectedVM.styles[i]=this.cTypeSelectedVMStyle[i];
//		}
//		view.cTypeSelectedVM.initStyles();
//		view.statsVM.styles=new GUIStyle[this.statsVMStyle.Length];
//		for(int i=0;i<this.statsVMStyle.Length;i++)
//		{
//			view.statsVM.styles[i]=this.statsVMStyle[i];
//		}
//		view.statsVM.initStyles();
//	}
//	private void initSkillsPagination()
//	{
//		view.skillsVM.nbPages = Mathf.CeilToInt(((float)view.skillsVM.skillsToBeDisplayed.Count) / (5f*(float)view.skillsVM.elementPerRow));
//		view.skillsVM.chosenPage = 0;
//	}
//	public void displayCTypeSelected()
//	{
//		view.skillsVM.skillsToBeDisplayed = new List<int> ();
//		view.skillsVM.percentages = new List<int> ();
//		for(int i=0;i<model.skillsList.Count;i++)
//		{
//			if(model.skillsList[i].CardType==view.cTypeSelectedVM.ctypeSelected)
//			{
//				view.skillsVM.skillsToBeDisplayed.Add (i);
//				view.skillsVM.percentages.Add (this.computePercentage(i));
//			}
//		}
//		view.cTypeSelectedVM.name = model.cardTypesList [view.cTypeSelectedVM.ctypeSelected];
//		view.cTypeSelectedVM.pictureStyle.normal.background = this.cardTypePictos [view.cTypeSelectedVM.ctypeSelected];
//		if(this.cardTypesNbCards [view.cTypeSelectedVM.ctypeSelected]>1)
//		{
//			view.cTypeSelectedVM.nbCards = this.cardTypesNbCards [view.cTypeSelectedVM.ctypeSelected].ToString()+" cartes";
//		}
//		else
//		{
//			view.cTypeSelectedVM.nbCards = this.cardTypesNbCards [view.cTypeSelectedVM.ctypeSelected].ToString()+" carte";
//		}
//		if(this.cardTypesNbCards [view.cTypeSelectedVM.ctypeSelected]>0)
//		{
//			view.cTypeSelectedVM.displayButton=true;
//		}
//		else
//		{
//			view.cTypeSelectedVM.displayButton=false;
//		}
//		if(this.cardTypesNbSkillsOwn [view.cTypeSelectedVM.ctypeSelected]>1)
//		{
//			view.cTypeSelectedVM.percentage = this.cardTypesNbSkillsOwn [view.cTypeSelectedVM.ctypeSelected].ToString() + "/" + this.cardTypesNbSkills [view.cTypeSelectedVM.ctypeSelected].ToString() + " competences acquises";
//		}
//		else
//		{
//			view.cTypeSelectedVM.percentage = this.cardTypesNbSkillsOwn [view.cTypeSelectedVM.ctypeSelected].ToString() + "/" + this.cardTypesNbSkills [view.cTypeSelectedVM.ctypeSelected].ToString() + " competence acquise";
//		}
//		if(this.cardTypesNbSkills [view.cTypeSelectedVM.ctypeSelected]>0)
//		{
//			view.cTypeSelectedVM.gaugeWidth=(float)this.cardTypesNbSkillsOwn [view.cTypeSelectedVM.ctypeSelected]/(float)this.cardTypesNbSkills [view.cTypeSelectedVM.ctypeSelected];
//		}
//		view.cTypeSelectedVM.gaugeBackgroundWidth = 1 - (float)view.cTypeSelectedVM.gaugeWidth;
//		if(view.cTypeSelectedVM.gaugeWidth>0.9)
//		{
//			view.cTypeSelectedVM.gaugeStyle.normal.background=this.gaugesLevels[2];
//		}
//		else if(view.cTypeSelectedVM.gaugeWidth>0.6)
//		{
//			view.cTypeSelectedVM.gaugeStyle.normal.background=this.gaugesLevels[1];
//		}
//		else
//		{
//			view.cTypeSelectedVM.gaugeStyle.normal.background=this.gaugesLevels[0];
//		}
//	}
//	public void selectCardType(int chosenCardType)
//	{
//		view.cTypesVM.tabButtonsStyles[chosenCardType].normal.background=this.cardTypesSelectedTab[chosenCardType];
//		view.cTypesVM.tabButtonsStyles[view.cTypeSelectedVM.ctypeSelected].normal.background=this.cardTypesTab[view.cTypeSelectedVM.ctypeSelected];
//		view.cTypeSelectedVM.ctypeSelected = chosenCardType;
//		this.displayCTypeSelected ();
//		this.initSkillsPagination ();
//		this.displaySkillsPage ();
//	}
//	public void selectNextPage()
//	{
//		view.skillsVM.chosenPage = view.skillsVM.chosenPage + 1;
//		this.displaySkillsPage ();
//	}
//	public void selectBackPage()
//	{
//		view.skillsVM.chosenPage = view.skillsVM.chosenPage - 1;
//		this.displaySkillsPage ();
//	}
//	private void displaySkillsPage()
//	{	
//		view.skillsVM.start = view.skillsVM.chosenPage*5*view.skillsVM.elementPerRow;
//		if (view.skillsVM.skillsToBeDisplayed.Count < (5*view.skillsVM.elementPerRow*(view.skillsVM.chosenPage+1)))
//		{
//			view.skillsVM.finish = view.skillsVM.skillsToBeDisplayed.Count;
//		}
//		else
//		{
//			view.skillsVM.finish = (view.skillsVM.chosenPage+1)*(5*view.skillsVM.elementPerRow);
//		}
//		view.skillsVM.names = new List<string> ();
//		view.skillsVM.descriptions = new List<string> ();
//		view.skillsVM.pictures = new List<GUIStyle> ();
//		view.skillsVM.percentages = new List<int> ();
//		view.skillsVM.gaugesWidth = new List<float> ();
//		view.skillsVM.gaugesBackgroundWidth = new List<float> ();
//		view.skillsVM.gauges = new List<GUIStyle> ();
//		view.skillsVM.nbCards = new List<string> ();
//		view.skillsVM.displayButtons = new List<bool> ();
//		for (int i=view.skillsVM.start;i<view.skillsVM.finish;i++)
//		{
//			view.skillsVM.names.Add (model.skillsList[view.skillsVM.skillsToBeDisplayed[i]].Name);
//			view.skillsVM.descriptions.Add (model.skillsList[view.skillsVM.skillsToBeDisplayed[i]].Description);
//			view.skillsVM.pictures.Add (new GUIStyle());
//			view.skillsVM.pictures[i-view.skillsVM.start].normal.background=model.skillsList[view.skillsVM.skillsToBeDisplayed[i]].texture;
//			StartCoroutine(model.skillsList[view.skillsVM.skillsToBeDisplayed[i]].setPicture());
//			if(this.skillsNbCards [view.skillsVM.skillsToBeDisplayed[i]]>1)
//			{
//				view.skillsVM.nbCards.Add (this.skillsNbCards [view.skillsVM.skillsToBeDisplayed[i]].ToString()+" cartes");
//			}
//			else
//			{
//				view.skillsVM.nbCards.Add (this.skillsNbCards [view.skillsVM.skillsToBeDisplayed[i]].ToString()+" carte");
//			}
//			if( this.skillsNbCards [view.skillsVM.skillsToBeDisplayed[i]]>0)
//			{
//				view.skillsVM.displayButtons.Add (true);
//			}
//			else
//			{
//				view.skillsVM.displayButtons.Add (false);
//			}
//			view.skillsVM.percentages.Add (this.skillsPercentages[view.skillsVM.skillsToBeDisplayed[i]]);
//			if((float)this.skillsPercentages[view.skillsVM.skillsToBeDisplayed[i]]/100f<1)
//			{
//				view.skillsVM.gaugesWidth.Add((float)this.skillsPercentages[view.skillsVM.skillsToBeDisplayed[i]]/100f);
//			}
//			else
//			{
//				view.skillsVM.gaugesWidth.Add(1);
//			}
//			view.skillsVM.gaugesBackgroundWidth.Add (1 - (float)view.skillsVM.gaugesWidth[i-view.skillsVM.start]);
//			view.skillsVM.gauges.Add (new GUIStyle());
//			if(view.skillsVM.gaugesWidth[i-view.skillsVM.start]>0.9)
//			{
//				view.skillsVM.gauges[i-view.skillsVM.start].normal.background=this.gaugesLevels[2];
//			}
//			else if(view.skillsVM.gaugesWidth[i-view.skillsVM.start]>0.6)
//			{
//				view.skillsVM.gauges[i-view.skillsVM.start].normal.background=this.gaugesLevels[1];
//			}
//			else
//			{
//				view.skillsVM.gauges[i-view.skillsVM.start].normal.background=this.gaugesLevels[0];
//			}
//		}
//	}
//	private int computePercentage(int skillIndex)
//	{
//		int percentage = 0;
//		for(int i=0;i<model.ownSkillsList.Count;i++)
//		{
//			if(model.ownSkillsList[i].Id==model.skillsList[skillIndex].Id&&model.ownSkillsList[i].Power>percentage)
//			{
//				percentage=model.ownSkillsList[i].Power;
//			}
//		}
//		return percentage;
//	}
//	public void resize()
//	{
//		view.screenVM.resize ();
//		view.skillsVM.resize (view.screenVM.heightScreen);
//		view.bookVM.resize (view.screenVM.heightScreen);
//		view.cTypesVM.resize (view.screenVM.heightScreen);
//		view.cTypeSelectedVM.resize (view.screenVM.heightScreen);
//		view.statsVM.resize (view.screenVM.heightScreen);
//		if(view.screenVM.widthScreen>1.4f*view.screenVM.heightScreen)
//		{
//			view.skillsVM.elementPerRow = 2;
//			view.bookVM.bookBackgroundStyle.normal.background=this.openBookBackgrounds[0];
//			view.skillsVM.blocksWidth = 0.46f*view.screenVM.blockSkillsWidth;
//			view.skillsVM.gapBetweenBlocks = 0.04f * view.screenVM.blockSkillsWidth;
//			view.screenVM.blockBackButton = new Rect (view.screenVM.blockSkills.xMin,
//			                                          view.screenVM.blockCTypes.yMin,
//			                                          view.screenVM.blockBackButtonWidth,
//			                                          view.screenVM.blockBackButtonHeight);
//
//		}
//		else
//		{
//			view.skillsVM.elementPerRow = 1;
//			view.bookVM.bookBackgroundStyle.normal.background=this.openBookBackgrounds[1];
//			view.skillsVM.blocksWidth = 0.88f*view.screenVM.blockSkillsWidth;
//			view.skillsVM.gapBetweenBlocks = 0.12f * view.screenVM.blockSkillsWidth;
//			view.screenVM.blockBackButton = new Rect (view.screenVM.blockSkills.xMin+view.skillsVM.gapBetweenBlocks*0.75f,
//			                                          view.screenVM.blockCTypes.yMin,
//			                                          view.screenVM.blockBackButtonWidth,
//			                                          view.screenVM.blockBackButtonHeight);
//		}
//		view.skillsVM.blocksHeight = view.screenVM.blockSkillsHeight / 5f;
//		view.screenVM.blockCTypeSelectedWidth = view.skillsVM.blocksWidth;
//		view.screenVM.blockCTypeSelected = new Rect (view.screenVM.blockSkills.xMin+view.skillsVM.gapBetweenBlocks,
//		                                             view.screenVM.blockBook.yMin,
//		                                             view.skillsVM.blocksWidth,
//		                                             view.screenVM.blockCTypeSelectedHeight);
//		view.skillsVM.blocks=new Rect[5*view.skillsVM.elementPerRow];
//		for(int i=0;i<view.skillsVM.blocks.Length;i++)
//		{
//			view.skillsVM.blocks[i]=new Rect(view.skillsVM.gapBetweenBlocks+view.screenVM.blockSkills.xMin+(i%view.skillsVM.elementPerRow)*(view.skillsVM.blocksWidth+view.skillsVM.gapBetweenBlocks),
//			                                 view.screenVM.blockSkills.yMin+0.2f*view.skillsVM.blocksHeight+Mathf.FloorToInt(i/view.skillsVM.elementPerRow)*view.skillsVM.blocksHeight,
//			                                 view.skillsVM.blocksWidth,
//			                                 view.skillsVM.blocksHeight);
//		}
//	}
//	public void displayCardsWidhCardType()
//	{
//		ApplicationModel.cardTypeChosen = view.cTypeSelectedVM.ctypeSelected;
//		Application.LoadLevel ("MyGame");
//	}
//	public void displayCardsWidhSkill(int chosenSkill)
//	{
//		ApplicationModel.skillChosen = view.skillsVM.names [chosenSkill];
//		Application.LoadLevel ("MyGame");
//	}
//	public void setButtonsGui(bool value)
//	{
//		view.bookVM.buttonsEnabled =value;
//	}
//	public IEnumerator endTutorial()
//	{
//		yield return StartCoroutine (model.player.setSkillBookTutorial(true));
//		MenuController.instance.setButtonsGui (true);
//		Destroy (this.tutorial);
//		this.isTutorialLaunched = false;
//		MenuController.instance.isTutorialLaunched = false;
//		this.setButtonsGui (true);
//	}
//}
