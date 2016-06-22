using UnityEngine;
using System.Collections.Generic;

public class Vampire : GameSkill
{
	public Vampire(){
		this.numberOfExpectedTargets = 0 ; 
		base.ciblage = 16 ;
		base.auto = true;
		base.id = 40 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0), GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
	}
	
	public override void resolve(List<Tile> targetsP)
	{	
		GameController.instance.play(this.id);
		GameCard currentCard = GameView.instance.getCurrentCard();
		List<int> targets = GameView.instance.getEveryoneNextCristal() ; 
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		for(int i = 0 ; i < targets.Count ; i++){
			if (Random.Range(1,101) <= GameView.instance.getCard(targets[i]).getMagicalEsquive()){
				GameController.instance.esquive(targets[i],1);
			}
			else{
				if (Random.Range(1,101) <= proba){
					GameController.instance.applyOn(targets[i]);
				}
				else{
					GameController.instance.esquive(targets[i],this.getText(0));
				}
			}
		}
		GameController.instance.playSound(36);
		GameController.instance.applyOnMe(targets.Count);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = 2+GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, level);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 40, this.getText(0), damages+" dégats subis"),  (target==GameView.instance.getCurrentPlayingCard()),-1);
		GameView.instance.displaySkillEffect(target, "-"+damages+"PV", 0);	
		GameView.instance.addAnim(5,GameView.instance.getTile(target));
	}

	public override void applyOnMe(int value){
		int level = 2+GameView.instance.getCurrentSkill().Power;
		int damages = value*level;

		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer(-1*damages, -1, 40, this.getText(0), damages+" dégats subis"), false,-1);
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0)+"\n+"+damages+"PV", 2);	
		GameView.instance.addAnim(7,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;

		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard;
		List<int> targets = GameView.instance.getEveryoneNextCristal() ; 
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int level = 2+s.Power;
		int life = currentCard.getLife();
		int bonusLife = 0 ;
		int malus ;
		for(int i = 0 ; i < targets.Count ; i++){
			targetCard = GameView.instance.getCard(targets[i]);
			malus = currentCard.getNormalDamagesAgainst(targetCard, level);
			bonusLife+=malus;
			if(targetCard.getLife()<=malus){
				if(targetCard.isMine){
					score += Mathf.RoundToInt((proba-targetCard.getMagicalEsquive())*200f);
				}
				else{
					score -= Mathf.RoundToInt((proba-targetCard.getMagicalEsquive())*200f);
				}
			}
			else{
				if(targetCard.isMine){
					score += Mathf.RoundToInt((proba-targetCard.getMagicalEsquive())*malus);
				}
				else{
					score -= Mathf.RoundToInt((proba-targetCard.getMagicalEsquive())*malus);
				}
			}
		}

		bonusLife = Mathf.Min(bonusLife, currentCard.GetTotalLife()-currentCard.getLife());
		score+=bonusLife;

		score = score * GameView.instance.IA.getAgressiveFactor() ;

		return score ;
	}
}
