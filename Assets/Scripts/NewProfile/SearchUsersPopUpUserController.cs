using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;

public class SearchUsersPopUpUserController : MonoBehaviour 
{
	
	public User u;
	public int Id;
	
	private bool isHovering;
	
	
	void OnMouseOver()
	{
		if(!isHovering)
		{
			this.isHovering=true;
			gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
		}
	}
	void OnMouseExit()
	{
		if(isHovering)
		{
			this.isHovering=false;
			gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseDown()
	{
		ApplicationModel.player.ProfileChosen = gameObject.transform.FindChild ("Username").GetComponent<TextMeshPro> ().text;
		SceneManager.LoadScene ("NewProfile");	
	}
	public void show()
	{
		gameObject.transform.FindChild ("Username").GetComponent<TextMeshPro> ().text = this.u.Username;
		gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnLargeProfilePicture(this.u.IdProfilePicture);
	}
	public void setId(int Id)
	{
		this.Id = Id;
	}
}

