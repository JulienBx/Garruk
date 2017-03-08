using System;
using UnityEngine;
using TMPro;

public class MessageController : MonoBehaviour{
	void Awake(){
		
	}

	public void display(bool b){
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = b;
		gameObject.transform.FindChild("TitleText").GetComponent<MeshRenderer>().enabled = b;
	}

	public void setTexts(string s1, string s2){
		this.setTitle(s1);
		this.setDescription(s2);
	}

	public void setTitle(string s){
		gameObject.transform.FindChild("TitleText").GetComponent<TextMeshPro>().text = s;
	}

	public void setDescription(string s){
		gameObject.transform.FindChild("DescriptionText").GetComponent<TextMeshPro>().text = s;
	}
}