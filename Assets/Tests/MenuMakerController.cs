using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class MenuMakerController : MonoBehaviour
{
	public static MenuMakerController instance;

	private GameObject menu;
	private GameObject block;
	private GameObject block2;
	private GameObject blockTitle;
	private GameObject blockTitle2;

	private int widthScreen;
	private int heightScreen;

	void Update()
	{	
		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			this.resize();
		}
	}
	void Awake()
	{
		instance = this;
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.initializeScene ();
		this.resize ();
	}
	public void initializeScene()
	{
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		menu = GameObject.Find ("Menu");
		block = GameObject.Find ("Block");
		block2 = GameObject.Find ("Block2");
		blockTitle = GameObject.Find ("BlockTitle");
		blockTitle2 = GameObject.Find ("BlockTitle2");
		menu.AddComponent<MenuMakerMenuController> ();
		menu.GetComponent<MenuController> ().setCurrentPage (3);
	}

	public void resize()
	{
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		menu.GetComponent<MenuController> ().resize ();

		float blockLeftMargin = ApplicationDesignRules.leftMargin;
		float blockRightMargin = ApplicationDesignRules.worldWidth/2f+0.025f;
		float blockUpMargin = 1.9f;
		float blockDownMargin = 0.2f;

		this.block.GetComponent<NewBlockController> ().resize(blockLeftMargin,blockRightMargin,blockUpMargin,blockDownMargin);
		this.blockTitle.transform.position = new Vector3 (-ApplicationDesignRules.worldWidth / 2f + blockLeftMargin + 0.3f, ApplicationDesignRules.worldHeight / 2f - blockUpMargin - 0.3f, 0f);

		float block2LeftMargin = ApplicationDesignRules.worldWidth/2f+0.025f;
		float block2RightMargin = ApplicationDesignRules.rightMargin;
		float block2UpMargin = 1.9f;
		float block2DownMargin = 0.2f;
		
		this.block2.GetComponent<NewBlockController> ().resize(block2LeftMargin,block2RightMargin,block2UpMargin,block2DownMargin);
		this.blockTitle2.transform.position = new Vector3 (-ApplicationDesignRules.worldWidth / 2f + block2LeftMargin + 0.3f, ApplicationDesignRules.worldHeight / 2f - block2UpMargin - 0.3f, 0f);




	}
}