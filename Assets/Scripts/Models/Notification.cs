using UnityEngine;
using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class Notification 
{

	public DateTime Date;
	public bool IsRead;
	public int Id;
	public int IdNotificationType;

	public Notification(int id, DateTime date, bool isread, int idnotificationtype)
	{
		this.Id = id;
		this.Date = date;
		this.IsRead = isread;
		this.IdNotificationType = idnotificationtype;
	}
}



