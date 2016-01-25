using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class ScrollUpHelpController : MonoBehaviour 
{

	private float scrollHelpCurrentPosition;
	private float scrollHelpStartPosition;
	private float scrollHelpEndPosition;

	private float timer;
	private float speed;


	void Update()
	{
		if(this.scrollHelpCurrentPosition<this.scrollHelpEndPosition)
		{
			this.scrollHelpCurrentPosition=this.scrollHelpCurrentPosition+Time.deltaTime*this.speed;
		}
		else
		{
			this.scrollHelpCurrentPosition=this.scrollHelpStartPosition;
		}
	}
	void Awake () 
	{
		this.speed = 2f;
		this.scrollHelpStartPosition=0f;
		this.scrollHelpCurrentPosition=0f;
		this.scrollHelpEndPosition=2f;
	}
	void Start () 
	{	
	}
}

