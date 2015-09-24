using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class testYBO : MonoBehaviour
{
	private GameObject tutorial;



	void Start()
	{
		this.tutorial = GameObject.Find ("TutorialObject2");
		this.tutorial.transform.FindChild("Background").GetComponent<TutorialBackgroundController> ().resize (new Rect(-4,-2,2,2),0.5f,0.5f);
	
	}
}