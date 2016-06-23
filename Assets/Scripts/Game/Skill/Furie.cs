using UnityEngine;
using System.Collections.Generic;

public class Furie : GameSkill
{
	public Furie()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Furie","Fury"});
		texts.Add(new string[]{"+ARG1 ATK", "+ARG1 ATK"});
		texts.Add(new string[]{"+ARG1 PV", "+ARG1 HP"});
		texts.Add(new string[]{". Permanent",". Permanent"});
		texts.Add(new string[]{"Incontrolable!","Uncontrolled!"});
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 93 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0), WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	                     
		GameController.instance.applyOnMe(-1);
		GameController.instance.playSound(37);
		GameController.instance.endPlay();
	}
	
	public override void applyOnMe(int target){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusLife = level*2+10;
		int bonusAttack = 5+level;
		target = GameView.instance.getCurrentPlayingCard();

		string text = this.getText(0)+"\n"+this.getText(1, new List<int>{bonusAttack});
		if(bonusLife>0){
			text+="\n+"+this.getText(2, new List<int>{bonusLife});
		}

		GameView.instance.getPlayingCardController(target).updateAttack(currentCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(bonusAttack, -1, 93, this.getText(0), this.getText(3)));
		GameView.instance.getPlayingCardController(target).updateLife(currentCard.getLife());
		GameView.instance.getPlayingCardController(target).addPVModifyer(new Modifyer(bonusLife, -1, 93, this.getText(0), this.getText(3)));

		GameView.instance.getCard(target).setFurious(new Modifyer(0, -1, 93, this.getText(0), this.getText(4)));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, text, 1);
		GameView.instance.addAnim(7,GameView.instance.getTile(target));

	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int score = 0;
		List<int> allys = GameView.instance.getAllys(false);
		if(allys.Count<=1){
			score+=50;
		}

		score+=10-currentCard.getLife();
		return score ;
	}
}
