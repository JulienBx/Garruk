using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScript : MonoBehaviour {

	public string gameName = ApplicationModel.username;
	public float WaitingTimeToRefresh;
	public GUIStyle style;

	private const string typeName = "oojump_garruk_game";
	private bool isConnecting = false;
	private bool connected = false;
	private List<string> playersName = new List<string>();
	private HostData[] hostList;


	void awake()
	{
		MasterServer.ClearHostList();
	}

	void start()
	{
		style.normal.textColor = Color.red;
	}
	
	void OnGUI()
	{
		if (playersName.Count > 1)
		{
			GUI.Label(new Rect(10, 0, 500, 50), "les deux joueurs sont connectés");
		}
		else
		{
			GUI.Label(new Rect(10, 0, 500, 50), "En attente d'autres joueurs");
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
					JoinServer(hd);
					connected = true;
				}
				if (!connected)
				{
					StartServer();
				}
			}
		}
		else
		{
			StartServer();
		}
	}
	
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
	
	void OnConnectedToServer()
	{
		Debug.Log("client connecté");
		networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, ApplicationModel.username);
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		MasterServer.RegisterHost(typeName, gameName, "Closed");
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		playersName.Remove (ApplicationModel.username);
	}

	void OnServerInitialized()
	{
		Debug.Log("serveur initialisé");
		networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, ApplicationModel.username);
	}
	
	[RPC]
	void AddPlayerToList(string loginName)
	{
		playersName.Add(loginName);
	}
}
	