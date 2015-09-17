using UnityEngine;
using System.Collections.Generic;

public class Cannibale : GameSkill
{
	public Cannibale(){
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
		
		GameController.instance.addCardModifier(GameController.instance.getCurrentPlayingCard(), bonusL, ModifierType.Type_BonusMalus, ModifierStat.Stat_Life, -1, -1, "", "", "");
		GameController.instance.addCardModifier(GameController.instance.getCurrentPlayingCard(), bonusA, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 9, "Cannibalisme", "+"+bonusA+" ATK et +"+bonusL+" LIFE", "Permanent");
		
		GameView.instance.displaySkillEffect(GameController.instance.getCurrentPlayingCard(), "+"+bonusL+" PV\n+"+bonusA+" ATK", 4);
		
		GameController.instance.addCardModifier(target, currentLife, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "Esquive", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentAllys();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		string text = "SACRIFICE\n";
		
		int probaEsquive = targetCard.GetEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
