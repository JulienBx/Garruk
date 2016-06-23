using UnityEngine;
using System.Collections.Generic;

public class Pisteur : GameSkill
{
	public Pisteur()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 0 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Pisteur","Pathfinder"});
		texts.Add(new string[]{"Découverte!","Unconvered!"});
		texts.Add(new string[]{"1 piège découvert","1 uncovered trap"});
		texts.Add(new string[]{"ARG1 pièges découverts","ARG1 uncovered traps"});
		texts.Add(new string[]{"Aucun piège découvert","No uncovered traps"});

		base.ciblage = 0 ;
		base.auto = true;
		base.id = 14 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0), GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}

	public override void resolve(List<Tile> targets)
	{	                     
		int level = GameView.instance.getCurrentSkill().Power;

		List<Tile> trappedTiles = GameView.instance.getTrappedTiles();
		int compteur = 0 ;
		for(int i = 0 ; i < trappedTiles.Count ; i++){
			if(Random.Range(1,101)<=(50+5*level)){
				GameView.instance.getTileController(trappedTiles[i]).trap.isVisible = true;
				GameView.instance.getTileController(trappedTiles[i]).showTrap(true);

				GameView.instance.displaySkillEffect(trappedTiles[i], this.getText(1), 1);
				GameView.instance.addAnim(0,trappedTiles[i]);
				compteur++;
			}
		}

		if(compteur>0){
			GameController.instance.playSound(28);
		}
		else{
			GameController.instance.playSound(30);
		}
		GameController.instance.applyOnMe(compteur);
		GameController.instance.endPlay();
	}

	public override void applyOnMe(int value){
		if(value==1){
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0)+"\n"+this.getText(2), 2);
		}
		else if(value>0){
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0)+"\n"+this.getText(3, new List<int>{value}), 2);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0)+"\n"+this.getText(4), 2);
		}
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		int nbTraps = GameView.instance.countTraps();
		int score = Mathf.RoundToInt(nbTraps*10f*(0.5f+0.05f*s.Power));
		score = score * GameView.instance.IA.getTrapFactor() ;
		return score ;
	}
}
