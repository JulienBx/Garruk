using UnityEngine;
using System.Collections.Generic;

public class Cristolife : GameSkill
{
	public Cristolife()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "CristoLife";
		base.ciblage = 11 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentRockTargets();
	}
	
	public override void resolve(List<Tile> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		Tile target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		int level = GameView.instance.getCurrentSkill().Power;

		if (Random.Range(1,101) <= proba){
			GameController.instance.applyOn2(GameView.instance.getCurrentPlayingCard(), Random.Range(2*level, 6+3*level));
			GameController.instance.removeRock(target);
		}
		else{
			GameController.instance.esquive(GameView.instance.getCurrentPlayingCard(),base.name);
		}
		
		GameController.instance.endPlay();
	}

	public override void applyOn(int target, int value){
		GameView.instance.getCard(target).pvModifyers.Add(new Modifyer(value, -1, 129, base.name, "+"+value+" PV. Permanent"));
		GameView.instance.getPlayingCardController(target).updateLife(GameView.instance.getCurrentCard().getLife());
		GameView.instance.displaySkillEffect(target, base.name+"\n+"+value+" PV. Permanent", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 129);
	}
	
	public override string getTargetText(int target){
		int level = GameView.instance.getCurrentSkill().Power;
		int minBonus = 2*level;
		int maxBonus = 3*level+5;

		string text = "Mange le cristal et gagne : ["+minBonus+" - "+maxBonus+"] PV";
		text += "\n\nHIT% : 100";
		
		return text ;
	}
}
