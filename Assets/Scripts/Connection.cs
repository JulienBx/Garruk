using UnityEngine;
using System.Collections;

public class Connection 
{
	public string User;
	public int State;
	public int Id;


	
	public Connection(int id, string user, int state)
	{
		this.Id = id;
		this.User = user;
		this.State = state;
	}
}
