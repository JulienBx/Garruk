using UnityEngine;
using System.Collections.Generic;

public class Assassinat : GameSkill
{
	public Assassinat(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAdjacentTargets();
		GameController.instance.displayMyControls("Assassinat");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int[] targets = new int[1];
		targets[0] = targetsPCC[0];
		
		GameController.instance.startPlayingSkill();
		
		if (Random.Range(1,100) > GameController.instance.getCard(targetsPCC[0]).GetEsquive())
		{                             
			if (Random.Range(1,100) <= GameController.instance.getCurrentSkill().ManaCost)
			{ 
				GameController.instance.applyOn(targets);
			}
			else{
				GameController.instance.failedToCastOnSkill(targets);
			}
		}
		else{
			GameController.instance.failedToCastOnSkill(targets);
		}
		GameController.instance.playSkill();
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
		
		int currentLife = GameController.instance.getCard(targets[0]).GetLife();
		
		GameController.instance.addCardModifier(targets[0], currentLife, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameController.instance.displaySkillEffect(targets[0], "Assassiné", 3, 1);
	}
	
	public override void failedToCastOn(int[] targets){
		for (int i = 0 ; i < targets.Length ; i++){
			GameController.instance.displaySkillEffect(targets[i], "Esquive", 3, 0);
		}
	}
	
	public override bool isLaunchable(Skill s){
		List<Tile> tempTiles;
		Tile t = GameController.instance.getCurrentPCC().tile;
		
		tempTiles = t.getImmediateNeighbouringTiles();
		bool isLaunchable = false ;
		int i = 0 ;
		int tempInt ; 
		
		while (!isLaunchable && i<tempTiles.Count){
			t = tempTiles[i];
			tempInt = GameController.instance.getTile(t.x, t.y).characterID;
			if (tempInt!=-1)
			{
				if (GameController.instance.getPCC(tempInt).cannotBeTargeted==-1)
				{
					isLaunchable = true ;
				}
			}
			i++;
		}
		return isLaunchable ;
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
		int i ;
		
		h.addInfo("Assassine",0);
		
		int probaHit = (100 - c.GetEsquive())*GameController.instance.getCurrentSkill().ManaCost/100;
		if (probaHit>=80){
			i = 2 ;
		}
		else if (probaHit>=20){
			i = 1 ;
		}
		else{
			i = 0 ;
		}
		h.addInfo("HIT% : "+probaHit,i);
		
		return h ;
	}
	
	public override string getPlayText(){
		return "Assassinat" ;
	}
}
