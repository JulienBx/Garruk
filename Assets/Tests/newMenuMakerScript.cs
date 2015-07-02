using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class newMenuMakerScript : MonoBehaviour
{
	public static newMenuMakerScript instance;
	public GameObject newMenuObject;
	public GameObject backgroundImageModel;

	private GameObject menu;
	private GameObject backgroundImage;
	
	void Start()
	{
		instance = this;
		this.menu = Instantiate(this.newMenuObject) as GameObject;
		this.menu.GetComponent<newMenuController> ().setCurrentPage (1);
		this.backgroundImage = (GameObject)Instantiate(this.backgroundImageModel);
	}
}