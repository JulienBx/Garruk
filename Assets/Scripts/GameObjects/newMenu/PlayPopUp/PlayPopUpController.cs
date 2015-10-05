using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PlayPopUpController : MonoBehaviour 
{
	public GameObject loadingScreenObject;
	public GameObject deckListObject;

	public static PlayPopUpController instance;
	private PlayPopUpModel model;

	private GameObject loadingScreen;
	private IList<int> decksDisplayed;
	private int deckDisplayed;
	private bool arePicturesLoading;
	private bool isSearchingDeck;
	private bool isMouseOnSelectDeckButton;
	private IList<GameObject> deckList;
	private bool attemptToPlay;

	private bool isLoadingScreenDisplayed;
	
	void Update ()
	{
		if(arePicturesLoading)
		{
			if(checkIfPicturesAreLoaded())
			{
				this.setPictures();
				this.arePicturesLoading=false;
			}
		}
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
		this.displayLoadingScreen ();
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
		this.hideLoadingScreen ();
		this.show ();
		if(newMenuController.instance.getIsTutorialLaunched())
		{
			TutorialObjectController.instance.actionIsDone();
		}
	}
	private void initializePopUp()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Choisir un type de match";
		gameObject.transform.FindChild ("Button0").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Entrainement";
		gameObject.transform.FindChild ("quitButton").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Annuler";
		for(int i=0;i<3;i++)
		{
			gameObject.transform.FindChild("Button"+i).GetComponent<PlayPopUpCompetitionButtonController>().setId(i);
		}
		gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Equipe sélectionnée";
		gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text="Aucune équipe";
		this.deckList=new List<GameObject>();
	}
	public void show()
	{
		gameObject.transform.FindChild ("Button1").FindChild ("Title").GetComponent<TextMeshPro> ().text = model.currentDivision.Name;
		gameObject.transform.FindChild ("Button2").FindChild ("Title").GetComponent<TextMeshPro> ().text = model.currentCup.Name;
		StartCoroutine (model.currentDivision.setPicture ());
		StartCoroutine (model.currentCup.setPicture ());
		this.arePicturesLoading=true;
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
			this.deckList[this.deckList.Count-1].transform.parent=gameObject.transform.FindChild("deckList").FindChild("currentDeck");
			this.deckList[this.deckList.Count-1].transform.localScale=new Vector3(1.6f,1.6f,1.6f);
			this.deckList[this.deckList.Count-1].transform.localPosition=new Vector3(0.58f, -0.77f+(this.deckList.Count-1)*(-0.4f),1f);
			this.deckList[this.deckList.Count-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text = model.decks [this.decksDisplayed[i]].Name;
			this.deckList[this.deckList.Count-1].GetComponent<DeckBoardDeckListPlayPopUpController>().setId(i);
		}
	}
	private bool checkIfPicturesAreLoaded()
	{
		if(!model.currentDivision.isTextureLoaded)
		{
			return false;
		}
		if(!model.currentCup.isTextureLoaded)
		{
			return false;
		}
		return true;
	}
	private void setPictures()
	{
		gameObject.transform.FindChild ("Button1").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = model.currentDivision.texture;
		gameObject.transform.FindChild ("Button2").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = model.currentCup.texture;
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
		this.displayLoadingScreen ();
		yield return StartCoroutine(model.player.SetSelectedDeck(model.decks[this.deckDisplayed].Id));
		attemptToPlay = true;
		if(newMenuController.instance.getIsTutorialLaunched())
		{
			TutorialObjectController.instance.actionIsDone();
		}
		else
		{
			this.joinGame();
		}
	}
	public void joinGame()
	{
		if(ApplicationModel.gameType==0)
		{
			this.loadingScreen.GetComponent<LoadingScreenController> ().changeLoadingScreenLabel ("En attente de joueurs ...");
			newMenuController.instance.joinRandomRoom();
		}
		else
		{
			Application.LoadLevel("NewLobby");
		}
	}
	public void quitPopUp()
	{
		newMenuController.instance.hidePlayPopUp ();
	}
	public void mouseOnSelectDeckButton(bool value)
	{
		this.isMouseOnSelectDeckButton = value;
	}
	public Vector3 getFriendlyGameButtonPosition()
	{
		return gameObject.transform.FindChild ("Button0").position;
	}
	public void displayLoadingScreen()
	{
		if(!isLoadingScreenDisplayed)
		{
			this.loadingScreen=Instantiate(this.loadingScreenObject) as GameObject;
			this.isLoadingScreenDisplayed=true;
		}
	}
	public void hideLoadingScreen()
	{
		if(isLoadingScreenDisplayed)
		{
			Destroy (this.loadingScreen);
			this.isLoadingScreenDisplayed=false;
		}
	}
}

