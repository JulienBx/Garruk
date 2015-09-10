using UnityEngine;
using System.Collections.Generic;

public class Calmant : GameSkill
{
	public Calmant()
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
		GameController.instance.backTurns(target, base.skill.ManaCost);
		if(arg==0){
			GameView.instance.displaySkillEffect(target, "Temps d'attente : +"+base.skill.ManaCost, 5);
		}
		else if (arg==1){
			GameView.instance.displaySkillEffect(target, "BONUS\nTemps d'attente : +"+base.skill.ManaCost, 5);
		}
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		if (indexFailure==1){
			GameView.instance.displaySkillEffect(target, "Esquive", 4);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchOpponentsTargets();
	}
	
	public override string getTargetText(Card targetCard){
		
		int amount = base.skill.ManaCost;
		string text;
		
		text = "Temps d'attente : +"+amount+" tours\n";
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
