using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class ButtonController : MonoBehaviour
{	
	void Awake(){
		this.hide ();
	} 
	
	public void hide(){
		gameObject.GetComponent<SpriteRenderer>().enabled = false ; 
	}
}


