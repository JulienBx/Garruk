using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Connection 
{
	private string URLConfirmConnection = ApplicationModel.host + "confirm_connection.php";
	private string URLAddConnection= ApplicationModel.host + "add_connection.php";
	private string URLRemoveConnection= ApplicationModel.host + "remove_connection.php";

	public int Id;
	public int User;
	public bool IsInviting;
	public bool IsAccepted; 
	public string Error="";


	public Connection()
	{
	}

	public IEnumerator remove()
	{
		WWWForm form = new WWWForm();
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_id", this.Id.ToString());

		ServerController.instance.setRequest(URLRemoveConnection, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();

	}
	public IEnumerator confirm()
	{
		WWWForm form = new WWWForm(); 
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_id", this.Id.ToString()); 					// hashcode de sécurité, doit etre identique à celui sur le serveur

		ServerController.instance.setRequest(URLConfirmConnection, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();

		if(this.Error=="")
		{
			this.IsAccepted=true;
		}
	}
	public IEnumerator add(int iduser1, int iduser2, bool isaccepted)
	{
		WWWForm form = new WWWForm(); 
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_iduser1", iduser1.ToString()); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_iduser2", iduser2.ToString()); 			
		form.AddField("myform_isaccepted", (System.Convert.ToInt32(isaccepted)).ToString()); 	

		ServerController.instance.setRequest(URLAddConnection, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");
		this.Error=ServerController.instance.getError();

		if(this.Error=="")
		{
			string result = ServerController.instance.getResult();
			this.Id=System.Convert.ToInt32(result);
		}
	}
}
