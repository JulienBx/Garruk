using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Invitation
{
	
	public DateTime Date;
	public User InvitedUser;
	public User SendingUser;
	public int Id;
	public int Status;
	public string Error;
	
	private string URLAddInvitation = ApplicationModel.host +"add_invitation.php";
	private string URLChangeStatus = ApplicationModel.host +"change_invitation_status.php";
	
	public Invitation()
	{
	}
	public Invitation(User inviteduser, User sendinguser)
	{
		this.InvitedUser = inviteduser;
		this.SendingUser = sendinguser;
	}
	public IEnumerator add()
	{
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_sendinguser", this.SendingUser.Id.ToString()); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_inviteduser", this.InvitedUser.Id.ToString()); 

		ServerController.instance.setRequest(URLAddInvitation, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();

		if(this.Error=="")
		{
			string result = ServerController.instance.getResult();
			this.Id=System.Convert.ToInt32(result);
		}
	}
	public IEnumerator changeStatus(int status)
	{
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_id", this.Id); 
		form.AddField ("myform_status", status); 
		
		WWW w = new WWW (URLChangeStatus, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente

		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		}
	}
}



