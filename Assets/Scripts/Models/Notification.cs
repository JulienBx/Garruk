using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class Notification
{
	public int SendingUser;
	public IList<int> Users;
	public Cards Cards;
	public IList<string> Values;
	public IList<string> Communications;
	public Results Results;
	public Trophies Trophies;
	public string Content;
	public DateTime Date;
	public bool IsRead;
	public int Id;
	public int IdUser;
	public int IdNotificationType;
	public string Description;
	public string HiddenParam;
	public string Param1;
	public string Param2;
	public string Param3; 
	private string URLAddNotification = ApplicationModel.host +"add_notification.php";

	public Notification()
	{
		this.Users = new List<int>();
		this.Cards = new Cards ();
		this.Values = new List<string> ();
		this.Results = new Results ();
		this.Trophies = new Trophies ();
		this.Communications=new List<string>();
	}
	public IEnumerator add()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_iduser",this.IdUser.ToString());
		form.AddField("myform_idsendinguser",this.SendingUser.ToString());
		form.AddField("myform_isread",System.Convert.ToInt32(this.IsRead).ToString());
		form.AddField("myform_idnotificationtype", this.IdNotificationType.ToString ());
		form.AddField("myform_hiddenparam", this.HiddenParam); 	
		form.AddField("myform_param1", this.Param1); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_param2", this.Param2);
		form.AddField("myform_param3", this.Param3);
		
		WWW w = new WWW(URLAddNotification, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;

		if (w.error != null)
		{ 
			Debug.Log (w.error); 
		}
	}
}

