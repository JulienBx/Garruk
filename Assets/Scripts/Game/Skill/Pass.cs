using UnityEngine;

public class Pass : GameSkill
{
	public override void launch(Skill skill)
	{
		Debug.Log("Je passe");

		GameController.instance.addPassEvent();
		GameController.instance.findNextPlayer();
	}
}
