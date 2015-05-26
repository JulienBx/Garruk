using UnityEngine;
using System.Collections.Generic;

public class GameSkill
{
	public virtual void launch()
	{
		Debug.Log("Skill non implémenté");
	}

	public virtual void setTarget(PlayingCardController pcc)
	{
		Debug.Log("Skill non implémenté");
	}

	public virtual void resolve(int[] args)
	{
		Debug.Log("Skill non implémenté");
	}
}
