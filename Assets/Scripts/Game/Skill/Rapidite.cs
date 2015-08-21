using UnityEngine;
using System.Collections.Generic;

public class Rapidite : GameSkill
{
	public Rapidite()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAllysButMeTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
		{                             
			GameController.instance.applyOn(target);
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target){
		int amount = base.skill.ManaCost;
		
		int baseD = GameView.instance.getCard(target).GetMove();
		int deplacement = Mathf.FloorToInt((amount)*baseD/100)+1;
		
		if (deplacement >= baseD){
			deplacement = baseD - 1 ;
		}
		
		GameController.instance.addCardModifier(target, deplacement, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, 9, "Rapidité", "+"+deplacement+" MOV", "Actif 1 tour");
		GameView.instance.displaySkillEffect(target, "HIT\n+"+deplacement+" MOV", 4);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAllysButMeTargets();
	}
	
	public override string getTargetText(Card targetCard){
		
		int amount = base.skill.ManaCost;
		
		int baseD = targetCard.GetMove();
		int deplacement = Mathf.FloorToInt((amount)*baseD/100)+1;
		
		if (deplacement >= baseD){
			deplacement = baseD - 1 ;
		}
		
		string text = "MOV : "+baseD+"->"+(baseD+deplacement)+"\n";
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int proba ;
		text += "HIT : ";
		if (probaEsquive!=0){
			proba = 100-probaEsquive;
			text+=proba+"% : "+100+"%(ATT) - "+probaEsquive+"%(ESQ)";
		}
		else{
			proba = 100;
			text+=proba+"%";
		}
		
		return text ;
	}
}
