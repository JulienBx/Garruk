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
		GameView.instance.displayAdjacentAllyTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
		{                             
			GameController.instance.applyOn(target);
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 0);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target){
		Card targetCard = GameView.instance.getCard(target);;
		int currentLife = targetCard.GetLife() ;
		int currentAttack = targetCard.GetAttack() ;
		
		int manacost = base.skill.ManaCost ;
		
		int bonusL = currentLife*manacost/100 ;
		int bonusA = currentAttack*manacost/100 ;
		
		GameController.instance.addCardModifier(GameController.instance.getCurrentPlayingCard(), -1*bonusL, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameController.instance.addCardModifier(GameController.instance.getCurrentPlayingCard(), bonusA, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 9, "Renforcement", "+"+bonusA+" ATK", "Permanent");
		
		GameView.instance.displaySkillEffect(GameController.instance.getCurrentPlayingCard(), "+"+bonusL+" PV\n+"+bonusA+" ATK", 4);
		
		GameController.instance.addCardModifier(target, currentLife, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentAllys();
	}
	
	public override string getTargetText(Card targetCard){
		
		string text = "SACRIFICE";
		int probaEsquive = targetCard.GetEsquive();
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
