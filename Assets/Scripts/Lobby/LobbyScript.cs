using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyScript : MonoBehaviour {

	public string gameName = "lobby";
	public GUIStyle style;

	private const string typeName = "oojump_garruk";
	private bool isConnecting = false;
	private List<string> playersName = new List<string>();
	private HostData[] hostList;
	private bool attemptToPlay = false;

	void Awake()
	{
		RefreshHostList();
	}

	void Start()
	{
		style.normal.textColor = Color.red;
	}

	void Update()
	{
		if (!Network.isClient && !Network.isServer) {
			RefreshHostList();
		}
	}

	void OnGUI()
	{
		if (playersName.Count > 0)
		{
			int i = 0;
			GUI.Label(new Rect(10, 0, 500, 50), "Liste des utilisateurs connectés");
			foreach(string name in playersName)
			{
				i++;
				if (name.Equals(ApplicationModel.username))
				{
					GUI.Label(new Rect(10, i * 30, 100, 50), name, style);
				}
				else
				{
					GUI.Label(new Rect(10, i * 30, 100, 50), name);
				}
			}
		}
		if (GUI.Button(new Rect(500, 0, 200, 50), "rejoindre un match"))
	    {
			attemptToPlay = true;
			Network.Disconnect();
			MasterServer.UnregisterHost();

		}
	}
	
	private void StartServer()
	{
		Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	// RPC

	[RPC]
	void AddPlayerToList(string loginName)
	{
		playersName.Add(loginName);
	}

	// Messages

	void OnServerInitialized()
	{
		Debug.Log("serveur initialisé");
		networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, ApplicationModel.username);
	}

	void OnConnectedToServer()
	{
		Debug.Log("client connecté");
		networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, ApplicationModel.username);
	}

	void OnDisconnectedFromServer()
	{
		if (attemptToPlay)
		{

			Application.LoadLevel("GamePage");
		}
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		//playersName.Remove (ApplicationModel.username);
	}

	// MasterServerEvent
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (!attemptToPlay)
		{
			if (msEvent == MasterServerEvent.HostListReceived) 
			{
				if (!Network.isClient && !Network.isServer) {
					hostList = MasterServer.PollHostList();
					if (hostList.Length > 0 )
					{
						if (!isConnecting)
						{
							isConnecting = true;
							JoinServer(hostList[0]);
						}
					}
					else
					{
						playersName.Clear();
						StartServer();
					}
				}
			}
		}
	}
}
