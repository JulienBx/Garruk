using UnityEngine;
using System.Collections.Generic;

public class FrappeBrute : GameSkill
{
	public FrappeBrute()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		int successType = 0 ;
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
		{                             
			int arg = Random.Range(1,base.skill.ManaCost+1)*GameController.instance.getCurrentCard().GetAttack()/100;
			GameController.instance.applyOn(target, arg);
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 0);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg){
		Card targetCard = GameView.instance.getCard(target);
		int currentLife = targetCard.GetLife();
		GameController.instance.addCardModifier(target, arg, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		if(currentLife>arg){
			GameView.instance.displaySkillEffect(target, "-"+arg+" PV", 5);
		}
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
	
	public override string getTargetText(Card targetCard){
		
		int currentLife = targetCard.GetLife();
		
		string text = "PV : "+currentLife+"->"+(currentLife-1)+"-"+(Mathf.Max(0,currentLife-base.skill.ManaCost))+"\n";
	
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
