using UnityEngine;

public class Furtivite : GameSkill
{
	public override void launch ()
	{
		GameController.instance.lookForValidation (true, "Furtivité cible le héros actif", "Lancer furtivité");
	}

	public override void resolve (int[] args)
	{
		int targetID = args [0];
		int myPlayerID = GameController.instance.currentPlayingCard;
		int amount = GameController.instance.getCurrentCard().Attack*GameController.instance.getCurrentSkill ().ManaCost/100;
		
		GameController.instance.setCannotBeTargeted ();
		GameController.instance.addModifier (GameController.instance.currentPlayingCard, amount, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Attack, 1);
		
		GameController.instance.displaySkillEffect(myPlayerID, "devient intouchable et gagne "+amount+" ATK", 3,0);
		
		GameController.instance.play ();
	}

	public override bool isLaunchable (Skill s)
	{
		return true;
	}
}
