using UnityEngine;
using System.Collections.Generic;

public class SkillC
{
	public int numberOfExpectedTargets ; 

	public List<int> targets;
	public List<int> results;
	public List<int> values;
	public List<Tile> tileTargets;
	public IList<string[]> texts ;
	public IList<string[]> texts2 ;
	public int ciblage ;
	public bool auto ;
	public int id ;
	public int soundId;
	public int animId;

//	public virtual void init(Skill s){
//		this.targets = new List<int>();
//		this.results = new List<int>();
//		this.values = new List<int>();
//		this.tileTargets = new List<Tile>();
//	} 

	public SkillC(){
		this.ciblage=99;
		this.id = 99;
	}

	public virtual void resolve(int x, int y, Skill skill){
		int targetID = Game.instance.getBoard().getTileC(x,y).getCharacterID();
		CardC target = Game.instance.getCards().getCardC(targetID);
		int level = skill.Power;
		if(UnityEngine.Random.Range(0,101)<=WordingSkills.getProba(this.id, level)){
			if(UnityEngine.Random.Range(0,101)<=target.getEsquive()){
				if(Game.instance.isIA() || Game.instance.isTutorial()){
					Game.instance.getSkills().skills[this.id].dodge(targetID);
				}
				else{
					GameRPC.instance.launchRPC("DodgeSkillRPC", this.id, targetID);
				}
			}
			else{
				if(Game.instance.isIA() || Game.instance.isTutorial()){
					Game.instance.getSkills().skills[this.id].effects(targetID);
				}
				else{
					GameRPC.instance.launchRPC("EffectsSkillRPC", this.id, targetID, level);
				}
			}
		}
		else{
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].fail();
			}
			else{
				GameRPC.instance.launchRPC("FailSkillRPC", this.id);
			}
		}
	}

	public virtual List<TileM> getTargetTiles(CardC card){
		List<TileM> targets = new List<TileM>();
		if(this.ciblage==1){
			targets = Game.instance.getBoard().getAdjacentOpponentsTargets(Game.instance.getBoard().getCurrentBoard(), card);
		}
		return targets;
	}

	public virtual string getEmptyTargetText(){
		string s = "" ;
		if(this.ciblage==1){
			s = WordingGame.getText(72);
		}
		return s;
	}

	public virtual void resolve(Skill s){
		Debug.Log("Skill non implémenté");
	}

	public virtual void effects(int x){
		Debug.Log("Skill non implémenté");
	}

	public virtual void effects(int x, int y){
		Debug.Log("Skill non implémenté");
	}

	public virtual void fail(){
		Game.instance.getCurrentCard().displaySkillEffect(WordingSkills.getName(this.id)+"\n"+WordingGame.getText(75), 2);
		Game.instance.getCurrentCard().launchSkillEffect();
	}

	public virtual void dodge(int x){
		Game.instance.getCurrentCard().displaySkillEffect(WordingSkills.getName(this.id), 1);
		Game.instance.getCards().getCardC(x).displaySkillEffect(WordingGame.getText(76)+"\n"+WordingSkills.getName(this.id),0);
	}

	public virtual string getSkillText(int i, int level){
		Debug.Log("Skill non implémenté");
		return "";
	}

