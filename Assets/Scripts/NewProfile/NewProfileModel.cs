using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NewProfileModel 
{
	public Player displayedUser;
	
	private string URLGetProfileData = ApplicationModel.host+"profile_getUserData.php";
	
	public NewProfileModel()
	{
		this.displayedUser=new Player();
	}
	public IEnumerator getData(bool isMyProfile, string profileChosen)
	{
		if(isMyProfile)
		{
			this.displayedUser=ApplicationModel.player;
		}
		else
		{
			WWWForm form = new WWWForm(); 											// Création de la connexion
			form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
			form.AddField("myform_nick", profileChosen);
			form.AddField("myform_idactiveuser", ApplicationModel.player.Id);
			form.AddField("myform_nbcardsbydeck", ApplicationModel.nbCardsByDeck);

			ServerController.instance.setRequest(URLGetProfileData, form);
			yield return ServerController.instance.StartCoroutine("executeRequest");

			if(ServerController.instance.getError()=="")
			{
				string result = ServerController.instance.getResult();
				string[] data=result.Split(new string[] { "END" }, System.StringSplitOptions.None);
				this.parsePlayer(data[0].Split(new string[] { "//" }, System.StringSplitOptions.None),isMyProfile);
				this.displayedUser.Users.parseUsers(data[5]);
				this.displayedUser.MyTrophies.parseTrophies(data[1]);
				this.displayedUser.MyFriends=this.displayedUser.parseFriends(data[2]);
				if(data[3]!="")
				{
					this.displayedUser.IsConnectedToPlayer=true;
					this.displayedUser.ConnectionWithPlayer=parseConnection(data[3].Split(new string[] {"//"}, System.StringSplitOptions.None));
				}
				else
				{
					this.displayedUser.IsConnectedToPlayer=false;
				}
				this.displayedUser.MyResults.parseResults(data[4]);
			}
			else
			{
				Debug.Log(ServerController.instance.getError());
				BackOfficeController.instance.displayDetectOfflinePopUp ();
			}
		}
		yield break;
	}
	private void parsePlayer(string[] array, bool isMyProfile)
	{
		this.displayedUser.Id=System.Convert.ToInt32(array[0]);
		this.displayedUser.Username = array [1];
		this.displayedUser.Ranking = System.Convert.ToInt32 (array [2]);
		this.displayedUser.RankingPoints = System.Convert.ToInt32 (array [3]);
		this.displayedUser.TotalNbWins = System.Convert.ToInt32 (array [4]);
		this.displayedUser.TotalNbLooses = System.Convert.ToInt32 (array [5]);
		this.displayedUser.CollectionPoints = System.Convert.ToInt32 (array [6]);
		this.displayedUser.CollectionRanking = System.Convert.ToInt32 (array [7]);
		this.displayedUser.IdProfilePicture = System.Convert.ToInt32(array [8]);
		this.displayedUser.Division = System.Convert.ToInt32(array [9]);
		this.displayedUser.TrainingStatus = System.Convert.ToInt32(array[10]);
	}
	private Connection parseConnection(string[] connectionData)
	{
		Connection connection = new Connection ();
		connection.Id = System.Convert.ToInt32 (connectionData [0]);
		if(System.Convert.ToInt32(connectionData [1])==this.displayedUser.Id)
		{
			connection.IsInviting=true;
		}
		connection.IsAccepted = System.Convert.ToBoolean(System.Convert.ToInt32 (connectionData [3]));
		return connection;
	}
}