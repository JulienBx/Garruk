using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class Users{

	public List<User> users ;
	public string[] usernameList;

	public Users()
	{
		this.users = new List<User>();
	}

	public User getUser(int index)
	{
		return this.users [index];
	}

	public int getCount()
	{
		return this.users.Count;
	}
	public void add()
	{
		this.users.Add(new User());
	}
	public int returnUsersIndex(int id)
	{
		int index=new int();
		for(int j=0;j<this.users.Count;j++)
		{
			if(this.users[j].Id==id)
			{
				index=j;
				break;
			}
		}
		return index;
	}
	public void parseUsers(string s)
	{
		string[] array = s.Split(new string[] { "#U#"  }, System.StringSplitOptions.None);

		for (int i=0;i<array.Length-1;i++)
		{
			string[] userData =array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			users.Add (new User());
			users[i].Id= System.Convert.ToInt32(userData[0]);
			users[i].Username= userData[1];
			users[i].IdProfilePicture= System.Convert.ToInt32(userData[2]);
			users[i].CollectionRanking = System.Convert.ToInt32 (userData [3]);
			users[i].RankingPoints = System.Convert.ToInt32 (userData [4]);
			users[i].Ranking = System.Convert.ToInt32 (userData [5]);
			users[i].TotalNbWins = System.Convert.ToInt32 (userData[6]);
			users[i].TotalNbLooses = System.Convert.ToInt32 (userData [7]);
			users[i].Division = System.Convert.ToInt32 (userData [8]);
			users[i].TrainingStatus = System.Convert.ToInt32 (userData [9]);
			users[i].isPublic=System.Convert.ToBoolean(System.Convert.ToInt32(userData [10]));
		}
		usernameList=new string[this.users.Count];
		for(int i=0;i<this.users.Count;i++)
		{
			this.usernameList[i]=this.users[i].Username;
		}
	}
}