using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameController : Photon.MonoBehaviour 
{	
	public static GameController instance;
	public bool isFirstPlayer = false;
	public bool isGameOver = false;
	public bool isReconnecting;
	private int nbPlayerReadyToFight = 0;
	public static Deck deck;

	const string roomNamePrefix = "GarrukGame";
	int nbPlayers = 0;
	User[] users;
	GameView gameView; 

	//string URLStat = ApplicationModel.dev + "updateResult.php";
	
	
	void Awake()
	{
		instance = this;
		gameView = GameObject.Find("Game Board").GetComponent<GameView>() as GameView;

	}
	
	void Start()
	{	
		users = new User[2];
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
		PhotonNetwork.autoCleanUpPlayerObjects = false ;
	}	
	
	public void EndOfGame(int player)
	{
		isGameOver = true;
	}
	
	private IEnumerator returnToLobby()
	{
		if (gameView.MyPlayerNumber == 1)
		{
			yield return new WaitForSeconds(5);
		} 
		else
		{
			yield return new WaitForSeconds(7);
		}
		PhotonNetwork.Disconnect();
	}
	
	public void addStat(int user1, int user2)
	{
		//StartCoroutine(sendStat(playersName[user1], playersName[user2]));
	}
	
	IEnumerator sendStat(string user1, string user2)
	{
		//		WWWForm form = new WWWForm(); 								// Création de la connexion
		//		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		//		form.AddField("myform_nick1", user1); 	                    // Pseudo de l'utilisateur victorieux
		//		form.AddField("myform_nick2", user2); 	                    // Pseudo de l'autre utilisateur
		//		
		//		WWW w = new WWW(URLStat, form); 							// On envoie le formulaire à l'url sur le serveur 
		//		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		//		if (w.error != null)
		//		{
		//			print(w.error); 										// donne l'erreur eventuelle
		//		} else
		//		{
		//			print(w.text);
		//		}
		yield break ;
	}
	
	// RPC
	
	[RPC]
	IEnumerator AddPlayerToList(int id, string loginName)
	{
		print ("J'ajoute " + loginName);
		if (ApplicationModel.username == loginName)
		{
			if (nbPlayers == 0)
			{
				isFirstPlayer = true;
				if (!isReconnecting)
				{
					gameView.initGrid();
				}
			}
			else
			{
				isFirstPlayer = false;
				Camera.main.transform.localRotation = Quaternion.Euler(30, 0, 180);
				Camera.main.transform.localPosition = new Vector3(0, 5.75f, -10f);
			}
			StartCoroutine(AddCardToBoard());
		}
		
		users[nbPlayers] = new User(loginName);
		yield return StartCoroutine(users[nbPlayers].retrievePicture());
		nbPlayers++;
		if (ApplicationModel.username == loginName)
		{
			gameView.bottomUserPicture = users[nbPlayers].texture;
			gameView.bottomUserName = users[nbPlayers].Username;
		}
		else
		{
			gameView.topUserPicture = users[nbPlayers].texture;
			gameView.topUserName = users[nbPlayers].Username;
		}
	}

	public IEnumerator AddCardToBoard()
	{
		deck = new Deck(ApplicationModel.username);
		yield return StartCoroutine(deck.LoadSelectedDeck());
		yield return StartCoroutine(deck.RetrieveCards());
		
		GameView.instance.ArrangeCards(GameController.instance.isFirstPlayer, deck);
	}

	[RPC]
	public void StartFight()
	{
		nbPlayerReadyToFight++;
		
		//		if (nbPlayerReadyToFight == 2)
		//		{  
		//			nbTurn = 1;
		//			TimeOfPositionning = false;
		//			GameTimeLine.instance.PlayingCard.transform.Find("Yellow Outline").renderer.enabled = true;
		//			if (GameTimeLine.instance.PlayingCard.ownerNumber == MyPlayerNumber)
		//			{
		//				GameScript.instance.labelText = "A vous de jouer";
		//			}
		//			else
		//			{
		//				GameScript.instance.labelText = "Au joueur adverse de jouer";
		//			}
		//		}
	}
	
	// Photon
	void OnJoinedLobby()
	{
		print("yo");
		gameView.nbPlayer = 2;
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);    
		string sqlLobbyFilter = "C0 = " + ApplicationModel.gameType;
		PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	}
	
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room! - creating a new room");
		print("yo");
		RoomOptions newRoomOptions = new RoomOptions();
		newRoomOptions.isOpen = true;
		newRoomOptions.isVisible = true;
		newRoomOptions.maxPlayers = 2;
		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", ApplicationModel.gameType } }; // CO pour une partie simple
		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
		
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);
		PhotonNetwork.CreateRoom(roomNamePrefix + Guid.NewGuid().ToString("N"), newRoomOptions, sqlLobby);
		gameView.nbPlayer = 1;
	}
	
	void OnJoinedRoom()
	{
		Debug.Log("Connected to a room");
		print("yo");
		if (!isReconnecting)
		{
			photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);
		}
		else
		{
			Debug.Log("Reconnecting...");
		}
	}
	
	
	void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		//RemovePlayerFromList(player.ID);
	}
	
	void OnDisconnectedFromPhoton()
	{
		Application.LoadLevel("LobbyPage");
		
	}
}
