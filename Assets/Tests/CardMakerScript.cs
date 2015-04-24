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
	private float screenHeight=Screen.height;
	private float screenWidth=Screen.width;
	private string xpString;
	private int xp;



	// Use this for initialization
	void OnGUI() 
	{
		xpString = GUI.TextField(new Rect(10, 10, 200, 20), xpString, 25);
		if (GUI.Button (new Rect(10, 40, 200, 20), "Ajouter Xp")) 
		{
			xp=System.Convert.ToInt32(xpString);
			c.Experience=c.Experience+xp;
			this.myCard.GetComponent<CardController> ().animateExperience ();

		}

	}
	void Start () {

		this.xpString = "";
		this.myCard = Instantiate(cardObject) as GameObject;
		this.myCard.name = "myCard";
		this.myCard.transform.localScale = new Vector3(4f, 4f, 4f); 
		this.myCard.transform.localPosition = new Vector3(0, 0, 0);

		this.c = new Card ();

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
		this.c.Experience = 0;

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

		this.myCard.GetComponent<CardController> ().setCard (c);
		this.myCard.GetComponent<CardController> ().setSkills();
		this.myCard.GetComponent<CardController> ().setExperience();
		this.myCard.GetComponent<CardController> ().show ();


	}
	
	// Update is called once per frame
	void Update () {

		if(screenHeight!=Screen.height || screenWidth!=Screen.width)
		{
			screenHeight=Screen.height;
			screenWidth=screenWidth;
			this.myCard.GetComponent<CardController> ().resize();

		}
	}
}
