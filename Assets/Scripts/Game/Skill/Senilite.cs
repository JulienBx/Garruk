using UnityEngine;
using System.Collections.Generic;

public class Senilite : GameSkill
{
	public Senilite()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Sénilité";
		base.ciblage = 1 ; 
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
		int max = 2 * GameView.instance.getCurrentSkill().Power+1;
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(1,max));
			}
			else{
				GameController.instance.esquive(target,56);
			}
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		value = -1*Mathf.Min(value, targetCard.getAttack()-1);

		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(value, -1, 56, base.name, value+" ATK. Permanent"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, value+" ATK", 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 57);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = 2*GameView.instance.getCurrentSkill().Power;
		string text = "";

		if(targetCard.getAttack()>2){
			text = "ATK : "+targetCard.getAttack()+" -> ["+Mathf.Max(1,targetCard.getAttack()-1)+"-"+Mathf.Max(1,(targetCard.getAttack()-level))+"]\nPermanent";
		}
		else{
			text = "ATK : "+targetCard.getAttack()+" -> 1\nPermanent";
		}

		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
