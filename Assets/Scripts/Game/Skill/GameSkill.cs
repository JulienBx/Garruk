using UnityEngine;
using System.Collections.Generic;

public class GameSkill
{
	public Skill skill;

	public virtual void launch(Skill skill)
	{
		Debug.Log("Skill non implémenté");
	}

	public virtual void setTarget(PlayingCardController pcc)
	{
		Debug.Log("Skill non implémenté");
	}
}
