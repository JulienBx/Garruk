using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class News 
{
	
	public DateTime Date;
	public int IdUser;
	public int IdSendingUser;
	public int IdNewsType;
	public string Param1;
	public string Param2;
	public string Param3;

	private string URLAddNews = ApplicationModel.host +"add_news.php";
	private string URLRemoveNews = ApplicationModel.host +"remove_news.php";
	
	public News()
	{
	}
	public News(int idnewstype, DateTime date)
	{
		this.IdNewsType = idnewstype;
		this.Date = date;
	}
	public News(int iduser, int idnewstype, string param1="", string param2="", string param3="")
	{
		this.IdUser = iduser;
		this.IdNewsType = idnewstype;
		this.Param1 = param1;
		this.Param2 = param2;
		this.Param3 = param3;
	}
	public IEnumerator add()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_iduser",this.IdUser.ToString());
		form.AddField("myform_idnewstype", this.IdNewsType.ToString ());
		form.AddField("myform_param1", this.Param1); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_param2", this.Param2);
		form.AddField("myform_param3", this.Param3);
		
		WWW w = new WWW(URLAddNews, form); 				// On envoie le formulaire à l'url sur le serveur 
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
		form.AddField("myform_idnewstype", this.IdNewsType.ToString ());
		form.AddField("myform_param1", this.Param1); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_param2", this.Param2);
		form.AddField("myform_param3", this.Param3);
		
		WWW w = new WWW(URLRemoveNews, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null){ 
			Debug.Log (w.error); 
		}
	}
}



