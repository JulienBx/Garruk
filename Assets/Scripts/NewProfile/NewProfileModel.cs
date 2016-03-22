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
	public IList<Result> confrontations;
	public string[] usernameList;
	public User displayedUser;
	
	private string URLGetProfileData = ApplicationModel.host+"get_profile_data.php";
	private string URLSearchUsers = ApplicationModel.host + "search_users.php";
	
	public NewProfileModel()
	{
	}
	public IEnumerator getData(bool isMyProfile, string profileChosen){

		this.friendsRequests = new List<FriendsRequest> ();
		this.trophies = new List<Trophy> ();
		this.displayedUser = new User();
		this.users = new List<User> ();
		this.friends = new List<int> ();

		string isMyProfileString;
		if(isMyProfile)
		{
			this.challengesRecords= new List<ChallengesRecord>();
			isMyProfileString="1";
		}
		else
		{
			this.confrontations=new List<Result>();
			isMyProfileString="0";
		}

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", profileChosen);
		form.AddField("myform_activeuser", ApplicationModel.player.Username);
		form.AddField("myform_nbcardsbydeck", ApplicationModel.nbCardsByDeck);
		form.AddField("myform_ismyprofile", isMyProfileString);
		
		WWW w = new WWW(URLGetProfileData, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			Debug.Log (w.error); 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.parsePlayer(data[0].Split(new string[] { "//" }, System.StringSplitOptions.None),isMyProfile);
			this.users = parseUsers(data[9].Split(new string[] { "#U#"  }, System.StringSplitOptions.None));
			this.users.Add(this.displayedUser);
			this.trophies=parseTrophies(data[1].Split(new string[] {"#TROPHY#"},System.StringSplitOptions.None));
			this.friends=parseFriends(data[2].Split(new string[] { "//" }, System.StringSplitOptions.None));

			if(isMyProfile)
			{
				this.friendsRequests=parseFriendsRequests(data[3].Split(new string[] {"#FR#"}, System.StringSplitOptions.None));
				this.challengesRecords=parseChallengesRecords(data[4].Split(new string[] {"#CR#"},System.StringSplitOptions.None));
				ApplicationModel.player.HasDeck=System.Convert.ToBoolean(System.Convert.ToInt32(data[5]));
			}
			else
			{
				if(data[3]!="")
				{
					this.displayedUser.IsConnectedToPlayer=true;
					this.displayedUser.ConnectionWithPlayer=parseConnection(data[3].Split(new string[] {"//"}, System.StringSplitOptions.None));
				}
				else
				{
					this.displayedUser.IsConnectedToPlayer=false;
				}
				this.confrontations=parseConfrontations(data[4].Split(new string[] {"#CF#"},System.StringSplitOptions.None));
				ApplicationModel.player.Id=System.Convert.ToInt32(data[5]);
			}
			ApplicationModel.player.TutorialStep=System.Convert.ToInt32(data[6]);
			//ApplicationModel.player.DisplayTutorial=System.Convert.ToBoolean(System.Convert.ToInt32(data[7]));
			ApplicationModel.player.ProfileTutorial=System.Convert.ToBoolean(System.Convert.ToInt32(data[8]));
			usernameList=new string[this.users.Count];
			for(int i=0;i<this.users.Count;i++)
			{
				this.usernameList[i]=this.users[i].Username;
			}
		}
	}
	private void parsePlayer(string[] array, bool isMyProfile)
	{
		this.displayedUser.Username = array [1];
		this.displayedUser.Ranking = System.Convert.ToInt32 (array [2]);
		this.displayedUser.RankingPoints = System.Convert.ToInt32 (array [3]);
		this.displayedUser.TotalNbWins = System.Convert.ToInt32 (array [4]);
		this.displayedUser.TotalNbLooses = System.Convert.ToInt32 (array [5]);
		this.displayedUser.CollectionPoints = System.Convert.ToInt32 (array [6]);
		this.displayedUser.CollectionRanking = System.Convert.ToInt32 (array [7]);
		this.displayedUser.IdProfilePicture = System.Convert.ToInt32(array [8]);

		if(isMyProfile)
		{
			this.displayedUser.Division = ApplicationModel.player.CurrentDivision.Id;
			ApplicationModel.player.FirstName = array [10];
			ApplicationModel.player.Surname = array [11];
			ApplicationModel.player.Mail = array [12];
			ApplicationModel.player.Id= System.Convert.ToInt32(array[0]);
		}
		else
		{
			this.displayedUser.Division = System.Convert.ToInt32(array [9]);
			this.displayedUser.Id = System.Convert.ToInt32(array[0]);
		}
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
			users[i].IdProfilePicture= System.Convert.ToInt32(userData[2]);
			users[i].CollectionRanking = System.Convert.ToInt32 (userData [3]);
			users[i].RankingPoints = System.Convert.ToInt32 (userData [4]);
			users[i].Ranking = System.Convert.ToInt32 (userData [5]);
			users[i].TotalNbWins = System.Convert.ToInt32 (userData[6]);
			users[i].TotalNbLooses = System.Convert.ToInt32 (userData [7]);
			users[i].Division = System.Convert.ToInt32 (userData [8]);
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
			friendsRequests.Add (new FriendsRequest());
			friendsRequests[i].Connection = new Connection();
			friendsRequests[i].User= this.users[returnUsersIndex(System.Convert.ToInt32(friendsRequestData[0]))];
			friendsRequests[i].IsInvitingPlayer=System.Convert.ToBoolean(System.Convert.ToInt32(friendsRequestData[1]));
			friendsRequests[i].Connection=new Connection();
			friendsRequests[i].Connection.Id=System.Convert.ToInt32(friendsRequestData[2]);
		}
		return friendsRequests;
	}
	private Connection parseConnection(string[] connectionData)
	{
		Connection connection = new Connection ();
		connection.Id = System.Convert.ToInt32 (connectionData [0]);
		connection.IdUser1 = System.Convert.ToInt32 (connectionData [1]);
		connection.IdUser2 = System.Convert.ToInt32 (connectionData [2]);
		connection.IsAccepted = System.Convert.ToBoolean(System.Convert.ToInt32 (connectionData [3]));
		return connection;
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
			trophies[i].competition.Name=WordingGameModes.getName(System.Convert.ToInt32(trophyData[1]),System.Convert.ToInt32(trophyData[2]));
			trophies[i].competition.GameType=System.Convert.ToInt32(trophyData[1]);
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
	private List<Result> parseConfrontations(string[] confrontationsData)
	{
		List<Result> confrontations = new List<Result> ();
		
		for (int i=0; i<confrontationsData.Length-1;i++)
		{
			string[] confrontationData = confrontationsData[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			confrontations.Add (new Result());
			confrontations[i].IdWinner= System.Convert.ToInt32(confrontationData[0]);
			confrontations[i].GameType= System.Convert.ToInt32(confrontationData[1]);
			confrontations[i].Date=DateTime.ParseExact(confrontationData[2], "yyyy-MM-dd HH:mm:ss", null);
		}
		return confrontations;
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
	public void moveToFriend(int id)
	{
		this.friends.Add (this.returnUsersIndex(this.friendsRequests[id].User.Id));
		this.friendsRequests.RemoveAt(id);
	}
}