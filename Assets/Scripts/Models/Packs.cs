using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class Packs{

	public List<Pack> packs ;
	
	public Packs()
	{
		this.packs = new List<Pack>();
	}
	public Pack getPack(int index)
	{
		return this.packs [index];
	}
	public int getCount()
	{
		return this.packs.Count;
	}
	public void add()
	{
		this.packs.Add(new Pack());
	}
	public void parsePacks(string s)
	{
		string[] array = s.Split (new string[] {"PACK"}, System.StringSplitOptions.None);
		for(int i=0;i<array.Length-1;i++)
		{
			string[] packInformation = array[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
			packs.Add (new Pack());
			packs[i].Id = System.Convert.ToInt32(packInformation[0]);
			packs[i].Name = WordingPacks.getName(System.Convert.ToInt32(packInformation[0])-1);
			packs[i].NbCards=System.Convert.ToInt32(packInformation[1]);
			packs[i].CardType = System.Convert.ToInt32(packInformation[2]);
			packs[i].Price=System.Convert.ToInt32(packInformation[3]);
			packs[i].New=System.Convert.ToBoolean(System.Convert.ToInt32(packInformation[4]));
			packs[i].IdPicture=System.Convert.ToInt32(packInformation[5]);
		}
	}

}