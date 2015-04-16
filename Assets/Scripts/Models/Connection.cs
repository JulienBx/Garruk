using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Connection 
{
	private string URLConfirmConnection = ApplicationModel.dev + "confirm_connection.php";
	private string URLAddConnection= ApplicationModel.dev + "add_connection.php";
	private string URLRemoveConnection= ApplicationModel.dev + "remove_connection.php";

	public int Id;
	public int IdUser1;
	public int IdUser2;
	public bool IsAccepted;


	public Connection(int id, int iduser1, int iduser2, bool isaccepted)
	{
		this.Id = id;
		this.IdUser1 = iduser1;
		this.IdUser2 = iduser2;
		this.IsAccepted = isaccepted;
	}
	public Connection(int iduser1, int iduser2, bool isaccepted)
	{
		this.IdUser1 = iduser1;
		this.IdUser2 = iduser2;
		this.IsAccepted = isaccepted;
	}
	public IEnumerator remove()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_id", this.Id.ToString()); 					// hashcode de sécurité, doit etre identique à celui sur le serveur

		WWW w = new WWW(URLRemoveConnection, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null)
		{
			Debug.Log (w.error); 
		}
	}
	public IEnumerator confirm()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_id", this.Id.ToString()); 					// hashcode de sécurité, doit etre identique à celui sur le serveur

		WWW w = new WWW(URLConfirmConnection, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null)
		{
			Debug.Log (w.error); 
		}
	}
	public IEnumerator add()
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_iduser1", this.IdUser1.ToString()); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_iduser2", this.IdUser2.ToString()); 			
		form.AddField("myform_isaccepted", (System.Convert.ToInt32(this.IsAccepted)).ToString()); 	

		WWW w = new WWW(URLConfirmConnection, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null)
		{
			Debug.Log (w.error); 
		}
		else
		{
			this.Id=System.Convert.ToInt32(w.text);
		}
	}
}
