using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class DisplayedNews
{
	public News News;
	public User User;
	public IList<User> Users;
	public IList<Card> Cards;
	public IList<string> Values;
	public IList<Result> Results;
	public IList<Trophy> Trophies;
	public string Content;
	
	public DisplayedNews(News news, User user)
	{
		this.News = news;
		this.Users = new List<User> ();
		this.Cards = new List<Card> ();
		this.Values = new List<string> ();
		this.Results = new List<Result> ();
		this.Trophies = new List<Trophy> ();
		this.User = user;
	}
}

