using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScript : MonoBehaviour {

	public string gameName = ApplicationModel.username;
	public float WaitingTimeToRefresh;
	public GameObject card;

	private const string typeName = "oojump_garruk_game";
	private bool isConnecting = false;
	private bool connected = false;
	private bool attemptToConnect = false;
	private List<string> playersName = new List<string>();
	private HostData[] hostList;
	public string labelText = "Placer vos héros sur le champ de bataille";
	public string labelMessage = "";
	private bool hasClicked = false;
	public static GameScript instance;

	void Awake()
	{
		MasterServer.ClearHostList();
		instance = this;
	}

	void Start()
	{	
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(530, 0, 800, 50), labelMessage);
		if (playersName.Count > 1)
		{

			GUI.Label(new Rect(10, 0, 500, 50), labelText);
			if (!hasClicked && GUI.Button(new Rect(10, 20, 200, 35), "Commencer le combat"))
			{
				hasClicked = true;
				labelText = "En attente d'actions de l'autre joueur";
				networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, ApplicationModel.username);
				networkView.RPC("StartFight", RPCMode.AllBuffered);
			}
		}
		else
		{
			GUI.Label(new Rect(10, 0, 500, 50), "En attente d'autres joueurs");
		}
		if (GUI.Button(new Rect(220, 20, 150, 35), "Quitter le match"))
		{
			Network.Disconnect();
			MasterServer.UnregisterHost();
		}

	}
	
	void Update()
	{
		if (!Network.isClient && !Network.isServer) {
			MasterServer.RequestHostList(typeName);
		}
	}

	private void StartServer()
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

	[RPC]
	void AddPlayerToList(string loginName)
	{
		playersName.Add(loginName);
	}
	[RPC]
	void StartFight()
	{
		GameBoard.instance.StartFight();
	}
}
