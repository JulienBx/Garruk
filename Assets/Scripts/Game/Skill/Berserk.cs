using UnityEngine;
using System.Collections.Generic;

public class Berserk : GameSkill
{
	public Berserk(){
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
			int arg = Random.Range(1,base.skill.ManaCost+1);
			GameController.instance.applyOn(target,arg);
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 0);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg){
		Card targetCard = GameView.instance.getCard(target);
		int myCurrentLife = base.card.GetLife();
		int currentLife = targetCard.GetLife();
		
		int myBouclier = base.card.GetBouclier();
		int bouclier = targetCard.GetBouclier();
		
		int amount = Mathf.CeilToInt(base.card.GetAttack()*120/100);
		
		int damageBonusPercentage = base.card.GetDamagesPercentageBonus(targetCard);
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		int myAttack = base.skill.ManaCost;
		
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameController.instance.addCardModifier(GameController.instance.getCurrentPlayingCard(), arg, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		
		if(currentLife!=amount){
			GameView.instance.displaySkillEffect(target, "-"+amount+" PV", 5);
		}
		
		if(myCurrentLife!=arg){
			GameView.instance.displaySkillEffect(GameController.instance.getCurrentPlayingCard(), "-"+arg+" PV", 5);
		}
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "Esquive", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
	
	public override string getTargetText(Card targetCard){
		
		int myCurrentLife = base.card.GetLife();
		int currentLife = targetCard.GetLife();
		
		int myBouclier = base.card.GetBouclier();
		int bouclier = targetCard.GetBouclier();
		
		int amount = Mathf.CeilToInt(base.card.GetAttack()*120f/100f);
		
		int damageBonusPercentage = base.card.GetDamagesPercentageBonus(targetCard);
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		int myAttack = base.skill.ManaCost;
		int myAmount = (myAttack)*(100+damageBonusPercentage)/100;
		myAmount = Mathf.Min(currentLife,myAmount-(myBouclier*myAmount/100));
		
		string text = "PV: "+currentLife+"->"+(currentLife-amount)+"\n";
		
		int probaEsquive = targetCard.GetEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
