using UnityEngine;
using System.Collections.Generic;

public class Attack : GameSkill
{
	public Attack(){
		this.idSkill = 0 ; 
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAdjacentTargets();
		GameController.instance.displayMyControls("Attaque");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int[] targets = new int[1];
		targets[0] = targetsPCC[0];
		
		if (Random.Range(1,100) > GameController.instance.getCard(targetsPCC[0]).GetEsquive())
		{                             
			GameController.instance.applyOn(this.idSkill, targets);
		}
		else{
			GameController.instance.failedToCastOnSkill(this.idSkill, targets);
		}
		GameController.instance.playSkill(this.idSkill);
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
		Card targetCard ;
		int currentLife ;
		int damageBonusPercentage ;
		int amount ;
		for (int i = 0 ; i < targets.Length ; i++){
			targetCard = GameController.instance.getCard(targets[i]);
			currentLife = targetCard.GetLife();
			damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(targetCard);
			amount = GameController.instance.getCurrentCard().GetAttack()*(100+damageBonusPercentage)/100;
			GameController.instance.addCardModifier(targets[i], amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
			GameController.instance.displaySkillEffect(targets[i], "PV : "+currentLife+" -> "+(currentLife-amount), 3, 1);
		}
	}
	
	public override void failedToCastOn(int[] targets){
		for (int i = 0 ; i < targets.Length ; i++){
			GameController.instance.displaySkillEffect(targets[i], "L'attaque échoue", 3, 0);
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
		
		int currentLife = c.GetLife();
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(c);
		int amount = GameController.instance.getCurrentCard().GetAttack()*(100+damageBonusPercentage)/100;
		
		h.addInfo("PV : "+currentLife+" -> "+Mathf.Max(0,(currentLife-amount)),0);
		
		int probaHit = 100 - c.GetEsquive();
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
		return "Attaque" ;
	}
}
