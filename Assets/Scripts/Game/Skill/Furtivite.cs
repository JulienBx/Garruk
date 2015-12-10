using UnityEngine;
using System.Collections.Generic;

public class Furtivite : GameSkill
{
	public Furtivite()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Furtivité";
		base.ciblage = 0 ;
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.play(GameView.instance.runningSkill);
		GameController.instance.applyOn(-1);
		GameController.instance.showResult(true);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		string text = base.name;
		int attack = GameView.instance.getCurrentSkill().Power;
		text += "\n+"+attack+" ATK\nInvisible au prochain tour";
		
		GameView.instance.getCurrentCard().attackModifyers.Add(new Modifyer(attack, 2, 9, base.name, "+"+attack+" ATK"));
		GameView.instance.getCurrentCard().setState(new Modifyer(0, 2, 2, base.name, "Invisible. Ne peut pas etre ciblé par une compétence"));
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).updateAttack();
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).showIcons();
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), text, 0);
	}
}
