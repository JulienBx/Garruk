using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PlayPopUpController : MonoBehaviour 
{
	
	private Cup c;
	private Division d;
	private FriendlyGame f;

	private bool arePicturesLoading;
	
	public virtual void Update ()
	{
		if(arePicturesLoading)
		{
			if(checkIfPicturesAreLoaded())
			{
				this.setPictures();
				this.arePicturesLoading=false;
			}
		}
	}
	void Awake()
	{
		this.initialize ();
	}
	public void setCup(Cup c)
	{
		this.c = c;
	}
	public void setDivision(Division d)
	{
		this.d = d;
	}
	public void setFriendlyGame(FriendlyGame f)
	{
		this.f = f;
	}
	public void initialize()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Choisir un type de match";
		gameObject.transform.FindChild ("Button0").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Match Amical";
		gameObject.transform.FindChild ("quitButton").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Annuler";
		for(int i=0;i<3;i++)
		{
			gameObject.transform.FindChild("Button"+i).GetComponent<PlayPopUpCompetitionButtonController>().setId(i);
		}
	}
	public void show()
	{
		gameObject.transform.FindChild ("Button1").FindChild ("Title").GetComponent<TextMeshPro> ().text = this.d.Name;
		gameObject.transform.FindChild ("Button2").FindChild ("Title").GetComponent<TextMeshPro> ().text = this.c.Name;
		StartCoroutine (this.d.setPicture ());
		StartCoroutine (this.c.setPicture ());
		this.arePicturesLoading=true;
	}
	private bool checkIfPicturesAreLoaded()
	{
		if(!this.d.isTextureLoaded)
		{
			return false;
		}
		if(!this.c.isTextureLoaded)
		{
			return false;
		}
		return true;
	}
	private void setPictures()
	{
		gameObject.transform.FindChild ("Button1").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.d.texture;
		gameObject.transform.FindChild ("Button2").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.c.texture;
	}
	public void joinGame(int id)
	{
		if(Application.loadedLevelName=="NewHomePage")
		{
			PhotonNetwork.Disconnect();
		}
		ApplicationModel.gameType = id;
		if(id==1)
		{
			Application.LoadLevel("DivisionLobby");
		}
		else if(id==2)
		{
			Application.LoadLevel("CupLobby");
		}
		else
		{
			Application.LoadLevel("Game");
		}
	}
	public void quitPopUp()
	{
		newMenuController.instance.hidePlayPopUp ();
	}
}

