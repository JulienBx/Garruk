using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class HasLeftRoomPopUpController : MonoBehaviour 
{
    public void reset()
    {
        gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingHasLeftRooomPopUp.getReference(0);
        gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text =  WordingHasLeftRooomPopUp.getReference(1);
        gameObject.transform.FindChild ("Button").GetComponent<HasLeftRoomPopUpConfirmButtonController> ().reset ();
        gameObject.transform.FindChild ("CloseButton").GetComponent<HasLeftRoomPopUpCloseButtonController> ().reset ();
    }
    public void resize()
    {
    }
    public void exitPopUp()
    {
        SoundController.instance.playSound(8);
        NewHomePageController.instance.hideHasLeftRoomPopUp ();
    }
}

