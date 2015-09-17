using UnityEngine;
using System.Collections.Generic;

public class Lest : GameSkill
{
	public Lest()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
		{                             
			GameController.instance.applyOn(target, 0);
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		
		if (base.card.isGenerous()){
			if (Random.Range(1,101) <= base.card.getPassiveManacost()){
				List<int> allys = GameView.instance.getOpponents();
				if(allys.Count>1){
					allys.Remove(target);
					target = allys[Random.Range(0,allys.Count)];
					
					if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
					{
						GameController.instance.applyOn(target,1);
					}
				}
			}
		}
		
		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg){
		int amount = base.skill.ManaCost;
		
		int baseD = GameView.instance.getCard(target).GetMove();
		int deplacement = Mathf.FloorToInt((amount)*baseD/100)+1;
		
		if (deplacement >= baseD){
			deplacement = baseD - 1 ;
		}
		
		GameController.instance.addCardModifier(target, -1*deplacement, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, 8, "Lesté", "-"+deplacement+" MOV", "Actif 1 tour");
		
		if(arg==0){
			GameView.instance.displaySkillEffect(target, "-"+deplacement+" MOV", 5);
		}
		else if (arg==1){
			GameView.instance.displaySkillEffect(target, "BONUS\n-"+deplacement+" MOV", 5);
		}
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "Esquive", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchOpponentsTargets();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		int amount = base.skill.ManaCost;
		
		int baseD = targetCard.GetMove();
		int deplacement = Mathf.FloorToInt((amount)*baseD/100)+1;
		
		if (deplacement >= baseD){
			deplacement = baseD - 1 ;
		}
		
		string text = "MOV: "+baseD+"->"+(baseD-deplacement)+"\n";
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
