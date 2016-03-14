using UnityEngine;
using System.Collections.Generic;

public class Furtivite : GameSkill
{
	public Furtivite()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Furtivité";
		base.ciblage = 10 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayMyUnitTarget();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.play(GameView.instance.runningSkill);
		GameController.instance.applyOn(targetsPCC[0]);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		int attack = GameView.instance.getCurrentSkill().Power+5;
		target = GameView.instance.getCurrentPlayingCard();

		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(attack, 2, 9, base.name, "+"+attack+" ATK"));
		GameView.instance.getPlayingCardController(target).updateAttack();

		GameView.instance.getCard(target).magicalEsquiveModifyers.Add(new Modifyer(100, 2, 9, base.name, "Esquive les attaques à distance"));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, base.name+"\n+"+attack+" ATK\nFurtif", 1);
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
