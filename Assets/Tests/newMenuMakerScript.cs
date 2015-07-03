using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class newMenuMakerScript : MonoBehaviour
{
	public static newMenuMakerScript instance;
	private GameObject menu;
	
	void Start()
	{
		instance = this;
		menu = GameObject.Find ("newMenu");
		menu.GetComponent<newMenuController> ().setCurrentPage (2);
	}
}