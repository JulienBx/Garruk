using UnityEngine;
using System.Collections.Generic;

public class Laser : GameSkill
{
	public Laser()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Laser";
		base.ciblage = 3 ;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getMagicalDamagesAgainst(targetCard, 2*level);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 22, base.name, damages+" dégats subis"));
		GameView.instance.displaySkillEffect(target, "-"+damages+"PV", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 22);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getMagicalDamagesAgainst(targetCard, 2*level);

		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages);
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
