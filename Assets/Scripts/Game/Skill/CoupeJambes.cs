using UnityEngine;
using System.Collections.Generic;

public class CoupeJambes : GameSkill
{
	public CoupeJambes(){
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
			GameController.instance.applyOn(target);
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 0);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target){
		Card targetCard = GameView.instance.getCard(target);
		int bouclier = targetCard.GetBouclier();
		int currentLife = targetCard.GetLife();
		int damageBonusPercentage = base.card.GetDamagesPercentageBonus(targetCard);
		int manacost = base.skill.ManaCost;
		int deplacement = targetCard.GetMove();
		int bonusDeplacement = Mathf.FloorToInt(manacost*deplacement/100)+1;
		if (bonusDeplacement>=deplacement){
			bonusDeplacement = deplacement - 1 ;
		}
		int attack = base.card.GetAttack()/2;
		int amount = attack*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameController.instance.addCardModifier(target, -1*bonusDeplacement, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, 8, "Lenteur", "-"+bonusDeplacement+" MOV", "Actif 1 tour");
		if(currentLife!=amount){
			GameView.instance.displaySkillEffect(target, "HIT\n-"+amount+" PV\n-"+bonusDeplacement+" MOV", 5);
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
		int deplacement = targetCard.GetMove();
		int bonusDeplacement = Mathf.FloorToInt(manacost*deplacement/100)+1;
		if (bonusDeplacement>=deplacement){
			bonusDeplacement = deplacement - 1 ;
		}
		int attack = base.card.GetAttack()/2;
		int amount = attack*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		string text = "PV : "+currentLife+"->"+(currentLife-amount)+"\n";
		text += "MOV : "+deplacement+"->"+Mathf.Max(1,deplacement-bonusDeplacement)+"\n";
		
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
