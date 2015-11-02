//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using TMPro;
//
//public class NewPackController : MonoBehaviour 
//{
//
//	private NewPackRessources ressources;
//
//	public Pack p;
//
//	public int Id;
//	private bool isClickable;
//
//	private Rect centralWindow;
//	private Rect collectionPointsWindow;
//	private Rect newSkillsWindow;
//
//
//
//
//
//	public virtual void Update ()
//	{
//		if(isCollectionPointsViewDisplayed)
//		{
//			timerCollectionPoints = timerCollectionPoints + speed * Time.deltaTime;
//			if(timerCollectionPoints>15f)
//			{
//				timerCollectionPoints=0f;
//				this.hideCollectionPointsPopUp();
//				if(isNewSkillsViewDisplayed)
//				{
//					this.hideNewSkillsPopUp();
//				}
//			}
//		}
//	}
//	void Awake()
//	{
//		this.ressources = this.gameObject.GetComponent<NewPackRessources> ();
//		this.isClickable = true;
//		this.speed = 5f;
//	}
//	public void setClickable(bool value)
//	{
//		this.isClickable = value;
//		if(!value)
//		{
//			gameObject.transform.FindChild("BuyButton").GetComponent<SpriteRenderer>().color=new Color(166f/255f,31f/255f,28f/255f);
//			gameObject.transform.FindChild("BuyButton").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(166f/255f,31f/255f,28f/255f);
//			gameObject.transform.FindChild("BuyButton").FindChild("Cristal").GetComponent<SpriteRenderer>().color=new Color(166f/255f,31f/255f,28f/255f);
//		}
//		else
//		{
//			gameObject.transform.FindChild("BuyButton").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
//			gameObject.transform.FindChild("BuyButton").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
//			gameObject.transform.FindChild("BuyButton").FindChild("Cristal").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
//		}
//	}
//	void OnMouseOver()
//	{
//		if(isClickable)
//		{
//			gameObject.transform.FindChild("PackTitle").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
//			gameObject.transform.FindChild("BuyButton").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
//			//gameObject.transform.FindChild("PackBorder").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
//			gameObject.transform.FindChild("PackPicture").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
//			gameObject.transform.FindChild("BuyButton").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
//		}
//	}
//	void OnMouseExit()
//	{
//		if(isClickable)
//		{
//			gameObject.transform.FindChild("PackTitle").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
//			gameObject.transform.FindChild("BuyButton").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
//			//gameObject.transform.FindChild("PackBorder").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
//			gameObject.transform.FindChild("PackPicture").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
//			gameObject.transform.FindChild("BuyButton").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
//		}
//	}
//	public virtual void OnMouseDown()
//	{
//		if(isClickable)
//		{
//			if(this.p.CardType==-2)
//			{
//				this.displaySelectCardTypePopUp();
//			}
//			else
//			{
//				this.buyPackHandler();
//			}
//		}
//	}
//	private void buyPackHandler()
//	{
//		StartCoroutine (this.buyPack ());
//	}
//	private IEnumerator buyPack(int cardType=-1)
//	{
//		if(isSelectCardTypeViewDisplayed)
//		{
//			this.hideSelectCardPopUp();
//		}
//		this.displayLoadingScreen ();
//		yield return StartCoroutine (this.p.buyPack (cardType));
//		this.refreshCredits ();
//		if(this.p.Error=="")
//		{
//			NewStoreController.instance.drawRandomCards(this.Id);
//			if(this.p.CollectionPoints>0)
//			{
//				this.displayCollectionPointsPopUp();
//			}
//			if(this.p.NewSkills.Count>0)
//			{
//				this.displayNewSkillsPopUp();
//			}
//		}
//		else
//		{
//			this.displayErrorPopUp();
//		}
//		this.hideLoadingScreen ();
//	}
//	public virtual void show()
//	{
//		gameObject.transform.FindChild("PackTitle").GetComponent<TextMeshPro>().text=p.Name;
//		gameObject.transform.FindChild("BuyButton").FindChild("Title").GetComponent<TextMeshPro>().text=p.Price.ToString();
//		gameObject.transform.FindChild ("PackPicture").GetComponent<SpriteRenderer> ().sprite = p.texture;
//	}
//	public void displayPack(bool value)
//	{
//		gameObject.transform.FindChild ("PackTitle").gameObject.SetActive (value);
//		gameObject.transform.FindChild ("BuyButton").gameObject.SetActive (value);
//		gameObject.transform.FindChild ("PackPicture").gameObject.SetActive (value);
//		//gameObject.transform.FindChild ("PackBorder").gameObject.SetActive (value);
//	}
//	public void setPackPicture(Sprite picture)
//	{
//		gameObject.transform.FindChild ("PackPicture").GetComponent<SpriteRenderer> ().sprite = picture;
//	}
//	public void setId(int Id)
//	{
//		this.Id = Id;
//	}
//	public void setCentralWindow(Rect centralWindow)
//	{
//		this.centralWindow = centralWindow;
//	}
//	public void setCollectionPointsWindow(Rect collectionPointsWindow)
//	{
//		this.collectionPointsWindow = collectionPointsWindow;
//	}
//	public void setNewSkillsWindow(Rect newSkillsWindow)
//	{
//		this.newSkillsWindow = newSkillsWindow;
//	}
//	public void displayErrorPopUp()
//	{
//		this.errorView = gameObject.AddComponent<NewPackErrorPopUpView> ();
//		this.isErrorViewDisplayed = true;
//		errorView.errorPopUpVM.error = this.p.Error;
//		this.p.Error = "";
//		errorView.popUpVM.centralWindowStyle = new GUIStyle(ressources.popUpSkin.customStyles[3]);
//		errorView.popUpVM.centralWindowTitleStyle = new GUIStyle (ressources.popUpSkin.customStyles [0]);
//		errorView.popUpVM.centralWindowButtonStyle = new GUIStyle (ressources.popUpSkin.button);
//		errorView.popUpVM.transparentStyle = new GUIStyle (ressources.popUpSkin.customStyles [2]);
//		this.errorPopUpResize ();
//	}
//
//	private void errorPopUpResize()
//	{
//		errorView.popUpVM.centralWindow = this.centralWindow;
//		errorView.popUpVM.resize ();
//	}
//
//	public void hideErrorPopUp()
//	{
//		Destroy (this.errorView);
//		this.isErrorViewDisplayed = false;
//	}
//
//
//	public virtual List<string> getCardTypesAllowed()
//	{
//		return new List<string> ();
//	}
//
//
//	public virtual int getCardTypeId(int id)
//	{
//		return -1;
//	}
//	public virtual void refreshCredits()
//	{
//	}
//	public void displayLoadingScreen()
//	{
//		newMenuController.instance.displayLoadingScreen ();
//	}
//	public void hideLoadingScreen()
//	{
//		newMenuController.instance.hideLoadingScreen ();
//	}
//}
//
