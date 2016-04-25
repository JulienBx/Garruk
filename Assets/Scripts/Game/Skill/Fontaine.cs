using UnityEngine;
using System.Collections.Generic;

public class Fontaine : GameSkill
{
	public Fontaine(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Fontaine";
		base.ciblage = 6 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int amount = 5+2*GameView.instance.getCurrentSkill().Power;
		GameController.instance.addFontaine(amount, targetsTile[0]);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}

	public override string getTargetText(int i){
		int amount = 10+2*GameView.instance.getCurrentSkill().Power;
		string s = "Construit une fontaine. Soigne "+amount+" PV aux unités stationnées";
		return s ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int score = 0 ; 

		if(t.y>=3){
			score+=10-Mathf.Abs(5-t.y);
		}
		return score ;
	}
}
