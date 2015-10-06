using UnityEngine;

public class Pass : GameSkill
{
	public override void launch()
	{
		Debug.Log("PASS");
		GameController.instance.resolvePass();
	}
	
	public override string isLaunchable(){
		return "" ;
	}
}
