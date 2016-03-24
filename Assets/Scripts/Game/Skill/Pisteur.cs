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
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1));
	}

	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.play(GameView.instance.runningSkill);
		int level = GameView.instance.getCurrentSkill().Power;

		List<Tile> trappedTiles = GameView.instance.getTrappedTiles();
		for(int i = 0 ; i < trappedTiles.Count ; i++){
			if(Random.Range(1,101)<=(50+5*level)){
				GameView.instance.getTileController(trappedTiles[i]).trap.isVisible = true;
				GameView.instance.getTileController(trappedTiles[i]).showTrap(true);

				GameView.instance.displaySkillEffect(trappedTiles[i], "Piège découvert !", 1);
				GameView.instance.addAnim(trappedTiles[i], 14);
			}
		}

		GameController.instance.endPlay();
	}
}
