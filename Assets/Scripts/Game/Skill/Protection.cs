using UnityEngine;
using System.Collections.Generic;

public class Protection : GameSkill
{
	public Protection(){
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Protection","Barrier"});
		texts.Add(new string[]{"Equipe 1 bouclier ARG1%","Wears a ARG1% shield"});
		texts.Add(new string[]{". Permanent",". Permanent"});
		texts.Add(new string[]{"Bouclier ARG1%","ARG1% shield"});
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 29 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0),  GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	                     
		GameController.instance.applyOnMe(-1);
		GameController.instance.playSound(28);
		GameController.instance.endPlay();
	}

	public override string getTargetText(int i){
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = 10+level*4;
		string s = this.getText(1, new List<int>{bonusShield});
		return s ;
	}

	public override void applyOnMe(int value){
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = 10+level*4;
		int target = GameView.instance.getCurrentPlayingCard();
		GameCard currentCard = GameView.instance.getCurrentCard();

		GameView.instance.getPlayingCardController(target).addShieldModifyer(new Modifyer(bonusShield, -1, 29, this.getText(0), this.getText(2)));
		GameView.instance.displaySkillEffect(target, this.getText(3, new List<int>{bonusShield}), 2);
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.addAnim(0,GameView.instance.getTile(target));

		if(currentCard.isFou()){
			int myLevel = GameView.instance.getCurrentCard().Skills[0].Power;
			GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer((11-myLevel), -1, 24, this.getText(0), ""), false,-1);
		}
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard currentCard = GameView.instance.getCurrentCard();

		score+=Mathf.RoundToInt((10+4*s.Power-currentCard.getBouclier()));
		if(currentCard.isFou()){
			int damages = 11-currentCard.Skills[0].Power;
			if(damages>=currentCard.getLife()){
				score-=100;
			}
			else{
				score-=damages;
			}
		}

		score = score * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}
}

