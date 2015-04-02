using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class Cup 
{
	public int NbRounds;
	public int CupPrize;
	public int Id;
	
	public Cup(int id, int nbrounds, int cupprize)
	{
		this.Id = id;
		this.NbRounds = nbrounds;
		this.CupPrize = cupprize;
	}
}



