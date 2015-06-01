using UnityEngine;
using System.Collections.Generic;

public class GameSkill
{
	public virtual void launch ()
	{
			Debug.Log ("Skill non implémenté");
	}

	public virtual void setTarget (PlayingCardController pcc)
	{
			Debug.Log ("Skill non implémenté");
	}

	public virtual void resolve (int[] args)
	{
			Debug.Log ("Skill non implémenté");
	}

	public virtual bool isLaunchable (Skill s)
	{
			return true;
	}

	public string convertStatToString (ModifierStat stat)
	{
			switch (stat) {
			case ModifierStat.Stat_Attack:
					return "en attaque";
					break;
			case ModifierStat.Stat_Life:
					return "de vie";
					break;
			case ModifierStat.Stat_Move:
					return "en mouvement";
					break;
			case ModifierStat.Stat_Speed:
					return "en vitesse";
					break;
			case ModifierStat.Stat_Dommage:
					return "de dommage";
					break;
			default:
					return "";
					break;
			}
	}
}
