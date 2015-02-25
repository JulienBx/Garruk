using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LobbyScript : Photon.MonoBehaviour {

	public GUIStyle style;
	public List<Deck> decks = new List<Deck>();
	private string URLGetDecks = ApplicationModel.host + "get_full_decks_by_user.php";
	private string URLSelectedDeck = ApplicationModel.host + "set_selected_deck.php";
	public Dictionary<int, string> playersName = new Dictionary<int, string>();

	private bool attemptToPlay = false;
	private const string roomName = "GarrukLobby";
	private RoomInfo[] roomsList;
	public int selectedDeck = 0;

	void Start()
	{
		StartCoroutine(RetrieveDecks());
		style.normal.textColor = Color.red;
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
	}

	void Update() {
	}

	void OnGUI()
	{
		if (playersName.Count > 0)
		{
			int i = 0;
			GUI.Label(new Rect(10, 0, 500, 50), "Liste des utilisateurs connectés");
			foreach(KeyValuePair<int, string> entry in playersName)
			{
				i++;
				if (entry.Value == ApplicationModel.username)
				{
					GUI.Label(new Rect(10, i * 30, 100, 50), entry.Value, style);
				}
				else
				{
					GUI.Label(new Rect(10, i * 30, 100, 50), entry.Value);
				}
			}
		}
		if (selectedDeck != 0)
		{
			if (GUI.Button(new Rect(500, 0, 200, 50), "rejoindre un match"))
			{
				attemptToPlay = true;
				PhotonNetwork.Disconnect();
			}
		} 
		else
		{
			GUI.Label(new Rect(500, 0, 200, 50), "Vous n'avez pas de deck complet");
		}
		for( int j = 0 ; j < decks.Count ; j++)
		{
			if (selectedDeck == decks[j].Id)
			{
				if (GUI.Button(new Rect(400, j * 20, 100, 20), decks[j].Name, style))
				{
					selectedDeck = decks[j].Id;
					StartCoroutine(SetSelectedDeck(selectedDeck));
				}
			}
			else
			{
				if(GUI.Button(new Rect(400, j * 20, 100, 20), decks[j].Name))
				{
					selectedDeck = decks[j].Id;
					StartCoroutine(SetSelectedDeck(selectedDeck));
				}
			}
		}
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
			print(w.text); 											// donne le retour
			
			string[] decksInformation = w.text.Split('\n'); 
			string[] deckInformation;
						
			for(int i = 0 ; i < decksInformation.Length - 1 ; i++) 		// On boucle sur les attributs d'un deck
			{
				deckInformation = decksInformation[i].Split('\\'); 	// On découpe les attributs du deck qu'on place dans un tableau
				
				int deckId = System.Convert.ToInt32(deckInformation[0]); 	// Ici, on récupère l'id en base
				string deckName = deckInformation[1]; 						// Le nom du deck
				int deckNbC = System.Convert.ToInt32(deckInformation[2]);	// le nombre de cartes
				if (i == 0)
				{
					selectedDeck = deckId;
					StartCoroutine(SetSelectedDeck(selectedDeck));
				}
				decks.Add(new Deck(deckId, deckName, deckNbC));
			}
		}
	}

	IEnumerator SetSelectedDeck(int selectedDeck)
	{
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
			print (w.text);
		}
	}

	void RemovePlayerFromList(int id)
	{
		playersName.Remove(id);
	}

	// Photon
	void OnJoinedLobby()
	{
		TypedLobby sqlLobby = new TypedLobby("lobby", LobbyType.SqlLobby);    
		string sqlLobbyFilter = "C0 = 0";
		PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		RoomOptions newRoomOptions = new RoomOptions();
		newRoomOptions.isOpen = true;
		newRoomOptions.isVisible = true;
		newRoomOptions.maxPlayers = 50;

		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", 0 } };
		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
		TypedLobby sqlLobby = new TypedLobby("lobby", LobbyType.SqlLobby);
		
		PhotonNetwork.CreateRoom(roomName, newRoomOptions, sqlLobby);
	}

	void OnReceivedRoomListUpdate()
	{
		roomsList = PhotonNetwork.GetRoomList();
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

	// RPC
	[RPC]
	void AddPlayerToList(int id, string loginName)
	{
		playersName.Add(id, loginName);
	}
}
