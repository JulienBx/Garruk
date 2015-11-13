using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class DragHelpController : MonoBehaviour 
{

	private float cardCurrentPosition;
	private float cardStartPosition;
	private float cardEndPosition;

	private float lineCurrentPosition;
	private float lineStartPosition;
	private float lineEndPosition;

	private float translationRatio;
	private float scale;

	private float timer;
	private float speed;


	void Update()
	{
		if(this.cardCurrentPosition<this.cardEndPosition)
		{
			this.cardCurrentPosition=this.cardCurrentPosition+Time.deltaTime*this.speed;
		}
		else
		{
			this.cardCurrentPosition=this.cardStartPosition;
			//this.gameObject.transform.FindChild("Line").gameObject.SetActive(true);
		}
		this.translationRatio = (this.cardCurrentPosition - this.cardStartPosition) / (this.cardEndPosition - this.cardStartPosition);
//		if(this.translationRatio>0.6)
//		{
//			this.gameObject.transform.FindChild("Line").gameObject.SetActive(false);
//		}
		this.lineCurrentPosition = this.lineStartPosition + this.translationRatio * (this.lineEndPosition - this.lineStartPosition);


		gameObject.transform.Find ("Card").localPosition = new Vector3 (this.cardCurrentPosition, 1.38f, 0f);
		//gameObject.transform.Find ("Line").localPosition = new Vector3 (this.lineCurrentPosition, 1.38f, 0f);
	}
	void Awake () 
	{
		this.speed = 2f;
		this.scale = 0.3825155f;
		this.cardStartPosition = -1.5f;
		this.cardEndPosition = 1.5f;
		this.lineStartPosition = 0f;
		this.lineEndPosition = 0.789f;
		this.cardCurrentPosition = this.cardStartPosition;
		this.lineCurrentPosition = this.lineStartPosition;
	}
	void Start () 
	{	

	}

}

