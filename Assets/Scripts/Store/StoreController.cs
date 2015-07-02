using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

public class StoreController : MonoBehaviour
{
	public static StoreController instance;
	public GameObject MenuObject;
	public GameObject CardObject;
	public GameObject TutorialObject;
	public GUIStyle[] storeVMStyle;
	public GUIStyle[] packsVMStyle;
	public GUIStyle[] popUpVMStyle;
	public GUIStyle[] popUpCollectionVMStyle;

	private StoreView view;
	private StoreModel model;
	private StoreErrorPopUpView errorPopUpView;
	private StoreAddCreditsPopUpView addCreditsPopUpView;
	private StoreSelectCardTypePopUpView selectCardTypePopUpView;
	private StoreNewSkillsPopUpView newSkillsPopUpView;
	private StoreCollectionPointsPopUpView collectionPointsPopUpView;
	private GameObject[] randomCards;
	private GameObject cardFocused;
	private bool[] toRotate;
	private bool startRotation;
	private float speed;
	private float angle;
	private Quaternion target;
	private GameObject cardPopUpBelongTo;
	private int selectedPackIndex;
	private bool isTutorialLaunched;
	private GameObject tutorial;
	
	public StoreController ()
	{
	}
	void Start()
	{
		instance = this;
		this.speed = 300.0f;
		this.model = new StoreModel ();
		this.view = Camera.main.gameObject.AddComponent <StoreView>();
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		StartCoroutine (this.initialization ());

	}
	void Update () 
	{
		if (this.startRotation)
		{
			for(int i=0;i<model.packList[this.selectedPackIndex].NbCards;i++)
			{
				if(toRotate[i])
				{
					if(this.angle==360)
					{
						this.angle=180;
					}
					
					this.angle = this.angle + this.speed * Time.deltaTime;
					if(this.angle>=360)
					{
						this.angle=360;
						this.toRotate[i]=false;
						if(i<model.packList[this.selectedPackIndex].NbCards-1)
						{
							this.toRotate[i+1]=true;
						}
						else
						{
							if(model.packList[this.selectedPackIndex].NbCards>1)
							{
								view.storeVM.areMoreThan1CardDisplayed = true;
							}
							else
							{
								this.randomCards[0].GetComponent<CardStoreController>().setFocusStoreFeatures();
								this.randomCards[0].GetComponent<CardController> ().setCentralWindowRect (view.storeScreenVM.centralWindow);
								this.randomCards[0].GetComponent<CardController> ().setCollectionPointsWindowRect (view.storeScreenVM.collectionPointsWindow);
								this.randomCards[0].GetComponent<CardController> ().setNewSkillsWindowRect (view.storeScreenVM.newSkillsWindow);
								this.randomCards[0].GetComponent<CardController> ().setNewCardTypeWindowRect(view.storeScreenVM.centralWindow);
							}
							view.storeVM.guiEnabled=true;
							this.startRotation=false;
							if(isTutorialLaunched)
							{
								if(this.tutorial.GetComponent<StoreTutorialController>().getSequenceID()==2)
								{
									this.tutorial.GetComponent<StoreTutorialController>().actionIsDone();
								}
							}
						}
					}
					this.target = Quaternion.Euler(0, this.angle, 0);
					this.randomCards[i].transform.rotation = target;
				}
			}
		}
	}
	private IEnumerator initialization()
	{
		yield return(StartCoroutine(this.model.initializeStore()));
		this.initVM ();
		this.initStyles ();
		this.initializePagination ();
		this.displayPage ();
		this.resize ();
		if(model.player.TutorialStep==6)
		{
			this.tutorial = Instantiate(this.TutorialObject) as GameObject;
			MenuObject.GetComponent<MenuController>().setTutorialLaunched(true);
			this.tutorial.AddComponent<StoreTutorialController>();
			this.tutorial.GetComponent<StoreTutorialController>().launchSequence(0);
			this.isTutorialLaunched=true;
		}
		if(ApplicationModel.packToBuy!=-1)
		{
			this.buyPackFromHomePage(ApplicationModel.packToBuy);
			ApplicationModel.packToBuy = -1;
		}
	}
	private void initVM()
	{
		view.storeVM.canAddCredits = model.player.IsAdmin;
	}
	private void displayPage()
	{
		view.packsVM.packNames = new List<string> ();
		view.packsVM.packPrices = new List<int> ();
		view.packsVM.isNew = new List<bool> ();
		view.packsVM.packPictureStyles = new List<GUIStyle> ();
		view.packsVM.guiEnabled = new List<bool> ();
		view.packsVM.start = view.packsVM.chosenPage * view.packsVM.nbElementsToDisplay;
		view.packsVM.finish = model.packList.Count;
		if((view.packsVM.chosenPage+1)*view.packsVM.nbElementsToDisplay<model.packList.Count)
		{
			view.packsVM.finish=(view.packsVM.chosenPage+1)*view.packsVM.nbElementsToDisplay;
		}
		for(int i=view.packsVM.start;i<view.packsVM.finish;i++)
		{
			view.packsVM.packPictureStyles.Add(new GUIStyle());
			view.packsVM.packPictureStyles[i-view.packsVM.start].normal.background=model.packList[i].texture;
			StartCoroutine(model.packList[i].setPicture());
			view.packsVM.packNames.Add (model.packList[i].Name);
			view.packsVM.packPrices.Add (model.packList[i].Price);
			view.packsVM.isNew.Add(model.packList[i].New);
			if(model.player.CardTypesAllowed.Contains(model.packList[i].CardType) || model.packList[i].CardType<0)
			{
				view.packsVM.guiEnabled.Add (true);
			}
			else
			{
				view.packsVM.guiEnabled.Add(false);
			}
		}
	}
	private void initStyles()
	{
		view.storeVM.styles=new GUIStyle[this.storeVMStyle.Length];
		for(int i=0;i<this.storeVMStyle.Length;i++)
		{
			view.storeVM.styles[i]=this.storeVMStyle[i];
		}
		view.storeVM.initStyles();
		view.packsVM.styles=new GUIStyle[this.packsVMStyle.Length];
		for(int i=0;i<this.packsVMStyle.Length;i++)
		{
			view.packsVM.styles[i]=this.packsVMStyle[i];
		}
		view.packsVM.initStyles();
	}
	public void resize()
	{
		view.storeScreenVM.resize ();
		view.storeVM.resize (view.storeScreenVM.heightScreen);
		view.packsVM.resize (view.storeScreenVM.heightScreen);
		if(this.errorPopUpView!=null)
		{
			this.errorPopUpResize();
		}
		if(this.addCreditsPopUpView!=null)
		{
			this.addCreditsPopUpResize();
		}
		if(this.cardFocused!=null)
		{
			this.cardFocused.GetComponent<CardController> ().setCentralWindowRect (view.storeScreenVM.centralWindow);
			this.cardFocused.GetComponent<CardController> ().setCollectionPointsWindowRect (view.storeScreenVM.collectionPointsWindow);
			this.cardFocused.GetComponent<CardController> ().setNewSkillsWindowRect (view.storeScreenVM.newSkillsWindow);
			this.cardFocused.GetComponent<CardController> ().setNewCardTypeWindowRect(view.storeScreenVM.centralWindow);
			this.cardFocused.GetComponent<CardController>().resize();
		}
		if(this.randomCards!=null)
		{
			if(this.randomCards.Length==1)
			{
				this.randomCards[0].GetComponent<CardController> ().setCentralWindowRect (view.storeScreenVM.centralWindow);
				this.randomCards[0].GetComponent<CardController> ().setCollectionPointsWindowRect (view.storeScreenVM.collectionPointsWindow);
				this.randomCards[0].GetComponent<CardController> ().setNewSkillsWindowRect (view.storeScreenVM.newSkillsWindow);
				this.randomCards[0].GetComponent<CardController> ().setNewCardTypeWindowRect(view.storeScreenVM.centralWindow);
				this.randomCards[0].GetComponent<CardController>().resize();
			}
			else
			{
				this.resizeRandomCards();
			}
		}
		if(this.selectCardTypePopUpView!=null)
		{
			this.selectCardTypePopUpResize();
		}
		if(this.collectionPointsPopUpView!=null)
		{
			this.collectionPointsPopUpResize();
		}
		if(this.newSkillsPopUpView!=null)
		{
			this.newSkillsPopUpResize();
		}
		if(isTutorialLaunched)
		{
			this.tutorial.GetComponent<StoreTutorialController>().resize();
		}
	}
	public void rightClickedCard(GameObject gameObject)
	{
		if(view.storeVM.areMoreThan1CardDisplayed && !this.startRotation)
		{
			this.hideCards();
			string name = "Fcrd"+gameObject.name.Substring(4);
			Vector3 scale = new Vector3(view.storeScreenVM.heightScreen / 140f,view.storeScreenVM.heightScreen / 140f,view.storeScreenVM.heightScreen / 140f);
			Vector3 position = Camera.main.ScreenToWorldPoint (new Vector3 (0.4f * view.storeScreenVM.widthScreen, 0.45f * view.storeScreenVM.heightScreen - 1, 10));
			this.cardFocused = Instantiate(CardObject) as GameObject;
			this.cardFocused.AddComponent<CardStoreController> ();
			this.cardFocused.GetComponent<CardController> ().setGameObject (name, scale, position);
			this.cardFocused.GetComponent<CardStoreController>().setStoreCard(gameObject.GetComponent<CardController> ().card);
			this.cardFocused.GetComponent<CardStoreController> ().setFocusStoreFeatures ();
			this.cardFocused.GetComponent<CardController> ().setCentralWindowRect (view.storeScreenVM.centralWindow);
			this.cardFocused.GetComponent<CardController> ().setCollectionPointsWindowRect (view.storeScreenVM.collectionPointsWindow);
			this.cardFocused.GetComponent<CardController> ().setNewSkillsWindowRect (view.storeScreenVM.newSkillsWindow);
			this.cardFocused.GetComponent<CardController> ().setNewCardTypeWindowRect(view.storeScreenVM.centralWindow);
		}
	}
	public void buyPackFromHomePage(int packId)
	{
		for(int i=0;i<model.packList.Count;i++)
		{
			if(model.packList[i].Id==packId)
			{
				this.selectedPackIndex=i;
				break;
			}
		}
		if(model.packList[selectedPackIndex].Price<=ApplicationModel.credits)
		{
			if(model.packList[selectedPackIndex].CardType==-2)
			{
				this.displaySelectCardPopUp();
			}
			else
			{
				StartCoroutine(this.getCards());
			}
		}
		else
		{
			this.displayErrorPopUp();
			errorPopUpView.errorPopUpVM.error="Vous n'avez pas assez de crédits pour acheter ce paquet ! A tout moment vous pouvez approvisionner votre portefeuille";
		}
	}
	public void buyPackHandler(int chosenPack)
	{
		this.selectedPackIndex = view.packsVM.start + chosenPack;
		if(model.packList[selectedPackIndex].CardType==-2)
		{
			this.displaySelectCardPopUp();
		}
		else
		{
			StartCoroutine(this.getCards());
		}
		if(isTutorialLaunched)
		{
			if(this.tutorial.GetComponent<StoreTutorialController>().getSequenceID()==1)
			{
				this.tutorial.GetComponent<StoreTutorialController>().actionIsDone();
			}
		}
	}
	public void buyPackWidthCardTypeHandler()
	{
		if(isCardTypeSelected())
		{
			selectCardTypePopUpView.selectCardTypePopUpVM.guiEnabled=false;
			StartCoroutine(this.getCards(model.player.CardTypesAllowed[selectCardTypePopUpView.selectCardTypePopUpVM.cardTypeSelected]));	
		}
	}
	private IEnumerator getCards(int cardType=-1)
	{
		yield return StartCoroutine (model.packList [selectedPackIndex].buyPack (cardType));
		if(selectCardTypePopUpView!=null)
		{
			this.hideSelectCardTypePopUp();
		}
		this.createRandomCards ();
	}
	private void createRandomCards()
	{
		this.refreshCredits();
		if(model.packList[this.selectedPackIndex].Error=="")
		{
			this.hideMainGUI();
			if(model.packList[this.selectedPackIndex].CollectionPoints>0)
			{
				StartCoroutine(this.displayCollectionPointsPopUp());
				if(model.packList[this.selectedPackIndex].NewSkills.Count>0)
				{
					StartCoroutine(this.displayNewSkillsPopUp());
				}
			}

			string name;
			Vector3 scale;
			Vector3 position=new Vector3(0,0,0);
			Quaternion rotation;
			float tempF = 2*Camera.main.GetComponent<Camera>().orthographicSize*view.storeScreenVM.widthScreen/view.storeScreenVM.heightScreen;
			float width = 0.75f * tempF;
			float scaleCard = width/(model.packList[this.selectedPackIndex].NbCards+1);
			if(scaleCard>view.storeScreenVM.heightScreen / 140f)
			{
				scaleCard=view.storeScreenVM.heightScreen / 140f;
			}
			this.randomCards = new GameObject[model.packList[this.selectedPackIndex].NbCards];
			this.toRotate= new bool[model.packList[this.selectedPackIndex].NbCards];
			for (int i = 0; i < model.packList[this.selectedPackIndex].NbCards; i++)
			{
				name="Card" + i;
				scale = new Vector3(scaleCard,scaleCard,scaleCard);
				if(model.packList[this.selectedPackIndex].NbCards>1)
				{
					position = new Vector3(-width/2+(scaleCard/2)+i*(scaleCard+1f/(model.packList[this.selectedPackIndex].NbCards-1)*scaleCard), 0f, 0f); 
				}
				rotation = Quaternion.Euler(0, 180, 0);
				this.toRotate[i]=false;
				this.randomCards [i] = Instantiate(this.CardObject) as GameObject;
				this.randomCards [i].AddComponent<CardStoreController>();
				this.randomCards [i].GetComponent<CardController>().setGameObject(name,scale,position);
				this.randomCards [i].GetComponent<CardStoreController>().setStoreCard(model.packList[this.selectedPackIndex].Cards[i]);
				this.randomCards [i].GetComponent<CardController>().setGameObjectRotation(rotation);
				if(isTutorialLaunched && i==0)
				{
					this.randomCards [i].GetComponent<CardStoreController>().setIsTutorialLaunched(true);
				}
			}
			this.startRotation = true;
			this.toRotate [0] = true;
			this.angle = 180;
		}
		else
		{
			this.displayErrorPopUp();
			errorPopUpView.errorPopUpVM.error=model.packList[this.selectedPackIndex].Error;
			model.packList[this.selectedPackIndex].Error="";
		}
	}
	private bool isCardTypeSelected()
	{
		if(selectCardTypePopUpView.selectCardTypePopUpVM.cardTypeSelected!=-1)
		{
			return true;
		}
		else
		{
			selectCardTypePopUpView.selectCardTypePopUpVM.error="Veuillez sélectionner une classe";
			return false;
		}
	}
	public void displayErrorPopUp()
	{
		this.disableMainGui ();
		this.errorPopUpView = Camera.main.gameObject.AddComponent <StoreErrorPopUpView>();
		errorPopUpView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
		for(int i=0;i<this.popUpVMStyle.Length;i++)
		{
			errorPopUpView.popUpVM.styles[i]=this.popUpVMStyle[i];
		}
		errorPopUpView.popUpVM.initStyles();
		this.errorPopUpResize ();
	}
	public void errorPopUpResize()
	{
		errorPopUpView.popUpVM.centralWindow = view.storeScreenVM.centralWindow;
		errorPopUpView.popUpVM.resize ();
	}
	public void hideErrorPopUp()
	{
		this.enableMainGui ();
		Destroy (this.errorPopUpView);
	}
	public void displaySelectCardPopUp()
	{
		this.disableMainGui ();
		this.selectCardTypePopUpView = Camera.main.gameObject.AddComponent <StoreSelectCardTypePopUpView>();
		selectCardTypePopUpView.selectCardTypePopUpVM.guiEnabled = true;
		selectCardTypePopUpView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
		for(int i=0;i<this.popUpVMStyle.Length;i++)
		{
			selectCardTypePopUpView.popUpVM.styles[i]=this.popUpVMStyle[i];
		}
		selectCardTypePopUpView.selectCardTypePopUpVM.cardTypes=new string[model.player.CardTypesAllowed.Count];
		for(int i =0;i<model.player.CardTypesAllowed.Count;i++)
		{
			selectCardTypePopUpView.selectCardTypePopUpVM.cardTypes[i]=model.cardTypeList[model.player.CardTypesAllowed[i]];
		}
		selectCardTypePopUpView.popUpVM.initStyles();
		this.selectCardTypePopUpResize ();
	}
	public void selectCardTypePopUpResize()
	{
		selectCardTypePopUpView.popUpVM.centralWindow = view.storeScreenVM.centralWindowSelectCardType;
		selectCardTypePopUpView.popUpVM.resize ();
	}
	public void hideSelectCardTypePopUp()
	{
		this.enableMainGui ();
		Destroy (this.selectCardTypePopUpView);
	}
	public void displayAddCreditsPopUp()
	{
		this.disableMainGui ();
		this.addCreditsPopUpView = Camera.main.gameObject.AddComponent <StoreAddCreditsPopUpView>();
		addCreditsPopUpView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
		for(int i=0;i<this.popUpVMStyle.Length;i++)
		{
			addCreditsPopUpView.popUpVM.styles[i]=this.popUpVMStyle[i];
		}
		addCreditsPopUpView.popUpVM.initStyles();
		this.addCreditsPopUpResize ();
	}
	public void addCreditsPopUpResize()
	{
		addCreditsPopUpView.popUpVM.centralWindow = view.storeScreenVM.centralWindow;
		addCreditsPopUpView.popUpVM.resize ();
	}
	public void hideAddCreditsPopUp()
	{
		Destroy (this.addCreditsPopUpView);
		this.enableMainGui ();
	}
	public IEnumerator displayCollectionPointsPopUp()
	{
		if(this.collectionPointsPopUpView!=null)
		{
			this.hideCollectionPointsPopUp();
		}
		this.collectionPointsPopUpView = Camera.main.gameObject.AddComponent <StoreCollectionPointsPopUpView>();
		collectionPointsPopUpView.storeCollectionPointsPopUpVM.collectionPoints = model.packList [this.selectedPackIndex].CollectionPoints;
		collectionPointsPopUpView.storeCollectionPointsPopUpVM.collectionPointsRanking = model.packList [this.selectedPackIndex].CollectionPointsRanking;
		collectionPointsPopUpView.storeCollectionPointsPopUpVM.styles=new GUIStyle[this.popUpCollectionVMStyle.Length];
		for(int i=0;i<this.popUpCollectionVMStyle.Length;i++)
		{
			collectionPointsPopUpView.storeCollectionPointsPopUpVM.styles[i]=this.popUpCollectionVMStyle[i];
		}
		collectionPointsPopUpView.storeCollectionPointsPopUpVM.initStyles();
		this.collectionPointsPopUpResize ();
		yield return new WaitForSeconds (5);
		this.hideCollectionPointsPopUp ();
	}
	public void collectionPointsPopUpResize()
	{
		collectionPointsPopUpView.storeCollectionPointsPopUpVM.centralWindow = view.storeScreenVM.collectionPointsWindow;
		collectionPointsPopUpView.storeCollectionPointsPopUpVM.resize ();
	}
	public void hideCollectionPointsPopUp()
	{
		Destroy (this.collectionPointsPopUpView);
		this.enableMainGui ();
	}
	public IEnumerator displayNewSkillsPopUp()
	{
		if(this.newSkillsPopUpView!=null)
		{
			this.hideNewSkillsPointsPopUp();
		}
		this.newSkillsPopUpView = Camera.main.gameObject.AddComponent <StoreNewSkillsPopUpView>();
		for(int i=0;i<model.packList [this.selectedPackIndex].NewSkills.Count;i++)
		{
			newSkillsPopUpView.storeNewSkillsPopUpVM.skills.Add (model.packList [this.selectedPackIndex].NewSkills[i].Name);
		}
		if(model.packList [this.selectedPackIndex].NewSkills.Count>1)
		{
			newSkillsPopUpView.storeNewSkillsPopUpVM.title="Nouvelles compétences :";
		}
		else if(model.packList [this.selectedPackIndex].NewSkills.Count==1)
		{
			newSkillsPopUpView.storeNewSkillsPopUpVM.title="Nouvelle compétence :";
		}
		newSkillsPopUpView.storeNewSkillsPopUpVM.styles=new GUIStyle[this.popUpCollectionVMStyle.Length];
		for(int i=0;i<this.popUpCollectionVMStyle.Length;i++)
		{
			newSkillsPopUpView.storeNewSkillsPopUpVM.styles[i]=this.popUpCollectionVMStyle[i];
		}
		newSkillsPopUpView.storeNewSkillsPopUpVM.initStyles();
		this.newSkillsPopUpResize ();
		yield return new WaitForSeconds (5);
		this.hideNewSkillsPointsPopUp ();
	}
	public void newSkillsPopUpResize()
	{
		newSkillsPopUpView.storeNewSkillsPopUpVM.centralWindow = view.storeScreenVM.newSkillsWindow;
		newSkillsPopUpView.storeNewSkillsPopUpVM.resize ();
	}
	public void hideNewSkillsPointsPopUp()
	{
		Destroy (this.newSkillsPopUpView);
		this.enableMainGui ();
	}
	public void addCreditsHandler()
	{
		StartCoroutine (addCredits ());
	}
	public IEnumerator addCredits()
	{
		addCreditsPopUpView.addCreditsPopUpVM.guiEnabled = false;
		yield return StartCoroutine (this.model.player.addMoney (System.Convert.ToInt32(addCreditsPopUpView.addCreditsPopUpVM.credits)));
		this.hideAddCreditsPopUp ();
		this.refreshCredits ();
	}
	public void exitCard()
	{
		if(this.randomCards!=null && this.randomCards.Length==1)
		{
			this.displayMainGUI ();
		}
		else if(this.cardFocused!=null)
		{
			this.displayCards();
		}
	}
	public void setGUI(bool value)
	{
		if(this.randomCards!=null)
		{
			if(this.randomCards.Length==1)
			{
				this.randomCards[0].GetComponent<CardController>().setMyGUI(value);
			}
		}
		if(this.cardFocused!=null)
		{
			this.cardFocused.GetComponent<CardController>().setMyGUI(value);
		}
		if(!isTutorialLaunched)
		{
			this.setButtonsGui(value);
		}
	}
	public void popUpDisplayed(bool value, GameObject gameObject)
	{
		this.cardPopUpBelongTo = gameObject;
		view.storeVM.isPopUpDisplayed = value;
	}
	public void returnPressed()
	{
		if(view.storeVM.isPopUpDisplayed)
		{
			this.cardPopUpBelongTo.GetComponent<CardController> ().confirmPopUp ();
		}
		else if(errorPopUpView!=null)
		{
			this.hideErrorPopUp();
		}
		else if(selectCardTypePopUpView!=null)
		{
			this.buyPackWidthCardTypeHandler();
		}
		else if(addCreditsPopUpView!=null)
		{
			this.addCreditsHandler();
		}
	}
	public void escapePressed()
	{
		if(view.storeVM.isPopUpDisplayed && this.cardPopUpBelongTo!=null)
		{
			this.cardPopUpBelongTo.GetComponent<CardController> ().exitPopUp ();
		}
		else if(this.cardFocused!=null)
		{
			this.cardFocused.GetComponent<CardStoreController>().exitFocus();
		}
		else if(this.randomCards.Length==1&& !this.startRotation)
		{
			this.exitCard();
		}
		else if(this.randomCards.Length>1 && !this.startRotation)
		{
			this.displayMainGUI();
		}
		else if(errorPopUpView!=null)
		{
			this.hideErrorPopUp();
		}
		else if(selectCardTypePopUpView!=null)
		{
			this.hideSelectCardTypePopUp();
		}
		else if(addCreditsPopUpView!=null)
		{
			this.hideAddCreditsPopUp();
		}
	}
	public void refreshCredits()
	{
		StartCoroutine(this.MenuObject.GetComponent<MenuController> ().getUserData ());
	}
	public IEnumerator sellCard(GameObject gameobject)
	{
		yield return StartCoroutine (model.packList[this.selectedPackIndex].Cards[System.Convert.ToInt32 (gameobject.name.Substring (4))].sellCard ());
		if(model.packList[this.selectedPackIndex].Cards[System.Convert.ToInt32 (gameobject.name.Substring (4))].Error=="")
		{
			if(model.packList[this.selectedPackIndex].Cards.Count==1)
			{
				this.displayMainGUI();
			}
			else
			{
				this.displayCards ();
				Destroy (this.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))]);
				Destroy (this.cardPopUpBelongTo);
				view.storeVM.isPopUpDisplayed = false;
			}
		}
		else
		{
			if(model.packList[this.selectedPackIndex].Cards.Count==1)
			{
				this.randomCards[0].GetComponent<CardController>().setError();
			}
			else
			{
				this.cardFocused.GetComponent<CardController>().setError();
			}
			model.packList[this.selectedPackIndex].Cards[System.Convert.ToInt32 (gameobject.name.Substring (4))].Error="";
		}
		this.refreshCredits ();
	}
	public IEnumerator buyXpCard(GameObject gameobject)
	{
		yield return StartCoroutine (model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].addXpLevel ());
		if(model.packList[this.selectedPackIndex].Cards[System.Convert.ToInt32 (gameobject.name.Substring (4))].Error=="")
		{
			this.setGUI (true);
			if(model.packList[this.selectedPackIndex].Cards.Count==1)
			{
				this.randomCards[0].GetComponent<CardController>().animateExperience(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))]);
				if(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].CollectionPoints>0)
				{
					StartCoroutine(this.randomCards[0].GetComponent<CardController>().displayCollectionPointsPopUp());
				}
				if(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].NewSkills.Count>0)
				{
					StartCoroutine(this.randomCards[0].GetComponent<CardController>().displayNewSkillsPopUp());
				}
				if(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].IdCardTypeUnlocked!=-1)
				{
					this.randomCards[0].GetComponent<CardController>().displayNewCardTypePopUp();
					model.player.CardTypesAllowed.Add (model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].IdCardTypeUnlocked);
				}
			}
			else
			{
				this.cardFocused.GetComponent<CardController>().animateExperience(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))]);
				this.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].GetComponent<CardStoreController>().resetStoreCard(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))]);
				if(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].CollectionPoints>0)
				{
					StartCoroutine(this.cardFocused.GetComponent<CardController>().displayCollectionPointsPopUp());
				}
				if(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].NewSkills.Count>0)
				{
					StartCoroutine(this.cardFocused.GetComponent<CardController>().displayNewSkillsPopUp());
				}
				if(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].IdCardTypeUnlocked!=-1)
				{
					this.cardFocused.GetComponent<CardController>().displayNewCardTypePopUp();
					model.player.CardTypesAllowed.Add(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].IdCardTypeUnlocked);
				}
			}
		}
		else
		{
			if(model.packList[this.selectedPackIndex].Cards.Count==1)
			{
				this.randomCards[0].GetComponent<CardStoreController>().resetFocusedStoreCard(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))]);
				this.randomCards[0].GetComponent<CardStoreController>().setError();
			}
			else
			{
				this.cardFocused.GetComponent<CardStoreController>().resetFocusedStoreCard(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))]);
				this.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].GetComponent<CardStoreController>().resetStoreCard(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))]);
				this.cardFocused.GetComponent<CardController>().setError();
			}
			model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].Error="";
		}
		this.refreshCredits ();
	}
	public IEnumerator renameCard(string value,GameObject gameobject)
	{
		int renameCost = model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].RenameCost;
		yield return StartCoroutine (model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].renameCard(value,renameCost));
		this.updateCard (gameobject.name);
		this.refreshCredits ();
	}
	public IEnumerator putOnMarketCard(int price,GameObject gameobject)
	{
		yield return StartCoroutine (model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].toSell(price));
		this.updateCard (gameobject.name);
		this.refreshCredits ();
	}
	public IEnumerator editSellPriceCard(int price,GameObject gameobject)
	{
		yield return StartCoroutine (model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].changePriceCard(price));
		this.updateCard (gameobject.name);
		this.refreshCredits ();
	}
	public IEnumerator unsellCard(GameObject gameobject)
	{
		yield return StartCoroutine (model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (gameobject.name.Substring (4))].notToSell());
		this.updateCard (gameobject.name);
		this.refreshCredits ();
	}
	private void updateCard(string name)
	{
		if(model.packList[this.selectedPackIndex].Cards[System.Convert.ToInt32 (name.Substring (4))].Error=="")
		{
			this.setGUI (true);
			if(model.packList[this.selectedPackIndex].Cards.Count==1)
			{
				this.randomCards[0].GetComponent<CardStoreController>().resetFocusedStoreCard(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (name.Substring (4))]);
			}
			else
			{
				this.cardFocused.GetComponent<CardStoreController>().resetFocusedStoreCard(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (name.Substring (4))]);
				this.randomCards[System.Convert.ToInt32(name.Substring(4))].GetComponent<CardStoreController>().resetStoreCard(model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (name.Substring (4))]);
			}
		}
		else
		{
			if(model.packList[this.selectedPackIndex].Cards.Count==1)
			{
				this.randomCards[0].GetComponent<CardStoreController>().setError();
			}
			else
			{
				this.cardFocused.GetComponent<CardController>().setError();
			}
			model.packList [this.selectedPackIndex].Cards [System.Convert.ToInt32 (name.Substring (4))].Error="";
		}
	}
	public void enableMainGui()
	{
		view.storeVM.guiEnabled = true;
		this.setButtonsGui (true);
	}
	public void disableMainGui()
	{
		view.storeVM.guiEnabled = false;
		this.setButtonsGui (false);
	}
	public void displayMainGUI()
	{
		if(this.randomCards!=null)
		{
			for(int i=0;i<this.randomCards.Length;i++)
			{
				if(this.randomCards[i]!=null)
				{
					Destroy(this.randomCards[i]);
				}
			}
		}
		this.randomCards=null;
		view.storeVM.hideGUI = false;
		view.storeVM.areMoreThan1CardDisplayed = false;
		this.enableMainGui ();
	}
	public void hideMainGUI()
	{
		view.storeVM.hideGUI = true;
	}
	public void displayCards()
	{
		view.storeVM.areMoreThan1CardDisplayed=true ;
		if(this.cardFocused!=null)
		{
			Destroy(this.cardFocused);
		}
		for(int i = 0 ; i < randomCards.Length ; i++)
		{
			if(this.randomCards[i]!=null)
			{
				this.randomCards[i].SetActive(true);
			}
		}
	}
	public void hideCards()
	{
		view.storeVM.areMoreThan1CardDisplayed=false ;
		
		for(int i = 0 ; i < randomCards.Length ; i++)
		{
			if(this.randomCards[i]!=null)
			{
				this.randomCards[i].SetActive(false);
			}
		}
	}
	private void resizeRandomCards()
	{
		if(this.cardFocused==null)
		{
			view.storeVM.areMoreThan1CardDisplayed = true;
		}
		view.storeVM.guiEnabled=true;
		this.startRotation=false;
		Vector3 scale;
		Vector3 position;
		Quaternion rotation;
		float tempF = 2*Camera.main.GetComponent<Camera>().orthographicSize*view.storeScreenVM.widthScreen/view.storeScreenVM.heightScreen;
		float width = 0.75f * tempF;
		float scaleCard = width/6f;
		for (int i = 0; i < randomCards.Length; i++)
		{
			if(this.randomCards[i]!=null)
			{
				scale = new Vector3(scaleCard,scaleCard,scaleCard);
				position = new Vector3(-width/2+(scaleCard/2)+i*(scaleCard+1f/4f*scaleCard), 0f, 0f); 
				rotation = Quaternion.Euler(0, 0, 0);
				this.randomCards [i].GetComponent<CardController>().setGameObjectScaleAndPosition(scale,position);
				this.randomCards [i].GetComponent<CardController>().setGameObjectRotation(rotation);
			}
		}
	}
	private void initializePagination()
	{
		view.packsVM.nbPages = Mathf.CeilToInt(((float)model.packList.Count) / ((float)view.packsVM.nbElementsToDisplay));
		view.packsVM.pageDebut = 0 ;
		if (view.packsVM.nbPages>10)
		{
			view.packsVM.pageFin = 10 ;
		}
		else
		{
			view.packsVM.pageFin = view.packsVM.nbPages;
		}
		view.packsVM.paginatorGuiStyle = new GUIStyle[view.packsVM.nbPages];
		for (int i = 0; i < view.packsVM.nbPages; i++) 
		{ 
			if (i==0)
			{
				view.packsVM.paginatorGuiStyle[i]=view.storeVM.paginationActivatedStyle;
			}
			else
			{
				view.packsVM.paginatorGuiStyle[i]=view.storeVM.paginationStyle;
			}
		}
	}
	public void paginationBack()
	{
		view.packsVM.pageDebut = view.packsVM.pageDebut-10;
		view.packsVM.pageFin = view.packsVM.pageDebut+10;
	}
	public void paginationSelect(int chosenPage)
	{
		view.packsVM.paginatorGuiStyle[view.packsVM.chosenPage]=view.storeVM.paginationStyle;
		view.packsVM.chosenPage=chosenPage;
		view.packsVM.paginatorGuiStyle[chosenPage]=this.view.storeVM.paginationActivatedStyle;
		this.displayPage();
	}
	public void paginationNext()
	{
		view.packsVM.pageDebut = view.packsVM.pageDebut+10;
		view.packsVM.pageFin = Mathf.Min(view.packsVM.pageFin+10, view.packsVM.nbPages);
	}
	public void setButtonsGui(bool value)
	{
		for(int i=0;i<view.storeVM.buttonsEnabled.Length;i++)
		{
			view.storeVM.buttonsEnabled[i]=value;
		}
	}
	public void setButtonGui(int index, bool value)
	{
		view.storeVM.buttonsEnabled[index]=value;
	}
	public IEnumerator endTutorial()
	{
		MenuController.instance.setButtonsGui (false);
		yield return StartCoroutine (model.player.setTutorialStep (-1));
		Application.LoadLevel ("MyGame");
	}
	public void tutorialCardLeaved()
	{
		this.tutorial.GetComponent<StoreTutorialController> ().actionIsDone ();
	}
	public Vector2 getCardsPosition()
	{
		return this.randomCards[0].GetComponent<CardController>().GOPosition;
	}
	public Vector2 getCardsSize()
	{
		return this.randomCards[0].GetComponent<CardController>().GOSize;
	}
	public void setExitButtonGui(bool value)
	{
		this.randomCards[0].GetComponent<CardStoreController> ().setExitButtonsGui(value);
	}
}
