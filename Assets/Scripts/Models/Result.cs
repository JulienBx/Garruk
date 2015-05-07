using UnityEngine;
using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class Result 
{
	public int IdWinner;
	public int IdLooser;
	public DateTime Date;
	public int GameType;
	
	public Result(int idwinner, int idlooser, DateTime date, int gametype)
	{
		this.IdWinner = idwinner;
		this.IdLooser = idlooser;
		this.Date = date;
		this.GameType = gametype;
	}
}



