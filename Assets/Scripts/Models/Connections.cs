using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Connections{

	public List<Connection> connections ;
	
	public Connections()
	{
		this.connections = new List<Connection>();
	}
	public Connection getConnection(int index)
	{
		return this.connections [index];
	}
	public int getCount()
	{
		return this.connections.Count;
	}
	public void add()
	{
		this.connections.Add(new Connection());
	}
	public void remove(int index)
	{
		this.connections.RemoveAt(index);
	}
	public void parseConnections(string s, Player p)
	{
		string[] array = s.Split(new string[] {"#CN#"}, System.StringSplitOptions.None);

		for (int i=0; i<array.Length-1;i++)
		{
			string[] connectionData =array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			connections.Add (new Connection());
			connections[i].User= p.Users.returnUsersIndex(System.Convert.ToInt32(connectionData[0]));
			connections[i].IsInviting=System.Convert.ToBoolean(System.Convert.ToInt32(connectionData[1]));
			connections[i].Id=System.Convert.ToInt32(connectionData[2]);
		}
	}
}