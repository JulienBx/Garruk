using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PopUpUserController : MonoBehaviour 
{
	private bool isHovering;
	
	void OnMouseOver()
	{
		if(!isHovering)
		{
			this.isHovering=true;
			gameObject.transform.parent.GetComponent<PopUpController>().OnMouseOver();
			gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("username").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("nbWins").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("nbLooses").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("ranking").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("collection").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
		}
	}
	void OnMouseExit()
	{
		if(isHovering)
		{
			this.isHovering=false;
			gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("username").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("nbWins").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("nbLooses").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("ranking").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("collection").FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseDown()
	{
		//ApplicationModel.profileChosen = gameObject.transform.FindChild ("username").GetComponent<TextMeshPro> ().text;
		//Application.LoadLevel ("Profile");
	}
	public void show (User u)
	{
		gameObject.transform.FindChild ("username").GetComponent<TextMeshPro> ().text = u.Username;
		gameObject.transform.FindChild ("picture").GetComponent<SpriteRenderer> ().sprite =newMenuController.instance.returnThumbPicture(u.idProfilePicture);
		gameObject.transform.FindChild ("nbWins").FindChild ("Title").GetComponent<TextMeshPro> ().text = "V";
		gameObject.transform.FindChild ("nbWins").FindChild ("Value").GetComponent<TextMeshPro> ().text = u.TotalNbWins.ToString();
		gameObject.transform.FindChild ("nbLooses").FindChild ("Title").GetComponent<TextMeshPro> ().text = "D";
		gameObject.transform.FindChild ("nbLooses").FindChild ("Value").GetComponent<TextMeshPro> ().text = u.TotalNbLooses.ToString();
		gameObject.transform.FindChild ("ranking").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Cl";
		gameObject.transform.FindChild ("ranking").FindChild ("Value").GetComponent<TextMeshPro> ().text = u.Ranking.ToString();
		gameObject.transform.FindChild ("collection").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Co";
		gameObject.transform.FindChild ("collection").FindChild ("Value").GetComponent<TextMeshPro> ().text = u.CollectionRanking.ToString();
	}
	public bool getIsHovering()
	{
		return isHovering;
	}
}

