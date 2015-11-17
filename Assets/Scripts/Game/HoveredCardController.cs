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
	
	public Card nextDisplayedCard ;
	public int nextDisplayedCharacter ;
	public int currentCharacter = -1;
}


