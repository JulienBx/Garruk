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
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingSelectPicturePopUp.getReference(0);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text =  WordingSelectPicturePopUp.getReference(1);
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
		SoundController.instance.playSound(8);
		NewProfileController.instance.changeUserPictureHandler(this.activeButton);
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		NewProfileController.instance.hideSelectPicturePopUp ();
	}
}

