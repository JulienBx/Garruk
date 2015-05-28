using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class SkillMakerScript : MonoBehaviour {
	
	public GameObject skillObject;
	
	private GameObject mySkill;
	private Skill s;
	private float heightScreen=Screen.height;
	private float widthScreen=Screen.width;

	
	
	// Use this for initialization
	void OnGUI() 
	{
		
	}
	

	
	void Start () {
		
		ApplicationModel.credits = 100000;
	
		this.resize ();
		this.mySkill = Instantiate(skillObject) as GameObject;
		this.mySkill.name = "mySkill";
		
		this.mySkill.transform.localScale = new Vector3(1,1,1);
		this.mySkill.transform.position =  new Vector3(0,0,0);
		
		

		this.s = new Skill ();
		s.Id = 1;
		s.Level = 2;
		s.IsActivated = 1;
		s.ManaCost = 2;
		s.Name = "ma compétence";
		s.Description = "bla bla bla";
		s.Power = 2;

		
		//this.mySkill.GetComponent<SkillObjectController> ().setSkill (s);
		//this.mySkill.GetComponent<SkillObjectController> ().setAttack();
		this.mySkill.GetComponent<SkillObjectController> ().show ();

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
	}
}
