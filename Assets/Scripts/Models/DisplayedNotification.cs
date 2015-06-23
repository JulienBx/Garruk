using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class DisplayedNotification
{
	public Notification Notification;
	public User SendingUser;
	public IList<User> Users;
	public IList<Card> Cards;
	public IList<string> Values;
	public IList<Result> Results;
	public IList<Trophy> Trophies;
	public string Content;

	public DisplayedNotification(Notification notification, User sendinguser)
	{
		this.Notification = notification;
		this.Users = new List<User> ();
		this.Cards = new List<Card> ();
		this.Values = new List<string> ();
		this.Results = new List<Result> ();
		this.Trophies = new List<Trophy> ();
		this.SendingUser = sendinguser;
	}
}

