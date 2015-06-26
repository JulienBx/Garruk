using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpponentSkillsViewModel
{
	public List<Skill> Skills = new List<Skill>();

	public void setSkill(int i, string name, string description)
	{
		Skills [i] = new Skill();
	}
}


