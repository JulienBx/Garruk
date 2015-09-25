using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameController : Photon.MonoBehaviour
{	
	public static GameController instance;
	
	private float timePerTurn = 30 ;
	
	//URL pour les appels en BDD
	string URLStat = ApplicationModel.host + "updateResult.php";

	//Variable Photon
	const string roomNamePrefix = "GarrukGame";
	
	//Variables de gestion
	bool isReconnecting = false ;
	bool haveIStarted = false ;
	bool isRunningSkill = false ;
	bool isFirstPlayer = false;
	bool bothPlayerLoaded = false ;
	int nbPlayers = 0 ;
	int nbPlayersReadyToFight = 0; 
	int currentPlayingCard = -1;
	int currentClickedCard = -1;
	List<int> playingCardTurnsToWait; 
	TargetPCCHandler targetPCCHandler ;
	TargetTileHandler targetTileHandler ;
	public Deck myDeck ;
	int turnsToWait ; 
	string myPlayerName;
	string hisPlayerName;
	
	void Awake()
	{
		instance = this;

		PhotonNetwork.autoCleanUpPlayerObjects = false;
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
		
		this.playingCardTurnsToWait = new List<int>();

	}
	
	public void moveToDestination(Tile t){
		int characterToMove = this.currentPlayingCard;
		if (characterToMove==-1){
			characterToMove = this.currentClickedCard;
		}
		
		if (characterToMove!=-1){
			photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, t.x, t.y, characterToMove);
		}
	}
	
	[RPC]
	public IEnumerator moveCharacterRPC(int x, int y, int c)
	{
		if (!this.isFirstPlayer)
		{
			while (!this.bothPlayerLoaded)
			{
				yield return new WaitForSeconds(1);
			}
		}
		
		GameView.instance.movePlayingCard(x, y, c);
		GameView.instance.removeDestinations();	
		
		if (this.nbPlayersReadyToFight==2){
			if (GameView.instance.hasPlayed(this.currentPlayingCard))
			{
				this.resolvePass();
			}
		}
		else{
			GameView.instance.setInitialDestinations(this.isFirstPlayer);
			if(GameView.instance.getIsMine(c)){
				this.changeClickedCard(c);
			}
		}
			
		yield break ;
	}

	public void clickPlayingCard(int c, Tile t){
		if (c!=this.currentClickedCard && !this.havIStarted()){
			this.changeClickedCard(c);
		}
	}
	
	public void changeClickedCard(int c){
		if(this.currentClickedCard!=-1){
			GameView.instance.unClickPC (this.currentClickedCard);
		}
		this.currentClickedCard = c ;
		GameView.instance.changeClickedCard(c);
	}
	
	public void changePlayingCard(int c){
		if(this.currentPlayingCard!=-1){
			GameView.instance.unSelectPC (this.currentPlayingCard);
			if(this.turnsToWait==0){
				if(GameView.instance.getIsMine(this.currentPlayingCard)){
					
				}
				else{
					GameView.instance.showSkills();
				}
			}
			else{
				if(GameView.instance.getIsMine(this.currentPlayingCard)){
					if(this.turnsToWait==1){
						GameView.instance.overloadSkills("A votre adversaire de jouer. "+this.turnsToWait+" tour d'attente");
					}
					else{
						GameView.instance.overloadSkills("A votre adversaire de jouer. "+this.turnsToWait+" tours d'attente");
					}
				}
				else{
					if(this.turnsToWait==1){
						GameView.instance.changeOverloadText("A votre adversaire de jouer. "+this.turnsToWait+" tour d'attente");
					}
					else{
						GameView.instance.changeOverloadText("A votre adversaire de jouer. "+this.turnsToWait+" tours d'attente");
					}
				}
			}
		}
		else{
			if(this.turnsToWait==0){
				GameView.instance.showSkills();
			}
			else{
				if(this.turnsToWait==1){
					GameView.instance.overloadSkills("A votre adversaire de jouer. "+this.turnsToWait+" tour d'attente");
				}
				else{
					GameView.instance.overloadSkills("A votre adversaire de jouer. "+this.turnsToWait+" tours d'attente");
				}
			}
		}
		
		this.currentPlayingCard = c ;
		GameView.instance.changePlayingCard(c);
	}
	
	[RPC]
	public void findNextPlayer()
	{
		if(this.currentPlayingCard!=-1){
			if(GameView.instance.getCard(this.currentPlayingCard).isNurse()){
				List<Tile> targets = new List<Tile>();
				if(GameView.instance.getIsMine(this.currentPlayingCard)){
					targets = GameView.instance.getAllyImmediateNeighbours(GameView.instance.getPlayingCardTile(this.currentPlayingCard));
				}
				else{
					targets = GameView.instance.getOpponentImmediateNeighbours(GameView.instance.getPlayingCardTile(this.currentPlayingCard));
				}
				for (int i = 0 ; i < targets.Count ; i++){
					int target = GameView.instance.getTileCharacterID(targets[i].x, targets[i].y);
					int amount = Mathf.CeilToInt(GameView.instance.getCard(this.currentPlayingCard).getPassiveManacost()*GameView.instance.getCard(target).GetTotalLife()/100f);
					this.addCardModifier(target, -1*amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
					GameView.instance.displaySkillEffect(target, "INFIRMIER\n+"+amount+" PV", 4);
				}
			}
			else if(GameView.instance.getCard(this.currentPlayingCard).isFrenetique()){
				int amount = GameView.instance.getCard(this.currentPlayingCard).getPassiveManacost();
				int amountAttack = Mathf.CeilToInt(GameView.instance.getCard(this.currentPlayingCard).GetAttack()*amount / 100f);
				
				GameView.instance.getCard(this.currentPlayingCard).addModifier(amountAttack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 13, "Frénétique", "Permanent, +"+amountAttack+" ATK", "Permanent");
				GameView.instance.show(this.currentPlayingCard, false);
			}
		}
		
		this.timeRunsOut(2);
		
		this.turnsToWait = 100;
		
		bool newTurn = true;
		int nextPlayingCard = -1;
		int i2 = 0;
		int length = this.playingCardTurnsToWait.Count;
		
		while (i2 < length && newTurn == true)
		{
			if(!GameView.instance.isDead(i2)){
				this.playingCardTurnsToWait[i2]--;
				
				if(GameView.instance.getIsMine(i2)){
					if(this.playingCardTurnsToWait[i2]<this.turnsToWait){
						this.turnsToWait = this.playingCardTurnsToWait[i2];
					}
				}
				
				if (this.playingCardTurnsToWait[i2]==0)
				{
					this.playingCardTurnsToWait[i2]=GameView.instance.countAlive();
					nextPlayingCard = i2;
				}
			}
			i2++;
		}
		
		this.initPlayer(nextPlayingCard);
	}
	
	public void displayTR(){
		for(int i3 = 0 ; i3 < this.playingCardTurnsToWait.Count ; i3++){
			GameView.instance.getCard(i3).nbTurnsToWait=this.playingCardTurnsToWait[i3];
			GameView.instance.showTR(i3);
		}
	}
	
	public void killHandle(int c){
		
		if(GameView.instance.areAllMyPlayersDead()){
			StartCoroutine(quitGame());
		}
		else{
			for (int i = 0 ; i < this.playingCardTurnsToWait.Count ; i++){
				if(this.playingCardTurnsToWait[i]>this.playingCardTurnsToWait[c]){
					this.playingCardTurnsToWait[i]--;
					GameView.instance.getCard(i).nbTurnsToWait=this.playingCardTurnsToWait[i];
					GameView.instance.showTR(i);
				}
			}
		}
	}
	
	public void displaySkillEffect(int id, string text, int color){
		photonView.RPC("displaySkillEffectRPC", PhotonTargets.AllBuffered, id, text, color);
	}
	
	[RPC]
	public void displaySkillEffectRPC(int id, string text, int color){
		GameView.instance.displaySkillEffect(id, text, color);
	}

	IEnumerator setPlayer(float time, int id){
		yield return new WaitForSeconds(time);
		
		this.displayTR();
		
		int length = this.playingCardTurnsToWait.Count;
		
		if (this.currentPlayingCard != -1)
		{
			GameView.instance.checkModifyers(this.currentPlayingCard);
		}
	
		GameView.instance.removeDestinations();
		
		if(this.turnsToWait==0){
			if (GameView.instance.getCard(id).isParalyzed()){
				GameView.instance.moveCard(id, false);
			}
			if (GameView.instance.getCard(id).isSleeping()){
				int sleepingPercentage = GameView.instance.getCard(id).getSleepingPercentage();
				if(UnityEngine.Random.Range(1,101)<sleepingPercentage){
					this.displaySkillEffect(id, "SE REVEILLE", 4);
					this.wakeUp(id);
				}
				else{
					this.displaySkillEffect(id, "RESTE ENDORMI", 5);
				}
			}
			else{
				GameView.instance.playCard(id, false);
				GameView.instance.moveCard(id, false);
			}
			this.changePlayingCard(id);
			if(!GameView.instance.hasMoved(id)){
				this.calculateDestinations();
			}
		}
		else{
			this.changePlayingCard(id);
			this.calculateHisDestinations();
		}
		this.isRunningSkill = false ;
	}
	
	public void calculateDestinations(){
		GameView.instance.setDestinations(this.currentPlayingCard);
		
		if(GameView.instance.getCard(this.currentPlayingCard).isFurious()){
			StartCoroutine(launchFury());
		}
	}
	
	IEnumerator launchFury(){
		yield return new WaitForSeconds(1);
		
		int enemy = GameView.instance.attackClosestEnnemy();
		
		yield return new WaitForSeconds(1);
		
		if(enemy!=-1){
			GameSkills.instance.getSkill(0).init(GameView.instance.getCard(this.currentPlayingCard), GameView.instance.getCard(this.currentPlayingCard).GetAttackSkill());
			GameSkills.instance.getSkill(0).addTarget(enemy,1);
			GameSkills.instance.getSkill(0).applyOn();
		}
	}
	
	public void calculateHisDestinations(){
		GameView.instance.setHisDestinations(this.currentPlayingCard);
	}

	public void initPlayer(int id)
	{
		if(this.currentPlayingCard!=-1){
			if(!GameView.instance.getIsMine(this.currentPlayingCard)){
				GameView.instance.unSelectSkill();
			}
		}
		StartCoroutine(setPlayer(1, id));
	}

	public void resolvePass()
	{	
		photonView.RPC("findNextPlayer", PhotonTargets.AllBuffered);
	}
	
	public void wakeUp(int id)
	{
		photonView.RPC("wakeUpRPC", PhotonTargets.AllBuffered, id);
	}
	
	[RPC]
	public void wakeUpRPC(int id)
	{
		GameView.instance.getCard(id).removeSleeping();
		GameView.instance.show(id, false);
	}

	[RPC]
	public void addPassEvent()
	{
		GameEventType ge = new PassType();
		addGameEvent(ge, "");
		//nbActionPlayed = 0;
		changeGameEvents();
		fillTimeline();
	}
	
	private IEnumerator returnToLobby()
	{
//		if (gameView.MyPlayerNumber == 1)
//		{
//			yield return new WaitForSeconds(5);
//		} 
//		else
//		{
//			yield return new WaitForSeconds(7);
//		}
//		PhotonNetwork.Disconnect();
		yield break;
	}
	
	IEnumerator sendStat(string user1, string user2, bool isTutorialGame)
	{
		int isTutorialGameInt = 0;
		if(isTutorialGame)
		{
			isTutorialGameInt=1;
		}

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick1", user1); 	                    // Pseudo de l'utilisateur victorieux
		form.AddField("myform_nick2", user2); 	                    // Pseudo de l'autre utilisateur
		form.AddField("myform_gametype", ApplicationModel.gameType);
		form.AddField ("myform_istutorialgame", isTutorialGameInt);

		WWW w = new WWW(URLStat, form); 							// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null)
		{
			print(w.error); 										// donne l'erreur eventuelle
		} else
		{
			//print(w.text);
		}
		yield break;
	}

	void initGrid()
	{
		print("J'initialise le terrain de jeu");
		bool isRock = false;
		List<Tile> rocks = new List<Tile>();
		Tile t = new Tile(0,0) ;
		if (!GameView.instance.getIsTutorialLaunched())
		{
			int nbRocksToAdd = UnityEngine.Random.Range(3, 6);
			int compteurRocks = 0;
			bool isOk = true;
			while (compteurRocks<nbRocksToAdd)
			{
				isOk = false;
				while (!isOk)
				{
					t = GameView.instance.getRandomRock(1);
					isOk = true;
					for (int a = 0; a < rocks.Count && isOk; a++)
					{
						if (rocks [a].x == t.x && rocks [a].y == t.y)
						{
							isOk = false;
						}
					}
				}
				rocks.Add(t);
				compteurRocks++;
			}
		} else
		{
			rocks.Add(new Tile(2, 3));
			rocks.Add(new Tile(3, 5));
			rocks.Add(new Tile(4, 3));
		}
		
		int w = GameView.instance.getBoardWidth();
		int h = GameView.instance.getBoardHeight();
		for (int x = 0; x < w; x++)
		{
			for (int y = 0; y < h; y++)
			{
				isRock = false;
				for (int z = 0; z < rocks.Count && !isRock; z++)
				{
					if (rocks [z].x == x && rocks [z].y == y)
					{
						isRock = true;
					}
				}
				if (!isRock)
				{
					photonView.RPC("AddTileToBoard", PhotonTargets.AllBuffered, x, y, 0);	
				}
				else{
					photonView.RPC("AddTileToBoard", PhotonTargets.AllBuffered, x, y, 1);
				}
			}
		}
	}

	public IEnumerator loadMyDeck()
	{
		Deck myDeck = new Deck(ApplicationModel.username);
		yield return StartCoroutine(myDeck.LoadDeck());

		photonView.RPC("SpawnCharacter", PhotonTargets.AllBuffered, this.isFirstPlayer, myDeck.Id);

		if (GameView.instance.getIsTutorialLaunched())
		{
			StartCoroutine(this.loadTutorialDeck(!this.isFirstPlayer, "Garruk"));
		}
		GameView.instance.hideLoadingScreen ();
	}
	public IEnumerator loadTutorialDeck(bool isFirstPlayer, string name)
	{
		Deck tutorialDeck = new Deck(name);
		yield return StartCoroutine(tutorialDeck.LoadDeck());

		photonView.RPC("SpawnCharacter", PhotonTargets.AllBuffered, isFirstPlayer, tutorialDeck.Id);
	}
	
	[RPC]
	void AddPlayerToList(int id, string loginName)
	{
		print(loginName+" se connecte");

		if (ApplicationModel.username == loginName)
		{
			GameView.instance.setMyPlayerName(loginName);
			this.myPlayerName=loginName;
			print (myPlayerName);
		} else
		{
			GameView.instance.setHisPlayerName(loginName);
			this.hisPlayerName=loginName;
			print (hisPlayerName);
		}
		
		this.nbPlayers++;
		if(this.nbPlayers==2){
			if(this.isFirstPlayer==true){
				PhotonNetwork.room.open = false;
			}
		}
	}

	[RPC]
	void AddTileToBoard(int x, int y, int type)
	{
		GameView.instance.createTile(x, y, type, this.isFirstPlayer);
	}

	[RPC]
	IEnumerator SpawnCharacter(bool isFirstP, int idDeck)
	{
		Deck deck;
		deck = new Deck(idDeck);
		yield return StartCoroutine(deck.RetrieveCards());

		for (int i = 0; i < ApplicationModel.nbCardsByDeck; i++)
		{
			GameView.instance.createPlayingCard(deck.Cards [i], isFirstP, this.isFirstPlayer);
		}
		
		if (isFirstP == this.isFirstPlayer)
		{
			this.myDeck = deck;
			GameView.instance.setInitialDestinations(this.isFirstPlayer);
			GameView.instance.showStartButton();
			GameView.instance.checkPassiveSkills(true);
		}
		else{
			GameView.instance.checkPassiveSkills(false);
		}

		if (GameView.instance.getIsTutorialLaunched())
		{
			TutorialObjectController.instance.actionIsDone();
		}

		if (this.nbPlayers==2){
			this.bothPlayerLoaded = true ;
		}

		if (GameView.instance.getIsTutorialLaunched())
		{

		}
		
		yield break;
	}

	public void playerReady()
	{
		GameView.instance.removeDestinations();
		GameView.instance.removeClickedCard(this.currentClickedCard);
		this.haveIStarted = true ;
		photonView.RPC("playerReadyRPC", PhotonTargets.AllBuffered, this.isFirstPlayer);
		if (GameView.instance.getIsTutorialLaunched())
		{
			photonView.RPC("playerReadyRPC", PhotonTargets.AllBuffered, !this.isFirstPlayer);
		}
		
	}

	public void StartFight()
	{		
		this.sortAllCards();
		photonView.RPC("findNextPlayer", PhotonTargets.AllBuffered);
	}
	
	[RPC]
	public void playerReadyRPC(bool isFirst)
	{	
		nbPlayersReadyToFight++;
		if (nbPlayersReadyToFight == 2)
		{
			GameView.instance.hideStartButton();
			GameView.instance.displayOpponentCards(this.isFirstPlayer);
			if (this.isFirstPlayer)
			{
				this.StartFight();
			}
		}
	}

	public void reloadSortedList()
	{
		if (this.isFirstPlayer)
		{
			this.sortAllCards();
			photonView.RPC("reloadTimeline", PhotonTargets.AllBuffered);
		}
	}
	
	public void sortAllCards()
	{
		List <int> quicknessesToRank = new List<int>();
		int length = GameView.instance.getNbPlayingCards();
		
		for (int i = 0; i < length; i++)
		{
			quicknessesToRank.Add(GameView.instance.getCard(i).GetSpeed());
			this.playingCardTurnsToWait.Add(1);
		}
		
		for (int i = 0; i < length-1; i++)
		{
			for (int j = i+1; j < length; j++)
			{
				if (i==j){
					
				}
				else if(quicknessesToRank[i]>quicknessesToRank[j]){
					this.playingCardTurnsToWait[j]++;
				}
				else if(quicknessesToRank[i]==quicknessesToRank[j]){
					if(i<j){
						this.playingCardTurnsToWait[j]++;
					}
				}
				else{
					this.playingCardTurnsToWait[i]++;
				}
			}
			print ("Héros "+i+" attend "+this.playingCardTurnsToWait[i]+" tours");
			photonView.RPC("addRankedCharacter", PhotonTargets.AllBuffered, i, this.playingCardTurnsToWait[i]);
		}
		photonView.RPC("addRankedCharacter", PhotonTargets.AllBuffered, length-1, this.playingCardTurnsToWait[length-1]);
	}
	
	public void advanceTurns(int idToRank, int nbTurns)
	{
		int i = 0;
		int length = this.playingCardTurnsToWait.Count;
		int firstRank = this.playingCardTurnsToWait[idToRank];
		int nextRank = Mathf.Max(1,this.playingCardTurnsToWait[idToRank]-nbTurns);
		
		while (i<length)
		{
			if(i==idToRank){
				
			}
			else if (this.playingCardTurnsToWait[i]<firstRank && this.playingCardTurnsToWait[i]>=nextRank)
			{
				this.playingCardTurnsToWait[i]++;
			}
			i++;
		}
		
		this.playingCardTurnsToWait[idToRank]=Mathf.Max(1, this.playingCardTurnsToWait[idToRank]-nbTurns) ;
		
		for (int j = 0 ; j < length ; j++){
			GameView.instance.getCard(j).nbTurnsToWait = this.playingCardTurnsToWait[j];
			if(j!=this.getCurrentPlayingCard()){
				GameView.instance.show(j, true);
			}
			else{
				GameView.instance.show(j, false);
			}
		}
	}
	
	public void backTurns(int idToRank, int nbTurns)
	{
		int i = 0;
		int length = this.playingCardTurnsToWait.Count;
		int firstRank = this.playingCardTurnsToWait[idToRank];
		int nextRank = Mathf.Min(length,this.playingCardTurnsToWait[idToRank]+nbTurns);
		
		while (i<length)
		{
			if(i==idToRank){
				
			}
			else if (this.playingCardTurnsToWait[i]>firstRank && this.playingCardTurnsToWait[i]<=nextRank)
			{
				this.playingCardTurnsToWait[i]--;
			}
			i++;
		}
		
		this.playingCardTurnsToWait[idToRank] = Mathf.Min (8, this.playingCardTurnsToWait[idToRank]+nbTurns) ;
		
		for (int j = 0 ; j < length ; j++){
			GameView.instance.getCard(j).nbTurnsToWait = this.playingCardTurnsToWait[j];
			if(j!=this.getCurrentPlayingCard()){
				GameView.instance.show(j, true);
			}
			else{
				GameView.instance.show(j, false);
			}
		}
	}

	public int FindMaxQuicknessIndex(List<int> list)
	{
		if (list.Count == 0)
		{
			throw new InvalidOperationException("Liste vide !");
		}
		int max = -1;
		int index = -1;
		for (int i = 0; i < list.Count; i++)
		{
			if (list [i] > max)
			{
				max = list [i];
				index = i;
			}
		}
		return index;
	}
	
	[RPC]
	public void addRankedCharacter(int id, int rank)
	{
		if (!this.isFirstPlayer)
		{
			this.playingCardTurnsToWait.Insert(id,rank);
		}
		GameView.instance.getCard(id).nbTurnsToWait = rank ;
		GameView.instance.showTR(id);
	}

	[RPC]
	public void inflictDamageRPC(int targetCharacter, bool isFisrtPlayerCharacter)
	{
		if (!photonView.isMine)
		{
			//displayPopUpMessage(this.playingCards [targetCharacter].GetComponentInChildren<PlayingCardController>().card.Title + " attaque", 2f);
		}
	}

	public void timeRunsOut(float time)
	{
//		startTurn = true;
//		gameView.gameScreenVM.timer = time;
	}
	
	// Photon
	void OnJoinedLobby()
	{
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);    
		string sqlLobbyFilter = "C0 = " + ApplicationModel.gameType;
		PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	}
	
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room! - creating a new room");
		RoomOptions newRoomOptions = new RoomOptions();
		newRoomOptions.isOpen = true;
		newRoomOptions.isVisible = true;
		newRoomOptions.maxPlayers = 2;
		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", ApplicationModel.gameType } }; // CO pour une partie simple
		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
		
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);
		PhotonNetwork.CreateRoom(roomNamePrefix + Guid.NewGuid().ToString("N"), newRoomOptions, sqlLobby);
		this.isFirstPlayer = true;
	}
	
	void OnJoinedRoom()
	{
		Debug.Log("Connecté à Photon");
		if (!isReconnecting)
		{
			photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);
			if (GameView.instance.getIsTutorialLaunched())
			{
				photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID + 1, "Garruk");
			}
		} 
		else
		{
			Debug.Log("Mode reconnection");
		}
		GameView.instance.createBackground();
		
		if (this.isFirstPlayer)
		{
			this.initGrid();
		}
		StartCoroutine(this.loadMyDeck());
		
			
	}
	
	void OnDisconnectedFromPhoton()
	{
		Application.LoadLevel("NewHomePage");
	}

	public void quitGameHandler()
	{
		StartCoroutine(this.quitGame());
	}
	
	public IEnumerator quitGame()
	{
		
		if(GameView.instance.getIsTutorialLaunched())
		{
			yield return (StartCoroutine(this.sendStat(myPlayerName, hisPlayerName, true)));
			photonView.RPC("quitGameRPC", PhotonTargets.AllBuffered, false);
		}
		else
		{
			if(isFirstPlayer)
			{
				yield return (StartCoroutine(this.sendStat(hisPlayerName, myPlayerName, false)));
			} 
			else
			{
				yield return (StartCoroutine(this.sendStat(hisPlayerName, myPlayerName, false)));
			}
			photonView.RPC("quitGameRPC", PhotonTargets.AllBuffered, this.isFirstPlayer);
		}
	yield break;
	}
	
	[RPC]
	public void quitGameRPC(bool isFirstP)
	{
		if (isFirstP == this.isFirstPlayer)
		{
			//Le joueur actif a perdu
			EndSceneController.instance.displayEndScene(false);
		} else
		{
			//Le deuxième joueur a perdu
			EndSceneController.instance.displayEndScene(true);
		}
	}
	public void addGameEvent(GameEventType type, string targetName)
	{
		setGameEvent(type);
		if (targetName != "")
		{
//			int midTimeline = (int)Math.Floor((double)eventMax / 2);
//			gameEvents [midTimeline].GetComponent<GameEventController>().setTarget(targetName);
		}
	}

	public void addGameEvent(string action, string targetName)
	{
		photonView.RPC("addGameEventRPC", PhotonTargets.AllBuffered, action, targetName);
	}
	
	[RPC]
	public void addGameEventRPC(string action, string targetName)
	{
		setGameEvent(new SkillType(action));
		if (targetName != "")
		{
//			int midTimeline = (int)Math.Floor((double)eventMax / 2);
//			gameEvents [midTimeline].GetComponent<GameEventController>().setTarget(targetName);
		}
	}
	
	GameObject setGameEvent(GameEventType type)
	{
//		int midTimeline = (int)Math.Floor((double)eventMax / 2);
//		GameObject go;
//		if (nbActionPlayed == 0)
//		{ 
//			go = gameEvents [midTimeline];
//			go.GetComponent<GameEventController>().setAction(type.toString());
//			nbActionPlayed++;
//		} else if (nbActionPlayed < 2)
//		{
//			go = gameEvents [midTimeline];
//			go.GetComponent<GameEventController>().addAction(type.toString());
//			nbActionPlayed++;
//		} else
//		{
//			go = gameEvents [0];
//		}

		//return go;
		return null;
	}

	void fillTimeline()
	{
		int rankedPlayingCardID = 0;
		bool nextChara = true;
		bool findCharactersHaveNoAlreadyPlayed = false;

//		if (nextCharacterPositionTimeline < 7)
//		{
//			findCharactersHaveNoAlreadyPlayed = true;
//		}
//
//		while (nextChara)
//		{
//			rankedPlayingCardID = rankedPlayingCardsID [nextCharacterPositionTimeline];
//			PlayingCardController pcc = this.playingCards [rankedPlayingCardID] .GetComponentInChildren<PlayingCardController>();
//			if (!pcc.isDead && (!pcc.hasPlayed && !findCharactersHaveNoAlreadyPlayed || findCharactersHaveNoAlreadyPlayed))
//			{
//				nextChara = false;
//			}
//			if (++nextCharacterPositionTimeline > playingCards.Length - 1)
//			{
//				nextCharacterPositionTimeline = 0;
//				findCharactersHaveNoAlreadyPlayed = true;
//			}
//		}
		addCardEvent(rankedPlayingCardID, 0);
	}

	[RPC]
	public void reloadTimeline()
	{
//		bool findCharactersHaveNoAlreadyPlayed = false;
//		//nextCharacterPositionTimeline = 0;
//		for (int i = 4; i >= 0; i--)
//		{
//			int rankedPlayingCardID = 0;
//			bool nextChara = true;
//			while (nextChara)
//			{
//				//rankedPlayingCardID = rankedPlayingCardsID [nextCharacterPositionTimeline];
//				PlayingCardController pcc = this.playingCards [rankedPlayingCardID] .GetComponentInChildren<PlayingCardController>();
//				
//				if (!pcc.isDead && (!pcc.hasPlayed && !findCharactersHaveNoAlreadyPlayed || findCharactersHaveNoAlreadyPlayed) && rankedPlayingCardID != currentPlayingCard)
//				{
//					nextChara = false;
//				}
////				if (++nextCharacterPositionTimeline > playingCards.Length - 1)
////				{
////					nextCharacterPositionTimeline = 0;
////					findCharactersHaveNoAlreadyPlayed = true;
////				}
//			}
//			addCardEvent(rankedPlayingCardID, i);
//		}

	}

	void addCardEvent(int idCharacter, int position)
	{
		//GameObject go = gameEvents [position];
//		PlayingCardController pcc = playingCards [idCharacter].GetComponent<PlayingCardController>();
//		go.GetComponent<GameEventController>().setCharacterName(pcc.card.Title);
//		Texture t2 = playingCards [idCharacter].GetComponent<PlayingCardController>().getPicture();
//		Texture2D temp = getImageResized(t2 as Texture2D);
//		go.GetComponent<GameEventController>().IDCharacter = idCharacter;
//		go.GetComponent<GameEventController>().setAction("");
//		go.GetComponent<GameEventController>().setArt(temp);
//		go.GetComponent<GameEventController>().setBorder(isThisCharacterMine(idCharacter));
//		go.GetComponent<GameEventController>().gameEventView.show();
//		go.renderer.enabled = true;
	}

	void initGameEvents()
	{
//		//gameEventInitialized = true;
//		GameObject go;
//		int i = 1;
//		while (gameEvents.Count < eventMax)
//		{
//			go = (GameObject)Instantiate(gameEvent);
//			gameEvents.Add(go);
//			go.GetComponent<GameEventController>().setScreenPosition(gameEvents.Count, boardWidth, boardHeight, tileScale);
//			go.GetComponent<GameEventController>().setBorder(0);
//			if (i < 7)
//			{
//				GameObject child = (GameObject)Instantiate(go.GetComponent<GameEventController>().transparentImage);
//				child.name = "TransparentEvent";
//				child.transform.parent = go.transform;
//				child.transform.localPosition = new Vector3(0f, 0f, -5f);
//				child.transform.localScale = new Vector3(0.9f, 0.9f, 10f);
//			}
////			if (i == 6)
////			{
////				go.transform.localScale = new Vector3(0.95f, 0.95f, 10f);
////			} 
//
//			if (i > 6)
//			{
//				GameObject child = (GameObject)Instantiate(go.GetComponent<GameEventController>().darkImage);
//				child.name = "DarkEvent";
//				child.transform.parent = go.transform;
//				child.transform.localPosition = new Vector3(0f, 0f, -5f);
//				child.transform.localScale = new Vector3(0.9f, 0.9f, 10f);
//				child.renderer.enabled = false;
//			}
//			go.renderer.enabled = false;
//			i++;
//		}
//		for (i = 0; i < 6; i++)
//		{
//			addCardEvent(rankedPlayingCardsID [5 - i], i);
//		}
//		nextCharacterPositionTimeline = 6;
	}

	Texture2D getImageResized(Texture2D t)
	{
		int size;
		Color[] pix;
		if (t.width > t.height)
		{
			size = t.height;
			pix = t.GetPixels((t.width - size) / 2, 0, size, size);
		} else
		{
			size = t.width;
			pix = t.GetPixels(0, (t.height - size) / 2, size, size);
		}
		Texture2D temp = new Texture2D(size, size);
		temp.SetPixels(pix);
		temp.Apply();
		
		return temp;
	}
	
	public int getCurrentPlayingCard()
	{
		return this.currentPlayingCard;
	}

	void changeGameEvents()
	{
//		for (int i = gameEvents.Count - 1; i > 0; i--)
//		{
//			int id = gameEvents [i - 1].GetComponent<GameEventController>().IDCharacter;
//			string title = gameEvents [i - 1].GetComponent<GameEventController>().getCharacterName();
//			string action = gameEvents [i - 1].GetComponent<GameEventController>().getAction();
//			GameObject[] movement = gameEvents [i - 1].GetComponent<GameEventController>().getMovement();
//			Texture2D t2 = gameEvents [i - 1].GetComponent<GameEventController>().getArt();
//			if (title != "" && i > 5)
//			{
//				gameEvents [i].renderer.enabled = true;
//				gameEvents [i].transform.Find("DarkEvent").renderer.enabled = true;
//			}
//
//			gameEvents [i].GetComponent<GameEventController>().IDCharacter = id;
//			gameEvents [i].GetComponent<GameEventController>().setCharacterName(title);
//			gameEvents [i].GetComponent<GameEventController>().setAction(action);
//			gameEvents [i].GetComponent<GameEventController>().setMovement(movement [0], movement [1]);
//			gameEvents [i].GetComponent<GameEventController>().setArt(t2);
//			if (i < 6)
//			{
//				gameEvents [i].GetComponent<GameEventController>().setBorder(isThisCharacterMine(id));
//			}
//
//			gameEvents [i].GetComponent<GameEventController>().gameEventView.show();
//			gameEvents [i - 1].GetComponent<GameEventController>().setMovement(null, null);
//		}
	}

	void recalculateGameEvents()
	{
		int i = 1;

//		foreach (GameObject go in gameEvents)
//		{
//			go.GetComponent<GameEventController>().setScreenPosition(i++, boardWidth, boardHeight, tileScale);
//		}
	}

	public void disconnect()
	{
		PhotonNetwork.Disconnect();
	}

	public void spawnMinion(string minionName, int targetX, int targetY, int amount, bool isFirstP)
	{
		photonView.RPC("spawnMinion", PhotonTargets.AllBuffered, this.isFirstPlayer, minionName, targetX, targetY, amount);
	}

	[RPC]
	public void spawnMinion(bool isFirstP, string minionName, int targetX, int targetY, int amount)
	{
//		//addGameEvent(new SkillType(getCurrentSkill().Action), "");
//		GameObject[] temp = new GameObject[playingCards.Length + 1];
//		int position;
//		if (isFirstP)
//		{
//			for (int i = 0; i < limitCharacterSide; i++)
//			{
//				temp [i] = playingCards [i];
//			}
//			for (int i = limitCharacterSide + 1; i < temp.Length; i++)
//			{
//				temp [i] = playingCards [i - 1];
//				PlayingCardController pcctemp = playingCards [i - 1].GetComponent<PlayingCardController>();
//				getTile(pcctemp.tile.x, pcctemp.tile.y).characterID = i;
//			}
//			position = limitCharacterSide;
//			limitCharacterSide++;
//		} else
//		{
//			for (int i = 0; i < playingCards.Length; i++)
//			{
//				temp [i] = playingCards [i];
//			}
//			position = playingCards.Length;
//		}
//
//		playingCards = temp;
//		//this.playingCards [position] = (GameObject)Instantiate(this.playingCard);
//		PlayingCardController pccTemp = this.playingCards [position].GetComponentInChildren<PlayingCardController>();
//		pccTemp.setStyles(isFirstP == this.isFirstPlayer);
//		Card minion;
//		switch (minionName)
//		{
//			case "Grizzly": 
//				minion = new Card(0, minionName, 60, 10, 50, 2, amount, new List<Skill>());
//				break;
//			case "Loup": 
//				minion = new Card(0, minionName, 40, 11, 100, 5, amount, new List<Skill>());
//				break;
//			default:
//				minion = new Card();
//				break;
//		}
//		
//		pccTemp.setCard(minion);
//		pccTemp.setIDCharacter(position);
//		pccTemp.setTile(new Tile(targetX, targetY), tiles [targetX, targetY].GetComponent<TileController>().tileView.tileVM.position, !isFirstP);
//		pccTemp.resize();
//		this.tiles [targetX, targetY].GetComponent<TileController>().characterID = position;
//		pccTemp.show();
//		reloadSortedList();
	}

	public PlayingCardController getCurrentPCC()
	{
		//return this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>();
		return null ;
	}

	public TileController getTile(int x, int y)
	{
		//return this.tiles [x, y].GetComponent<TileController>();
		return null;
	}

	public int getCurrentSkillID()
	{
//		if (this.clickedSkill == 4)
//		{
//			return 0;
//		} else if (this.clickedSkill == 5)
//		{
//			return 1;
//		}
//		return this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().card.Skills [this.clickedSkill].Id;
		return 0;
	}

	public void play()
	{	
		StartCoroutine(playEnum ());
	}
	
	public IEnumerator playEnum()
	{
		this.applyOn();
		
		yield return new WaitForSeconds(1);
		
		if (GameView.instance.hasMoved(this.currentPlayingCard) || GameView.instance.isDead(this.currentPlayingCard))
		{
			this.isRunningSkill = false ;
			this.resolvePass();
		}
		else{
			GameView.instance.playCard(this.currentPlayingCard, true);
			if (!GameView.instance.isDead(this.currentPlayingCard)){
				GameView.instance.displayDestinations();
			}
			GameView.instance.checkSkillsLaunchability();
			this.isRunningSkill = false ;
		}
	}
	
	public void startPlayingSkill(int id)
	{
		photonView.RPC("startPlayingSkillRPC", PhotonTargets.AllBuffered, id);
	}
	
	[RPC]
	public void startPlayingSkillRPC(int idskill)
	{
		if (idskill==-1){
			GameSkills.instance.setCurrentSkill(new Skill("", "", 0));
		}
		else {
			GameSkills.instance.setCurrentSkill(GameView.instance.getCard(this.currentPlayingCard).getSkills()[idskill]);
		}
		if (!GameView.instance.getIsMine(this.currentPlayingCard) && idskill!=-1){
			GameView.instance.selectSkill(idskill);
		}
		
		if(idskill==-1){
			idskill = 0 ;
			Skill s = new Skill();
			GameSkills.instance.getSkill(0).init(GameView.instance.getCard(this.currentPlayingCard), s);
			if (GameController.instance.isMyCharacterPlaying()){
				GameSkills.instance.getSkill(0).launch ();
			}
		}
		else{
			Skill s = GameView.instance.getCard(this.currentPlayingCard).getSkills()[idskill];
			GameSkills.instance.getSkill(s.Id).init(GameView.instance.getCard(this.currentPlayingCard), s);	
			if (GameController.instance.isMyCharacterPlaying()){
				GameSkills.instance.getSkill(s.Id).launch ();
			}
		}
	}
	
	public void addCardModifier(int target, int amount, ModifierType type, ModifierStat stat, int duration, int idIcon, string t, string d, string a)
	{ 
		GameView.instance.getCard(target).addModifier(amount, type, stat, duration, idIcon, t, d, a);
		if(target!=this.getCurrentPlayingCard()){
			GameView.instance.show(target, true);
		}
		else{
			GameView.instance.show(target, false);
		}
		if (stat == ModifierStat.Stat_Dommage)
		{
			if (!GameView.instance.isDead(target) && GameView.instance.getCard(target).GetLife() <= 0)
			{
				GameView.instance.kill(target);
			}
			GameView.instance.updateMyLifeBar();
			GameView.instance.updateHisLifeBar();
		}
	}
	
	public void addTileModifier(Tile tile, int amount, ModifierType type, ModifierStat stat, int duration, int idIcon, string t, string d, string a)
	{ 
		bool b = false;
		if(GameView.instance.getIsMine(this.currentPlayingCard)){
			b = true ;
		}
		GameView.instance.setModifier(tile, amount, type, stat, duration, idIcon, t, d, a, b);
	}
	
	public void addElectroPiege(int x, int y, int manacost, bool isFirstP){
		photonView.RPC("addElectroPiegeRPC", PhotonTargets.AllBuffered, x, y, manacost, isFirstP);
	}
	
	[RPC]
	public void addElectroPiegeRPC(int x, int y, int manacost, bool isFirstP){
		GameView.instance.setModifier(new Tile(x,y), manacost, ModifierType.Type_Wolftrap, ModifierStat.Stat_No, -1, 4, "Electropiege", "Inflige "+manacost+" dégats", "Permanent. Non visible du joueur adverse", (isFirstP==this.isFirstPlayer));
	}
	
	public IEnumerator kill(int target)
	{
		yield return new WaitForSeconds(2f);
		//this.tiles [this.getPCC(target).tile.x, this.getPCC(target).tile.x].GetComponent<TileController>().characterID = -1;
		GameView.instance.getPCC(target).kill();
	}

	public void clearDeads()
	{
		//deads.Clear();
		foreach (GameObject goTag in GameObject.FindGameObjectsWithTag("deadCharacter"))
		{
			Destroy(goTag);
		}
	}
	
	public void initPCCTargetHandler(int numberOfExpectedTargets)
	{
		this.targetPCCHandler = new TargetPCCHandler(numberOfExpectedTargets);
	}
	
	public void initTileTargetHandler(int numberOfExpectedTargets)
	{
		this.targetTileHandler = new TargetTileHandler(numberOfExpectedTargets);
	}
	
	public void applyOn()
	{
		photonView.RPC("applyOnRPC5", PhotonTargets.AllBuffered);
	}
	
	[RPC]
	public void applyOnRPC5()
	{
		GameSkills.instance.getCurrentGameSkill().applyOn();
	}
	
	public void applyOn(int[] targets)
	{
		photonView.RPC("applyOnRPC", PhotonTargets.AllBuffered, targets);
	}
	
	[RPC]
	public void applyOnRPC(int[] targets)
	{
		//GameSkills.instance.getCurrentGameSkill().applyOn(targets);
	}
	
	public void applyOn(int target)
	{
		photonView.RPC("applyOnRPC4", PhotonTargets.AllBuffered, target);
	}
	
	[RPC]
	public void applyOnRPC4(int target)
	{
		//GameSkills.instance.getCurrentGameSkill().applyOn(target);
	}
	
	public void applyOn(int target, int arg)
	{
		photonView.RPC("applyOnRPC3", PhotonTargets.AllBuffered, target, arg);
	}
	
	[RPC]
	public void applyOnRPC3(int target, int arg)
	{
		//GameSkills.instance.getCurrentGameSkill().applyOn(target, arg);
	}
	
	public void addTarget(int target, int result)
	{
		photonView.RPC("addTargetRPC", PhotonTargets.AllBuffered, target, result);
	}
	
	[RPC]
	public void addTargetRPC(int target, int result)
	{
		GameSkills.instance.getCurrentGameSkill().addTarget(target, result);
	}
	
	public void addTarget(int target, int result, int value)
	{
		photonView.RPC("addTargetValueRPC", PhotonTargets.AllBuffered, target, result, value);
	}
	
	[RPC]
	public void addTargetValueRPC(int target, int result, int value)
	{
		GameSkills.instance.getCurrentGameSkill().addTarget(target, result, value);
	}
	
	public void addTargetTile(int tileX, int tileY, int result)
	{
		photonView.RPC("addTargetTileRPC", PhotonTargets.AllBuffered, tileX, tileY, result);
	}
	
	[RPC]
	public void addTargetTileRPC(int tileX, int tileY, int result)
	{
		GameSkills.instance.getCurrentGameSkill().addTarget(new Tile(tileX, tileY), result);
	}
	
	public void applyOn(int target, int arg, int arg2)
	{
		photonView.RPC("applyOnRPC6", PhotonTargets.AllBuffered, target, arg, arg2);
	}
	
	[RPC]
	public void applyOnRPC6(int target, int arg, int arg2)
	{
		//GameSkills.instance.getCurrentGameSkill().applyOn(target, arg, arg2);
	}
	
	public void applyOn(int[] targets, int[] args)
	{
		photonView.RPC("applyOnRPC2", PhotonTargets.AllBuffered, targets, args);
	}
	
	[RPC]
	public void applyOnRPC2(int[] targets, int[] args)
	{
		//GameSkills.instance.getCurrentGameSkill().applyOn(targets, args);
	}
	
	public void activateTrap(int idSkill, int[] targets, int[] args)
	{
		photonView.RPC("activateTrapRPC", PhotonTargets.AllBuffered, idSkill, targets, args);
	}
	
	[RPC]
	public void activateTrapRPC(int idSkill, int[] targets, int[] args)
	{
		GameSkills.instance.getSkill(idSkill).activateTrap(targets, args);
	}
	
	public void hideTrap(int[] targets)
	{
		photonView.RPC("hideTrapRPC", PhotonTargets.AllBuffered, targets);
	}
	
	[RPC]
	public void hideTrapRPC(int[] targets)
	{
		GameView.instance.hideTrap(targets [0], targets [1]);
	}
	
	public void failedToCastOnSkill(int[] targets, int[] failures)
	{
		photonView.RPC("failedToCastOnSkillRPC", PhotonTargets.AllBuffered, targets, failures);
	}
	
	[RPC]
	public void failedToCastOnSkillRPC(int[] targets, int[] failures)
	{
		//GameSkills.instance.getCurrentGameSkill().failedToCastOn(targets, failures);
	}
	
	public void failedToCastOnSkill(int target, int failure)
	{
		photonView.RPC("failedToCastOnSkillRPC", PhotonTargets.AllBuffered, target, failure);
	}
	
	[RPC]
	public void failedToCastOnSkillRPC(int target, int failure)
	{
		//GameSkills.instance.getCurrentGameSkill().failedToCastOn(target, failure);
	}
	
	public int nbMyPlayersAlive()
	{
		int compteur = 0;
//		for (int i = 0; i < this.playingCards.Length; i++)
//		{
//			if (!this.getPCC(i).isDead && i != this.currentPlayingCard && this.getPCC(i).isMine)
//			{
//				compteur++;
//			}
//		}
		return compteur;
	}
	
	public int nbOtherPlayersAlive()
	{
		int compteur = 0;
//		for (int i = 0; i < this.playingCards.Length; i++)
//		{
//			if (!this.getPCC(i).isDead && i != this.currentPlayingCard && !this.getPCC(i).isMine)
//			{
//				compteur++;
//			}
//		}
		return compteur;
	}
	
	public int getNbPlayingCards()
	{
		//return (this.playingCards.Length);
		return 0;
	}

	// Méthodes pour le tutoriel

	public void setButtonsGUI(bool value)
	{
//		for (int i =0; i<gameView.gameScreenVM.buttonsEnabled.Length; i++)
//		{
//			gameView.gameScreenVM.buttonsEnabled [i] = value;
//		}
	}
	public void setButtonGUI(int index, bool value)
	{
		//gameView.gameScreenVM.buttonsEnabled [index] = value;
	}
	public void activeSingleCharacter(int index)
	{
		//this.playingCards [index].GetComponent<PlayingCardController>().hideTargetHalo();
	}
	public void activeTargetingOnCharacter(int index)
	{
		//this.playingCards [index].GetComponent<PlayingCardController>().setIsDisable(false);
	}
	public void activeAllCharacters()
	{
//		for (int i=0; i<this.limitCharacterSide; i++)
//		{
//			//this.playingCards [i].GetComponent<PlayingCardController>().hideTargetHalo();
//		}
	}
	public void disableAllCharacters()
	{
//		for (int i=0; i<this.playingCards.Length; i++)
//		{
//			this.playingCards [i].GetComponent<PlayingCardController>().setTargetHalo(new HaloTarget(1), true);
//		}
	}
	public Vector2 getPlayingCardsPosition(int index)
	{
		//return this.playingCards [index].GetComponent<PlayingCardController>().getGOScreenPosition(this.playingCards [index]);
		return new Vector2();
	}
	
	public Vector2 getPlayingCardsSize(int index)
	{
		//return this.playingCards [index].GetComponent<PlayingCardController>().getGOScreenSize(this.playingCards [index]);
		return new Vector2();
	}
	
	public Vector2 getTilesPosition(int x, int y)
	{
		//return this.tiles [x, y].GetComponent<TileController>().getGOScreenPosition(this.tiles [x, y]);
		return new Vector2();
	}
	
	public Vector2 getTilesSize(int x, int y)
	{
		//return this.tiles [x, y].GetComponent<TileController>().getGOScreenSize(this.tiles [x, y]);
		return new Vector2();
	}
	
	public void addTileHalo(int x, int y, int haloIndex, bool isHaloDisabled)
	{
		//this.tiles [x, y].GetComponent<TileController>().setTargetHalo(new HaloTarget(haloIndex), isHaloDisabled);
	}
	public void hideTileHalo(int x, int y)
	{
		//this.tiles [x, y].GetComponent<TileController>().hideTargetHalo();
	}
	public void disableAllSkillObjects()
	{
//		for (int i=0; i<this.skillsObjects.Length; i++)
//		{
//			this.skillsObjects [i].GetComponent<SkillObjectController>().setControlActive(false);
//		}
	}
	public void setAllSkillObjects(bool value)
	{
//		for (int i=0; i<this.skillsObjects.Length; i++)
//		{
//			this.skillsObjects [i].GetComponent<SkillObjectController>().setActive(value);
//		}
	}
	public void activeSingleSkillObjects(int index)
	{
		//this.skillsObjects [index].GetComponent<SkillObjectController>().setControlActive(true);
	}
	public Vector2 getSkillObjectsPosition(int index)
	{
		//return this.skillsObjects [index].GetComponent<SkillObjectController>().getGOScreenPosition(this.skillsObjects [index]);
		return new Vector2();
	}
	public Vector2 getSkillObjectsSize(int index)
	{
		//return this.skillsObjects [index].GetComponent<SkillObjectController>().getGOScreenSize(this.skillsObjects [index]);
		return new Vector2();
	}
	
	public void callTutorial()
	{
		if (GameView.instance.getIsTutorialLaunched())
		{
			//this.tutorial.GetComponent<GameTutorialController>().actionIsDone();
		}
	}
	public bool getIsTutorialLaunched()
	{
		return GameView.instance.getIsTutorialLaunched();
	}
	public void setEndSceneControllerGUI(bool value)
	{
		//EndSceneController.instance.setGUI (value);
	}
	public IEnumerator endTutorial()
	{
//		this.setEndSceneControllerGUI (false);
//		//yield return StartCoroutine (this.users[0].setTutorialStep (5));
//		ApplicationModel.launchGameTutorial = false;
//		Application.LoadLevel ("EndGame");
	yield return 0 ;
	}
	
	public bool amIFirstPlayer()
	{
		return this.isFirstPlayer;
	}
	
	public void launchSkill(int id){
		if (id!=-2){
			if (!GameView.instance.hasMoved(this.currentPlayingCard)){
				GameView.instance.removeDestinations();
			}
			this.isRunningSkill = true ;
			print ("je lance "+id);
			this.startPlayingSkill(id);
		}
		else{
			this.resolvePass();
		}
	}
	
	public void cancelSkill(){
		if (!GameView.instance.hasMoved(this.currentPlayingCard)){
			GameView.instance.displayDestinations();
		}
		GameView.instance.checkSkillsLaunchability();
		GameView.instance.hideTargets();
		this.isRunningSkill = false ;
	}
	
	public bool getIsRunningSkill(){
		return this.isRunningSkill;
	}
	
	public void hitTarget(int c){
		this.targetPCCHandler.addTargetPCC(c);
	}
	
	public void hitTarget(Tile t){
		print (t.x+","+t.y);
		this.targetTileHandler.addTargetTile(t);
	}
	
	public bool isMyCharacterPlaying(){
		return (GameView.instance.getIsMine(this.currentPlayingCard));
	}
	
	public bool hasGameStarted(){
		return (this.nbPlayersReadyToFight==2);
	}
	
	public bool havIStarted(){
		return (this.haveIStarted);
	}
	
//	public void quitGame(){
//		PhotonNetwork.Disconnect();
//	}
//	
	public int getClickedCard(){
		return this.currentClickedCard ;
	}

	public string getMyPlayerName()
	{
		return this.myPlayerName;
	}

	public string getHisPlayerName()
	{
		return this.hisPlayerName;
	}
	
	public bool getIsFirstPlayer()
	{
		return this.isFirstPlayer;
	}
}

