using UnityEngine;
using System.Collections.Generic;

public class GameSkill
{
	public int numberOfExpectedTargets ; 
	public List<int> targets;
	public List<int> results;
	public List<int> values;
	public List<Tile> tileTargets;
	
	public string name ;
	
	public int ciblage ;
	public bool auto ;
	public int id ;
	
	public virtual void init(Skill s){
		this.targets = new List<int>();
		this.results = new List<int>();
		this.values = new List<int>();
		this.tileTargets = new List<Tile>();
	}
	
	public virtual void launch()
	{
		Debug.Log("Skill non implémenté");
	}

	public virtual int getActionScore(Tile target, Skill s)
	{
		Debug.Log("IA non implémentée "+s.Id);
		return 0 ; 
	}
	
	public virtual void resolve(List<Tile> targetsTile)
	{
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void addTarget(int a, int b)
	{ 
		this.targets.Add(a);
		this.results.Add(b);
	}
	
	public virtual void addTarget(int a, int b, int c)
	{ 
		this.targets.Add(a);
		this.results.Add(b);
		this.values.Add(c);
	}
	
	public virtual void addTarget(Tile t, int b)
	{ 
		this.tileTargets.Add(t);
		this.results.Add(b);
	}
	
	public virtual void applyOn(Tile t)
	{ 
		Debug.Log("Skill non implémenté");
	}

	public virtual int getBestChoice(Tile t, Skill s){
		Debug.Log("Skill non implémenté");
		return -1;
	}
	
	public virtual void applyOn(int target)
	{ 
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void applyOn(int target, int value)
	{ 
		Debug.Log("Skill non implémenté");
	}

	public virtual void applyOnViro(int target, int value)
	{ 
		Debug.Log("Skill non implémenté");
	}

	public virtual void applyOnViro2(int target, int value, int value2)
	{ 
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void applyOn()
	{ 
		Debug.Log("Skill non implémenté");
	}

	public virtual void applyOnMe()
	{ 
		Debug.Log("Skill non implémenté");
	}

	public virtual void applyOnMe(int value)
	{ 
		Debug.Log("Skill non implémenté");
	}

	public virtual void launchFou(int target)
	{ 
		Debug.Log("Skill non implémenté");
	}

	public virtual void activateTrap(int[] targets, int[] args)
	{
		Debug.Log("Skill non implémenté");
	}

	public virtual string isLaunchable(Tile t, bool isM)
	{
		GameCard gc = GameView.instance.getCurrentCard();
		Skill s = GameView.instance.getCurrentSkill();
		string launchability = "" ;
		if (s.hasBeenPlayed){
			launchability = "Compétence déjà utilisée!";
		}
		if(gc.hasPlayed){
			launchability = "Le personnage a déjà joué";
		}
		else if(gc.isFurious()){
			launchability = "Furie : les compétences sont indisponibles";
		}
		else if(gc.isEffraye()){
			launchability = "Paralysé : Ne peut utiliser ses compétences";
		}
		else{
			if(this.ciblage==1){
				launchability = GameView.instance.canLaunchAdjacentOpponents(t, isM);
			}
			else if(this.ciblage==-1){
				launchability = GameView.instance.canLaunchAdjacentOpponents(t, isM);
			}
			else if(this.ciblage==2){
				launchability = GameView.instance.canLaunchAdjacentAllys(t, isM);
			}
			else if(this.ciblage==3){
				launchability = GameView.instance.canLaunchOpponentsTargets(isM);
			}
			else if(this.ciblage==4){
				launchability = GameView.instance.canLaunchAllysButMeTargets(isM);
			}
			else if(this.ciblage==6){
				launchability = GameView.instance.canLaunchAdjacentTileTargets(t);
			}
			else if(this.ciblage==7){
				launchability = GameView.instance.canLaunchAllButMeTargets();
			}
			else if(this.ciblage==8){
				launchability = GameView.instance.canLaunch1TileAwayOpponents(t, isM);
			}
			else if(this.ciblage==9){
				launchability = GameView.instance.canLaunchAdjacentUnits(t);
			}
			else if(this.ciblage==10){
				launchability = GameView.instance.canLaunchMyUnit();
			}
			else if(this.ciblage==11){
				launchability = GameView.instance.canLaunchAdjacentRock(t);
			}
			else if(this.ciblage==14){
				launchability = GameView.instance.canLaunchWoundedAllysButMeTargets(isM);
			}
			else if(this.ciblage==15){
				launchability = GameView.instance.canLaunchAdjacentCristoidOpponents(t, isM);
			}
			else if(this.ciblage==16){
				launchability = GameView.instance.canLaunchNextCristal();
			}
			else if(this.ciblage==17){
				launchability = GameView.instance.canLaunchDead();
			}
			else if(this.ciblage==18){
				launchability = GameView.instance.canLaunchAdjacentDroidOpponents(t, isM);
			}
			else{
				launchability = "";
			}
		}
		return launchability ;
	}

	public virtual void displayTargets()
	{
		Tile t = GameView.instance.getCurrentCardTile();
		GameCard gc = GameView.instance.getCurrentCard();
		Skill s = GameView.instance.getCurrentSkill();

		List<Tile> targets = this.getTargets(t, true);
		for (int i = 0 ; i < targets.Count ; i++){
			Tile t2 = targets[i];
			GameView.instance.targets.Add(t2);
			GameView.instance.getTileController(t2.x,t2.y).displayTarget(true);
			GameView.instance.getTileController(t2).setTargetText(GameSkills.instance.getSkill(this.id).name, GameSkills.instance.getCurrentGameSkill().getTargetText(GameView.instance.getTileCharacterID(targets[i].x,targets[i].y)));	
		}		
	}

	public virtual List<Tile> getTargets(Tile t, bool isM){
		List<Tile> targets ;
		if(this.ciblage==1){
			targets = GameView.instance.getAdjacentOpponentsTargets(t, isM);
		}
		else if(this.ciblage==-1){
			targets = GameView.instance.getAdjacentOpponentsTargets(t, isM);
		}
		else if(this.ciblage==2){
			targets = GameView.instance.getAdjacentAllyTargets(t, isM);
		}
		else if(this.ciblage==3){
			targets = GameView.instance.getOpponentsTargets(isM);
		}
		else if(this.ciblage==4){
			targets = GameView.instance.getAllysButMeTargets(isM);
		}
		else if(this.ciblage==6){
			targets = GameView.instance.getAdjacentTileTargets(t);
		}
		else if(this.ciblage==11){
			targets = GameView.instance.getAdjacentRockTargets(t);
		}
		else if(this.ciblage==7){
			targets = GameView.instance.getAllButMeTargets();
		}
		else if(this.ciblage==8){
			targets = GameView.instance.get1TileAwayOpponentsTargets(t, isM);
		}
		else if(this.ciblage==9){
			targets = GameView.instance.getAdjacentUnitsTargets(t);
		}
		else if(this.ciblage==10){
			targets = GameView.instance.getMyUnitTarget();
		}
		else if(this.ciblage==14){
			targets = GameView.instance.getWoundedAllysButMeTargets(isM);
		}
		else if(this.ciblage==15){
			targets = GameView.instance.getAdjacentCristoidOpponents(isM);
		}
		else if(this.ciblage==18){
			targets = GameView.instance.getAdjacentDroidOpponents(isM);
		}
		else{
			targets = new List<Tile>();
		}
		return targets;
	}

	public virtual string getTargetText(int id)
	{
		return null;
	}
	
	public virtual string getTargetText()
	{
		return null;
	}

	public virtual string getTimelineText(){
		return "" ;
	}

	public virtual void esquive(int target, string s)
	{ 
		string text = "Echec "+s;
		GameView.instance.displaySkillEffect(target, text, 1);
		GameView.instance.addSE(GameView.instance.getTile(target));
		GameView.instance.addAnim(8,GameView.instance.getTile(target));
		SoundController.instance.playSound(27);
	}

	public virtual void esquive(int target, int result)
	{ 
		string text = "";
		int type = 1 ; 
		if(result==1){
			text = "Esquive";
		}

		GameView.instance.displaySkillEffect(target, text, type);
		GameView.instance.addSE(GameView.instance.getTile(target));
		SoundController.instance.playSound(27);
	}
}
