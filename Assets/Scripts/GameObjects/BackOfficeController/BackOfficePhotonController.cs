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
	float waitingTime = 0f ; 
	float limitTime = 2f ;
	bool isWaiting ;
	public AsyncOperation async ;

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
			isWaiting = false ;
			ApplicationModel.player.ToLaunchGameIA  = true ;
			this.CreateIADeck();
			this.startGame();
		}
	}

	public void leaveRoom()
	{
		this.isWaiting = false ;
		this.waitingTime = 0f;
		PhotonNetwork.room.open = false;
		print("Je ferme la room LEAVE");

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

    public void joinLeavedRoom()
    {
        
    }

	void OnPhotonRandomJoinFailed()
	{
        if(ApplicationModel.player.HasToJoinLeavedRoom)
        {
            BackOfficeController.instance.loadScene("NewHomePage");
        }
        else
        {
            Debug.Log("Can't join random room! - creating a new room");
            StartCoroutine(this.CreateNewRoom ());
        }
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
			this.startLoadingScene();
		}
		yield break;
	}

	public void startLoadingScene(){
		StartCoroutine("loadGame");
	}

	IEnumerator loadGame(){
		this.async = SceneManager.LoadSceneAsync("Game");
		this.async.allowSceneActivation = false ;
		yield return async ;
	}
	
	void OnJoinedRoom()
	{
        if(ApplicationModel.player.HasToJoinLeavedRoom)
        {
        }
        else
        {
            photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.player.Username, ApplicationModel.player.SelectedDeckId, ApplicationModel.player.IsFirstPlayer, ApplicationModel.currentGameId, ApplicationModel.player.RankingPoints);
		    print("Je rejoins une nouvelle room");
		    BackOfficeController.instance.displayLoadingScreenButton(true);
        }
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
				BackOfficeController.instance.toDisconnect();
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
				ApplicationModel.gameRoomId	=PhotonNetwork.room.name;
                this.startGame();
			}
		}
	}
	private void startGame()
	{
		if(ApplicationModel.player.IsFirstPlayer==true)
		{
			PhotonNetwork.room.open = false;
			print("Je ferme la room START");
		}
		SoundController.instance.playMusic(new int[]{3,4});
		if(ApplicationModel.player.ToLaunchGameTutorial)
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
		if(!ApplicationModel.player.ToDeconnect)
		{
			ApplicationModel.player.HasLostConnection=true;
		}
        BackOfficeController.instance.loadScene("Authentication");
	}
	private void storeGameInformations()
	{
		
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
		ApplicationModel.hisPlayerName="IA";
		ApplicationModel.opponentDeck=new Deck();
		ApplicationModel.opponentDeck.cards=new List<Card>();
		int fixedIDType = -1;
		int cardType ;
		List<Skill> skills ;
		int nbSkills ;
		int compteurSkills;
		int idSkill=-1;
		bool hasFoundSkill ;
		List<int> passive = new List<int>() ;

		int[,] passiveSkills = new int[10,4];
		passiveSkills[0,0]=72;
		passiveSkills[0,1]=73;
		passiveSkills[0,2]=75;
		passiveSkills[0,3]=76;
		passiveSkills[1,0]=64;
		passiveSkills[1,1]=65;
		passiveSkills[1,2]=66;
		passiveSkills[1,3]=67;
		passiveSkills[2,0]=68;
		passiveSkills[2,1]=69;
		passiveSkills[2,2]=70;
		passiveSkills[2,3]=71;
		passiveSkills[3,0]=32;
		passiveSkills[3,1]=33;
		passiveSkills[3,2]=34;
		passiveSkills[3,3]=35;
		passiveSkills[4,0]=51;
		passiveSkills[4,1]=0;
		passiveSkills[4,2]=0;
		passiveSkills[4,3]=0;
		passiveSkills[5,0]=0;
		passiveSkills[5,1]=0;
		passiveSkills[5,2]=0;
		passiveSkills[5,3]=0;
		passiveSkills[6,0]=138;
		passiveSkills[6,1]=139;
		passiveSkills[6,2]=140;
		passiveSkills[6,3]=141;
		passiveSkills[7,0]=110;
		passiveSkills[7,1]=111;
		passiveSkills[7,2]=112;
		passiveSkills[7,3]=113;
		passiveSkills[8,0]=0;
		passiveSkills[8,1]=0;
		passiveSkills[8,2]=0;
		passiveSkills[8,3]=0;
		passiveSkills[9,0]=43;
		passiveSkills[9,1]=0;
		passiveSkills[9,2]=0;
		passiveSkills[9,3]=0;

		int[,] activeSkills = new int[10,10]{{2,3,4,5,6,7,39,56,57,94},{8,9,10,11,12,13,14,15,58,59},{16,17,18,19,20,21,63,91,92,93},{22,23,24,25,26,27,28,29,30,31},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{40,41,42,105,128,129,130,131,132,133},{95,101,100,102,103,104,106,107,108,109},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0}};
	
		if(ApplicationModel.player.ChosenGameType>0 && ApplicationModel.player.ChosenGameType<11){
			fixedIDType = ApplicationModel.player.ChosenGameType-1;
		}
		 
		for (int i = 0 ; i < 4 ; i++){
			if(fixedIDType==-1){
				cardType = UnityEngine.Random.Range(1,11);
			}
			else{
				cardType = fixedIDType;
			}

			skills = new List<Skill>();
			hasFoundSkill = true ;
			while (hasFoundSkill){
				idSkill = passiveSkills[cardType,UnityEngine.Random.Range(0,4)];
				hasFoundSkill = false ;
				for(int l = 0 ; l < passive.Count ; l++){
					if(idSkill==passive[l]){
						hasFoundSkill = true ;
					}
				}
			}

			passive.Add(idSkill);
			skills.Add(new Skill(idSkill, 11-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,101)))));

			nbSkills = UnityEngine.Random.Range(1,4);
			compteurSkills=0 ; 

			for(int j = 0 ; j < nbSkills ; j++){
				hasFoundSkill = true ;
				while (hasFoundSkill){
					idSkill = activeSkills[cardType, UnityEngine.Random.Range(0,10)];
					hasFoundSkill = false ;
					if(idSkill!=0){
						for (int k = 0 ; k < compteurSkills ; k++){
							if(skills[1+k].Id==idSkill){
								hasFoundSkill = true ;
							}
						}
					}
				}
				skills.Add(new Skill(idSkill, 11-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,101)))));
				compteurSkills++;
			}
			ApplicationModel.opponentDeck.cards.Add(new GameCard(WordingCardName.getName(skills[0].Id), this.getRandomLife(cardType), cardType, this.getRandomMove(cardType), this.getRandomAttack(cardType), skills,i));
		}
	}

	private int getRandomLife(int cardType){
		if(cardType==0){
			return (40+Mathf.RoundToInt(20*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==1){
			return (30+Mathf.RoundToInt(20*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==2){
			return (50+Mathf.RoundToInt(20*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==3){
			return (20+Mathf.RoundToInt(20*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==4){
			return (20+Mathf.RoundToInt(20*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==5){
			return (60+Mathf.RoundToInt(20*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==6){
			return (40+Mathf.RoundToInt(20*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==7){
			return (30+Mathf.RoundToInt(20*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==8){
			return (50+Mathf.RoundToInt(20*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else{
			return (40+Mathf.RoundToInt(20*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
	}

	private int getRandomAttack(int cardType){
		if(cardType==0){
			return (10+Mathf.RoundToInt(15*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==1){
			return (15+Mathf.RoundToInt(15*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==2){
			return (20+Mathf.RoundToInt(15*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==3){
			return (5+Mathf.RoundToInt(15*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==4){
			return (10+Mathf.RoundToInt(15*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==5){
			return (15+Mathf.RoundToInt(15*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==6){
			return (10+Mathf.RoundToInt(15*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==7){
			return (5+Mathf.RoundToInt(15*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else if(cardType==8){
			return (10+Mathf.RoundToInt(15*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
		else{
			return (15+Mathf.RoundToInt(15*(101-Mathf.CeilToInt(Mathf.Sqrt(UnityEngine.Random.Range(1,10001))))/100f));
		}
	}

	private int getRandomMove(int cardType){
		if(cardType==0){
			return 3;
		}
		else if(cardType==1){
			return 5;
		}
		else if(cardType==2){
			return 2;
		}
		else if(cardType==3){
			return 4;
		}
		else if(cardType==4){
			return 5;
		}
		else if(cardType==5){
			return 2;
		}
		else if(cardType==6){
			return 3;
		}
		else if(cardType==7){
			return 3;
		}
		else if(cardType==8){
			return 2;
		}
		else{
			return 4;
		}
	}

    private IEnumerator initializeGame()
    {
        WWWForm form = new WWWForm();                               		// Création de la connexion
        form.AddField("myform_hash", ApplicationModel.hash);        		// hashcode de sécurité, doit etre identique à celui sur le serveur
        form.AddField("myform_nick", ApplicationModel.player.Username);     // Pseudo de l'utilisateur connecté

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

