using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewPackStoreController : NewPackController
{
	public override void buyPackHandler ()
	{
        if(!BackOfficeController.instance.getIsScrolling() && !BackOfficeController.instance.getIsSwiping())
        {
            NewStoreController.instance.buyPackHandler (base.getId(),false,false);
        }
	}

}

