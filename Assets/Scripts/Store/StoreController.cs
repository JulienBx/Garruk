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
	public GUIStyle[] storeVMStyle;
	public GUIStyle[] popUpVMStyle;
	public int cardCreationCost;
	public int fiveCardsCreationCost;
	public int cardWithCardTypeCreationCost;
	public int fiveCardsWidthCardTypeCreationCost;

	private StoreView view;
	private StoreModel model;
	private StoreErrorPopUpView errorPopUpView;
	private StoreAddCreditsPopUpView addCreditsPopUpView;
	private StoreSelectCardTypePopUpView selectCardTypePopUpView;
	private Card card;
	private GameObject randomCard;
	private GameObject[] randomCards;
	private GameObject cardFocused;
	private Animation anim;
	private bool[] toRotate;
	private bool startRotation;
	private bool rotateRandomCards;
	private bool rotateRandomCard;
	private float speed;
	private float angle;
	private Quaternion target;
	private GameObject cardPopUpBelongTo;
	
	public StoreController ()
	{
	}
	void Start()
	{
		instance = this;
		this.speed = 300.0f;
		this.model = new StoreModel ();
		this.card = new Card ();
		this.view = Camera.main.gameObject.AddComponent <StoreView>();
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		StartCoroutine (this.initialization ());

	}
	void Update () 
	{
		if (this.startRotation)
		{
			if(this.rotateRandomCards)
			{
				for(int i=0;i<5;i++)
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
							if(i<4)
							{
								this.toRotate[i+1]=true;
							}
							else
							{
								view.storeVM.are5CardsDisplayed = true;
								view.storeVM.guiEnabled=true;
								this.startRotation=false;
								this.rotateRandomCards=false;
							}
						}
						this.target = Quaternion.Euler(0, this.angle, 0);
						this.randomCards[i].transform.rotation = target;
					}
				}
			}
			if(this.rotateRandomCard)
			{
				this.angle = this.angle + this.speed * Time.deltaTime;
				if(this.angle>=360)
				{
					this.angle=360;
					this.startRotation=false;
					this.rotateRandomCard=false;
					this.randomCard.GetComponent<CardStoreController>().setFocusStoreFeatures();
				}
				this.target = Quaternion.Euler(0, this.angle, 0);
				this.randomCard.transform.rotation = target;
			}
		}
	}
	private IEnumerator initialization()
	{
		yield return(StartCoroutine(this.model.initializeStore()));
		this.initVM ();
		this.initStyles ();
		this.resize ();
	}
	private void initVM()
	{
		view.storeVM.canAddCredits = model.player.IsAdmin;
		view.storeVM.cardCreationCost = this.cardCreationCost;
		view.storeVM.cardWithCardTypeCreationCost = this.cardWithCardTypeCreationCost;
		view.storeVM.fiveCardsCreationCost = this.fiveCardsCreationCost;
		view.storeVM.fiveCardsWidthCardTypeCreationCost = this.fiveCardsWidthCardTypeCreationCost;
		if(model.player.CardTypesAllowed.Count==0)
		{
			view.storeVM.guiEnabled=false;
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
	}
	public void resize()
	{
		view.storeScreenVM.resize ();
		view.storeVM.resize (view.storeScreenVM.heightScreen);
		if(this.errorPopUpView!=null)
		{
			this.errorPopUpResize();
		}
		if(this.addCreditsPopUpView!=null)
		{
			this.addCreditsPopUpResize();
		}
		if(this.randomCard!=null)
		{
			this.randomCard.GetComponent<CardController>().resize();
			this.randomCard.GetComponent<CardController> ().setCentralWindowRect (view.storeScreenVM.centralWindow);
		}
		if(this.cardFocused!=null)
		{
			this.cardFocused.GetComponent<CardController>().resize();
			this.cardFocused.GetComponent<CardController> ().setCentralWindowRect (view.storeScreenVM.centralWindow);
		}
		if(this.randomCards!=null)
		{
			this.resize5RandomCards();
		}
		if(this.selectCardTypePopUpView!=null)
		{
			this.selectCardTypePopUpResize();
		}
	}
	public void rightClickedCard(GameObject gameObject)
	{
		if(gameObject.name.StartsWith("Card") && !this.startRotation)
		{
			this.hide5Cards();
			string name = "Fcrd"+gameObject.name.Substring(4);
			Vector3 scale = new Vector3(view.storeScreenVM.heightScreen / 120f,view.storeScreenVM.heightScreen / 120f,view.storeScreenVM.heightScreen / 120f);
			Vector3 position = Camera.main.ScreenToWorldPoint (new Vector3 (0.4f * view.storeScreenVM.widthScreen, 0.45f * view.storeScreenVM.heightScreen - 1, 10));
			this.cardFocused = Instantiate(CardObject) as GameObject;
			this.cardFocused.AddComponent<CardStoreController> ();
			this.cardFocused.GetComponent<CardController> ().setGameObject (name, scale, position);
			this.cardFocused.GetComponent<CardStoreController>().setStoreCard(gameObject.GetComponent<CardController> ().card);
			this.cardFocused.GetComponent<CardStoreController> ().setFocusStoreFeatures ();
			this.cardFocused.GetComponent<CardController> ().setCentralWindowRect (view.storeScreenVM.centralWindow);
		}
	}
	public void getCardsWithCardTypeHandler()
	{
		if(isCardTypeSelected())
		{
			if(selectCardTypePopUpView.selectCardTypePopUpVM.are5CardsCreated)
			{
				this.disableMainGui ();
				StartCoroutine(this.get5RandomCardsWithCardType());	
			}
			else
			{
				this.disableMainGui ();
				StartCoroutine(this.getRandomCardWithCardType());	
			}
		}
	}
	public void getRandomCardHandler()
	{
		this.disableMainGui ();
		StartCoroutine (this.getRandomCard ());
	}
	public void get5RandomCardsHandler()
	{
		this.disableMainGui ();
		StartCoroutine (this.get5RandomCards ());
	}
	private IEnumerator getRandomCardWithCardType()
	{
		this.hideSelectCardTypePopUp ();
		yield return StartCoroutine(this.card.buyRandomCard (this.cardWithCardTypeCreationCost,model.player.CardTypesAllowed[selectCardTypePopUpView.selectCardTypePopUpVM.cardTypeSelected]));
		this.createRandomCard();
	}
	private IEnumerator get5RandomCardsWithCardType()
	{
		this.hideSelectCardTypePopUp ();
		yield return StartCoroutine(model.create5RandomCards(this.fiveCardsWidthCardTypeCreationCost,model.player.CardTypesAllowed[selectCardTypePopUpView.selectCardTypePopUpVM.cardTypeSelected]));
		this.create5RandomCards();
	}
	private IEnumerator getRandomCard()
	{
		yield return StartCoroutine(this.card.buyRandomCard (this.cardCreationCost));
		this.createRandomCard();
	}
	private IEnumerator get5RandomCards()
	{
		yield return StartCoroutine(model.create5RandomCards(this.fiveCardsCreationCost));
		this.create5RandomCards();
	}
	private void create5RandomCards()
	{
		this.refreshCredits();
		if(model.error=="")
		{
			this.hideMainGUI();

			this.toRotate= new bool[] { false, false,false,false,false };

			string name;
			Vector3 scale;
			Vector3 position;
			Quaternion rotation;
			float tempF = 2*Camera.main.camera.orthographicSize*view.storeScreenVM.widthScreen/view.storeScreenVM.heightScreen;
			float width = 0.75f * tempF;
			float scaleCard = width/6f;
			this.randomCards = new GameObject[5];
			for (int i = 0; i < 5; i++)
			{
				name="Card" + i;
				scale = new Vector3(scaleCard,scaleCard,scaleCard);
				position = new Vector3(-width/2+(scaleCard/2)+i*(scaleCard+1f/4f*scaleCard), 0f, 0f); 
				rotation = Quaternion.Euler(0, 180, 0);
				this.randomCards [i] = Instantiate(this.CardObject) as GameObject;
				this.randomCards [i].AddComponent<CardStoreController>();
				this.randomCards [i].GetComponent<CardController>().setGameObject(name,scale,position);
				this.randomCards [i].GetComponent<CardStoreController>().setStoreCard(model.randomCards[i]);
				this.randomCards [i].GetComponent<CardController>().setGameObjectRotation(rotation);
			}
			this.startRotation = true;
			this.rotateRandomCards=true;
			this.toRotate [0] = true;
			this.angle = 180;
		}
		else
		{
			this.displayErrorPopUp();
			errorPopUpView.errorPopUpVM.error=model.error;
			model.error="";
		}
	}
	private void createRandomCard()
	{
		this.refreshCredits();
		if(this.card.Error=="")
		{
			this.hideMainGUI();

			string name;
			Vector3 scale = new Vector3(view.storeScreenVM.heightScreen / 120f,view.storeScreenVM.heightScreen / 120f,view.storeScreenVM.heightScreen / 120f);
			Vector3 position = Camera.main.ScreenToWorldPoint (new Vector3 (0.4f * view.storeScreenVM.widthScreen, 0.45f * view.storeScreenVM.heightScreen - 1, 10));
			Quaternion rotation;
			name="RandomCard";
			rotation = Quaternion.Euler(0, 180, 0);
			this.randomCard = Instantiate(this.CardObject) as GameObject;
			this.randomCard.AddComponent<CardStoreController>();
			this.randomCard.GetComponent<CardController>().setGameObject(name,scale,position);
			this.randomCard.GetComponent<CardStoreController>().setStoreCard(this.card);
			this.randomCard.GetComponent<CardController>().setGameObjectRotation(rotation);
			this.randomCard.GetComponent<CardController> ().setCentralWindowRect (view.storeScreenVM.centralWindow);
			this.startRotation = true;
			this.rotateRandomCard=true;
			this.angle = 180;
		}
		else
		{
			this.displayErrorPopUp();
			errorPopUpView.errorPopUpVM.error=this.card.Error;
			this.card.Error="";
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
	public void displaySelectCardPopUp(bool are5CardsCreated)
	{
		this.disableMainGui ();
		this.selectCardTypePopUpView = Camera.main.gameObject.AddComponent <StoreSelectCardTypePopUpView>();
		if(are5CardsCreated)
		{
			selectCardTypePopUpView.selectCardTypePopUpVM.are5CardsCreated=true;
		}
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
		if(this.randomCard!=null)
		{
			this.displayMainGUI ();
		}
		else if(this.cardFocused!=null)
		{
			this.display5Cards();
		}
	}
	public void setGUI(bool value)
	{
		if(this.randomCard!=null)
		{
			this.randomCard.GetComponent<CardController>().setMyGUI(value);
		}
		if(this.cardFocused!=null)
		{
			this.cardFocused.GetComponent<CardController>().setMyGUI(value);
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
			this.getCardsWithCardTypeHandler();
		}
		else if(addCreditsPopUpView!=null)
		{
			this.addCreditsHandler();
		}
	}
	public void escapePressed()
	{
		if(view.storeVM.isPopUpDisplayed)
		{
			this.cardPopUpBelongTo.GetComponent<CardController> ().exitPopUp ();
		}
		else if(this.cardFocused!=null)
		{
			this.exitCard();
		}
		else if(this.randomCard!=null)
		{
			this.exitCard();
		}
		else if(view.storeVM.are5CardsDisplayed)
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
		if(gameobject.name.StartsWith("Random"))
		{
			yield return StartCoroutine (this.card.sellCard ());
			if(this.card.Error=="")
			{
				this.displayMainGUI();
			}
			else
			{
				randomCard.GetComponent<CardController>().setError();
				this.card.Error="";
			}
		}
		else
		{
			yield return StartCoroutine (this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].sellCard ());
			if(this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].Error=="")
			{
				this.display5Cards();
				Destroy (this.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))]);
			}
			else
			{
				this.cardFocused.GetComponent<CardController>().setError();
				this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].Error="";
			}
		}
		this.refreshCredits ();
	}
	public IEnumerator buyXpCard(GameObject gameobject)
	{
		if(gameobject.name.StartsWith("Random"))
		{
			yield return StartCoroutine(this.card.addXp(this.card.getPriceForNextLevel(),this.card.getPriceForNextLevel()));
			if(this.card.Error=="")
			{
				this.setGUI (true);
				randomCard.GetComponent<CardController>().animateExperience (this.card);
			}
			else
			{
				randomCard.GetComponent<CardStoreController>().resetFocusedStoreCard(this.card);
				randomCard.GetComponent<CardController>().setError(); 
				this.card.Error="";
			}
		}
		else
		{
			yield return StartCoroutine(this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].addXp(
				this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].getPriceForNextLevel(),
				this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].getPriceForNextLevel()));
			if(this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].Error=="")
			{
				this.setGUI (true);
				this.cardFocused.GetComponent<CardController>().animateExperience (this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))]);
			}
			else
			{
				this.cardFocused.GetComponent<CardStoreController>().resetFocusedStoreCard(this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))]);
				this.randomCards[System.Convert.ToInt32(name.Substring(4))].GetComponent<CardStoreController>().resetStoreCard(this.model.randomCards[System.Convert.ToInt32(name.Substring(4))]);
				this.cardFocused.GetComponent<CardController>().setError(); 
				this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].Error="";
			}
		}
		this.refreshCredits ();
	}
	public IEnumerator renameCard(string value,GameObject gameobject)
	{
		if(gameobject.name.StartsWith("Random"))
		{
			yield return StartCoroutine(this.card.renameCard(value,this.card.RenameCost));
			this.updateRandomCard ();
		}
		else
		{
			yield return StartCoroutine (this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].renameCard(value,this.card.RenameCost));
			this.updateCardFocused (gameobject.name);
		}
		this.refreshCredits ();
	}
	public IEnumerator putOnMarketCard(int price,GameObject gameobject)
	{
		if(gameobject.name.StartsWith("Random"))
		{
			yield return StartCoroutine (this.card.toSell (price));
			this.updateRandomCard ();
		}
		else
		{
			yield return StartCoroutine (this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].toSell(price));
			this.updateCardFocused (gameobject.name);
		}
	}
	public IEnumerator editSellPriceCard(int price,GameObject gameobject)
	{
		if(gameobject.name.StartsWith("Random"))
		{
			yield return StartCoroutine (this.card.changePriceCard (price));
			this.updateRandomCard ();
		}
		else
		{
			yield return StartCoroutine (this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].changePriceCard(price));
			this.updateCardFocused (gameobject.name);
		}
	}
	public IEnumerator unsellCard(GameObject gameobject)
	{
		if(gameobject.name.StartsWith("Random"))
		{
			yield return StartCoroutine (this.card.notToSell ());
			this.updateRandomCard ();
		}
		else
		{
			yield return StartCoroutine (this.model.randomCards[System.Convert.ToInt32(gameobject.name.Substring(4))].notToSell());
			this.updateCardFocused (gameobject.name);
		}
	}
	private void updateRandomCard()
	{
		randomCard.GetComponent<CardStoreController>().resetFocusedStoreCard(this.card);
		if(this.card.Error=="")
		{
			this.setGUI (true);
		}
		else
		{
			randomCard.GetComponent<CardController>().setError();
			this.card.Error="";
		}
	}
	private void updateCardFocused(string name)
	{
		cardFocused.GetComponent<CardStoreController>().resetFocusedStoreCard(this.model.randomCards[System.Convert.ToInt32(name.Substring(4))]);
		randomCards[System.Convert.ToInt32(name.Substring(4))].GetComponent<CardStoreController>().resetStoreCard(this.model.randomCards[System.Convert.ToInt32(name.Substring(4))]);
		if(this.model.randomCards[System.Convert.ToInt32(name.Substring(4))].Error=="")
		{
			this.setGUI (true);
		}
		else
		{
			this.cardFocused.GetComponent<CardController>().setError();
			this.model.randomCards[System.Convert.ToInt32(name.Substring(4))].Error="";
		}
	}
	public void enableMainGui()
	{
		view.storeVM.guiEnabled = true;
	}
	public void disableMainGui()
	{
		view.storeVM.guiEnabled = false;
	}
	public void displayMainGUI()
	{
		if(this.randomCards!=null)
		{
			for(int i=0;i<5;i++)
			{
				if(this.randomCards[i]!=null)
				{
					Destroy(this.randomCards[i]);
				}
			}
		}
		if(this.randomCard!=null)
		{
			Destroy(this.randomCard);
			Destroy(this.anim);
		}
		view.storeVM.hideGUI = false;
		view.storeVM.are5CardsDisplayed = false;
		this.enableMainGui ();
	}
	public void hideMainGUI()
	{
		view.storeVM.hideGUI = true;
	}
	public void display5Cards()
	{
		view.storeVM.are5CardsDisplayed=true ;
		if(this.cardFocused!=null)
		{
			Destroy(this.cardFocused);
		}
		for(int i = 0 ; i < 5 ; i++)
		{
			if(this.randomCards[i]!=null)
			{
				this.randomCards[i].SetActive(true);
			}
		}
	}
	public void hide5Cards()
	{
		view.storeVM.are5CardsDisplayed=false ;
		
		for(int i = 0 ; i < 5 ; i++)
		{
			if(this.randomCards[i]!=null)
			{
				this.randomCards[i].SetActive(false);
			}
		}
	}
	private void resize5RandomCards()
	{
		if(this.cardFocused==null)
		{
			view.storeVM.are5CardsDisplayed = true;
		}
		view.storeVM.guiEnabled=true;
		this.startRotation=false;
		this.rotateRandomCards=false;
		string name;
		Vector3 scale;
		Vector3 position;
		Quaternion rotation;
		float tempF = 2*Camera.main.camera.orthographicSize*view.storeScreenVM.widthScreen/view.storeScreenVM.heightScreen;
		float width = 0.75f * tempF;
		float scaleCard = width/6f;
		for (int i = 0; i < 5; i++)
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
}
