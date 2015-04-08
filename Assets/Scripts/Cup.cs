using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class Cup 
{
	public int NbRounds;
	public int CupPrize;
	public int Id;
	public string Name;
	
	public Cup(int id, int nbrounds, int cupprize, string name)
	{
		this.Id = id;
		this.NbRounds = nbrounds;
		this.CupPrize = cupprize;
		this.Name = name;
	}
}



