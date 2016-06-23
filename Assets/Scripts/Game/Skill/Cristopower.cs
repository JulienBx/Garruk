using UnityEngine;
using System.Collections.Generic;

public class Cristopower : GameSkill
{
	public Cristopower()
	{
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Cristo Power","Cristo Power"});
		texts.Add(new string[]{". Permanent",". Permanent"});
		texts.Add(new string[]{"+ARG1 ATK","+ARG1 ATK"});
		texts.Add(new string[]{"Mange le cristal et gagne [ARG1-ARG2] ATK","Eats the cristal et earns [ARG1-ARG2] ATK"});
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
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(value, -1, 128, this.getText(0), this.getText(1)));
		GameView.instance.displaySkillEffect(target, this.getText(0)+"\n"+this.getText(2, new List<int>{value}), 2);	
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){
		int level = GameView.instance.getCurrentSkill().Power;
		int minBonus = level;
		int maxBonus = level + 4 ;

		string text = this.getText(2, new List<int>{minBonus, maxBonus});
		text += "\nHIT% : 100";
		
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
