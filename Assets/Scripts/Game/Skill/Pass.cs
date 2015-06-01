using UnityEngine;

public class Pass : GameSkill
{
	public override void launch()
	{
		Debug.Log("Je passe");

		GameController.instance.resolvePass();
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
