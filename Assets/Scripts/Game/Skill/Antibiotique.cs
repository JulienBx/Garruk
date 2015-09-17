using UnityEngine;
using System.Collections.Generic;

public class Antibiotique : GameSkill
{
	public Antibiotique()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAllButMeModifiersTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		int successChances = base.skill.ManaCost;
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
		{                             
			if (Random.Range(1,101) <= successChances)
			{ 
				GameController.instance.applyOn(target,0);
			}
			else{
				GameController.instance.failedToCastOnSkill(target, 2);
			}
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		
		if (base.card.isGenerous()){
			if (Random.Range(1,101) <= base.card.getPassiveManacost()){
				List<int> allys = GameView.instance.getEveryoneButMe();
				if(allys.Count>1){
					allys.Remove(target);
					target = allys[Random.Range(0,allys.Count)];
					
					if (Random.Range(1,101) <= successChances)
					{
						if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
						{
							GameController.instance.applyOn(target,1);
						}
					}
				}
			}
		}
		
		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg){
		GameView.instance.getCard(target).emptyModifiers();
		if(target!=GameController.instance.getCurrentPlayingCard()){
			GameView.instance.show(target, true);
		}
		else{
			GameView.instance.show(target, false);
		}
		if(arg==0){
			GameView.instance.displaySkillEffect(target, "Effets supprimés !", 5);
		}
		else if (arg==1){
			GameView.instance.displaySkillEffect(target, "BONUS\nEffets supprimés !", 5);
		}
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		if (indexFailure==1){
			GameView.instance.displaySkillEffect(target, "Esquive", 4);
		}
		else if (indexFailure==2){
			GameView.instance.displaySkillEffect(target, "Sans effet", 4);
		}
	}
	
	public override string isLaunchable()
	{
		return GameView.instance.canLaunchAllButMeModifiersTargets();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		int amount = base.skill.ManaCost;
		int attack = base.card.GetAttack();
		string text;
		
		text = "Dissipe les effets\n";
		
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int probaHit = Mathf.Max(0,(amount*(100-probaEsquive)/100)) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
