using UnityEngine;
using System.Collections.Generic;

public class Renaissance : GameSkill
{
	public Renaissance()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Renaissance";
		base.ciblage = 2 ;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentAllyTargets();
	}

	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.esquive(target,39);
			}
		}
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = level*5;
		
		GameView.instance.getCard(target).addShieldModifyer(new Modifyer(bonusShield, -1, 39, base.name, "Bouclier : "+bonusShield+"%. Permanent"));
		GameView.instance.displaySkillEffect(target, "Renaissance ", 1);
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.addAnim(GameView.instance.getTile(target), 39);
	}	

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);

		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = 5*level;
		
		string text = "Renaissance "+bonusShield+"%";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
