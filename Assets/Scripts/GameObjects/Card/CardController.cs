using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CardController : GameObjectController {


	public Card card;
	public CardRessources ressources;

	private CardView view;
	private MarketFeaturesView marketFeaturesView;
	private FocusMarketFeaturesView focusMarketFeaturesView;
	private BuyCardPopUpView buyPopUpView;
	private DeleteCardPopUpView deletePopUpView;
	private BuyXpCardPopUpView buyXpPopUpView;
	private RenameCardPopUpView renamePopUpView;
	private SellCardPopUpView sellPopUpView;
	private EditSellCardPopUpView editSellPopUpView;
	private EditSellPriceCardPopUpView editSellPricePopUpView;
	private ErrorCardPopUpView errorPopUpView;

	private IList<GameObject> skills;
	private GameObject experience;


	void Awake () 
	{	
		this.view = gameObject.AddComponent <CardView>();
		this.ressources = gameObject.GetComponent<CardRessources> ();
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
	}
	public void setSkills()
	{
		for (int i = 0 ; i < this.card.Skills.Count ; i++) 
		{
			if (card.Skills[i].IsActivated == 1) 
			{
				this.skills.Add (Instantiate(ressources.skillAreaObject) as GameObject);
				this.skills[skills.Count-1].name="Skill"+(i+1);
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
		this.updateExperience ();
	}
	public void updateExperience()
	{
		this.experience.GetComponent<ExperienceController> ().setXp (this.card.getXpLevel(),this.card.percentageToNextXpLevel());
	}
	public void animateExperience()
	{
		this.experience.GetComponent<ExperienceController> ().animateXp (this.card.getXpLevel(),this.card.percentageToNextXpLevel());
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
	public void hideBuyCardPopUp()
	{
		Destroy (this.buyPopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void buyCard()
	{
		if(this.buyPopUpView!=null)
		{
			this.hideBuyCardPopUp();
		}
		this.applySoldTexture ();
		this.hideFeatures ();
		//this.card.buyCard ();
	}
	public void applySoldTexture()
	{
		view.cardVM.soldCardTexture = ressources.soldCardTexture;
		view.applySoldTexture ();
	}
	public void buyCardPopUpResize()
	{
		buyPopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		buyPopUpView.popUpVM.resize ();
	}
	public void displayDeleteCardPopUp()
	{
		this.deletePopUpView = gameObject.AddComponent<DeleteCardPopUpView> ();
		deletePopUpView.deletePopUpVM.price = this.card.getCost();
		deletePopUpView.popUpVM.styles=new GUIStyle[ressources.popUpStyles.Length];
		for(int i=0;i<ressources.popUpStyles.Length;i++)
		{
			deletePopUpView.popUpVM.styles[i]=ressources.popUpStyles[i];
		}
		deletePopUpView.popUpVM.initStyles();
		this.deleteCardPopUpResize ();
		this.setGUI (false);
		this.popUpDisplayed (true);
	}
	public void hideDeleteCardPopUp()
	{
		Destroy (this.deletePopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void deleteCard()
	{
		if(this.deletePopUpView!=null)
		{
			this.hideDeleteCardPopUp();
		}
		//this.card.delete ();
		this.hideCard ();
	}
	public void deleteCardPopUpResize()
	{
		deletePopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		deletePopUpView.popUpVM.resize ();
	}
	public void displayBuyXpCardPopUp()
	{
		this.buyXpPopUpView = gameObject.AddComponent<BuyXpCardPopUpView> ();
		buyXpPopUpView.buyXpPopUpVM.price = this.card.getPriceForNextLevel();
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
	public void hideBuyXpCardPopUp()
	{
		Destroy (this.buyXpPopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void buyXpCard()
	{
		if(this.buyXpPopUpView!=null)
		{
			this.hideBuyXpCardPopUp();
		}
		//this.card.addXp(this.card.getPriceForNextLevel,this.card.getPriceForNextLevel);
		this.card.Experience = this.card.Experience + this.card.getPriceForNextLevel ();
		gameObject.GetComponent<CardController> ().animateExperience ();
		this.updateVM ();

	}
	public void buyXpCardPopUpResize()
	{
		buyXpPopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		buyXpPopUpView.popUpVM.resize ();
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
	public void hideRenameCardPopUp()
	{
		Destroy (this.renamePopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void renameCard()
	{
		//this.card.renameCard (renamePopUpView.renamePopUpVM.newTitle,this.card.RenameCost);
		if(this.renamePopUpView!=null)
		{
			this.hideRenameCardPopUp();
		}
		view.cardVM.title = this.card.Title;
		view.updateName ();
	}
	public void renameCardPopUpResize()
	{
		renamePopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		renamePopUpView.popUpVM.resize ();
	}
	public void displaySellCardPopUp()
	{
		this.sellPopUpView = gameObject.AddComponent<SellCardPopUpView> ();
		sellPopUpView.sellPopUpVM.price = "";
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
	public void hideSellCardPopUp()
	{
		Destroy (this.sellPopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void sellCard()
	{
		//this.card.toSell (System.Convert.ToInt32 (sellPopUpView.sellPopUpVM.price));
		if(this.sellPopUpView!=null)
		{
			this.hideSellCardPopUp();
		}
		this.updateVM ();
	}
	public void sellCardPopUpResize()
	{
		sellPopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		sellPopUpView.popUpVM.resize ();
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
	public void hideEditSellCardPopUp()
	{
		Destroy (this.editSellPopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void editSellCardPopUpResize()
	{
		editSellPopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		editSellPopUpView.popUpVM.resize ();
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
	public void hideEditSellPriceCardPopUp()
	{
		Destroy (this.editSellPricePopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void editSellPriceCardPopUpResize()
	{
		editSellPricePopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		editSellPricePopUpView.popUpVM.resize ();
	}
	public void editSellPrice()
	{
		//this.card.changePriceCard (System.Convert.ToInt32(editSellPricePopUpView.editSellPricePopUpVM.price));
		if(this.editSellPricePopUpView!=null)
		{
			this.hideEditSellPriceCardPopUp();
		}
		this.updateVM ();
	}
	public void unsellCard()
	{
		if(this.editSellPopUpView!=null)
		{
			this.hideEditSellCardPopUp();
		}
		this.card.notToSell ();
		this.updateVM ();
	}
	public void displayErrorCardPopUp()
	{
		this.errorPopUpView = gameObject.AddComponent<ErrorCardPopUpView> ();
		errorPopUpView.errorPopUpVM.error = this.card.Error;
		errorPopUpView.popUpVM.styles=new GUIStyle[ressources.popUpStyles.Length];
		for(int i=0;i<ressources.popUpStyles.Length;i++)
		{
			errorPopUpView.popUpVM.styles[i]=ressources.popUpStyles[i];
		}
		errorPopUpView.popUpVM.initStyles();
		this.errorCardPopUpResize ();
		this.setGUI (false);
		this.popUpDisplayed (true);
	}
	public void hideErrorPriceCardPopUp()
	{
		Destroy (this.errorPopUpView);
		this.setGUI (true);
		this.popUpDisplayed (false);
	}
	public void errorCardPopUpResize()
	{
		errorPopUpView.popUpVM.centralWindow = this.getCentralWindowsRect ();
		errorPopUpView.popUpVM.resize ();
	}
	public virtual void updateVM()
	{
	}
	public virtual void setGUI(bool value)
	{
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
	public void confirmPopUp()
	{
		if(this.buyPopUpView!=null)
		{
			this.buyCard();
		}
	}
	public void exitPopUp()
	{
		if(this.buyPopUpView!=null)
		{
			this.hideBuyCardPopUp();
		}
	}
	public void show()
	{
		base.getGOCoordinates (gameObject.transform.Find("texturedGameCard").gameObject);
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
		float resolution = base.GOSize.y / 200f;
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
		base.getGOCoordinates (gameObject.transform.Find("texturedGameCard").gameObject);
		this.setTextResolution ();
		for (int i=0;i<skills.Count;i++)
		{
			this.skills[i].GetComponent<SkillController>().resize();
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
		if(this.deletePopUpView!=null)
		{
			this.deleteCardPopUpResize();
		}
	}
	public void setCentralWindowRect(Rect centralWindowRect)
	{
		view.cardVM.centralWindowsRect = centralWindowRect;
	}
	public Rect getCentralWindowsRect()
	{
		return view.cardVM.centralWindowsRect;
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

