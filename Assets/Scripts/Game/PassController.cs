using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class PassController : MonoBehaviour
{
	public string launchabilityText ;

	void Awake(){
		this.show(false);
		this.showDescription(false);
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().text = "Passer";
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = "Terminer son tour et donner la main à l'adversaire";
	}

	public void show(bool b){
		gameObject.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.GetComponent<BoxCollider>().enabled = b;
		if(!b){
			this.showDescription(false);
		}
	}
	
	public void updateButtonStatus(GameCard g){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = "Passer mon tour";
		if(GameView.instance.getCurrentPlayingCard()!=-1){
			this.show (!GameView.instance.getCurrentCard().hasPlayed || !GameView.instance.getCurrentCard().hasMoved);
		}
		else{
			this.show (true);
		}
	}

	public void getLaunchability(){
		this.launchabilityText = "" ;
		if(GameView.instance.getCurrentCard().isFurious()){
			this.launchabilityText = "Le personnage est furieux et ne plus être controlé" ;
		}
		if(this.launchabilityText.Length>1){
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = "Passer le tour de l'unité active"+"\n\n"+this.launchabilityText;
		}
		else{
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = "Passer le tour de l'unité active";
		}
	}

	public void setLaunchability(string s){
		this.launchabilityText = s ;
		if(this.launchabilityText.Length>1){
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = "Passer le tour de l'unité active"+"\n\n"+this.launchabilityText;
		}
		else{
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = "Passer le tour de l'unité active";
		}
	}
	
	public void OnMouseEnter(){
		if (this.launchabilityText.Length<2){
			gameObject.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		}
		this.showDescription(true);
	}

	public void showDescription(bool b){
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = b ;
	}
	
	public void OnMouseExit(){
		if (this.launchabilityText.Length<2){
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
		}
		if(!GameView.instance.getSkillZoneController().isRunningSkill){
			GameView.instance.hideTargets();
			this.showDescription(false);
		}
	}
	
	public void OnMouseDown(){
		if (this.launchabilityText.Length<2){
			GameController.instance.findNextPlayer();
		}
		if(ApplicationModel.player.ToLaunchGameTutorial){
			
		}
	}
}


