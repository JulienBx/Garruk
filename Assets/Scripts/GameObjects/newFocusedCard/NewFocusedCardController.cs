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

	public GameObject[] skills;
	public GameObject experience;
	public GameObject cardUpgrade;
	public GameObject panelSold;
	public GameObject skillPopUp;
	public GameObject nextLevelPopUp;
	public GameObject attack;
	public GameObject life;
	public GameObject quickness;
	public GameObject name;
	public GameObject face;
	public GameObject caracter;
	public GameObject cardbox;
	public GameObject card;

	public Card c;
	public int collectionPointsEarned;
	public int newCollectionRanking;
	public int idCardTypeUnlocked;
	public string titleCardTypeUnlocked;
	public List<Skill> skillsUnlocked;
	public bool getNewSkill;
	public int caracteristicUpgraded;
	public int caracteristicIncrease;

	private string urlAddXpLevel = ApplicationModel.host + "add_xplevel_to_card.php"; 
	private string urlUpgradeCardAttribute = ApplicationModel.host + "upgrade_card_attribute.php";
	private string urlBuyCard = ApplicationModel.host + "buyCard.php";
	private string urlChangeMarketPrice = ApplicationModel.host + "changeMarketPrice.php";
	private string urlRenameCard = ApplicationModel.host + "renameCard.php";
	private string urlRemoveFromMarket = ApplicationModel.host + "removeFromMarket.php";
	private string urlSellCard = ApplicationModel.host + "sellCard.php";
	private string urlPutOnMarket = ApplicationModel.host + "putonmarket.php";
	private string urlBuyRandomCard = ApplicationModel.host + "buyRandomCard.php";
	
	private Rect centralWindow;

	private NewFocusedCardSellPopUpView sellView;
	private bool isSellViewDisplayed;
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
	private NewFocusedCardSoldPopUpView soldCardView;
	private bool isSoldCardViewDisplayed;
	private NewFocusedCardErrorPopUpView errorView;
	private bool isErrorViewDisplayed;

	private bool isCardUpgradeDisplayed;
	private bool isSkillHighlighted;
	private bool isPanelSoldIsDisplayed;
	private bool isXpBeingUpdated;
	private bool isSkillPopUpDisplayed;

	private float speed;
	private float timerCollectionPoints;
	private float timerCardUpgrade;
	private float timerSkillHighlighted;
	
	private bool isNextLevelPopUpDisplayed;
	private bool isNextLevelPopUpDisplaying;
	private bool isNextLevelPopUpHiding;

	private float angle;
	

	public virtual void Update ()
	{
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
			if(timerSkillHighlighted>15f)
			{
				this.skills[this.c.Skills.Count-1].GetComponent<NewFocusedCardSkillController>().highlightSkill(false);
				this.isSkillHighlighted=false;
			}
		}
		if(isNextLevelPopUpDisplaying)
		{
			this.angle = this.angle + 500f * Time.deltaTime;
			if(this.angle>90)
			{
				if(!this.isNextLevelPopUpDisplayed)
				{
					this.displayNextLevelPopUp();
				}
				if(this.angle>180)
				{
					this.angle=180;
					this.isNextLevelPopUpDisplaying=false;
				}
			}
			Quaternion targetFocusedCard= Quaternion.Euler(0, this.angle, 0);
			Quaternion targetNextLevelPopUp= Quaternion.Euler(0,this.angle+180,0);
			this.card.transform.rotation = targetFocusedCard;
			if(this.isNextLevelPopUpDisplayed)
			{
				this.nextLevelPopUp.transform.rotation=targetNextLevelPopUp;
			}
		}
		if(isNextLevelPopUpHiding)
		{
			this.angle = this.angle - 500f * Time.deltaTime;
			if(this.angle<90)
			{
				if(this.isNextLevelPopUpDisplayed)
				{
					this.hideNextLevelPopUp ();
				}
				if(this.angle<0)
				{
					this.angle=0;
					this.isNextLevelPopUpHiding=false;
				}
			}
			Quaternion targetFocusedCard= Quaternion.Euler(0, this.angle, 0);
			Quaternion targetNextLevelPopUp= Quaternion.Euler(0,this.angle+180,0);
			this.card.transform.rotation = targetFocusedCard;
			if(this.isNextLevelPopUpDisplayed)
			{
				this.nextLevelPopUp.transform.rotation=targetNextLevelPopUp;
			}
			if(!this.isNextLevelPopUpHiding)
			{
				this.endUpdatingCardToNextLevel();
				MenuController.instance.hideTransparentBackground ();
			}
		}
	}
	public virtual void Awake()
	{
		this.skills=new GameObject[4];
		this.getRessources ();
		this.setPopUpRessources ();
		this.setUpdateSpeed ();
		this.cardUpgrade = this.gameObject.transform.FindChild ("CardUpgrade").gameObject;
		this.panelSold = this.gameObject.transform.FindChild ("PanelSold").gameObject;
		this.skillPopUp = this.gameObject.transform.FindChild ("SkillPopUp").gameObject;
		this.getCardsComponents ();
		this.initializeFocusFeatures ();
	}
	public virtual void getCardsComponents()
	{
		this.experience = this.gameObject.transform.FindChild("Card").FindChild ("Experience").gameObject;
		for(int i=0;i<this.skills.Length;i++)
		{
			this.skills[i]=this.gameObject.transform.FindChild("Card").FindChild("Skill"+i).gameObject;
		}
		this.name = this.gameObject.transform.FindChild ("Card").FindChild ("Name").gameObject;
		this.attack = this.gameObject.transform.FindChild ("Card").FindChild ("Attack").gameObject;
		this.life = this.gameObject.transform.FindChild ("Card").FindChild ("Life").gameObject;
		this.quickness = this.gameObject.transform.FindChild ("Card").FindChild ("Quickness").gameObject;
		this.cardbox = this.gameObject.transform.FindChild ("Card").FindChild ("cardbox").gameObject;
		this.face = this.gameObject.transform.FindChild("Card").FindChild ("Face").gameObject;
		this.caracter = this.gameObject.transform.FindChild("Card").FindChild ("Caracter").gameObject;
		this.card = this.gameObject.transform.FindChild ("Card").gameObject;
	}
	public virtual void getRessources()
	{
		this.ressources = this.gameObject.GetComponent<NewFocusedCardRessources> ();
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
		this.name.GetComponent<TextMeshPro> ().text = this.c.Title.ToUpper();
		//this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Power.ToString();
		//this.gameObject.transform.FindChild ("Power").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.PowerLevel - 1];
		this.life.transform.FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Life.ToString();
		this.life.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [this.c.LifeLevel - 1];
		//this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Move.ToString();
		this.attack.transform.FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Attack.ToString();
		this.attack.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [this.c.AttackLevel - 1];
		this.quickness.transform.FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Speed.ToString();
		this.quickness.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().color = ressources.colors [this.c.SpeedLevel - 1];

		for(int i=0;i<this.skills.Length;i++)
		{
			if(i<this.c.Skills.Count && this.c.Skills[i].IsActivated==1)
			{
				this.skills[i].transform.GetComponent<NewFocusedCardSkillController>().setSkill(this.c.Skills[i]);
				this.skills[i].transform.GetComponent<NewFocusedCardSkillController>().setDescription(this.c.getSkillText(this.c.Skills[i].Description));
				this.skills[i].SetActive(true);
			}
			else
			{
				this.skills[i].SetActive(false);
			}
		}
		this.setExperience ();
		this.updateFocusFeatures ();
	}
	public virtual void setExperience()
	{
		this.experience.GetComponent<NewFocusedCardExperienceController> ().setExperience (this.c.ExperienceLevel, this.c.PercentageToNextLevel);
	}
	public virtual void applyFrontTexture()
	{
		this.caracter.GetComponent<SpriteRenderer> ().sprite = ressources.caracters[this.c.IdClass];
		this.face.GetComponent<SpriteRenderer> ().sprite = ressources.faces [this.c.PowerLevel - 1];
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
	public virtual void hidePanelMarket()
	{
	}
	public virtual void endUpdatingXp(bool hasChangedLevel)
	{
		if(hasChangedLevel)
		{
			this.show ();
			if(this.collectionPointsEarned>0)
			{
				MenuController.instance.displayCollectionPointsPopUp(this.collectionPointsEarned,this.newCollectionRanking);
			}
			if(this.skillsUnlocked.Count>0)
			{
				MenuController.instance.displayNewSkillsPopUp(this.skillsUnlocked);
			}
			if(this.c.GetNewSkill)
			{
				this.setHighlightedSkills();
			}
			this.isNextLevelPopUpDisplaying=true;
			MenuController.instance.displayTransparentBackground ();
		}
		else
		{
			MenuController.instance.setIsUserBusy (false);
		}
		this.setIsXpBeingUpdated (false);
	}
	public void endUpdatingCardToNextLevel()
	{
		if(this.c.GetNewSkill)
		{
			this.c.GetNewSkill=false;
		}
		this.show ();
		this.updateFocus ();
		if(this.collectionPointsEarned>0)
		{
			MenuController.instance.displayCollectionPointsPopUp(this.collectionPointsEarned,this.newCollectionRanking);
		}
		if(this.idCardTypeUnlocked!=-1)
		{
			MenuController.instance.displayNewCardTypePopUp(this.titleCardTypeUnlocked);
		}
		if(this.caracteristicUpgraded>-1&&this.caracteristicIncrease>0)
		{
			this.setCardUpgrade();
		}
	}
	public void setCentralWindow(Rect centralWindow)
	{
		this.centralWindow = centralWindow;
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
	public void displayRenameCardPopUp()
	{
		this.renameView = gameObject.AddComponent<NewFocusedCardRenamePopUpView> ();
		this.isRenameViewDisplayed = true;
		renameView.renamePopUpVM.price = this.c.RenameCost;
		renameView.renamePopUpVM.newTitle = this.c.Title;
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
	public void displayErrorPopUp(string error)
	{
		this.errorView = gameObject.AddComponent<NewFocusedCardErrorPopUpView> ();
		this.isErrorViewDisplayed = true;
		errorView.errorPopUpVM.error = error;
		errorView.popUpVM.centralWindowStyle = new GUIStyle(popUpRessources.popUpSkin.customStyles[3]);
		errorView.popUpVM.centralWindowTitleStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [0]);
		errorView.popUpVM.centralWindowButtonStyle = new GUIStyle (popUpRessources.popUpSkin.button);
		errorView.popUpVM.transparentStyle = new GUIStyle (popUpRessources.popUpSkin.customStyles [2]);
		this.errorPopUpResize ();
	}
	private void sellPopUpResize()
	{
		sellView.popUpVM.centralWindow = this.centralWindow;
		sellView.popUpVM.resize ();
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
	private void soldCardPopUpResize()
	{
		soldCardView.popUpVM.centralWindow = this.centralWindow;
		soldCardView.popUpVM.resize ();
	}
	private void errorPopUpResize()
	{
		errorView.popUpVM.centralWindow = this.centralWindow;
		errorView.popUpVM.resize ();
	}
	public void hideSellPopUp()
	{
		Destroy (this.sellView);
		this.isSellViewDisplayed = false;
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
	public void hideSoldCardPopUp()
	{
		Destroy (this.soldCardView);
		this.isSoldCardViewDisplayed = false;
	}
	public void hideErrorPopUp()
	{
		Destroy (this.errorView);
		this.isErrorViewDisplayed = false;
	}
	public void sellCardHandler()
	{
		this.StartCoroutine (sellCard ());
	}
	public IEnumerator sellCard()
	{
		this.hideSellPopUp ();
		this.displayLoadingScreen ();

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", this.c.Id);		
		WWW w = new WWW(urlSellCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null)
		{
			this.displayErrorPopUp(w.error);
		} 
		else
		{
			if(w.text=="")
			{
				this.deleteCard();
			}
			else
			{
				this.displayErrorPopUp(w.text);
			}
		}
		this.hideLoadingScreen ();
	}
	public void renameCardHandler()
	{
		string tempString = this.renameCardSyntaxCheck ();
		if(tempString!="")
		{
			StartCoroutine(this.renameCard(tempString));
		}
	}
	public IEnumerator upgradeCardAttribute(int attributeToUpgrade, int newPower, int newLevel)
	{
		this.displayLoadingScreen ();

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idcard", this.c.Id.ToString());
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField ("myform_attribute", attributeToUpgrade);
		form.AddField ("myform_newpower", newPower);
		form.AddField ("myform_newlevel", newLevel);
		
		WWW w = new WWW(urlUpgradeCardAttribute, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			this.displayErrorPopUp(w.error);									// donne l'erreur eventuelle
		} 
		else
		{
			if (w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				this.displayErrorPopUp(errors[1]);
			} 
			else
			{
				string [] cardData = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
				string [] experienceData = cardData[0].Split(new string[] {"#EXPERIENCEDATA#"},System.StringSplitOptions.None);
				this.c.parseCard(experienceData[0]);
				this.titleCardTypeUnlocked=experienceData[1];
				this.idCardTypeUnlocked=System.Convert.ToInt32(experienceData[2]);
				this.caracteristicUpgraded=System.Convert.ToInt32(experienceData[3]);
				this.caracteristicIncrease=System.Convert.ToInt32(experienceData[4]);
				this.collectionPointsEarned = System.Convert.ToInt32(cardData [1]);
				this.newCollectionRanking=System.Convert.ToInt32(cardData[2]);
				this.isNextLevelPopUpHiding=true;
			}
		}
		this.hideLoadingScreen ();
	}
	public IEnumerator renameCard(string newName)
	{
		this.hideRenamePopUp ();
		this.displayLoadingScreen ();
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", this.c.Id);
		form.AddField("myform_title", newName);
		form.AddField("myform_cost", this.c.RenameCost);
		
		WWW w = new WWW(urlRenameCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			this.displayErrorPopUp(w.error);
		} 
		else
		{
			if (w.text == "")
			{
				this.c.Title = newName;
				this.name.GetComponent<TextMeshPro> ().text = this.c.Title.ToUpper();
			}
			else
			{
				this.displayErrorPopUp(w.text);
			}
		}
		this.updateFocus ();
		this.hideLoadingScreen ();
	}
	public virtual void buyXpCardHandler()
	{
		StartCoroutine (this.buyXpCard ());
	}
	public IEnumerator buyXpCard()
	{
		this.hideBuyXpPopUp();
		this.displayLoadingScreen ();

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idcard", this.c.Id.ToString());
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(urlAddXpLevel, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			this.displayErrorPopUp(w.error); 										// donne l'erreur eventuelle
		} 
		else
		{
			if (w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				this.displayErrorPopUp(errors [1]);
			} 
			else
			{
				string [] cardData = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
				string [] experienceData = cardData[0].Split(new string[] {"#EXPERIENCEDATA#"},System.StringSplitOptions.None);
				this.c.parseCard(experienceData[0]);
				this.getNewSkill=System.Convert.ToBoolean(System.Convert.ToInt32(experienceData[1]));
				this.skillsUnlocked=new List<Skill>();
				if(this.getNewSkill)
				{
					for(int i=0;i<this.c.Skills.Count;i++)
					{
						if(this.c.Skills[this.c.Skills.Count-i-1].IsActivated==1)
						{
							if(System.Convert.ToBoolean(System.Convert.ToInt32(experienceData[2])))
							{
								this.skillsUnlocked.Add (this.c.Skills[this.c.Skills.Count-i-1]);
								this.c.Skills[this.c.Skills.Count-i-1].IsNew=true;
							}
							break;
						}
					}
				}
				this.collectionPointsEarned = System.Convert.ToInt32(cardData [1]);
				this.newCollectionRanking=System.Convert.ToInt32(cardData[2]);
				this.animateExperience();
			}
		}
		this.refreshCredits();
		this.hideLoadingScreen ();
	}
	public void buyCardHandler()
	{
		StartCoroutine (this.buyCard ());
	}
	public IEnumerator buyCard()
	{
		this.hideBuyPopUp ();
		this.displayLoadingScreen ();

		int oldPrice = this.c.Price;
		this.skillsUnlocked = new List<Skill>();

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", this.c.Id);
		form.AddField ("myform_price", this.c.Price);
		
		WWW w = new WWW(urlBuyCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			this.displayErrorPopUp(w.error);
		} else
		{
			if (w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				if (w.text.Contains("#SOLD#"))
				{
					this.c.onSale = 0;
					this.c.IdOWner=-1;
					this.setCardSold();
				}
				else if (w.text.Contains("#PRICECHANGED#"))
				{
					string[] newPrice = w.text.Split(new string[] { "#PRICECHANGED#" }, System.StringSplitOptions.None);
					this.c.Price=System.Convert.ToInt32(newPrice[0]);
					this.actualizePrice();
					this.displayErrorPopUp(errors [1]);
				}
				else
				{
					this.displayErrorPopUp(errors [1]);
				}
			}
			else
			{
				this.c.onSale = 0;
				this.c.isMine=true;
				string[] data = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
				string[] cardData = data [0].Split(new string[] { "//" }, System.StringSplitOptions.None);
				this.collectionPointsEarned = System.Convert.ToInt32(cardData [0]);
				this.idCardTypeUnlocked=System.Convert.ToInt32(cardData[1]);
				this.titleCardTypeUnlocked=cardData[2];
				string[] newSkills = data [1].Split(new string[] { "//" }, System.StringSplitOptions.None);
				for (int i=0; i<newSkills.Length-1; i++)
				{
					this.skillsUnlocked.Add(new Skill());
					this.skillsUnlocked [i].Name = newSkills [i];
				}
				int newIdOwner = System.Convert.ToInt32(data[2]);
				Notification tempNotification = new Notification(c.IdOWner,newIdOwner,false,2,"",c.Id.ToString(),c.Price.ToString(),"");
				StartCoroutine(tempNotification.add ());
				this.c.IdOWner=newIdOwner;

				if(this.collectionPointsEarned>0)
				{
					MenuController.instance.displayCollectionPointsPopUp(this.collectionPointsEarned,this.newCollectionRanking);
				}
				if(this.skillsUnlocked.Count>0)
				{
					MenuController.instance.displayNewSkillsPopUp(this.skillsUnlocked);
				}
				if(this.idCardTypeUnlocked!=-1)
				{
					MenuController.instance.displayNewCardTypePopUp(this.titleCardTypeUnlocked);
				}
				this.deleteCard();
			}
		}
		this.hideLoadingScreen ();
	}
	public virtual void actualizePrice()
	{
	}
	public virtual void deleteCard()
	{
	}
	public virtual void updateScene()
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
		this.hideEditSellPricePopUp ();
		this.displayLoadingScreen ();
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", this.c.Id);
		form.AddField("myform_price", newPrice);
		WWW w = new WWW(urlChangeMarketPrice, form); 				            // On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			this.displayErrorPopUp(w.error);
		} 
		else
		{
			if (w.text == "")
			{
				this.c.Price = newPrice;
				this.updateFocus ();
				this.updateScene();
			}
			else
			{
				this.displayErrorPopUp(w.text);
			}
		}
		this.hideLoadingScreen ();
	}
	public void unsellCardHandler()
	{
		StartCoroutine(this.unsellCard());
	}
	public IEnumerator unsellCard()
	{
		this.hideEditSellPopUp ();
		this.displayLoadingScreen ();

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", this.c.Id);
		WWW w = new WWW(urlRemoveFromMarket, form);             				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			this.displayErrorPopUp(w.error);
		} 
		else
		{
			if (w.text == "")
			{
				this.c.onSale = 0;
			}
			else
			{
				this.displayErrorPopUp(w.text);
			}
		}
		this.updateFocus ();
		this.updateScene ();
		this.hideLoadingScreen ();
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
		this.hidePutOnMarketPopUp ();
		this.displayLoadingScreen ();

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", this.c.Id);
		form.AddField("myform_price", price);	
		WWW w = new WWW(urlPutOnMarket, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			this.displayErrorPopUp(w.error);
		} 
		else
		{
			if (w.text == "")
			{
				this.c.onSale = 1;
				this.c.Price = price;
			}
			else
			{
				this.displayErrorPopUp(w.text);
			}
		}
		this.updateFocus ();
		this.updateScene ();
		this.hideLoadingScreen ();
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
		else if(tempString.Length>11 )
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
	public void cleanFocus()
	{
		if(this.isCardUpgradeDisplayed)
		{
			this.isCardUpgradeDisplayed=false;
			this.cardUpgrade.SetActive(false);
		}
		if(this.isSkillPopUpDisplayed)
		{
			this.hideSkillPopUp();
		}
		if(this.isSkillHighlighted)
		{
			this.isSkillHighlighted=false;
			this.skills[this.c.Skills.Count-1].GetComponent<NewFocusedCardSkillController>().highlightSkill(false);
		}
		if(this.isSoldCardViewDisplayed)
		{
			this.hideSoldCardPopUp();
		}
		if(this.isPanelSoldIsDisplayed)
		{
			this.hidePanelSold();
		}
		if(this.isXpBeingUpdated)
		{
			this.experience.GetComponent<NewFocusedCardExperienceController>().setToUpdateXp(false);
			this.setIsXpBeingUpdated(false);
		}
		if(this.isNextLevelPopUpDisplayed)
		{
			this.hideNextLevelPopUp();
		}
		if(this.isErrorViewDisplayed)
		{
			this.hideErrorPopUp();
		}
	}
	public virtual void exitCard()
	{
		this.cleanFocus();
		this.goBackToScene();
	}
	public virtual void goBackToScene()
	{
	}
	public void focusFeaturesHandler (int feature)
	{
		if(!this.isNextLevelPopUpDisplayed&&!this.isXpBeingUpdated)
		{
			this.selectAFeature(feature);
		}
	}
	public virtual void selectAFeature(int feature)
	{
	}
	public void updateFocus()
	{
		this.updateFocusFeatures ();
		this.refreshCredits();
	}
	public void resize()
	{
		if(isEditSellViewDisplayed)
		{
			this.editSellPopUpResize();
		}
		else if(isSellViewDisplayed)
		{
			this.sellPopUpResize();
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
		else if(isSoldCardViewDisplayed)
		{
			this.soldCardPopUpResize();
		}
		else if(this.isErrorViewDisplayed)
		{
			this.hideErrorPopUp();
		}
		else if(this.isNextLevelPopUpDisplayed)
		{
			this.nextLevelPopUp.GetComponent<NextLevelPopUpController>().resize();
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
		else if(this.isErrorViewDisplayed)
		{
			this.hideErrorPopUp();
		}
		else if(isSoldCardViewDisplayed)
		{
			this.exitCard();
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
		else if(this.isErrorViewDisplayed)
		{
			this.hideErrorPopUp();
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
			this.exitCard();
		}
	}
	public void setCardUpgrade()
	{
		this.cardUpgrade.SetActive(true);
		this.cardUpgrade.GetComponent<NewCardUpgradeController> ().setCardUpgrade (this.caracteristicIncrease);
		this.cardUpgrade.transform.position = this.getCardUpgradePosition (this.caracteristicUpgraded);
		this.timerCardUpgrade = 0;
		this.isCardUpgradeDisplayed = true;
	}
	public void setHighlightedSkills()
	{
		if(isSkillHighlighted)
		{
			this.skills[this.c.Skills.Count-2].GetComponent<NewFocusedCardSkillController>().highlightSkill(false);
		}
		else
		{
			this.isSkillHighlighted=true;
		}
		this.skills[this.c.Skills.Count-1].GetComponent<NewFocusedCardSkillController>().highlightSkill(true);
		this.timerSkillHighlighted=0;
	}
	public virtual Vector3 getCardUpgradePosition (int caracteristicUpgraded)
	{
		GameObject refObject = new GameObject ();
		float gap = 0.6f;
		switch(caracteristicUpgraded)
		{
		case 0:
			refObject = this.attack.transform.FindChild("Text").gameObject;
			break;
		case 1:
			refObject=this.life.transform.FindChild("Text").gameObject;
			break;
		case 2:
			refObject=this.quickness.transform.FindChild("Text").gameObject;
			break;
		case 3:
			refObject=this.skills[0].transform.FindChild ("Power").gameObject;
			break;
		case 4:
			refObject=this.skills[1].transform.FindChild ("Power").gameObject;
			break;
		case 5:
			refObject=this.skills[2].transform.FindChild ("Power").gameObject;
			break;
		case 6:
			refObject=this.skills[3].transform.FindChild ("Power").gameObject;
			break;
		}
		Vector3 refPosition =refObject.transform.position;
		float refSizeX = refObject.transform.GetComponent<MeshRenderer> ().bounds.size.x;
		return new Vector3 (refPosition.x+gap+refSizeX/2f,refPosition.y,0f);
	}
	public void setBackFace(bool value)
	{
		if(value)
		{
			this.applyBackTexture();
			this.face.GetComponent<SpriteRenderer> ().sortingOrder = 10;
		}
		else
		{
			this.applyFrontTexture();
			this.face.GetComponent<SpriteRenderer> ().sortingOrder = 0;
		}

	}
	public virtual void applyBackTexture()
	{
		this.face.GetComponent<SpriteRenderer> ().sprite = ressources.backFace;
	}
	public virtual void animateExperience()
	{
		this.setIsXpBeingUpdated (true);
		MenuController.instance.setIsUserBusy (true);
		this.experience.GetComponent<NewFocusedCardExperienceController>().startUpdatingXp(this.c.ExperienceLevel,this.c.PercentageToNextLevel);
	}
	public virtual Color getColors(int id)
	{
		return this.ressources.colors[id];	
	}
	public virtual Sprite getSkillSprite(int id)
	{
		return this.ressources.skills [id];
	}
	public Sprite getSkillTypeSprite(int id)
	{
		return this.ressources.skillTypes [id];
	}
	public void displayLoadingScreen()
	{
		MenuController.instance.displayLoadingScreen ();
	}
	public void hideLoadingScreen()
	{
		MenuController.instance.hideLoadingScreen ();
	}
	public void setIsXpBeingUpdated(bool value)
	{
		this.isXpBeingUpdated = value;
	}
	public bool getIsXpBeingUpdated()
	{
		return this.isXpBeingUpdated;
	}
	public void displayNextLevelPopUp()
	{
		this.nextLevelPopUp = Instantiate(ressources.nextLevelPopUpObject) as GameObject;
		this.nextLevelPopUp.transform.parent=this.gameObject.transform;
		this.nextLevelPopUp.transform.position = new Vector3(ApplicationDesignRules.menuPosition.x+this.face.transform.position.x,ApplicationDesignRules.menuPosition.y+this.face.transform.position.y,-2f);
		this.nextLevelPopUp.AddComponent<NextLevelPopUpControllerNewFocusedCard> ();
		this.nextLevelPopUp.transform.GetComponent<NextLevelPopUpController> ().initialize (this.c);
		this.isNextLevelPopUpDisplayed=true;
	}
	public void hideNextLevelPopUp()
	{
		MenuController.instance.setIsUserBusy (false);
		Destroy (this.nextLevelPopUp);
		this.isNextLevelPopUpDisplayed=false;
	}
	public void clickOnAttribute(int index, int newPower, int newLevel)
	{
		StartCoroutine(this.upgradeCardAttribute(index, newPower, newLevel));
	}
	public void showSkillTypePopUp(int id)
	{
		this.skillPopUp.SetActive (true);
		this.isSkillPopUpDisplayed = true;
		this.skillPopUp.transform.FindChild ("title").GetComponent<TextMeshPro> ().text = this.c.Skills [id].SkillType.Name;
		this.skillPopUp.transform.FindChild ("description").GetComponent<TextMeshPro> ().text = this.c.Skills [id].SkillType.Description;
		this.skillPopUp.transform.position = new Vector3 (this.skills[id].transform.FindChild ("SkillType").position.x, this.skills[id].transform.FindChild ("SkillType").position.y-System.Convert.ToInt32(id==0)*1f+System.Convert.ToInt32(id>0)*1f, 0f);
	}
	public void hideSkillPopUp()
	{
		this.skillPopUp.SetActive (false);
		this.isSkillPopUpDisplayed = false;
	}
	public void showSkillProbaPopUp(int id)
	{
		this.skillPopUp.SetActive (true);
		this.isSkillPopUpDisplayed = true;
		this.skillPopUp.transform.FindChild ("title").GetComponent<TextMeshPro> ().text = "Probabilité de succès";
		this.skillPopUp.transform.FindChild ("description").GetComponent<TextMeshPro> ().text = "Cette compétence a un taux de réussite de : "+this.c.Skills[id].proba+" %.";
		this.skillPopUp.transform.position = new Vector3 (this.skills[id].transform.FindChild ("Proba").position.x, this.skills[id].transform.FindChild ("Proba").position.y+1f, 0f);
	}
}