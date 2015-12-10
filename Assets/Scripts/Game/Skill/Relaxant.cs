using UnityEngine;
using System.Collections.Generic;

public class Relaxant : GameSkill
{
	public Relaxant()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Relaxant";
		base.ciblage = 3 ;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		bool isSuccess = false ;
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		
		if (Random.Range(1,101) < GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) < proba){
				GameController.instance.applyOn(target);
				isSuccess = true;
			}
			else{
				GameController.instance.esquive(target,4);
			}
		}
		
		if(GameView.instance.getCurrentCard().isGenerous()){
			List<int> targets = GameView.instance.getOpponents();
			targets.Remove(target);
			target = targets[Random.Range(0,targets.Count)];
			GameController.instance.applyOn(target);
			isSuccess = true ;
		}
		GameController.instance.showResult(isSuccess);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = -1*GameView.instance.getCurrentSkill().Power;
		
		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(level, 1, 3, text, level+" ATK. Actif 1 tour"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, base.name+"\n"+level+" ATK. Actif 1 tour", 1);
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}	

	public override string getTargetText(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = -1*GameView.instance.getCurrentSkill().Power;
		
		text += "\n"+level+" ATK. Actif 1 tour";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
