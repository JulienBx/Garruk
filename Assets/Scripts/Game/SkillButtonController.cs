using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SkillButtonController : MonoBehaviour
{
	public Skill skill ;
	public bool isLaunched = false ;
	public bool isLaunchable = false ;
	public string launchabilityText ;
	public int id ; 
	
	void Awake(){
		this.showDescription(false);
		this.skill = new Skill();
	}

	public void setDescription(string s){
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = s ;
	}

	public void setSkill(Skill s){
		this.skill = s  ;
		gameObject.GetComponent<SpriteRenderer>().sprite = GameView.instance.getSkillSprite(s.Id);
		gameObject.transform.FindChild("SkillTextZone").FindChild("Description").GetComponent<TextMeshPro>().text = s.Name;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().text = s.Name;
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = GameView.instance.getCurrentCard().getSkillText(s.Description)+"\n\nProbabilité de succès : "+s.proba+"%";
	}
	
	public void getLaunchability(){
		this.launchabilityText = GameSkills.instance.getSkill(this.skill.Id).isLaunchable() ;
		if(this.launchabilityText.Length>1){
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("SkillTextZone").FindChild("Description").GetComponent<TextMeshPro>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = GameView.instance.getCurrentCard().getSkillText(this.skill.Description)+"\n\n"+this.launchabilityText;
		}
		else{
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			gameObject.transform.FindChild("SkillTextZone").FindChild("Description").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = GameView.instance.getCurrentCard().getSkillText(this.skill.Description);
		}
	}

	public void setLaunchability(string s){
		this.launchabilityText = s ;
		if(this.launchabilityText.Length>1){
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			gameObject.transform.FindChild("SkillTextZone").FindChild("Description").GetComponent<TextMeshPro>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = GameView.instance.getCurrentCard().getSkillText(this.skill.Description)+"\n\n"+this.launchabilityText;
		}
		else{
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			gameObject.transform.FindChild("SkillTextZone").FindChild("Description").GetComponent<TextMeshPro>().color = new Color(1f, 1f, 1f, 1f) ;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = GameView.instance.getCurrentCard().getSkillText(this.skill.Description);
		}
	}
	
	public void show(bool b){
		gameObject.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("SkillTextZone").FindChild("Description").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.GetComponent<BoxCollider>().enabled = b;
	}
	
	public void showDescription(bool b){
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = b ;
	}
	
	public void OnMouseEnter(){
		if(GameView.instance.isMobile){

		}
		else{
			if (this.launchabilityText.Length<2){
				gameObject.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			}
			GameView.instance.runningSkill = this.skill.Id ;
			if(!GameSkills.instance.getCurrentGameSkill().auto){
				GameSkills.instance.getSkill(this.skill.Id).launch();
			}
			this.showDescription(true);
		}
	}
	
	public void OnMouseExit(){
		if(GameView.instance.isMobile){
			this.showDescription(false);
		}
		else{
			if (this.launchabilityText.Length<2){
				gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
			}
			if(GameView.instance.isMobile){
				if(!GameView.instance.getSkillZoneController().isRunningSkill){
					GameView.instance.hideTargets();
					this.showDescription(false);
				}
			}
			else{
				if(!GameView.instance.getSkillZoneController().isRunningSkill){
					GameView.instance.hideTargets();
				}
				this.showDescription(false);
			}
		}
	}
	
	public void OnMouseDown(){
		if (this.launchabilityText.Length<2){
			if(!GameView.instance.isMobile){
				this.showDescription(false);
			}
			else{
				GameView.instance.runningSkill = this.skill.Id ;
				print("Je red "+this.id);
				gameObject.GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
				GameView.instance.clickSkillButton(this.id);
			}
			GameView.instance.getSkillZoneController().isRunningSkill = true ;
			GameView.instance.getSkillZoneController().updateButtonStatus(GameView.instance.getCurrentCard());
			GameSkills.instance.getSkill(this.skill.Id).launch();
		}
		if(ApplicationModel.player.ToLaunchGameTutorial){
			GameView.instance.hideTuto();
		}
	}

	public void OnMouseUp(){
		if(GameView.instance.draggingSkillButton==this.id){
			Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int x=-1, y=-1 ;
			if(GameView.instance.getIsFirstPlayer()){
				x = Mathf.FloorToInt(vec.x/GameView.instance.tileScale)+3;
				y = Mathf.FloorToInt(vec.y/GameView.instance.tileScale)+4;
			}
			else{
				x = (GameView.instance.boardWidth-1)-x;
				y = (GameView.instance.boardHeight-1)-y;
			}

			if(x>=0 && x<GameView.instance.boardWidth && y>=0 && y<GameView.instance.boardHeight){
				if(GameView.instance.getTileController(new Tile(x,y)).isDisplayingTarget){
					if(GameView.instance.getTileController(new Tile(x,y)).getCharacterID()!=-1){
						GameView.instance.hitTarget(GameView.instance.getTileController(new Tile(x,y)).getCharacterID());
						GameView.instance.dropSkillButton(this.id);
					}
					else{
						GameView.instance.hitTarget(new Tile(x,y));
					}
				}
				else{
					GameView.instance.dropSkillButton(this.id);
					GameView.instance.getSkillZoneController().getSkillButtonController(this.id).showCollider(true);
				}
			}
			else{
				GameView.instance.dropSkillButton(this.id);
				GameView.instance.getSkillZoneController().getSkillButtonController(this.id).showCollider(true);
			}
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f) ;
		}
	}

	public void setPosition2(Vector3 p){
		p.z = -0.5f;
		p.x -= GameView.instance.stepButton;
		gameObject.transform.localPosition = new Vector3(p.x, p.y, p.z);
	}

	public void shiftRight(){
		Vector3 q;
		q.x = 1f;
		q.y = 0.9f;
		q.z = 0;
		gameObject.transform.FindChild("DescriptionZone").localPosition = q;
	}

	public void shiftLeft(){
		Vector3 q;
		q.x = -1f;
		q.y = 0.9f;
		q.z = 0;
		gameObject.transform.FindChild("DescriptionZone").localPosition = q;
	}

	public void shiftCenter(){
		Vector3 q;
		q.x = 0f;
		q.y = 0.9f;
		q.z = 0;
		gameObject.transform.FindChild("DescriptionZone").localPosition = q;
	}

	public void showCollider(bool b){
		gameObject.GetComponent<BoxCollider>().enabled = b;
		gameObject.transform.FindChild("SkillTextZone").FindChild("Description").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void setRed(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
			
	}

	public void setWhite(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f) ;
	}

	public void setBlue(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
	}
}


