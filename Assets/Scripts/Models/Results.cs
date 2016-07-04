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

	public User getResult(int index)
	{
		return this.results [index];
	}

	public int getCount()
	{
		return this.results.Count;
	}


}