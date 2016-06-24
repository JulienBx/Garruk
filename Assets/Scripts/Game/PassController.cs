using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class PassController : MonoBehaviour
{
	public string launchabilityText ;
	public IList<string[]> texts ;


	void Awake(){
		this.show(false);
		this.showDescription(false);

		texts = new List<string[]>();
		texts.Add(new string[]{"Terminer","End my turn"});
		texts.Add(new string[]{"Achever le tour de l'unité et donner la main à l'unité suivante dans la timeline","End my unit's turn. Next unit in the timeline plays"});
		texts.Add(new string[]{"L'unité est furieuse : ne peut plus être controllée","Unit is furious and can not be controlled anymore"});

		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().text = this.getText(0);
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.getText(1);
	}

	public void show(bool b){
		gameObject.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.GetComponent<BoxCollider>().enabled = b;
		if(!b){
			this.showDescription(false);
		}
	}

	public virtual string getText(int id){
		return this.texts[id][ApplicationModel.player.IdLanguage];
	}

	public virtual string getText(int id, List<int> args){
		string text = this.texts[id][ApplicationModel.player.IdLanguage];
		for(int i = 0 ; i < args.Count ; i++){
			text = text.Replace("ARG"+(i+1), ""+args[i]);
		}

		return text ;
	}
	
	public void updateButtonStatus(GameCard g){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = this.getText(0);
		if(GameView.instance.sequenceID<18 && ApplicationModel.player.ToLaunchGameTutorial){
			this.show(false);
		}
		else if(GameView.instance.getCurrentPlayingCard()!=-1){
			this.show (!GameView.instance.getCurrentCard().hasPlayed || !GameView.instance.getCurrentCard().hasMoved);
		}
		else{
			this.show (true);
		}

	}

	public void getLaunchability(){
		this.launchabilityText = "" ;
		if(GameView.instance.getCurrentCard().isFurious()){
			this.launchabilityText = this.getText(2) ;
		}
		if((ApplicationModel.player.ToLaunchGameTutorial && GameView.instance.sequenceID<18)||this.launchabilityText.Length>1){
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.getText(1)+"\n\n"+this.launchabilityText;
		}
		else{
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.getText(1);
		}
	}

	public void setLaunchability(string s){
		this.launchabilityText = s ;
		if(this.launchabilityText.Length>1){
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.getText(1)+"\n\n"+this.launchabilityText;
		}
		else{
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.getText(1);
		}
	}
	
	public void OnMouseEnter(){
		if(GameView.instance.isMobile){

		}
		else{
			if (this.launchabilityText.Length<2){
				gameObject.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			}
			this.showDescription(true);
		}
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
		}
		this.showDescription(false);
	}
	
	public void OnMouseDown(){
		if (this.launchabilityText.Length<2){
			GameView.instance.hideAllTargets();
			GameView.instance.removeDestinations();
			//GameView.instance
			GameController.instance.findNextPlayer(false);
		}
		if(ApplicationModel.player.ToLaunchGameTutorial){
			
		}
	}
}


