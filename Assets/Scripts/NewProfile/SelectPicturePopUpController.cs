using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class SelectPicturePopUpController : MonoBehaviour 
{
	
	public int activeButton;


	void Awake()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Sélectionnez un avatar";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Confirmer";
	}

	public void selectPicture(int id)
	{
		this.activeButton = id;
		for(int i=0;i<10;i++)
		{
			if(id==i)
			{
				gameObject.transform.FindChild("picture"+i).GetComponent<SelectPicturePopUpPictureController>().setActive(true);
			}
			else
			{
				gameObject.transform.FindChild("picture"+i).GetComponent<SelectPicturePopUpPictureController>().setActive(false);
			}
		}
	}
	public void confirmPicture()
	{
		NewProfileController.instance.changeUserPictureHandler(this.activeButton);
	}
}
