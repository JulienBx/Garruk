using UnityEngine;
using System.Collections.Generic;

public class RobotSpecialise : GameSkill
{
	public RobotSpecialise()
	{
	
	}
	
	public override void launch()
	{
		GameController.instance.lookForTarget("Choisir une cible à attaquer", "Lancer robot spécialisé");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args [0];
		int bonusDamage = GameController.instance.getCurrentSkill().ManaCost;
		
		int myPlayerID = GameController.instance.currentPlayingCard;
		
		GameController.instance.displaySkillEffect(myPlayerID, "Robot spécialisé", 3, 2);                            
		GameController.instance.setRobotSpecialise(targetID, bonusDamage, GameController.instance.getCard(targetID).ArtIndex);
		
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
