using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class IconController : GameObjectController
{
	public int cardId ; 
	public int iconId ;
	
	string title;
	string description;
	
	void Awake()
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		for (int i = 1; i < renderers.Length; i++)
		{
			renderers [i].GetComponent<Renderer>().enabled = false;
		}
		gameObject.GetComponent<BoxCollider>().enabled=false;
	}
	
	public void setInformation(string t, string d){
		this.title = t ;
		this.description = d ; 
		gameObject.GetComponent<BoxCollider>().enabled=true;
		gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = this.title;
		gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.description;
	}
	
	public void resetInformation(){
		gameObject.GetComponent<BoxCollider>().enabled=false;
	}
	
	public void OnMouseEnter(){
		gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = true;
		gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled = true;
		gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = true;
		
	}
	
	public void OnMouseExit(){
		gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = false;
		gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled = false;
		gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = false;
	}
}

