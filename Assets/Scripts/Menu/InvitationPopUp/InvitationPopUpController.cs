using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class InvitationPopUpController : MonoBehaviour 
{
	public GameObject deckListObject;
	
	public static InvitationPopUpController instance;
	private InvitationPopUpModel model;
	
	private IList<int> decksDisplayed;
	private int deckDisplayed;
	private bool isSearchingDeck;
	private bool isMouseOnSelectDeckButton;
	private IList<GameObject> deckList;
	
	void Update ()
	{
		if(this.isSearchingDeck)
		{
			if((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))&& !this.isMouseOnSelectDeckButton)
			{
				this.isSearchingDeck=false;
				this.cleanDeckList();
			}
		}
	}
	void Awake()
	{
		MenuController.instance.displayLoadingScreen ();
		instance = this;
		this.model = new InvitationPopUpModel ();
		this.initializePopUp ();
		StartCoroutine (this.initialization());
	}
	public IEnumerator initialization()
	{
		yield return StartCoroutine (model.loadUserData ());
		if(model.isInvitationStillExists)
		{
			this.retrieveDefaultDeck ();
			this.retrieveDecksList ();
			this.drawDeck ();
			this.show ();
			MenuController.instance.hideLoadingScreen ();
		}
		else
		{
			MenuController.instance.hideLoadingScreen ();
			MenuController.instance.hideInvitationPopUp();
		}
	}
	private void initializePopUp()
	{
		gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Armée sélectionnée";
		gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text="Aucune armée";
		gameObject.transform.FindChild("description").GetComponent<TextMeshPro>().text="vous a lancé un défi";
		gameObject.transform.FindChild("acceptButton").FindChild("Title").GetComponent<TextMeshPro>().text="Accepter";
		gameObject.transform.FindChild("declineButton").FindChild("Title").GetComponent<TextMeshPro>().text="Refuser";
		this.deckList=new List<GameObject>();
	}
	public void show()
	{
		gameObject.transform.FindChild ("username").GetComponent<TextMeshPro> ().text=model.invitation.SendingUser.Username;
		gameObject.transform.FindChild ("picture").GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnLargeProfilePicture (model.invitation.SendingUser.idProfilePicture);
	}
	public void selectDeck(int id)
	{
		this.deckDisplayed = this.decksDisplayed [id];
		this.cleanDeckList ();
		this.isSearchingDeck = false;
		this.retrieveDecksList ();
		this.drawDeck ();
	}
	public void drawDeck()
	{
		if(deckDisplayed!=-1)
		{
			gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text = model.decks[this.deckDisplayed].Name;
			gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("selectButton").gameObject.SetActive(true);
		}
		else
		{
			gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text="Aucun deck créé";
			gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("selectButton").gameObject.SetActive(false);
		}
		
	}
	private void retrieveDefaultDeck()
	{
		if(model.decks.Count>0)
		{
			this.deckDisplayed = 0;
			for(int i=0;i<model.decks.Count;i++)
			{
				if(model.decks[i].Id==model.player.SelectedDeckId)
				{
					this.deckDisplayed=i;
					break;
				}
			}
		}
		else
		{
			this.deckDisplayed=-1;
		}
	}
	private void retrieveDecksList()
	{
		this.decksDisplayed=new List<int>();
		if(this.deckDisplayed!=-1)
		{
			for(int i=0;i<model.decks.Count;i++)
			{
				if(i!=this.deckDisplayed)
				{
					this.decksDisplayed.Add (i);
				}
			}
		}
	}
	public void displayDeckList()
	{
		this.cleanDeckList ();
		if(!isSearchingDeck)
		{
			this.setDeckList ();
			this.isSearchingDeck=true;
		}
		else
		{
			this.isSearchingDeck=false;
		}
	}
	private void cleanDeckList ()
	{
		for(int i=0;i<this.deckList.Count;i++)
		{
			Destroy(this.deckList[i]);
		}
		this.deckList=new List<GameObject>();
	}
	private void setDeckList()
	{
		for (int i = 0; i < this.decksDisplayed.Count; i++) 
		{  
			this.deckList.Add (Instantiate(this.deckListObject) as GameObject);
			this.deckList[this.deckList.Count-1].transform.parent=gameObject.transform;
			this.deckList[this.deckList.Count-1].transform.localScale=new Vector3(2.6f,2.6f,2.6f);
			this.deckList[this.deckList.Count-1].transform.localPosition=new Vector3(0f, -1.88f+(this.deckList.Count-1)*(-0.62f),-1f);
			this.deckList[this.deckList.Count-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text = model.decks [this.decksDisplayed[i]].Name;
			this.deckList[this.deckList.Count-1].GetComponent<DeckBoardDeckListInvitationPopUpController>().setId(i);
		}
	}
	public void quitPopUp()
	{
		MenuController.instance.hideInvitationPopUp ();
	}
	public void mouseOnSelectDeckButton(bool value)
	{
		this.isMouseOnSelectDeckButton = value;
	}
	public void acceptInvitationHandler()
	{
		StartCoroutine(acceptInvitation());
	}
	private IEnumerator acceptInvitation()
	{
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.player.SetSelectedDeck(model.decks[this.deckDisplayed].Id));
		ApplicationModel.gameType = 2 + model.invitation.Id;
		MenuController.instance.joinRandomRoomHandler ();
		MenuController.instance.hideInvitationPopUp ();
		StartCoroutine(model.invitation.changeStatus(2));
	}
	public void declineInvitationHandler()
	{
		MenuController.instance.hideInvitationPopUp ();
		StartCoroutine(model.invitation.changeStatus(-1));
	}
}

