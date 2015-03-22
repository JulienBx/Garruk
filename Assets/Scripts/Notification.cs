using UnityEngine;
using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class Notification 
{
	public string[] Content;
	public DateTime Date;
	public bool IsRead;
	public int Id;
	public int IdNotificationType;
	public List<NotificationObject> NotificationObjects;

	
	
	public Notification(int id, string[] content, DateTime date, bool isread, int idnotificationtype)
	{
		this.Id = id;
		this.Content = content;
		this.Date = date;
		this.IsRead = isread;
		this.IdNotificationType = idnotificationtype;
	}

	public Notification(List<NotificationObject> notificationObjects)
	{
		this.NotificationObjects = notificationObjects;
	}

}



