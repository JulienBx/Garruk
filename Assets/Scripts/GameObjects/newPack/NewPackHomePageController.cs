using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewPackHomePageController : NewPackController
{

	public override void show()
	{
		base.show ();
		if(this.p.New)
		{
			this.gameObject.transform.FindChild("New").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("New").GetComponent<TextMeshPro>().text="Nouveau !";
		}
		else
		{
			this.gameObject.transform.FindChild("New").gameObject.SetActive(false);
		}
	}
	public override void OnMouseDown()
	{
		NewHomePageController.instance.buyPackHandler (this.Id);
	}
}

