using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameController : Photon.MonoBehaviour
{	
	public static GameController instance;
	
	private float timePerTurn = 30 ;

	//Variable Photon
	const string roomNamePrefix = "GarrukGame";

	//Variables de gestion
	//bool isReconnecting = false ;
	bool haveIStarted = false ;
	bool isRunningSkill = false ;
	bool bothPlayerLoaded = true ;
	//int nbPlayers = 0 ;
	int nbPlayersReadyToFight = 0; 
	int currentClickedCard = -1;
	
	int turnsToWait ; 
	
	void Awake()
	{
		instance = this;
	}
	
	public void createTile(int x, int y, int type){
		photonView.RPC("AddTileToBoard", PhotonTargets.AllBuffered, x, y, type);
	}
	
	[RPC]
	void AddTileToBoard(int x, int y, int type)
	{
		GameView.instance.createTile(x, y, type, GameView.instance.getIsFirstPlayer());
	}
	
	public void launchCardCreation(){
		photonView.RPC("launchCardCreationRPC", PhotonTargets.AllBuffered);
	}
	
	[RPC]
	public void launchCardCreationRPC(){
		StartCoroutine(GameView.instance.loadMyDeck());
	}
	
	public void spawnCharacter(int idDeck){
		photonView.RPC("SpawnCharacterRPC", PhotonTargets.AllBuffered, GameView.instance.getIsFirstPlayer(), idDeck);
	}
	
	[RPC]
	IEnumerator SpawnCharacterRPC(bool isFirstP, int idDeck)
	{
		Deck deck;
		deck = new Deck(idDeck);
		yield return StartCoroutine(deck.RetrieveCards());
		GameView.instance.loadDeck(deck, isFirstP);
	}
	
	public void addPiegeurTrap(Tile t, int level, bool isFirstP){
		photonView.RPC("addPiegeurTrapRPC", PhotonTargets.AllBuffered, t.x, t.y, isFirstP, level);
	}
	
	[RPC]
	public void addPiegeurTrapRPC(int x, int y, bool isFirstP, int level){
		Trap t = ((Piegeur)GameSkills.instance.getSkill(64)).getTrap(level, isFirstP);
		t.setVisible((isFirstP==GameView.instance.getIsFirstPlayer()));
		GameView.instance.addTrap(t, new Tile(x,y));
	}
	
	public void addElectropiege(int amount, Tile t){
		photonView.RPC("addElectropiegeRPC", PhotonTargets.AllBuffered, amount, t.x, t.y);
	}
	
	[RPC]
	public void addElectropiegeRPC(int amount, int x, int y){
		string description = "Inflige "+amount+" dégats à l'unité piégée" ;
		Trap trap = new Trap(amount, 1, GameView.instance.getCurrentCard().isMine, "Electropiège", description);
		GameView.instance.getTileController(x,y).setTrap(trap);
	}
	
	public void addParapiege(int amount, Tile t, int value){
		photonView.RPC("addParapiegeRPC", PhotonTargets.AllBuffered, amount, t.x, t.y, value);
	}
	
	[RPC]
	public void addParapiegeRPC(int amount, int x, int y, int value){
		string description = "Paralyse l'unité piégee. HIT% : "+ value ;
		Trap trap = new Trap(amount, 2, GameView.instance.getCurrentCard().isMine, "Parapiège", description);
		GameView.instance.getTileController(x,y).setTrap(trap);
	}
	
	public void playerReady(bool isFirstP){
		photonView.RPC("playerReadyRPC", PhotonTargets.AllBuffered, isFirstP);
	}
	
	[RPC]
	public void playerReadyRPC(bool b)
	{	
		GameView.instance.playerReadyR(b);
	}
	
	public void addRankedCharacter(int id, int rank){
		photonView.RPC("addRankedCharacterRPC", PhotonTargets.AllBuffered, id, rank);
	}
	
	[RPC]
	public void addRankedCharacterRPC(int id, int rank)
	{	
		GameView.instance.setTurn(id, rank);
	}

	public void findNextPlayer()
	{
		photonView.RPC("findNextPlayerRPC", PhotonTargets.AllBuffered);
	}
	
	[RPC]
	public void findNextPlayerRPC()
	{
		GameView.instance.setNextPlayer();
	}
	
	public void wakeUp(int id)
	{
		photonView.RPC("wakeUpRPC", PhotonTargets.AllBuffered, id);
	}
	
	[RPC]
	public void wakeUpRPC(int id)
	{
		GameView.instance.getCard(id).removeSleeping();
		GameView.instance.getPlayingCardController(id).showIcons();
	}
	
	public void clickDestination(Tile t, int c, bool cancel)
	{
		photonView.RPC("clickDestinationRPC", PhotonTargets.AllBuffered, t.x, t.y, c, cancel);
	}
	
	[RPC]
	public void clickDestinationRPC(int x, int y, int c, bool cancel)
	{
		GameView.instance.clickDestination(new Tile(x,y), c, cancel);
	}
	
	public void setChosenSkill(int target, int result)
	{
		photonView.RPC("addTargetRPC", PhotonTargets.AllBuffered, target, result);
	}
	
	[RPC]
	public void addTargetRPC(int target, int result)
	{
		GameSkills.instance.getCurrentGameSkill().addTarget(target, result);
	}
	
	public void esquive(int target, int result)
	{
		photonView.RPC("esquiveRPC", PhotonTargets.AllBuffered, target, result);
	}
	
	[RPC]
	public void esquiveRPC(int target, int result)
	{
		GameSkills.instance.getCurrentGameSkill().esquive(target, result);
	}
	
	public void applyOn(int target)
	{
		photonView.RPC("applyOnRPC", PhotonTargets.AllBuffered, target);
	}
	
	[RPC]
	public void applyOnRPC(int target)
	{
		GameSkills.instance.getCurrentGameSkill().applyOn(target);
	}
	
	public void applyOn2(int target, int value)
	{
		photonView.RPC("applyOn2RPC", PhotonTargets.AllBuffered, target, value);
	}
	
	[RPC]
	public void applyOn2RPC(int target, int value)
	{
		GameSkills.instance.getCurrentGameSkill().applyOn(target, value);
	}
	
	public void applyOnTile(Tile t)
	{
		photonView.RPC("applyOnTileRPC", PhotonTargets.AllBuffered, t.x, t.y);
	}
	
	[RPC]
	public void applyOnTileRPC(int arg, int arg2)
	{
		GameSkills.instance.getCurrentGameSkill().applyOn(new Tile(arg, arg2));
	}
	
	public void play(int skill)
	{
		photonView.RPC("playRPC", PhotonTargets.AllBuffered, skill);
	}
	
	[RPC]
	public void playRPC(int skill)
	{
		GameView.instance.play(skill);
	}
	
	
	
		
	public void changePlayingCard(int c){
//		if(this.currentPlayingCard!=-1){
//			GameView.instance.unSelectPC (this.currentPlayingCard);
//			if(this.turnsToWait==0){
//				if(GameView.instance.getIsMine(this.currentPlayingCard)){
//					
//				}
//				else{
//					GameView.instance.showSkills();
//				}
//			}
//			else{
//				if(GameView.instance.getIsMine(this.currentPlayingCard)){
//					if(this.turnsToWait==1){
//						GameView.instance.overloadSkills("A votre adversaire de jouer. "+this.turnsToWait+" tour d'attente");
//					}
//					else{
//						GameView.instance.overloadSkills("A votre adversaire de jouer. "+this.turnsToWait+" tours d'attente");
//					}
//				}
//				else{
//					if(this.turnsToWait==1){
//						GameView.instance.changeOverloadText("A votre adversaire de jouer. "+this.turnsToWait+" tour d'attente");
//					}
//					else{
//						GameView.instance.changeOverloadText("A votre adversaire de jouer. "+this.turnsToWait+" tours d'attente");
//					}
//				}
//			}
//		}
//		else{
//			if(this.turnsToWait==0){
//				GameView.instance.showSkills();
//			}
//			else{
//				if(this.turnsToWait==1){
//					GameView.instance.overloadSkills("A votre adversaire de jouer. "+this.turnsToWait+" tour d'attente");
//				}
//				else{
//					GameView.instance.overloadSkills("A votre adversaire de jouer. "+this.turnsToWait+" tours d'attente");
//				}
//			}
//		}
//		
//		this.currentPlayingCard = c ;
//		GameView.instance.changePlayingCard(c);
	}
	
	
	
	public void displayTR(){
//		for(int i3 = 0 ; i3 < this.playingCardTurnsToWait.Count ; i3++){
//			GameView.instance.getCard(i3).nbTurnsToWait=this.playingCardTurnsToWait[i3];
//			GameView.instance.showTR(i3);
//		}
	}
	
	public void displaySkillEffect(int id, string text, int color){
		photonView.RPC("displaySkillEffectRPC", PhotonTargets.AllBuffered, id, text, color);
	}
	
	[RPC]
	public void displaySkillEffectRPC(int id, string text, int color){
		GameView.instance.displaySkillEffect(id, text, color);
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
	
	public IEnumerator loadTutorialDeck(bool isFirstPlayer, string name)
	{
		Deck tutorialDeck = new Deck(name);
		yield return StartCoroutine(tutorialDeck.LoadDeck());

		photonView.RPC("SpawnCharacter", PhotonTargets.AllBuffered, isFirstPlayer, tutorialDeck.Id);
	}
	
//	[RPC]
//	void AddPlayerToList(int id, string loginName)
//	{
//		print(loginName+" se connecte");
//
//		if (ApplicationModel.username == loginName)
//		{
//			GameView.instance.setMyPlayerName(loginName);
//			this.myPlayerName=loginName;
//			print (myPlayerName);
//		} else
//		{
//			GameView.instance.setHisPlayerName(loginName);
//			this.hisPlayerName=loginName;
//			print (hisPlayerName);
//		}
//		
//		this.nbPlayers++;
//		if(this.nbPlayers==2){
//			if(this.isFirstPlayer==true){
//				PhotonNetwork.room.open = false;
//			}
//		}
//	}	

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
//	void OnJoinedLobby()
//	{	
//		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);    
//		string sqlLobbyFilter = "C0 = " + ApplicationModel.gameType;
//		PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
//	}
//	
//	void OnPhotonRandomJoinFailed()
//	{
//		Debug.Log("Can't join random room! - creating a new room");
//		RoomOptions newRoomOptions = new RoomOptions();
//		newRoomOptions.isOpen = true;
//		newRoomOptions.isVisible = true;
//		newRoomOptions.maxPlayers = 2;
//		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", ApplicationModel.gameType } }; // CO pour une partie simple
//		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
//		
//		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);
//		PhotonNetwork.CreateRoom(roomNamePrefix + Guid.NewGuid().ToString("N"), newRoomOptions, sqlLobby);
//		this.isFirstPlayer = true;
//	}
	
//	void OnJoinedRoom()
//	{
//		if(ApplicationModel.launchGameTutorial)
//		{
//			PhotonNetwork.room.open = false;
//		}
//		Debug.Log("Connecté à Photon");
//		if (!isReconnecting)
//		{
//			photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);
//			if (GameView.instance.getIsTutorialLaunched())
//			{
//				photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID + 1, "Garruk");
//			}
//		} 
//		else
//		{
//			Debug.Log("Mode reconnection");
//		}
//		GameView.instance.createBackground();
//		
//		if (this.isFirstPlayer)
//		{
//			this.initGrid();
//		}
//		StartCoroutine(this.loadMyDeck());
//		
//			
//	}
	
	void OnDisconnectedFromPhoton()
	{
		Application.LoadLevel("Authentication");
	}

	public void quitGameHandler()
	{
		StartCoroutine (GameView.instance.quitGame ());
	}

	public void quitGame(bool hasFirstPlayerWon)
	{
		photonView.RPC("quitGameRPC", PhotonTargets.AllBuffered, hasFirstPlayerWon);
	}
	
	[RPC]
	public void quitGameRPC(bool hasFirstPlayerWon)
	{
		if (hasFirstPlayerWon == GameView.instance.getIsFirstPlayer())
		{
			EndSceneController.instance.displayEndScene(true);
		} 
		else
		{
			EndSceneController.instance.displayEndScene(false);
		}
		PhotonNetwork.LeaveRoom ();
	}
	
	public void addGameEvent(string action, string targetName)
	{
		photonView.RPC("addGameEventRPC", PhotonTargets.AllBuffered, action, targetName);
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

//	public void disconnect()
//	{
//		PhotonNetwork.Disconnect();
//	}

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
	
	public void startPlayingSkill(int id)
	{
		photonView.RPC("startPlayingSkillRPC", PhotonTargets.AllBuffered, id);
	}
	
	[RPC]
	public void startPlayingSkillRPC(int idskill)
	{
//		if (idskill==-1){
//			GameSkills.instance.setCurrentSkill(new Skill("", "", 0));
//		}
//		else {
//			GameSkills.instance.setCurrentSkill(GameView.instance.getCard(this.currentPlayingCard).getSkills()[idskill]);
//		}
//		if (!GameView.instance.getIsMine(this.currentPlayingCard) && idskill!=-1){
//			GameView.instance.selectSkill(idskill);
//		}
//		
//		if(idskill==-1){
//			idskill = 0 ;
//			Skill s = new Skill();
//			GameSkills.instance.getSkill(0).init(GameView.instance.getCard(this.currentPlayingCard), s);
//			if (GameController.instance.isMyCharacterPlaying()){
//				GameSkills.instance.getSkill(0).launch ();
//			}
//		}
//		else{
//			Skill s = GameView.instance.getCard(this.currentPlayingCard).getSkills()[idskill];
//			GameSkills.instance.getSkill(s.Id).init(GameView.instance.getCard(this.currentPlayingCard), s);	
//			if (GameController.instance.isMyCharacterPlaying()){
//				GameSkills.instance.getSkill(s.Id).launch ();
//			}
//		}
	}
	
//	public void addCardModifier(int target, int amount, ModifierType type, ModifierStat stat, int duration, int idIcon, string t, string d, string a)
//	{ 
//		GameView.instance.getCard(target).addModifier(amount, type, stat, duration, idIcon, t, d, a);
//		if(target!=this.getCurrentPlayingCard()){
//			GameView.instance.show(target, true);
//		}
//		else{
//			GameView.instance.show(target, false);
//		}
//		if (stat == ModifierStat.Stat_Dommage)
//		{
//			if (!GameView.instance.isDead(target) && GameView.instance.getCard(target).GetLife() <= 0)
//			{
//				GameView.instance.kill(target);
//			}
//			GameView.instance.updateMyLifeBar();
//			GameView.instance.updateHisLifeBar();
//		}
//	}
//	
//	public void addTileModifier(Tile tile, int amount, ModifierType type, ModifierStat stat, int duration, int idIcon, string t, string d, string a)
//	{ 
//		bool b = false;
//		if(GameView.instance.getIsMine(this.currentPlayingCard)){
//			b = true ;
//		}
//		GameView.instance.setModifier(tile, amount, type, stat, duration, idIcon, t, d, a, b);
//	}
	
	public void clearDeads()
	{
		//deads.Clear();
		foreach (GameObject goTag in GameObject.FindGameObjectsWithTag("deadCharacter"))
		{
			Destroy(goTag);
		}
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
	
	public void applyOnB(int target)
	{
		photonView.RPC("applyOnRPC4", PhotonTargets.AllBuffered, target);
	}
	
	[RPC]
	public void applyOnRPC4(int target)
	{
		//GameSkills.instance.getCurrentGameSkill().applyOn(target);
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
	
	public void launchSkill(int id){
//		if (id!=-2){
//			if (!GameView.instance.hasMoved(this.currentPlayingCard)){
//				GameView.instance.removeDestinations();
//			}
//			this.isRunningSkill = true ;
//			this.startPlayingSkill(id);
//		}
//		else{
//			this.resolvePass();
//		}
	}
	
	public void cancelSkill(){
//		if (!GameView.instance.hasMoved(this.currentPlayingCard)){
//			GameView.instance.displayDestinations(this.currentPlayingCard);
//		}
//		GameView.instance.checkSkillsLaunchability();
//		GameView.instance.hideTargets();
//		this.isRunningSkill = false ;
	}
	
	public bool getIsRunningSkill(){
		return this.isRunningSkill;
	}
	
	public bool hasGameStarted(){
		return (this.nbPlayersReadyToFight==2);
	}
	
	public bool havIStarted(){
		return (this.haveIStarted);
	}
	
	public int getClickedCard(){
		return this.currentClickedCard ;
	}
}

