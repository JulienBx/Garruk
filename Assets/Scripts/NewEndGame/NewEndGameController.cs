using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class NewEndGameController : MonoBehaviour
{
	public static NewEndGameController instance;
	private NewEndGameModel model;
	
	public GameObject tutorialObject;
	public GameObject blockObject;
	
	private GameObject menu;
	private GameObject tutorial;

	private GameObject mainBlock;
	private GameObject lastResultsBlock;
	private GameObject statsBlock;

	private int widthScreen;
	private int heightScreen;
	private float pixelPerUnit;
	private float worldWidth;
	private float worldHeight;

	
	private bool isTutorialLaunched;
	private bool toResizeBackUI;
	
	void Update()
	{	

	}
	void Awake()
	{
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.pixelPerUnit = 108f;
		this.initializeScene ();
	}
	void Start()
	{
		instance = this;
		this.model = new NewEndGameModel ();
		this.resize ();
		StartCoroutine (this.initialization ());
	}
	private IEnumerator initialization()
	{
		//yield return StartCoroutine (model.getEndGameData ()); A REMODIFIER AVEC LE NOUVEAU SYSTEME D'ENREGISTREMENT DES MATCHS

		if(model.currentUser.TutorialStep==4)
		{
			this.tutorial = Instantiate(this.tutorialObject) as GameObject;
			this.tutorial.AddComponent<EndGameTutorialController>();
			this.tutorial.GetComponent<EndGameTutorialController>().launchSequence(0);
			this.menu.GetComponent<newMenuController>().setTutorialLaunched(true);
			this.isTutorialLaunched=true;
		} 

		yield break;
	}
	public void initializeScene()
	{
		menu = GameObject.Find ("newMenu");
		menu.GetComponent<newMenuController> ().setCurrentPage (0);
		this.mainBlock = Instantiate(this.blockObject) as GameObject;
		this.statsBlock = Instantiate(this.blockObject) as GameObject;
		this.lastResultsBlock = Instantiate(this.blockObject) as GameObject;

	}


	public void resize()
	{

		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		menu.GetComponent<newMenuController> ().resizeMeunObject (worldHeight,worldWidth);

		float mainBlockLeftMargin = 3f;
		float mainBlockRightMargin = 3f;
		float mainBlockUpMargin = 0.2f;
		float mainBlockDownMargin = 2.7f;
		
		float mainBlockHeight = worldHeight - mainBlockUpMargin-mainBlockDownMargin;
		float mainBlockWidth = worldWidth-mainBlockLeftMargin-mainBlockRightMargin;
		Vector2 mainBlockOrigin = new Vector3 (-worldWidth/2f+mainBlockLeftMargin+mainBlockWidth/2f, -worldHeight / 2f + mainBlockDownMargin + mainBlockHeight / 2,0f);
		
		this.mainBlock.GetComponent<BlockController> ().resize(new Rect(mainBlockOrigin.x,mainBlockOrigin.y,mainBlockWidth,mainBlockHeight));

		float lastResultsBlockLeftMargin = this.worldWidth-2.8f;
		float lastResultsBlockRightMargin = 0f;
		float lastResultsBlockUpMargin = 0.6f;
		float lastResultsBlockDownMargin = 0.2f;
		
		float lastResultsBlockHeight = worldHeight - lastResultsBlockUpMargin-lastResultsBlockDownMargin;
		float lastResultsBlockWidth = worldWidth-lastResultsBlockLeftMargin-lastResultsBlockRightMargin;
		Vector2 lastResultsBlockOrigin = new Vector3 (-worldWidth/2f+lastResultsBlockLeftMargin+lastResultsBlockWidth/2f, -worldHeight / 2f + lastResultsBlockDownMargin + lastResultsBlockHeight / 2,0f);
		
		this.lastResultsBlock.GetComponent<BlockController> ().resize(new Rect(lastResultsBlockOrigin.x,lastResultsBlockOrigin.y,lastResultsBlockWidth,lastResultsBlockHeight));

		float statsBlockLeftMargin = 3f;
		float statsBlockRightMargin = 3f;
		float statsBlockUpMargin = 7.5f;
		float statsBlockDownMargin = 0.2f;
		
		float statsBlockHeight = worldHeight - statsBlockUpMargin-statsBlockDownMargin;
		float statsBlockWidth = worldWidth-statsBlockLeftMargin-statsBlockRightMargin;
		Vector2 statsBlockOrigin = new Vector3 (-worldWidth/2f+statsBlockLeftMargin+statsBlockWidth/2f, -worldHeight / 2f + statsBlockDownMargin + statsBlockHeight / 2,0f);
		
		this.statsBlock.GetComponent<BlockController> ().resize(new Rect(statsBlockOrigin.x,statsBlockOrigin.y,statsBlockWidth,statsBlockHeight));


		if(this.isTutorialLaunched)
		{
			this.tutorial.GetComponent<TutorialObjectController>().resize();
		}
	}

}