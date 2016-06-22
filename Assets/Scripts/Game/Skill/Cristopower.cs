using UnityEngine;
using System.Collections.Generic;

public class Cristopower : GameSkill
{
	public Cristopower()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.ciblage = 11 ;
		base.auto = false;
		base.id = 128 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets();
	}
	
	public override void resolve(List<Tile> targetsPCC)
	{	
		GameController.instance.play(this.id);
		Tile target = targetsPCC[0];
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int level = GameView.instance.getCurrentSkill().Power;

		if (Random.Range(1,101) <= proba){
			GameController.instance.applyOnMe(Random.Range(level, 1+4+level));
			GameController.instance.removeRock(target);
		}
		else{
			GameController.instance.esquive(GameView.instance.getCurrentPlayingCard(),this.getText(0));
		}
		GameController.instance.playSound(37);

		GameController.instance.endPlay();
	}

	public override void applyOnMe(int value){
		int target = GameView.instance.getCurrentPlayingCard();
		GameCard targetCard = GameView.instance.getCard(target);
		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(value, -1, 128, this.getText(0), ". Permanent"));
		GameView.instance.displaySkillEffect(target, this.getText(0)+"\n+"+value+" ATK", 2);	
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){
		int level = GameView.instance.getCurrentSkill().Power;
		int minBonus = level;
		int maxBonus = level + 4 ;

		string text = "Mange le cristal et gagne : ["+minBonus+" - "+maxBonus+"] ATK";
		text += "\n\nHIT% : 100";
		
		return text ;
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);

		int score = Mathf.RoundToInt((proba/100f)*(currentCard.getLife()/50f)*(2f)*(3f+1.5f*s.Power)) ;
		score = score * GameView.instance.IA.getSoutienFactor() ;

		return score ;
	}
}
