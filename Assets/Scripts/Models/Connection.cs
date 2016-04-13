using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Connection 
{
	private string URLConfirmConnection = ApplicationModel.host + "confirm_connection.php";
	private string URLAddConnection= ApplicationModel.host + "add_connection.php";
	private string URLRemoveConnection= ApplicationModel.host + "remove_connection.php";

	public int Id;
	public int IdUser1;
	public int IdUser2;
	public bool IsAccepted;
	public string Error="";


	public Connection()
	{
	}
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
	public IEnumerator add()
	{
		WWWForm form = new WWWForm(); 
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_iduser1", this.IdUser1.ToString()); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_iduser2", this.IdUser2.ToString()); 			
		form.AddField("myform_isaccepted", (System.Convert.ToInt32(this.IsAccepted)).ToString()); 	

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
