//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using TMPro;
//
//public class ConfrontationController : MonoBehaviour 
//{
//	
//	public Result r;
//	private int Id;
//	private bool isHovering;
//	
//	
//	void OnMouseOver()
//	{
//		if(!isHovering)
//		{
//			this.isHovering=true;
//		}
//	}
//	void OnMouseExit()
//	{
//		if(isHovering)
//		{
//			this.isHovering=false;
//		}
//	}
//	void OnMouseDown()
//	{
//
//	}
//	public void show(bool hasWon)
//	{
//		if(hasWon)
//		{
//			gameObject.transform.FindChild ("Result").GetComponent<TextMeshPro> ().text = "Victoire";
//		}
//		else
//		{
//			gameObject.transform.FindChild ("Result").GetComponent<TextMeshPro> ().text = "Défaite";
//		}
//		gameObject.transform.FindChild ("Date").GetComponent<TextMeshPro> ().text = "Le " + this.r.Date.ToString("dd/MM/yyyy");
//		if(r.GameType>2 || r.GameType==0)
//		{
//			gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = NewProfileController.instance.gameTypesPicto[0];
//		}
//		else if(r.GameType==1)
//		{
//			gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = NewProfileController.instance.gameTypesPicto[1];
//		}
//		else if(r.GameType==2)
//		{
//			gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = NewProfileController.instance.gameTypesPicto[2];
//		}
//	}
//	public void setId(int Id)
//	{
//		this.Id = Id;
//	}
//}
//
