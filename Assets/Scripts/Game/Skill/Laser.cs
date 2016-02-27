using UnityEngine;
using System.Collections.Generic;

public class Laser : GameSkill
{
	public Laser()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Laser";
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
		int level = GameView.instance.getCurrentSkill().Power;

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
				if(isFou==-1){
					GameController.instance.applyOn(target);
				}
				else{
					GameController.instance.applyOn2(target, Random.Range(10+level, 20+3*level+1));
				}
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
		int damages = currentCard.getMagicalDamagesAgainst(targetCard, value);
		string text = base.name+"\n-"+damages+"PV";
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 22, base.name, damages+" dégats subis"));
		GameView.instance.displaySkillEffect(target, text, 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 22);
	}

	public override void applyOn(int target){
		if(target==-1){
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), "Fou!\nse trompe de cible!", 0);	
			GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 22);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);	
			GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 22);
		}
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
