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
		base.id = 18 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name,GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	                     
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOnMe(int i){
		GameCard currentCard = GameView.instance.getCurrentCard();

		int level = GameView.instance.getCurrentSkill().Power;
		int life = Mathf.RoundToInt((0.55f-level*0.05f)*currentCard.getAttack());
		int target = GameView.instance.getCurrentPlayingCard();
		int damages = currentCard.getNormalDamagesAgainst(currentCard, life);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,18,base.name,damages+" dégats subis"), true,-1);
		GameView.instance.getPlayingCardController(target).updateAttack(currentCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(5, -1, 18, base.name, ". Permanent"));
		GameView.instance.displaySkillEffect(target, base.name+"\n+5ATK\n-"+damages+"PV", 1);
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
		SoundController.instance.playSound(37);
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int score = 10;
		int life = Mathf.RoundToInt((0.55f-s.Power*0.05f)*currentCard.getAttack());
		int damages = currentCard.getNormalDamagesAgainst(currentCard, life);

		score += Mathf.Min(0,currentCard.getLife()-damages-40);

		if(damages>currentCard.getLife()){
			score-=100;
		}

		score = score * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}
}
