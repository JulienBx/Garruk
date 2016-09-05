using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class Divisions{

	public List<Division> divisions ;

	public Divisions()
	{
		this.divisions = new List<Division>();
	}
	public Division getDivision(int index)
	{
		return this.divisions [index];
	}
	public int getCount()
	{
		return this.divisions.Count;
	}
	public void add()
	{
		this.divisions.Add(new Division());
	}
	public void remove(int index)
	{
		this.divisions.RemoveAt(index);
	}
	public void parseDivisions(string s)
	{
		string[] divisionsIds;
		divisionsIds = s.Split(new string[] { "#DIVISION#" }, System.StringSplitOptions.None);

		string[] divisionData = null;

		for (int i = 0; i < divisionsIds.Length - 1; i++) 		// On boucle sur les attributs d'un deck
		{
			divisionData = divisionsIds[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
			for(int j=0 ; j<divisionData.Length-1;j++)
			{
				this.divisions.Add(new Division());
				this.divisions[i].Id=System.Convert.ToInt32(divisionData [1]);
				this.divisions[i].earnCredits_W=System.Convert.ToInt32(divisionData [1]);
				this.divisions[i].earnCredits_L=System.Convert.ToInt32(divisionData [2]);
				this.divisions[i].earnXp_W=System.Convert.ToInt32(divisionData [3]);
				this.divisions[i].earnXp_L=System.Convert.ToInt32(divisionData [4]);

			}	                     
		}
	}
}