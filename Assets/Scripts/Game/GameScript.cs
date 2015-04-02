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
	bool isReconnecting = false ;
	public bool isFirstPlayer = false ;
	string URLStat = ApplicationModel.dev + "updateResult.php";
	bool hasClicked = false;
	const string roomName = "GarrukGame";
	HostData[] hostList;
	int nbPlayers = 0 ;

	void Awake()
	{
		MasterServer.ClearHostList();
		instance = this;
	}

	void Start()
	{	
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
	}

	private void initGrid(){
		
		int decalage ;
		int xmax = GameBoard.instance.gridWidthInHexes;
		int ymax = GameBoard.instance.gridHeightInHexes;
		
		for (int x = -1*xmax/2; x <= 1*xmax/2; x++)
		{
			if ((xmax-x)%2==0){
				decalage = 0;
			}
			else{
				decalage = 1;
			}
			for (int y = -1*ymax/2; y <= 1*ymax/2-decalage; y++)
			{
				int type = Mathf.RoundToInt (UnityEngine.Random.Range (1,25));
				if (type>4){
					type = 0 ;
				}
				photonView.RPC("AddTileToList",PhotonTargets.AllBuffered,x,y,type);
			}
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
		StartCoroutine(GameBoard.instance.setUser(loginName));
		nbPlayers++;
	}


	[RPC]
	void StartFight()
	{
		GameBoard.instance.StartFight();
	}

	[RPC]
	void AddTileToList(int x, int y, int type)
	{
		GameBoard.instance.addTile(x, y, type);
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
		isFirstPlayer = true ;
	}
	
	void OnReceivedRoomListUpdate()
	{
		//roomsList = PhotonNetwork.GetRoomList();
	}

	void OnJoinedRoom()
	{
		Debug.Log("Connected to Room");
		if (!isReconnecting){
			if (isFirstPlayer){
				photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);
				initGrid();
			}
			else{
				photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);
				Camera.main.transform.localRotation=Quaternion.Euler(30,0,180);
				Camera.main.transform.localPosition=new Vector3(0,5.75f,-10f);
			}
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
