using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class PlayingCardMakerScript : MonoBehaviour {
	
	public GameObject playingCardObject;
	
	private GameObject myCard;
	private Card c;
	private float heightScreen=Screen.height;
	private float widthScreen=Screen.width;
	private string xpString;
	private int xp;
	private Rect centralWindow;
	
	public static PlayingCardMakerScript instance;
	
	// Use this for initialization
	void OnGUI() 
	{
	
	}
	
	public void clickedCard(int id)
	{

	}
	
	void Start () {
		
		ApplicationModel.credits = 100000;
		
		instance = this;
		this.resize ();
		this.xpString = "";
		this.myCard = Instantiate(playingCardObject) as GameObject;
		this.myCard.name = "myCard";
		
		this.myCard.transform.localScale = new Vector3(5,5,5);
		this.myCard.transform.position =  new Vector3(0,0,0);

		
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
	
		//this.myCard.GetComponent<PlayingCardController> ().isMine = false;
		//this.myCard.GetComponent<PlayingCardController> ().setCard (c);
		this.myCard.GetComponent<PlayingCardController> ().show ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(heightScreen!=Screen.height || widthScreen!=Screen.width)
		{
			heightScreen=Screen.height;
			widthScreen=Screen.width;
			this.resize();
		}
	}
	
	private void resize()
	{
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
	}
}
