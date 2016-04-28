using UnityEngine;
using System.Collections.Generic;

public class Reparation : GameSkill
{
	public Reparation()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Réparation";
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 36 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name,WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	                     
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusMin = level;
		int bonusMax = level*2+5;
		GameController.instance.applyOnMe(Mathf.Min(currentCard.GetTotalLife()-currentCard.getLife(),UnityEngine.Random.Range(bonusMin, bonusMax+1)));
		GameController.instance.endPlay();
	}
	
	public override void applyOnMe(int i){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int target = GameView.instance.getCurrentPlayingCard();

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(-1*i,-1,36,base.name,i+" dégats soignés"), false,-1);
		GameView.instance.displaySkillEffect(target, base.name+"\n+"+i+"PV", 2);
		GameView.instance.addAnim(GameView.instance.getTile(target), 36);
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int missingLife = currentCard.GetTotalLife()-currentCard.GetTotalLife();
		int levelMin = s.Power;
		int levelMax = s.Power*2+5;

		int score = Mathf.RoundToInt(((missingLife*(Mathf.Max(0f,levelMax-missingLife)))+(((levelMin+Mathf.Min(levelMax,missingLife))/2f)*Mathf.Min(levelMax,missingLife)))/(levelMax-levelMin+1f));
				
		score = score * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}
}
