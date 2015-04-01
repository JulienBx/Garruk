using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class User 
{
	public string Username;
	public string Mail;
	public string FirstName;
	public string Surname;
	public string Picture;
	public int Money;
	public List<Connection> Connections;


	public User()
	{
		this.Username = "";
		this.Picture = "";
	}

	public User(string username, string picture)
	{
		this.Username = username;
		this.Picture = picture;
	}

	public User(string username, string mail, int money, string firstname, string surname, string picture)
	{
		this.Username = username;
		this.Mail = mail;
		this.Money = money;
		this.FirstName = firstname;
		this.Surname = surname;
		this.Picture = picture;
	}

	public User(List<Connection> connections)
	{
		this.Connections = connections;
	}
	
}



