using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class StartButtonController : MonoBehaviour
{	
	bool isPushed = false ;
	
	public void OnMouseEnter(){
		if (!isPushed){
			gameObject.GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f, 1f);
		}
	}
	
	public void OnMouseExit(){
		if (!isPushed){
			gameObject.GetComponent<TextMeshPro>().color=new Color(1f, 1f, 1f, 1f);
		}
	}
	
	public void OnMouseDown(){
		if(this.isPushed==false){
			this.isPushed = true ;
			gameObject.GetComponent<TextMeshPro>().text="En attente du joueur 2";
			gameObject.GetComponent<TextMeshPro>().color=new Color(1f,1f,1f, 1f);
			GameController.instance.playerReady();
			if(GameView.instance.getIsTutorialLaunched())
			{
				print (TutorialObjectController.instance.getSequenceID());
				TutorialObjectController.instance.actionIsDone();
			}
		}
	}
}


