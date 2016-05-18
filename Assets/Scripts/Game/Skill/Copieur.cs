using UnityEngine;
using System.Collections.Generic;

public class Copieur : GameSkill
{
	public Copieur(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Copieur";
		base.ciblage = 9 ;
		base.auto = false;
		base.id = 105 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets();
	}
	
	public override void resolve(List<Tile> targets)
	{	
		GameController.instance.play(this.id);
		int target = GameView.instance.getTileCharacterID(targets[0].x, targets[0].y);

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			GameController.instance.applyOnMe(target);
		}
		GameController.instance.playSound(28);
		GameController.instance.endPlay();
	}
	
	public override void applyOnMe(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();

		currentCard.Skills[1].Id = targetCard.Skills[1].Id ;
		currentCard.Skills[1].Power = GameView.instance.getCurrentSkill().Power ;
		GameView.instance.getMyHoveredCardController().updateCharacter();
		GameView.instance.getHisHoveredCardController().updateCharacter();

		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), "Apprend "+WordingSkills.getName(targetCard.getSkills()[1].Id), 2);
		GameView.instance.addAnim(0,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
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

	public override int getActionScore(Tile t, Skill s){
		int score = s.Power*2 ;
		score = score * GameView.instance.IA.getSoutienFactor() ;

		return score ;
	}
}
