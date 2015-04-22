using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ProfileModel
{
	private string URLGetUserProfile = ApplicationModel.host + "get_profile.php";

	public User Profile;
	public User Player;
	public IList<Connection> Connections;
	public IList<User> Contacts;
	public IList <DisplayedTrophy> Trophies;

	public ProfileModel(){
	}
	
	public IEnumerator getProfile() {

		this.Connections = new List<Connection> ();
		this.Contacts = new List<User> ();
		this.Trophies = new List<DisplayedTrophy> ();

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_player", ApplicationModel.username);
		form.AddField("myform_user",ApplicationModel.profileChosen);

		WWW w = new WWW(URLGetUserProfile, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			Debug.Log (w.error); 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			string[] playerInformations = data[0].Split(new string[] { "\\" }, System.StringSplitOptions.None);
			string[] playerConnections = data[2].Split(new string[] { "CONNECTION" }, System.StringSplitOptions.None);
			string[] playerTrophies = data[3].Split(new string[] { "TROPHY" }, System.StringSplitOptions.None);

			if(ApplicationModel.profileChosen=="" || ApplicationModel.profileChosen==ApplicationModel.username)
			{
				Player=new User(System.Convert.ToInt32(playerInformations[0]),
				                playerInformations[1],
				                playerInformations[2],
				                System.Convert.ToInt32(playerInformations[3]),
				                playerInformations[4],
				                playerInformations[5],
				                playerInformations[6],
				                System.Convert.ToInt32(playerInformations[7]),
				                System.Convert.ToInt32(playerInformations[8]),
				                System.Convert.ToInt32(playerInformations[9]),
				                System.Convert.ToInt32(playerInformations[10]),
				                System.Convert.ToInt32(playerInformations[11]));
				Profile=new User();
				Profile=Player;
			}
			else
			{
				Player=new User(System.Convert.ToInt32(playerInformations[0]),
				                playerInformations[1],
				                playerInformations[2],
				                System.Convert.ToInt32(playerInformations[3]),
				                System.Convert.ToInt32(playerInformations[4]),
				                System.Convert.ToInt32(playerInformations[5]),
				                System.Convert.ToInt32(playerInformations[6]),
				                System.Convert.ToInt32(playerInformations[7]));
				string[] userInformations = data[1].Split(new string[] { "\\" }, System.StringSplitOptions.None);
				Profile=new User(System.Convert.ToInt32(userInformations[0]),
				                userInformations[1],
				                userInformations[2],
				                System.Convert.ToInt32(userInformations[3]),
				                System.Convert.ToInt32(userInformations[4]),
				                System.Convert.ToInt32(userInformations[5]),
				                System.Convert.ToInt32(userInformations[6]),
				                System.Convert.ToInt32(userInformations[7]));
			}
			for(int i=0;i<playerConnections.Length-1;i++)
			{
				string[] connectionData = playerConnections[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
				Connections.Add (new Connection(System.Convert.ToInt32(connectionData[0]),
				                                System.Convert.ToInt32(connectionData[1]),
				                                System.Convert.ToInt32(connectionData[2]),
				                                System.Convert.ToBoolean(System.Convert.ToInt32(connectionData[3]))));
				Contacts.Add (new User(System.Convert.ToInt32(connectionData[4]),
				                       connectionData[5],
				                       connectionData[6],
				                       System.Convert.ToInt32(connectionData[7]),
				                       System.Convert.ToInt32(connectionData[8]),
				                       System.Convert.ToInt32(connectionData[9]),
				                       System.Convert.ToInt32(connectionData[10]),
				                       System.Convert.ToInt32(connectionData[11])));
			}
			for(int i=0;i<playerTrophies.Length-1;i++)
			{
				string[] trophyData = playerTrophies[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
				Trophies.Add (new DisplayedTrophy(new Trophy(this.Profile.Id,
				                                             System.Convert.ToInt32(trophyData[0]),
				                                             System.Convert.ToInt32(trophyData[1]),
				                                             System.DateTime.ParseExact(trophyData[2], "yyyy-MM-dd HH:mm:ss", null)),
				                                  trophyData[3],
				                                  trophyData[4]));
			}

		}
	}
}