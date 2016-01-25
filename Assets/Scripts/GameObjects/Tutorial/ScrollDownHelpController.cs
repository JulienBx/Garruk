using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class ScrollDownHelpController : MonoBehaviour 
{

	private float scrollHelpCurrentPosition;
	private float scrollHelpStartPosition;
	private float scrollHelpEndPosition;

	private float timer;
	private float speed;


	void Update()
	{
		if(this.scrollHelpCurrentPosition>this.scrollHelpEndPosition)
		{
			this.scrollHelpCurrentPosition=this.scrollHelpCurrentPosition-Time.deltaTime*this.speed;
		}
		else
		{
			this.scrollHelpCurrentPosition=this.scrollHelpStartPosition;
		}
	}
	void Awake () 
	{
		this.speed = 2f;
		this.scrollHelpStartPosition=2f;
		this.scrollHelpCurrentPosition=2f;
		this.scrollHelpEndPosition=0f;
	}
	void Start () 
	{	
	}
}

