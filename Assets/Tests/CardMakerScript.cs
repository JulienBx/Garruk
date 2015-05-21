using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CardMakerScript : MonoBehaviour {

	public GameObject cardObject;

	private GameObject myCard;
	private Card c;
	private float heightScreen=Screen.height;
	private float widthScreen=Screen.width;
	private string xpString;
	private int xp;
	private Rect centralWindow;

	public static CardMakerScript instance;



	// Use this for initialization
	void OnGUI() 
	{
		xpString = GUI.TextField(new Rect(10, 10, 200, 20), xpString, 25);
		if (GUI.Button (new Rect(10, 40, 200, 20), "Ajouter Xp")) 
		{
			xp=System.Convert.ToInt32(xpString);
			c.Experience=c.Experience+xp;
			this.myCard.GetComponent<CardController> ().animateExperience (this.c);

		}
	}

	public void clickedCard(int id)
	{
		print ("La carte " + id + " a été cliqué");
	}

	void Start () {

		ApplicationModel.credits = 100000;

		instance = this;
		this.resize ();
		this.xpString = "";
		this.myCard = Instantiate(cardObject) as GameObject;
		this.myCard.name = "myCard";

		this.myCard.transform.localScale = new Vector3(Screen.height/120f,Screen.height/120f,Screen.height/120f);
		this.myCard.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0.385f*Screen.width ,0.45f*Screen.height-1 , 10));
	
		//this.myCard.transform.localScale = new Vector3(2,2,2);
		//this.myCard.transform.position = new Vector3(0,0,0);
		


		this.c = new Card ();

		this.c.Id = 6;
		this.c.Title = "Ma carte";
		this.c.TitleClass = "Berkek";
		this.c.ArtIndex = 3;
		this.c.Life = 99;
		this.c.LifeLevel = 2;
		this.c.Attack = 99;
		this.c.AttackLevel = 2;
		this.c.Move = 9;
		this.c.MoveLevel = 2;
		this.c.Speed = 99;
		this.c.SpeedLevel = 1;
		this.c.Experience = 10;
		this.c.IdOWner = 2;
		this.c.UsernameOwner = "yoann";
		this.c.Price = 2000;
		this.c.nbWin = 10;
		this.c.nbLoose = 15;
		this.c.onSale = 1;
		this.c.Price = 1000;

		Skill tempSkill1 = new Skill ();
		tempSkill1.Id = 1;
		tempSkill1.Level = 2;
		tempSkill1.IsActivated = 1;
		tempSkill1.ManaCost = 2;
		tempSkill1.Name = "ma compétence";
		tempSkill1.Description = "bla bla bla";
		tempSkill1.Power = 2;

		Skill tempSkill2 = new Skill ();
		tempSkill2.Id = 3;
		tempSkill2.Level = 2;
		tempSkill2.IsActivated = 1;
		tempSkill2.ManaCost = 3;
		tempSkill2.Name = "ma compétence 2";
		tempSkill2.Description = "bla bla";
		tempSkill2.Power = 1;

		this.c.Skills = new List<Skill> ();
		this.c.Skills.Add (tempSkill1);
		this.c.Skills.Add (tempSkill2);

		this.myCard.AddComponent<CardMarketController> ();
		//this.myCard.AddComponent<CardMyGameController> ();
		this.myCard.GetComponent<CardController> ().setCard (c);
		this.myCard.GetComponent<CardController> ().setSkills();
		this.myCard.GetComponent<CardController> ().setExperience();
		this.myCard.GetComponent<CardController> ().show ();
		this.myCard.GetComponent<CardMarketController> ().setMarketFeatures ();
		this.myCard.GetComponent<CardController> ().setCentralWindowRect (centralWindow);
		//this.myCard.GetComponent<CardMyGameController> ().setFocusMyGameFeatures ();
		this.myCard.GetComponent<CardMarketController> ().setFocusMarketFeatures ();

	}
	
	// Update is called once per frame
	void Update () {

		if(heightScreen!=Screen.height || widthScreen!=Screen.width)
		{
			heightScreen=Screen.height;
			widthScreen=Screen.width;
			this.resize();
			this.myCard.GetComponent<CardController> ().setCentralWindowRect (centralWindow);
			this.myCard.GetComponent<CardController> ().resize();

		}
	}
	private void resize()
	{
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
	}
}
