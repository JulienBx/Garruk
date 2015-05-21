using UnityEngine;
using System.Collections.Generic;

public class Attack : GameSkill
{
	public void cast()
	{
		GameController.instance.lookForTarget(this);
	}

	public void giveTarget(int idTarget)
	{
	
	}
}
