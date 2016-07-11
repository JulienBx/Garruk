using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Results{

	public List<Result> results ;

	public Results()
	{
		this.results = new List<Result>();
	}
	public Result getResult(int index)
	{
		return this.results [index];
	}
	public int getCount()
	{
		return this.results.Count;
	}
	public void parseResults(string s)
	{
		string[] array = s.Split(new string[] {"#RESULT#"},System.StringSplitOptions.None);
		
		for (int i=0; i<array.Length-1;i++)
		{
			string[] confrontationData = array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			results.Add (new Result());
			results[i].IdWinner= System.Convert.ToInt32(confrontationData[0]);
			results[i].GameType= System.Convert.ToInt32(confrontationData[1]);
			results[i].Date=DateTime.ParseExact(confrontationData[2], "yyyy-MM-dd HH:mm:ss", null);
		}
	}


}