//	public void initTexts(){
//		texts2 = new List<string[]>();
//		texts2.Add(new string[]{"Compétence déjà utilisée","Skill has been used"});
//		texts2.Add(new string[]{"Compétence déjà utilisée","You have already used a skill"});
//		texts2.Add(new string[]{"Unité incontrolable","Your unit is furious"});
//		texts2.Add(new string[]{"Unité paralysée","Paralyzed unit"});
//		texts2.Add(new string[]{"Echec ","Fails "});
//		texts2.Add(new string[]{"Esquive ","Dodge "});
//	}
//
//	public virtual string getText(int id){
//		return this.texts[id][ApplicationModel.player.IdLanguage];
//	}
//
//	public virtual string getText2(int id){
//		return this.texts2[id][ApplicationModel.player.IdLanguage];
//	}
//
//	public virtual string getText(int id, List<int> args){
//		string text = this.texts[id][ApplicationModel.player.IdLanguage];
//		for(int i = 0 ; i < args.Count ; i++){
//			text = text.Replace("ARG"+(i+1), ""+args[i]);
//		}
//
//		return text ;
//	}
//
//	public virtual string getText2(int id, List<int> args){
//		string text = this.texts2[id][ApplicationModel.player.IdLanguage];
//		for(int i = 0 ; i < args.Count ; i++){
//			text = text.Replace("ARG"+(i+1), ""+args[i]);
//		}
//
//		return text ;
//	}
//
//	public virtual void launch()
//	{
//		Debug.Log("Skill non implémenté");
//	}
//
//	public virtual int getActionScore(Tile target, Skill s)
//	{
//		Debug.Log("IA non implémentée "+s.Id);
//		return 0 ; 
//	}	
//	public virtual void addTarget(int a, int b)
//	{ 
//		this.targets.Add(a);
//		this.results.Add(b);
//	}
//	
//	public virtual void addTarget(int a, int b, int c)
//	{ 
//		this.targets.Add(a);
//		this.results.Add(b);
//		this.values.Add(c);
//	}
//	
//	public virtual void addTarget(Tile t, int b)
//	{ 
//		this.tileTargets.Add(t);
//		this.results.Add(b);
//	}
//	
//	public virtual void applyOn(Tile t)
//	{ 
//		Debug.Log("Skill non implémenté");
//	}
//
//	public virtual int getBestChoice(Tile t, Skill s){
//		Debug.Log("Skill non implémenté");
//		return -1;
//	}
//	
//	public virtual void applyOn(int target)
//	{ 
//		Debug.Log("Skill non implémenté");
//	}
//	
//	public virtual void applyOn(int target, int value)
//	{ 
//		Debug.Log("Skill non implémenté");
//	}
//
//	public virtual void applyOnViro(int target, int value)
//	{ 
//		Debug.Log("Skill non implémenté");
//	}
//
//	public virtual void applyOnViro2(int target, int value, int value2)
//	{ 
//		Debug.Log("Skill non implémenté");
//	}
//	
//	public virtual void applyOn()
//	{ 
//		Debug.Log("Skill non implémenté");
//	}
//
//	public virtual void applyOnMe()
//	{ 
//		Debug.Log("Skill non implémenté");
//	}
//
//	public virtual void applyOnMe(int value)
//	{ 
//		Debug.Log("Apply On Me non implémenté");
//	}
//
//	public virtual void launchFou(int target)
//	{ 
//		Debug.Log("Skill non implémenté");
//	}
//
//	public virtual void activateTrap(int[] targets, int[] args)
//	{
//		Debug.Log("Skill non implémenté");
//	}
//
//	public virtual string isLaunchable(Tile t, bool isM)
//	{
//		GameCard gc = GameView.instance.getCurrentCard();
//		Skill s = GameView.instance.getCurrentSkill();
//		string launchability = "" ;
//		if (s.hasBeenPlayed){
//			launchability = this.getText2(0);
//		}
//		if(gc.hasPlayed){
//			launchability = this.getText2(1);
//		}
//		else if(gc.isFurious()){
//			launchability = this.getText2(2);
//		}
//		else if(gc.isEffraye()){
//			launchability = this.getText2(3);
//		}
//		else{
//			if(this.ciblage==1){
//				launchability = GameView.instance.canLaunchAdjacentOpponents(t, isM);
//			}
//			else if(this.ciblage==-1){
//				launchability = GameView.instance.canLaunchAdjacentOpponents(t, isM);
//			}
//			else if(this.ciblage==2){
//				launchability = GameView.instance.canLaunchAdjacentAllys(t, isM);
//			}
//			else if(this.ciblage==3){
//				launchability = GameView.instance.canLaunchOpponentsTargets(isM);
//			}
//			else if(this.ciblage==4){
//				launchability = GameView.instance.canLaunchAllysButMeTargets(isM);
//			}
//			else if(this.ciblage==6){
//				launchability = GameView.instance.canLaunchAdjacentTileTargets(t);
//			}
//			else if(this.ciblage==7){
//				launchability = GameView.instance.canLaunchAllButMeTargets();
//			}
//			else if(this.ciblage==8){
//				launchability = GameView.instance.canLaunch1TileAwayOpponents(t, isM);
//			}
//			else if(this.ciblage==9){
//				launchability = GameView.instance.canLaunchAdjacentUnits(t);
//			}
//			else if(this.ciblage==10){
//				launchability = GameView.instance.canLaunchMyUnit();
//			}
//			else if(this.ciblage==11){
//				launchability = GameView.instance.canLaunchAdjacentRock(t);
//			}
//			else if(this.ciblage==14){
//				launchability = GameView.instance.canLaunchWoundedAllysButMeTargets(isM);
//			}
//			else if(this.ciblage==15){
//				launchability = GameView.instance.canLaunchAdjacentCristoidOpponents(t, isM);
//			}
//			else if(this.ciblage==16){
//				launchability = GameView.instance.canLaunchNextCristal();
//			}
//			else if(this.ciblage==17){
//				launchability = GameView.instance.canLaunchDead();
//			}
//			else if(this.ciblage==18){
//				launchability = GameView.instance.canLaunchAdjacentDroidOpponents(t, isM);
//			}
//			else{
//				launchability = "";
//			}
//		}
//		return launchability ;
//	}
//
//	public virtual void displayTargets()
//	{
//		Tile t = GameView.instance.getCurrentCardTile();
//		GameCard gc = GameView.instance.getCurrentCard();
//		Skill s = GameView.instance.getCurrentSkill();
//
//		List<Tile> targets = this.getTargets(t, true);
//		for (int i = 0 ; i < targets.Count ; i++){
//			Tile t2 = targets[i];
//			GameView.instance.targets.Add(t2);
//			GameView.instance.getTileController(t2.x,t2.y).displayTarget(true);
//			GameView.instance.getTileController(t2).setTargetText(GameSkills.instance.getSkill(this.id).getText(0), GameSkills.instance.getCurrentGameSkill().getTargetText(GameView.instance.getTileCharacterID(targets[i].x,targets[i].y)));	
//		}		
//	}
//
//	public virtual List<Tile> getTargets(Tile t, bool isM){
//		List<Tile> targets ;
//		if(this.ciblage==1){
//			targets = GameView.instance.getAdjacentOpponentsTargets(t, isM);
//		}
//		else if(this.ciblage==-1){
//			targets = GameView.instance.getAdjacentOpponentsTargets(t, isM);
//		}
//		else if(this.ciblage==2){
//			targets = GameView.instance.getAdjacentAllyTargets(t, isM);
//		}
//		else if(this.ciblage==3){
//			targets = GameView.instance.getOpponentsTargets(isM);
//		}
//		else if(this.ciblage==4){
//			targets = GameView.instance.getAllysButMeTargets(isM);
//		}
//		else if(this.ciblage==6){
//			targets = GameView.instance.getAdjacentTileTargets(t);
//		}
//		else if(this.ciblage==11){
//			targets = GameView.instance.getAdjacentRockTargets(t);
//		}
//		else if(this.ciblage==7){
//			targets = GameView.instance.getAllButMeTargets();
//		}
//		else if(this.ciblage==8){
//			targets = GameView.instance.get1TileAwayOpponentsTargets(t, isM);
//		}
//		else if(this.ciblage==9){
//			targets = GameView.instance.getAdjacentUnitsTargets(t);
//		}
//		else if(this.ciblage==10){
//			targets = GameView.instance.getMyUnitTarget();
//		}
//		else if(this.ciblage==14){
//			targets = GameView.instance.getWoundedAllysButMeTargets(isM);
//		}
//		else if(this.ciblage==15){
//			targets = GameView.instance.getAdjacentCristoidOpponents(isM);
//		}
//		else if(this.ciblage==18){
//			targets = GameView.instance.getAdjacentDroidOpponents(isM);
//		}
//		else{
//			targets = new List<Tile>();
//		}
//		return targets;
//	}
//
//	public virtual List<TileM> getTargetTiles(CardC card){
//		List<TileM> targets = new List<TileM>();
//		if(this.ciblage==1){
//			//targets = Game.instance.getBoard().getAdjacentOpponentsTargets(new int[][] board, t, isM);
//		}
//		return targets;
//	}
//
//	public virtual string getTargetText(int id)
//	{
//		return null;
//	}
//	
//	public virtual string getTargetText()
//	{
//		return null;
//	}
//
//	public virtual string getTimelineText(){
//		return "" ;
//	}
//
//	public virtual void esquive(int target, string s)
//	{ 
//		string text = this.getText2(4)+s;
//		GameView.instance.displaySkillEffect(target, text, 1);
//		GameView.instance.addAnim(8,GameView.instance.getTile(target));
//		SoundController.instance.playSound(27);
//	}
//
//	public virtual void esquive(int target, int result)
//	{ 
//		string text = "";
//		int type = 1 ; 
//		if(result==1){
//			text = this.getText2(5);
//		}
//
//		GameView.instance.displaySkillEffect(target, text, type);
//		GameView.instance.addAnim(8,GameView.instance.getTile(target));
//		SoundController.instance.playSound(27);
//	}
}
