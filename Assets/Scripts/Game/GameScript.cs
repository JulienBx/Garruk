using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameScript : Photon.MonoBehaviour {

	public string gameName = ApplicationModel.username;
	public float WaitingTimeToRefresh;
	public GameObject card;
	public Dictionary<int, string> playersName = new Dictionary<int, string>();
	public string labelText = "Placer vos héros sur le champ de bataille";
	public string labelInfo = "En attente d'autres joueurs";
	public bool gameOver = false;
	public string labelMessage = "";
	public static GameScript instance;

	string URLStat = ApplicationModel.dev + "updateResult.php";
	bool hasClicked = false;
	const string roomName = "GarrukGame";
	HostData[] hostList;
	

	void Awake()
	{
		MasterServer.ClearHostList();
		instance = this;
	}

	void Start()
	{	
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
	}
	
	void OnGUI()
	{
		if (gameOver)
		{
			StartCoroutine(returnToLobby());
		}
		GUI.Label(new Rect(530, 0, 800, 50), labelMessage);
		if (playersName.Count > 1)
		{
			GUI.Label(new Rect(10, 0, 500, 50), labelText);
			if (!hasClicked && GUI.Button(new Rect(10, 20, 200, 35), "Commencer le combat"))
			{
				hasClicked = true;
				labelText = "En attente d'actions de l'autre joueur";
				photonView.RPC("StartFight", PhotonTargets.AllBuffered);
			}
			if (!GameBoard.instance.TimeOfPositionning)
			{
				GUI.Label(new Rect(220, 0, 500, 50), "tour " + GameBoard.instance.nbTurn);
			}
		}
		else
		{
			GUI.Label(new Rect(10, 0, 500, 50), labelInfo);
		}
		if (GUI.Button(new Rect(220, 20, 150, 35), "Quitter le match"))
		{
			PhotonNetwork.Disconnect();
		}

	}
	
	void Update() {
	}


	public void EndOfGame(int player)
	{
		gameOver = true;
		labelText = "Joueur " + player + " a gagné";
	}

	private IEnumerator returnToLobby()
	{
		if (GameBoard.instance.MyPlayerNumber == 1)
		{
			yield return new WaitForSeconds(5);
		} 
		else
		{
			yield return new WaitForSeconds(7);
		}
		PhotonNetwork.Disconnect();
	}

	void RemovePlayerFromList(int id)
	{
		playersName.Remove(id);
	}

	public void addStat(int user1, int user2)
	{
		StartCoroutine(sendStat(playersName[user1], playersName[user2]));
	}

	IEnumerator sendStat(string user1, string user2)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick1", user1); 	                    // Pseudo de l'utilisateur victorieux
		form.AddField("myform_nick2", user2); 	                    // Pseudo de l'autre utilisateur
		
		WWW w = new WWW(URLStat, form); 							// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null)
		{
			print(w.error); 										// donne l'erreur eventuelle
		} else
		{
			print(w.text);
		}
	}

	// RPC

	[RPC]
	void AddPlayerToList(int id, string loginName)
	{
		playersName.Add(id, loginName);
	}


	[RPC]
	void StartFight()
	{
		GameBoard.instance.StartFight();
	}


	// PHoton

	void OnJoinedLobby()
	{
		GameBoard.instance.nbPlayer = 2;
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);    
		string sqlLobbyFilter = "C0 = " + ApplicationModel.gameType;
		PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	}
	
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		RoomOptions newRoomOptions = new RoomOptions();
		newRoomOptions.isOpen = true;
		newRoomOptions.isVisible = true;
		newRoomOptions.maxPlayers = 2;
		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", ApplicationModel.gameType } }; // CO pour une partie simple
		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
		
		
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);
		PhotonNetwork.CreateRoom(roomName + Guid.NewGuid().ToString("N"), newRoomOptions, sqlLobby);
		GameBoard.instance.nbPlayer = 1;
		
	}
	
	void OnReceivedRoomListUpdate()
	{
		//roomsList = PhotonNetwork.GetRoomList();
	}

	void OnJoinedRoom()
	{
		Debug.Log("Connected to Room");
		photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);

		
		if (GameBoard.instance.nbPlayer == 1)
		{
			GameTile.AvailableStartingColumns.Add("Column1");
			GameTile.AvailableStartingColumns.Add("Column2");
		} else
		{
			GameTile.AvailableStartingColumns.Add("Column7");
			GameTile.AvailableStartingColumns.Add("Column8");
		}
		

		GameBoard gb = GameObject.Find("Game Board").GetComponent<GameBoard> () as GameBoard;
		StartCoroutine(gb.AddCardToBoard());
	}
	
	
	void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		labelInfo = "l'utilisateur a quitté le match, en attente d'autres utilisateurs";
		RemovePlayerFromList(player.ID);
	}

	void OnDisconnectedFromPhoton()
	{
		Application.LoadLevel("LobbyPage");

	}
}
