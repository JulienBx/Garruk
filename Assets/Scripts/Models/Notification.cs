using UnityEngine;
using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class Notification 
{

	public DateTime Date;
	public bool IsRead;
	public int Id;
	public int IdUser;
	public int IdSendingUser;
	public int IdNotificationType;
	public string Param1;
	public string Param2;
	public string Param3;

	private string URLAddNotification = ApplicationModel.host +"add_notification.php";
	private string URLRemoveNotification = ApplicationModel.host +"remove_notification.php";

	public Notification()
	{
	}
	public Notification(int id, DateTime date, bool isread, int idnotificationtype)
	{
		this.Id = id;
		this.Date = date;
		this.IsRead = isread;
		this.IdNotificationType = idnotificationtype;
	}
	public Notification(int iduser, int idsendinguser, int idnotificationtype)
	{
		this.IdUser = iduser;
		this.IdSendingUser = idsendinguser;
		this.IdNotificationType = idnotificationtype;
	}
	public Notification(int iduser, int idsendinguser, bool isread, int idnotificationtype, string param1="", string param2="", string param3="")
	{
		this.IdUser = iduser;
		this.IdSendingUser = idsendinguser;
		this.IsRead = isread;
		this.IdNotificationType = idnotificationtype;
		this.Param1 = param1;
		this.Param2 = param2;
		this.Param3 = param3;
	}
	public IEnumerator add()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_iduser",this.IdUser.ToString());
		form.AddField("myform_idsendinguser",this.IdSendingUser.ToString());
		form.AddField("myform_isread",System.Convert.ToInt32(this.IsRead).ToString());
		form.AddField("myform_idnotificationtype", this.IdNotificationType.ToString ());
		form.AddField("myform_param1", this.Param1); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_param2", this.Param2);
		form.AddField("myform_param3", this.Param3);
		
		WWW w = new WWW(URLAddNotification, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null){ 
			Debug.Log (w.error); 
		}
	}
	public IEnumerator remove()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_iduser",this.IdUser.ToString());
		form.AddField("myform_idsendinguser",this.IdSendingUser.ToString());
		form.AddField("myform_idnotificationtype", this.IdNotificationType.ToString ());
		form.AddField("myform_param1", this.Param1); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_param2", this.Param2);
		form.AddField("myform_param3", this.Param3);
		
		WWW w = new WWW(URLRemoveNotification, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null){ 
			Debug.Log (w.error); 
		}
	}
}



