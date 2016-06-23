using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
	
public class GameController : Photon.MonoBehaviour
{	
	public static GameController instance;
	public IList<string[]> texts ;
	
	void Awake()
	{
		instance = this;
		texts = new List<string[]>();
		texts.Add(new string[]{"Empoisonne!\nARG1 dégat(s) / tour","Poisons the targets!\nARG1 dégat(s) / tour"});
		texts.Add(new string[]{"Piège Poison","Poison Trap"});
		texts.Add(new string[]{"Inflige ARG1 dégats à l'unité piégée","Inflicts ARG1 damages to the trapped unit"});
		texts.Add(new string[]{"Electropiège","ElekTrap"});
		texts.Add(new string[]{"Soigne de ARG1 PV les unités stationnées","Heals ARG1 PV to each visiting unit"});
		texts.Add(new string[]{"Fontaine","Fountain"});
		texts.Add(new string[]{"+ARG1 ATK aux unités stationnées","Adds ARG1 ATK to each visiting unit"});
		texts.Add(new string[]{"Caserne","Barracks"});
		texts.Add(new string[]{"Téléportera dans un rayon de ARG1 cases l'unité touchée","Will teleport the trapped unit ARG1 tile(s) away"});
		texts.Add(new string[]{"TelePiège","TeleporTrap"});
	}

	public virtual string getText(int id){
		return this.texts[id][ApplicationModel.player.IdLanguage];
	}

	public virtual string getText(int id, List<int> args){
		string text = this.texts[id][ApplicationModel.player.IdLanguage];
		for(int i = 0 ; i < args.Count ; i++){
			text = text.Replace("ARG"+(i+1), ""+args[i]);
		}

		return text ;
	}
	
	public void createTile(int x, int y, int type){
		photonView.RPC("AddTileToBoard", PhotonTargets.AllBuffered, x, y, type);
	}
	
	[PunRPC]
	void AddTileToBoard(int x, int y, int type)
	{
		GameView.instance.createTile(x, y, type);
	}
	
	public void addPiegeurTrap(Tile t, int level, bool isFirstP){
		photonView.RPC("addPiegeurTrapRPC", PhotonTargets.AllBuffered, t.x, t.y, isFirstP, level);
	}
	
	[PunRPC]
	public void addPiegeurTrapRPC(int x, int y, bool isFirstP, int level){
		string description = this.getText(0, new List<int>{level}) ;
		Trap trap = new Trap(level, 2, (isFirstP==GameView.instance.getIsFirstPlayer()), this.getText(1), description);
		GameView.instance.getTileController(x,y).setTrap(trap);
	}
	
	public void addElectropiege(int amount, Tile t){
		photonView.RPC("addElectropiegeRPC", PhotonTargets.AllBuffered, amount, t.x, t.y);
	}
	
	[PunRPC]
	public void addElectropiegeRPC(int amount, int x, int y){
		string description = this.getText(2, new List<int>{amount}) ;
		Trap trap = new Trap(amount, 1, GameView.instance.getCurrentCard().isMine, this.getText(3), description);
		GameView.instance.getTileController(x,y).setTrap(trap);
	}

	public void addFontaine(int amount, Tile t){
		photonView.RPC("addFontaineRPC", PhotonTargets.AllBuffered, amount, t.x, t.y);
	}
	
	[PunRPC]
	public void addFontaineRPC(int amount, int x, int y){
		string description = this.getText(4, new List<int>{amount}) ;
		Trap trap = new Trap(amount, 4, true, this.getText(5), description);
		GameView.instance.getTileController(x,y).setTrap(trap);
	}

	public void addCaserne(int amount, Tile t){
		photonView.RPC("addCaserneRPC", PhotonTargets.AllBuffered, amount, t.x, t.y);
	}
	
	[PunRPC]
	public void addCaserneRPC(int amount, int x, int y){
		string description = this.getText(6, new List<int>{amount}) ;
		Trap trap = new Trap(amount, 5, true, this.getText(7), description);
		GameView.instance.getTileController(x,y).setTrap(trap);
	}

	public void removeRock(Tile t){
		photonView.RPC("removeRockRPC", PhotonTargets.AllBuffered, t.x, t.y);
	}
	
	[PunRPC]
	public void removeRockRPC(int x, int y){
		GameView.instance.getTileController(x,y).removeRock();
		GameView.instance.recalculateDestinations();
	}

