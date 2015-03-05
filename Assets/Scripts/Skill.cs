using UnityEngine;
using System.Collections;

public class Skill 
{
	public string Name;
	public int Id;
	public int IsActivated;
	public int Level;
	public int Power;
	public int ManaCost;
	public string Description;

	public Skill(string name, int id, int isactivated, int level, int power, int manaCost, string description)
	{
		this.Name = name;
		this.Id = id;
		this.IsActivated = isactivated;
		this.Level = level;
		this.Power = power;
		this.ManaCost = manaCost;
		this.Description = description;
	}

	public Skill(string name, int id, int isactivated, int level, int power, int manaCost)
	{
		this.Name = name;
		this.Id = id;
		this.IsActivated = isactivated;
		this.Level = level;
		this.Power = power;
		this.ManaCost = manaCost;

	}

	public Skill(string name)
	{
		this.Name = name;
	}
}



