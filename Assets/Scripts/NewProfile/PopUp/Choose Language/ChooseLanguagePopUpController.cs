using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class ChooseLanguagePopUpController : MonoBehaviour 
{
	public void reset()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingChooseLanguagePopUp.getReference(0);
		gameObject.transform.FindChild ("CloseButton").GetComponent<ChooseLanguagePopUpCloseButtonController> ().reset ();
		if(ApplicationModel.player.IdLanguage==0)
		{
			gameObject.transform.FindChild("Picture0").GetComponent<ChooseLanguagePopUpSelectionButtonController>().setActive(true);
			gameObject.transform.FindChild("Picture1").GetComponent<ChooseLanguagePopUpSelectionButtonController>().setActive(false);
		}
		else if(ApplicationModel.player.IdLanguage==1)
		{
			gameObject.transform.FindChild("Picture0").GetComponent<ChooseLanguagePopUpSelectionButtonController>().setActive(false);
			gameObject.transform.FindChild("Picture1").GetComponent<ChooseLanguagePopUpSelectionButtonController>().setActive(true);
		}
		else
		{
			gameObject.transform.FindChild("Picture0").GetComponent<ChooseLanguagePopUpSelectionButtonController>().setActive(false);
			gameObject.transform.FindChild("Picture1").GetComponent<ChooseLanguagePopUpSelectionButtonController>().setActive(false);
		}
	}
	public void resize()
	{
	}
	public void chooseLanguageHandler(int idLanguage)
	{
		NewProfileController.instance.chooseLanguageHandler(idLanguage);	
	}
	public void exitPopUp()
	{
		NewProfileController.instance.hideChooseLanguagePopUp();
	}

}

