using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SkillValidationController : MonoBehaviour
{
	public void setTexts(string t, string d, string b){
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text = t;
		gameObject.transform.FindChild("Description").GetComponent<TextMeshPro>().text = d;
		gameObject.transform.FindChild("Button").FindChild("Text").GetComponent<TextMeshPro>().text = b;
	}

	public void OnMouseEnter(){
		gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		gameObject.transform.FindChild("Button").FindChild("Text").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
	}

	public void OnMouseExit(){
		gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().color = Color.white;
		gameObject.transform.FindChild("Button").FindChild("Text").GetComponent<TextMeshPro>().color = Color.white;
	}

	public void OnMouseDown(){
		if(GameView.instance.isDisplayedPopUp){
			GameView.instance.hideValidationButton();
		}
		GameSkills.instance.getCurrentGameSkill().resolve(new List<Tile>());
	}
	
	public void show(bool b){
		gameObject.GetComponent<SpriteRenderer>().enabled=b;
		gameObject.GetComponent<BoxCollider>().enabled=b;
		gameObject.transform.FindChild("Title").GetComponent<MeshRenderer>().enabled=b;
		gameObject.transform.FindChild("Description").GetComponent<MeshRenderer>().enabled=b;
		gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().enabled=b;
		gameObject.transform.FindChild("Button").FindChild("Text").GetComponent<MeshRenderer>().enabled=b;
		gameObject.GetComponent<BoxCollider>().enabled=b;
	}
}


