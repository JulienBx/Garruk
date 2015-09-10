using UnityEngine;
using System.Collections.Generic;

public class Fortifiant : GameSkill
{
	public Fortifiant()
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
			GameController.instance.applyOn(target,0);
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		
		if (base.card.isGenerous()){
			if (Random.Range(1,101) <= base.card.getPassiveManacost()){
				List<int> allys = GameView.instance.getAllys();
				if(allys.Count>1){
					allys.Remove(target);
					target = Random.Range(0,allys.Count+1);
					
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
		int arg2 = base.skill.ManaCost;
		GameController.instance.addCardModifier(target, arg2, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 1, 9, "Renforcé", "+"+arg+" ATK", "Actif 1 tour");
		if(arg==0){
			GameView.instance.displaySkillEffect(target, "+"+arg2+" ATK / 1 tour", 5);
		}
		else if (arg==1){
			GameView.instance.displaySkillEffect(target, "BONUS\n+"+arg2+" ATK / 1 tour", 5);
		}
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "Esquive", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchOpponentsTargets();
	}
	
	public override string getTargetText(Card targetCard){
		
		int amount = base.skill.ManaCost;
		int attack = base.card.GetAttack();
		string text;
		
		text = "ATK : "+attack+"->"+(attack+amount);
	
		int probaEsquive = targetCard.GetMagicalEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
