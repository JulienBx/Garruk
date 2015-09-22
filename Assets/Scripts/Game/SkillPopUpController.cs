using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SkillPopUpController : MonoBehaviour
{	
	bool isHovered = false ;
	int timeDoDisplay = 2 ;
	
	public void Awake(){
		//gameObject.transform.FindChild("Receiver0").GetComponent
	}
	
	public void setTimeToDisplay(int a){
		this.timeDoDisplay = a 	;
	}
	
	public void getTimeToDisplay(int a){
		this.timeDoDisplay = a 	;
	}
}


