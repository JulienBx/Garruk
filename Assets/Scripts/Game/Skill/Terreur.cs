using UnityEngine;
using System.Collections.Generic;

public class Terreur : GameSkill
{
	public Terreur(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Terreur";
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
				int result = -1 ;
				if (Random.Range(1,101)<26){
					result = 1;
				}
				else{
					result = 0;
				}
				GameController.instance.applyOn2(target,result);
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int result){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;

		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(0.5f+0.05f*level)));

		string text = "-"+damages+"PV";				
		if(result==1){
			text+="\nEffrayé";
			GameView.instance.getCard(target).setTerreur(new Modifyer(0, 1, 20, base.name, "Inactif au prochain tour"));
			GameView.instance.getPlayingCardController(target).showIcons();
		}

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,11,base.name,damages+" dégats subis"), false);

		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 20);
	}
		
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(0.5f+0.05f*level)));

		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\n25% de chances de paralyser";				
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
