using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NewProfileModel 
{
	
	public IList<ChallengesRecord> challengesRecords;
	public IList<User> users;
	public IList<int> friends;
	public IList<FriendsRequest> friendsRequests;
	public IList<Trophy> trophies;
	public string[] usernameList;
	public User player;
	public bool hasDeck;

	
	private string URLGetProfileData = ApplicationModel.host+"get_profile_data.php";
	
	public NewProfileModel()
	{
	}
	public IEnumerator getData(){
		
		this.challengesRecords= new List<ChallengesRecord>();
		this.friendsRequests = new List<FriendsRequest> ();
		this.trophies = new List<Trophy> ();
		this.player = new User();
		this.users = new List<User> ();
		this.friends = new List<int> ();
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);

		
		WWW w = new WWW(URLGetProfileData, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			Debug.Log (w.error); 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.player = parsePlayer(data[0].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.users = parseUsers(data[6].Split(new string[] { "#U#"  }, System.StringSplitOptions.None));
			this.friends=parseFriends(data[1].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.friendsRequests=parseFriendsRequests(data[2].Split(new string[] {"//"}, System.StringSplitOptions.None));
			this.trophies=parseTrophies(data[3].Split(new string[] {"//"},System.StringSplitOptions.None));
			this.challengesRecords=parseChallengesRecords(data[4].Split(new string[] {"//"},System.StringSplitOptions.None));
			this.hasDeck=System.Convert.ToBoolean(System.Convert.ToInt32(data[5]));
			
			usernameList=new string[this.users.Count];
			for(int i=0;i<this.users.Count;i++)
			{
				this.usernameList[i]=this.users[i].Username;
			}
		}
	}

	private User parsePlayer(string[] array)
	{
		User player = new User ();
		player.Id= System.Convert.ToInt32(array[0]);
		player.Ranking = System.Convert.ToInt32 (array [1]);
		player.RankingPoints = System.Convert.ToInt32 (array [2]);
		player.TotalNbWins = System.Convert.ToInt32 (array [3]);
		player.TotalNbLooses = System.Convert.ToInt32 (array [4]);
		player.CollectionPoints = System.Convert.ToInt32 (array [5]);
		player.CollectionRanking = System.Convert.ToInt32 (array [6]);
		player.FirstName = array [7];
		player.Surname = array [8];
		player.Mail = array [9];
		player.Picture = array [10];
		return player;
	}
	private IList<User> parseUsers(string[] array)
	{
		IList<User> users = new List<User>();
		
		for (int i=0;i<array.Length-1;i++)
		{
			string[] userData =array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			
			users.Add (new User());
			users[i].Id= System.Convert.ToInt32(userData[0]);
			users[i].Username= userData[1];
			users[i].ThumbPicture= userData[2];
			users[i].CollectionRanking = System.Convert.ToInt32 (userData [3]);
			users[i].RankingPoints = System.Convert.ToInt32 (userData [4]);
			users[i].Ranking = System.Convert.ToInt32 (userData [5]);
			users[i].TotalNbWins = System.Convert.ToInt32 (userData[6]);
			users[i].TotalNbLooses = System.Convert.ToInt32 (userData [7]);
		}
		return users;
	}
	private List<int> parseFriends(string[] friendsData)
	{
		List<int> friends = new List<int> ();
		for(int i=0;i<friendsData.Length-1;i++)
		{
			friends.Add (this.returnUsersIndex(System.Convert.ToInt32(friendsData[i])));
		}
		return friends;
	}
	private List<FriendsRequest> parseFriendsRequests(string[] friendsRequestsData)
	{
		List<FriendsRequest> friendsRequests = new List<FriendsRequest> ();

		for (int i=0; i<friendsRequestsData.Length-1;i++)
		{
			string[] friendsRequestData =friendsRequestsData[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			friendsRequests[i].Connection = new Connection();
			friendsRequests[i].User= this.users[returnUsersIndex(System.Convert.ToInt32(friendsRequestData[0]))];
			friendsRequests[i].IsInvitingPlayer=System.Convert.ToBoolean(System.Convert.ToInt32(friendsRequestData[1]));
			friendsRequests[i].Connection=new Connection();
			friendsRequests[i].Connection.Id=System.Convert.ToInt32(friendsRequestData[2]);
		}
		return friendsRequests;
	}
	private List<Trophy> parseTrophies(string[] trophiesData)
	{
		List<Trophy> trophies = new List<Trophy> ();
		
		for (int i=0; i<trophiesData.Length-1;i++)
		{
			string[] trophyData = trophiesData[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			trophies.Add (new Trophy());
			trophies[i].competition=new Competition();
			trophies[i].Date=DateTime.ParseExact(trophyData[0], "yyyy-MM-dd HH:mm:ss", null);
			trophies[i].competition.Name=trophyData[1];
			trophies[i].competition.Picture=trophyData[2];
		}
		return trophies;
	}
	private List<ChallengesRecord> parseChallengesRecords(string[] challengesRecordsData)
	{
		List<ChallengesRecord> challengesRecords = new List<ChallengesRecord> ();
		
		for (int i=0; i<challengesRecordsData.Length-1;i++)
		{
			string[] challengesRecordData = challengesRecordsData[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			challengesRecords.Add (new ChallengesRecord());
			challengesRecords[i].Friend= this.users[returnUsersIndex(System.Convert.ToInt32(challengesRecordData[0]))];
			challengesRecords[i].NbWins=System.Convert.ToInt32(challengesRecordData[1]);
			challengesRecords[i].NbLooses=System.Convert.ToInt32(challengesRecordData[2]);
		}
		return challengesRecords;
	}
	private int returnUsersIndex(int id)
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
}


