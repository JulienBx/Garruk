﻿using UnityEngine;
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
	
	public virtual void resolve(List<int> targetsPCC)
	{
		Debug.Log("Skill non implémenté");
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
	
	public virtual void activateTrap(int[] targets, int[] args)
	{
		Debug.Log("Skill non implémenté");
	}

	public virtual string isLaunchable()
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
		else if(gc.isParalyzed()){
			launchability = "Paralysé : Ne peut utiliser ses compétences";
		}
		else{
			if(this.ciblage==1){
				launchability = GameView.instance.canLaunchAdjacentOpponents();
			}
			else if(this.ciblage==2){
				launchability = GameView.instance.canLaunchAdjacentAllys();
			}
			else if(this.ciblage==3){
				launchability = GameView.instance.canLaunchOpponentsTargets();
			}
			else if(this.ciblage==4){
				launchability = GameView.instance.canLaunchAllysButMeTargets();
			}
			else if(this.ciblage==6){
				launchability = GameView.instance.canLaunchAdjacentTileTargets();
			}
			else if(this.ciblage==7){
				launchability = GameView.instance.canLaunchAllButMeTargets();
			}
			else if(this.ciblage==8){
				launchability = GameView.instance.canLaunch1TileAwayOpponents();
			}
			else if(this.ciblage==9){
				launchability = GameView.instance.canLaunchAdjacentUnits();
			}
			else{
				launchability = "";
			}
		}
		return launchability ;
	}
	
	public virtual HaloTarget getTargetPCCText(Card c)
	{
		return null;
	}
	
	public virtual string getTargetText(int id)
	{
		return null;
	}
	
	public virtual string getTargetText()
	{
		return null;
	}
	
	public virtual HaloTarget getTargetTileText()
	{
		return null;
	}

	public virtual string getTimelineText(){
		return "" ;
	}
	
	public virtual void esquive(int target, int result)
	{ 
		string text = "";
		int type = 1 ; 
		if(result==1){
			text = "Esquive";
		}
		else if(result==2){
			text = "Echec Soin";
		}
		else if(result==3){
			text = "Echec Fortifiant";
		}
		else if(result==4){
			text = "Echec Relaxant";
		}
		else if(result==5){
			text = "Echec Lest";
		}
		else if(result==6){
			text = "Echec Adrénaline";
		}
		else if(result==7){
			text = "Echec Antibiotique";
		}
		else if(result==8){
			text = "Echec Tir à l'arc";
		}
		else if(result==10){
			text = "Echec Assassinat";
		}
		else if(result==15){
			text = "Echec Coupe-Jambes";
		}
		else if(result==11){
			text = "Echec Estoc";
		}
		else if(result==19){
			text = "Echec Cri de rage";
		}
		else if(result==20){
			text = "Echec Terreur";
		}
		else if(result==39){
			text = "Echec Renfoderme";
		}
		else if(result==56){
			text = "Echec Stéroide";
		}
		else if(result==92){
			text = "Echec Déséquilibre";
		}
		else if(result==94){
			text = "Echec Morphine";
		}
		GameView.instance.displaySkillEffect(target, text, type);
		GameView.instance.addSE(GameView.instance.getTile(target));
	}
}
