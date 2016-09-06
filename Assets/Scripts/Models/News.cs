using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class News
{
	public int User;
	public IList<int> Users;
	public Cards Cards;
	public IList<string> Values;
	public Trophies Trophies;
	public string Content;
	public DateTime Date;
	public int IdNewsType;
	public string Param1;
	public string Param2;
	public string Param3;
	public string Description;
	
	public News()
	{
		this.Users = new List<int>();
		this.Cards = new Cards();
		this.Values = new List<string> ();
		this.Trophies = new Trophies();
		this.Content="";
	}
}

