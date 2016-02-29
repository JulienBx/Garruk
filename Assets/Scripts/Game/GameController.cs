using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : Photon.MonoBehaviour
{	
	public static GameController instance;
	
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
		string description = "Inflige "+level+" dégats à l'unité piégée" ;
		Trap trap = new Trap(level, 1, (isFirstP==GameView.instance.getIsFirstPlayer()), "Electropiège", description);
		GameView.instance.getTileController(x,y).setTrap(trap);
	}

	public void addCharacter(int id, int atk, int pv, Tile t){
		photonView.RPC("addCharacterRPC", PhotonTargets.AllBuffered, id, atk, pv, t.x, t.y, GameView.instance.getIsFirstPlayer());
	}
	
	[RPC]
	public void addCharacterRPC(int id, int atk, int pv, int x, int y, bool isFirstP){
		GameView.instance.addCharacter(id, atk, pv, x, y, isFirstP);
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
	
	public void addPoisonPiege(int amount, Tile t){
		photonView.RPC("addPoisonpiegeRPC", PhotonTargets.AllBuffered, amount, t.x, t.y);
	}
	
	[RPC]
	public void addPoisonpiegeRPC(int amount, int x, int y){
		string description = "Empoisonné. Inflige "+amount+" dégats en fin de tour" ;
		Trap trap = new Trap(amount, 2, GameView.instance.getCurrentCard().isMine, "Poisonpiège", description);
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

	public void sendShuriken(int target, int nb, int currentCard){
		photonView.RPC("sendShurikenRPC", PhotonTargets.AllBuffered, target, nb, currentCard);
	}

	[RPC]
	public void sendShurikenRPC(int target, int nb, int currentCard)
	{	
		int damages = (5+GameView.instance.getCard(currentCard).Skills[0].Power)*nb;
		string text = "Shuriken\nHIT X"+nb+"\n-"+damages+"PV";
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,67,"Ninja",damages+" dégats subis"));
		GameView.instance.addAnim(GameView.instance.getTile(target), 67);
	}

	public void sendEsquiveShuriken(int target, int currentCard){
		photonView.RPC("sendEsquiveShurikenRPC", PhotonTargets.AllBuffered, target, currentCard);
	}

	[RPC]
	public void sendEsquiveShurikenRPC(int target, int currentCard)
	{	
		string text = "Shuriken\nEsquive!";
		GameView.instance.displaySkillEffect(target, text, 1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 67);
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
	
	public void clickDestination(Tile t, int c, bool toDisplay)
	{
		print(c);
		photonView.RPC("clickDestinationRPC", PhotonTargets.AllBuffered, t.x, t.y, c, GameView.instance.getIsFirstPlayer(), toDisplay);
	}
	
	[RPC]
	public void clickDestinationRPC(int x, int y, int c, bool isFirstP, bool toDisplay)
	{
		GameView.instance.dropCharacter(c, new Tile(x,y), isFirstP, toDisplay);
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

	public void esquive(int target, string s)
	{
		photonView.RPC("esquiveSRPC", PhotonTargets.AllBuffered, target, s);
	}
	
	[RPC]
	public void esquiveSRPC(int target, string s)
	{
		GameSkills.instance.getCurrentGameSkill().esquive(target, s);
	}
	
	public void launchFou(int idSkill, int target)
	{
		photonView.RPC("launchFouRPC", PhotonTargets.AllBuffered, idSkill, target);
	}
	
	[RPC]
	public void launchFouRPC(int idSkill, int target)
	{
		GameSkills.instance.getSkill(idSkill).launchFou(target);
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

	public void applyOnViro(int target, int perc)
	{
		photonView.RPC("applyOnViroRPC", PhotonTargets.AllBuffered, target, perc);
	}
	
	[RPC]
	public void applyOnViroRPC(int target, int value)
	{
		GameSkills.instance.getCurrentGameSkill().applyOnViro(target, value);
	}

	public void applyOnViro2(int target, int perc, int perc2)
	{
		photonView.RPC("applyOnViro2RPC", PhotonTargets.AllBuffered, target, perc, perc2);
	}
	
	[RPC]
	public void applyOnViro2RPC(int target, int value, int value2)
	{
		GameSkills.instance.getCurrentGameSkill().applyOnViro2(target, value, value2);
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
	
	public void endPlay()
	{
		photonView.RPC("endPlayRPC", PhotonTargets.AllBuffered);
	}
	
	[RPC]
	public void endPlayRPC()
	{
		StartCoroutine(GameView.instance.endPlay());
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
		print ("Je load "+isFirstPlayer);
		Deck tutorialDeck = new Deck(name);
		yield return StartCoroutine(tutorialDeck.LoadDeck());

		photonView.RPC("SpawnCharacter", PhotonTargets.AllBuffered, isFirstPlayer, tutorialDeck.Id);
	}
	
	void OnDisconnectedFromPhoton()
	{
		SceneManager.LoadScene("Authentication");
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
		ApplicationModel.player.MyDeck=GameView.instance.getMyDeck();
		if (hasFirstPlayerWon == GameView.instance.getIsFirstPlayer())
		{
			GameView.instance.getMyHoveredCardController().lowerCharacter();
			GameView.instance.getHisHoveredCardController().lowerCharacter();
			ApplicationModel.player.HasWonLastGame=true;

		} 
		else
		{
			GameView.instance.getMyHoveredCardController().lowerCharacter();
			GameView.instance.getHisHoveredCardController().lowerCharacter();
			ApplicationModel.player.PercentageLooser=GameView.instance.getPercentageTotalDamages(false);
			ApplicationModel.player.HasWonLastGame=false;
		}
		PhotonNetwork.LeaveRoom ();
		SceneManager.LoadScene("EndGame");
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
}

