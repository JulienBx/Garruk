using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyScript : MonoBehaviour {

	public string gameName = "oojump_garruk";
	private const string typeName = "oojump_garruk";
	
	private bool isConnecting = false;
	private bool isConnected = false;
	private List<string> playersName = new List<string>();
	private HostData[] hostList;
	public GUIStyle style;

	void awake()
	{
		RefreshHostList();
	}
	void start()
	{
		RefreshHostList();
		if(networkView.isMine)
		{
			GetComponent<Camera>().enabled = true;
		}
		else{
			GetComponent<Camera>().enabled = false;
		}
		style.normal.textColor = Color.red;
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
					//GUI.contentColor = Color.red;
					GUI.Label(new Rect(10, i * 30, 100, 50), name, style);
				}
				else
				{
					GUI.Label(new Rect(10, i * 30, 100, 50), name);
				}
			}
		}
	}
	
	private void StartServer()
	{
		Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}
	
	void OnServerInitialized()
	{
		Debug.Log("serveur initialisé");
		networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, ApplicationModel.username);
	}
	
	
	void Update()
	{
		if (!Network.isClient && !Network.isServer) {
			RefreshHostList();
		}
	}
	
	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}
	
	
	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
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
					StartServer();
				}
			}
		}
	}

	void OnConnectedToServer()
	{
		Debug.Log("client connecté");
		networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, ApplicationModel.username);
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		playersName.Remove (ApplicationModel.username);
	}
	
	[RPC]
	void AddPlayerToList(string loginName)
	{
		playersName.Add(loginName);
	}

}
