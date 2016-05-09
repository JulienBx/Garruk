using UnityEngine;
using System.Collections.Generic;

public class Caserne : GameSkill
{
	public Caserne(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Caserne";
		base.ciblage = 6 ;
		base.auto = false;
		base.id = 46 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets();
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		GameController.instance.play(this.id);
		int amount = 2+GameView.instance.getCurrentSkill().Power;
		GameController.instance.addCaserne(amount, targetsTile[0]);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}

	public override string getTargetText(int i){
		int amount = 2+GameView.instance.getCurrentSkill().Power;
		string s = "Construit une caserne. Ajoute "+amount+" ATK aux unités stationnées";
		return s ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
		SoundController.instance.playSound(28);
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
