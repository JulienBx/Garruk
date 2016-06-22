using UnityEngine;
using System.Collections.Generic;

public class ArmureTech : GameSkill
{
	public ArmureTech()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.ciblage = 0 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Armure Tech","Tech Armor"});
		texts.Add(new string[]{"Armure ARG1%","ARG1% Armor"});
		base.auto = true;
		base.id = 45 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0), GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	                     
		GameController.instance.applyOnMe(-1);
		GameController.instance.playSound(28);
		GameController.instance.endPlay();
	}
	
	public override void applyOnMe(int target){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		target = GameView.instance.getCurrentPlayingCard();
		string text = this.getText(0)+"\n"+this.getText(1,new List<int>{10+4*level});
		 
		GameView.instance.getCard(target).setArmureTech(new Modifyer(10+4*level, -1, 45, this.getText(0), ""));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, text, 1);
		GameView.instance.addAnim(0,GameView.instance.getTile(target));

	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int score = 0 ; 

		if(!currentCard.isTechArmor()){
			score += Mathf.RoundToInt(2*(s.Power)*(currentCard.getLife()/50f));
		}
		return score ;
	}
}
