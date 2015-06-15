using UnityEngine;

public class Concentration : GameSkill
{
	public override void launch ()
	{
		GameController.instance.lookForValidation (true, "Concentration cible le héros actif", "Lancer concentration");
	}

	public override void resolve (int[] args)
	{
		int myPlayerID = GameController.instance.currentPlayingCard;
		int amount = GameController.instance.getCurrentSkill ().ManaCost;
		
		GameController.instance.setBonusDamages (amount, -10);
		
		GameController.instance.displaySkillEffect(myPlayerID, "Augmente de "+amount+" % les DMG", 3,0);
		GameController.instance.play ();
	}

	public override bool isLaunchable (Skill s)
	{
		return true;
	}
}
