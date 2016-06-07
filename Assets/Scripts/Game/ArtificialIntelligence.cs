using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class ArtificialIntelligence : MonoBehaviour
{
	int aggressiveFactor ;
	int trapFactor ;
	int soutienFactor ;
	List<Tile> emplacements ; 

	void Awake(){
		this.aggressiveFactor = 1;
		this.soutienFactor = 1;
		this.trapFactor = 1;
	}

	public int getAgressiveFactor(){
		return this.aggressiveFactor;
	}

	public int getTrapFactor(){
		return this.trapFactor;
	}

	public int getSoutienFactor(){
		return this.soutienFactor;
	}

	public void setAgressiveFactor(int i){
		this.aggressiveFactor = i ;
	}

	public void setSoutienFactor(int j){
		this.soutienFactor = j ;
	}

	public void setTrapFactor(int i){
		this.trapFactor = i ;
	}

	public void play(){
		this.updateDestinations();
		StartCoroutine(getActionScore());
	}

	public void updateDestinations(){
		this.emplacements = GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getDestinations();
		for(int i = emplacements.Count-1 ; i>=0 ;i--){
			if(GameView.instance.getTileController(emplacements[i]).getIsTrapped()){
				if(!GameView.instance.getTileController(emplacements[i]).trap.isVisible){
					emplacements.RemoveAt(i);
				}
			}
		}
		this.emplacements.Add(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public IEnumerator getActionScore(){
		print("AU TOUR DE "+GameView.instance.getCurrentPlayingCard());
		yield return new WaitForSeconds(1);

		Tile tempTile ;
		int actionScore = -100 ;

		int passiveScore ;
		int bestScore = -200 ; 
		Tile bestEmplacement = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard());
		Tile bestTarget = new Tile(-1,-1);
		Skill bestSkill = new Skill();
		Skill passiveSkill = GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).Skills[0];
		GameCard targetCard ;
		int minDistanceOpponent = -1 ;

		int amount = 5 ;
		int bonusM = GameView.instance.getBonusMeteorites();
		amount = Mathf.RoundToInt(amount * bonusM / 100f);
		int lifeBonus ;
		List<Tile> targets ;
		bool isDead = false ;

		GameSkill gs ;
		List<Skill> skills = new List<Skill>() ;
		if(GameView.instance.getCurrentCard().isEffraye()){
			skills = new List<Skill>();
		} 
		else{
			skills = GameView.instance.getCurrentCard().Skills;
		}
		for (int i = 0 ; i < emplacements.Count ; i++){
			tempTile = emplacements[i];
			passiveScore=0;
			lifeBonus = 0 ; 
			if(passiveSkill.Id!=35 || i==emplacements.Count-1){
				if(GameView.instance.getTileController(emplacements[i]).getIsTrapped()){
					if(GameView.instance.getTileController(emplacements[i]).trap.getType()==4){
						lifeBonus = Mathf.Min(GameView.instance.getCurrentCard().GetTotalLife()-GameView.instance.getCurrentCard().getLife(),GameView.instance.getTileController(emplacements[i]).trap.getAmount());
						passiveScore+=lifeBonus;
					}
				}
				if(i==emplacements.Count-1){
					passiveScore+=5;
					if(passiveSkill.Id==65){
						passiveScore+=passiveSkill.Power+5;
					}
				}
				else{
					if((emplacements[i].y==7 || emplacements[i].y==0)){
						if((GameView.instance.getCurrentCard().getLife()+lifeBonus<=amount*(GameView.instance.nbTurns+1))||(passiveSkill.Id==69 && GameView.instance.getCurrentCard().getLife()+lifeBonus-10<=amount*(GameView.instance.nbTurns+1))){
							passiveScore-=200;
						}
						else{
							passiveScore-=amount*(GameView.instance.nbTurns+1);
						}
					}
					else if((emplacements[i].y==6 || emplacements[i].y==1)){
						if((GameView.instance.getCurrentCard().getLife()+lifeBonus<=amount*(GameView.instance.nbTurns))||(passiveSkill.Id==69 && GameView.instance.getCurrentCard().getLife()+lifeBonus-10<=amount*(GameView.instance.nbTurns))){
							passiveScore-=200;
						}
						else{
							passiveScore-=amount*(GameView.instance.nbTurns+0);
						}
					}
					else if((emplacements[i].y==5 || emplacements[i].y==2)){
						if((GameView.instance.getCurrentCard().getLife()+lifeBonus<=amount*(GameView.instance.nbTurns-1))||(passiveSkill.Id==69 && GameView.instance.getCurrentCard().getLife()+lifeBonus-10<=amount*(GameView.instance.nbTurns-1))){
							passiveScore-=200;
						}
						else{
							passiveScore-=Mathf.Max(0,amount*(GameView.instance.nbTurns-1));
						}
					}
					else if((emplacements[i].y==4 || emplacements[i].y==3)){
						if((GameView.instance.getCurrentCard().getLife()+lifeBonus<=amount*(GameView.instance.nbTurns-2))||(passiveSkill.Id==69 && GameView.instance.getCurrentCard().getLife()+lifeBonus-10<=amount*(GameView.instance.nbTurns-2))){
							passiveScore-=200;
						}
						else{
							passiveScore-=Mathf.Max(0,amount*(GameView.instance.nbTurns-2));
						}
					}

					if(passiveSkill.Id!=141){
						List<int> ennemis = GameView.instance.getOpponents(false);
						int tempScore = 0 ;
						int distance ;
						Tile tmpTile ;

						for(int j = 0 ; j < ennemis.Count ; j++){
							tempScore = 0 ; 
							tmpTile = GameView.instance.getTile(ennemis[j]);
							distance = Mathf.Abs(emplacements[i].x-tmpTile.x)+Mathf.Abs(emplacements[i].y-tmpTile.y);

							if(distance<=GameView.instance.getCard(ennemis[j]).getMove()+1){
								tempScore -= Mathf.RoundToInt(15f*(GameView.instance.getCard(ennemis[j]).getAttack()/GameView.instance.getCurrentCard().getLife()));
							}

							if(passiveSkill.Id!=76 && passiveSkill.Id!=138){
								if(distance<=GameView.instance.getCurrentCard().getMove()+1){
									tempScore += Mathf.RoundToInt(10f*(GameView.instance.getCurrentCard().getAttack()/GameView.instance.getCard(ennemis[j]).getLife()));
								}

								tempScore+=Mathf.RoundToInt(((12f-distance)/10f)*((1+GameView.instance.nbTurns*2)/2f)*(GameView.instance.getCurrentCard().getAttack()+GameView.instance.getCurrentCard().getLife()-GameView.instance.getCard(ennemis[j]).getAttack())/10f);
							}
							passiveScore += tempScore;
						}
					}
				}

				if(passiveSkill.Id==140){
					List<int> allys = GameView.instance.getAllys(false);
					if(i<emplacements.Count-1){
						passiveScore += Mathf.RoundToInt(10f*(30f+5f*passiveSkill.Power)/100f);
						for (int j = 0 ; j < allys.Count ; j++){
							if(GameView.instance.getCard(allys[j]).Skills[0].Id == 139 || GameView.instance.getCard(allys[j]).Skills[0].Id == 141){
								passiveScore +=Mathf.RoundToInt(30f * (GameView.instance.getCurrentSkill().Power*5f+30f) /100f);
							}
						}
					}
				}
				else if(passiveSkill.Id==141){
					if(i<emplacements.Count-1){
						if((11-passiveSkill.Power)>=GameView.instance.getCurrentCard().getLife()+lifeBonus){
							passiveScore-=500;
						}
						else{
							passiveScore -= (11-passiveSkill.Power);
						}
					}
				}
				else if(passiveSkill.Id==75){
					List<Tile> neighbours = emplacements[i].getImmediateNeighbourTiles();
					for(int s = 0 ; s < neighbours.Count ; s++){
						if(GameView.instance.getTileCharacterID(neighbours[s].x, neighbours[s].y)!=-1){
							targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[s].x, neighbours[s].y));
							passiveScore+=Mathf.Min(targetCard.GetTotalLife()-targetCard.getLife(), 3+2*GameView.instance.getCurrentCard().Skills[0].Power);
							if(targetCard.isMine){
								passiveScore=passiveScore*(-1);
							}
						}
					}
				}
				
				for(int j = 0 ; j < skills.Count ; j++){
					if(j==0){
						gs = GameSkills.instance.getSkill(0);
						targets = gs.getTargets(tempTile, false);
						for (int k = 0 ; k < targets.Count ;k++){
							actionScore = gs.getActionScore(targets[k], skills[j]);
							if(actionScore>0){
								Debug.Log("je teste Attaque : ("+passiveScore+"+"+actionScore+") - Empla : ("+emplacements[i].x+","+emplacements[i].y+") - Target : ("+targets[k].x+","+targets[k].y+") minD :"+minDistanceOpponent);
							}
							if(actionScore+passiveScore>bestScore){
								bestScore = actionScore+passiveScore;
								bestEmplacement = emplacements[i];
								bestTarget = targets[k];
								bestSkill.Id = 0;
								Debug.Log("Meilleur score trouvé");
							}
						}
					}
					else{
						gs = GameSkills.instance.getSkill(skills[j].Id);
						if (gs.isLaunchable(emplacements[i], false).Length<3){
							targets = new List<Tile>();
							if(gs.auto){
								targets.Add(emplacements[i]);
							}
							else{
								if(gs.id==27){
									targets.Add(emplacements[i]);
								}
								else if(skills[j].Id==130 || skills[j].Id==23){
									for(int i2 = 0; i2 < 6 ; i2++){
										for(int i3 = 0; i3 < 8 ; i3++){
											targets.Add(new Tile(i2,i3));
										}
									}
								}
								else{
									targets = gs.getTargets(tempTile, false);
								}
							}
							for (int k = 0 ; k < targets.Count ;k++){
								if(skills[j].Id==13 ||skills[j].Id==58){
									actionScore=0;
									List<Tile> neighbours = targets[k].getImmediateNeighbourTiles();
									for(int n = 0 ; n < neighbours.Count ; n++){
										if(GameView.instance.getTileCharacterID(neighbours[n].x, neighbours[n].y)==-1){
											
										}
										else{
											if(GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[n].x, neighbours[n].y)).isMine){
												
											}
											else{
												actionScore+=3;
											}
										}
									}
									if(targets[k].y==1||targets[k].y==6){
										actionScore+=1;
									}
									else if(targets[k].y==2||targets[k].y==5){
										actionScore+=7;
									}
									else if(targets[k].y==3||targets[k].y==4){
										actionScore+=15;
									}
									actionScore = Mathf.RoundToInt(actionScore*(skills[j].Power)/10f);
								}
								else{
									actionScore = gs.getActionScore(targets[k], skills[j]);
									if(skills[j].Id==92){
										Tile desti = new Tile(2*targets[k].x-emplacements[i].x, 2*targets[k].y-emplacements[i].y);
										if(desti.x>=0 && desti.x<GameView.instance.boardWidth && desti.y>=0 && desti.y<GameView.instance.boardHeight){
											if(GameView.instance.getTileController(desti).getIsTrapped()){
												int trap = GameView.instance.getTileController(desti).trap.getType();
												if(trap==1){
													actionScore+=10;
												}
												else if(trap==2){
													actionScore+=10;
												}
												else if(trap==3){
													actionScore+=10;
												}
												else if(trap==4){
													actionScore-=10;
												}
												else if(trap==5){
													actionScore-=10;
												}
											}
										}
									}
								}

								Debug.Log("Je teste "+GameSkills.instance.getSkill(skills[j].Id).name+" : ("+passiveScore+"+"+actionScore+") - Empla : ("+emplacements[i].x+","+emplacements[i].y+") - Target : ("+targets[k].x+","+targets[k].y+") minD :"+minDistanceOpponent);
									
								if(actionScore>0){
									if(actionScore+passiveScore>bestScore){
										bestScore = actionScore+passiveScore;
										bestEmplacement = emplacements[i];
										bestTarget = targets[k];
										bestSkill = skills[j];
										Debug.Log("Meilleur score trouvé !");
									}
								}
							}
						}
					}
				}
			}
		}
		Tile origine = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard());
		if(bestEmplacement.x!=origine.x || bestEmplacement.y!=origine.y){
			yield return new WaitForSeconds(UnityEngine.Random.Range(2,6));
			GameView.instance.dropCharacter(GameView.instance.getCurrentPlayingCard(), bestEmplacement, false, true);
			yield return new WaitForSeconds(UnityEngine.Random.Range(2,6));

			if(GameView.instance.getCurrentCardTile().x==bestEmplacement.x && GameView.instance.getCurrentCardTile().y==bestEmplacement.y){

				List<Tile> tempList = new List<Tile>();
				tempList.Add(bestTarget);
				GameController.instance.play(bestSkill.Id);

				if(bestSkill.Id == 131 || bestSkill.Id == 103){
					tempList.Add(new Tile(GameSkills.instance.getSkill(bestSkill.Id).getBestChoice(new Tile(0,0), new Skill()),0));
				}
				else{
					tempList.Add(bestTarget);
				}
				if(bestSkill.Id == 27){
					int resultat = GameSkills.instance.getSkill(bestSkill.Id).getBestChoice(bestEmplacement, bestSkill);
					Debug.Log("Lance-Flammes "+resultat);
					if(resultat==0){
						tempList[0].x = bestEmplacement.x;
						tempList[0].y = bestEmplacement.y-1;
					}
					else if(resultat==1){
						tempList[0].x = bestEmplacement.x;
						tempList[0].y = bestEmplacement.y+1;
					}
					else if(resultat==2){
						tempList[0].x = bestEmplacement.x-1;
						tempList[0].y = bestEmplacement.y;
					}
					else if(resultat==3){
						tempList[0].x = bestEmplacement.x+1;
						tempList[0].y = bestEmplacement.y;
					}
				}

				GameSkills.instance.getSkill(bestSkill.Id).resolve(tempList);
				if(passiveSkill.Id==33){
					if((11-passiveSkill.Power)>GameView.instance.getCurrentCard().getLife()){
						isDead = true ;
					}
				}
				if(bestSkill.Id==33){
					if((11-passiveSkill.Power)>GameView.instance.getCurrentCard().getLife()){
						isDead = true ;
					}
				}

				yield return new WaitForSeconds(2f);

				if((passiveSkill.Id==141 ||bestSkill.Id==15)&&isDead){
					bestScore = -100 ;
					bestEmplacement = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard());

					passiveSkill = GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).Skills[0];

					for (int i = 0 ; i < emplacements.Count ; i++){
						tempTile = emplacements[i];
						passiveScore = 0 ;
						lifeBonus = 0 ;
						if(GameView.instance.getTileController(emplacements[i]).getIsTrapped()){
							if(GameView.instance.getTileController(emplacements[i]).trap.getType()==4){
								lifeBonus = Mathf.Min(GameView.instance.getCurrentCard().GetTotalLife()-GameView.instance.getCurrentCard().getLife(),GameView.instance.getTileController(emplacements[i]).trap.getAmount());
								passiveScore+=lifeBonus;
							}
						}

						if(passiveSkill.Id==141){
							if(i<emplacements.Count-1){
								if((11-passiveSkill.Power)>=GameView.instance.getCurrentCard().getLife()+lifeBonus){
									passiveScore-=500;
								}
								else{
									passiveScore -= (11-passiveSkill.Power);
								}
							}
						}
						else if(passiveSkill.Id==75){
							List<Tile> neighbours = emplacements[i].getImmediateNeighbourTiles();
							for(int s = 0 ; s < neighbours.Count ; s++){
								if(GameView.instance.getTileCharacterID(neighbours[s].x, neighbours[s].y)!=-1){
									targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[s].x, neighbours[s].y));
									passiveScore+=Mathf.Min(targetCard.GetTotalLife()-targetCard.getLife(), 3+2*GameView.instance.getCurrentCard().Skills[0].Power);
									if(targetCard.isMine){
										passiveScore=passiveScore*(-1);
									}
								}
							}
						}

						if((emplacements[i].y==7 || emplacements[i].y==0)){
							if((GameView.instance.getCurrentCard().getLife()+lifeBonus<=amount*(GameView.instance.nbTurns+1))||(passiveSkill.Id==69 && GameView.instance.getCurrentCard().getLife()+lifeBonus-10<=amount*(GameView.instance.nbTurns+1))){
								passiveScore-=200;
							}
							else{
								passiveScore-=amount*(GameView.instance.nbTurns+1);
							}
						}
						else if((emplacements[i].y==6 || emplacements[i].y==1)){
							if((GameView.instance.getCurrentCard().getLife()+lifeBonus<=amount*(GameView.instance.nbTurns))||(passiveSkill.Id==69 && GameView.instance.getCurrentCard().getLife()+lifeBonus-10<=amount*(GameView.instance.nbTurns))){
								passiveScore-=200;
							}
							else{
								passiveScore-=amount*(GameView.instance.nbTurns);
							}
						}
						else if((emplacements[i].y==5 || emplacements[i].y==2)){
							if((GameView.instance.getCurrentCard().getLife()+lifeBonus<=amount*(GameView.instance.nbTurns-1))||(passiveSkill.Id==69 && GameView.instance.getCurrentCard().getLife()+lifeBonus-10<=amount*(GameView.instance.nbTurns-1))){
								passiveScore-=200;
							}
							else{
								passiveScore-=Mathf.Max(0,amount*(GameView.instance.nbTurns-1));
							}
						}
						else if((emplacements[i].y==4 || emplacements[i].y==3)){
							if((GameView.instance.getCurrentCard().getLife()+lifeBonus<=amount*(GameView.instance.nbTurns-2))||(passiveSkill.Id==69 && GameView.instance.getCurrentCard().getLife()+lifeBonus-10<=amount*(GameView.instance.nbTurns-2))){
								passiveScore-=200;
							}
							else{
								passiveScore-=Mathf.Max(0,amount*(GameView.instance.nbTurns-2));
							}
						}
					
						List<int> ennemis = GameView.instance.getOpponents(false);
						int tempScore = 0 ;
						int distance ;
						Tile tmpTile ;

						for(int j = 0 ; j < ennemis.Count ; j++){
							tempScore = 0 ; 
							tmpTile = GameView.instance.getTile(ennemis[j]);
							distance = Mathf.Abs(emplacements[i].x-tmpTile.x)+Mathf.Abs(emplacements[i].y-tmpTile.y);

							if(distance<=GameView.instance.getCard(ennemis[j]).getMove()+1){
								tempScore -= Mathf.RoundToInt(15f*(GameView.instance.getCard(ennemis[j]).getAttack()/GameView.instance.getCurrentCard().getLife()));
							}

							if(passiveSkill.Id!=76 && passiveSkill.Id!=138){
								if(distance<=GameView.instance.getCurrentCard().getMove()+1){
									tempScore += Mathf.RoundToInt(10f*(GameView.instance.getCurrentCard().getAttack()/GameView.instance.getCard(ennemis[j]).getLife()));
								}

								tempScore+=Mathf.RoundToInt(((12f-distance)/10f)*((1+GameView.instance.nbTurns*2)/2f)*(GameView.instance.getCurrentCard().getAttack()+GameView.instance.getCurrentCard().getLife()-GameView.instance.getCard(ennemis[j]).getAttack())/10f);
							}
							passiveScore += tempScore;
						}

						if(passiveScore>bestScore){
							bestScore = passiveScore ; 
							bestEmplacement = emplacements[i];
						}
					}
					if(bestEmplacement.x!=GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()).x||bestEmplacement.y!=GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()).y){
						GameView.instance.dropCharacter(GameView.instance.getCurrentPlayingCard(), bestEmplacement, false, true);
						yield return new WaitForSeconds(2f);
					}
				}
			}
		}
		else{
			if(bestScore>5){
				yield return new WaitForSeconds(2f);

				Debug.Log("Je joue2 "+bestSkill.Id);
				List<Tile> tempList = new List<Tile>();
				GameController.instance.play(bestSkill.Id);

				if(bestSkill.Id == 131 || bestSkill.Id == 103){
					tempList.Add(new Tile(GameSkills.instance.getSkill(bestSkill.Id).getBestChoice(new Tile(0,0), new Skill()),0));
				}
				else if(bestSkill.Id == 27){
					tempList.Add(new Tile(0,0));
					int resultat = GameSkills.instance.getSkill(bestSkill.Id).getBestChoice(bestEmplacement, bestSkill);
					Debug.Log("Lance-Flammes2 "+resultat);
					if(resultat==0){
						tempList[0].x = bestEmplacement.x;
						tempList[0].y = bestEmplacement.y-1;
					}
					else if(resultat==1){
						tempList[0].x = bestEmplacement.x;
						tempList[0].y = bestEmplacement.y+1;
					}
					else if(resultat==2){
						tempList[0].x = bestEmplacement.x-1;
						tempList[0].y = bestEmplacement.y;
					}
					else if(resultat==3){
						tempList[0].x = bestEmplacement.x+1;
						tempList[0].y = bestEmplacement.y;
					}
				}
				else{
					tempList.Add(bestTarget);
				}


				GameSkills.instance.getSkill(bestSkill.Id).resolve(tempList);
			}
			yield return new WaitForSeconds(2f);

			if(passiveSkill.Id!=35){
				this.updateDestinations();
				bestScore = -200 ;
				bestEmplacement = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard());

				for (int i = 0 ; i < emplacements.Count ; i++){
					tempTile = emplacements[i];
					passiveScore = 0 ;
					lifeBonus = 0 ;
					if(GameView.instance.getTileController(emplacements[i]).getIsTrapped()){
						if(GameView.instance.getTileController(emplacements[i]).trap.getType()==4){
							lifeBonus = Mathf.Min(GameView.instance.getCurrentCard().GetTotalLife()-GameView.instance.getCurrentCard().getLife(),GameView.instance.getTileController(emplacements[i]).trap.getAmount());
							passiveScore+=lifeBonus;
						}
					}
					if(passiveSkill.Id==141){
						if(i<emplacements.Count-1){
							if((11-passiveSkill.Power)>=GameView.instance.getCurrentCard().getLife()+lifeBonus){
								passiveScore-=500;
							}
							else{
								passiveScore -= (11-passiveSkill.Power);
							}
						}
					}
					else if(passiveSkill.Id==75){
						List<Tile> neighbours = emplacements[i].getImmediateNeighbourTiles();
						for(int s = 0 ; s < neighbours.Count ; s++){
							if(GameView.instance.getTileCharacterID(neighbours[s].x, neighbours[s].y)!=-1){
								targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[s].x, neighbours[s].y));
								passiveScore+=Mathf.Min(targetCard.GetTotalLife()-targetCard.getLife(), 3+2*GameView.instance.getCurrentCard().Skills[0].Power);
								if(targetCard.isMine){
									passiveScore=passiveScore*(-1);
								}
							}
						}
					}

					if((emplacements[i].y==7 || emplacements[i].y==0)){
						if((GameView.instance.getCurrentCard().getLife()+lifeBonus<=amount*(GameView.instance.nbTurns+1))||(passiveSkill.Id==69 && GameView.instance.getCurrentCard().getLife()+lifeBonus-10<=amount*(GameView.instance.nbTurns+1))){
							passiveScore-=200;
						}
						else{
							passiveScore-=amount*(GameView.instance.nbTurns+1);
						}
					}
					else if((emplacements[i].y==6 || emplacements[i].y==1)){
						if((GameView.instance.getCurrentCard().getLife()+lifeBonus<=amount*(GameView.instance.nbTurns))||(passiveSkill.Id==69 && GameView.instance.getCurrentCard().getLife()+lifeBonus-10<=amount*(GameView.instance.nbTurns))){
							passiveScore-=200;
						}
						else{
							passiveScore-=amount*(GameView.instance.nbTurns);
						}
					}
					else if((emplacements[i].y==5 || emplacements[i].y==2)){
						if((GameView.instance.getCurrentCard().getLife()+lifeBonus<=amount*(GameView.instance.nbTurns-1))||(passiveSkill.Id==69 && GameView.instance.getCurrentCard().getLife()+lifeBonus-10<=amount*(GameView.instance.nbTurns-1))){
							passiveScore-=200;
						}
						else{
							passiveScore-=Mathf.Max(0,amount*(GameView.instance.nbTurns-1));
						}
					}
					else if((emplacements[i].y==4 || emplacements[i].y==3)){
						if((GameView.instance.getCurrentCard().getLife()+lifeBonus<=amount*(GameView.instance.nbTurns-2))||(passiveSkill.Id==69 && GameView.instance.getCurrentCard().getLife()+lifeBonus-10<=amount*(GameView.instance.nbTurns-2))){
							passiveScore-=200;
						}
						else{
							passiveScore-=Mathf.Max(0,amount*(GameView.instance.nbTurns-2));
						}
					}
				
					if(passiveSkill.Id!=141){
						List<int> ennemis = GameView.instance.getOpponents(false);
						int tempScore = 0 ;
						int distance ;
						Tile tmpTile ;

						for(int j = 0 ; j < ennemis.Count ; j++){
							tempScore = 0 ; 
							tmpTile = GameView.instance.getTile(ennemis[j]);
							distance = Mathf.Abs(emplacements[i].x-tmpTile.x)+Mathf.Abs(emplacements[i].y-tmpTile.y);

							if(distance<=GameView.instance.getCard(ennemis[j]).getMove()+1){
								tempScore -= Mathf.RoundToInt(15f*(GameView.instance.getCard(ennemis[j]).getAttack()/GameView.instance.getCurrentCard().getLife()));
							}

							if(passiveSkill.Id!=76 && passiveSkill.Id!=138){
								if(distance<=GameView.instance.getCurrentCard().getMove()+1){
									tempScore += Mathf.RoundToInt(10f*(GameView.instance.getCurrentCard().getAttack()/GameView.instance.getCard(ennemis[j]).getLife()));
								}

								tempScore+=Mathf.RoundToInt(((12f-distance)/10f)*((2+GameView.instance.nbTurns*2)/2f)*(GameView.instance.getCurrentCard().getAttack()+GameView.instance.getCurrentCard().getLife()-GameView.instance.getCard(ennemis[j]).getAttack())/10f);
							}
							passiveScore += tempScore;
						}
					}

					Debug.Log("Score déplacement2 : "+passiveScore+" - ("+emplacements[i].x+","+emplacements[i].y+")");
						
					if(passiveScore>bestScore){
						Debug.Log("Meilleur score");
						bestScore = passiveScore ; 
						bestEmplacement = emplacements[i];
					}
				}

				if(bestEmplacement.x!=GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()).x||bestEmplacement.y!=GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()).y){
					GameView.instance.dropCharacter(GameView.instance.getCurrentPlayingCard(), bestEmplacement, false, true);
					yield return new WaitForSeconds(2f);
				}
			}
		}
		yield return new WaitForSeconds(2f);
		GameView.instance.setNextPlayer(false);
		yield break;
	}



}

