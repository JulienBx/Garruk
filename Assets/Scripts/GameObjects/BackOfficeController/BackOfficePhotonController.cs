using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;

public class BackOfficePhotonController : Photon.MonoBehaviour 
{
	public const string roomNamePrefix = "GarrukGame";
	private int nbPlayersReady;
	private int nbPlayersInRoom;
	private int deckLoaded;
	float waitingTime ; 
	float limitTime = 5f ;
	bool isWaiting ;

    private string URLInitiliazeGame = ApplicationModel.host + "initialize_game.php";

	void Update()
	{
		if(this.isWaiting){
			this.addWaitingTime(Time.deltaTime);
		}
	}

	public void addWaitingTime(float f){
		this.waitingTime += f ;
		if(waitingTime>limitTime){
			ApplicationModel.player.ToLaunchGameIA  = true ;
			this.startGame();
		}
	}

	public void leaveRoom()
	{
		PhotonNetwork.LeaveRoom ();
	}

	public void joinRandomRoom()
	{
		this.nbPlayersInRoom = 0;
		this.nbPlayersReady = 0;
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);    
		string sqlLobbyFilter = "C0 = " + ApplicationModel.player.ChosenGameType;
		ApplicationModel.player.IsFirstPlayer = false;
		PhotonNetwork.JoinRandomRoom(null, 0, ExitGames.Client.Photon.MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	}

