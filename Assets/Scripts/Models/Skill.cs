using UnityEngine;
using System.Collections;

public class Skill
{
	public string Name;
	public int Id;
	public int IsActivated;
	public int Level;
	public int Power;
	public int proba;
	public int ManaCost;
	public int nextProba;
	public string Description;
	public string nextDescription;
	public string ResourceName;
	public float Ponderation;
	public int XMin;
	public string Action;
	public int IdCardType;
	public Texture2D texture;
	public string Picture;
	public bool IsNew;
	public int cible ;
	public int Upgrades;
	public int nextLevel;
	public string[] AllDescriptions;
	public int[] AllProbas;
	public int IdSkillType;
	public int IdPicture;
	public bool hasBeenPlayed ;
	
	public Skill(string name, string description, int id, int c, int p)
	{
		this.Name = name;
		this.Description = description;
		this.Id = id ;
		this.cible = c ;
		this.proba = p ;
		this.hasBeenPlayed = false ;
	}

	public Skill(string name, int id, int isactivated, int level, int power, int manaCost, string description, string action) : this(name, id, isactivated, level, power, manaCost, description)
	{
		this.Action = action;
		this.hasBeenPlayed = false ;
	}
	public Skill(string name, int id, int isactivated, int level, int power, int manaCost, string description)
	{
		this.Name = name;
		this.Id = id;
		this.IsActivated = isactivated;
		this.Level = level;
		this.Power = power;
		this.ManaCost = manaCost;
		this.Description = description;
		this.hasBeenPlayed = false ;
	}
	
	public Skill(string name, int id, int isactivated, int level, int power, int manaCost, string description, int c, int p)
	{
		this.Name = name;
		this.Id = id;
		this.IsActivated = isactivated;
		this.Level = level;
		this.Power = power;
		this.ManaCost = manaCost;
		this.Description = description;
		this.cible = c;
		this.proba = p;
		this.hasBeenPlayed = false ;
	}
	
	public Skill(string name, int id, int isactivated, int level, int power, int manaCost)
	{
		this.Name = name;
		this.Id = id;
		this.IsActivated = isactivated;
		this.Level = level;
		this.Power = power;
		this.ManaCost = manaCost;
		this.hasBeenPlayed = false ;
	}
	public Skill(string name)
	{
		this.Name = name;
		this.hasBeenPlayed = false ;
	}

	public Skill(string name, int id)
	{
		this.Name = name;
		this.Id = id;
		this.hasBeenPlayed = false ;
	}

	public Skill(int id)
	{
		this.Id = id;
		this.hasBeenPlayed = false ;
	}
	public Skill()
	{
		this.texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		this.Description = "";
		this.hasBeenPlayed = false ;
	}
	
	public IEnumerator setPicture()
	{
		var www = new WWW(ApplicationModel.host + this.Picture);
		yield return www;
		www.LoadImageIntoTexture(this.texture);
	}
	
	public string getProbaText()
	{
		if(this.proba<10){
			return ("0"+proba);
		}
		else{
			return ""+proba;
		}
	}
}



