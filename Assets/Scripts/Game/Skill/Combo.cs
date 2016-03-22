using UnityEngine;
using System.Collections.Generic;

public class Combo : GameSkill
{
	public Combo(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Combo";
		base.ciblage = 1 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		int max = 6+GameView.instance.getCurrentSkill().Power;
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(1,max+1));
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.endPlay();
	}

	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard,value*Mathf.RoundToInt(20*currentCard.getAttack()/100f));
		string text = base.name+"\nHIT X"+value+"\n-"+damages+"PV";

		if (currentCard.isLache() && !currentCard.hasMoved){
			damages = currentCard.getNormalDamagesAgainst(targetCard, damages+5+currentCard.getSkills()[0].Power);
			text = base.name+"\nHIT X"+value+"\n-"+damages+"PV\n(lâche)";			
		}

		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,12,base.name,damages+" dégats subis"), false);
		GameView.instance.addAnim(GameView.instance.getTile(target), 12);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damagesMin = currentCard.getNormalDamagesAgainst(targetCard,Mathf.RoundToInt(20*currentCard.getAttack()/100f));
		int damagesMax = currentCard.getNormalDamagesAgainst(targetCard,(6+GameView.instance.getCurrentSkill().Power)*Mathf.RoundToInt(20*currentCard.getAttack()/100f));
		string text = "PV : "+currentCard.getLife()+" -> ["+(currentCard.getLife()-damagesMax)+"-"+(currentCard.getLife()-damagesMin)+"]";

		if (currentCard.isLache() && !currentCard.hasMoved){
			damagesMin = currentCard.getNormalDamagesAgainst(targetCard,damagesMin+5+currentCard.getSkills()[0].Power);
			damagesMax = currentCard.getNormalDamagesAgainst(targetCard,damagesMax+5+currentCard.getSkills()[0].Power);
		
			text = "PV : "+currentCard.getLife()+" -> ["+(currentCard.getLife()-damagesMax)+"-"+(currentCard.getLife()-damagesMin)+"]";	
		}
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