	public void addRock(Tile t, int type){
		photonView.RPC("addRockRPC", PhotonTargets.AllBuffered, t.x, t.y, type);
	}
	
	[PunRPC]
	public void addRockRPC(int x, int y, int type){
		GameView.instance.getTileController(x,y).addRock(type);
	}
	
	public void addPoisonPiege(int amount, Tile t){
		photonView.RPC("addPoisonpiegeRPC", PhotonTargets.AllBuffered, amount, t.x, t.y);
	}
	
	[PunRPC]
	public void addPoisonpiegeRPC(int amount, int x, int y){
		string description = this.getText(0, new List<int>{amount}) ;
		Trap trap = new Trap(amount, 2, GameView.instance.getCurrentCard().isMine, this.getText(1), description);
		GameView.instance.getTileController(x,y).setTrap(trap);
	}

	public void addTelepiege(int amount, Tile t){
		photonView.RPC("addTelepiegeRPC", PhotonTargets.AllBuffered, amount, t.x, t.y);
	}
	
	[PunRPC]
	public void addTelepiegeRPC(int amount, int x, int y){
		string description = this.getText(8, new List<int>{amount}) ;
		Trap trap = new Trap(amount, 3, GameView.instance.getCurrentCard().isMine, this.getText(9), description);
		GameView.instance.getTileController(x,y).setTrap(trap);
	}
	
	public void playerReady(bool isFirstP){
		photonView.RPC("playerReadyRPC", PhotonTargets.AllBuffered, isFirstP);
	}
	
	[PunRPC]
	public void playerReadyRPC(bool b)
	{	
		GameView.instance.playerReadyR(b);
	}

	public void sendShuriken(int target, int nb, int currentCard){
		photonView.RPC("sendShurikenRPC", PhotonTargets.AllBuffered, target, nb, currentCard);
	}

	[PunRPC]
	public void sendShurikenRPC(int target, int nb, int currentCard)
	{	
		int damages = (2+GameView.instance.getCard(currentCard).Skills[0].Power)*nb;
		string text = "Shuriken\nHIT X"+nb+"\n-"+damages+"PV";
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,67,"Ninja",damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.addAnim(5,GameView.instance.getTile(target));
	}

	public void sendEsquiveShuriken(int target, int currentCard){
		photonView.RPC("sendEsquiveShurikenRPC", PhotonTargets.AllBuffered, target, currentCard);
	}

	[PunRPC]
	public void sendEsquiveShurikenRPC(int target, int currentCard)
	{	
		string text = "Shuriken\nEsquive!";
		GameView.instance.displaySkillEffect(target, text, 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(target));
	}

	public void findNextPlayer(bool b)
	{
		photonView.RPC("findNextPlayerRPC", PhotonTargets.AllBuffered, b);
	}
	
	[PunRPC]
	public void findNextPlayerRPC(bool b)
	{
		GameView.instance.setNextPlayer(b);
	}
	
	public void clickDestination(Tile t, int c, bool toDisplay)
	{
		//print(toDisplay);
		GameView.instance.draggingCard = -1;

		photonView.RPC("clickDestinationRPC", PhotonTargets.AllBuffered, t.x, t.y, c, GameView.instance.getIsFirstPlayer(), toDisplay);
	}
	
	[PunRPC]
	public void clickDestinationRPC(int x, int y, int c, bool isFirstP, bool toDisplay)
	{
		GameView.instance.dropCharacter(c, new Tile(x,y), isFirstP, toDisplay);
	}
	
	public void setChosenSkill(int target, int result)
	{
		photonView.RPC("addTargetRPC", PhotonTargets.AllBuffered, target, result);
	}
	
	[PunRPC]
	public void addTargetRPC(int target, int result)
	{
		GameSkills.instance.getCurrentGameSkill().addTarget(target, result);
	}
	
	public void esquive(int target, int result)
	{
		photonView.RPC("esquiveRPC", PhotonTargets.AllBuffered, target, result);
	}
	
	[PunRPC]
	public void esquiveRPC(int target, int result)
	{
		GameSkills.instance.getCurrentGameSkill().esquive(target, result);
	}

	public void esquive(int target, string s)
	{
		photonView.RPC("esquiveSRPC", PhotonTargets.AllBuffered, target, s);
	}
	
	[PunRPC]
	public void esquiveSRPC(int target, string s)
	{
		GameSkills.instance.getCurrentGameSkill().esquive(target, s);
	}
	
