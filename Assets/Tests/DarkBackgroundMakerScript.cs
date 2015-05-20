using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class DarkBackgroundMakerScript : MonoBehaviour {
	
	public GameObject darkBackgroundObject;
	private GameObject darkBackground;

	private float heightScreen=Screen.height;
	private float widthScreen=Screen.width;
	
	public static DarkBackgroundMakerScript instance;

	
	// Use this for initialization
	void OnGUI() 
	{

	}
	void Start () 
	{	
		this.darkBackground = Instantiate(darkBackgroundObject) as GameObject;
		this.darkBackground.name = "darkBackground";
		this.resize ();
	}
	// Update is called once per frame
	void Update () {
		
		if(heightScreen!=Screen.height || widthScreen!=Screen.width)
		{
			this.resize();
		}
	}
	private void resize()
	{
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.darkBackground.GetComponent<DarkBackgroundController> ().resize();
	}
}
