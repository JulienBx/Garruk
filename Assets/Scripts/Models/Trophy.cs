using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class Trophy 
{
	public int Id;
	public int UserId;
	public int TrophyType;
	public int TrophyNumber;
	
	public Trophy(int userid, int trophytype, int trophynumber)
	{
		this.UserId = userid;
		this.TrophyType = trophytype;
		this.TrophyNumber = trophynumber;
	}
}