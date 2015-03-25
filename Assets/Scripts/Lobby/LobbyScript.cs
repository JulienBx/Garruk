using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class LobbyScript : Photon.MonoBehaviour {

	public GUIStyle playerTitle;
	public GUIStyle playerName;
	public GUIStyle myPlayerName;
	public GUIStyle deckTitle;
	public GUIStyle activatedDeck;
	public GUIStyle deck;
	public GUIStyle joinButton;
	public GUIStyle cantJoin;
	
	public List<Deck> decks = new List<Deck>();
	private string URLGetDecks = ApplicationModel.host + "get_full_decks_by_user.php";
	private string URLSelectedDeck = ApplicationModel.host + "set_selected_deck.php";
	public Dictionary<int, string> playersName = new Dictionary<int, string>();

	private bool attemptToPlay = false;
	private const string roomName = "GarrukLobby";

	public int selectedDeck = 0;
	public int countPlayers = 0 ;

	int widthScreen ;
	int heightScreen ;

	void Start()
	{
		this.setStyles();
		StartCoroutine(RetrieveDecks());
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
	}

	void Update(){
		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			this.setStyles();
		}
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(0.10f*widthScreen,0.12f*heightScreen,widthScreen * 0.80f,0.86f*heightScreen));
		{
			GUILayout.BeginVertical();
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.BeginVertical(GUILayout.Width(0.4f*widthScreen), GUILayout.Height(0.6f*heightScreen));
					{
						if (countPlayers>0){
							if (countPlayers==1){
								GUILayout.Label("Vous etes le seul utilisateur connecté", playerTitle);
							}
							else{
								GUILayout.Label(countPlayers+" utilisateurs connectés", playerTitle);
							}
							
							foreach(KeyValuePair<int, string> entry in playersName)
							{
								if (entry.Value == ApplicationModel.username)
								{
									GUILayout.Label(entry.Value, myPlayerName);
								}
								else
								{
									GUILayout.Label(entry.Value, playerName);
								}
							}
						}
					}
					GUILayout.EndVertical();
					GUILayout.FlexibleSpace();
					GUILayout.BeginVertical(GUILayout.Width(0.4f*widthScreen),GUILayout.Height(0.6f*heightScreen));
					{
						if (selectedDeck != 0){
							GUILayout.Label("Choisissez le deck avec lequel jouer", deckTitle);
						}
						else{
							GUILayout.Label("Aucun deck complet n'est disponible", deckTitle);
						}

						for( int j = 0 ; j < decks.Count ; j++)
						{
							if (selectedDeck == decks[j].Id)
							{
								if (GUILayout.Button(decks[j].Name, activatedDeck))
								{

								}
							}
							else
							{
								if(GUILayout.Button(decks[j].Name, deck))
								{
									selectedDeck = decks[j].Id;
									StartCoroutine(SetSelectedDeck(selectedDeck));
								}
							}
						}

					}
					GUILayout.EndVertical();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				if (selectedDeck != 0)
				{
					if (GUILayout.Button("rejoindre un match d'entrainement", joinButton))
					{
						ApplicationModel.gameType = 0; // 0 pour training
						attemptToPlay = true;
						PhotonNetwork.Disconnect();
					}
					if (GUILayout.Button("rejoindre un match officiel", joinButton))
					{
						ApplicationModel.gameType = 1; // 1 pour Official
						attemptToPlay = true;
						PhotonNetwork.Disconnect();
					}
				}
				else{
					GUILayout.Label("Impossible de rejoindre un match", cantJoin);
				}

				}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();

	}

	private void setStyles() {
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;

		this.playerTitle.fontSize = heightScreen * 2 / 100 ;
		this.playerTitle.fixedHeight = heightScreen * 5 / 100 ;
		this.playerTitle.fixedWidth = widthScreen * 40 / 100 ;

		this.myPlayerName.fontSize = heightScreen * 2 / 100 ;
		this.myPlayerName.fixedHeight = heightScreen * 3 / 100 ;
		this.myPlayerName.fixedWidth = widthScreen * 40 / 100 ;

		this.playerName.fontSize = heightScreen * 2 / 100 ;
		this.playerName.fixedHeight = heightScreen * 3 / 100 ;
		this.playerName.fixedWidth = widthScreen * 40 / 100 ;

		this.deckTitle.fontSize = heightScreen * 2 / 100 ;
		this.deckTitle.fixedHeight = heightScreen * 5 / 100 ;
		this.deckTitle.fixedWidth = widthScreen * 40 / 100 ;

		this.deck.fontSize = heightScreen * 2 / 100 ;
		this.deck.fixedHeight = heightScreen * 3 / 100 ;
		this.deck.fixedWidth = widthScreen * 40 / 100 ;

		this.activatedDeck.fontSize = heightScreen * 2 / 100 ;
		this.activatedDeck.fixedHeight = heightScreen * 3 / 100 ;
		this.activatedDeck.fixedWidth = widthScreen * 40 / 100 ;

		this.joinButton.fontSize = heightScreen * 4 / 100 ;
		this.joinButton.fixedHeight = heightScreen * 10 / 100 ;
		this.joinButton.fixedWidth = widthScreen * 80 / 100 ;

		this.cantJoin.fontSize = heightScreen * 4 / 100 ;
		this.cantJoin.fixedHeight = heightScreen * 10 / 100 ;
		this.cantJoin.fixedWidth = widthScreen * 80 / 100 ;
	}

	IEnumerator RetrieveDecks() {

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		
		WWW w = new WWW(URLGetDecks, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			//print(w.text); 											// donne le retour
			
			string[] decksInformation = w.text.Split('\n'); 
			string[] deckInformation;

			selectedDeck = System.Convert.ToInt32(decksInformation[0]);

			for(int i = 1 ; i < decksInformation.Length - 1 ; i++) 		// On boucle sur les attributs d'un deck
			{
				deckInformation = decksInformation[i].Split('\\'); 	// On découpe les attributs du deck qu'on place dans un tableau
				
				int deckId = System.Convert.ToInt32(deckInformation[0]); 	// Ici, on récupère l'id en base
				string deckName = deckInformation[1]; 						// Le nom du deck
				if (i == 0)
				{
					selectedDeck = deckId;
					StartCoroutine(SetSelectedDeck(selectedDeck));
				}
				decks.Add(new Deck(deckId, deckName, 5));
			}
		}
	}

	IEnumerator SetSelectedDeck(int selectedDeck)
	{
		print ("je selectionne"+selectedDeck);
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_deck", selectedDeck);                 // Deck sélectionné
		
		WWW w = new WWW (URLSelectedDeck, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} else {
//			print (w.text);
		}
	}

	void RemovePlayerFromList(int id)
	{
		playersName.Remove(id);
		countPlayers--;
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
		Debug.Log("Creating room");
	}

	void OnReceivedRoomListUpdate()
	{
		//roomsList = PhotonNetwork.GetRoomList();
	}

	void OnJoinedRoom()
	{
		Debug.Log("Connected to Room");
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
			StartCoroutine(SetSelectedDeck(selectedDeck));
			Application.LoadLevel("GamePage");
		}
	}

	public void link1()
	{
		Application.LoadLevel("HomePage");
		PhotonNetwork.Disconnect();
	}
	
	public void link2()
	{
		Application.LoadLevel("MyGame");
		PhotonNetwork.Disconnect();
	}
	
	public void link3()
	{
		Application.LoadLevel("BuyCards");
		PhotonNetwork.Disconnect();
	}
	
	public void link4()
	{
		Application.LoadLevel("Market");
		PhotonNetwork.Disconnect();
	}
	
	public void logOutLink() 
	{
		ApplicationModel.username = "";
		ApplicationModel.toDeconnect = true;
		PhotonNetwork.Disconnect();
		Application.LoadLevel("ConnectionPage");
	}
	
	public void profileLink() 
	{
		Application.LoadLevel("Profile");
		PhotonNetwork.Disconnect();
	}

	// RPC
	[RPC]
	void AddPlayerToList(int id, string loginName)
	{
		//print ("I add a player");
		playersName.Add(id, loginName);
		countPlayers++;
	}
}
