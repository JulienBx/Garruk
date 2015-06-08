using UnityEngine;

public class Furtivite : GameSkill
{
	public override void launch ()
	{
		Debug.Log ("Je lance Furtivité");
		GameController.instance.lookForValidation (true, "Furtivité cible le héros actif", "Lancer furtivité");
	}

	public override void resolve (int[] args)
	{
		Debug.Log ("Je résous Furtivité");
		int targetID = args [0];
		GameController.instance.getCurrentPCC ().setCannotBeTargeted (true, "Invisible", "Le héros ne peut pas etre ciblé tant qu'il n'a pas activé une de ses compétences");
		GameController.instance.play (GameController.instance.getCurrentCard ().Title + " a lancé furtivité");
		GameController.instance.getCurrentCard ().modifiers.Add (new StatModifier (GameController.instance.getCurrentSkill ().ManaCost, ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
		GameController.instance.reloadTimeline ();
	}

	public override bool isLaunchable (Skill s)
	{
		return true;
	}
}
