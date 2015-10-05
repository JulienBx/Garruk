using UnityEngine;

public class Pass : GameSkill
{
	public override void launch()
	{
		Debug.Log("PASS");
		GameView.instance.getGC().resolvePass();
	}
	
	public override string isLaunchable(){
		return "" ;
	}
}
