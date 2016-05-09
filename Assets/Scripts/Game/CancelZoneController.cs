using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CancelZoneController : MonoBehaviour
{
	public void OnMouseEnter(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
	}
	
	public void OnMouseExit(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
	}

	public void OnMouseDown(){
		if(ApplicationModel.player.ToLaunchGameTutorial){
			GameView.instance.hideTuto();
		}
		GameView.instance.hideValidationButton();
		GameView.instance.hideTargets();
		if(GameView.instance.hoveringZone!=-1){
			GameView.instance.hideAllTargets();
		}
		SoundController.instance.playSound(20);
		GameView.instance.cancelSkill();
	}

	public void show(bool b){
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.GetComponent<BoxCollider>().enabled = b ;
	}
}

