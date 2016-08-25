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
	public GameObject levelUp;
	public GameObject panelSold;
	public GameObject nextLevelPopUp;
	public GameObject attack;
	public GameObject life;
	public GameObject face;
	public GameObject caracter;
	public GameObject cardbox;
	public GameObject card;
	public GameObject cardType;
	new public GameObject name;

	public Card c;
	//public int collectionPointsEarned;
	//public int newCollectionRanking;
	//public List<Skill> skillsUnlocked;
	//public bool getNewSkill;
	//public int caracteristicUpgraded;
	//public int caracteristicIncrease;

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
	public GameObject buyPopUp;
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
				SoundController.instance.playSound(7);
				BackOfficeController.instance.hideTransparentBackground ();
				this.hideNextLevelPopUp ();
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
		this.levelUp=this.gameObject.transform.FindChild("LevelUp").gameObject;
		this.panelSold = this.gameObject.transform.FindChild ("PanelSold").gameObject;
		this.panelSold.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingCard.getReference(2);
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
		this.cardType.transform.GetComponent<NewFocusedCardCardTypeController>().setCardType(this.c.CardType);

		for(int i=0;i<this.skills.Length;i++)
		{
			this.skills[i].transform.GetComponent<NewFocusedCardSkillController>().setId(i);
			if(i<this.c.Skills.Count && this.c.Skills[i].IsActivated==1)
			{
				this.skills[i].transform.GetComponent<NewFocusedCardSkillController>().setSkill(this.c.Skills[i]);
				string description = this.c.getSkillText(WordingSkills.getDescription(this.c.Skills[i].Id,this.c.Skills[i].Power-1));
				if(i!=0 && this.c.Skills[i].getProba(c.Skills[i].Power-1)<100)
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
		this.face.GetComponent<SpriteRenderer> ().sprite = ressources.faces [this.c.PowerLevel-1];
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
			this.levelUp.SetActive(false);
			this.show ();
            Cards cards = new Cards();
            cards.add();
            cards.cards[0]=this.c;
            ApplicationModel.player.updateMyCollection(cards);
            if(this.c.GetNewSkill)
            {
                this.setHighlightedSkills();
            }
			if(this.c.GetNewSkill)
			{
				this.setHighlightedSkills();
			}
			this.isNextLevelPopUpDisplaying=true;
			SoundController.instance.playSound(7);
			BackOfficeController.instance.displayTransparentBackground ();
		}
		else
		{
			ApplicationModel.player.IsBusy=false;
		}
		this.setIsXpBeingUpdated (false);
	}
	public virtual void endUpdatingCardToNextLevel()
	{
		if(this.c.GetNewSkill)
		{
			this.c.GetNewSkill=false;
		}
		this.show ();
		this.updateFocus ();
        Cards cards = new Cards();
        cards.add();
        cards.cards[0]=this.c;
        ApplicationModel.player.updateMyCollection(cards);
        if(this.c.GetNewSkill)
        {
            this.setHighlightedSkills();
        }
		if(this.c.CaracteristicUpgraded>-1&&this.c.CaracteristicIncrease>0)
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
		this.putOnMarketPopUp.transform.GetComponent<PutOnMarketPopUpController> ().reset (this.c.destructionPrice);
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
		SoundController.instance.playSound(5);
		this.hideSellPopUp ();
		this.displayLoadingScreen ();

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_idcard", this.c.Id);

		ServerController.instance.setRequest(urlSellCard, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		string error=ServerController.instance.getError();
		
		if (error != "")
		{
			BackOfficeController.instance.displayErrorPopUp(error); 										// donne l'erreur eventuelle
		} 
		else
		{
			this.deleteCard();
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
	public IEnumerator renameCard(string newName)
	{
		SoundController.instance.playSound(15);
		this.hideRenamePopUp ();
		this.displayLoadingScreen ();
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_idcard", this.c.Id);
		form.AddField("myform_title", newName);
		form.AddField("myform_cost", this.c.RenameCost);

		ServerController.instance.setRequest(urlRenameCard, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		string error=ServerController.instance.getError();

		if (error == "")
		{
			this.c.Title = newName;
			this.name.GetComponent<TextMeshPro> ().text = this.c.Title;
		}
		else 
		{	
			BackOfficeController.instance.displayErrorPopUp(error);
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

		yield return StartCoroutine(ApplicationModel.player.payMoney (this.c.NextLevelPrice));
		if (ApplicationModel.player.Error != "") 
		{
			BackOfficeController.instance.displayErrorPopUp(ApplicationModel.player.Error);
			ApplicationModel.player.Error = "";
		} 
		else 
		{
			this.c.updateCardXp (true,0);
			Cards cards = new Cards ();
			cards.add ();
			cards.cards [cards.getCount () - 1] = this.c;
			ApplicationModel.player.updateMyCollection (cards);
			this.animateExperience();
		}
		this.hideLoadingScreen ();
	}
	public IEnumerator upgradeCardAttribute(int attributeToUpgrade, int newPower, int newLevel)
	{
		this.displayLoadingScreen ();
		this.c.updateCardAttribute (attributeToUpgrade, newPower, newLevel);
		this.c.setString ();
		yield return StartCoroutine(this.c.syncCard ());
		if (this.c.Error != "") 
		{
			Debug.Log (this.c.Error);
			this.c.Error = "";
		}
		Cards cards = new Cards ();
		cards.add ();
		cards.cards [cards.getCount () - 1] = this.c;
		ApplicationModel.player.updateMyCollection (cards);
		this.isNextLevelPopUpHiding=true;
		this.hideLoadingScreen ();
	}
	public void buyCardHandler()
	{
		StartCoroutine (this.buyCard ());
	}
	public IEnumerator buyCard()
	{
		SoundController.instance.playSound(14);
		this.hideBuyPopUp ();
		this.displayLoadingScreen ();

		int oldPrice = this.c.Price;
		//this.skillsUnlocked = new List<Skill>();

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_idcard", this.c.Id);
		form.AddField ("myform_price", this.c.Price);

		ServerController.instance.setRequest(urlBuyCard, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		string error=ServerController.instance.getError();
		string result=ServerController.instance.getResult();
		
		if (error != "" && result.Contains("#SOLD#"))
		{
			this.c.onSale = 0;
			this.c.IdOWner=-1;
			this.setCardSold();
		}
		else if(error != "" && result.Contains("#PRICECHANGED#"))
		{
			string[] newPrice = error.Split(new string[] { "#PRICECHANGED#" }, System.StringSplitOptions.None);
			this.c.Price=System.Convert.ToInt32(newPrice[0]);
			this.actualizePrice();
			BackOfficeController.instance.displayErrorPopUp(error);
		}
		else if(error !="")
		{
			BackOfficeController.instance.displayErrorPopUp(error);
		}
		else
		{
			this.c.onSale = 0;
			this.c.isMine=true;
			string[] data = result.Split(new string[] { "END" }, System.StringSplitOptions.None);
			string[] cardData = data [0].Split(new string[] { "//" }, System.StringSplitOptions.None);
			//this.collectionPointsEarned = System.Convert.ToInt32(cardData [0]);
			string[] newSkills = data [1].Split(new string[] { "//" }, System.StringSplitOptions.None);
//			for (int i=0; i<newSkills.Length-1; i++)
//			{
//				this.skillsUnlocked.Add(new Skill());
//				this.skillsUnlocked [i].Id = System.Convert.ToInt32(newSkills [i]);
//			}
			int newIdOwner = System.Convert.ToInt32(data[2]);
			//this.newCollectionRanking= System.Convert.ToInt32(data[3]);
			Notification tempNotification = new Notification();
			tempNotification.SendingUser=newIdOwner;
			tempNotification.IdUser=c.IdOWner;
			tempNotification.IsRead=false;
			tempNotification.IdNotificationType=2;
			tempNotification.HiddenParam="";
			tempNotification.Param1=c.Id.ToString();
			tempNotification.Param2=c.Price.ToString();
			tempNotification.Param3="";
			StartCoroutine(tempNotification.add ());
			this.c.IdOWner=newIdOwner;
            Cards cards = new Cards();
            cards.add();
            cards.cards[0]=this.c;
            ApplicationModel.player.updateMyCollection(cards);
			this.deleteCard();
		}
		this.hideLoadingScreen();
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
		SoundController.instance.playSound(14);
		this.hideEditSellPricePopUp ();
		this.displayLoadingScreen ();
		WWWForm form = new WWWForm(); 	
												
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_idcard", this.c.Id);
		form.AddField("myform_price", newPrice);

		ServerController.instance.setRequest(urlChangeMarketPrice, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		string error=ServerController.instance.getError();

		if (error == "")
		{
			string result=ServerController.instance.getResult();
			this.c.Price = newPrice;
			this.updateFocus ();
			this.updateScene();
		}
		else 
		{	
			BackOfficeController.instance.displayErrorPopUp(error);
		}
		this.hideLoadingScreen ();
	}
	public void unsellCardHandler()
	{
		StartCoroutine(this.unsellCard());
	}
	public IEnumerator unsellCard()
	{
		SoundController.instance.playSound(14);
		this.hideEditSellPopUp ();
		this.displayLoadingScreen ();

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_idcard", this.c.Id);

		ServerController.instance.setRequest(urlRemoveFromMarket, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		string error=ServerController.instance.getError();

		if (error == "")
		{
			this.c.onSale = 0;
		}
		else 
		{	
			BackOfficeController.instance.displayErrorPopUp(error);
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
		SoundController.instance.playSound(14);
		this.hidePutOnMarketPopUp ();
		this.displayLoadingScreen ();

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_idcard", this.c.Id);
		form.AddField("myform_price", price);

		ServerController.instance.setRequest(urlPutOnMarket, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		string error=ServerController.instance.getError();
		if (error == "")
		{
			this.c.onSale = 1;
			this.c.Price = price;
		}
		else 
		{	
			BackOfficeController.instance.displayErrorPopUp(error);
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
		if(error!="")
		{
			SoundController.instance.playSound(13);
		}
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
		SoundController.instance.playSound(13);
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
		SoundController.instance.playSound(13);
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
			this.levelUp.SetActive(false);
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
		SoundController.instance.playSound(8);
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
			SoundController.instance.playSound(8);
			this.editSellPriceCardHandler();
		}
		else if(isSellPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.sellCardHandler();
		}
		else if(isRenamePopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.renameCardHandler();
		}
		else if(isBuyXpPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.buyXpCardHandler();
		}
		else if(isBuyPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.buyCardHandler();
		}
		else if(isEditSellPricePopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.editSellPriceCardHandler();
		}
		else if(isPutOnMarketPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.putOnMarketCardHandler();
		}
		else if(isSoldCardPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
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
			SoundController.instance.playSound(8);
			this.exitCard();
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
		if(!ApplicationModel.player.NextLevelTutorial)
		{
			HelpController.instance.startHelp();
		}
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
		SoundController.instance.playSound(0);
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

        string description = WordingSkills.getDescription(idSkill,level);
        if(WordingSkills.getProba(idSkill,level)<100)
        {
            description +=WordingCard.getReference(0)+WordingSkills.getProba(idSkill,level)+WordingCard.getReference(1);
        }
        return description;	
	}
	public GameObject returnLevelUpObject()
	{
		return this.levelUp;
	}

	#region help functions

	public bool getIsNextLevelPopUpDisplayed()
	{
		return this.isNextLevelPopUpDisplayed;
	}
	public int GetSkillsNumber()
	{
		for(int i=0;i<4;i++)
		{
			if(this.c.Skills[i].IsActivated==0)
			{
				return (i);
			}
		}
		return 4;
	}
	public bool getIsSkillFocusedDisplayed()
	{
		return this.isSkillFocusedDisplayed;
	}
	public Vector3 getSkillPosition(int id)
	{
		return this.skills[id].transform.position;
	}
	public Vector3 getExperienceLevelPosition()
	{
		return this.experience.transform.FindChild("ExperienceLevel").position;
	}
	public Vector3 getFocusFeaturePosition(int id)
	{
		return this.gameObject.transform.FindChild("FocusFeature"+id).transform.position;
	}
	public Vector3 getCardTypePosition()
	{
		return this.cardType.transform.position;
	}
	public Vector3 getLifePosition()
	{
		return this.life.transform.FindChild("Text").position;
	}
	#endregion
}