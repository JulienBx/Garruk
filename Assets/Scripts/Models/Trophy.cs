using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Trophy 
{
	public int Id;
	public int UserId;
	public int TrophyType;
	public int TrophyNumber;
	public DateTime Date;
	
	public Trophy(int userid, int trophytype, int trophynumber)
	{
		this.UserId = userid;
		this.TrophyType = trophytype;
		this.TrophyNumber = trophynumber;
	}
	public Trophy(int userid, int trophytype, int trophynumber, DateTime date)
	{
		this.UserId = userid;
		this.TrophyType = trophytype;
		this.TrophyNumber = trophynumber;
		this.Date = date;
	}
}