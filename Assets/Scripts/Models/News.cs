using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class News 
{
	
	public DateTime Date;
	public int IdUser;
	public int IdSendingUser;
	public int IdNewsType;
	public string Param1;
	public string Param2;
	public string Param3;
	public string Description;
	
	public News()
	{
	}
	public News(int idnewstype, DateTime date, String description)
	{
		this.IdNewsType = idnewstype;
		this.Date = date;
		this.Description = description;
	}
	public News(int iduser, int idnewstype)
	{
		this.IdUser = iduser;
		this.IdNewsType = idnewstype;
		this.Param1 = "";
		this.Param2 = "";
		this.Param3 = "";
	}
	
	public News(int iduser, int idnewstype, string param1)
	{
		this.IdUser = iduser;
		this.IdNewsType = idnewstype;
		this.Param1 = param1;
		this.Param2 = "";
		this.Param3 = "";
	}
	public News(int iduser, int idnewstype, string param1, string param2)
	{
		this.IdUser = iduser;
		this.IdNewsType = idnewstype;
		this.Param1 = param1;
		this.Param2 = param2;
		this.Param3 = "";
	}
	public News(int iduser, int idnewstype, string param1, string param2, string param3)
	{
		this.IdUser = iduser;
		this.IdNewsType = idnewstype;
		this.Param1 = param1;
		this.Param2 = param2;
		this.Param3 = param3;
	}
}



