using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayPopUpController : MonoBehaviour 
{
	public GameObject deckListObject;

	public static PlayPopUpController instance;
	//private PlayPopUpModel model;
	
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
		BackOfficeController.instance.displayLoadingScreen ();
		this.gameObject.transform.FindChild("Error").gameObject.SetActive(false);
	}
	void Start () 
	{	
		instance = this;
		this.initializePopUp ();
		this.initialization();
	}
	public void initialization()
	{
		BackOfficeController.instance.hideLoadingScreen ();
		if(ApplicationModel.player.MyDecks.getCount()>0)
		{
			this.retrieveDefaultDeck ();
			this.retrieveDecksList ();
			this.drawDeck ();
			this.show ();
		}
		else
		{
			BackOfficeController.instance.failPlayPopUp();
		}
	}
	private void initializePopUp()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingPlayPopUp.getReference(0);
		gameObject.transform.FindChild ("Button0").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingGameModes.getReference(0);
		for(int i=0;i<2;i++)
		{
			gameObject.transform.FindChild("Button"+i).GetComponent<PlayPopUpCompetitionButtonController>().setId(i);
		}
		gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDeck.getReference(6);
		gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text=WordingDeck.getReference(5);
		this.deckList=new List<GameObject>();
	}
	public void show()
	{
		this.gameObject.transform.FindChild("FriendlyGameTitle").GetComponent<TextMeshPro> ().text = WordingGameModes.getReference(0).ToUpper ();
		this.gameObject.transform.FindChild("Button0").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingGameModes.getReference(2);

		if(ApplicationModel.player.TrainingStatus!=-1)
		{
			this.gameObject.transform.FindChild("TrainingGamePicture").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("DivisionGamePicture").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("TrainingGamePicture").GetComponent<TrainingController>().draw(ApplicationModel.player.TrainingStatus);
			this.gameObject.transform.FindChild("OfficialGameTitle").GetComponent<TextMeshPro> ().text = WordingGameModes.getReference(10).ToUpper ();
			this.gameObject.transform.FindChild("Button1").FindChild ("Title").GetComponent<TextMeshPro> ().text  = WordingGameModes.getReference(11);
		}
		else
		{
			this.gameObject.transform.FindChild("TrainingGamePicture").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("DivisionGamePicture").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("OfficialGameTitle").GetComponent<TextMeshPro> ().text = WordingGameModes.getReference(1).ToUpper ();
			string divisionState;
			if(ApplicationModel.player.MyDivision.GamesPlayed>0)
			{
				divisionState=WordingGameModes.getReference(3);
			}
			else
			{
				divisionState=WordingGameModes.getReference(4);
			}
			this.gameObject.transform.FindChild("Button1").FindChild ("Title").GetComponent<TextMeshPro> ().text  = divisionState;
		}

		if(ApplicationModel.player.MyDecks.getCount()<2)
		{
			this.gameObject.transform.FindChild ("deckList").FindChild("currentDeck").FindChild("selectButton").gameObject.SetActive(false);
			Vector3 deckNamePosition = this.gameObject.transform.FindChild ("deckList").FindChild("currentDeck").FindChild("deckName").localPosition;
			deckNamePosition.x=0;
			this.gameObject.transform.FindChild ("deckList").FindChild("currentDeck").FindChild("deckName").localPosition=deckNamePosition;
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
			gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text = ApplicationModel.player.MyDecks.getDeck(this.deckDisplayed).Name;
			gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("selectButton").gameObject.SetActive(true);
			ApplicationModel.player.setSelectedDeck(this.deckDisplayed);
		}
		else
		{
			gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text=WordingDeck.getReference(5);
			gameObject.transform.FindChild("deckList").FindChild("currentDeck").FindChild("selectButton").gameObject.SetActive(false);
		}

	}
	private void retrieveDefaultDeck()
	{
		if(ApplicationModel.player.MyDecks.getCount()>0)
		{
			this.deckDisplayed = 0;
			for(int i=0;i<ApplicationModel.player.MyDecks.getCount();i++)
			{
				if(i==ApplicationModel.player.SelectedDeckIndex)
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
			for(int i=0;i<ApplicationModel.player.MyDecks.getCount();i++)
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
			this.deckList[this.deckList.Count-1].transform.localPosition=new Vector3(0f, 2.52f+(this.deckList.Count-1)*(-0.62f),-1f);
			this.deckList[this.deckList.Count-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text = ApplicationModel.player.MyDecks.getDeck(this.decksDisplayed[i]).Name;
			this.deckList[this.deckList.Count-1].GetComponent<DeckBoardDeckListPlayPopUpController>().setId(i);
		}
	}
	public void selectGame(int id)
	{
		SoundController.instance.playSound(9);
		if (this.deckDisplayed == -1) 
		{
			this.gameObject.transform.FindChild ("Error").gameObject.SetActive (true);
			this.gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = WordingGameModes.getReference (5);
		} 
		else if (id == 0) 
		{
			ApplicationModel.player.ChosenGameType = 0;
			StartCoroutine (this.joinGame ());
		} 
		else if (!BackOfficeController.instance.isOnline ()) 
		{
		}
		else if(ApplicationModel.player.TrainingStatus==-1)
		{
			
			ApplicationModel.player.ChosenGameType=10+ApplicationModel.player.MyDivision.Id;
			StartCoroutine (this.joinGame ());
		}
		else if(!ApplicationModel.player.canAccessTrainingMode())
		{
			this.gameObject.transform.FindChild("Error").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("Error").GetComponent<TextMeshPro>().text=WordingGameModes.getReference(12)+" "+WordingCardTypes.getName(ApplicationModel.player.TrainingAllowedCardType);
		}
		else
		{
			ApplicationModel.player.ChosenGameType=1+ApplicationModel.player.TrainingAllowedCardType;
			StartCoroutine (this.joinGame ());
		}
	}
	public IEnumerator joinGame()
	{
		this.gameObject.transform.FindChild("Error").gameObject.SetActive(false);
		BackOfficeController.instance.displayLoadingScreen ();
		ApplicationModel.player.setSelectedDeck(this.deckDisplayed);
		if(ApplicationModel.player.ChosenGameType>10)
		{
            BackOfficeController.instance.loadScene("NewLobby");
		}
		else
		{
			BackOfficeController.instance.joinRandomRoomHandler();
		}
		yield break;
	}
	public void quitPopUp()
	{
		SoundController.instance.playSound(8);
		BackOfficeController.instance.hidePlayPopUp ();
	}
	public void mouseOnSelectDeckButton(bool value)
	{
		this.isMouseOnSelectDeckButton = value;
	}
}

