using UnityEngine;
using System.Collections.Generic;

public class Excitant : GameSkill
{
	public Excitant()
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
			GameController.instance.applyOn(target, 0);
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		
		if (base.card.isGenerous()){
			if (Random.Range(1,101) <= base.card.getPassiveManacost()){
				List<int> allys = GameView.instance.getAllys();
				if(allys.Count>1){
					allys.Remove(target);
					target = allys[Random.Range(0,allys.Count)];
					
					if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
					{
						GameController.instance.applyOn(target,1);
					}
				}
			}
		}
		
		GameController.instance.play();
	}
	
//	public override void applyOn(int target, int arg){
//		GameController.instance.advanceTurns(target, base.skill.ManaCost);
//		if(arg==0){
//			GameView.instance.displaySkillEffect(target, "Attente: -"+base.skill.ManaCost+" tours", 5);
//		}
//		else if(arg==1){
//			GameView.instance.displaySkillEffect(target, "Bonus\n Attente: -"+base.skill.ManaCost+" tours", 5);
//		}
//	}
//	
//	public override void failedToCastOn(int target, int indexFailure){
//		if (indexFailure==1){
//			GameView.instance.displaySkillEffect(target, "Esquive", 4);
//		}
//	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchOpponentsTargets();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		int amount = base.skill.ManaCost;
		string text;
		
		text = "Attente : -"+amount+" tours\n";
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
