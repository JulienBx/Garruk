using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Trophies{

	public List<Trophy> trophies ;

	public Trophies()
	{
		this.trophies = new List<Trophy>();
	}

	public Trophy getTrophy(int index)
	{
		return this.trophies [index];
	}

	public int getCount()
	{
		return this.trophies.Count;
	}
	public void add()
	{
		this.trophies.Add(new Trophy());
	}
	public void parseTrophies(string s)
	{
		string[] array = s.Split(new string[] {"#TROPHY#"},System.StringSplitOptions.None);
		
		for (int i=0; i<array.Length-1;i++)
		{
			string[] trophyData = array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			trophies.Add (new Trophy());
			trophies[i].division=new Division();
			trophies[i].Date=DateTime.ParseExact(trophyData[0], "yyyy-MM-dd HH:mm:ss", null);
			trophies[i].division.Id=System.Convert.ToInt32(trophyData[2]);
			trophies[i].division.GameType=System.Convert.ToInt32(trophyData[1]);
		}
	}
	public void writeTrophies()
	{
		for (int i=0; i<trophies.Count;i++)
		{
			trophies[i].division.Name=WordingGameModes.getName(System.Convert.ToInt32(trophies[i].division.Id));
		}
	}
}