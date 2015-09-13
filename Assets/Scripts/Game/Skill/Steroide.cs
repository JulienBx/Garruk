using UnityEngine;
using System.Collections.Generic;

public class Steroide : GameSkill
{
	public Steroide()
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
			int arg = Random.Range(1,base.skill.ManaCost+1);
			GameController.instance.applyOn(target,arg,0);
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
						int arg = Random.Range(1,base.skill.ManaCost+1);
						GameController.instance.applyOn(target,arg,1);
					}
				}
			}
		}
		
		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg, int arg2){
		GameController.instance.addCardModifier(target, arg, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 9, "Renforcé", "+"+arg+" ATK", "Permanent");
		GameView.instance.displaySkillEffect(target, "+"+arg+" ATK", 5);
		
		if(arg2==0){
			GameView.instance.displaySkillEffect(target, "+"+arg+" ATK", 5);
		}
		else if(arg2==1){
			GameView.instance.displaySkillEffect(target, "BONUS\n+"+arg+" ATK", 5);
		}
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "Esquive", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAllysButMeTargets();
	}
	
	public override string getTargetText(Card targetCard){
		
		int amount = base.skill.ManaCost;
		int attack = targetCard.GetAttack();
		string text;
		
		text = "ATK : "+attack+"->"+(attack+1)+"-"+(attack+amount)+"\n";
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
