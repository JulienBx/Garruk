using UnityEngine;
using System.Collections.Generic;

public class PerfoTir : GameSkill
{
	public PerfoTir()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "PerfoTir";
		base.ciblage = 3 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		if(currentCard.isSniper()){
			proba = 100 ;
		}
		int isFou = 1 ;
		if(currentCard.isFou()){
			if(Random.Range(1,101)<26){
				isFou = -1 ;
				List<int> potentialTargets = GameView.instance.getEveryoneButMe();
				List<int> targets = new List<int>();
				for (int i = 0 ; i < potentialTargets.Count ; i++){
					if(target!=potentialTargets[i]){
						targets.Add(potentialTargets[i]);
					}
				}
				if(targets.Count>0){
					target = targets[Random.Range(0,targets.Count)];
				}
			}
		}
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, isFou);
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
		int level = Mathf.Min(currentCard.getLife(),GameView.instance.getCurrentSkill().Power);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(level, -1, 31, base.name, level+" dégats subis"));
		string text = "-"+level+"PV";
		if(targetCard.getBouclier()>0){
			text+="\nBouclier détruit";
		}
		if(value==-1){
			text+="\nse trompe de cible!";
		}
		GameView.instance.displaySkillEffect(target, text, 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 31);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = Mathf.Min(currentCard.getLife(),GameView.instance.getCurrentSkill().Power);

		string text = "-"+level+"PV";
		if(targetCard.getBouclier()>0){
			text+="\nBouclier détruit";
		}
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
