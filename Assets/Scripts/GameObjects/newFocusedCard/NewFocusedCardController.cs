using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class NewFocusedCardController : MonoBehaviour 
{
	private NewFocusedCardRessources ressources;

	public GameObject[] skills;
	public GameObject experience;
	public GameObject cardUpgrade;
	public GameObject panelSold;
	public GameObject skillPopUp;
	public GameObject nextLevelPopUp;
	public GameObject attack;
	public GameObject life;
	public GameObject face;
	public GameObject caracter;
	public GameObject cardbox;
	public GameObject card;
	public GameObject cardType;
	public GameObject name;

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
	
	private GameObject sellPopUp;
	private bool isSellPopUpDisplayed;
	private GameObject renamePopUp;
	private bool isRenamePopUpDisplayed;
	private GameObject buyPopUp;
	private bool isBuyPopUpDisplayed;
	private GameObject buyXpPopUp;
	private bool isBuyXpPopUpDisplayed;
	private GameObject editSellPopUp;
	private bool isEditSellPopUpDisplayed;
	private GameObject editSellPricePopUp;
	private bool isEditSellPricePopUpDisplayed;
	private GameObject putOnMarketPopUp;
	private bool isPutOnMarketPopUpDisplayed;
	private GameObject soldCardPopUp;
	private bool isSoldCardPopUpDisplayed;

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

	private GameObject skillFocused;
	private bool isSkillFocusedDisplayed;

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
				BackOfficeController.instance.hideTransparentBackground ();
				this.endUpdatingCardToNextLevel();
			}
		}
	}
	public virtual void Awake()
	{
		this.skills=new GameObject[4];
		this.getRessources ();
		this.setUpdateSpeed ();
		this.cardUpgrade = this.gameObject.transform.FindChild ("CardUpgrade").gameObject;
		this.panelSold = this.gameObject.transform.FindChild ("PanelSold").gameObject;
		this.panelSold.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingCard.getReference(2);
		this.skillPopUp = this.gameObject.transform.FindChild ("SkillPopUp").gameObject;
		this.buyPopUp = this.gameObject.transform.FindChild ("BuyPopUp").gameObject;
		this.editSellPopUp = this.gameObject.transform.FindChild ("EditSellPopUp").gameObject;
		this.editSellPricePopUp = this.gameObject.transform.FindChild ("EditSellPricePopUp").gameObject;
		this.putOnMarketPopUp = this.gameObject.transform.FindChild ("PutOnMarketPopUp").gameObject;
		this.soldCardPopUp = this.gameObject.transform.FindChild ("SoldCardPopUp").gameObject;
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
		this.attack = this.gameObject.transform.FindChild ("Card").FindChild ("Attack").gameObject;
		this.life = this.gameObject.transform.FindChild ("Card").FindChild ("Life").gameObject;
		this.cardbox = this.gameObject.transform.FindChild ("Card").FindChild ("cardbox").gameObject;
		this.face = this.gameObject.transform.FindChild("Card").FindChild ("Face").gameObject;
		this.caracter = this.gameObject.transform.FindChild("Card").FindChild ("Caracter").gameObject;
		this.card = this.gameObject.transform.FindChild ("Card").gameObject;
		this.sellPopUp = this.gameObject.transform.FindChild ("SellPopUp").gameObject;
		this.renamePopUp = this.gameObject.transform.FindChild ("RenamePopUp").gameObject;
		this.buyXpPopUp = this.gameObject.transform.FindChild ("BuyXpPopUp").gameObject;
		this.cardType = this.gameObject.transform.FindChild("Card").FindChild("CardType").gameObject;
		this.name=this.gameObject.transform.FindChild("Card").FindChild("Name").gameObject;

	}
	public virtual void getRessources()
	{
		this.ressources = this.gameObject.GetComponent<NewFocusedCardRessources> ();
	}
	public void setUpdateSpeed()
	{
		this.speed = 5f;
	}
	public virtual void show()
	{
		this.applyFrontTexture ();
		this.name.GetComponent<TextMeshPro>().text=this.c.getName();
		this.life.transform.FindChild("Text").GetComponent<TextMeshPro>().text = this.c.GetLifeString();
		this.life.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.returnCardColor (this.c.LifeLevel);
		this.attack.transform.FindChild("Text").GetComponent<TextMeshPro>().text = this.c.GetAttackString();
		this.attack.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.returnCardColor (this.c.AttackLevel);
		this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(this.c.CardType.getPictureId());

		for(int i=0;i<this.skills.Length;i++)
		{
			if(i<this.c.Skills.Count && this.c.Skills[i].IsActivated==1)
			{
				this.skills[i].transform.GetComponent<NewFocusedCardSkillController>().setSkill(this.c.Skills[i]);
				string description = this.c.getSkillText(WordingSkills.getDescription(this.c.Skills[i].Id,this.c.Skills[i].Power-1));
				if(i!=0)
				{
					description +=WordingCard.getReference(0)+this.c.Skills[i].getProba(c.Skills[i].Power-1)+WordingCard.getReference(1);
				}
				this.skills[i].transform.GetComponent<NewFocusedCardSkillController>().setDescription(description);
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
		this.caracter.GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnLargeCardsCaracter(this.c.Skills[0].getPictureId());
		this.face.GetComponent<SpriteRenderer> ().sprite = ressources.faces [this.c.PowerLevel - 1];
	}
	public void setCardSold()
	{
		if(!isPanelSoldIsDisplayed)
		{
			this.displayPanelSold();
			this.closePopUps ();
			this.displaySoldPopUp ();
			this.updateFocusFeatures();
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
				BackOfficeController.instance.displayCollectionPointsPopUp(this.collectionPointsEarned,this.newCollectionRanking);
			}
			if(this.skillsUnlocked.Count>0)
			{
				BackOfficeController.instance.displayNewSkillsPopUps(this.skillsUnlocked);
			}
			if(this.c.GetNewSkill)
			{
				this.setHighlightedSkills();
			}
			this.isNextLevelPopUpDisplaying=true;
			BackOfficeController.instance.displayTransparentBackground ();
		}
		else
		{
			ApplicationModel.player.IsBusy=false;
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
			BackOfficeController.instance.displayCollectionPointsPopUp(this.collectionPointsEarned,this.newCollectionRanking);
		}
		if(this.idCardTypeUnlocked!=-1)
		{
			BackOfficeController.instance.displayNewCardTypePopUp(this.titleCardTypeUnlocked);
		}
		if(this.caracteristicUpgraded>-1&&this.caracteristicIncrease>0)
		{
			this.setCardUpgrade();
		}
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
		this.closePopUps();
		BackOfficeController.instance.displayTransparentBackground ();
		this.sellPopUp.transform.GetComponent<SellPopUpController> ().reset (this.c.destructionPrice);
		this.isSellPopUpDisplayed = true;
		this.sellPopUp.SetActive (true);
		this.sellPopUpResize ();
	}
	public void displayRenameCardPopUp()
	{
		this.closePopUps();
		BackOfficeController.instance.displayTransparentBackground ();
		this.renamePopUp.transform.GetComponent<RenamePopUpController> ().reset (this.c.RenameCost,this.c.Title);
		this.isRenamePopUpDisplayed = true;
		this.renamePopUp.SetActive (true);
		this.renamePopUpResize ();
	}
	public void displayBuyXpCardPopUp()
	{
		this.closePopUps();
		BackOfficeController.instance.displayTransparentBackground ();
		this.buyXpPopUp.transform.GetComponent<BuyXpPopUpController> ().reset (this.c.NextLevelPrice);
		this.isBuyXpPopUpDisplayed = true;
		this.buyXpPopUp.SetActive (true);
		this.buyXpPopUpResize ();
	}
	public void displayBuyCardPopUp()
	{
		this.closePopUps();
		BackOfficeController.instance.displayTransparentBackground ();
		this.buyPopUp.transform.GetComponent<BuyPopUpController> ().reset (this.c.Price);
		this.isBuyPopUpDisplayed = true;
		this.buyPopUp.SetActive (true);
		this.buyPopUpResize ();
	}
	public void displayEditSellCardPopUp()
	{
		this.closePopUps();
		BackOfficeController.instance.displayTransparentBackground ();
		this.editSellPopUp.transform.GetComponent<EditSellPopUpController> ().reset (this.c.Price);
		this.isEditSellPopUpDisplayed = true;
		this.editSellPopUp.SetActive (true);
		this.editSellPopUpResize ();
	}
	public void displayEditSellPriceCardPopUp()
	{
		this.closePopUps();
		BackOfficeController.instance.displayTransparentBackground ();
		this.editSellPricePopUp.transform.GetComponent<EditSellPricePopUpController> ().reset (this.c.Price);
		this.isEditSellPricePopUpDisplayed = true;
		this.editSellPricePopUp.SetActive (true);
		this.editSellPricePopUpResize ();
	}
	public void displayputOnMarketCardPopUp()
	{
		this.closePopUps();
		BackOfficeController.instance.displayTransparentBackground ();
		this.putOnMarketPopUp.transform.GetComponent<PutOnMarketPopUpController> ().reset ();
		this.isPutOnMarketPopUpDisplayed = true;
		this.putOnMarketPopUp.SetActive (true);
		this.putOnMarketPopUpResize ();
	}
	public void displaySoldPopUp()
	{
		this.closePopUps();
		BackOfficeController.instance.displayTransparentBackground ();
		this.soldCardPopUp.transform.GetComponent<SoldCardPopUpController> ().reset ();
		this.isSoldCardPopUpDisplayed = true;
		this.soldCardPopUp.SetActive (true);
		this.soldCardPopUpResize ();
	}
	private void sellPopUpResize()
	{
		this.sellPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.sellPopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1/this.gameObject.transform.localScale.x);
		this.sellPopUp.GetComponent<SellPopUpController> ().resize ();
	}
	private void renamePopUpResize()
	{
		this.renamePopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.renamePopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1/this.gameObject.transform.localScale.x);
		this.renamePopUp.GetComponent<RenamePopUpController> ().resize ();
	}
	private void buyXpPopUpResize()
	{
		this.buyXpPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.buyXpPopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1/this.gameObject.transform.localScale.x);
		this.buyXpPopUp.GetComponent<BuyXpPopUpController> ().resize ();
	}
	private void buyPopUpResize()
	{
		this.buyPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.buyPopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1/this.gameObject.transform.localScale.x);
		this.buyPopUp.GetComponent<BuyPopUpController> ().resize ();
	}
	private void editSellPopUpResize()
	{
		this.editSellPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.editSellPopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1/this.gameObject.transform.localScale.x);
		this.editSellPopUp.GetComponent<EditSellPopUpController> ().resize ();
	}
	private void editSellPricePopUpResize()
	{
		this.editSellPricePopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.editSellPricePopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1/this.gameObject.transform.localScale.x);
		this.editSellPricePopUp.GetComponent<EditSellPricePopUpController> ().resize ();
	}
	private void putOnMarketPopUpResize()
	{
		this.putOnMarketPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.putOnMarketPopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1/this.gameObject.transform.localScale.x);
		this.putOnMarketPopUp.GetComponent<PutOnMarketPopUpController> ().resize ();
	}
	private void soldCardPopUpResize()
	{
		this.soldCardPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.soldCardPopUp.transform.localScale = ApplicationDesignRules.popUpScale*(1/this.gameObject.transform.localScale.x);
		this.soldCardPopUp.GetComponent<SoldCardPopUpController> ().resize ();
	}
	public void hideSellPopUp()
	{
		this.sellPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isSellPopUpDisplayed = false;
	}
	public void hideRenamePopUp()
	{
		this.renamePopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isRenamePopUpDisplayed = false;
	}
	public void hideBuyXpPopUp()
	{
		this.buyXpPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isBuyXpPopUpDisplayed = false;
	}
	public void hideBuyPopUp()
	{
		this.buyPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isBuyPopUpDisplayed = false;
	}
	public void hideEditSellPopUp()
	{
		this.editSellPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isEditSellPopUpDisplayed = false;
	}
	public void hideEditSellPricePopUp()
	{
		this.editSellPricePopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isEditSellPricePopUpDisplayed = false;
	}
	public void hidePutOnMarketPopUp()
	{
		this.putOnMarketPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isPutOnMarketPopUpDisplayed = false;
	}
	public void hideSoldCardPopUp()
	{
		this.soldCardPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isSoldCardPopUpDisplayed = false;
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
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_idcard", this.c.Id);		
		WWW w = new WWW(urlSellCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null)
		{
			BackOfficeController.instance.displayErrorPopUp(w.error);
		} 
		else
		{
			if(w.text=="")
			{
				this.deleteCard();
			}
			else
			{
				BackOfficeController.instance.displayErrorPopUp(w.text);
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
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField ("myform_attribute", attributeToUpgrade);
		form.AddField ("myform_newpower", newPower);
		form.AddField ("myform_newlevel", newLevel);
		
		WWW w = new WWW(urlUpgradeCardAttribute, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			BackOfficeController.instance.displayErrorPopUp(w.error);									// donne l'erreur eventuelle
		} 
		else
		{
			if (w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				BackOfficeController.instance.displayErrorPopUp(errors[1]);
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
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_idcard", this.c.Id);
		form.AddField("myform_title", newName);
		form.AddField("myform_cost", this.c.RenameCost);
		
		WWW w = new WWW(urlRenameCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			BackOfficeController.instance.displayErrorPopUp(w.error);
		} 
		else
		{
			if (w.text == "")
			{
				this.c.Title = newName;
				this.name.GetComponent<TextMeshPro> ().text = this.c.Title;
			}
			else
			{
				BackOfficeController.instance.displayErrorPopUp(w.text);
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
		form.AddField("myform_nick", ApplicationModel.player.Username);
		
		WWW w = new WWW(urlAddXpLevel, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			BackOfficeController.instance.displayErrorPopUp(w.error); 										// donne l'erreur eventuelle
		} 
		else
		{
			if (w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				BackOfficeController.instance.displayErrorPopUp(errors [1]);
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
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_idcard", this.c.Id);
		form.AddField ("myform_price", this.c.Price);
		
		WWW w = new WWW(urlBuyCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			BackOfficeController.instance.displayErrorPopUp(w.error);
		} 
		else
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
					BackOfficeController.instance.displayErrorPopUp(errors [1]);
				}
				else
				{
					BackOfficeController.instance.displayErrorPopUp(errors [1]);
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
					this.skillsUnlocked [i].Id = System.Convert.ToInt32(newSkills [i]);
				}
				int newIdOwner = System.Convert.ToInt32(data[2]);
				Notification tempNotification = new Notification(c.IdOWner,newIdOwner,false,2,"",c.Id.ToString(),c.Price.ToString(),"");
				StartCoroutine(tempNotification.add ());
				this.c.IdOWner=newIdOwner;

				if(this.collectionPointsEarned>0)
				{
					BackOfficeController.instance.displayCollectionPointsPopUp(this.collectionPointsEarned,this.newCollectionRanking);
				}
				if(this.skillsUnlocked.Count>0)
				{
					BackOfficeController.instance.displayNewSkillsPopUps(this.skillsUnlocked);
				}
				if(this.idCardTypeUnlocked!=-1)
				{
					BackOfficeController.instance.displayNewCardTypePopUp(this.titleCardTypeUnlocked);
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
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_idcard", this.c.Id);
		form.AddField("myform_price", newPrice);
		WWW w = new WWW(urlChangeMarketPrice, form); 				            // On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			BackOfficeController.instance.displayErrorPopUp(w.error);
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
				BackOfficeController.instance.displayErrorPopUp(w.text);
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
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_idcard", this.c.Id);
		WWW w = new WWW(urlRemoveFromMarket, form);             				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			BackOfficeController.instance.displayErrorPopUp(w.error);
		} 
		else
		{
			if (w.text == "")
			{
				this.c.onSale = 0;
			}
			else
			{
				BackOfficeController.instance.displayErrorPopUp(w.text);
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
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_idcard", this.c.Id);
		form.AddField("myform_price", price);	
		WWW w = new WWW(urlPutOnMarket, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;

		if (w.error != null)
		{
			BackOfficeController.instance.displayErrorPopUp(w.error);
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
				BackOfficeController.instance.displayErrorPopUp(w.text);
			}
		}
		this.updateFocus ();
		this.updateScene ();
		this.hideLoadingScreen ();
	}
	public string renameCardSyntaxCheck()
	{
		string tempString = this.renamePopUp.transform.GetComponent<RenamePopUpController> ().getFirstInput ();;
		string error="";
		if(tempString=="")
		{
			error=WordingFocusedCard.getReference(0);
			tempString="";
		}
		else if(tempString==this.c.Title)
		{
			error=WordingFocusedCard.getReference(1);
			tempString="";
		}
		else if(tempString.Length<4 )
		{
			error= WordingFocusedCard.getReference(2);
			tempString="";
		}
		else if(tempString.Length>11 )
		{
			error=WordingFocusedCard.getReference(3);
			tempString="";
		}
		else if(!Regex.IsMatch(tempString, @"^[a-zA-Z0-9_]+$"))
		{
			error=WordingFocusedCard.getReference(4);
			tempString="";
		}
		this.renamePopUp.transform.GetComponent<RenamePopUpController> ().setError (error);
		return tempString;
	}
	public int editSellPriceSyntaxCheck()
	{
		int n;
		string priceString = this.editSellPricePopUp.transform.GetComponent<EditSellPricePopUpController> ().getFirstInput ();
		bool isNumeric = int.TryParse(priceString, out n);
		if(priceString!="" && isNumeric)
		{
			if(System.Convert.ToInt32(priceString)>0)
			{
				return System.Convert.ToInt32(priceString);
			}
		}
		this.editSellPricePopUp.transform.GetComponent<EditSellPricePopUpController> ().setError(WordingFocusedCard.getReference(5));
		return -1;
	}
	public int putOnMarketSyntaxCheck()
	{
		int n;
		string priceString = this.putOnMarketPopUp.transform.GetComponent<PutOnMarketPopUpController> ().getFirstInput ();
		bool isNumeric = int.TryParse(priceString, out n);
		if(priceString!="" && isNumeric)
		{
			if(System.Convert.ToInt32(priceString)>0)
			{ 
				return System.Convert.ToInt32(priceString);
			}
		}
		this.putOnMarketPopUp.transform.GetComponent<PutOnMarketPopUpController> ().setError(WordingFocusedCard.getReference(5));
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
		if(this.isSoldCardPopUpDisplayed)
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
		if(this.isSkillFocusedDisplayed)
		{
			this.hideSkillFocused();
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
		if(isSkillFocusedDisplayed)
		{
			this.resizeSkillFocused();
		}
		else if(isEditSellPopUpDisplayed)
		{
			this.editSellPopUpResize();
		}
		else if(isSellPopUpDisplayed)
		{
			this.sellPopUpResize();
		}
		else if(isRenamePopUpDisplayed)
		{
			this.renamePopUpResize();
		}
		else if(isBuyXpPopUpDisplayed)
		{
			this.buyXpPopUpResize();
		}
		else if(isBuyPopUpDisplayed)
		{
			this.buyXpPopUpResize();
		}
		else if(isEditSellPricePopUpDisplayed)
		{
			this.editSellPricePopUpResize();
		}
		else if(isPutOnMarketPopUpDisplayed)
		{
			this.putOnMarketPopUpResize();
		}
		else if(isSoldCardPopUpDisplayed)
		{
			this.soldCardPopUpResize();
		}
		else if(this.isNextLevelPopUpDisplayed)
		{
			this.nextLevelPopUp.GetComponent<NextLevelPopUpController>().resize();
		}
	}
	public void returnPressed()
	{
		if(isEditSellPopUpDisplayed)
		{
			this.editSellPriceCardHandler();
		}
		else if(isSellPopUpDisplayed)
		{
			this.sellCardHandler();
		}
		else if(isRenamePopUpDisplayed)
		{
			this.renameCardHandler();
		}
		else if(isBuyXpPopUpDisplayed)
		{
			this.buyXpCardHandler();
		}
		else if(isBuyPopUpDisplayed)
		{
			this.buyCardHandler();
		}
		else if(isEditSellPricePopUpDisplayed)
		{
			this.editSellPriceCardHandler();
		}
		else if(isPutOnMarketPopUpDisplayed)
		{
			this.putOnMarketCardHandler();
		}
		else if(isSoldCardPopUpDisplayed)
		{
			this.exitCard();
		}
	}
	public bool closePopUps()
	{
		if(isSkillFocusedDisplayed)
		{
			this.hideSkillFocused();
		}
		if(isEditSellPopUpDisplayed)
		{
			this.hideEditSellPopUp();
		}
		else if(isSellPopUpDisplayed)
		{
			this.hideSellPopUp();
		}
		else if(isRenamePopUpDisplayed)
		{
			this.hideRenamePopUp();
		}
		else if(isBuyXpPopUpDisplayed)
		{
			this.hideBuyXpPopUp();
		}
		else if(isBuyPopUpDisplayed)
		{
			this.hideBuyPopUp();
		}
		else if(isEditSellPricePopUpDisplayed)
		{
			this.hideEditSellPricePopUp();
		}
		else if(isPutOnMarketPopUpDisplayed)
		{
			this.hidePutOnMarketPopUp();
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
		ApplicationModel.player.IsBusy=true;
		this.experience.GetComponent<NewFocusedCardExperienceController>().startUpdatingXp(this.c.ExperienceLevel,this.c.PercentageToNextLevel);
	}
	public virtual Sprite getSkillSprite(int id)
	{
		return this.ressources.skills [id];
	}
	public Sprite getSkillTypeSprite(int id)
	{
		return this.ressources.skillTypes [id];
	}
	public CardType getCardType()
	{
		return this.c.CardType;
	}
	public void displayLoadingScreen()
	{
		BackOfficeController.instance.displayLoadingScreen ();
	}
	public void hideLoadingScreen()
	{
		BackOfficeController.instance.hideLoadingScreen ();
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
		this.closePopUps();
		this.isNextLevelPopUpDisplayed=true;
		this.nextLevelPopUp = Instantiate(ressources.nextLevelPopUpObject) as GameObject;
		this.nextLevelPopUp.transform.parent=this.gameObject.transform;
		this.nextLevelPopUp.AddComponent<NextLevelPopUpControllerNewFocusedCard> ();
		this.nextLevelPopUp.transform.GetComponent<NextLevelPopUpController> ().initialize (this.c);
	}
	public void hideNextLevelPopUp()
	{
		ApplicationModel.player.IsBusy=false;
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
		this.skillPopUp.transform.FindChild ("title").GetComponent<TextMeshPro> ().text = WordingSkillTypes.getName(this.c.Skills [id].IdSkillType);
		this.skillPopUp.transform.FindChild ("description").GetComponent<TextMeshPro> ().text =WordingSkillTypes.getDescription(this.c.Skills [id].IdSkillType);

		float skillTypePopUpWorldSize=0f;
		float skillTypePopUpXPosition=0f;

		skillTypePopUpWorldSize=this.skillPopUp.GetComponent<SpriteRenderer>().bounds.size.x;
		
		if(this.skills[id].transform.FindChild ("SkillType").position.x-skillTypePopUpWorldSize/2f<-ApplicationDesignRules.worldWidth/2f)
		{
			skillTypePopUpXPosition=this.skills[id].transform.FindChild ("SkillType").position.x-(this.skills[id].transform.FindChild ("SkillType").position.x-skillTypePopUpWorldSize/2f+ApplicationDesignRules.worldWidth/2f);
		}
		else if(this.skills[id].transform.FindChild ("SkillType").position.x+skillTypePopUpWorldSize/2f>ApplicationDesignRules.worldWidth/2f)
		{
			skillTypePopUpXPosition=this.skills[id].transform.FindChild ("SkillType").position.x-(this.skills[id].transform.FindChild ("SkillType").position.x+skillTypePopUpWorldSize/2f-ApplicationDesignRules.worldWidth/2f);
		}
		else
		{
			skillTypePopUpXPosition=this.skills[id].transform.FindChild ("SkillType").position.x;
		}
		this.skillPopUp.transform.position = new Vector3 (skillTypePopUpXPosition, this.skills[id].transform.FindChild ("SkillType").position.y-System.Convert.ToInt32(id==0)*1f+System.Convert.ToInt32(id>0)*1f, 0f);
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
		this.skillPopUp.transform.FindChild ("title").GetComponent<TextMeshPro> ().text = WordingFocusedCard.getReference(6);
		this.skillPopUp.transform.FindChild ("description").GetComponent<TextMeshPro> ().text = WordingFocusedCard.getReference(7)+this.c.Skills[id].proba.ToString()+WordingFocusedCard.getReference(8);

		float skillProbaPopUpWorldSize=0f;
		float skillProbaPopUpXPosition=0f;
		
		skillProbaPopUpWorldSize=this.skillPopUp.GetComponent<SpriteRenderer>().bounds.size.x;
		
		if(this.skills[id].transform.FindChild ("Proba").position.x-skillProbaPopUpWorldSize/2f<-ApplicationDesignRules.worldWidth/2f)
		{
			skillProbaPopUpXPosition=this.skills[id].transform.FindChild ("Proba").position.x-(this.skills[id].transform.FindChild ("Proba").position.x-skillProbaPopUpWorldSize/2f+ApplicationDesignRules.worldWidth/2f);
		}
		else if(this.skills[id].transform.FindChild ("Proba").position.x+skillProbaPopUpWorldSize/2f>ApplicationDesignRules.worldWidth/2f)
		{
			skillProbaPopUpXPosition=this.skills[id].transform.FindChild ("Proba").position.x-(this.skills[id].transform.FindChild ("Proba").position.x+skillProbaPopUpWorldSize/2f-ApplicationDesignRules.worldWidth/2f);
		}
		else
		{
			skillProbaPopUpXPosition=this.skills[id].transform.FindChild ("Proba").position.x;
		}
		this.skillPopUp.transform.position = new Vector3 (skillProbaPopUpXPosition, this.skills[id].transform.FindChild ("Proba").position.y+1f, 0f);
	}
	public Sprite returnFocusFeaturePicto (int id)
	{
		return this.ressources.focusPictos[id];
	}
	public Vector3 returnFacePosition()
	{
		return this.face.transform.position;
	}
	public void displaySkillFocused(int idSkill)
	{
		this.isSkillFocusedDisplayed=true;
		this.skillFocused = Instantiate(ressources.skillFocusedObject) as GameObject;
		this.skillFocused.transform.parent=this.gameObject.transform;
		this.skillFocused.AddComponent<FocusedSkillControllerFocusedCard> ();
		this.skillFocused.transform.GetComponent<FocusedSkillController>().show(this.c.Skills[idSkill]);
		this.skillFocused.transform.GetComponent<FocusedSkillController>().highlightLevel(c.Skills[idSkill].Power-1);
		this.resizeSkillFocused();
	}
	public void resizeSkillFocused()
	{
		Vector3 cardsPosition = this.card.transform.localPosition;
		cardsPosition.z=cardsPosition.z-1f;
		this.skillFocused.transform.localPosition=cardsPosition;
		this.skillFocused.transform.localScale=this.card.transform.localScale;
	}
	public void hideSkillFocused()
	{
		this.isSkillFocusedDisplayed=false;
		Destroy(this.skillFocused);
	}
	public string getSkillFocusedDescription(int idSkill, int level)
	{
		return this.c.getSkillText(WordingSkills.getDescription(idSkill,level));	
	}
	public bool getIsSkillFocusedDisplayed()
	{
		return this.isSkillFocusedDisplayed;
	}
}