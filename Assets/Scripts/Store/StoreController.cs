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
	private Animation anim;
	private bool[] toRotate;
	private bool startRotation;
	private float speed;
	private float angle;
	private Quaternion target;
	
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
		}
		if(this.selectCardTypePopUpView!=null)
		{
			this.selectCardTypePopUpResize();
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
		this.create5RandomCard();
	}
	private IEnumerator getRandomCard()
	{
		yield return StartCoroutine(this.card.buyRandomCard (this.cardCreationCost));
		this.createRandomCard();
	}
	private IEnumerator get5RandomCards()
	{
		yield return StartCoroutine(model.create5RandomCards(this.fiveCardsCreationCost));
		this.create5RandomCard();
	}
	private void create5RandomCard()
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
			string name = "RandomCard";
			Vector3 scale = new Vector3(1f, 1f, 1f);
			Vector3 position = new Vector3(0f, 0f, 0f);
			this.randomCard = Instantiate(CardObject) as GameObject;
			this.randomCard.AddComponent<CardStoreController>();
			this.randomCard.GetComponent<CardController>().setGameObject(name,scale,position);
			this.randomCard.GetComponent<CardStoreController>().setStoreCard(this.card);
			this.randomCard.GetComponent<CardController>().setCentralWindowRect(view.storeScreenVM.centralWindow);
			StartCoroutine(animation());
		}
		else
		{
			this.displayErrorPopUp();
			errorPopUpView.errorPopUpVM.error=this.card.Error;
			this.card.Error="";
		}
	}
	public IEnumerator animation()
	{
		this.anim = this.randomCard.transform.FindChild("texturedGameCard").GetComponent<Animation>();
		this.anim.Play("flipCard");
		yield return new WaitForSeconds(this.anim["flipCard"].length);
		randomCard.GetComponent<CardController> ().resize ();
		randomCard.GetComponent<CardStoreController>().setFocusStoreFeatures();
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
		selectCardTypePopUpView.popUpVM.centralWindow = view.storeScreenVM.centralWindow;
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
		this.displayMainGUI ();
	}
	public void setGUI(bool value)
	{
		if(this.randomCard!=null)
		{
			this.randomCard.GetComponent<CardController>().setMyGUI(value);
		}
	}
	public void popUpDisplayed(bool value)
	{
		view.storeVM.isPopUpDisplayed = value;
	}
	public void returnPressed()
	{
		if(view.storeVM.isPopUpDisplayed)
		{
			this.randomCard.GetComponent<CardController> ().confirmPopUp ();
		}
		if(errorPopUpView!=null)
		{
			this.hideErrorPopUp();
		}
	}
	public void escapePressed()
	{
		if(view.storeVM.isPopUpDisplayed)
		{
			this.randomCard.GetComponent<CardController> ().exitPopUp ();
		}
		else if(view.storeVM.hideGUI && view.storeVM.guiEnabled)
			// Avoid any reload.
		{
			this.exitCard();
		}
		if(errorPopUpView!=null)
		{
			this.hideErrorPopUp();
		}
	}
	public void refreshCredits()
	{
		StartCoroutine(this.MenuObject.GetComponent<MenuController> ().getUserData ());
	}
	public IEnumerator sellCard()
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
	public IEnumerator buyXpCard()
	{
		yield return StartCoroutine(this.card.addXp(this.card.getPriceForNextLevel(),this.card.getPriceForNextLevel()));
		if(this.card.Error=="")
		{
			this.setGUI (true);
			randomCard.GetComponent<CardController>().animateExperience (this.card);
		}
		else
		{
			randomCard.GetComponent<CardStoreController>().resetStoreCard(this.card);
			randomCard.GetComponent<CardController>().setError();
			this.card.Error="";
		}
	}
	public IEnumerator renameCard(string value)
	{
		yield return StartCoroutine(this.card.renameCard(value,this.card.RenameCost));
		this.updateRandomCard ();
	}
	public IEnumerator putOnMarketCard(int price)
	{
		yield return StartCoroutine (this.card.toSell (price));
		this.updateRandomCard ();
	}
	public IEnumerator editSellPriceCard(int price)
	{
		yield return StartCoroutine (this.card.changePriceCard (price));
		this.updateRandomCard ();
	}
	public IEnumerator unsellCard()
	{
		yield return StartCoroutine (this.card.notToSell ());
		this.updateRandomCard ();
	}
	private void updateRandomCard()
	{
		randomCard.GetComponent<CardStoreController>().resetStoreCard(this.card);
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
		for(int i=0;i<5;i++)
		{
			if(this.randomCards[i]!=null)
			{
				Destroy(this.randomCards[i]);
			}
		}
		if(this.randomCard!=null)
		{
			Destroy(this.randomCard);
			Destroy(this.anim);
		}
		view.storeVM.hideGUI = false;
		view.storeVM.are5CardsDisplayed = false;
	}
	public void hideMainGUI()
	{
		view.storeVM.hideGUI = true;
	}
}
