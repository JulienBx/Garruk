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

	private StoreView view;
	private StoreErrorPopUpView errorPopUpView;
	private Card card;
	private GameObject randomCard;
	private Animation anim;

	public StoreController ()
	{
	}
	void Start()
	{
		instance = this;
		this.card = new Card ();
		this.view = Camera.main.gameObject.AddComponent <StoreView>();
		view.storeVM.creationCost = this.card.buyRandomCardCost;
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		this.initStyles ();
		this.resize ();
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
		if(this.randomCard!=null)
		{
			this.randomCard.GetComponent<CardController>().resize();
		}
	}
	public IEnumerator createRandomCard()
	{
		view.storeVM.guiEnabled = false;
		if (this.randomCard!=null) 
		{
			Destroy(this.randomCard);
			Destroy (this.anim);
		}
		yield return StartCoroutine(this.card.buyRandomCard ());
		if(this.card.Error=="")
		{
			this.randomCard = Instantiate(CardObject) as GameObject;
			this.randomCard.AddComponent<CardStoreController>();
			this.randomCard.transform.localScale = new Vector3(1f, 1f, 1f);                
			this.randomCard.transform.localPosition = new Vector3(0f, 0f, 0f);  
			this.randomCard.gameObject.name = "RandomCard";
			this.randomCard.GetComponent<CardStoreController>().setStoreCard(this.card);
			this.randomCard.GetComponent<CardController>().setCentralWindowRect(view.storeScreenVM.centralWindow);
			StartCoroutine(animation());
		}
		else
		{
			this.displayErrorPopUp();
		}
	}
	public IEnumerator animation()
	{
		this.anim = this.randomCard.transform.FindChild("texturedGameCard").GetComponent<Animation>();
		this.anim.Play("flipCard");
		yield return new WaitForSeconds(this.anim["flipCard"].length);
		randomCard.GetComponent<CardController> ().resize ();
		randomCard.GetComponent<CardStoreController>().setFocusStoreFeatures();
		view.storeVM.guiEnabled = true;
	}
	public void displayErrorPopUp()
	{
		this.errorPopUpView = Camera.main.gameObject.AddComponent <StoreErrorPopUpView>();
		errorPopUpView.errorPopUpVM.error = this.card.Error;
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
		this.setGUI (true);
		Destroy (this.errorPopUpView);
	}
	public void clickedCard()
	{
		view.storeVM.isCardZoomed=true ;
		float scale = view.storeScreenVM.heightScreen / 120f;
		this.randomCard.transform.localScale = new Vector3(scale, scale, scale); 
	}
	public void exitCard()
	{
		Destroy(this.randomCard);
		Destroy (this.anim);
	}
	public void setGUI(bool value)
	{
		view.storeVM.guiEnabled = value;
		this.randomCard.GetComponent<CardController>().setMyGUI(value);
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
		else if(view.storeVM.isCardZoomed)
		{
			this.exitCard();
		}
		if(errorPopUpView!=null)
		{
			this.hideErrorPopUp();
		}
	}
	public void hideCard()
	{
		Destroy (this.randomCard);
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
			this.setGUI (true);
			Destroy(this.randomCard);
			Destroy (this.anim);
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
}