	void OnPhotonRandomJoinFailed()
	{
        Debug.Log("Can't join random room! - creating a new room");
        StartCoroutine(this.CreateNewRoom ());
	}
	public IEnumerator CreateNewRoom()
	{
        ApplicationModel.player.IsFirstPlayer = true;
        ApplicationModel.player.ToLaunchGameIA  = false ;
        this.nbPlayersInRoom = 0;
		this.nbPlayersReady = 0;
        RoomOptions newRoomOptions = new RoomOptions();
        newRoomOptions.isOpen = true;
        newRoomOptions.isVisible = true;
        newRoomOptions.maxPlayers = 2;
        newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", ApplicationModel.player.ChosenGameType } }; // CO pour une partie simple
        newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
        TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);
        PhotonNetwork.CreateRoom(roomNamePrefix + Guid.NewGuid().ToString("N"), newRoomOptions, sqlLobby);
        if(!ApplicationModel.player.ToLaunchGameTutorial)
        {
            yield return StartCoroutine(this.initializeGame());
        }
		if(ApplicationModel.player.ChosenGameType<=20)
		{
			this.isWaiting = true ;
		}
        yield break;
	}
	
	void OnJoinedRoom()
	{
		photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.player.Username, ApplicationModel.player.SelectedDeckId, ApplicationModel.player.IsFirstPlayer, ApplicationModel.currentGameId, ApplicationModel.player.RankingPoints);
	}
	
	[PunRPC]
	IEnumerator AddPlayerToList(int id, string loginName, int selectedDeckId, bool isFirstPlayer, int currentGameId, int rankingPoints)
	{
		if(ApplicationModel.player.ToLaunchGameTutorial)
		{
			this.CreateTutorialDeck();
			this.startGame();
			yield break;
		}
		else
		{
			this.nbPlayersInRoom++;
            if(this.nbPlayersInRoom==2)
            {
                this.isWaiting=false;
            }
            Deck deck;
			deck = new Deck(selectedDeckId);
			yield return StartCoroutine(deck.RetrieveCards());
			if(deck.Error!="")
			{
				this.OnDisconnectedFromPhoton();
			}
            if(isFirstPlayer)
            {
                ApplicationModel.currentGameId=currentGameId;
            }
			if (ApplicationModel.player.IsFirstPlayer == isFirstPlayer)
			{
				ApplicationModel.myPlayerName=loginName;
                ApplicationModel.player.RankingPoints=rankingPoints;
				ApplicationModel.player.MyDeck=deck;
				print("deck :"+ApplicationModel.myPlayerName);
			} 
			else
			{
				ApplicationModel.hisPlayerName=loginName;
                ApplicationModel.hisRankingPoints=rankingPoints;
				ApplicationModel.opponentDeck=deck;
				print("deck :"+ApplicationModel.hisPlayerName);
			}
			this.nbPlayersReady++;
			if(this.nbPlayersReady==2)
			{
                this.startGame();
			}
		}
	}
	private void startGame()
	{
		if(ApplicationModel.player.IsFirstPlayer==true)
		{
			PhotonNetwork.room.open = false;
		}
		SoundController.instance.playMusic(new int[]{3,4});
		if(ApplicationModel.player.ToLaunchGameTutorial || ApplicationModel.player.ToLaunchGameIA)
		{
			SceneManager.LoadScene("Game");
		}
		else
		{
			BackOfficeController.instance.launchPreMatchLoadingScreen();
		}
	}
	void OnDisconnectedFromPhoton()
	{
		ServerController.instance.lostConnection();
	}
	private void CreateTutorialDeck()
	{
		ApplicationModel.myPlayerName=ApplicationModel.player.Username;
		ApplicationModel.hisPlayerName="Garruk";
		ApplicationModel.opponentDeck=new Deck();
		ApplicationModel.opponentDeck.cards=new List<Card>();

		List<Skill> skills = new List<Skill>();
		skills.Add (new Skill("Tank", 70, 1, 1, 2, 0, "", 0, 0));
		skills.Add (new Skill("Attaque 360", 17, 1, 2, 6, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.opponentDeck.cards.Add(new Card(-1, "Cartor", 35, 2, 0, 7, 16, skills));
		
		skills = new List<Skill>();
		skills.Add (new Skill("Leader", 76, 1, 1, 3, 0, "", 0, 0));
		skills.Add (new Skill("PistoSoin", 2, 1, 1, 1, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.opponentDeck.cards.Add(new Card(-1, "Myst", 24, 1, 0, 6, 11, skills));
				
		skills = new List<Skill>();
		skills.Add (new Skill("Rapide", 71, 1, 1, 4, 0, "", 0, 0));
		skills.Add (new Skill("Massue", 63, 1, 1, 1, 0, "", 0, 100));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.opponentDeck.cards.Add(new Card(-1, "Alien", 38, 2, 0, 3, 21, skills));
				
		skills = new List<Skill>();
		skills.Add (new Skill("Tank", 70, 1, 1, 2, 0, "", 0, 0));
		skills.Add (new Skill("Attaque 360", 17, 1, 2, 6, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.opponentDeck.cards.Add(new Card(-1, "Psycho", 42, 2, 0, 2, 17, skills));

		ApplicationModel.player.MyDeck=new Deck();
		ApplicationModel.player.MyDeck.cards=new List<Card>();

		skills = new List<Skill>();
		skills.Add (new Skill("Lâche", 65, 1, 1, 2, 0, "", 0, 0));
		skills.Add (new Skill("Vitamines", 6, 1, 2, 6, 0, "", 0, 100));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.player.MyDeck.cards.Add(new Card(-1, "Flash", 35, 2, 0, 7, 16, skills));
		
		skills = new List<Skill>();
		skills.Add (new Skill("Paladin", 73, 1, 1, 3, 0, "", 0, 0));
		skills.Add (new Skill("PistoSoin", 2, 1, 1, 6, 0, "", 0, 100));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.player.MyDeck.cards.Add(new Card(-1, "Arthur", 51, 1, 0, 3, 14, skills));
				
		skills = new List<Skill>();
		skills.Add (new Skill("Cuirassé", 70, 1, 1, 4, 0, "", 0, 0));
		skills.Add (new Skill("Attaque 360", 17, 1, 1, 8, 0, "", 0, 100));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.player.MyDeck.cards.Add(new Card(-1, "Psycho", 52, 2, 0, 3, 28, skills));
				
		skills = new List<Skill>();
		skills.Add (new Skill("Agile", 66, 1, 1, 2, 0, "", 0, 0));
		skills.Add (new Skill("Assassinat", 18, 1, 2, 10, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.player.MyDeck.cards.Add(new Card(-1, "Slayer", 35, 2, 0, 3, 16, skills));
	}

	private void CreateIADeck()
	{
		ApplicationModel.myPlayerName=ApplicationModel.player.Username;
		ApplicationModel.hisPlayerName="Garruk";
		ApplicationModel.opponentDeck=new Deck();
		ApplicationModel.opponentDeck.cards=new List<Card>();

		List<Skill> skills = new List<Skill>();
		skills.Add (new Skill("Tank", 70, 1, 1, 2, 0, "", 0, 0));
		skills.Add (new Skill("Attaque 360", 17, 1, 2, 6, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.opponentDeck.cards.Add(new Card(-1, "Cartor", 35, 2, 0, 7, 16, skills));
		
		skills = new List<Skill>();
		skills.Add (new Skill("Leader", 76, 1, 1, 3, 0, "", 0, 0));
		skills.Add (new Skill("PistoSoin", 2, 1, 1, 1, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.opponentDeck.cards.Add(new Card(-1, "Myst", 24, 1, 0, 6, 11, skills));
				
		skills = new List<Skill>();
		skills.Add (new Skill("Rapide", 71, 1, 1, 4, 0, "", 0, 0));
		skills.Add (new Skill("Massue", 63, 1, 1, 1, 0, "", 0, 100));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.opponentDeck.cards.Add(new Card(-1, "Alien", 38, 2, 0, 3, 21, skills));
				
		skills = new List<Skill>();
		skills.Add (new Skill("Tank", 70, 1, 1, 2, 0, "", 0, 0));
		skills.Add (new Skill("Attaque 360", 17, 1, 2, 6, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.opponentDeck.cards.Add(new Card(-1, "Psycho", 42, 2, 0, 2, 17, skills));

		ApplicationModel.player.MyDeck=new Deck();
		ApplicationModel.player.MyDeck.cards=new List<Card>();

		skills = new List<Skill>();
		skills.Add (new Skill("Lâche", 65, 1, 1, 2, 0, "", 0, 0));
		skills.Add (new Skill("Vitamines", 6, 1, 2, 6, 0, "", 0, 100));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.player.MyDeck.cards.Add(new Card(-1, "Flash", 35, 2, 0, 7, 16, skills));
		
		skills = new List<Skill>();
		skills.Add (new Skill("Paladin", 73, 1, 1, 3, 0, "", 0, 0));
		skills.Add (new Skill("PistoSoin", 2, 1, 1, 6, 0, "", 0, 100));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.player.MyDeck.cards.Add(new Card(-1, "Arthur", 51, 1, 0, 3, 14, skills));
				
		skills = new List<Skill>();
		skills.Add (new Skill("Cuirassé", 70, 1, 1, 4, 0, "", 0, 0));
		skills.Add (new Skill("Attaque 360", 17, 1, 1, 8, 0, "", 0, 100));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.player.MyDeck.cards.Add(new Card(-1, "Psycho", 52, 2, 0, 3, 28, skills));
				
		skills = new List<Skill>();
		skills.Add (new Skill("Agile", 66, 1, 1, 2, 0, "", 0, 0));
		skills.Add (new Skill("Assassinat", 18, 1, 2, 10, 0, "", 0, 80));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
		ApplicationModel.player.MyDeck.cards.Add(new Card(-1, "Slayer", 35, 2, 0, 3, 16, skills));
	}
    private IEnumerator initializeGame()
    {
        WWWForm form = new WWWForm();                               // Création de la connexion
        form.AddField("myform_hash", ApplicationModel.hash);        // hashcode de sécurité, doit etre identique à celui sur le serveur
        form.AddField("myform_nick", ApplicationModel.player.Username);    // Pseudo de l'utilisateur connecté

        ServerController.instance.setRequest(URLInitiliazeGame, form);
        yield return ServerController.instance.StartCoroutine("executeRequest");
        
        if(ServerController.instance.getError()=="")
        {
            string result = ServerController.instance.getResult();
            ApplicationModel.currentGameId=System.Convert.ToInt32(result);
        }
        else
        {
            Debug.Log(ServerController.instance.getError());
            ServerController.instance.lostConnection();
        }
    }
}

