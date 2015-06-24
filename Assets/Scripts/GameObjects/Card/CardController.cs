﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

public class CardController : GameObjectController {


	public Card card;
	public CardRessources ressources;

	private CardView view;
	private MarketFeaturesView marketFeaturesView;
	private FocusMarketFeaturesView focusMarketFeaturesView;
	private BuyCardPopUpView buyPopUpView;
	private SellCardPopUpView sellPopUpView;
	private BuyXpCardPopUpView buyXpPopUpView;
	private RenameCardPopUpView renamePopUpView;
	private PutOnMarketCardPopUpView putOnMarketPopUpView;
	private EditSellCardPopUpView editSellPopUpView;
	private EditSellPriceCardPopUpView editSellPricePopUpView;
	private ErrorCardPopUpView errorPopUpView;
	private CardCollectionPointsPopUpView cardCollectionPointsPopUpView;
	private CardNewSkillsPopUpView newSkillsPopUpView;
	private CardNewCardTypePopUpView newCardTypePopUpView;
	private DeckOrderFeaturesView deckOrderFeaturesView;

	private IList<GameObject> skills;
	private GameObject experience;
	private GameObject cardUpgrade;
	

	void Awake () 
	{	
		this.view = gameObject.AddComponent <CardView>();
		this.ressources = gameObject.GetComponent<CardRessources> ();
		this.skills=new List<GameObject>();
	}
	public void setGameObject(string name, Vector3 scale, Vector3 position)
	{
		gameObject.name = name;
		gameObject.transform.localScale = scale;
		gameObject.transform.position = position;
	}
	public void setGameObjectScaleAndPosition(Vector3 scale, Vector3 position)
	{
		gameObject.transform.localScale = scale;
		gameObject.transform.position = position;
	}
	public void setGameObjectName(string name)
	{
		gameObject.name = name;
	}
	public void setGameObjectRotation(Quaternion rotation)
	{
		gameObject.transform.rotation = rotation;
	}
	public void setCard(Card c)
	{
		this.card = c;
		view.cardVM.attackArea = ressources.Areas [0];
		view.cardVM.speedArea = ressources.Areas [1];
		view.cardVM.moveArea = ressources.Areas [2];
		view.cardVM.cardFaces = new Texture[]{ressources.frontFaces [c.ArtIndex],
										      ressources.backFaces [0],
											  ressources.leftFaces [0],
											  ressources.rightFaces [0],
				                              ressources.topFaces [0],
											  ressources.bottomFaces [0]};
		view.cardVM.title = c.Title;
		view.cardVM.attack = c.Attack;
		view.cardVM.life = c.Life;
		view.cardVM.move = c.Move;
		view.cardVM.speed = c.Speed;
		view.cardVM.titleClass = c.TitleClass;
		for (int i = 0 ; i < 6 ; i++)
		{		
			view.cardVM.lifeLevel[i]=ressources.metals[c.LifeLevel];
			view.cardVM.attackLevel[i]=ressources.metals[c.AttackLevel];
			view.cardVM.speedLevel[i]=ressources.metals[c.SpeedLevel];
			view.cardVM.moveLevel[i]=ressources.metals[c.MoveLevel];
		}
		this.skills = new List<GameObject> ();
		if(this.cardUpgrade!=null)
		{
			Destroy(this.cardUpgrade);
		}
	}
	public void setSkills()
	{
		for (int i = 0 ; i < this.card.Skills.Count ; i++) 
		{
			if (card.Skills[i].IsActivated == 1) 
			{
				this.skills.Add (Instantiate(ressources.skillAreaObject) as GameObject);
				this.skills[skills.Count-1].name="Skill"+(skills.Count);
				this.skills[skills.Count-1].transform.parent=gameObject.transform.Find("texturedGameCard");
				this.skills[skills.Count-1].transform.localPosition=new Vector3(0,-0.12f-0.08f*(skills.Count-1),-0.5f);
				this.skills[skills.Count-1].transform.localScale=new Vector3(0.9f, 0.07f, 20f);
				this.skills[skills.Count-1].GetComponent<SkillController>().setSkill(card.Skills[i]);
				this.skills[skills.Count-1].GetComponent<SkillController>().setSkillLevelMetals(ressources.metals[card.Skills[i].Level]);
				this.skills[skills.Count-1].GetComponent<SkillController>().setSkillPicto(ressources.skillsPictos[card.Skills[i].Id]);
			}
		}
	}
	public void setExperience()
	{
		this.experience=Instantiate(ressources.experienceAreaObject) as GameObject;
		this.experience.name = "experienceArea";
		this.experience.transform.parent=gameObject.transform.Find("texturedGameCard");
		this.experience.transform.localPosition=new Vector3(0f,0f,0f);
		this.experience.transform.localScale=new Vector3(1f, 1f, 1f);
		this.experience.GetComponent<ExperienceController> ().setXp (this.card.ExperienceLevel,this.card.PercentageToNextLevel);
	}
	public IEnumerator setCardUpgrade(int caracteristicUpgraded, int caracteristicIncrease)
	{
		if(this.cardUpgrade!=null)
		{
			Destroy(this.cardUpgrade);
		}
		this.cardUpgrade=Instantiate(ressources.cardUpgradeObject) as GameObject;
		this.cardUpgrade.name = "cardUpgrade";
		this.cardUpgrade.transform.parent=gameObject.transform.Find("texturedGameCard");
		this.cardUpgrade.GetComponent<CardUpgradeController> ().setCardUpgrade (this.getCardUpgradeRect (caracteristicUpgraded), caracteristicIncrease);
		yield return new WaitForSeconds(5);
		if(this.cardUpgrade!=null)
		{
			Destroy(this.cardUpgrade);
		}
		this.card.CaracteristicUpgraded = -1;
		this.card.CaracteristicIncrease = -1;
	}
	public Rect getCardUpgradeRect (int caracteristicUpgraded)
	{
		GameObject refObject = new GameObject ();
		switch(caracteristicUpgraded)
		{
		case 0:
			refObject = transform.Find("texturedGameCard").FindChild("PictoMetalLife").gameObject;
			break;
		case 1:
			refObject=transform.Find("texturedGameCard").FindChild("AttackArea").FindChild("PictoMetalAttack").gameObject;
			break;
		case 2:
			refObject=transform.Find("texturedGameCard").FindChild("MoveArea").FindChild("PictoMetalMove").gameObject;
			break;
		case 3:
			refObject=transform.Find("texturedGameCard").FindChild("SpeedArea").FindChild("PictoMetalSpeed").gameObject;
			break;
		case 4:
			refObject=transform.Find("texturedGameCard").FindChild("Skill1").FindChild ("PictoMetalSkill").gameObject;
			break;
		case 5:
			refObject=transform.Find("texturedGameCard").FindChild("Skill2").FindChild ("PictoMetalSkill").gameObject;
			break;
		case 6:
			refObject=transform.Find("texturedGameCard").FindChild("Skill3").FindChild ("PictoMetalSkill").gameObject;
			break;
		case 7:
			refObject=transform.Find("texturedGameCard").FindChild("Skill4").FindChild ("PictoMetalSkill").gameObject;
			break;
		}
		Vector2 refPosition = base.getGOScreenPosition(refObject);
		Vector2 refSize = base.getGOScreenSize(refObject);
		return new Rect (refPosition.x+refSize.x/2f,(float)Screen.height-refPosition.y-base.GOSize.y/20f,base.GOSize.x/5f,base.GOSize.y/10f);
	}
	public void setDeckOrderFeatures(int deckOrder)
	{
		this.card.deckOrder = deckOrder;
		this.deckOrderFeaturesView = gameObject.AddComponent <DeckOrderFeaturesView>();
		deckOrderFeaturesView.deckOrderFeaturesVM.deckOrderName = ressources.deckOrderNames [this.card.deckOrder];
		if(this.card.deckOrder>0)
		{
			deckOrderFeaturesView.deckOrderFeaturesVM.displayLeftArrow=true;
		}
		if(this.card.deckOrder<ApplicationModel.nbCardsByDeck-1)
		{
			deckOrderFeaturesView.deckOrderFeaturesVM.displayRightArrow=true;
		}
		deckOrderFeaturesView.deckOrderFeaturesVM.styles=new GUIStyle[ressources.deckOrderFeaturesStyles.Length];
		for(int i=0;i<ressources.deckOrderFeaturesStyles.Length;i++)
		{
			deckOrderFeaturesView.deckOrderFeaturesVM.styles[i]=ressources.deckOrderFeaturesStyles[i];
		}
		deckOrderFeaturesView.deckOrderFeaturesVM.initStyles();
		this.deckOrderFeaturesResize ();
	}
	public void deckOrderFeaturesResize()
	{
		deckOrderFeaturesView.deckOrderFeaturesVM.leftArrowRect = new Rect(base.GOPosition.x-base.GOSize.x/2f,(Screen.height-base.GOPosition.y)+base.GOSize.y/2f,1.47f*(base.GOSize.y/8f),base.GOSize.y/8f);
		deckOrderFeaturesView.deckOrderFeaturesVM.rightArrowRect = new Rect(base.GOPosition.x+base.GOSize.x/2f-1.47f*(base.GOSize.y/8f),(Screen.height-base.GOPosition.y)+base.GOSize.y/2f,1.47f*(base.GOSize.y/8f),base.GOSize.y/8f);
		deckOrderFeaturesView.deckOrderFeaturesVM.deckOrderNameRect = new Rect(base.GOPosition.x-base.GOSize.x/2f,(Screen.height-base.GOPosition.y)-base.GOSize.y/2f-base.GOSize.y/6f,base.GOSize.x,base.GOSize.y/6f);
		deckOrderFeaturesView.deckOrderFeaturesVM.resize(base.GOSize.y);
	}
	public virtual void updateExperience()
	{
		if(this.experience!=null)
		{
			Destroy (this.experience);
		}
		for(int i=0;i<this.skills.Count;i++)
		{
			Destroy (this.skills[i]);
		}
		this.setCard (this.card);
		this.setExperience ();
		this.setSkills ();
		if(this.card.GetNewSkill)
		{
			StartCoroutine(this.skills[skills.Count-1].GetComponent<SkillController>().setSkillAsNew());
			this.card.GetNewSkill=false;
		}
		if(this.card.CaracteristicUpgraded>-1&&this.card.CaracteristicIncrease>0)
		{
			StartCoroutine(this.setCardUpgrade(this.card.CaracteristicUpgraded,this.card.CaracteristicIncrease));
		}
		this.show ();
		this.setMyGUI (true);
	}
	public virtual void updateCardXpLevel()
	{
	}
	public void animateExperience(Card c)
	{
		this.card = new Card();
		this.card = c;
		if(this.experience!=null)
		{
			this.experience.GetComponent<ExperienceController> ().animateXp (this.card.ExperienceLevel,this.card.PercentageToNextLevel);
		}
		this.setMyGUI (false);
	}
	public virtual void eraseCard()
	{
		this.card = new Card ();
		for(int i=0;i<this.skills.Count;i++)
		{
			Destroy (this.skills[i]);
		}
		if(experience!=null)
		{
			Destroy (this.experience);
		}
		if(deckOrderFeaturesView!=null)
		{
			Destroy (this.deckOrderFeaturesView);
		}
	}
	public void applySoldTexture()
	{
		view.cardVM.cardFaces[0] = ressources.soldCardTexture;
	}
	public void setError()
	{
		this.displayErrorCardPopUp ();
	}
	public void displayBuyCardPopUp()
	{
		this.buyPopUpView = gameObject.AddComponent<BuyCardPopUpView> ();
		buyPopUpView.buyPopUpVM.price = this.card.Price;
		buyPopUpView.popUpVM.styles=new GUIStyle[ressources.popUpStyles.Length];
		for(int i=0;i<ressources.popUpStyles.Length;i++)
		{
			buyPopUpView.popUpVM.styles[i]=ressources.popUpStyles[i];
		}
		buyPopUpView.popUpVM.initStyles();
		this.buyCardPopUpResize ();
		this.setGUI (false);
		this.popUpDisplayed (true);
	}
	public void displaySellCardPopUp()
	{
		this.sellPopUpView = gameObject.AddComponent<SellCardPopUpView> ();
		sellPopUpView.sellPopUpVM.price = this.card.destructionPrice;
		sellPopUpView.popUpVM.styles=new GUIStyle[ressources.popUpStyles.Length];
		for(int i=0;i<ressources.popUpStyles.Length;i++)
		{
			sellPopUpView.popUpVM.styles[i]=ressources.popUpStyles[i];
		}
		sellPopUpView.popUpVM.initStyles();
		this.sellCardPopUpResize ();
		this.setGUI (false);
		this.popUpDisplayed (true);
	}
	public void displayBuyXpCardPopUp()
	{
		this.buyXpPopUpView = gameObject.AddComponent<BuyXpCardPopUpView> ();
		buyXpPopUpView.buyXpPopUpVM.price = this.card.NextLevelPrice;
		buyXpPopUpView.popUpVM.styles=new GUIStyle[ressources.popUpStyles.Length];
		for(int i=0;i<ressources.popUpStyles.Length;i++)
		{
			buyXpPopUpView.popUpVM.styles[i]=ressources.popUpStyles[i];
		}
		buyXpPopUpView.popUpVM.initStyles();
		this.buyXpCardPopUpResize ();
		this.setGUI (false);
		this.popUpDisplayed (true);
	}
	public void displayRenameCardPopUp()
	{
		this.renamePopUpView = gameObject.AddComponent<RenameCardPopUpView> ();
		renamePopUpView.renamePopUpVM.price = this.card.RenameCost;
		renamePopUpView.renamePopUpVM.newTitle = "";
		renamePopUpView.popUpVM.styles=new GUIStyle[ressources.popUpStyles.Length];
		for(int i=0;i<ressources.popUpStyles.Length;i++)
		{
			renamePopUpView.popUpVM.styles[i]=ressources.popUpStyles[i];
		}
		renamePopUpView.popUpVM.initStyles();
		this.renameCardPopUpResize ();
		this.setGUI (false);
		this.popUpDisplayed (true);
	}
	public void displayputOnMarketCardPopUp()
	{
		this.putOnMarketPopUpView = gameObject.AddComponent<PutOnMarketCardPopUpView> ();
		putOnMarketPopUpView.putOnMarketPopUpVM.price = "";
		putOnMarketPopUpView.popUpVM.styles=new GUIStyle[ressources.popUpStyles.Length];
		for(int i=0;i<ressources.popUpStyles.Length;i++)
		{
			putOnMarketPopUpView.popUpVM.styles[i]=ressources.popUpStyles[i];
		}
		putOnMarketPopUpView.popUpVM.initStyles();
		this.putOnMarketCardPopUpResize ();
		this.setGUI (false);
		this.popUpDisplayed (true);
	}
	public void displayErrorCardPopUp()
	{
		this.errorPopUpView = gameObject.AddComponent<ErrorCardPopUpView> ();
		errorPopUpView.errorPopUpVM.error = this.card.Error;
		this.card.Error = "";
		errorPopUpView.popUpVM.styles=new GUIStyle[ressources.popUpStyles.Length];
		for(int i=0;i<ressources.popUpStyles.Length;i++)
		{
			errorPopUpView.popUpVM.styles[i]=ressources.popUpStyles[i];
		}
		errorPopUpView.popUpVM.initStyles();
		this.errorCardPopUpResize ();
	}
	public void displayEditSellCardPopUp()
	{
		this.editSellPopUpView = gameObject.AddComponent<EditSellCardPopUpView> ();
		editSellPopUpView.editSellPopUpVM.price = this.card.Price;
		editSellPopUpView.popUpVM.styles=new GUIStyle[ressources.popUpStyles.Length];
		for(int i=0;i<ressources.popUpStyles.Length;i++)
		{
			editSellPopUpView.popUpVM.styles[i]=ressources.popUpStyles[i];
		}
		editSellPopUpView.popUpVM.initStyles();
		this.editSellCardPopUpResize ();
		this.setGUI (false);
		this.popUpDisplayed (true);
	}
	public void displayEditSellPriceCardPopUp()
	{
		if(this.editSellPopUpView!=null)
		{
			this.hideEditSellCardPopUp();
		}
		this.editSellPricePopUpView = gameObject.AddComponent<EditSellPriceCardPopUpView> ();
		editSellPricePopUpView.editSellPricePopUpVM.price = this.card.Price.ToString();
		editSellPricePopUpView.popUpVM.styles=new GUIStyle[ressources.popUpStyles.Length];
		for(int i=0;i<ressources.popUpStyles.Length;i++)
		{
			editSellPricePopUpView.popUpVM.styles[i]=ressources.popUpStyles[i];
		}
		editSellPricePopUpView.popUpVM.initStyles();
		this.editSellPriceCardPopUpResize ();
		this.setGUI (false);
		this.popUpDisplayed (true);
	}
	public IEnumerator displayCollectionPointsPopUp()
	{
		if(this.cardCollectionPointsPopUpView!=null)
		{
			this.hideCollectionPointsPopUp();
		}
		cardCollectionPointsPopUpView = gameObject.AddComponent<CardCollectionPointsPopUpView>();
		cardCollectionPointsPopUpView.cardCollectionPointsPopUpVM.centralWindow = this.getCollectionPointsWindowsRect ();
		cardCollectionPointsPopUpView.cardCollectionPointsPopUpVM.collectionPoints = this.card.CollectionPoints;
		cardCollectionPointsPopUpView.cardCollectionPointsPopUpVM.styles=new GUIStyle[ressources.collectionPopUpStyles.Length];
		for(int i=0;i<ressources.collectionPopUpStyles.Length;i++)
		{
			cardCollectionPointsPopUpView.cardCollectionPointsPopUpVM.styles[i]=ressources.collectionPopUpStyles[i];
		}
		cardCollectionPointsPopUpView.cardCollectionPointsPopUpVM.initStyles();
		this.collectionPointsPopUpResize ();
		yield return new WaitForSeconds (5);
		this.hideCollectionPointsPopUp ();
	}
	public IEnumerator displayNewSkillsPopUp()
	{
		if(this.newSkillsPopUpView!=null)
		{
			this.hideNewSkillsPopUp();
		}
		this.newSkillsPopUpView = gameObject.AddComponent<CardNewSkillsPopUpView>();
		for(int i=0;i<this.card.NewSkills.Count;i++)
		{
			newSkillsPopUpView.cardNewSkillsPopUpVM.skills.Add (this.card.NewSkills[i].Name);
		}
		if(this.card.NewSkills.Count>1)
		{
			newSkillsPopUpView.cardNewSkillsPopUpVM.title="Nouvelles compétences :";
		}
		else if(this.card.NewSkills.Count==1)
		{
			newSkillsPopUpView.cardNewSkillsPopUpVM.title="Nouvelle compétence :";
		}
		newSkillsPopUpView.cardNewSkillsPopUpVM.styles=new GUIStyle[ressources.collectionPopUpStyles.Length];
		for(int i=0;i<ressources.collectionPopUpStyles.Length;i++)
		{
			newSkillsPopUpView.cardNewSkillsPopUpVM.styles[i]=ressources.collectionPopUpStyles[i];
		}
		newSkillsPopUpView.cardNewSkillsPopUpVM.initStyles();
		this.newSkillsPopUpResize ();
		yield return new WaitForSeconds (5);
		this.hideNewSkillsPopUp ();
	}
	public void displayNewCardTypePopUp()
	{
		newCardTypePopUpView = gameObject.AddComponent<CardNewCardTypePopUpView>();
		newCardTypePopUpView.cardNewCardTypePopUpVM.centralWindow = this.getNewCardTypeWindowsRect ();
		newCardTypePopUpView.cardNewCardTypePopUpVM.newCardType = this.card.TitleCardTypeUnlocked;
		newCardTypePopUpView.cardNewCardTypePopUpVM.styles=new GUIStyle[ressources.collectionPopUpStyles.Length];
		for(int i=0;i<ressources.collectionPopUpStyles.Length;i++)
		{
			newCardTypePopUpView.cardNewCardTypePopUpVM.styles[i]=ressources.collectionPopUpStyles[i];
		}
		newCardTypePopUpView.cardNewCardTypePopUpVM.initStyles();
		this.newCardTypePopUpResize ();
		this.setGUI (false);
		this.popUpDisplayed (true);
	}
	public virtual void changeDeckOrder(bool moveLeft)
	{
	}
	public virtual void buyCard()
	{
		if(this.buyPopUpView!=null)
		{
			Destroy (buyPopUpView);
			this.popUpDisplayed (false);
		}
	}
	public virtual void sellCard()
	{
		if(this.sellPopUpView!=null)
		{
			Destroy (this.sellPopUpView);
			this.popUpDisplayed (false);
		}
	}
	public virtual void buyXpCard()
	{
		if(this.buyXpPopUpView!=null)
		{
			Destroy (this.buyXpPopUpView);
			this.popUpDisplayed (false);
		}
	}
	public virtual void renameCard()
	{
		if(this.renamePopUpView!=null)
		{
			Destroy (this.renamePopUpView);
			this.popUpDisplayed (false);
		}
	}
	public virtual void putOnMarketCard()
	{
		if(this.putOnMarketPopUpView!=null)
		{
			Destroy (putOnMarketPopUpView);
			this.popUpDisplayed (false);
		}
	}
	public virtual void editSellPriceCard()
	{
		if(this.editSellPricePopUpView!=null)
		{
			Destroy (this.editSellPricePopUpView);
			this.popUpDisplayed (false);
		}
	}
	public virtual void unsellCard()
	{
		if(this.editSellPopUpView!=null)
		{
			Destroy(this.editSellPopUpView);
			this.popUpDisplayed (false);
		}
	}
	public void hideBuyCardPopUp()
	{
		Destroy (this.buyPopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void hideSellCardPopUp()
	{
		Destroy (this.sellPopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void hideBuyXpCardPopUp()
	{
		Destroy (this.buyXpPopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void hideRenameCardPopUp()
	{
		Destroy (this.renamePopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void hideputOnMarketCardPopUp()
	{
		Destroy (this.putOnMarketPopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void hideEditSellCardPopUp()
	{
		Destroy (this.editSellPopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void hideEditSellPriceCardPopUp()
	{
		Destroy (this.editSellPricePopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void hideErrorCardPopUp()
	{
		Destroy (this.errorPopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void hideCollectionPointsPopUp()
	{
		Destroy (this.cardCollectionPointsPopUpView);
	}
	public void hideNewSkillsPopUp()
	{
		Destroy (this.newSkillsPopUpView);
	}
	public void hideNewCardTypePopUp()
	{
		Destroy (this.newCardTypePopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	private void buyCardPopUpResize()
	{
		buyPopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		buyPopUpView.popUpVM.resize ();
	}
	private void sellCardPopUpResize()
	{
		sellPopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		sellPopUpView.popUpVM.resize ();
	}
	private void buyXpCardPopUpResize()
	{
		buyXpPopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		buyXpPopUpView.popUpVM.resize ();
	}
	private void putOnMarketCardPopUpResize()
	{
		putOnMarketPopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		putOnMarketPopUpView.popUpVM.resize ();
	}
	private void renameCardPopUpResize()
	{
		renamePopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		renamePopUpView.popUpVM.resize ();
	}
	private void editSellCardPopUpResize()
	{
		editSellPopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		editSellPopUpView.popUpVM.resize ();
	}
	private void errorCardPopUpResize()
	{
		errorPopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		errorPopUpView.popUpVM.resize ();
	}
	private void editSellPriceCardPopUpResize()
	{
		editSellPricePopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		editSellPricePopUpView.popUpVM.resize ();
	}
	private void collectionPointsPopUpResize()
	{
		cardCollectionPointsPopUpView.cardCollectionPointsPopUpVM.centralWindow = this.getCollectionPointsWindowsRect ();
		cardCollectionPointsPopUpView.cardCollectionPointsPopUpVM.resize ();
	}
	private void newSkillsPopUpResize()
	{
		newSkillsPopUpView.cardNewSkillsPopUpVM.centralWindow = this.getNewSkillsWindowsRect ();
		newSkillsPopUpView.cardNewSkillsPopUpVM.resize ();
	}
	private void newCardTypePopUpResize()
	{
		newCardTypePopUpView.cardNewCardTypePopUpVM.centralWindow = this.getNewCardTypeWindowsRect ();
		newCardTypePopUpView.cardNewCardTypePopUpVM.resize ();
	}
	public int editSellPriceSyntaxCheck()
	{
		int n;
		bool isNumeric = int.TryParse(editSellPricePopUpView.editSellPricePopUpVM.price, out n);
		if(editSellPricePopUpView.editSellPricePopUpVM.price!="" && isNumeric)
		{
			if(System.Convert.ToInt32(editSellPricePopUpView.editSellPricePopUpVM.price)>0)
			{
				return System.Convert.ToInt32(editSellPricePopUpView.editSellPricePopUpVM.price);
			}
		}
		editSellPricePopUpView.editSellPricePopUpVM.error="Merci de bien vouloir saisir un prix";
		return -1;
	}
	public string renameCardSyntaxCheck()
	{
		string tempString = renamePopUpView.renamePopUpVM.newTitle;
		if(tempString=="")
		{
			renamePopUpView.renamePopUpVM.error="Merci de bien vouloir saisir un nom";
			return "";
		}
		else if(tempString==this.card.Title)
		{
			renamePopUpView.renamePopUpVM.error="Le nom saisi est identique à l'ancien";
			return "";
		}
		else if(tempString.Length<4 )
		{
			renamePopUpView.renamePopUpVM.error= "Le nom doit au moins comporter 4 caractères";
			return "";
		}
		else if(tempString.Length>14 )
		{
			renamePopUpView.renamePopUpVM.error="Le nom doit faire moins de 12 caractères";
			return "";
		}
		else if(!Regex.IsMatch(tempString, @"^[a-zA-Z0-9_]+$"))
		{
			renamePopUpView.renamePopUpVM.error="Vous ne pouvez pas utiliser de caractères spéciaux";
			return "";
		}
		return tempString;
	}
	public int putOnMarketSyntaxCheck()
	{
		int n;
		bool isNumeric = int.TryParse(putOnMarketPopUpView.putOnMarketPopUpVM.price, out n);
		if(putOnMarketPopUpView.putOnMarketPopUpVM.price!="" && isNumeric)
		{
			if(System.Convert.ToInt32(putOnMarketPopUpView.putOnMarketPopUpVM.price)>0)
			{ 
				return System.Convert.ToInt32(putOnMarketPopUpView.putOnMarketPopUpVM.price);
			}
		}
		putOnMarketPopUpView.putOnMarketPopUpVM.error="Merci de bien vouloir saisir un prix";
		return -1;
	}
	public virtual void setGUI(bool value)
	{
	}
	public virtual void setMyGUI(bool value)
	{
		if(deckOrderFeaturesView!=null)
		{
			deckOrderFeaturesView.deckOrderFeaturesVM.guiEnabled=value;
		}
	}
	public virtual void setButtonsGui(bool value)
	{
		if(this.deckOrderFeaturesView!=null)
		{
			deckOrderFeaturesView.deckOrderFeaturesVM.buttonsEnabled=value;
		}
	}
	public virtual void popUpDisplayed(bool value)
	{
	}
	public virtual void hideFeatures()
	{
	}
	public virtual void hideCard()
	{
	}
	public virtual void exitFocus()
	{
	}
	public virtual void refreshCredits()
	{
	}
	public virtual void updateSceneModel()
	{
	}
	public void confirmPopUp()
	{
		if(this.buyPopUpView!=null)
		{
			this.buyCard();
		}
		if(this.renamePopUpView!=null)
		{
			this.renameCard();
		}
		if(this.sellPopUpView!=null)
		{
			this.sellCard();
		}
		if(this.editSellPricePopUpView!=null)
		{
			this.editSellPriceCard();
		}
		if(this.buyXpPopUpView!=null)
		{
			this.buyXpCard();
		}
		if(this.putOnMarketPopUpView!=null)
		{
			this.putOnMarketCard();
		}
	}
	public void exitPopUp()
	{
		if(this.buyPopUpView!=null)
		{
			this.hideBuyCardPopUp();
		}
		if(this.renamePopUpView!=null)
		{
			this.hideRenameCardPopUp();
		}
		if(this.sellPopUpView!=null)
		{
			this.hideSellCardPopUp();
		}
		if(this.editSellPopUpView!=null)
		{
			this.hideEditSellCardPopUp();
		}
		if(this.editSellPricePopUpView!=null)
		{
			this.hideEditSellPriceCardPopUp();
		}
		if(this.buyXpPopUpView!=null)
		{
			this.hideBuyXpCardPopUp();
		}
		if(this.putOnMarketPopUpView!=null)
		{
			this.hideputOnMarketCardPopUp();
		}
	}
	public void show()
	{
		base.setGOCoordinates (gameObject.transform.Find("texturedGameCard").gameObject);
		this.setTextResolution ();
		view.show ();
		for (int i=0;i<skills.Count;i++)
		{
			this.skills[i].GetComponent<SkillController>().show();
		}
		if(this.experience!=null)
		{
			this.experience.GetComponent<ExperienceController> ().show ();
		}
	}
	public void setTextResolution()
	{
		float resolution = base.GOSize.y / 150f;
		view.setTextResolution (resolution);
		for (int i=0;i<skills.Count;i++)
		{
			this.skills[i].GetComponent<SkillController>().setTextResolution(resolution);
		}
		if(this.experience!=null)
		{
			this.experience.GetComponent<ExperienceController> ().setTextResolution (resolution);
		}
	}
	public virtual void resize()
	{
		base.setGOCoordinates (gameObject.transform.Find("texturedGameCard").gameObject);
		this.setTextResolution ();
		for (int i=0;i<skills.Count;i++)
		{
			this.skills[i].GetComponent<SkillController>().resize();
		}
		if(this.cardUpgrade!=null)
		{
			this.cardUpgrade.GetComponent<CardUpgradeController>().resize(getCardUpgradeRect(this.card.CaracteristicUpgraded));
		}
		if(this.buyPopUpView!=null)
		{
			this.buyCardPopUpResize();
		}
		if(this.renamePopUpView!=null)
		{
			this.renameCardPopUpResize();
		}
		if(this.sellPopUpView!=null)
		{
			this.sellCardPopUpResize();
		}
		if(this.editSellPopUpView!=null)
		{
			this.editSellCardPopUpResize();
		}
		if(this.editSellPricePopUpView!=null)
		{
			this.editSellPriceCardPopUpResize();
		}
		if(this.buyXpPopUpView!=null)
		{
			this.buyXpCardPopUpResize();
		}
		if(this.putOnMarketPopUpView!=null)
		{
			this.putOnMarketCardPopUpResize();
		}
		if(this.errorPopUpView!=null)
		{
			this.errorCardPopUpResize();
		}
		if(this.cardCollectionPointsPopUpView!=null)
		{
			this.collectionPointsPopUpResize();
		}
		if(this.newSkillsPopUpView!=null)
		{
			this.newSkillsPopUpResize();
		}
		if(this.newCardTypePopUpView!=null)
		{
			this.newCardTypePopUpResize();
		}
	}
	public void setCentralWindowRect(Rect centralWindowRect)
	{
		view.cardVM.centralWindowsRect = centralWindowRect;
	}
	public void setCollectionPointsWindowRect(Rect collectionPointsWindowRect)
	{
		view.cardVM.collectionPointsWindowsRect = collectionPointsWindowRect;
	}
	public void setNewSkillsWindowRect(Rect newSkillsWindowRect)
	{
		view.cardVM.newSkillsWindowsRect = newSkillsWindowRect;
	}
	public void setNewCardTypeWindowRect(Rect newCardTypeWindowRect)
	{
		view.cardVM.newCardTypeWindowsRect = newCardTypeWindowRect;
	}
	public Rect getCentralWindowsRect()
	{
		return view.cardVM.centralWindowsRect;
	}
	public Rect getCollectionPointsWindowsRect()
	{
		return view.cardVM.collectionPointsWindowsRect;
	}
	public Rect getNewSkillsWindowsRect()
	{
		return view.cardVM.newSkillsWindowsRect;
	}
	public Rect getNewCardTypeWindowsRect()
	{
		return view.cardVM.newCardTypeWindowsRect;
	}
	public Rect getCardFeaturesFocusRect(int position)
	{
		Rect cardFeaturesRect = new Rect(base.GOPosition.x+base.GOSize.x/2f,
		                                 (Screen.height-base.GOPosition.y)-base.GOSize.y/2f+position*(base.GOSize.y/6f),
		                                 base.GOSize.x/2f,
		                                 base.GOSize.y/6f);
		return cardFeaturesRect;
	}
}

