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
			gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
			gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			gameObject.transform.FindChild("divisionIcon").GetComponent<DivisionIconController>().setHoveredState();
		}
	}
	void OnMouseExit()
	{
		if(isHovering)
		{
			this.isHovering=false;
			gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
			gameObject.transform.FindChild("divisionIcon").GetComponent<DivisionIconController>().setInitialState();
		}
	}
	void OnMouseDown()
	{
		SoundController.instance.playSound(8);
		ApplicationModel.player.ProfileChosen = gameObject.transform.FindChild ("Username").GetComponent<TextMeshPro> ().text;
		SceneManager.LoadScene ("NewProfile");	
	}
	public void show()
	{
		gameObject.transform.FindChild ("Username").GetComponent<TextMeshPro> ().text = this.u.Username;
		gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnLargeProfilePicture(this.u.IdProfilePicture);
		if(this.u.TrainingStatus==-1)
		{
			gameObject.transform.FindChild("divisionIcon").gameObject.SetActive(true);
			gameObject.transform.FindChild("divisionIcon").GetComponent<DivisionIconController>().setDivision(this.u.Division);
		}
		else
		{
			gameObject.transform.FindChild("divisionIcon").gameObject.SetActive(false);
		}
	}
	public void setId(int Id)
	{
		this.Id = Id;
	}
}

