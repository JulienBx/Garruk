using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameScript : Photon.MonoBehaviour {

	public string gameName = ApplicationModel.username;
	public float WaitingTimeToRefresh;
	public GameObject card;

	//private const string typeName = "oojump_garruk_game";
//	private bool isConnecting = false;
//	private bool connected = false;
//	private bool attemptToConnect = false;
	private Dictionary<int, string> playersName = new Dictionary<int, string>();
	private HostData[] hostList;
	public string labelText = "Placer vos héros sur le champ de bataille";
	public string labelInfo = "En attente d'autres joueurs";

	public bool gameOver = false;

	public string labelMessage = "";
	private bool hasClicked = false;
	public static GameScript instance;

	private const string roomName = "GarrukGame";
	private RoomInfo[] roomsList;

	void Awake()
	{
		MasterServer.ClearHostList();
		instance = this;
	}

	void Start()
	{	
		PhotonNetwork.ConnectUsingSettings("0.1");
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
		}
		else
		{
			GUI.Label(new Rect(10, 0, 500, 50), labelInfo);
		}
		if (GUI.Button(new Rect(220, 20, 150, 35), "Quitter le match"))
		{
			Network.Disconnect();
			MasterServer.UnregisterHost();
		}

	}
	
	void Update()
	{
//		if (!Network.isClient && !Network.isServer) {
//			MasterServer.RequestHostList(typeName);
//		}

	}

	/*private void StartServer()
	{
		Network.InitializeServer(1, 25002, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName, "Open");
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	void OnDisconnectedFromServer()
	{
		Application.LoadLevel("LobbyPage");
	}

	private IEnumerator RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
		yield return new WaitForSeconds(WaitingTimeToRefresh);
	}

	public IEnumerator JoinGame()
	{
		yield return StartCoroutine(RefreshHostList());
		hostList = MasterServer.PollHostList();
		if (hostList.Length > 0 )
		{
			foreach(HostData hd in hostList)
			{
				if (!hd.gameName.Equals("lobby") && hd.comment.Equals("Open"))
				{
					attemptToConnect = true;
					JoinServer(hd);
					break;
				}
			}
			if (!connected && !attemptToConnect)
			{
				StartServer();
			}
		}
		else
		{
			StartServer();
		}
	}

	// Messages
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived) 
		{
			if (!Network.isClient && !Network.isServer) 
			{
				if (!isConnecting)
				{
					isConnecting = true;
					StartCoroutine(JoinGame());
				}
			}
		}
	}
	void OnServerInitialized()
	{
		Debug.Log("serveur initialisé");
	}

	void OnConnectedToServer()
	{
		connected = true;
		GameTile.AvailableStartingColumns.Add("Column7");
		GameTile.AvailableStartingColumns.Add("Column8");
		Debug.Log("client connecté");
		networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, ApplicationModel.username);
		GameBoard gb = GameObject.Find("Game Board").GetComponent<GameBoard> () as GameBoard;
		StartCoroutine(gb.AddCardToBoard());
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		MasterServer.RegisterHost(typeName, gameName, "Closed");
		GameTile.AvailableStartingColumns.Add("Column1");
		GameTile.AvailableStartingColumns.Add("Column2");
		networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, ApplicationModel.username);
		GameBoard gb = GameObject.Find("Game Board").GetComponent<GameBoard> () as GameBoard;
		StartCoroutine(gb.AddCardToBoard());
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		labelInfo = "l'utilisateur a quitté le match, en attente d'autres utilisateurs";
		playersName.Remove (ApplicationModel.username);
	}

	void OnFailedToConnectToMasterServer(NetworkConnectionError info) {
		Debug.Log("Could not connect to master server: " + info);
		if (!Network.isClient && !Network.isServer) 
		{
			isConnecting = false;
			attemptToConnect = false;
			StartCoroutine(JoinGame());
		}
	}
*/
	void OnJoinedLobby()
	{
		GameBoard.instance.nbPlayer = 2;
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);    
		string sqlLobbyFilter = "C0 = 1";
		PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	}
	
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		RoomOptions newRoomOptions = new RoomOptions();
		newRoomOptions.isOpen = true;
		newRoomOptions.isVisible = true;
		newRoomOptions.maxPlayers = 2;
		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", 1 } }; // CO pour une partie simple
		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
		

		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);
		PhotonNetwork.CreateRoom(roomName + Guid.NewGuid().ToString("N"), newRoomOptions, sqlLobby);
		GameBoard.instance.nbPlayer = 1;
		
	}
	
	void OnReceivedRoomListUpdate()
	{
		roomsList = PhotonNetwork.GetRoomList();
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


	public void EndOfGame(int player)
	{
		gameOver = true;
		labelText = "Joueur " + player + " a gagné";
	}

	private IEnumerator returnToLobby()
	{
		yield return new WaitForSeconds(5);
		Application.LoadLevel("LobbyPage");
	}

	void RemovePlayerFromList(int id)
	{
		playersName.Remove(id);
	}

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
}
