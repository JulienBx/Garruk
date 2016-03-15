using UnityEngine;
using System.Collections.Generic;

public class Frenesie : GameSkill
{
	public Frenesie()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Frénésie";
		base.ciblage = 0 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name,WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power));
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.play(GameView.instance.runningSkill);
		GameController.instance.applyOn(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int i){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int life = Mathf.RoundToInt((0.75f-level*0.05f)*currentCard.getAttack());
		int target = GameView.instance.getCurrentPlayingCard();
		int damages = currentCard.getNormalDamagesAgainst(currentCard, life);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,18,base.name,damages+" dégats subis"), true);
		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(10, -1, 18, base.name, "+10ATK. Permanent"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, base.name+"\n+10ATK\n-"+damages+"PV", 1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 18);
	}
}
