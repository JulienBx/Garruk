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
		StartCoroutine(GameView.instance.clickDestination(new Tile(x,y), c, cancel));
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
		StartCoroutine(GameView.instance.play(skill));
	}
	
	public void displaySkillEffect(int id, string text, int color){
		photonView.RPC("displaySkillEffectRPC", PhotonTargets.AllBuffered, id, text, color);
	}
	
	[RPC]
	public void displaySkillEffectRPC(int id, string text, int color){
		GameView.instance.displaySkillEffect(id, text, color);
	}
	
	public IEnumerator loadTutorialDeck(bool isFirstPlayer, string name)
	{
		Deck tutorialDeck = new Deck(name);
		yield return StartCoroutine(tutorialDeck.LoadDeck());

		photonView.RPC("SpawnCharacter", PhotonTargets.AllBuffered, isFirstPlayer, tutorialDeck.Id);
	}
	
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

