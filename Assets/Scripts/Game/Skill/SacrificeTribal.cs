using UnityEngine;
using System.Collections.Generic;

public class SacrificeTribal : GameSkill
{
	public SacrificeTribal(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAdjacentAllyTargets();
		GameController.instance.displayMyControls("Sacrifice tribal");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int[] targets = new int[1];
		targets[0] = targetsPCC[0];
		
		GameController.instance.startPlayingSkill();
		
		if (Random.Range(1,100) > GameController.instance.getCard(targetsPCC[0]).GetEsquive())
		{                             
			GameController.instance.applyOn(targets);
		}
		else{
			GameController.instance.failedToCastOnSkill(targets);
		}
		GameController.instance.playSkill();
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
		Card targetCard = GameController.instance.getCard(targets[0]);;
		int currentLife = targetCard.GetLife() ;
		int currentAttack = targetCard.GetAttack() ;
		
		int manacost = GameController.instance.getCurrentSkill().ManaCost ;
		
		int bonusL = currentLife*manacost/100 ;
		int bonusA = currentAttack*manacost/100 ;
		
		GameController.instance.addCardModifier(GameController.instance.currentPlayingCard, -1*bonusL, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameController.instance.addCardModifier(GameController.instance.currentPlayingCard, bonusA, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, -1, "Tribalité", "Bonus d'attaque : +"+bonusA, "Permanent");
		GameController.instance.displaySkillEffect(GameController.instance.currentPlayingCard, "Gagne "+bonusL+" PV et "+bonusA+" ATK", 3, 0);
		GameController.instance.addCardModifier(targets[0], currentLife, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameController.instance.displaySkillEffect(targets[0], "Sacrifié !", 3, 1);
	}
	
	public override void failedToCastOn(int[] targets){
		for (int i = 0 ; i < targets.Length ; i++){
			GameController.instance.displaySkillEffect(targets[i], "Echec", 3, 0);
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
				if (GameController.instance.getPCC(tempInt).canBeTargeted() && ((tempInt < GameController.instance.limitCharacterSide)==GameController.instance.isFirstPlayer))	
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
		
		h.addInfo("Sacrifie le héros",0);
		
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
		return "Sacrifice tribal" ;
	}
}
