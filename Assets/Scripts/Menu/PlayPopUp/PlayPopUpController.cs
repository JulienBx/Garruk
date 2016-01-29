using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PlayPopUpController : MonoBehaviour 
{
	public GameObject deckListObject;

	public static PlayPopUpController instance;
	private PlayPopUpModel model;
	
	private IList<int> decksDisplayed;
	private int deckDisplayed;
	private bool isSearchingDeck;
	private bool isMouseOnSelectDeckButton;
	private IList<GameObject> deckList;
	private bool isLoadingScreenDisplayed;
	
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
	}
	void Start () 
	{	
		instance = this;
		this.model = new PlayPopUpModel ();
		this.initializePopUp ();
		StartCoroutine (this.initialization());
	}
	public IEnumerator initialization()
	{
		yield return StartCoroutine (model.loadUserData ());
		this.retrieveDefaultDeck ();
		this.retrieveDecksList ();
		this.drawDeck ();
		MenuController.instance.hideLoadingScreen ();
		MenuController.instance.setGUI(false);
		this.show ();
		TutorialObjectController.instance.tutorialTrackPoint ();
	}
	private void initializePopUp()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingPlayPopUp.getReference(0);
		gameObject.transform.FindChild ("Button0").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingGameModes.getReference(0);
		gameObject.transform.FindChild ("quitButton").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingPlayPopUp.getReference(1);
		for(int i=0;i<3;i++)
		{
			gameObject.transform.FindChild("Button"+i).GetComponent<PlayPopUpCompetitionButtonController>().setId(i);
		}
		gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDeck.getReference(6);
		gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text=WordingDeck.getReference(5);
		this.deckList=new List<GameObject>();
	}
	public void show()
	{
		gameObject.transform.FindChild ("Button1").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingGameModes.getReference(1);
		//gameObject.transform.FindChild ("Button2").FindChild ("Title").GetComponent<TextMeshPro> ().text = model.currentCup.Name;
		gameObject.transform.FindChild ("Button1").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnCompetitionPicture(model.currentDivision.IdPicture);
		gameObject.transform.FindChild ("Button2").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnCompetitionPicture(model.currentCup.IdPicture);
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
			this.deckList[this.deckList.Count-1].transform.localPosition=new Vector3(0f, 1.62f+(this.deckList.Count-1)*(-0.62f),-1f);
			this.deckList[this.deckList.Count-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text = model.decks [this.decksDisplayed[i]].Name;
			this.deckList[this.deckList.Count-1].GetComponent<DeckBoardDeckListPlayPopUpController>().setId(i);
		}
	}
	public void selectGame(int id)
	{
		if(deckDisplayed!=-1)
		{
			ApplicationModel.gameType = id;
			StartCoroutine (this.setSelectedDeck ());
		}
	}
	private IEnumerator setSelectedDeck()
	{
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.player.SetSelectedDeck(model.decks[this.deckDisplayed].Id));
		this.joinGame();
	}
	public void joinGame()
	{
		if(ApplicationModel.gameType==0)
		{
			MenuController.instance.joinRandomRoomHandler();
		}
		else
		{
			Application.LoadLevel("NewLobby");
		}
	}
	public void quitPopUp()
	{
		MenuController.instance.hidePlayPopUp ();
	}
	public void mouseOnSelectDeckButton(bool value)
	{
		this.isMouseOnSelectDeckButton = value;
	}

	#region TUTORIAL FUNCTIONS

	public Vector3 getFriendlyGameButtonPosition()
	{
		Vector3 buttonPosition = gameObject.transform.FindChild ("Button0").position;
		buttonPosition.x = buttonPosition.x - ApplicationDesignRules.menuPosition.x;
		buttonPosition.y = buttonPosition.y - ApplicationDesignRules.menuPosition.y;
		return buttonPosition;
	}
	public Vector3 getQuitPopUpButtonPosition()
	{
		Vector3 buttonPosition = gameObject.transform.FindChild ("quitButton").position;
		buttonPosition.x = buttonPosition.x - ApplicationDesignRules.menuPosition.x;
		buttonPosition.y = buttonPosition.y - ApplicationDesignRules.menuPosition.y;
		return buttonPosition;
	}

	#endregion
}

