using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;

public class PhotonController : Photon.MonoBehaviour 
{

	private GameObject preMatchScreen;

    public const string roomNamePrefix = "GarrukGame";
	public static PhotonController instance;
    private int nbDecksLoaded;
    private int nbPlayersInRoom;
    private int nbPlayersReady;
    float waitingTime = 0f ; 
    float limitTime = 10f ;
    bool isWaiting ;
    public AsyncOperation async ;
	private bool isInitialized;
	private string sceneName;
	private bool isLoadingScreenDisplayed;
	private bool isQuittingGame;
	private bool hasToJoinRoom;

    private string URLInitiliazeGame = ApplicationModel.host + "initialize_game.php";

    void Update()
    {
        if(this.isWaiting)
        {
            this.addWaitingTime(Time.deltaTime);
        }
        if(this.isQuittingGame)
        {
        	if(async.progress==1)
        	{
				this.isQuittingGame=false;
				this.hideLoadingScreen();
        	}
        }
    }
	public void initialize()
	{
		instance = this;
		DontDestroyOnLoad(this.gameObject);
		this.preMatchScreen=this.gameObject.transform.FindChild("PreMatchScreen").gameObject;
		this.isInitialized=true;
		this.isQuittingGame=false;
	}
	public void initializeGame()
	{
        this.joinRandomRoom();
	}
	public void joinRandomRoom()
    {
		this.displayLoadingScreen();
		if(ApplicationModel.player.ChosenGameType>20)
		{
			this.changeLoadingScreenLabel(WordingSocial.getReference(19));
            this.displayLoadingScreenButton(false);
		}
		else if(ApplicationModel.player.ChosenGameType<=20 && !ApplicationModel.player.ToLaunchGameTutorial)
		{
			this.changeLoadingScreenLabel (WordingGameModes.getReference(7));
		}
        print("Jessaye de joindre une room");
        this.hasToJoinRoom=false;
        this.isWaiting=false;
        this.nbPlayersReady=0;
        this.nbPlayersInRoom=0;
        this.waitingTime=0f;
        if(ApplicationModel.player.ToLaunchChallengeGame || ApplicationModel.player.ToLaunchGameTutorial)
        {
            this.displayLoadingScreenButton(false);
            this.CreateNewRoom();
        }
        else
        {
            TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);    
            string sqlLobbyFilter = "C0 = " + ApplicationModel.player.ChosenGameType;
            ApplicationModel.player.IsFirstPlayer = false;
            ApplicationModel.player.ToLaunchGameIA=false;
            PhotonNetwork.JoinRandomRoom(null, 0, ExitGames.Client.Photon.MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
        }
    }
	void OnPhotonRandomJoinFailed()
    {
        if(ApplicationModel.player.ChosenGameType>20)
        {
            Debug.Log("Can't join random room! - trying again");
            this.joinRandomRoom();
        }
        else
        {
            Debug.Log("Can't join random room! - creating a new room");
            this.CreateNewRoom ();
        }
    }
	void OnPhotonJoinRoomFailed()
    {
		Debug.Log("Can't join room! - starting again");
		this.hasToJoinRoom=true;
    }
    void OnJoinedLobby()
    {
    	Debug.Log("retour au lobby");
    	if(hasToJoinRoom)
    	{
    		this.joinRandomRoom();
    	}
    }
	public void CreateNewRoom()
    {
        print("Je crée une nouvelle Room");
        ApplicationModel.player.IsFirstPlayer = true;
        ApplicationModel.player.ToLaunchGameIA  = false ;
        this.nbPlayersReady=0;
        RoomOptions newRoomOptions = new RoomOptions();
        newRoomOptions.isOpen = true;
        newRoomOptions.isVisible = true;
        newRoomOptions.maxPlayers = 2;
        newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", ApplicationModel.player.ChosenGameType } }; // CO pour une partie simple
        newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
        TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);
        PhotonNetwork.CreateRoom(roomNamePrefix + Guid.NewGuid().ToString("N"), newRoomOptions, sqlLobby);
        if(ApplicationModel.player.ChosenGameType<=20 && !ApplicationModel.player.ToLaunchGameTutorial)
        {
            this.isWaiting = true ;
            this.waitingTime=0f;
        }
        if(ApplicationModel.player.ToLaunchChallengeGame=true)
        {
            ApplicationModel.player.ToLaunchChallengeGame=false;
        }
    }
    public void addWaitingTime(float f){
        
        this.waitingTime += f ;
        if(waitingTime>limitTime)
        {
            isWaiting = false ;
			this.waitingTime=0f;
			if(PhotonNetwork.room.playerCount<2)
	        {
				ApplicationModel.player.ToLaunchGameIA  = true ;
            	StartCoroutine(this.startIAGame());
	        }
        }
    }
    public void leaveRoom()
    {
    	this.isWaiting=false;
    	this.waitingTime=0f;
        PhotonNetwork.LeaveRoom ();
    }
    void OnJoinedRoom()
    {
    	print("j'ai rejoint une room");
        ApplicationModel.gameRoomId=PhotonNetwork.room.name;
		ApplicationModel.myPlayerName=ApplicationModel.player.Username;

		if(PhotonNetwork.room.playerCount==2)
		{
			print("il y a deux joueurs je ferme la room");
            PhotonNetwork.room.open = false;
            this.displayLoadingScreenButton(false);
        }

        if(!ApplicationModel.player.ToLaunchGameTutorial)
        {
        	this.addPlayerToListHandler();
            if(ApplicationModel.player.ChosenGameType<=20)
            {
        	    this.displayLoadingScreenButton(true);
            }
        }
        else
        {
			ApplicationModel.hisPlayerName="Garruk";
            this.hideLoadingScreen();
            GameView.instance.init();
        }
    }

    // ETAPE 1 -> Attendre 2 joueurs

    void addPlayerToListHandler()
    {
		photonView.RPC("AddPlayerToListRPC", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    void AddPlayerToListRPC()
    {
    	this.nbPlayersInRoom++;
    	if(this.nbPlayersInRoom==2)
    	{
    		Debug.Log("il y a deux joueurs je ferme la room");
    		this.displayLoadingScreenButton(false);
			PhotonNetwork.room.open = false;
			this.isWaiting=false;
			this.checkPlayersState();
		}
    }

    // ETAPE 2 -> Vérifier que personne n'a lancé l'IA

    void checkPlayersState()
    {
		photonView.RPC("CheckPlayerStateRPC", PhotonTargets.AllBuffered, ApplicationModel.player.ToLaunchGameIA, ApplicationModel.player.IsFirstPlayer);
    }

	[PunRPC]
    void CheckPlayerStateRPC(bool toLaunchGameIA, bool isFirstPlayer)
    {
		if(ApplicationModel.player.IsFirstPlayer!=isFirstPlayer)
		{
			if(!toLaunchGameIA)
			{
				this.getPlayerDataHandler();
			}
			else
			{
				Debug.Log("il y a l'IA, je quitte la room");
				this.leaveRoom();
        		this.hasToJoinRoom=true;
			}
		}
    }

    // ETAPE 3 -> Récupérer les infos de matchs

	void getPlayerDataHandler()
    {
		photonView.RPC("getPlayerDataRPC", PhotonTargets.AllBuffered, ApplicationModel.player.IsFirstPlayer, ApplicationModel.player.Username,ApplicationModel.player.Id,ApplicationModel.player.RankingPoints,ApplicationModel.player.SelectedDeckId);
    }

	[PunRPC]
	IEnumerator getPlayerDataRPC(bool isFirstPlayer, string playerName, int playerId, int playerRankingPoints, int playerSelectedDeckId)
    {
		if(ApplicationModel.player.IsFirstPlayer!=isFirstPlayer)
        {
			ApplicationModel.hisPlayerName=playerName;
			ApplicationModel.hisPlayerID=playerId;
			ApplicationModel.hisRankingPoints=playerRankingPoints;
			yield return StartCoroutine(this.initializeGame(ApplicationModel.player.IsFirstPlayer,false, playerId,playerRankingPoints,playerSelectedDeckId));
			this.getCurrentGameIdHandler();
        }
        else
        {
			yield break;
        }
    }

	void getCurrentGameIdHandler()
    {
		photonView.RPC("getCurrentGameIdRPC", PhotonTargets.AllBuffered, ApplicationModel.player.IsFirstPlayer, ApplicationModel.currentGameId);
    }

    // ETAPE 4 -> Transmettre le game ID et lancer le match

	[PunRPC]
	void getCurrentGameIdRPC(bool isFirstPlayer, int currentGameId)
    {
		if(isFirstPlayer)
        {
			ApplicationModel.currentGameId=currentGameId;
        }
        this.nbPlayersReady++;
        if(this.nbPlayersReady==2)
        {
			this.preMatchScreen.GetComponent<PreMatchScreenController>().launchPreMatchLoadingScreen();
        }
    }



	private IEnumerator startIAGame()
    {
		this.displayLoadingScreenButton(false);
		PhotonNetwork.room.open = false;
		this.CreateIADeck();
		yield return StartCoroutine(this.initializeGame(ApplicationModel.player.IsFirstPlayer,true,ApplicationModel.player.Id,ApplicationModel.player.RankingPoints,ApplicationModel.player.SelectedDeckId));
		this.preMatchScreen.GetComponent<PreMatchScreenController>().launchPreMatchLoadingScreen();
    }
    public void OnDisconnectedFromPhoton()
    {
        if(!ApplicationModel.player.ToDeconnect)
        {
            ApplicationModel.player.HasLostConnection=true;
            ApplicationModel.player.ToDeconnect=true;
        }
        SceneManager.LoadScene("Authentication");
    }
    private void CreateIADeck()
    {
    	print("Je crée le deck de l'IA");
        ApplicationModel.opponentDeck=new Deck();
        ApplicationModel.opponentDeck.cards=new List<Card>();
        int fixedIDType = -1;
        int cardType ;
        List<Skill> skills ;
        int nbSkills ;
        int compteurSkills;
        int idSkill=-1;
        bool hasFoundSkill ;
        int difficultyLevel = -1 ; 
        int randomTest ;
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
		randomTest = UnityEngine.Random.Range(1,11);

        if(ApplicationModel.player.ChosenGameType>0 && ApplicationModel.player.ChosenGameType<11){
            fixedIDType = ApplicationModel.player.ChosenGameType-1;
           	if(randomTest<8){
           		difficultyLevel = 1 ; 
           	}
			else if(randomTest<10){
           		difficultyLevel = 2 ; 
           	}
           	else{
				difficultyLevel = 3 ; 
           	}
        }
		else if(ApplicationModel.player.ChosenGameType==0){
			if(randomTest<6){
           		difficultyLevel = 1 ; 
           	}
			else if(randomTest<9){
           		difficultyLevel = 2 ; 
           	}
           	else{
				difficultyLevel = 3 ; 
           	}
        }
		else if(ApplicationModel.player.ChosenGameType<21){
			if(randomTest<ApplicationModel.player.ChosenGameType-12){
				difficultyLevel=2;
			}
			else{
				difficultyLevel=3;
			}
        }
         
        for (int i = 0 ; i < 4 ; i++){
            if(fixedIDType==-1){
                cardType = UnityEngine.Random.Range(0,10);
                while(cardType==4 || cardType==5 || cardType==8 || cardType==9){
                    cardType = UnityEngine.Random.Range(0,10);
                }
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
			if(difficultyLevel==1){
				if(randomTest>90){
					randomTest = 10;
				}
				else if(randomTest>80){
					randomTest = 9;
				}
				else if(randomTest>70){
					randomTest = 8;
				}
				else if(randomTest>60){
					randomTest = 7;
				}
				else if(randomTest>50){
					randomTest = 6;
				}
				else if(randomTest>40){
					randomTest = 5;
				}
				else if(randomTest>30){
					randomTest = 4;
				}
				else if(randomTest>20){
					randomTest = 3;
				}
				else if(randomTest>10){
					randomTest = 2;
				}
				else {
					randomTest = 1;
				}
			}
			else if(difficultyLevel==2){
				if(randomTest>85){
					randomTest = 10;
				}
				else if(randomTest>71){
					randomTest = 9;
				}
				else if(randomTest>58){
					randomTest = 8;
				}
				else if(randomTest>46){
					randomTest = 7;
				}
				else if(randomTest>35){
					randomTest = 6;
				}
				else if(randomTest>25){
					randomTest = 5;
				}
				else if(randomTest>16){
					randomTest = 4;
				}
				else if(randomTest>8){
					randomTest = 3;
				}
				else if(randomTest>3){
					randomTest = 2;
				}
				else {
					randomTest = 1;
				}
            }
			else if(difficultyLevel==3){
				if(randomTest>75){
					randomTest = 10;
				}
				else if(randomTest>60){
					randomTest = 9;
				}
				else if(randomTest>42){
					randomTest = 8;
				}
				else if(randomTest>32){
					randomTest = 7;
				}
				else if(randomTest>24){
					randomTest = 6;
				}
				else if(randomTest>18){
					randomTest = 5;
				}
				else if(randomTest>13){
					randomTest = 4;
				}
				else if(randomTest>9){
					randomTest = 3;
				}
				else if(randomTest>4){
					randomTest = 2;
				}
				else {
					randomTest = 1;
				}
            }
            skills.Add(new Skill(idSkill, randomTest));

            nbSkills = 0 ;
            if(difficultyLevel==1){
            	nbSkills = UnityEngine.Random.Range(1,4);
            }
			else if(difficultyLevel==2){
            	nbSkills = UnityEngine.Random.Range(2,4);
            }
			else if(difficultyLevel==3){
            	nbSkills = 3;
            }

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

				randomTest = UnityEngine.Random.Range(1,101);
                if(difficultyLevel==1){
					if(randomTest>90){
						randomTest = 10;
					}
					else if(randomTest>80){
						randomTest = 9;
					}
					else if(randomTest>70){
						randomTest = 8;
					}
					else if(randomTest>60){
						randomTest = 7;
					}
					else if(randomTest>50){
						randomTest = 6;
					}
					else if(randomTest>40){
						randomTest = 5;
					}
					else if(randomTest>30){
						randomTest = 4;
					}
					else if(randomTest>20){
						randomTest = 3;
					}
					else if(randomTest>10){
						randomTest = 2;
					}
					else {
						randomTest = 1;
					}
                }
				else if(difficultyLevel==2){
					if(randomTest>85){
						randomTest = 10;
					}
					else if(randomTest>71){
						randomTest = 9;
					}
					else if(randomTest>58){
						randomTest = 8;
					}
					else if(randomTest>46){
						randomTest = 7;
					}
					else if(randomTest>35){
						randomTest = 6;
					}
					else if(randomTest>25){
						randomTest = 5;
					}
					else if(randomTest>16){
						randomTest = 4;
					}
					else if(randomTest>8){
						randomTest = 3;
					}
					else if(randomTest>3){
						randomTest = 2;
					}
					else {
						randomTest = 1;
					}
                }
				else if(difficultyLevel==3){
					if(randomTest>75){
						randomTest = 10;
					}
					else if(randomTest>60){
						randomTest = 9;
					}
					else if(randomTest>42){
						randomTest = 8;
					}
					else if(randomTest>32){
						randomTest = 7;
					}
					else if(randomTest>24){
						randomTest = 6;
					}
					else if(randomTest>18){
						randomTest = 5;
					}
					else if(randomTest>13){
						randomTest = 4;
					}
					else if(randomTest>9){
						randomTest = 3;
					}
					else if(randomTest>4){
						randomTest = 2;
					}
					else {
						randomTest = 1;
					}
                }

                skills.Add(new Skill(idSkill, randomTest));
                print("J'add idSkill "+idSkill+" au joueur "+i);
                compteurSkills++;
            }
			ApplicationModel.opponentDeck.cards.Add(new GameCard(WordingCardName.getName(skills[0].Id), this.getRandomLife(cardType, difficultyLevel), cardType, this.getRandomMove(cardType), this.getRandomAttack(cardType, difficultyLevel), skills,i));
        }
    }

    private int getRandomLife(int cardType, int difficultyLevel){
    	int randomTest = -1;
		randomTest = UnityEngine.Random.Range(1,101);
		if(difficultyLevel==1){
			if(randomTest>90){
				randomTest = 10;
			}
			else if(randomTest>80){
				randomTest = 9;
			}
			else if(randomTest>70){
				randomTest = 8;
			}
			else if(randomTest>60){
				randomTest = 7;
			}
			else if(randomTest>50){
				randomTest = 6;
			}
			else if(randomTest>40){
				randomTest = 5;
			}
			else if(randomTest>30){
				randomTest = 4;
			}
			else if(randomTest>20){
				randomTest = 3;
			}
			else if(randomTest>10){
				randomTest = 2;
			}
			else {
				randomTest = 1;
			}
        }
		else if(difficultyLevel==2){
			if(randomTest>85){
				randomTest = 10;
			}
			else if(randomTest>71){
				randomTest = 9;
			}
			else if(randomTest>58){
				randomTest = 8;
			}
			else if(randomTest>46){
				randomTest = 7;
			}
			else if(randomTest>35){
				randomTest = 6;
			}
			else if(randomTest>25){
				randomTest = 5;
			}
			else if(randomTest>16){
				randomTest = 4;
			}
			else if(randomTest>8){
				randomTest = 3;
			}
			else if(randomTest>3){
				randomTest = 2;
			}
			else {
				randomTest = 1;
			}
        }
		else if(difficultyLevel==3){
			if(randomTest>75){
				randomTest = 10;
			}
			else if(randomTest>60){
				randomTest = 9;
			}
			else if(randomTest>42){
				randomTest = 8;
			}
			else if(randomTest>32){
				randomTest = 7;
			}
			else if(randomTest>24){
				randomTest = 6;
			}
			else if(randomTest>18){
				randomTest = 5;
			}
			else if(randomTest>13){
				randomTest = 4;
			}
			else if(randomTest>9){
				randomTest = 3;
			}
			else if(randomTest>4){
				randomTest = 2;
			}
			else {
				randomTest = 1;
			}
        }
        randomTest = randomTest*10;

        if(cardType==0){
			return (40+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==1){
            return (30+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==2){
			return (50+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==3){
			return (30+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==4){
			return (40+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==5){
			return (60+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==6){
			return (30+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==7){
			return (30+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==8){
			return (50+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else{
			return (40+Mathf.RoundToInt(20*(randomTest)/100f));
        }
    }

    private int getRandomAttack(int cardType, int difficultyLevel){
    	float randomTest = -1;
		randomTest = UnityEngine.Random.Range(1,101);
		if(difficultyLevel==1){
			if(randomTest>90){
				randomTest = 10;
			}
			else if(randomTest>80){
				randomTest = 9;
			}
			else if(randomTest>70){
				randomTest = 8;
			}
			else if(randomTest>60){
				randomTest = 7;
			}
			else if(randomTest>50){
				randomTest = 6;
			}
			else if(randomTest>40){
				randomTest = 5;
			}
			else if(randomTest>30){
				randomTest = 4;
			}
			else if(randomTest>20){
				randomTest = 3;
			}
			else if(randomTest>10){
				randomTest = 2;
			}
			else {
				randomTest = 1;
			}
        }
		else if(difficultyLevel==2){
			if(randomTest>85){
				randomTest = 10;
			}
			else if(randomTest>71){
				randomTest = 9;
			}
			else if(randomTest>58){
				randomTest = 8;
			}
			else if(randomTest>46){
				randomTest = 7;
			}
			else if(randomTest>35){
				randomTest = 6;
			}
			else if(randomTest>25){
				randomTest = 5;
			}
			else if(randomTest>16){
				randomTest = 4;
			}
			else if(randomTest>8){
				randomTest = 3;
			}
			else if(randomTest>3){
				randomTest = 2;
			}
			else {
				randomTest = 1;
			}
        }
		else if(difficultyLevel==3){
			if(randomTest>75){
				randomTest = 10;
			}
			else if(randomTest>60){
				randomTest = 9;
			}
			else if(randomTest>42){
				randomTest = 8;
			}
			else if(randomTest>32){
				randomTest = 7;
			}
			else if(randomTest>24){
				randomTest = 6;
			}
			else if(randomTest>18){
				randomTest = 5;
			}
			else if(randomTest>13){
				randomTest = 4;
			}
			else if(randomTest>9){
				randomTest = 3;
			}
			else if(randomTest>4){
				randomTest = 2;
			}
			else {
				randomTest = 1;
			}
        }
        randomTest = randomTest*10f;

        if(cardType==0){
			return (10+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==1){
			return (10+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==2){
			return (20+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==3){
			return (5+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==4){
			return (5+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==5){
			return (15+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==6){
			return (5+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==7){
			return (10+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==8){
			return (10+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else{
			return (15+Mathf.RoundToInt(15*(randomTest)/100f));
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

    private IEnumerator initializeGame(bool isFirstPlayer, bool isIAGame, int hisId, int hisRankingPoints, int hisDeckId)
    {
    	string isFirstPlayerString="1";
    	if(!isFirstPlayer)
    	{
    		isFirstPlayerString="0";
    	}
		string isIAGameString="1";
    	if(!isIAGame)
    	{
    		isIAGameString="0";
    	}
        WWWForm form = new WWWForm();                                       
        form.AddField("myform_hash", ApplicationModel.hash);                
		form.AddField("myform_isfirstplayer", isFirstPlayerString);
		form.AddField("myform_isiagame", isIAGameString);        
		form.AddField("myform_myid", ApplicationModel.player.Id.ToString());     
		form.AddField("myform_hisid", hisId.ToString());     
		form.AddField("myform_myrankingpoints",ApplicationModel.player.RankingPoints.ToString());     
		form.AddField("myform_hisrankingpoints", hisRankingPoints.ToString());  
		form.AddField("myform_gametype", ApplicationModel.player.ChosenGameType.ToString());    
		form.AddField("myform_mydeck", ApplicationModel.player.SelectedDeckId.ToString());     
		form.AddField("myform_hisdeck", hisDeckId.ToString()); 

        ServerController.instance.setRequest(URLInitiliazeGame, form);
        yield return ServerController.instance.StartCoroutine("executeRequest");
        
        if(ServerController.instance.getError()=="")
        {
			string result = ServerController.instance.getResult();
			string[] data=result.Split(new string[] { "#END#" }, System.StringSplitOptions.None);

			ApplicationModel.player.MyDeck=new Deck();

			string[] myDeckData = data[0].Split(new string[] { "#CARD#" }, System.StringSplitOptions.None);
			for(int i = 0 ; i < myDeckData.Length ; i++){
				ApplicationModel.player.MyDeck.cards.Add(new Card());
				ApplicationModel.player.MyDeck.cards[i].parseCard(myDeckData[i]);
				ApplicationModel.player.MyDeck.cards[i].deckOrder=i;
			}

			if(!isIAGame)
			{
				ApplicationModel.opponentDeck=new Deck();

				string[] hisDeckData = data[1].Split(new string[] { "#CARD#" }, System.StringSplitOptions.None);
				for(int i = 0 ; i < hisDeckData.Length ; i++){
					ApplicationModel.opponentDeck.cards.Add(new Card());
					ApplicationModel.opponentDeck.cards[i].parseCard(hisDeckData[i]);
					ApplicationModel.opponentDeck.cards[i].deckOrder=i;
				}
			}
			else
			{
				string[] iAData = data[1].Split(new string[] { "#IAINFO#" }, System.StringSplitOptions.None);
				ApplicationModel.hisPlayerID=System.Convert.ToInt32(iAData[0]);
				ApplicationModel.hisPlayerName=iAData[1];
				ApplicationModel.hisRankingPoints=System.Convert.ToInt32(iAData[2]);
			}

			if(isFirstPlayer)
			{
				ApplicationModel.currentGameId=System.Convert.ToInt32(data[2]);
			}
        }
        else
        {
            Debug.Log(ServerController.instance.getError());
            ServerController.instance.lostConnection();
        }
    }
    public void setSceneName(string sceneName)
    {
    	this.sceneName=sceneName;
    }
	public void leaveRandomRoomHandler()
	{
		this.isWaiting=false;
		this.displayLoadingScreenButton(false);
		this.leaveRoom ();

		if(ApplicationModel.player.ChosenGameType>20)
		{
			Invitation invitation = new Invitation ();
			invitation.Id = ApplicationModel.player.ChosenGameType-20;
			StartCoroutine(invitation.changeStatus(-1));
		}
		this.loadScene(this.sceneName);
	}
	public void loadScene(string sceneName)
    {
        StartCoroutine(this.preLoadScene(sceneName));
    }
    private IEnumerator preLoadScene(string sceneName) 
    {
    	this.isQuittingGame=true;
		async = Application.LoadLevelAsync(sceneName);
		async.allowSceneActivation = true;
        yield return async;
         
     }
	public void displayLoadingScreen()
	{
		if(!isLoadingScreenDisplayed)
		{
			this.preMatchScreen.SetActive(true);
			this.isLoadingScreenDisplayed=true;
			this.changeLoadingScreenLabel(WordingLoadingScreen.getReference(0));
			this.preMatchScreen.GetComponent<PreMatchScreenController>().reset();
		}
	}
	public void hideLoadingScreen()
	{
		if(isLoadingScreenDisplayed)
		{
			this.displayLoadingScreenButton (false);
			this.preMatchScreen.SetActive(false);
			this.isLoadingScreenDisplayed=false;
		}
		if(ApplicationModel.player.IsInviting)
		{
			ApplicationModel.player.IsInviting=false;
		}

	}
	public void changeLoadingScreenLabel(string label)
	{
		if(isLoadingScreenDisplayed)
		{
			this.preMatchScreen.GetComponent<PreMatchScreenController> ().changeLoadingScreenLabel (label);
		}
	}
	public void displayLoadingScreenButton(bool value)
	{
		if(isLoadingScreenDisplayed)
		{
            this.preMatchScreen.transform.FindChild("button").GetComponent<PreMatchScreenButtonController>().reset();
            this.preMatchScreen.GetComponent<PreMatchScreenController> ().displayButton (value);
		}
	}
	public void launchPreMatchLoadingScreen()
	{
		if(isLoadingScreenDisplayed)
		{
			this.preMatchScreen.GetComponent<PreMatchScreenController> ().launchPreMatchLoadingScreen();
		}
	}
	public void resize()
	{
		this.preMatchScreen.GetComponent<PreMatchScreenController>().resize();
	}
}
