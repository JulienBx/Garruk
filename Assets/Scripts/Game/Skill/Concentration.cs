using UnityEngine;
using System.Collections.Generic;

public class Concentration : GameSkill
{
	public override void launch ()
	{
		//GameController.instance.lookForValidation ();
	}

	public override void resolve (List<int> targetsPCC)
	{
		int myPlayerID = GameController.instance.currentPlayingCard;
		int amount = GameController.instance.getCurrentSkill ().ManaCost;
		
		//GameController.instance.setBonusDamages (amount, -10);
		
		GameController.instance.displaySkillEffect(myPlayerID, "Augmente de "+amount+" % les DMG", 3,0);
		GameController.instance.play ();
	}

	public override bool isLaunchable (Skill s)
	{
		return true;
	}
}
