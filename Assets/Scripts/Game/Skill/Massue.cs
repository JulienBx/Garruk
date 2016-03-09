using UnityEngine;
using System.Collections.Generic;

public class Massue : GameSkill
{
	public Massue()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Massue";
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
		GameCard currentCard = GameView.instance.getCurrentCard();
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(Mathf.RoundToInt(currentCard.getAttack()*50f/100f),Mathf.RoundToInt(currentCard.getAttack()*(120f+10f*GameView.instance.getCurrentCard().Skills[0].Power)/100f)));
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
		int damages = currentCard.getNormalDamagesAgainst(targetCard, value);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,63,base.name,damages+" dégats subis"), false);
		GameView.instance.displaySkillEffect(target, base.name+"\n-"+damages+"PV", 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 63);
	}

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damagesMin = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*0.5f)); ;
		int damagesMax = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(120f+10f*GameView.instance.getCurrentCard().Skills[0].Power)/100f));
		string text = "PV : "+targetCard.getLife()+" -> ["+(targetCard.getLife()-damagesMin)+"-"+(targetCard.getLife()-damagesMax)+"]";
		
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