	public void launchFou(int idSkill, int target)
	{
		photonView.RPC("launchFouRPC", PhotonTargets.AllBuffered, idSkill, target);
	}
	
	[PunRPC]
	public void launchFouRPC(int idSkill, int target)
	{
		GameSkills.instance.getSkill(idSkill).launchFou(target);
	}

	public void applyOn(int target)
	{
		photonView.RPC("applyOnRPC", PhotonTargets.AllBuffered, target);
	}
	
	[PunRPC]
	public void applyOnRPC(int target)
	{
		GameSkills.instance.getCurrentGameSkill().applyOn(target);
	}

	public void applyOnMe(int value)
	{
		photonView.RPC("applyOnMeRPC", PhotonTargets.AllBuffered, value);
	}
	
	[PunRPC]
	public void applyOnMeRPC(int value)
	{
		GameSkills.instance.getCurrentGameSkill().applyOnMe(value);
	}

	public void applyOnViro(int target, int perc)
	{
		photonView.RPC("applyOnViroRPC", PhotonTargets.AllBuffered, target, perc);
	}
	
	[PunRPC]
	public void applyOnViroRPC(int target, int value)
	{
		GameSkills.instance.getCurrentGameSkill().applyOnViro(target, value);
	}

	public void applyOnViro2(int target, int perc, int perc2)
	{
		photonView.RPC("applyOnViro2RPC", PhotonTargets.AllBuffered, target, perc, perc2);
	}
	
	[PunRPC]
	public void applyOnViro2RPC(int target, int value, int value2)
	{
		GameSkills.instance.getCurrentGameSkill().applyOnViro2(target, value, value2);
	}
	
	public void applyOn2(int target, int value)
	{
		photonView.RPC("applyOn2RPC", PhotonTargets.AllBuffered, target, value);
	}
	
	[PunRPC]
	public void applyOn2RPC(int target, int value)
	{
		GameSkills.instance.getCurrentGameSkill().applyOn(target, value);
	}
	
	public void applyOnTile(Tile t)
	{
		photonView.RPC("applyOnTileRPC", PhotonTargets.AllBuffered, t.x, t.y);
	}
	
	[PunRPC]
	public void applyOnTileRPC(int arg, int arg2)
	{
		GameSkills.instance.getCurrentGameSkill().applyOn(new Tile(arg, arg2));
	}
	
	public void play(int skill)
	{
		photonView.RPC("playRPC", PhotonTargets.AllBuffered, skill);
	}
	
	[PunRPC]
	public void playRPC(int skill)
	{
		GameView.instance.play(skill);
	}
	
	public void endPlay()
	{
		photonView.RPC("endPlayRPC", PhotonTargets.AllBuffered);
	}
	
	[PunRPC]
	public void endPlayRPC()
	{
		StartCoroutine(GameView.instance.endPlay());
	}
	
	public void displaySkillEffect(int id, string text, int color){
		photonView.RPC("displaySkillEffectRPC", PhotonTargets.AllBuffered, id, text, color);
	}
	
	[PunRPC]
	public void displaySkillEffectRPC(int id, string text, int color){
		GameView.instance.displaySkillEffect(id, text, color);
	}

    public void quitGameHandler(bool hasFirstPlayerLost)
	{
        photonView.RPC("quitGameHandlerRPC", PhotonTargets.AllBuffered,hasFirstPlayerLost);
    }

    [PunRPC]
    public void quitGameHandlerRPC(bool hasFirstPlayerLost)
    {
        StartCoroutine(GameView.instance.quitGame(hasFirstPlayerLost,false));
    }

	public void quitGame()
	{
		ApplicationModel.player.ShouldQuitGame=true;
		PhotonNetwork.LeaveRoom ();
	}

	public void disconnect()
	{
		PhotonNetwork.Disconnect();
	}

