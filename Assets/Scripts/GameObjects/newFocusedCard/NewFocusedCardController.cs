using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class NewFocusedCardController : MonoBehaviour 
{
	private NewFocusedCardRessources ressources;
	public NewPopUpRessources popUpRessources;
	public Card c;
	private GameObject[] skills;
	private GameObject experience;
	private GameObject cardUpgrade;
	private GameObject panelSold;
	
	private Rect centralWindow;
	private Rect collectionPointsWindow;
	private Rect newSkillsWindow;
	private Rect newCardTypeWindow;

	private NewFocusedCardSellPopUpView sellView;
	private bool isSellViewDisplayed;
	private NewFocusedCardErrorPopUpView errorView;
	private bool isErrorViewDisplayed;
	private NewFocusedCardRenamePopUpView renameView;
	private bool isRenameViewDisplayed;
	private NewFocusedCardBuyPopUpView buyView;
	private bool isBuyViewDisplayed;
	private NewFocusedCardBuyXpView buyXpView;
	private bool isBuyXpViewDisplayed;
	private NewFocusedCardEditSellPopUpView editSellView;
	private bool isEditSellViewDisplayed;
	private NewFocusedCardEditSellPricePopUpView editSellPriceView;
	private bool isEditSellPriceViewDisplayed;
	private NewFocusedCardPutOnMarketPopUpView putOnMarketView;
	private bool isPutOnMarketViewDisplayed;
	private NewCollectionPointsPopUpView collectionPointsView;
	private bool isCollectionPointsViewDisplayed;
	private NewSkillsPopUpView newSkillsView;
	private bool isNewSkillsViewDisplayed;
	private NewFocusedCardNewCardTypePopUpView newCardTypeView;
	private bool isNewCardTypeViewDisplayed;
	private NewFocusedCardSoldPopUpView soldCardView;
	private bool isSoldCardViewDisplayed;

	private bool isCardUpgradeDisplayed;
	private bool isSkillHighlighted;
	private bool isPanelSoldIsDisplayed;

	private float speed;
	private float timerCollectionPoints;
	private float timerCardUpgrade;
	private float timerSkillHighlighted;

	public virtual void Update ()
	{
		if(isCollectionPointsViewDisplayed)
		{
			timerCollectionPoints = timerCollectionPoints + speed * Time.deltaTime;
			if(timerCollectionPoints>15f)
			{
				timerCollectionPoints=0f;
				this.hideCollectionPointsPopUp();
				if(isNewSkillsViewDisplayed)
				{
					this.hideNewSkillsPopUp();
				}
			}
		}
		if(isCardUpgradeDisplayed)
		{
			timerCardUpgrade = timerCardUpgrade + speed * Time.deltaTime;
			if(timerCardUpgrade>15f)
			{
				this.cardUpgrade.SetActive(false);
				this.isCardUpgradeDisplayed=false;
			}
		}
		if(isSkillHighlighted)
		{
			timerSkillHighlighted = timerSkillHighlighted + speed * Time.deltaTime;
			if(timerCardUpgrade>15f)
			{
				this.skills[skills.Length-1].GetComponent<NewFocusedCardSkillController>().highlightSkill(false);
				this.isSkillHighlighted=false;
			}
		}
	}
	public virtual void Awake()
	{
		this.skills=new GameObject[0];
		this.ressources = this.gameObject.GetComponent<NewFocusedCardRessources> ();
		this.setPopUpRessources ();
		this.setUpdateSpeed ();
		this.experience = this.gameObject.transform.FindChild ("Experience").gameObject;
		this.cardUpgrade = this.gameObject.transform.FindChild ("CardUpgrade").gameObject;
		this.panelSold = this.gameObject.transform.FindChild ("PanelSold").gameObject;
		this.initializeFocusFeatures ();
	}
	public void setPopUpRessources()
	{
		this.popUpRessources = this.gameObject.GetComponent<NewPopUpRessources> ();
	}
	public void setUpdateSpeed()
	{
		this.speed = 5f;
	}
	public virtual void show()
	{
		this.applyFrontTexture ();
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().text = this.c.Title;
		this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Power.ToString();
		this.gameObject.transform.FindChild ("Power").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.PowerLevel - 1];
		this.gameObject.transform.FindChild("Life").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Life.ToString();
		this.gameObject.transform.FindChild ("Life").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.LifeLevel - 1];
		this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Move.ToString();
		this.gameObject.transform.FindChild("Attack").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Attack.ToString();
		this.gameObject.transform.FindChild ("Attack").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.AttackLevel - 1];
		this.gameObject.transform.FindChild("Quickness").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Speed.ToString();
		this.gameObject.transform.FindChild ("Quickness").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.SpeedLevel - 1];
		for(int i=0;i<this.skills.Length;i++)
		{
			Destroy (this.skills[i]);
		}
		this.skills = new GameObject[this.c.Skills.Count];
		for(int i=0;i<c.Skills.Count;i++)
		{
			this.skills[i]= Instantiate(ressources.skillObject) as GameObject;
			this.skills[i].transform.parent=this.gameObject.transform;
			this.skills[i].transform.name="Skill"+i;
			this.skills[i].transform.localPosition=new Vector3(-1.46f,-1.35f-i*0.75f,0);
			this.skills[i].transform.GetComponent<NewFocusedCardSkillController>().setSkill(c.Skills[i]);
		}
		this.experience.GetComponent<NewFocusedCardExperienceController> ().setExperience (this.c.ExperienceLevel, this.c.PercentageToNextLevel);
		this.updateFocusFeatures ();
	}
	public virtual void applyFrontTexture()
	{
		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sprite = ressources.faces[0];
	}
	public void setCardSold()
	{
		if(!isSoldCardViewDisplayed)
		{
			this.displayPanelSold();
			this.closePopUps ();
			this.displaySoldPopUp ();
		}
	}
	public virtual void displayPanelSold()
	{
		this.panelSold.SetActive (true);
		this.isPanelSoldIsDisplayed=true;
	}
	public virtual void hidePanelSold()
	{
		this.panelSold.SetActive (false);
		this.isPanelSoldIsDisplayed=false;
	}
	public void endUpdatingXp()
	{
		this.show ();
		this.updateFocus ();
		if(this.c.CollectionPoints>0)
		{
			this.displayCollectionPointsPopUp();
		}
		if(this.c.NewSkills.Count>0)
		{
			this.displayNewSkillsPopUp();
		}
		if(this.c.IdCardTypeUnlocked!=-1)
		{
			this.displayNewCardTypePopUp();
		}
		if(this.c.CaracteristicUpgraded>-1&&this.c.CaracteristicIncrease>0)
		{
			this.setCardUpgrade();
		}
		if(this.c.GetNewSkill)
		{
			if(isSkillHighlighted)
			{
				this.skills[skills.Length-2].GetComponent<NewFocusedCardSkillController>().highlightSkill(false);
			}
			else
			{
				this.isSkillHighlighted=true;
			}
			this.skills[skills.Length-1].GetComponent<NewFocusedCardSkillController>().highlightSkill(true);
			this.c.GetNewSkill=false;
			this.timerSkillHighlighted=0;
		}
	}
	public void setCentralWindow(Rect centralWindow)
	{
		this.centralWindow = centralWindow;
	}
	public void setCollectionPointsWindow(Rect collectionPointsWindowRect)
	{
		this.collectionPointsWindow = collectionPointsWindowRect;
	}
	public void setNewSkillsWindow(Rect newSkillsWindowRect)
	{
		this.newSkillsWindow = newSkillsWindowRect;
	}
	public void setNewCardTypeWindow(Rect newCardTypeWindowRect)
	{
		this.newCardTypeWindow = newCardTypeWindowRect;
	}
	public virtual void initializeFocusFeatures()
	{
	}
	public virtual void updateFocusFeatures()
	{
	}
	public virtual void refreshCredits()
	{
	}
	public void displaySellCardPopUp()
	{
		this.sellView = gameObject.AddComponent<NewFocusedCardSellPopUpView> ();
		this.isSellViewDisplayed = true;
		sellView.sellPopUpVM.price = this.c.destructionPrice;
		sellView.popUpVM.centralWindowStyle = new GUIStyle(popUpRessources.popUpSkin.window);
		sellView.popUpVM.centralWindowTitleStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [0]);
		sellView.popUpVM.centralWindowButtonStyle = new GUIStyle (popUpRessources.popUpSkin.button);
		sellView.popUpVM.transparentStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [2]);
		this.sellPopUpResize ();
	}
	public void displayErrorPopUp()
	{
		this.errorView = gameObject.AddComponent<NewFocusedCardErrorPopUpView> ();
		this.isErrorViewDisplayed = true;
		errorView.errorPopUpVM.error = this.c.Error;
		this.c.Error = "";
		errorView.popUpVM.centralWindowStyle = new GUIStyle(popUpRessources.popUpSkin.customStyles[3]);
		errorView.popUpVM.centralWindowTitleStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [0]);
		errorView.popUpVM.centralWindowButtonStyle = new GUIStyle (popUpRessources.popUpSkin.button);
		errorView.popUpVM.transparentStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [2]);
		this.errorPopUpResize ();
	}
	public void displayRenameCardPopUp()
	{
		this.renameView = gameObject.AddComponent<NewFocusedCardRenamePopUpView> ();
		this.isRenameViewDisplayed = true;
		renameView.renamePopUpVM.price = this.c.RenameCost;
		renameView.renamePopUpVM.newTitle = "";
		renameView.popUpVM.centralWindowStyle = new GUIStyle(popUpRessources.popUpSkin.window);
		renameView.popUpVM.centralWindowTitleStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [0]);
		renameView.popUpVM.centralWindowButtonStyle = new GUIStyle (popUpRessources.popUpSkin.button);
		renameView.popUpVM.centralWindowTextfieldStyle = new GUIStyle (popUpRessources.popUpSkin.textField);
		renameView.popUpVM.centralWindowErrorStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [1]);
		renameView.popUpVM.transparentStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [2]);
		this.renamePopUpResize ();
	}
	public void displayBuyXpCardPopUp()
	{
		this.buyXpView = gameObject.AddComponent<NewFocusedCardBuyXpView> ();
		this.isBuyXpViewDisplayed = true;
		buyXpView.buyXpPopUpVM.price = this.c.NextLevelPrice;
		buyXpView.popUpVM.centralWindowStyle = new GUIStyle(popUpRessources.popUpSkin.window);
		buyXpView.popUpVM.centralWindowTitleStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [0]);
		buyXpView.popUpVM.centralWindowButtonStyle = new GUIStyle (popUpRessources.popUpSkin.button);
		buyXpView.popUpVM.transparentStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [2]);
		this.buyXpPopUpResize ();
	}
	public void displayBuyCardPopUp()
	{
		this.buyView = gameObject.AddComponent<NewFocusedCardBuyPopUpView> ();
		this.isBuyViewDisplayed = true;
		buyView.buyPopUpVM.price = this.c.Price;
		buyView.popUpVM.centralWindowStyle = new GUIStyle(popUpRessources.popUpSkin.window);
		buyView.popUpVM.centralWindowTitleStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [0]);
		buyView.popUpVM.centralWindowButtonStyle = new GUIStyle (popUpRessources.popUpSkin.button);
		buyView.popUpVM.transparentStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [2]);
		this.buyPopUpResize ();
	}
	public void displayEditSellCardPopUp()
	{
		this.editSellView = gameObject.AddComponent<NewFocusedCardEditSellPopUpView> ();
		this.isEditSellViewDisplayed = true;
		editSellView.editSellPopUpVM.price = this.c.Price;
		editSellView.popUpVM.centralWindowStyle = new GUIStyle(popUpRessources.popUpSkin.window);
		editSellView.popUpVM.centralWindowTitleStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [0]);
		editSellView.popUpVM.centralWindowButtonStyle = new GUIStyle (popUpRessources.popUpSkin.button);
		editSellView.popUpVM.transparentStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [2]);
		this.editSellPopUpResize ();
	}
	public void displayEditSellPriceCardPopUp()
	{
		if(isEditSellViewDisplayed)
		{
			this.hideEditSellPopUp();
		}
		this.editSellPriceView = gameObject.AddComponent<NewFocusedCardEditSellPricePopUpView> ();
		this.isEditSellPriceViewDisplayed = true;
		editSellPriceView.editSellPricePopUpVM.price = this.c.Price.ToString();
		editSellPriceView.popUpVM.centralWindowStyle = new GUIStyle(popUpRessources.popUpSkin.window);
		editSellPriceView.popUpVM.centralWindowTitleStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [0]);
		editSellPriceView.popUpVM.centralWindowButtonStyle = new GUIStyle (popUpRessources.popUpSkin.button);
		editSellPriceView.popUpVM.centralWindowTextfieldStyle = new GUIStyle (popUpRessources.popUpSkin.textField);
		editSellPriceView.popUpVM.centralWindowErrorStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [1]);
		editSellPriceView.popUpVM.transparentStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [2]);
		this.editSellPricePopUpResize ();
	}
	public void displayputOnMarketCardPopUp()
	{
		if(isEditSellViewDisplayed)
		{
			this.hideEditSellPopUp();
		}
		this.putOnMarketView = gameObject.AddComponent<NewFocusedCardPutOnMarketPopUpView> ();
		this.isPutOnMarketViewDisplayed = true;
		putOnMarketView.putOnMarketPopUpVM.price = "";
		putOnMarketView.popUpVM.centralWindowStyle = new GUIStyle(popUpRessources.popUpSkin.window);
		putOnMarketView.popUpVM.centralWindowTitleStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [0]);
		putOnMarketView.popUpVM.centralWindowButtonStyle = new GUIStyle (popUpRessources.popUpSkin.button);
		putOnMarketView.popUpVM.centralWindowTextfieldStyle = new GUIStyle (popUpRessources.popUpSkin.textField);
		putOnMarketView.popUpVM.centralWindowErrorStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [1]);
		putOnMarketView.popUpVM.transparentStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [2]);
		this.putOnMarketPopUpResize ();
	}
	public void displayCollectionPointsPopUp()
	{
		if(this.isCollectionPointsViewDisplayed)
		{
			this.hideCollectionPointsPopUp();
		}
		collectionPointsView = gameObject.AddComponent<NewCollectionPointsPopUpView>();
		this.isCollectionPointsViewDisplayed = true;
		this.timerCollectionPoints = 0f;
		collectionPointsView.popUpVM.centralWindow = this.collectionPointsWindow;
		collectionPointsView.cardCollectionPointsPopUpVM.collectionPoints = this.c.CollectionPoints;
		collectionPointsView.cardCollectionPointsPopUpVM.collectionPointsRanking = this.c.CollectionPointsRanking;
		collectionPointsView.popUpVM.centralWindowStyle = new GUIStyle(popUpRessources.popUpSkin.window);
		collectionPointsView.popUpVM.centralWindowTitleStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [0]);
		this.collectionPointsPopUpResize ();
	}
	public void displayNewSkillsPopUp()
	{
		if(this.isNewSkillsViewDisplayed)
		{
			this.hideNewSkillsPopUp();
		}
		this.newSkillsView = gameObject.AddComponent<NewSkillsPopUpView>();
		this.isNewSkillsViewDisplayed = true;
		newSkillsView.popUpVM.centralWindow = this.newSkillsWindow;
		for(int i=0;i<this.c.NewSkills.Count;i++)
		{
			newSkillsView.cardNewSkillsPopUpVM.skills.Add (this.c.NewSkills[i].Name);
		}
		if(this.c.NewSkills.Count>1)
		{
			newSkillsView.cardNewSkillsPopUpVM.title="Nouvelles compétences :";
		}
		else if(this.c.NewSkills.Count==1)
		{
			newSkillsView.cardNewSkillsPopUpVM.title="Nouvelle compétence :";
		}
		newSkillsView.popUpVM.centralWindowStyle = new GUIStyle(popUpRessources.popUpSkin.window);
		newSkillsView.popUpVM.centralWindowTitleStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [0]);
		this.newSkillsPopUpResize ();
	}
	public void displayNewCardTypePopUp()
	{
		newCardTypeView = gameObject.AddComponent<NewFocusedCardNewCardTypePopUpView>();
		this.isNewCardTypeViewDisplayed = true;
		newCardTypeView.popUpVM.centralWindow = this.newCardTypeWindow;
		newCardTypeView.cardNewCardTypePopUpVM.newCardType = this.c.TitleCardTypeUnlocked;
		newCardTypeView.popUpVM.centralWindowStyle = new GUIStyle(popUpRessources.popUpSkin.window);
		newCardTypeView.popUpVM.centralWindowTitleStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [0]);
		newCardTypeView.popUpVM.centralWindowButtonStyle = new GUIStyle (popUpRessources.popUpSkin.button);
		this.newCardTypePopUpResize ();
	}
	public void displaySoldPopUp()
	{
		this.soldCardView = gameObject.AddComponent<NewFocusedCardSoldPopUpView> ();
		this.isSoldCardViewDisplayed = true;
		soldCardView.soldPopUpVM.error = "Votre carte vient d'être vendue";
		soldCardView.popUpVM.centralWindowStyle = new GUIStyle(popUpRessources.popUpSkin.customStyles[3]);
		soldCardView.popUpVM.centralWindowTitleStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [0]);
		soldCardView.popUpVM.centralWindowButtonStyle = new GUIStyle (popUpRessources.popUpSkin.button);
		soldCardView.popUpVM.transparentStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [2]);
		this.soldCardPopUpResize ();
	}
	private void sellPopUpResize()
	{
		sellView.popUpVM.centralWindow = this.centralWindow;
		sellView.popUpVM.resize ();
	}
	private void errorPopUpResize()
	{
		errorView.popUpVM.centralWindow = this.centralWindow;
		errorView.popUpVM.resize ();
	}
	private void renamePopUpResize()
	{
		renameView.popUpVM.centralWindow = this.centralWindow;
		renameView.popUpVM.resize ();
	}
	private void buyXpPopUpResize()
	{
		buyXpView.popUpVM.centralWindow = this.centralWindow;
		buyXpView.popUpVM.resize ();
	}
	private void buyPopUpResize()
	{
		buyView.popUpVM.centralWindow = this.centralWindow;
		buyView.popUpVM.resize ();
	}
	private void editSellPopUpResize()
	{
		editSellView.popUpVM.centralWindow = this.centralWindow;
		editSellView.popUpVM.resize ();
	}
	private void editSellPricePopUpResize()
	{
		editSellPriceView.popUpVM.centralWindow = this.centralWindow;
		editSellPriceView.popUpVM.resize ();
	}
	private void putOnMarketPopUpResize()
	{
		putOnMarketView.popUpVM.centralWindow = this.centralWindow;
		putOnMarketView.popUpVM.resize ();
	}
	private void collectionPointsPopUpResize()
	{
		collectionPointsView.popUpVM.centralWindow = this.collectionPointsWindow;
		collectionPointsView.popUpVM.resize ();
	}
	private void newCardTypePopUpResize()
	{
		newCardTypeView.popUpVM.centralWindow = this.newCardTypeWindow;
		newCardTypeView.popUpVM.resize ();
	}
	private void newSkillsPopUpResize()
	{
		newSkillsView.popUpVM.centralWindow = this.newSkillsWindow;
		newSkillsView.popUpVM.resize ();
	}
	private void soldCardPopUpResize()
	{
		soldCardView.popUpVM.centralWindow = this.centralWindow;
		soldCardView.popUpVM.resize ();
	}
	public void hideSellPopUp()
	{
		Destroy (this.sellView);
		this.isSellViewDisplayed = false;
	}
	public void hideErrorPopUp()
	{
		Destroy (this.errorView);
		this.isErrorViewDisplayed = false;
	}
	public void hideRenamePopUp()
	{
		Destroy (this.renameView);
		this.isRenameViewDisplayed = false;
	}
	public void hideBuyXpPopUp()
	{
		Destroy (this.buyXpView);
		this.isBuyXpViewDisplayed = false;
	}
	public void hideBuyPopUp()
	{
		Destroy (this.buyView);
		this.isBuyViewDisplayed = false;
	}
	public void hideEditSellPopUp()
	{
		Destroy (this.editSellView);
		this.isEditSellViewDisplayed = false;
	}
	public void hideEditSellPricePopUp()
	{
		Destroy (this.editSellPriceView);
		this.isEditSellPriceViewDisplayed = false;
	}
	public void hidePutOnMarketPopUp()
	{
		Destroy (this.putOnMarketView);
		this.isPutOnMarketViewDisplayed = false;
	}
	public void hideCollectionPointsPopUp()
	{
		Destroy (this.collectionPointsView);
		this.isCollectionPointsViewDisplayed = false;
	}
	public void hideNewCardTypePopUp()
	{
		Destroy (this.newCardTypeView);
		this.isNewCardTypeViewDisplayed = false;
	}
	public void hideNewSkillsPopUp()
	{
		Destroy (this.newSkillsView);
		this.isNewSkillsViewDisplayed = false;
	}
	public void hideSoldCardPopUp()
	{
		Destroy (this.soldCardView);
		this.isSoldCardViewDisplayed = false;
	}
	public void sellCardHandler()
	{
		this.StartCoroutine (sellCard ());
	}
	public IEnumerator sellCard()
	{
		sellView.popUpVM.guiEnabled = false;
		yield return StartCoroutine (this.c.sellCard());
		this.hideSellPopUp ();
		this.refreshCredits();
		if(this.c.Error=="")
		{
			this.deleteCard();
		}
		else
		{
			this.displayErrorPopUp();
		}
	}
	public virtual void deleteCard()
	{
	}
	public void renameCardHandler()
	{
		string tempString = this.renameCardSyntaxCheck ();
		if(tempString!="")
		{
			StartCoroutine(this.renameCard(tempString));
		}
	}
	public IEnumerator renameCard(string newName)
	{
		renameView.popUpVM.guiEnabled = false;
		yield return StartCoroutine(this.c.renameCard(newName,this.c.RenameCost));
		this.hideRenamePopUp ();
		this.updateFocus ();
	}
	public void buyXpCardHandler()
	{
		StartCoroutine (this.buyXpCard ());
	}
	public IEnumerator buyXpCard()
	{
		buyXpView.popUpVM.guiEnabled = false;
		yield return StartCoroutine(this.c.addXpLevel());
		this.hideBuyXpPopUp();
		this.refreshCredits();
		if(this.c.Error=="")
		{
			this.experience.GetComponent<NewFocusedCardExperienceController>().startUpdatingXp(c.ExperienceLevel,c.PercentageToNextLevel);
		}
		else
		{
			this.displayErrorPopUp();
		}
	}
	public void buyCardHandler()
	{
		StartCoroutine (this.buyCard ());
	}
	public IEnumerator buyCard()
	{
		buyView.popUpVM.guiEnabled = false;
		int oldPrice = this.c.Price;
		yield return StartCoroutine(this.c.buyCard());
		this.hideBuyPopUp ();
		this.refreshCredits ();
		if(this.c.Error=="")
		{
			this.displayPanelSold();
			this.updateFocusFeatures ();
			if(this.c.CollectionPoints>0)
			{
				this.displayCollectionPointsPopUp();
			}
			if(this.c.NewSkills.Count>0)
			{
				this.displayNewSkillsPopUp();
			}
			if(this.c.IdCardTypeUnlocked!=-1)
			{
				this.displayNewCardTypePopUp();
			}
		}
		else
		{
			if(this.c.onSale==0)
			{
				this.c.Error="";
				this.setCardSold();
			}
			else if(this.c.Price!=oldPrice)
			{
				this.actualizePrice();
				this.displayErrorPopUp();
			}
			else
			{
				this.displayErrorPopUp();
			}
		}
	}
	public virtual void actualizePrice()
	{
	}
	public void editSellPriceCardHandler()
	{
		int tempInt = editSellPriceSyntaxCheck ();
		if(tempInt!=-1)
		{
			StartCoroutine(this.editSellPrice(tempInt));
		}
	}
	public IEnumerator editSellPrice(int newPrice)
	{
		editSellPriceView.popUpVM.guiEnabled = false;
		yield return StartCoroutine (this.c.changePriceCard (newPrice));
		this.hideEditSellPricePopUp ();
		this.updateFocus ();
	}
	public void unsellCardHandler()
	{
		StartCoroutine(this.unsellCard());
	}
	public IEnumerator unsellCard()
	{
		editSellView.popUpVM.guiEnabled = false;
		yield return StartCoroutine (this.c.notToSell ());
		this.hideEditSellPopUp ();
		this.updateFocus ();
	}
	public void putOnMarketCardHandler()
	{
		int tempInt = putOnMarketSyntaxCheck ();
		if(tempInt!=-1)
		{
			StartCoroutine(this.putOnMarketCard(tempInt));
		}
	}
	public IEnumerator putOnMarketCard(int price)
	{
		putOnMarketView.popUpVM.guiEnabled = false;
		yield return StartCoroutine (this.c.toSell (price));
		this.hidePutOnMarketPopUp ();
		this.updateFocus ();
	}
	public string renameCardSyntaxCheck()
	{
		string tempString = renameView.renamePopUpVM.newTitle;
		if(tempString=="")
		{
			renameView.renamePopUpVM.error="Merci de bien vouloir saisir un nom";
			return "";
		}
		else if(tempString==this.c.Title)
		{
			renameView.renamePopUpVM.error="Le nom saisi est identique à l'ancien";
			return "";
		}
		else if(tempString.Length<4 )
		{
			renameView.renamePopUpVM.error= "Le nom doit au moins comporter 4 caractères";
			return "";
		}
		else if(tempString.Length>14 )
		{
			renameView.renamePopUpVM.error="Le nom doit faire moins de 12 caractères";
			return "";
		}
		else if(!Regex.IsMatch(tempString, @"^[a-zA-Z0-9_]+$"))
		{
			renameView.renamePopUpVM.error="Vous ne pouvez pas utiliser de caractères spéciaux";
			return "";
		}
		return tempString;
	}
	public int editSellPriceSyntaxCheck()
	{
		int n;
		bool isNumeric = int.TryParse(editSellPriceView.editSellPricePopUpVM.price, out n);
		if(editSellPriceView.editSellPricePopUpVM.price!="" && isNumeric)
		{
			if(System.Convert.ToInt32(editSellPriceView.editSellPricePopUpVM.price)>0)
			{
				return System.Convert.ToInt32(editSellPriceView.editSellPricePopUpVM.price);
			}
		}
		editSellPriceView.editSellPricePopUpVM.error="Merci de bien vouloir saisir un prix";
		return -1;
	}
	public int putOnMarketSyntaxCheck()
	{
		int n;
		bool isNumeric = int.TryParse(putOnMarketView.putOnMarketPopUpVM.price, out n);
		if(putOnMarketView.putOnMarketPopUpVM.price!="" && isNumeric)
		{
			if(System.Convert.ToInt32(putOnMarketView.putOnMarketPopUpVM.price)>0)
			{ 
				return System.Convert.ToInt32(putOnMarketView.putOnMarketPopUpVM.price);
			}
		}
		putOnMarketView.putOnMarketPopUpVM.error="Merci de bien vouloir saisir un prix";
		return -1;
	}
	public virtual void exitFocus()
	{
		if(this.isCollectionPointsViewDisplayed)
		{
			this.hideCollectionPointsPopUp();
		}
		if(this.isNewSkillsViewDisplayed)
		{
			this.hideNewSkillsPopUp();
		}
		if(this.isCardUpgradeDisplayed)
		{
			this.isCardUpgradeDisplayed=false;
			this.cardUpgrade.SetActive(false);
		}
		if(this.isSkillHighlighted)
		{
			this.isSkillHighlighted=false;
			this.skills[skills.Length-1].GetComponent<NewFocusedCardSkillController>().highlightSkill(false);
		}
		if(this.isSoldCardViewDisplayed)
		{
			this.hideSoldCardPopUp();
		}
		if(this.isPanelSoldIsDisplayed)
		{
			this.hidePanelSold();
		}
	}
	public virtual void focusFeaturesHandler (int type)
	{
	}
	public void updateFocus()
	{
		this.show ();
		this.updateFocusFeatures ();
		this.refreshCredits();
		if(this.c.Error!="")
		{
			this.displayErrorPopUp();
		}
	}
	public void resize()
	{
		if(isEditSellViewDisplayed)
		{
			this.editSellPopUpResize();
		}
		else if(isSellViewDisplayed)
		{
			this.editSellPopUpResize();
		}
		else if(isErrorViewDisplayed)
		{
			this.errorPopUpResize();
		}
		else if(isRenameViewDisplayed)
		{
			this.renamePopUpResize();
		}
		else if(isBuyXpViewDisplayed)
		{
			this.buyXpPopUpResize();
		}
		else if(isBuyViewDisplayed)
		{
			this.buyXpPopUpResize();
		}
		else if(isEditSellViewDisplayed)
		{
			this.editSellPopUpResize();
		}
		else if(isEditSellPriceViewDisplayed)
		{
			this.editSellPricePopUpResize();
		}
		else if(isPutOnMarketViewDisplayed)
		{
			this.putOnMarketPopUpResize();
		}
		else if(isNewCardTypeViewDisplayed)
		{
			this.newCardTypePopUpResize();
		}
		else if(isSoldCardViewDisplayed)
		{
			this.soldCardPopUpResize();
		}
		if(isCollectionPointsViewDisplayed)
		{
			this.collectionPointsPopUpResize();
		}
		if(isNewSkillsViewDisplayed)
		{
			this.newSkillsPopUpResize();
		}
	}
	public void returnPressed()
	{
		if(isEditSellViewDisplayed)
		{
			this.editSellPriceCardHandler();
		}
		else if(isSellViewDisplayed)
		{
			this.sellCardHandler();
		}
		else if(isErrorViewDisplayed)
		{
			this.hideErrorPopUp();
		}
		else if(isRenameViewDisplayed)
		{
			this.renameCardHandler();
		}
		else if(isBuyXpViewDisplayed)
		{
			this.buyXpCardHandler();
		}
		else if(isBuyViewDisplayed)
		{
			this.buyCardHandler();
		}
		else if(isEditSellViewDisplayed)
		{
			this.hideEditSellPopUp();
		}
		else if(isEditSellPriceViewDisplayed)
		{
			this.editSellPriceCardHandler();
		}
		else if(isPutOnMarketViewDisplayed)
		{
			this.putOnMarketCardHandler();
		}
		else if(isNewCardTypeViewDisplayed)
		{
			this.hideNewCardTypePopUp();
		}
		else if(isSoldCardViewDisplayed)
		{
			this.exitFocus();
		}
	}
	public bool closePopUps()
	{
		if(isEditSellViewDisplayed)
		{
			this.hideEditSellPopUp();
		}
		else if(isSellViewDisplayed)
		{
			this.hideSellPopUp();
		}
		else if(isErrorViewDisplayed)
		{
			this.hideErrorPopUp();
		}
		else if(isRenameViewDisplayed)
		{
			this.hideRenamePopUp();
		}
		else if(isBuyXpViewDisplayed)
		{
			this.hideBuyXpPopUp();
		}
		else if(isBuyViewDisplayed)
		{
			this.hideBuyPopUp();
		}
		else if(isEditSellViewDisplayed)
		{
			this.hideEditSellPricePopUp();
		}
		else if(isEditSellPriceViewDisplayed)
		{
			this.hideEditSellPricePopUp();
		}
		else if(isPutOnMarketViewDisplayed)
		{
			this.hidePutOnMarketPopUp();
		}
		else if(isNewCardTypeViewDisplayed)
		{
			this.hideNewCardTypePopUp();
		}
		else
		{
			return false;
		}
		return true;
	}
	public void escapePressed()
	{
		if(closePopUps())
		{
		}
		else
		{
			this.exitFocus();
		}
	}
	public void setCardUpgrade()
	{
		this.cardUpgrade.SetActive(true);
		this.cardUpgrade.GetComponent<NewCardUpgradeController> ().setCardUpgrade (this.c.CaracteristicIncrease);
		this.cardUpgrade.transform.position = this.getCardUpgradePosition (this.c.CaracteristicUpgraded);
		this.timerCardUpgrade = 0;
		this.isCardUpgradeDisplayed = true;
	}
	public Vector3 getCardUpgradePosition (int caracteristicUpgraded)
	{
		GameObject refObject = new GameObject ();
		float gap = 1f;
		switch(caracteristicUpgraded)
		{
		case 0:
			refObject = transform.FindChild("Life").FindChild("Text").gameObject;
			break;
		case 1:
			refObject=transform.FindChild("Attack").FindChild("Text").gameObject.gameObject;
			break;
		case 2:
			refObject=transform.FindChild("Move").FindChild("Text").gameObject.gameObject;
			break;
		case 3:
			refObject=transform.FindChild("Quickness").FindChild("Text").gameObject.gameObject;
			break;
		case 4:
			refObject=transform.FindChild("Skill0").FindChild ("Power").gameObject;
			break;
		case 5:
			refObject=transform.FindChild("Skill1").FindChild ("Power").gameObject;
			break;
		case 6:
			refObject=transform.FindChild("Skill2").FindChild ("Power").gameObject;
			break;
		case 7:
			refObject=transform.FindChild("Skill3").FindChild ("Power").gameObject;
			break;
		}
		Vector3 refPosition =refObject.transform.position;
		float refSizeX = refObject.transform.GetComponent<MeshRenderer> ().bounds.max.x-refObject.transform.GetComponent<MeshRenderer> ().bounds.min.x;
		return new Vector3 (refPosition.x+gap,refPosition.y,0f);
	}
	public void setBackFace(bool value)
	{
		if(value)
		{
			this.applyBackTexture();
			this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sortingOrder = 10;
		}
		else
		{
			this.applyFrontTexture();
			this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sortingOrder = 0;
		}

	}
	public virtual void applyBackTexture()
	{
		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sprite = ressources.backFace;
	}
}

