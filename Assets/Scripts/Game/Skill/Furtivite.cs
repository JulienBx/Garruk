using UnityEngine;
using System.Collections.Generic;

public class Furtivite : GameSkill
{
	public Furtivite()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Furtivité";
		base.ciblage = 10 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name,  WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1));
		GameController.instance.play(GameView.instance.runningSkill);
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOnMe(int target){
		target = GameView.instance.getCurrentPlayingCard();
		GameCard targetCard = GameView.instance.getCard(target);
		int attack = GameView.instance.getCurrentSkill().Power+5;

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(attack, 2, 9, base.name, ". Actif 1 tour"));

		GameView.instance.getPlayingCardController(target).addMagicalEsquiveModifyer(new Modifyer(100, 2, 9, base.name, ". Actif 1 tour"));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, base.name+"\n+"+attack+" ATK", 2);
		GameView.instance.addAnim(GameView.instance.getTile(target), 9);
	}

	public override string getTargetText(int target){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int attack = GameView.instance.getCurrentSkill().Power+5;

		string text = "ATK : "+currentCard.getAttack()+" -> "+(currentCard.getAttack()+attack);

		text += "\n\nHIT% : 100";
		
		return text ;
	}
}
