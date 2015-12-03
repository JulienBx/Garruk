using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class PopUpGameController : MonoBehaviour
{
	
	public void setTexts(string t, string d){
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text = t;
		gameObject.transform.FindChild("Description").GetComponent<TextMeshPro>().text = t;
	}
	
	public void OnMouseDown(){
		this.show (false);
	}
	
	public void show(bool b){
		gameObject.GetComponent<SpriteRenderer>().enabled=b;
		gameObject.transform.FindChild("Title").GetComponent<MeshRenderer>().enabled=b;
		gameObject.transform.FindChild("Description").GetComponent<MeshRenderer>().enabled=b;
	}
	
	public void changePosition(Vector3 position){
		gameObject.transform.localPosition = position;
	}
}


