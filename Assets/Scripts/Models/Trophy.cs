using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable] 
public class Trophy 
{
	public User User;
	public int Id;
	public Division division;
	public DateTime Date;
	
	public Trophy()
	{
	}
}