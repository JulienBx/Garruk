using UnityEngine;
using System.Collections.Generic;

public class AttaqueFrontale : GameSkill
{
	public AttaqueFrontale(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAdjacentTargets();
		GameController.instance.displayMyControls("Attaque frontale");
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
//		Card targetCard = GameController.instance.getCard(targets[0]);
//		int myCurrentLife = GameController.instance.getCurrentCard().GetLife();
//		int currentLife = targetCard.GetLife();
//		
//		int myBouclier = GameController.instance.getCurrentCard().GetBouclier();
//		int bouclier = targetCard.GetBouclier();
//		
//		int myAttack = GameController.instance.getCurrentCard().GetAttack();
//		int attack = targetCard.GetAttack();
//		
//		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(targetCard);
//		int amount = (bonusAttack/2)*(100+damageBonusPercentage)/100;
//		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
//		
//		GameController.instance.addCardModifier(targets[0], amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//		GameController.instance.addCardModifier(targets[0], -1*bonusAttack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 1, 5, "Sape", "Attaque diminuée de "+bonusAttack, "Actif 1 tour");
//		
//		GameController.instance.displaySkillEffect(targets[0], "PV : "+currentLife+" -> "+Mathf.Max(0,(currentLife-amount))+"\nATK : "+currentAttack+" -> "+Mathf.Max(0,(currentAttack-bonusAttack)), 3, 1);
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
		
		int currentAttack = c.GetAttack();
		int currentLife = c.GetLife();
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(c);
		int bouclier = c.GetBouclier();
		int bonusAttack = GameController.instance.getCurrentSkill().ManaCost;
		int amount = (bonusAttack/2)*(100+damageBonusPercentage)/100;
		amount = amount-(bouclier*amount/100);
		
		h.addInfo("PV : "+currentLife+" -> "+Mathf.Max(0,(currentLife-amount)),0);
		h.addInfo("ATK : "+currentAttack+" -> "+Mathf.Max(0,(currentAttack-bonusAttack)),0);
		
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
		return "Attaque précise" ;
	}
}
