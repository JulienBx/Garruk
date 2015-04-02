using UnityEngine;
using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class Result 
{
	public User Winner;
	public User Looser;
	public DateTime Date;
	public User Opponent;
	public bool HasWon;
	
	public Result(User winner, User looser, DateTime date)
	{
		this.Winner = winner;
		this.Looser = looser;
		this.Date = date;
	}
	public Result(User opponent, bool haswon, DateTime date)
	{
		this.Opponent = opponent;
		this.HasWon = haswon;
		this.Date = date;
	}	
}



