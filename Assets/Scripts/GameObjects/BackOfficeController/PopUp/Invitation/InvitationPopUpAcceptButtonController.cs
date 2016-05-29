using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class InvitationPopUpAcceptButtonController : SimpleButtonController 
{
    public override void mainInstruction ()
	{
		InvitationPopUpController.instance.acceptInvitationHandler ();
	}
}

