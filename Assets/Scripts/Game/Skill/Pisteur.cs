using UnityEngine;
using System.Collections.Generic;

public class Pisteur : GameSkill
{
	public Pisteur()
	{
		this.numberOfExpectedTargets = 0 ;
		base.name = "Pisteur";
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 14 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1));
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

				GameView.instance.displaySkillEffect(trappedTiles[i], "Piège découvert !", 1);
				GameView.instance.addAnim(trappedTiles[i], 14);
				compteur++;
			}
		}
		GameController.instance.applyOnMe(compteur);
		GameController.instance.endPlay();
	}

	public override void applyOnMe(int value){
		if(value>0){
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\n"+value+" pièges découverts", 2);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\naucun piège découvert", 2);
		}
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}

	public override int getActionScore(Tile t, Skill s){
		int nbTraps = GameView.instance.countTraps();
		int score = Mathf.RoundToInt(nbTraps*10f*(0.5f+0.05f*s.Power));
		score = score * GameView.instance.IA.getTrapFactor() ;
		return score ;
	}
}
