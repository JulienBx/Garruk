using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SearchUsersPopUpModel 
{
	public IList<User> users;

	private string URLSearchUsers = ApplicationModel.host + "search_users.php";
	
	public SearchUsersPopUpModel()
	{
	}
	public IEnumerator searchForUsers(string search){
		
		this.users = new List<User> ();
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField("myform_search", search);
		
		
		WWW w = new WWW(URLSearchUsers, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			Debug.Log (w.error); 
		else 
		{
			string[] data=w.text.Split(new string[] { "#USER#" }, System.StringSplitOptions.None);
			for (int i=0;i<data.Length-1;i++)
			{
				string[] userData =data[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
				this.users.Add (new User());
				this.users[i].Username= userData[0];
				this.users[i].IdProfilePicture= System.Convert.ToInt32(userData[1]);
			}
		}
	}
}



