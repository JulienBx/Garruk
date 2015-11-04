using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cup : Competition 
{
	public int NbRounds;
	public int CupPrize;

	public Cup()
	{
	}
	public Cup(int id, int nbrounds, int cupprize, string name)
	{
		this.Id = id;
		this.NbRounds = nbrounds;
		this.CupPrize = cupprize;
		this.Name = name;
	}
}



