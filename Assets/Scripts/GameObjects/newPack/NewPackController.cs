using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewPackController : MonoBehaviour 
{

	private NewPackRessources ressources;

	public Pack p;

	public int Id;
	private bool isClickable;

	private Rect centralWindow;
	private Rect collectionPointsWindow;
	private Rect newSkillsWindow;

	private NewPackErrorPopUpView errorView;
	private bool isErrorViewDisplayed;
	private NewCollectionPointsPopUpView collectionPointsView;
	private bool isCollectionPointsViewDisplayed;
	private NewSkillsPopUpView newSkillsView;
	private bool isNewSkillsViewDisplayed;
	private NewPackSelectCardTypePopUpView selectCardTypeView;
	private bool isSelectCardTypeViewDisplayed;
	private float speed;
	private float timerCollectionPoints;


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
	}
	void Awake()
	{
		this.ressources = this.gameObject.GetComponent<NewPackRessources> ();
		this.isClickable = true;
		this.speed = 5f;
	}
	public void setClickable(bool value)
	{
		this.isClickable = value;
		if(!value)
		{
			gameObject.transform.FindChild("BuyButton").GetComponent<SpriteRenderer>().color=new Color(166f/255f,31f/255f,28f/255f);
			gameObject.transform.FindChild("BuyButton").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(166f/255f,31f/255f,28f/255f);
			gameObject.transform.FindChild("BuyButton").FindChild("Cristal").GetComponent<SpriteRenderer>().color=new Color(166f/255f,31f/255f,28f/255f);
		}
		else
		{
			gameObject.transform.FindChild("BuyButton").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("BuyButton").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("BuyButton").FindChild("Cristal").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseOver()
	{
		if(isClickable)
		{
			gameObject.transform.FindChild("PackTitle").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("BuyButton").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
			//gameObject.transform.FindChild("PackBorder").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("PackPicture").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("BuyButton").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
		}
	}
	void OnMouseExit()
	{
		if(isClickable)
		{
			gameObject.transform.FindChild("PackTitle").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("BuyButton").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			//gameObject.transform.FindChild("PackBorder").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("PackPicture").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("BuyButton").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		}
	}
	public virtual void OnMouseDown()
	{
		if(isClickable)
		{
			if(this.p.CardType==-2)
			{
				this.displaySelectCardTypePopUp();
			}
			else
			{
				this.buyPackHandler();
			}
		}
	}
	private void buyPackHandler()
	{
		StartCoroutine (this.buyPack ());
	}
	private IEnumerator buyPack(int cardType=-1)
	{
		yield return StartCoroutine (this.p.buyPack (cardType));
		this.refreshCredits ();
		if(isSelectCardTypeViewDisplayed)
		{
			this.hideSelectCardPopUp();
		}
		if(this.p.Error=="")
		{
			NewStoreController.instance.drawRandomCards(this.Id);
			if(this.p.CollectionPoints>0)
			{
				this.displayCollectionPointsPopUp();
			}
			if(this.p.NewSkills.Count>0)
			{
				this.displayNewSkillsPopUp();
			}
		}
		else
		{
			this.displayErrorPopUp();
		}
	}
	public virtual void show()
	{
		gameObject.transform.FindChild("PackTitle").GetComponent<TextMeshPro>().text=p.Name;
		gameObject.transform.FindChild("BuyButton").FindChild("Title").GetComponent<TextMeshPro>().text=p.Price.ToString();
		gameObject.transform.FindChild ("PackPicture").GetComponent<SpriteRenderer> ().sprite = p.texture;
	}
	public void displayPack(bool value)
	{
		gameObject.transform.FindChild ("PackTitle").gameObject.SetActive (value);
		gameObject.transform.FindChild ("BuyButton").gameObject.SetActive (value);
		gameObject.transform.FindChild ("PackPicture").gameObject.SetActive (value);
		//gameObject.transform.FindChild ("PackBorder").gameObject.SetActive (value);
	}
	public void setPackPicture(Sprite picture)
	{
		gameObject.transform.FindChild ("PackPicture").GetComponent<SpriteRenderer> ().sprite = picture;
	}
	public void setId(int Id)
	{
		this.Id = Id;
	}
	public void setCentralWindow(Rect centralWindow)
	{
		this.centralWindow = centralWindow;
	}
	public void setCollectionPointsWindow(Rect collectionPointsWindow)
	{
		this.collectionPointsWindow = collectionPointsWindow;
	}
	public void setNewSkillsWindow(Rect newSkillsWindow)
	{
		this.newSkillsWindow = newSkillsWindow;
	}
	public void displayErrorPopUp()
	{
		this.errorView = gameObject.AddComponent<NewPackErrorPopUpView> ();
		this.isErrorViewDisplayed = true;
		errorView.errorPopUpVM.error = this.p.Error;
		this.p.Error = "";
		errorView.popUpVM.centralWindowStyle = new GUIStyle(ressources.popUpSkin.customStyles[3]);
		errorView.popUpVM.centralWindowTitleStyle = new GUIStyle (ressources.popUpSkin.customStyles [0]);
		errorView.popUpVM.centralWindowButtonStyle = new GUIStyle (ressources.popUpSkin.button);
		errorView.popUpVM.transparentStyle = new GUIStyle (ressources.popUpSkin.customStyles [2]);
		this.errorPopUpResize ();
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
		collectionPointsView.cardCollectionPointsPopUpVM.collectionPoints = this.p.CollectionPoints;
		collectionPointsView.cardCollectionPointsPopUpVM.collectionPointsRanking = this.p.CollectionPointsRanking;
		collectionPointsView.popUpVM.centralWindowStyle = new GUIStyle(ressources.popUpSkin.window);
		collectionPointsView.popUpVM.centralWindowTitleStyle = new GUIStyle (ressources.popUpSkin.customStyles [0]);
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
		for(int i=0;i<this.p.NewSkills.Count;i++)
		{
			newSkillsView.cardNewSkillsPopUpVM.skills.Add (this.p.NewSkills[i].Name);
		}
		if(this.p.NewSkills.Count>1)
		{
			newSkillsView.cardNewSkillsPopUpVM.title="Nouvelles compétences :";
		}
		else if(this.p.NewSkills.Count==1)
		{
			newSkillsView.cardNewSkillsPopUpVM.title="Nouvelle compétence :";
		}
		newSkillsView.popUpVM.centralWindowStyle = new GUIStyle(ressources.popUpSkin.window);
		newSkillsView.popUpVM.centralWindowTitleStyle = new GUIStyle (ressources.popUpSkin.customStyles [0]);
		this.newSkillsPopUpResize ();
	}
	private void errorPopUpResize()
	{
		errorView.popUpVM.centralWindow = this.centralWindow;
		errorView.popUpVM.resize ();
	}
	private void collectionPointsPopUpResize()
	{
		collectionPointsView.popUpVM.centralWindow = this.collectionPointsWindow;
		collectionPointsView.popUpVM.resize ();
	}
	private void newSkillsPopUpResize()
	{
		newSkillsView.popUpVM.centralWindow = this.newSkillsWindow;
		newSkillsView.popUpVM.resize ();
	}
	public void hideErrorPopUp()
	{
		Destroy (this.errorView);
		this.isErrorViewDisplayed = false;
	}
	public void hideCollectionPointsPopUp()
	{
		Destroy (this.collectionPointsView);
		this.isCollectionPointsViewDisplayed = false;
	}
	public void hideNewSkillsPopUp()
	{
		Destroy (this.newSkillsView);
		this.isNewSkillsViewDisplayed = false;
	}
	public void displaySelectCardTypePopUp()
	{
		this.selectCardTypeView = gameObject.AddComponent<NewPackSelectCardTypePopUpView> ();
		this.isSelectCardTypeViewDisplayed = true;
		List<string> cardTypesAllowed = this.getCardTypesAllowed ();
		selectCardTypeView.selectCardTypePopUpVM.cardTypes=new string[cardTypesAllowed.Count];
		for(int i =0;i<cardTypesAllowed.Count;i++)
		{
			selectCardTypeView.selectCardTypePopUpVM.cardTypes[i]=cardTypesAllowed[i];
		}
		selectCardTypeView.popUpVM.centralWindowStyle = new GUIStyle(ressources.popUpSkin.window);
		selectCardTypeView.popUpVM.centralWindowTitleStyle = new GUIStyle (ressources.popUpSkin.customStyles [0]);
		selectCardTypeView.popUpVM.centralWindowButtonStyle = new GUIStyle (ressources.popUpSkin.button);
		selectCardTypeView.popUpVM.centralWindowSelGridStyle = new GUIStyle (ressources.popUpSkin.toggle);
		selectCardTypeView.popUpVM.transparentStyle = new GUIStyle (ressources.popUpSkin.customStyles [2]);
		selectCardTypeView.popUpVM.centralWindowErrorStyle = new GUIStyle (ressources.popUpSkin.customStyles [1]);
		this.selectCardPopUpResize ();
	}
	public virtual List<string> getCardTypesAllowed()
	{
		return new List<string> ();
	}
	private void selectCardPopUpResize()
	{
		selectCardTypeView.popUpVM.centralWindow = this.centralWindow;
		selectCardTypeView.popUpVM.resize ();
	}
	public void hideSelectCardPopUp()
	{
		Destroy (this.selectCardTypeView);
		this.isSelectCardTypeViewDisplayed = false;
	}
	private bool isCardTypeSelected()
	{
		if(selectCardTypeView.selectCardTypePopUpVM.cardTypeSelected!=-1)
		{
			return true;
		}
		else
		{
			selectCardTypeView.selectCardTypePopUpVM.error="Veuillez sélectionner une classe";
			return false;
		}
	}
	public void buyPackWidthCardTypeHandler()
	{
		if(isCardTypeSelected())
		{
			selectCardTypeView.selectCardTypePopUpVM.guiEnabled=false;
			int cardType;
			cardType=this.getCardTypeId(selectCardTypeView.selectCardTypePopUpVM.cardTypeSelected);
			StartCoroutine(this.buyPack(cardType));	
		}
	}
	public virtual int getCardTypeId(int id)
	{
		return -1;
	}
	public virtual void refreshCredits()
	{
	}
}

