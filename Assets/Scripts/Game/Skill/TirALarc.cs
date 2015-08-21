using UnityEngine;
using System.Collections.Generic;

public class TirALarc : GameSkill
{
	public TirALarc()
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
		int nbHitMax = Random.Range(1,3);
		int arg = 0;
		
		for (int i = 0 ; i < nbHitMax ; i++){
			if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
			{                             
				arg++;
			}
		}
		
		if (arg!=0){
			GameController.instance.applyOn(target, arg);
		}
		else{
			GameController.instance.failedToCastOnSkill(target,1);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg){
		
		Card targetCard = GameView.instance.getCard(target);
		int currentLife = targetCard.GetLife();
		int bouclier = targetCard.GetBouclier();
		
		int damageBonusPercentage = this.card.GetDamagesPercentageBonus(targetCard);
		
		int amount = arg*this.skill.ManaCost*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		
		if(currentLife!=(arg*amount)){
			GameView.instance.displaySkillEffect(target, "HIT X"+arg+"\n-"+(arg*amount)+" PV", 5);
		}
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchOpponentsTargets();
	}
	
	public override string getTargetText(Card targetCard){
		
		int proba;
		int probaEsquive = targetCard.GetEsquive();
		int currentLife = targetCard.GetLife();
		int bouclier = targetCard.GetBouclier();
		
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(targetCard);
		int amount = this.skill.ManaCost*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		string text ;
		
		if(currentLife-Mathf.Min(currentLife,amount)==0){
			text = "PV : "+currentLife+"->0";
		}
		else{
			text = "PV : "+currentLife+"->"+(currentLife-Mathf.Min(currentLife,amount))+"-"+(currentLife-Mathf.Min(currentLife,(2*amount)));
		}
		
		text += "\nHIT : ";
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