	void OnLeftRoom()
	{
		if(ApplicationModel.player.ShouldQuitGame)
		{
			ApplicationModel.player.ShouldQuitGame=false;
			if(ApplicationModel.player.ToLaunchGameTutorial)
			{
				ApplicationModel.player.ToLaunchGameTutorial=false;
				SceneManager.LoadScene("NewHomePage");
			}
			else
			{
				SceneManager.LoadScene("EndGame");
			}
		}
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
	
	[PunRPC]
	public void applyOnRPC5()
	{
		GameSkills.instance.getCurrentGameSkill().applyOn();
	}
	
	public void applyOn(int[] targets)
	{
		photonView.RPC("applyOnRPC", PhotonTargets.AllBuffered, targets);
	}
	
	[PunRPC]
	public void applyOnRPC(int[] targets)
	{
		//GameSkills.instance.getCurrentGameSkill().applyOn(targets);
	}
	
	public void applyOnB(int target)
	{
		photonView.RPC("applyOnRPC4", PhotonTargets.AllBuffered, target);
	}
	
	[PunRPC]
	public void applyOnRPC4(int target)
	{
		//GameSkills.instance.getCurrentGameSkill().applyOn(target);
	}
	
	public void addTarget(int target, int result, int value)
	{
		photonView.RPC("addTargetValueRPC", PhotonTargets.AllBuffered, target, result, value);
	}
	
	[PunRPC]
	public void addTargetValueRPC(int target, int result, int value)
	{
		GameSkills.instance.getCurrentGameSkill().addTarget(target, result, value);
	}
	
	public void addTargetTile(int tileX, int tileY, int result)
	{
		photonView.RPC("addTargetTileRPC", PhotonTargets.AllBuffered, tileX, tileY, result);
	}
	
	[PunRPC]
	public void addTargetTileRPC(int tileX, int tileY, int result)
	{
		GameSkills.instance.getCurrentGameSkill().addTarget(new Tile(tileX, tileY), result);
	}
	
	public void applyOn(int[] targets, int[] args)
	{
		photonView.RPC("applyOnRPC2", PhotonTargets.AllBuffered, targets, args);
	}
	
	[PunRPC]
	public void applyOnRPC2(int[] targets, int[] args)
	{
		//GameSkills.instance.getCurrentGameSkill().applyOn(targets, args);
	}
	
	public void activateTrap(int idSkill, int[] targets, int[] args)
	{
		photonView.RPC("activateTrapRPC", PhotonTargets.AllBuffered, idSkill, targets, args);
	}
	
	[PunRPC]
	public void activateTrapRPC(int idSkill, int[] targets, int[] args)
	{
		GameSkills.instance.getSkill(idSkill).activateTrap(targets, args);
	}
	
	public void hideTrap(int[] targets)
	{
		photonView.RPC("hideTrapRPC", PhotonTargets.AllBuffered, targets);
	}
	
	[PunRPC]
	public void hideTrapRPC(int[] targets)
	{
		GameView.instance.hideTrap(targets [0], targets [1]);
	}
	
	public void failedToCastOnSkill(int[] targets, int[] failures)
	{
		photonView.RPC("failedToCastOnSkillRPC", PhotonTargets.AllBuffered, targets, failures);
	}
	
	[PunRPC]
	public void failedToCastOnSkillRPC(int[] targets, int[] failures)
	{
		//GameSkills.instance.getCurrentGameSkill().failedToCastOn(targets, failures);
	}
	
	public void failedToCastOnSkill(int target, int failure)
	{
		photonView.RPC("failedToCastOnSkillRPC", PhotonTargets.AllBuffered, target, failure);
	}

	public void purify(int target, bool b)
	{
		photonView.RPC("purifyRPC", PhotonTargets.AllBuffered, target, b);
	}
	
	[PunRPC]
	public void purifyRPC(int target, bool b)
	{
		GameView.instance.purify(target, b);
	}

	public void convert(int target)
	{
		photonView.RPC("convertRPC", PhotonTargets.AllBuffered, target);
	}
	
	[PunRPC]
	public void convertRPC(int target)
	{
		GameView.instance.convert(target);
	}

	public void playSound(int id)
	{
		photonView.RPC("playSoundRPC", PhotonTargets.AllBuffered, id);
	}
	
	[PunRPC]
	public void playSoundRPC(int id)
	{
		SoundController.instance.playSound(id);
	}

	public void addAttackPretresse(int id, int value)
	{
		photonView.RPC("addAttackPretresseRPC", PhotonTargets.AllBuffered, id, value);
	}
	
	[PunRPC]
	public void addAttackPretresseRPC(int id, int value)
	{
		GameView.instance.getPlayingCardController(id).updateAttack(GameView.instance.getCard(id).getAttack());
		GameView.instance.getPlayingCardController(id).addAttackModifyer(new Modifyer(value, 1, 112, "Prêtresse", ". Actif 1 tour"));
		GameView.instance.displaySkillEffect(id, "Prêtresse\n+"+value+" ATK", 2);
		GameView.instance.addAnim(7,GameView.instance.getTile(id));
	}
}

