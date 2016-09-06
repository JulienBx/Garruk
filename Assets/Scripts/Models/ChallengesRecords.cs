using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class ChallengesRecords{

	public List<ChallengesRecord> challengesRecords ;

	public ChallengesRecords()
	{
		this.challengesRecords = new List<ChallengesRecord>();
	}
	public ChallengesRecord getChallengesRecord(int index)
	{
		return this.challengesRecords [index];
	}
	public int getCount()
	{
		return this.challengesRecords.Count;
	}
	public void add()
	{
		this.challengesRecords.Add(new ChallengesRecord());
	}
	public void parseChallengesRecords(string s, Player p)
	{
		string[] array = s.Split(new string[] {"#CR#"},System.StringSplitOptions.None);
		
		for (int i=0; i<array.Length-1;i++)
		{
			string[] challengesRecordData = array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			challengesRecords.Add (new ChallengesRecord());
			challengesRecords[i].Friend= p.Users.returnUsersIndex(System.Convert.ToInt32(challengesRecordData[0]));
			challengesRecords[i].NbWins=System.Convert.ToInt32(challengesRecordData[1]);
			challengesRecords[i].NbLooses=System.Convert.ToInt32(challengesRecordData[2]);
		}
	}
}