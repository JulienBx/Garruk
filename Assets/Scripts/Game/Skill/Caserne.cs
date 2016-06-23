using UnityEngine;
using System.Collections.Generic;

public class Caserne : GameSkill
{
	public Caserne(){
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Caserne","Barracks"});
		texts.Add(new string[]{"Construit une caserne. Ajoute ARG1 ATK aux unités stationnées","Builds barracks. Adds ARG1 ATK to every unit visiting the building"});
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
		GameController.instance.playSound(28);
		GameController.instance.endPlay();
	}

	public override string getTargetText(int i){
		int amount = 2+GameView.instance.getCurrentSkill().Power;
		string s = this.getText(1,new List<int>{amount});
		return s ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));

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
