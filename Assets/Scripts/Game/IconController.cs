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
	bool isHovered = false ;
	
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
		
		
	}
	
	public void resetInformation(){
		this.isActive = false ;
	}
	
	public void OnMouseEnter(){
		if(this.isActive){
			if (!this.isHovered){	
				Vector3 position = gameObject.transform.position;
				position.y += 0.45f;
				GameView.instance.displayPopUp(this.description, position, this.title);
				this.isHovered = true ;
			}
		}
	}
	
	public void OnMouseExit(){
		if(this.isActive){
			if (this.isHovered){	
				GameView.instance.hidePopUp();
				this.isHovered = false ;
			}
		}
	}
	
}

