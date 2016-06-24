using UnityEngine;
using System.Collections.Generic;
using TMPro ;

public class TileController : GameObjectController
{
	public Sprite[] trapSprites ;
	public Sprite[] destinationSprites ;
	public Sprite[] targetSprites ;
	public Color[] skillEffectColor;
	public Sprite[] animSprites;
	
	Tile tile ;
	int type ;
	int characterID = -1;
	public Trap trap ;
	bool isTrapped ;
	int isDestination = -1;
	
	public bool isDisplayingTarget;
	float timerTarget;
	float targetTime = 0.8f;
	bool isTargetDisplayed;
	
	string skillEffectDescription ;
	bool isDisplayingSkillEffect;
	bool isShowingSE ;
	float timerSE;
	float timerAnim = 0 ;
	float animTime = 0.08f ;
	float skillEffectTime = 0.6f ;
	int animIndex;
	int basicAnimIndex ;

	bool isHovering = false ;
	public bool isFinishedTransi = false ; 
	bool isGrowing = false ;

	public IList<string[]> texts ;
	bool isDisplayingDescription ;

	void Awake()
	{
		this.showTrap(false);
		this.showDestination(false);
		this.showDescription(false);
		gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = false ;
		gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().enabled = false ;
		this.isDisplayingTarget = false ;
		this.showEffect(false);
		this.displayAnim(false);
		this.isDisplayingDescription = false ;

		texts = new List<string[]>();
		texts.Add(new string[]{"Créateur\nCrée un cristal","Creator\nMade a cristal"});
		texts.Add(new string[]{"Cristal créé!","Cristal created!"});
		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
		texts.Add(new string[]{"Poison","Poison"});
		texts.Add(new string[]{"-ARG1 PV en fin de tour","-ARG1 HP per turn"});
		texts.Add(new string[]{"Téléporté!","Teleported!"});
		texts.Add(new string[]{"Cristal","Cristal"});
		texts.Add(new string[]{"Cette case ne peut être franchie","You can not move on this tile"});
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

	public void displayTarget(bool b){
		this.isDisplayingTarget = b ;
		this.isTargetDisplayed = b ;
		gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sprite = this.targetSprites[0] ;
		gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().enabled = b ;
		this.timerTarget=0f;
	}
	
	public void addTargetTime(float f){
		this.timerTarget += f ;
		if (this.timerTarget>this.targetTime){
			this.isTargetDisplayed = !this.isTargetDisplayed ;
			if(!this.isHovering){
				if(this.isTargetDisplayed){
					gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sprite = this.targetSprites[0] ;
				}
				else{
					gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sprite = this.targetSprites[1] ;
				}
			}
			this.timerTarget = 0f ;
		}
	}
	
	public void setTargetText(string t, string d){
		gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = t;
		gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = d;
	}
	
	public void showTarget(bool b){
		if(GameView.instance.hoveringZone<1){
			gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().enabled = b ;
		}
		this.isDisplayingTarget = b ;
	}
	
	public void initTileController(Tile t, int ty){
		this.tile = t ;
		this.type = ty ;
		if (this.type==1){
			this.showRock();
		}
		this.showDestination(false);
		gameObject.name = "Tile " + (t.x) + "-" + (t.y);
		this.resize();
	}
	
	public void resize(){
		Vector3 position ;
		if (GameView.instance.getIsFirstPlayer()){
			position = new Vector3((-2.5f+this.tile.x)*(GameView.instance.tileScale), (-3.5f+this.tile.y)*(GameView.instance.tileScale), 0);
		}
		else{
			position = new Vector3((2.5f-this.tile.x)*(GameView.instance.tileScale), (3.5f-this.tile.y)*(GameView.instance.tileScale), 0);
		}
		
		Vector3 scale = new Vector3(GameView.instance.tileScale, GameView.instance.tileScale, GameView.instance.tileScale);

		gameObject.transform.position = position ;
		gameObject.transform.localScale = scale ;
	}
	
	public void setTrap(Trap t){
		this.trap = t ;
		isTrapped = true ;
		this.showTrap (this.trap.getIsVisible());
		SoundController.instance.playSound(41);
	}
	
	public int getIsDestination(){
		return this.isDestination;
	}
	
	public void setDestination(int i){
		this.isDestination = i ;
		this.showDestination (true);
	}
	
	public void removeDestination(){
		this.isDestination = -1 ;
		this.showDestination (false);
	}
	
	public bool canBeDestination(){
		return (type!=1 && characterID==-1);
	}

	public void removeRock(){
		this.type=0;
		this.showRock();
		GameView.instance.updateCristoEater();
	}

	public void addRock(int type){
		this.type=1;
		this.showRock();

		if(type==140){
			GameView.instance.displaySkillEffect(this.tile, this.getText(0), 2);
			GameView.instance.addAnim(0,this.tile);
			SoundController.instance.playSound(28);
		}
		else if(type==42){
			GameView.instance.displaySkillEffect(this.tile, this.getText(1), 2);
			GameView.instance.addAnim(0,this.tile);
			SoundController.instance.playSound(28);
		}

		GameView.instance.updateCristoEater();
	}

	public bool isRock(){
		return (this.type==1 || this.type==2);
	}
	
	public bool getIsTrapped(){
		return (this.isTrapped);
	}
	
	public Vector3 getPosition()
	{
		return gameObject.transform.position;
	}
	
	public int getTileType()
	{
		return this.type;
	}
	
	public void changeType(int a)
	{
		this.type = a;
		if (this.type==1 || this.type==2){
			this.showRock();
		}
	}
	
	public Tile getTile()
	{
		return this.tile;
	}
	
	public void setCharacterID(int i){
		this.characterID = i ;
		gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = false ;
	}
	
	public int getCharacterID(){
		return this.characterID ;
	}
	
	public bool checkTrap()
	{
		bool isSuccess = false ;
		if(this.isTrapped){
			if(this.trap.getType()==1){
				GameView.instance.getPlayingCardController(this.characterID).addDamagesModifyer(new Modifyer(this.trap.getAmount(), -1, 0, "", ""), true,-1);
				GameView.instance.displaySkillEffect(this.characterID, "Trap!\n"+this.getText(2, new List<int>{this.trap.getAmount()}), 0);
				GameView.instance.addAnim(5,GameView.instance.getTile(this.characterID));
			}
			else if(this.trap.getType()==2){	
				GameView.instance.getCard(this.characterID).setPoison(new Modifyer(this.trap.getAmount(), -1, 58, this.getText(3), this.getText(4, new List<int>{this.trap.getAmount()})));
				GameView.instance.getPlayingCardController(this.characterID).showIcons();
				GameView.instance.displaySkillEffect(this.characterID, this.getText(3)+"\n"+this.getText(4, new List<int>{this.trap.getAmount()}), 0);	
				GameView.instance.addAnim(4,GameView.instance.getTile(this.characterID));
			}
			else if(this.trap.getType()==3){	
				GameView.instance.displaySkillEffect(this.characterID, this.getText(5), 0);	
				GameView.instance.addAnim(0,this.tile);

				if(GameView.instance.getCard(this.characterID).isMine || (ApplicationModel.player.ToLaunchGameIA && !GameView.instance.getCard(this.characterID).isMine)){
					List<Tile> tiles = GameView.instance.getAllTilesWithin(this.tile, this.trap.getAmount());
					Tile tile = tiles[UnityEngine.Random.Range(0,tiles.Count)];
					GameController.instance.clickDestination(tile, this.characterID, true);
				}
			}
			SoundController.instance.playSound(34);
			this.removeTrap();
			this.showDescription(false);
			isSuccess = true ;
		}
		return isSuccess;
	}
	
	public void removeTrap()
	{
		this.isTrapped = false;
		this.showTrap(false);
	}
	
	public void showTrap(bool b)
	{
		if (this.isTrapped){
			gameObject.transform.FindChild("TrapLayer").GetComponent<SpriteRenderer>().sprite = this.trapSprites[this.trap.getType()];
		}
		gameObject.transform.FindChild("TrapLayer").GetComponent<SpriteRenderer>().enabled = b;
	}
	
	public void showRock()
	{
		if(this.type==0){
			if(!this.isTrapped){
				gameObject.transform.FindChild("TrapLayer").GetComponent<SpriteRenderer>().enabled = false;
			}
		}
		else if(this.type==1){
			gameObject.transform.FindChild("TrapLayer").GetComponent<SpriteRenderer>().sprite = this.trapSprites[0];
			gameObject.transform.FindChild("TrapLayer").GetComponent<SpriteRenderer>().enabled = true;
		}
	}
	
	public void showDestination(bool b)
	{
		
		if (this.isDestination>=0){
			gameObject.transform.FindChild("DestinationLayer").GetComponent<SpriteRenderer>().sprite = this.destinationSprites[isDestination];
			gameObject.transform.FindChild("DestinationLayer").GetComponent<SpriteRenderer>().enabled = true;
		}
		else{
			gameObject.transform.FindChild("DestinationLayer").GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	public void setTargetSprite(int i){
		gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sprite = this.targetSprites[i] ;
		gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().enabled = true;
		if(this.characterID!=-1){
			this.showDescription(true);
		}
	}

	public void OnMouseEnter()
	{
		if(GameView.instance.isEndedGame){

		}
		else if(!ApplicationModel.player.ToLaunchGameTutorial || GameView.instance.sequenceID>5){
			if(GameView.instance.isMobile){
				if(GameView.instance.draggingSkillButton!=-1){
					if(GameView.instance.hoveringZone!=-1){
						if(GameView.instance.hoveringZone==1){
							GameView.instance.hideAllTargets();
							this.setTargetSprite(3) ;
							if(this.tile.x<GameView.instance.boardWidth-1){
								GameView.instance.getTileController(new Tile(this.tile.x+1, this.tile.y)).setTargetSprite(3);
							}
							if(this.tile.x>0){
								GameView.instance.getTileController(new Tile(this.tile.x-1, this.tile.y)).setTargetSprite(3);
							}
							if(this.tile.y<GameView.instance.boardHeight-1){
								GameView.instance.getTileController(new Tile(this.tile.x, this.tile.y+1)).setTargetSprite(3);
							}
							if(this.tile.y>0){
								GameView.instance.getTileController(new Tile(this.tile.x, this.tile.y-1)).setTargetSprite(3);
							}
							GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).setBlue();
						}
						else if(GameView.instance.hoveringZone==2){
							GameView.instance.hideAllTargets();
							Tile currentTile = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard());
							if(this.tile.x == currentTile.x || this.tile.y == currentTile.y){
								if(this.tile.x != currentTile.x || this.tile.y != currentTile.y){
									GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).setBlue();
									if(this.tile.x==currentTile.x){
										if(this.tile.y<currentTile.y){
											for(int i = currentTile.y-1 ; i>=0 ; i--){
												GameView.instance.getTileController(new Tile(this.tile.x, i)).setTargetSprite(3);
											}
										}
										else if(this.tile.y>currentTile.y){
											for(int i = currentTile.y+1 ; i<GameView.instance.boardHeight ; i++){
												GameView.instance.getTileController(new Tile(this.tile.x, i)).setTargetSprite(3);
											}
										}
									}
									else if(this.tile.y==currentTile.y){
										if(this.tile.x<currentTile.x){
											for(int i = currentTile.x-1 ; i>=0 ; i--){
												GameView.instance.getTileController(new Tile(i, this.tile.y)).setTargetSprite(3);
											}
										}
										else if(this.tile.x>currentTile.x){
											for(int i = currentTile.x+1 ; i<GameView.instance.boardWidth ; i++){
												GameView.instance.getTileController(new Tile(i, this.tile.y)).setTargetSprite(3);
											}
										}
									} 
								}
								else{
									GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).setRed();
								}
							}
							else{
								GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).setRed();
							}
						}
					}
					else if(this.isDisplayingTarget){
						SoundController.instance.playSound(32);
						if(this.characterID!=-1){
							GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).setDescription(GameSkills.instance.getCurrentGameSkill().getTargetText(this.characterID));
						}
						else{
							GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).setDescription(GameSkills.instance.getCurrentGameSkill().getTargetText(-1));
						}
						if(GameView.instance.getIsFirstPlayer()){
							if(this.tile.x==0){
								GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).shiftRight2();
							}
							else if(this.tile.x==GameView.instance.boardWidth-1){
								GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).shiftLeft2();
							}
							else{
								GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).shiftCenter2();
							}
						}
						else{
							if(this.tile.x==GameView.instance.boardWidth-1){
								GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).shiftRight2();
							}
							else if(this.tile.x==0){
								GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).shiftLeft2();
							}
							else{
								GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).shiftCenter2();
							}
						}
						GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).setBlue();
						GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).showDescription(true);
					}
					else if(GameView.instance.runningSkill!=-1){
						if(GameSkills.instance.getCurrentGameSkill().auto){
							GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).setBlue();
						}
						else{
							GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).setRed();
						}
						if(GameView.instance.getIsFirstPlayer()){
							if(this.tile.x==0){
								GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).shiftRight2();
							}
							else if(this.tile.x==GameView.instance.boardWidth-1){
								GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).shiftLeft2();
							}
							else{
								GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).shiftCenter2();
							}
						}
						else{
							if(this.tile.x==GameView.instance.boardWidth-1){
								GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).shiftRight2();
							}
							else if(this.tile.x==0){
								GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).shiftLeft2();
							}
							else{
								GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).shiftCenter2();
							}
						}
						GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).setDescription(GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
						GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).showDescription(true);
					}
				}
			}
			else{
				this.isHovering = true ;
				gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = true ;
				if(GameView.instance.hoveringZone!=-1){
					if(GameView.instance.hoveringZone==1){
						GameView.instance.hideAllTargets();
						this.setTargetSprite(3) ;
						if(this.tile.x<GameView.instance.boardWidth-1){
							GameView.instance.getTileController(new Tile(this.tile.x+1, this.tile.y)).setTargetSprite(3);
						}
						if(this.tile.x>0){
							GameView.instance.getTileController(new Tile(this.tile.x-1, this.tile.y)).setTargetSprite(3);
						}
						if(this.tile.y<GameView.instance.boardHeight-1){
							GameView.instance.getTileController(new Tile(this.tile.x, this.tile.y+1)).setTargetSprite(3);
						}
						if(this.tile.y>0){
							GameView.instance.getTileController(new Tile(this.tile.x, this.tile.y-1)).setTargetSprite(3);
						}
					}
					else if(GameView.instance.hoveringZone==2){
						GameView.instance.hideAllTargets();
						Tile currentTile = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard());
						if(this.tile.x == currentTile.x || this.tile.y == currentTile.y){
							if(this.tile.x != currentTile.x || this.tile.y != currentTile.y){
								if(this.tile.x==currentTile.x){
									if(this.tile.y<currentTile.y){
										for(int i = currentTile.y-1 ; i>=0 ; i--){
											GameView.instance.getTileController(new Tile(this.tile.x, i)).setTargetSprite(3);
										}
									}
									else if(this.tile.y>currentTile.y){
										for(int i = currentTile.y+1 ; i<GameView.instance.boardHeight ; i++){
											GameView.instance.getTileController(new Tile(this.tile.x, i)).setTargetSprite(3);
										}
									}
								}
								else if(this.tile.y==currentTile.y){
									if(this.tile.x<currentTile.x){
										for(int i = currentTile.x-1 ; i>=0 ; i--){
											GameView.instance.getTileController(new Tile(i, this.tile.y)).setTargetSprite(3);
										}
									}
									else if(this.tile.x>currentTile.x){
										for(int i = currentTile.x+1 ; i<GameView.instance.boardWidth ; i++){
											GameView.instance.getTileController(new Tile(i, this.tile.y)).setTargetSprite(3);
										}
									}
								} 
							}
						}
					}
				}
				else if(this.isDisplayingTarget){
					SoundController.instance.playSound(32);
						
					gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sprite = this.targetSprites[2] ;
					this.showDescription(true);
				}
				if(this.characterID==-1){
					GameView.instance.hoverTile();
				}
				else{
					if(!GameView.instance.getPlayingCardController(this.characterID).getIsHidden()){
						if(GameView.instance.draggingCard==-1){
							GameView.instance.hoverCharacter(this.characterID);
						}
					}
				}
			}
		}
	}
	
	public void OnMouseDown()
	{
		if(GameView.instance.isEndedGame){

		}
		else{
			if(GameView.instance.hoveringZone!=-1){
				if(GameView.instance.hoveringZone==1){
					GameView.instance.hitTarget(this.tile);
				}
				else if(GameView.instance.hoveringZone==2){
					Tile currentTile = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard());
					if(this.tile.x == currentTile.x || this.tile.y == currentTile.y){
						if(this.tile.x != currentTile.x || this.tile.y != currentTile.y){
							GameView.instance.hitTarget(this.tile);
						}
					}
				}
			}
			else if(GameView.instance.isDisplayedPopUp){
				GameView.instance.hideValidationButton();
				if(GameView.instance.getSkillZoneController().isRunningSkill){
					GameView.instance.hideTargets();
					GameView.instance.getSkillZoneController().isRunningSkill = false ;
					GameView.instance.getSkillZoneController().updateButtonStatus(GameView.instance.getCurrentCard());
				}
			}
			else{
				if(this.isDisplayingTarget){
					this.showDescription(false);
				}
				if(this.characterID!=-1){
					if(GameView.instance.getTileController(this.characterID).isDisplayingTarget){
						GameView.instance.hitTarget(this.tile);
					}
					else if(GameView.instance.getCard(this.characterID).isMine && (!GameView.instance.hasFightStarted ||(GameView.instance.getCurrentPlayingCard()==this.characterID && !GameView.instance.getCard(this.characterID).hasMoved))){
						GameView.instance.clickCharacter(this.characterID);
					}
					else if(GameView.instance.isMobile){
						GameView.instance.clickMobileCharacter(this.characterID);
					}
				}
				else{
					if(this.isDisplayingTarget){
						GameView.instance.hitTarget(this.tile);
					}
					else if(this.isDestination==1 && GameView.instance.hasFightStarted){
						if(ApplicationModel.player.ToLaunchGameTutorial && GameView.instance.sequenceID>5 && GameView.instance.sequenceID<14){
							if(this.tile.x==3 && this.tile.y==5){
								GameController.instance.clickDestination(this.tile, GameView.instance.getCurrentPlayingCard(), true);
								GameView.instance.hitNextTutorial();
							}
						}
						else{
							GameController.instance.clickDestination(this.tile, GameView.instance.getCurrentPlayingCard(), true);
						}
					}
					else if(this.characterID==-1){
						if(GameView.instance.draggingCard==-1){
							if(!this.isDisplayingDescription){
								if(this.isDisplayingTarget){
									if(GameView.instance.hoveringZone==-1){
										gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = GameSkills.instance.getCurrentGameSkill().getText(0);
										gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = GameSkills.instance.getCurrentGameSkill().getTargetText(-1);
										gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = true;
										gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=true;
										gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=true;
									}
								}
								else if(this.type==1){
									gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = this.getText(6);
									gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.getText(7);
									gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = true;
									gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=true;
									gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=true;
								}
								else if (this.isTrapped){
									if(this.trap.getIsVisible()){
										gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<TextMeshPro>().text = this.trap.title;
										gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.trap.description;
										gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = true;
										gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=true;
										gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=true;
									}
								}
								this.isDisplayingDescription = true ;
								GameView.instance.hoverTile();
							}
							else{
								gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = false ;
								gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = false;
								gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=false;
								gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=false;
								this.isDisplayingDescription = false ;
							}
						}
					}
				}
				if(ApplicationModel.player.ToLaunchGameTutorial){
					GameView.instance.hideTuto();
				}
			}
		}
	}

	public void OnMouseUp()
	{
		if(GameView.instance.isEndedGame){

		}
		else{
			if(this.characterID!=-1 && GameView.instance.draggingCard==this.characterID){
				if(GameView.instance.timeDragging>0.2f){
					Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					int x=-1, y=-1 ;
					if(GameView.instance.getIsFirstPlayer()){
						x = Mathf.FloorToInt(vec.x+3);
						y = Mathf.FloorToInt(vec.y+4);
					}
					else{
						x = (GameView.instance.boardWidth-1)-Mathf.FloorToInt(vec.x+3);
						y = (GameView.instance.boardHeight-1)-Mathf.FloorToInt(vec.y+4);
					}
					if(x>=0 && x<GameView.instance.boardWidth && y>=0 && y<GameView.instance.boardHeight){
						Tile t = new Tile(x,y);
						if(!GameView.instance.hasFightStarted && GameView.instance.getTileController(t).getIsDestination()==0){
							GameController.instance.clickDestination(t, this.characterID, false);
						}
						else if(GameView.instance.hasFightStarted && GameView.instance.getTileController(t).getIsDestination()==1){
							if(ApplicationModel.player.ToLaunchGameTutorial){
								if(GameView.instance.sequenceID==10){
									if(x==3 && y==5){
										GameController.instance.clickDestination(t, this.characterID, true);
										GameView.instance.hitNextTutorial();
									}
									else{
										GameView.instance.dropCharacter(this.characterID); 
									}
								}
								else{
									GameController.instance.clickDestination(t, this.characterID, true);
								}
							}
							else{
								GameController.instance.clickDestination(t, this.characterID, true);
							}

						}
						else{
							GameView.instance.dropCharacter(this.characterID); 

						}
					}
					else{
						GameView.instance.dropCharacter(this.characterID);
					}
				}
				else{
					GameView.instance.dropCharacter(this.characterID);
					if(GameView.instance.isMobile){
						GameView.instance.mobileClick(this.characterID);
					}
				}
			}
			else if(GameView.instance.timeDragging>=0){
				GameView.instance.mobileClick(this.characterID);
			}
			GameView.instance.timeDragging = -1f;
		}
	}

	public void OnMouseExit()
	{
		if(GameView.instance.isEndedGame){

		}
		else{
			if(GameView.instance.isMobile){
				
			}
			else{
				this.isHovering = false ;
				if(this.isDisplayingTarget){
					this.showDescription(false);
					if(this.isTargetDisplayed){
						gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sprite = this.targetSprites[0] ;
					}
					else{
						gameObject.transform.FindChild("TargetLayer").GetComponent<SpriteRenderer>().sprite = this.targetSprites[1] ;
					}
				}
				if(this.characterID==-1){
					gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = false ;

				}
				else if(GameView.instance.getPlayingCardController(this.characterID).getIsHidden()){
					gameObject.transform.FindChild("HoverLayer").GetComponent<SpriteRenderer>().enabled = false ;	
				}
				
				if(this.type==1 || this.type==2 || this.isTrapped){
					//this.showDescription(false);
				}
			}
		}
	}
	
	public void showDescription(bool b){
		gameObject.transform.FindChild("DescriptionBox").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("DescriptionBox").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled=b;
		gameObject.transform.FindChild("DescriptionBox").FindChild("TitleText").GetComponent<MeshRenderer>().enabled=b;
	}
	
	public void setSkillEffect(string s, int type){
		gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<TextMeshPro>().text= s;
		gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<TextMeshPro>().color = this.skillEffectColor[type] ;
	}
	
	public void addAnimTime(float t){
		this.timerAnim += t ;
		int newIndex = Mathf.FloorToInt(timerAnim/animTime);
		if(newIndex!=animIndex){
			animIndex = newIndex;
			if(newIndex>9){
				this.timerAnim = 0f ;
				GameView.instance.removeAnim(this.tile);
			}
			else{
				this.changeAnimSprite(basicAnimIndex + animIndex);
			}
		}
	}

	public void addSETime(float t){
		this.timerAnim += t ;
		if(this.isFinishedTransi){
			if(this.characterID!=GameView.instance.getCurrentPlayingCard()){
				if(this.timerAnim < 0.5f){
					if(this.isGrowing){
						gameObject.transform.FindChild("SkillEffect").localScale = new Vector3(0.8f+0.4f*(timerAnim), 0.8f+0.4f*(timerAnim), 0.8f+0.4f*(timerAnim));
					}
					else{
						gameObject.transform.FindChild("SkillEffect").localScale = new Vector3(1f-0.4f*(timerAnim), 1f-0.4f*(timerAnim), 1f-0.4f*(timerAnim));
					}
				}
				else{
					this.timerAnim = 0f ;
					this.isGrowing = !this.isGrowing ;
				}
			}
		}
		else{
			if(timerAnim<4*skillEffectTime){
				gameObject.transform.FindChild("SkillEffect").localScale = new Vector3(0.5f+0.5f*(1.0f*timerAnim/(4*skillEffectTime)), 0.5f+0.5f*(1.0f*timerAnim/(4*skillEffectTime)), 0.5f+0.5f*(1.0f*timerAnim/(4*skillEffectTime)));
				gameObject.transform.FindChild("SkillEffect").localPosition = new Vector3(0, 0.15f*(1.0f*timerAnim/(skillEffectTime)), 0f);
			}
			else{
				
				if(this.characterID!=GameView.instance.getCurrentPlayingCard()){
					this.timerAnim = 0f ;
					this.isFinishedTransi = true ;
					this.isGrowing = false ;
					if(GameView.instance.sequenceID!=12 && GameView.instance.sequenceID!=21){
						GameView.instance.removeSE(this.tile);
					}
				}
				else{
					if(GameView.instance.sequenceID!=12 && GameView.instance.sequenceID!=21){
						GameView.instance.removeSE(this.tile);
					}
				}
			}
		}
	}
	
	public void changeAnimSprite(int index){
		gameObject.transform.FindChild("AnimLayer").GetComponent<SpriteRenderer>().sprite = this.animSprites[index] ;
	}
	
	public void displayAnim(bool b){
		gameObject.transform.FindChild("AnimLayer").GetComponent<SpriteRenderer>().enabled = b ;
		this.timerAnim = 0f;
		this.animIndex = 0;
	}
	
	public void showEffect(bool b){
		gameObject.transform.FindChild("SkillEffect").FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void setAnimIndex(int i){
		basicAnimIndex = i*10 ;
	}
}

