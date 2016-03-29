using UnityEngine;
using System.Collections.Generic;

public class Chocbleu : GameSkill
{
	public Chocbleu(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Choc bleu";
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
	
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
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
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(1.2f+0.04f*GameView.instance.getCurrentSkill().Power)));
		int malus = Mathf.RoundToInt(currentCard.getAttack()*0.5f);

		string text = base.name+"\n-"+damages+"PV";				
		GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).attackModifyers.Add(new Modifyer(-1*malus, 1, 11, base.name, (-1*malus)+"ATK. Actif 2 tours"));
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).updateAttack();

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,11,base.name,damages+" dégats subis"), false);
		GameView.instance.displaySkillEffect(target, text, 0);

		GameView.instance.addAnim(GameView.instance.getTile(target), 132);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(1.2f+0.04f*GameView.instance.getCurrentSkill().Power)));
		int malus = Mathf.RoundToInt(currentCard.getAttack()*0.5f);

		string text = base.name+"\n-"+damages+"PV";

		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
