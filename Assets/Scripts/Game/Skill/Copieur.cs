using UnityEngine;
using System.Collections.Generic;

public class Copieur : GameSkill
{
	public Copieur(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Copieur";
		base.ciblage = 9 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentUnitsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			GameController.instance.applyOnMe(target);
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOnMe(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();

		currentCard.Skills[1].Id = targetCard.Skills[1].Id ;
		currentCard.Skills[1].Power = targetCard.Skills[1].Power ;
		currentCard.Skills[1].proba = targetCard.Skills[1].proba;
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();

		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), "Apprend "+WordingSkills.getName(targetCard.getSkills()[1].Id), 2);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 105);
	}

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();

		string text = "Copiera la compétence : "+WordingSkills.getName(targetCard.getSkills()[1].Id);

		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
