using UnityEngine;
using System.Collections.Generic;

public class Justice : GameSkill
{
	public Justice()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Justice","Justice"});
		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
		texts.Add(new string[]{"+ARG1 PV","+ARG1 HP"});
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 95 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0),  GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		GameController.instance.applyOn(-1);

		int targetMax = GameView.instance.getMaxPVCard();
		int targetMin = GameView.instance.getMinPVCard();
		if(targetMin!=GameView.instance.getCurrentPlayingCard() && targetMax!=GameView.instance.getCurrentPlayingCard()){
			GameController.instance.applyOnMe(-1);
		}
		GameController.instance.playSound(30);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		int targetMax = GameView.instance.getMaxPVCard();
		int targetMin = GameView.instance.getMinPVCard();
		int level = GameView.instance.getCurrentSkill().Power*2+5;

		int malus = Mathf.Min(GameView.instance.getCard(targetMax).getLife(),level);
		GameView.instance.getPlayingCardController(targetMax).addDamagesModifyer(new Modifyer(malus,-1,1,this.getText(0),""), (targetMax==GameView.instance.getCurrentPlayingCard()), -1);
		GameView.instance.displaySkillEffect(targetMax, this.getText(1, new List<int>{malus}), 0);
		GameView.instance.addAnim(4,GameView.instance.getTile(targetMax));

		int bonus = Mathf.Min(GameView.instance.getCard(targetMax).GetTotalLife()-GameView.instance.getCard(targetMax).getLife(),level);
		GameView.instance.getPlayingCardController(targetMin).addDamagesModifyer(new Modifyer(-1*bonus,-1,1,this.getText(0),""), false, -1);
		GameView.instance.displaySkillEffect(targetMin, this.getText(2, new List<int>{malus}), 2);
		GameView.instance.addAnim(0,GameView.instance.getTile(targetMin));

	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		float score = 0 ;
		GameCard targetCardMin = GameView.instance.getCard(GameView.instance.getMinPVCard());
		GameCard targetCardMax = GameView.instance.getCard(GameView.instance.getMaxPVCard());
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int bonusMax = currentCard.getNormalDamagesAgainst(targetCardMax, 5+2*s.Power);
		int bonusMin = Mathf.Min(targetCardMin.GetTotalLife()-targetCardMin.getLife(), 5+2*s.Power);

		if(targetCardMax.getLife()<=bonusMax){
			if(targetCardMax.isMine){
				score+=100;
			}
			else{
				score-=100;
			}
		}
		else{
			if(targetCardMax.isMine){
				score+=bonusMax+5-targetCardMax.getLife()/10f;
			}
			else{
				score-=bonusMax+5-targetCardMax.getLife()/10f;
			}
		}

		if(targetCardMin.isMine){
			score-=bonusMin;
		}
		else{
			score+=bonusMin;
		}

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return (int)score ;
	}
}
