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
		gameObject.GetComponent<BoxCollider>().enabled=false;
	}
	
	public void setInformation(string t, string d, string i){
		this.title = t ;
		this.description = d ; 
		this.additionnalInfo = i ; 
		this.isActive = true ;
		gameObject.GetComponent<BoxCollider>().enabled=true;
	}
	
	public void resetInformation(){
		gameObject.GetComponent<BoxCollider>().enabled=false;
		this.isActive = false ;
	}
	
	public void OnMouseEnter(){
		if(this.isActive){
			if (!this.isHovered){	
				Vector3 position = gameObject.transform.position;
				position.x -= 2.2f;
				GameView.instance.displayPopUp(this.description, position, this.title);
				this.isHovered = true ;
			}
		}
	}
	
	public void OnMouseDown(){
//		if(!this.isActive){
//			GameController.instance.clickPlayingCard(this.id, this.tile);
//			if(GameView.instance.getIsTutorialLaunched())
//			{
//				TutorialObjectController.instance.actionIsDone();
//			}
//		}
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

