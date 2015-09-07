using UnityEngine;
using System.Collections.Generic;

public class Terreur : GameSkill
{
	public Terreur(){
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
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
		{                             
			int arg = Random.Range(1,101);
			GameController.instance.applyOn(target,arg);
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 0);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg){
		Card targetCard = GameView.instance.getCard(target);
		int bouclier = targetCard.GetBouclier();
		int currentLife = targetCard.GetLife();
		int damageBonusPercentage = base.card.GetDamagesPercentageBonus(targetCard);
		int manacost = base.skill.ManaCost;
		int amount = (base.card.GetAttack()/2)*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		
		if (arg<=manacost){
			GameController.instance.addCardModifier(target, amount, ModifierType.Type_Paralized, ModifierStat.Stat_No, 1, 2, "Paralisé", "Ne peut plus utiliser ses compétences", "Actif 1 tour");
			if(currentLife>amount){
				GameView.instance.displaySkillEffect(target, "-"+amount+" PV\nParalyse", 5);
			}
		}
		else{
			if(currentLife>amount){
				GameView.instance.displaySkillEffect(target, "-"+amount+" PV\nEchec paralysie", 4);
			}
		}	
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
	
	public override string getTargetText(Card targetCard){
		
		int bouclier = targetCard.GetBouclier();
		int currentLife = targetCard.GetLife();
		int damageBonusPercentage = base.card.GetDamagesPercentageBonus(targetCard);
		int manacost = base.skill.ManaCost;
		int amount = (base.card.GetAttack()/2)*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		string text = "PV : "+currentLife+"->"+(currentLife-amount)+"\n";
		text += "PARALYSE HIT% : "+manacost;
		
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
