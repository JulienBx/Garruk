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
		base.id = 9 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
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
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}

	public override string getTargetText(int target){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int attack = GameView.instance.getCurrentSkill().Power+5;

		string text = "ATK : "+currentCard.getAttack()+" -> "+(currentCard.getAttack()+attack);

		text += "\n\nHIT% : 100";
		
		return text ;
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard ;
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int score = Mathf.RoundToInt((proba/100f)*(currentCard.getLife()/50f)*(5f+s.Power));
		List<int> enemies = GameView.instance.getOpponents(false);
		for(int i = 0 ; i < enemies.Count ; i++){
			targetCard = GameView.instance.getCard(enemies[i]);
			if(targetCard.CardType.Id==3){
				score+=15;
			}
		}

		score = score * GameView.instance.IA.getSoutienFactor() ;

		return score ;
	}
}
