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
		BackOfficeController.instance.displayLoadingScreen ();
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
			BackOfficeController.instance.hideLoadingScreen ();
		}
		else
		{
			BackOfficeController.instance.hideLoadingScreen ();
			BackOfficeController.instance.hideInvitationPopUp();
		}
	}
	private void initializePopUp()
	{
		gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDeck.getReference(6);
		gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text=WordingDeck.getReference(5);
		gameObject.transform.FindChild("description").GetComponent<TextMeshPro>().text=WordingInvitationPopUp.getReference(0);
		gameObject.transform.FindChild("acceptButton").FindChild("Title").GetComponent<TextMeshPro>().text=WordingInvitationPopUp.getReference(1);
		gameObject.transform.FindChild("declineButton").FindChild("Title").GetComponent<TextMeshPro>().text=WordingInvitationPopUp.getReference(2);
		this.deckList=new List<GameObject>();
	}
	public void show()
	{
		gameObject.transform.FindChild ("username").GetComponent<TextMeshPro> ().text=model.invitation.SendingUser.Username;
		gameObject.transform.FindChild ("picture").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnLargeProfilePicture (model.invitation.SendingUser.IdProfilePicture);

		if(model.decks.Count<2)
		{
			this.gameObject.transform.FindChild ("deckList").FindChild("currentDeck").FindChild("selectButton").gameObject.SetActive(false);
		}
	}
	public void selectDeck(int id)
	{
		SoundController.instance.playSound(8);
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
			gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text=WordingDeck.getReference(5);
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
				if(model.decks[i].Id==ApplicationModel.player.SelectedDeckId)
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
		SoundController.instance.playSound(8);
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
			this.deckList[this.deckList.Count-1].transform.localPosition=new Vector3(0f, -1.88f+(this.deckList.Count-1)*(-0.62f),-1f);
			this.deckList[this.deckList.Count-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text = model.decks [this.decksDisplayed[i]].Name;
			this.deckList[this.deckList.Count-1].GetComponent<DeckBoardDeckListInvitationPopUpController>().setId(i);
		}
	}
	public void quitPopUp()
	{
		this.declineInvitationHandler ();
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
		SoundController.instance.playSound(8);
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine(ApplicationModel.player.SetSelectedDeck(model.decks[this.deckDisplayed].Id));
		ApplicationModel.player.ChosenGameType = 20 + model.invitation.Id;
		BackOfficeController.instance.joinRandomRoomHandler ();
		BackOfficeController.instance.hideInvitationPopUp ();
		StartCoroutine(model.invitation.changeStatus(2));
	}
	public void declineInvitationHandler()
	{
		SoundController.instance.playSound(8);
		BackOfficeController.instance.hideInvitationPopUp ();
		StartCoroutine(model.invitation.changeStatus(-1));
	}
}

