using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewPackHomePageController : NewPackController
{
	public override void buyPackHandler ()
	{
        if(!BackOfficeController.instance.getIsScrolling() && !BackOfficeController.instance.getIsSwiping())
        {
            NewHomePageController.instance.buyPackHandler();
        }
	}
	public override void buttonHovered(bool value)
	{
		NewHomePageController.instance.mouseOnBuyPackButton(value);
	}
}

