using UnityEngine;
using System.Collections.Generic;

public class ForetLianesGGU : GameSkill
{
	public ForetLianesGGU()
	{
	
	}
	
	public override void launch()
	{
		GameController.instance.lookForEmptyTileGGU("Choisir une cible à attaquer", "Lancer attaque");
	}
	
	public override void resolve(int[] args)
	{
		int targetX  = args [0];
		int targetY  = args [1];
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		
		int myPlayerID = GameController.instance.currentPlayingCard;
		string myPlayerName = GameController.instance.getCurrentCard().Title;

		GameController.instance.displaySkillEffect(myPlayerID, "Foret de lianes", 3, 2);
		                           
		GameController.instance.addForetLianesGGU(targetX, targetY);

	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
