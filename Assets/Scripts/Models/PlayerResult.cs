using UnityEngine;
using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class PlayerResult 
{
	public User Opponent;
	public bool HasWon;
	public DateTime Date;
	public int GameType;
	
	public PlayerResult()
	{
	}
	public PlayerResult(bool haswon, DateTime date,int gametype,User opponent)
	{
		this.Opponent = opponent;
		this.HasWon = haswon;
		this.Date = date;
		this.GameType = gametype;
	}
}



