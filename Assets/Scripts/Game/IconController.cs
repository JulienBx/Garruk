using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class IconController : GameObjectController
{
	public int id ; 
	
	string title;
	string description;
	string additionnalInfo;
	
	bool isActive = false;
	
	void Awake()
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		for (int i = 1; i < renderers.Length; i++)
		{
			renderers [i].GetComponent<Renderer>().enabled = false;
		}
	}
	
	public void setInformation(string t, string d, string i){
		this.title = t ;
		this.description = d ; 
		this.additionnalInfo = i ; 
		this.isActive = true ;
		gameObject.transform.FindChild("Focus").FindChild("TitleI"+this.id).GetComponent<TextMeshPro>().text = t ;
		gameObject.transform.FindChild("Focus").FindChild("DescriptionI"+this.id).GetComponent<TextMeshPro>().text = d ;
		gameObject.transform.FindChild("Focus").FindChild("AdditionnalInfoI"+this.id).GetComponent<TextMeshPro>().text = i ;
	}
	
	public void resetInformation(){
		this.isActive = false ;
	}
	
	public void OnMouseEnter(){
		if(this.isActive){
			Renderer[] renderers = GetComponentsInChildren<Renderer>();
			for (int i = 1; i < renderers.Length; i++)
			{
				renderers [i].GetComponent<Renderer>().enabled = true;
			}
		}
	}
	
	public void OnMouseExit(){
		if(this.isActive){
			Renderer[] renderers = GetComponentsInChildren<Renderer>();
			for (int i = 1; i < renderers.Length; i++)
			{
				renderers [i].GetComponent<Renderer>().enabled = false;
			}
		}
	}
	
}

