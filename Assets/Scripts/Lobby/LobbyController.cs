using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class LobbyController : Photon.MonoBehaviour
{

	public static LobbyController instance;
	private LobbyModel model;
	private LobbyView view;

	public int totalNbResultLimit;
	public GameObject MenuObject;
	public GameObject CardObject;
	public GUIStyle[] screenVMStyle;
	public GUIStyle[] decksVMStyle;
	public GUIStyle[] divisionGameVMStyle;
	public GUIStyle[] cupGameVMStyle;
	public GUIStyle[] friendlyGameVMStyle;
	public GUIStyle[] resultsVMStyle;
	public GUIStyle[] opponentVMStyle;
	public GUIStyle[] lobbyVMStyle;
	public GUIStyle[] deckCardVMStyle;
	public GUIStyle[] playersVMStyle;

	private GameObject[] displayedDeckCards;
	private GameObject cardFocused;
	private GameObject cardPopUpBelongTo;

	private bool attemptToPlay = false;
	private const string roomName = "GarrukLobby";
	
	private int selectedDeck = 0;
	public int countPlayers = 0 ;
	public Dictionary<int, string> playersName = new Dictionary<int, string>();


	void Start()
	{
		instance = this;
		this.model = new LobbyModel ();
		this.view = Camera.main.gameObject.AddComponent <LobbyView>();
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
		StartCoroutine (this.initialization());
	}
	private IEnumerator initialization ()
	{
		yield return StartCoroutine (model.getLobbyData (this.totalNbResultLimit));
		this.initStyles ();
		this.resize ();
		this.initVM ();
		if(model.decks.Count>0)
		{
			this.createDeckCards ();
		}
	}
	public void loadAll()
	{
		this.resize ();
		if(model.decks.Count>0)
		{
			this.clearFocus();
			this.clearDeckCards();
			this.createDeckCards ();
		}
		view.lobbyVM.displayView=true ;
	}
	public void rightClickedCard(GameObject gameObject)
	{
		view.lobbyVM.displayView=false ;

		for(int i = 0 ; i < 5 ; i++)
		{
			this.displayedDeckCards[i].SetActive(false);
		}
		string name = "Fcrd"+gameObject.name.Substring(4);
		Vector3 scale = new Vector3(view.screenVM.heightScreen / 120f,view.screenVM.heightScreen / 120f,view.screenVM.heightScreen / 120f);
		Vector3 position = Camera.main.ScreenToWorldPoint (new Vector3 (0.4f * view.screenVM.widthScreen, 0.45f * view.screenVM.heightScreen - 1, 10));
		this.cardFocused = Instantiate(CardObject) as GameObject;
		this.cardFocused.AddComponent<CardLobbyController> ();
		this.cardFocused.GetComponent<CardController> ().setGameObject (name, scale, position);
		this.cardFocused.GetComponent<CardLobbyController> ().setFocusedLobbyCard (gameObject.GetComponent<CardController> ().card);
		this.cardFocused.GetComponent<CardController> ().setCentralWindowRect (view.screenVM.centralWindow);
	}
	public void exitCard()
	{
		this.clearFocus ();
		for(int i = 0 ; i < 5 ; i++)
		{
			this.displayedDeckCards[i].SetActive(true);
		}
		view.lobbyVM.displayView=true ;
	}
	public IEnumerator buyXpCard(GameObject gameobject)
	{
		int cardIndex = System.Convert.ToInt32(gameobject.name.Substring(4));
		int deckIndex = view.decksVM.decksToBeDisplayed [view.decksVM.chosenDeck];
		yield return StartCoroutine(model.decks [deckIndex].Cards[cardIndex].addXpLevel());
		this.refreshCredits();
		if(model.decks [deckIndex].Cards[cardIndex].Error=="")
		{
			this.setGUI (true);
			this.cardFocused.GetComponent<CardController>().animateExperience (model.decks [deckIndex].Cards[cardIndex]);
		}
		else
		{
			this.cardFocused.GetComponent<CardLobbyController>().resetFocusedLobbyCard(model.decks [deckIndex].Cards[cardIndex]);
			this.cardFocused.GetComponent<CardController>().setError();
			model.decks [deckIndex].Cards[cardIndex].Error="";
		}
		this.displayedDeckCards [cardIndex].GetComponent<CardLobbyController> ().resetLobbyCard (model.decks [deckIndex].Cards[cardIndex]);
	}
	public IEnumerator renameCard(string value, GameObject gameobject)
	{
		int cardIndex = System.Convert.ToInt32(gameobject.name.Substring(4));
		int deckIndex = view.decksVM.decksToBeDisplayed [view.decksVM.chosenDeck];
		int tempPrice = model.decks [deckIndex].Cards[cardIndex].RenameCost;
		yield return StartCoroutine(model.decks [deckIndex].Cards[cardIndex].renameCard(value,tempPrice));
		this.displayedDeckCards [cardIndex].GetComponent<CardLobbyController> ().resetLobbyCard (model.decks [deckIndex].Cards[cardIndex]);
		this.cardFocused.GetComponent<CardLobbyController> ().resetFocusedLobbyCard (model.decks [deckIndex].Cards[cardIndex]);
		this.refreshCredits();
		if(model.decks [deckIndex].Cards[cardIndex].Error=="")
		{
			this.setGUI (true);
		}
		else
		{
			this.cardFocused.GetComponent<CardController>().setError();
			model.decks [deckIndex].Cards[cardIndex].Error="";
		}
	}
	private void initVM()
	{
		view.divisionGameVM.buttonStyle.normal.background = model.currentDivision.texture;
		view.cupGameVM.buttonStyle.normal.background = model.currentCup.texture;
		StartCoroutine (model.currentDivision.setPicture ());
		StartCoroutine (model.currentCup.setPicture ());
		view.decksVM.decksName = new List<string> ();
		view.decksVM.decksToBeDisplayed = new List<int> ();
		view.decksVM.myDecksButtonGuiStyle = new List<GUIStyle> ();
		for (int i=0;i<model.decks.Count;i++)
		{
			if(model.decks[i].Id==model.player.SelectedDeckId)
			{
				view.decksVM.decksToBeDisplayed.Insert(0,i);
				view.decksVM.decksName.Insert(0,model.decks[i].Name);
			}
			else
			{
				view.decksVM.decksToBeDisplayed.Add (i);
				view.decksVM.decksName.Add (model.decks[i].Name);
			}
			view.decksVM.myDecksButtonGuiStyle.Add (new GUIStyle());
			view.decksVM.myDecksButtonGuiStyle[i]=view.decksVM.deckButtonStyle;
		}
		if(model.decks.Count>0)
		{
			view.decksVM.myDecksButtonGuiStyle[view.decksVM.chosenDeck]=view.decksVM.deckButtonChosenStyle;
			view.lobbyVM.gameButtonsEnabled=true;
		}
		else
		{
			view.decksCardVM.noDeckLabel="Vous n'avez pas encore constitué de deck, rendez-vous sur la page mon jeu pour créer votre deck";
			view.decksVM.decksTitle="";
			view.lobbyVM.gameButtonsEnabled=false;
		}
		if(model.player.NbGamesCup==0)
		{
			view.cupGameVM.cupInformationsLabel=
				model.currentCup.Name+
					"\nnon démarrée " 
					+"\nTours restants : "+(model.currentCup.NbRounds-model.player.NbGamesCup)
					+"\n Prix : "+model.currentCup.CupPrize+" crédit";
		}
		else
		{
			view.cupGameVM.cupInformationsLabel=
				model.currentCup.Name+
					"\n" + model.player.NbGamesCup +" victoires " 
					+"\nTours restants : "+(model.currentCup.NbRounds-model.player.NbGamesCup)
					+"\n Récompense : "+model.currentCup.CupPrize;
		}
		if(model.player.NbGamesDivision==0)
		{
			view.divisionGameVM.divisionInformationsLabel=
				model.currentDivision.Name+
					"\nnon démarrée " 
					+"\nMatchs restants : "+(model.currentDivision.NbGames-model.player.NbGamesDivision)
					+"\n Titre : "+model.currentDivision.TitlePrize+" crédit";
		}
		else
		{
			view.divisionGameVM.divisionInformationsLabel=
				model.currentDivision.Name+
					"\n" + model.player.NbGamesDivision +" matchs joués " 
					+"\nMatchs restants : "+(model.currentDivision.NbGames-model.player.NbGamesDivision)
					+"\n Titre : "+model.currentDivision.TitlePrize+" crédit";
		}
		if(model.currentDivision.PromotionPrize>0)
		{
			view.divisionGameVM.divisionInformationsLabel=view.divisionGameVM.divisionInformationsLabel+"\n Promotion : "+model.currentDivision.PromotionPrize+" crédit";
		}
		this.updateNbPlayersLabel ();
	}
	private void updateNbPlayersLabel()
	{
		if (countPlayers>0)
		{
			if (countPlayers==1)
			{
				view.playersVM.label="Vous etes le seul utilisateur connecté";
			}
			else
			{
				view.playersVM.label=countPlayers+" utilisateurs connectés";
			}
		}
	}
	private void createDeckCards()
	{
		string name;
		Vector3 scale;
		Vector3 position;
		float tempF = 10f*view.screenVM.widthScreen/view.screenVM.heightScreen;
		float width = 10f*0.68f*(view.screenVM.blockTopCenterWidth)/(0.9f*view.screenVM.heightScreen-2*view.screenVM.gapBetweenblocks);
		float scaleDeck = Mathf.Min(1.6f, width / 6f);
		float pas = (width - 5f * scaleDeck) / 6f;
		float debutLargeur = -0.28f * tempF + pas + scaleDeck / 2;
		int deckIndex = view.decksVM.decksToBeDisplayed [view.decksVM.chosenDeck];
		this.displayedDeckCards = new GameObject[5];
		
		for (int i = 0; i < 5; i++)
		{
			name="Card" + i;
			scale = new Vector3(scaleDeck,scaleDeck,scaleDeck);
			position = new Vector3(debutLargeur + (scaleDeck + pas) * i, 2.65f, 0); 
			this.displayedDeckCards [i] = Instantiate(this.CardObject) as GameObject;
			this.displayedDeckCards [i].AddComponent<CardLobbyController>();
			this.displayedDeckCards [i].GetComponent<CardController>().setGameObject(name,scale,position);
			this.displayedDeckCards[i].GetComponent<CardLobbyController>().setLobbyCard(model.decks[deckIndex].Cards[i]);
			this.displayedDeckCards[i].GetComponent<CardController> ().setCentralWindowRect (view.screenVM.centralWindow);
		} 
	}
	private void clearDeckCards()
	{
		for (int i = 0; i < 5; i++) 
		{
			Destroy(this.displayedDeckCards[i]);
		}
	}
	private void clearFocus()
	{
		if(this.cardFocused!=null)
		{
			Destroy (this.cardFocused);
		}
	}
	public void displayDeck(int chosenDeck)
	{
		view.decksVM.myDecksButtonGuiStyle [view.decksVM.chosenDeck] = view.decksVM.deckButtonStyle;
		view.decksVM.myDecksButtonGuiStyle [chosenDeck] = view.decksVM.deckButtonChosenStyle;

		view.decksVM.chosenDeck = chosenDeck;
		for(int i=0;i<5;i++)
		{
			this.displayedDeckCards[i].GetComponent<CardLobbyController>().resetLobbyCard(model.decks[view.decksVM.decksToBeDisplayed[view.decksVM.chosenDeck]].Cards[i]);
		}
	}
	private void initStyles()
	{
		view.screenVM.styles=new GUIStyle[this.screenVMStyle.Length];
		for(int i=0;i<this.screenVMStyle.Length;i++)
		{
			view.screenVM.styles[i]=this.screenVMStyle[i];
		}
		view.screenVM.initStyles();
		view.decksVM.styles=new GUIStyle[this.decksVMStyle.Length];
		for(int i=0;i<this.decksVMStyle.Length;i++)
		{
			view.decksVM.styles[i]=this.decksVMStyle[i];
		}
		view.decksVM.initStyles();
		view.divisionGameVM.styles=new GUIStyle[this.divisionGameVMStyle.Length];
		for(int i=0;i<this.divisionGameVMStyle.Length;i++)
		{
			view.divisionGameVM.styles[i]=this.divisionGameVMStyle[i];
		}
		view.divisionGameVM.initStyles();
		view.cupGameVM.styles=new GUIStyle[this.cupGameVMStyle.Length];
		for(int i=0;i<this.cupGameVMStyle.Length;i++)
		{
			view.cupGameVM.styles[i]=this.cupGameVMStyle[i];
		}
		view.cupGameVM.initStyles();
		view.friendlyGameVM.styles=new GUIStyle[this.friendlyGameVMStyle.Length];
		for(int i=0;i<this.friendlyGameVMStyle.Length;i++)
		{
			view.friendlyGameVM.styles[i]=this.friendlyGameVMStyle[i];
		}
		view.friendlyGameVM.initStyles();
		view.lobbyVM.styles=new GUIStyle[this.lobbyVMStyle.Length];
		for(int i=0;i<this.lobbyVMStyle.Length;i++)
		{
			view.lobbyVM.styles[i]=this.lobbyVMStyle[i];
		}
		view.lobbyVM.initStyles();
		view.decksCardVM.styles=new GUIStyle[this.deckCardVMStyle.Length];
		for(int i=0;i<this.deckCardVMStyle.Length;i++)
		{
			view.decksCardVM.styles[i]=this.deckCardVMStyle[i];
		}
		view.decksCardVM.initStyles();
		view.playersVM.styles=new GUIStyle[this.playersVMStyle.Length];
		for(int i=0;i<this.playersVMStyle.Length;i++)
		{
			view.playersVM.styles[i]=this.playersVMStyle[i];
		}
		view.playersVM.initStyles();
	}
	private void resize()
	{
		view.screenVM.resize ();
		view.decksVM.resize (view.screenVM.heightScreen);
		view.divisionGameVM.resize (view.screenVM.heightScreen);
		view.cupGameVM.resize (view.screenVM.heightScreen);
		view.friendlyGameVM.resize (view.screenVM.heightScreen);
		view.lobbyVM.resize (view.screenVM.heightScreen);
		view.decksCardVM.resize (view.screenVM.heightScreen);
		view.playersVM.resize (view.screenVM.heightScreen);
	}
	public void setGUI(bool value)
	{
		view.lobbyVM.guiEnabled = value;
		if(this.cardFocused!=null)
		{
			this.cardFocused.GetComponent<CardController>().setMyGUI(value);
		}
		for(int i = 0 ; i < 5 ; i++)
		{
			this.displayedDeckCards[i].GetComponent<CardController>().setMyGUI(value);
		}
	}
	public void refreshCredits()
	{
		StartCoroutine(this.MenuObject.GetComponent<MenuController> ().getUserData ());
	}
	public void popUpDisplayed(bool value, GameObject gameObject)
	{
		this.cardPopUpBelongTo = gameObject;
		view.lobbyVM.isPopUpDisplayed = value;
	}
	public void returnPressed()
	{
		if(view.lobbyVM.isPopUpDisplayed)
		{
			this.cardPopUpBelongTo.GetComponent<CardController> ().confirmPopUp ();
		}
	}
	public void escapePressed()
	{
		if(view.lobbyVM.isPopUpDisplayed)
		{
			this.cardPopUpBelongTo.GetComponent<CardController> ().exitPopUp ();
		}
	}
	public void joinFriendlyGame()
	{
		ApplicationModel.gameType = 0; // 0 pour training
		attemptToPlay = true;
		PhotonNetwork.Disconnect();
	}
	public void joinDivisionLobby()
	{
		ApplicationModel.gameType = 1;
		attemptToPlay = true;
		PhotonNetwork.Disconnect();
	}
	public void joinCupGame()
	{
		ApplicationModel.gameType = 2; // 1 pour Official
		attemptToPlay = true;
		PhotonNetwork.Disconnect();
	}
	void RemovePlayerFromList(int id)
	{
		playersName.Remove(id);
		countPlayers--;
		this.updateNbPlayersLabel ();
	}
	
	void OnJoinedLobby()
	{
		TypedLobby sqlLobby = new TypedLobby("lobby", LobbyType.SqlLobby);    
		string sqlLobbyFilter = "C0 = 0";
		PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	}
	
	void OnPhotonRandomJoinFailed()
	{
		RoomOptions newRoomOptions = new RoomOptions();
		newRoomOptions.isOpen = true;
		newRoomOptions.isVisible = true;
		newRoomOptions.maxPlayers = 50;
		
		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", 0 } };
		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
		TypedLobby sqlLobby = new TypedLobby("lobby", LobbyType.SqlLobby);
		
		PhotonNetwork.CreateRoom(roomName, newRoomOptions, sqlLobby);
		//Debug.Log("Creating room");
	}
	
	void OnReceivedRoomListUpdate()
	{
		//roomsList = PhotonNetwork.GetRoomList();
	}
	
	void OnJoinedRoom()
	{
		//Debug.Log("Connected to Room");
		photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);
	}
	
	void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		RemovePlayerFromList(player.ID);
	}
	
	void OnDisconnectedFromPhoton()
	{
		if (attemptToPlay)
		{
			StartCoroutine(model.player.SetSelectedDeck(model.decks[view.decksVM.decksToBeDisplayed[view.decksVM.chosenDeck]].Id));
			if(ApplicationModel.gameType==1)
			{
				Application.LoadLevel("DivisionLobby");
			}
			else if(ApplicationModel.gameType==2)
			{
				Application.LoadLevel("CupLobby");
			}
			else
			{
				Application.LoadLevel("Game");
			}
		}
	}

	[RPC]
	void AddPlayerToList(int id, string loginName)
	{
		//print ("I add a player");
		playersName.Add(id, loginName);
		countPlayers++;
		this.updateNbPlayersLabel ();
	}

}
