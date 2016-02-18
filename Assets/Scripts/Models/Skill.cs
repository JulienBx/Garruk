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
	public int getPictureId()
	{
		switch (this.Id)
		{
		case 2:
			return 47;
			break;
		case 3:
			return 21;
			break;
		case 4:
			return 44;
			break;
		case 5:
			return 33;
			break;
		case 6:
			return 0;
			break;
		case 7:
			return 3;
			break;
		case 39:
			return 45;
			break;
		case 56:
			return 48;
			break;
		case 57:
			return 49;
			break;
		case 72:
			return 14;
			break;
		case 73:
			return 7;
			break;
		case 75:
			return 6;
			break;
		case 76:
			return 4;
			break;
		case 94:
			return 41;
			break;
		case 8:
			return 34;
			break;
		case 9:
			return 24;
			break;
		case 10:
			return 4;
			break;
		case 11:
			return 19;
			break;
		case 12:
			return 11;
			break;
		case 13:
			return 17;
			break;
		case 14:
			return 1;
			break;
		case 15:
			return 12;
			break;
		case 58:
			return 40;
			break;
		case 59:
			return 13;
			break;
		case 64:
			return 13;
			break;
		case 65:
			return 2;
			break;
		case 66:
			return 3;
			break;
		case 67:
			return 5;
			break;
		case 16:
			return 7;
			break;
		case 17:
			return 5;
			break;
		case 18:
			return 22;
			break;
		case 19:
			return 14;
			break;
		case 20:
			return 51;
			break;
		case 21:
			return 10;
			break;
		case 63:
			return 35;
			break;
		case 68:
			return 0;
			break;
		case 69:
			return 9;
			break;
		case 70:
			return 11;
			break;
		case 71:
			return 12;
			break;
		case 91:
			return 29;
			break;
		case 92:
			return 16;
			break;
		case 93:
			return 23;
			break;
		case 22:
			return 31;
			break;
		case 23:
			return 25;
			break;
		case 24:
			return 9;
			break;
		case 25:
			return 54;
			break;
		case 26:
			return 26;
			break;
		case 27:
			return 30;
			break;
		case 28:
			return 27;
			break;
		case 29:
			return 42;
			break;
		case 30:
			return 36;
			break;
		case 31:
			return 46;
			break;
		case 32:
			return 15;
			break;
		case 33:
			return 1;
			break;
		case 34:
			return 8;
			break;
		case 35:
			return 10;
			break;
		default:
			return 0;
			break;
		}
	}
}



