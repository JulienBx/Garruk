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

	private float timer;
	private float speed;

	private bool isHorizontal;

	void Update()
	{
		if(this.cardCurrentPosition<this.cardEndPosition)
		{
			this.cardCurrentPosition=this.cardCurrentPosition+Time.deltaTime*this.speed;
		}
		else
		{
			this.cardCurrentPosition=this.cardStartPosition;
		}
		if(this.isHorizontal)
		{
			gameObject.transform.Find ("Card").localPosition = new Vector3 (this.cardCurrentPosition, 0f, 0f);
		}
		else
		{
			gameObject.transform.Find ("Card").localPosition = new Vector3 (0f, this.cardCurrentPosition, 0f);
		}

	}
	public void setVerticalTranslation()
	{
		this.speed = 2f;
		this.cardStartPosition = -1.5f;
		this.cardEndPosition = 1.5f;
		this.cardCurrentPosition = this.cardStartPosition;
		this.isHorizontal=false;
		gameObject.transform.FindChild("Halo").localPosition=new Vector3(0f,1.5f,0f);
		gameObject.transform.FindChild("Card").localPosition=new Vector3(0f,-1.5f,0f);
	}
	public void setHorizontalTranslation()
	{
		this.speed = 2f;
		this.cardStartPosition = -1.5f;
		this.cardEndPosition = 1.5f;
		this.cardCurrentPosition = this.cardStartPosition;
		this.isHorizontal=true;
		gameObject.transform.FindChild("Halo").localPosition=new Vector3(1.5f,0f,0f);
		gameObject.transform.FindChild("Card").localPosition=new Vector3(-1.5f,0f,0f);
	}
}

