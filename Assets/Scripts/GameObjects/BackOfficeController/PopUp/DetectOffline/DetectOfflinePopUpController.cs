using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class DetectOfflinePopUpController : MonoBehaviour 
{
	public void reset()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDetectOfflinePopUp.getReference(0);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDetectOfflinePopUp.getReference(1);
		gameObject.transform.FindChild ("Button").GetComponent<DetectOfflinePopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<DetectOfflinePopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		BackOfficeController.instance.hideDetectOfflinePopUp ();
	}
}

