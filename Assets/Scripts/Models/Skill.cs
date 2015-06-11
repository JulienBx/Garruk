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
	public string ResourceName;
	public float Ponderation;
	public int XMin;
	public string Action;
	public int nbLeft = -1 ;
	public int CardType;
	public Texture2D texture;
	public string Picture;
	public bool IsNew;


	public Skill(string name, int id, int isactivated, int level, int power, int manaCost, string description, string action) : this(name, id, isactivated, level, power, manaCost, description)
	{
		this.Action = action;
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

		if (id==10){
			nbLeft = 2 ;
			this.Description+=nbLeft+" restant(s)";
		}
	}
	public Skill(string name, int id, int isactivated, int level, int power, int manaCost)
	{
		this.Name = name;
		this.Id = id;
		this.IsActivated = isactivated;
		this.Level = level;
		this.Power = power;
		this.ManaCost = manaCost;

		if (id==10){
			nbLeft = 2 ;
			this.Description+=nbLeft+" restant(s)";
		}
	}
	public Skill(string name)
	{
		this.Name = name;
	}

	public Skill(int id)
	{
		this.Id = id;
	}
	public Skill()
	{
		this.texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
	}
	public IEnumerator setPicture()
	{
		var www = new WWW(ApplicationModel.host+this.Picture);
		yield return www;
		www.LoadImageIntoTexture(this.texture);
	}
	public void lowerNbLeft(){
		nbLeft--;
		this.Description = this.Description.Substring(0,this.Description.Length-12)+nbLeft+" restant(s)";
	}

}



