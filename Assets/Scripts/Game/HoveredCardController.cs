using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class HoveredCardController : MonoBehaviour
{	
	public float timer = 0f;
	public float animationTime = 0.25f;
	public Vector3 myPosition ;
	public Vector3 initialPosition ;
	public float realwidth ;
	
	public int status = 0;
	public bool isDisplayed = false ;
	
	public GameCard nextDisplayedCard ;
	public int nextDisplayedCharacter ;
	public int currentCharacter = -1;
	
	public void setNextDisplayedCharacter(int i, GameCard c){
		this.nextDisplayedCharacter = i;
		this.nextDisplayedCard = c;
	}
}


