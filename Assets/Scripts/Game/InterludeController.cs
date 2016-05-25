using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class InterludeController : MonoBehaviour
{	
	public float realwidth ;
	public float animationTime = 0.4f;
	public float time;
	public bool isRunning;
	public Sprite[] sprites ;
	bool isEndTurn ;
	
	bool isPaused ;
		
	void Awake(){
		this.isRunning = false ;
		gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().enabled = false ;
		gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().enabled = false ;
		gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().enabled = false ;
		gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().enabled = false ;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = false ;
		this.isPaused = false ;
	}
	
	public void resize(float realwidth){
		this.realwidth = realwidth ;
	}
	
	public void OnMouseDown(){
		if(this.isPaused){
			this.unPause();
		}
		else{
			if(this.time>1*this.animationTime){
				this.time=5*this.animationTime;
			}
		}
	}
	
	public void pause(){
		this.isPaused = true;
	}
	
	public void unPause(){
		this.isPaused = false;
		GameView.instance.blockFury = true ;
	}
	
	public void set(string s, int type){
		this.isEndTurn = (type==3);
		GameView.instance.myTimer.show(false);
		GameView.instance.hisTimer.show(false);
		gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().enabled = true ;
		Vector3 position ;
		position = gameObject.transform.FindChild("Bar1").localPosition ;
		position.x = realwidth/2f+10f;
		gameObject.transform.FindChild("Bar1").localPosition = position;
		position = gameObject.transform.FindChild("Bar2").localPosition ;
		position.x = -realwidth/2f-10f;
		gameObject.transform.FindChild("Bar2").localPosition = position;
		position = gameObject.transform.FindChild("Bar3").localPosition ;
		position.x = realwidth/2f+10f;
		gameObject.transform.FindChild("Bar3").localPosition = position;
		if(type==1){
			SoundController.instance.playSound(39);
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().sprite = this.sprites[0];
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().sprite = this.sprites[1];
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().sprite = this.sprites[2];
		}
		else if(type==2){
			SoundController.instance.playSound(40);
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().sprite = this.sprites[3];
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().sprite = this.sprites[4];
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().sprite = this.sprites[5];
		}
		else if(type==3){
			SoundController.instance.playSound(38);
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().sprite = this.sprites[6];
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().sprite = this.sprites[7];
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().sprite = this.sprites[8];
		}

		gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().enabled = true;
		gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().enabled = true;
		gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().enabled = true ;
		
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = s ;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = true ;
			
		this.time=0f;
		this.isRunning = true ;
	}
	
	public void addTime(float f){
		if(!isPaused){
			this.time += f ;
		}
		bool hasAutoPlayed = false ;
		
		if(this.time>4f*this.animationTime){
			if(!isEndTurn){
				if(GameView.instance.hasFightStarted){
					GameView.instance.changePlayer(-1);
				}
				else{
					GameView.instance.hasFightStarted = true ;
					if(ApplicationModel.player.ToLaunchGameTutorial){
						if(GameView.instance.sequenceID==5){
							GameView.instance.hitNextTutorial();
						}
					}
				}

				GameView.instance.recalculateDestinations();
				GameView.instance.removeDestinations();
				if(!GameView.instance.getCurrentCard().hasMoved){
					GameView.instance.displayDestinations (GameView.instance.getCurrentPlayingCard());
				}
			
				if(ApplicationModel.player.ToLaunchGameTutorial){
					if (GameView.instance.sequenceID!=14){
						if(!GameView.instance.getCurrentCard().isMine){
							StartCoroutine(GameView.instance.launchIABourrin2());
						}
					}
				}
				if(GameView.instance.getCurrentCard().isMine){
					if(GameView.instance.getCurrentCard().isFurious()){
						StartCoroutine(GameView.instance.launchFury());
						hasAutoPlayed = true ;
					}
					else if(GameView.instance.getCurrentCard().isTourelle()){
						StartCoroutine(GameView.instance.launchTourelle());
						hasAutoPlayed = true ;
					}
				}
				else{
					if(GameView.instance.getCurrentCard().isFurious() && ApplicationModel.player.ToLaunchGameIA){
						StartCoroutine(GameView.instance.launchFury());
						hasAutoPlayed = true ;
					}
				}

				GameView.instance.runningSkill = -1;

				if(GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).isMine){
					GameView.instance.SB.GetComponent<StartButtonController>().showText(false);
					GameView.instance.updateActionStatus();
				}
				else{
					GameView.instance.skillZone.GetComponent<SkillZoneController>().showCancelButton(false);
					GameView.instance.skillZone.GetComponent<SkillZoneController>().showSkillButtons(false);
					GameView.instance.getPassZoneController().show(false);
					GameView.instance.SB.GetComponent<StartButtonController>().setText("En attente du joueur adverse");
					GameView.instance.SB.GetComponent<StartButtonController>().showText(true);
				}

				if(GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).isNinja()){
					GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), "Ninja!", 1);
					SoundController.instance.playSound(36);
					GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
					if(GameView.instance.getCurrentCard().isMine || ApplicationModel.player.ToLaunchGameIA){
						List<int> opponents = GameView.instance.getOpponents(GameView.instance.getCurrentCard().isMine);
						List<int> nbHits = new List<int>();
						for (int i = 0 ; i < opponents.Count ; i++){
							nbHits.Add(0);
						}
						int nbShurikens = UnityEngine.Random.Range(1,3);
						for (int i = 1 ; i < nbShurikens+1 ;i++){
							int chosenOne = UnityEngine.Random.Range(0,opponents.Count);
							nbHits[chosenOne]++;
						}
						for (int i = 0 ; i < nbHits.Count ; i++){
							if(nbHits[i]>0){
								if(UnityEngine.Random.Range(1,101)<=GameView.instance.getCard(opponents[i]).getMagicalEsquive()){
									GameController.instance.sendEsquiveShuriken(opponents[i], GameView.instance.getCurrentPlayingCard());
								}
								else{
									GameController.instance.sendShuriken(opponents[i], nbHits[i], GameView.instance.getCurrentPlayingCard());
								}
							}
						}
					}
				}
				GameView.instance.isChangingTurn = false ;
			}
			else{
				GameView.instance.hideEndTurnPopUp();
			}

			bool b = GameView.instance.getCurrentCard().isMine;
			int u = -1 ;
			List<int> units = GameView.instance.getEveryone();
			for(int i = 0 ; i < units.Count ; i++){
				if(GameView.instance.getCard(units[i]).isMine!=b){
					if(GameView.instance.getCard(units[i]).isManipulator()){
						u = units[i];
					}
				}
			}

			if(b){
				if(u==-1){
					GameView.instance.myTimer.setTime();
				}
				else{
					GameView.instance.myTimer.setTime(25f-GameView.instance.getCard(u).getSkills()[0].Power);
				}
				if(!ApplicationModel.player.ToLaunchGameTutorial){
					GameView.instance.myTimer.show(true);
				}
			}
			else{
				if(u==-1){
					GameView.instance.hisTimer.setTime();
				}
				else{
					GameView.instance.hisTimer.setTime(25f-GameView.instance.getCard(u).getSkills()[0].Power);
				}
				if(!ApplicationModel.player.ToLaunchGameTutorial){
					GameView.instance.hisTimer.show(true);
				}
			}

			GameView.instance.isFreezed = false ;
			this.isRunning = false ;
			GameView.instance.hideSkillEffects();
			gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = false ;
			print("Je quitte"+Time.realtimeSinceStartup);
			if(!isEndTurn){
				if(ApplicationModel.player.ToLaunchGameIA && !GameView.instance.getCurrentCard().isMine && !hasAutoPlayed){
					GameView.instance.IA.play();
				}
			}

			if (GameView.instance.sequenceID==14){
				GameView.instance.hitNextTutorial();
			}
			if(GameView.instance.sequenceID==16){
				GameView.instance.hitNextTutorial();
			}
			if(this.isEndTurn && GameView.instance.sequenceID==20){
				GameView.instance.hitNextTutorial();
			}
		}
		else if(this.time>3f*this.animationTime){
			Vector3 position ;
			float rapport = Mathf.Min(1,(this.time-3f*this.animationTime)/this.animationTime);
			position = gameObject.transform.FindChild("Bar1").localPosition ;
			position.x = (realwidth)*rapport;
			gameObject.transform.FindChild("Bar1").localPosition = position;
			position = gameObject.transform.FindChild("Bar2").localPosition ;
			position.x = -(realwidth)*rapport;
			gameObject.transform.FindChild("Bar2").localPosition = position;
			position = gameObject.transform.FindChild("Bar3").localPosition ;
			position.x = (realwidth)*rapport;
			gameObject.transform.FindChild("Bar3").localPosition = position;
			gameObject.transform.FindChild("Text").localScale=new Vector3(1-rapport, 1-rapport, 1-rapport);
		}
		else if(this.time<this.animationTime){
			Vector3 position ;
			float rapport = Mathf.Min(1,(this.time)/this.animationTime);
			position = gameObject.transform.FindChild("Bar1").localPosition ;
			position.x = realwidth-(realwidth)*rapport;
			gameObject.transform.FindChild("Bar1").localPosition = position;
			position = gameObject.transform.FindChild("Bar2").localPosition ;
			position.x = -realwidth+(realwidth)*rapport;
			gameObject.transform.FindChild("Bar2").localPosition = position;
			position = gameObject.transform.FindChild("Bar3").localPosition ;
			position.x = realwidth-(realwidth)*rapport;
			gameObject.transform.FindChild("Bar3").localPosition = position;
			gameObject.transform.FindChild("Text").localScale=new Vector3(rapport, rapport, rapport);
		}
	}	
	
	public bool getIsRunning(){
		return this.isRunning;
	}
}