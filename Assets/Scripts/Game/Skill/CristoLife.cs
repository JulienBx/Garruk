using UnityEngine;
using System.Collections.Generic;

public class Cristolife : GameSkill
{
	public Cristolife()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Cristo Life","Cristo Life"});
		texts.Add(new string[]{". Permanent",". Permanent"});
		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
		texts.Add(new string[]{"Mange le cristal et gagne [ARG1-ARG2] PV","Eats the cristal et earns [ARG1-ARG2] HP"}); 
		base.ciblage = 11 ;
		base.auto = false;
		base.id = 129 ;
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
			GameController.instance.applyOnMe(Random.Range(5+level, 10+level));
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
		GameView.instance.getPlayingCardController(target).addPVModifyer(new Modifyer(value, -1, 129, this.getText(0), this.getText(1)));
		GameView.instance.getPlayingCardController(target).updateLife(GameView.instance.getCurrentCard().getLife());
		GameView.instance.displaySkillEffect(target, this.getText(0)+"\n"+this.getText(2,new List<int>{value}), 2);	
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){
		int level = GameView.instance.getCurrentSkill().Power;
		int minBonus = 2*level;
		int maxBonus = 2*level+6;

		string text = this.getText(2,new List<int>{minBonus, maxBonus});
		text += "\nHIT% : 100";
		
		return text ;
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);

		int score = Mathf.RoundToInt((proba/100f)*(currentCard.getAttack()/20f)*(7.5f+s.Power)) ;
		score = score * GameView.instance.IA.getSoutienFactor() ;

		return score ;
	}
}
