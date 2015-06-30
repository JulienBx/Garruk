using UnityEngine;
using System.Collections.Generic;

public class Reparation : GameSkill
{
	public override void launch ()
	{
		//GameController.instance.lookForValidation (true, "Concentration cible le héros actif", "Lancer reparation");
	}

	public override void resolve (List<int> targetsPCC)
	{
//		int myPlayerID = GameController.instance.currentPlayingCard;
//		int amount = -1*GameController.instance.getCurrentSkill ().ManaCost;
//		
//		//GameController.instance.addModifier(myPlayerID, amount, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
//		
//		GameController.instance.displaySkillEffect(myPlayerID, "Récupère "+amount+" points de vie", 3,0);
//		GameController.instance.play ();
	}

	public override bool isLaunchable (Skill s)
	{
		return true;
	}
}
