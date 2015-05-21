using UnityEngine;
using System.Collections.Generic;

public class GameSkill
{
	public Skill skill;
	//public int SkillNumber;

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	public virtual void cast()
	{
		Debug.Log("Skill non implémenté");
	}

	public virtual void setTarget(PlayingCardController pcc)
	{
		Debug.Log("Skill non implémenté");
	}
}
