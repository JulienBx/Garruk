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
		this.aggressiveFactor = UnityEngine.Random.Range(1,4);
		this.soutienFactor = UnityEngine.Random.Range(1,4);
		this.trapFactor = UnityEngine.Random.Range(1,4);
		print("IA Parameters ("+this.aggressiveFactor+","+this.soutienFactor+","+this.trapFactor+")");
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
		this.emplacements.Add(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public IEnumerator getActionScore(){
		Debug.Log("Je cherche");
		Tile tempTile ;
		int actionScore = 0 ;

		int passiveScore = 0 ;
	
		int bestScore = 0 ; 
		Tile bestEmplacement = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard());
		Tile bestTarget = new Tile(-1,-1);
		Skill bestSkill = new Skill();
		Skill passiveSkill = GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).Skills[0];

		GameSkill gs ;
		List<Skill> skills = GameView.instance.getCurrentCard().Skills; 
		for (int i = 0 ; i < emplacements.Count ; i++){
			tempTile = emplacements[i];
			passiveScore=0;

			if(i==emplacements.Count-1){
				passiveScore+=20;
			}
			else{
				if((emplacements[i].y==7 || emplacements[i].y==0)){
					if(GameView.instance.getCurrentCard().getLife()<=10*(GameView.instance.nbTurns+1)){
						passiveScore-=500;
					}
					else{
						passiveScore-=10*(GameView.instance.nbTurns+1);
					}
				}
				else if((emplacements[i].y==6 || emplacements[i].y==1)){
					if(GameView.instance.getCurrentCard().getLife()<=10*(GameView.instance.nbTurns)){
						passiveScore-=500;
					}
					else{
						passiveScore-=10*(GameView.instance.nbTurns);
					}
				}
				else if((emplacements[i].y==5 || emplacements[i].y==2)){
					if(GameView.instance.getCurrentCard().getLife()<=10*(GameView.instance.nbTurns-1)){
						passiveScore-=500;
					}
					else{
						passiveScore-=Mathf.Max(0,10*(GameView.instance.nbTurns-1));
					}
				}

				passiveScore+=Mathf.RoundToInt((GameView.instance.getCurrentCard().getLife()-40)*(7-emplacements[i].y)/10f);
			}

			if(passiveSkill.Id==138){
				passiveScore += (emplacements[i].y)*3;
			}
			else if(passiveSkill.Id==140){
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
					if((11-passiveSkill.Power)>=GameView.instance.getCurrentCard().getLife()){
						passiveScore-=500;
					}
					else{
						passiveScore -= (11-passiveSkill.Power);
					}
				}
			}
			for(int j = 0 ; j < skills.Count ; j++){
				if(j==0){
					gs = GameSkills.instance.getSkill(0);
					List<Tile> targets = gs.getTargets(tempTile, false);
					for (int k = 0 ; k < targets.Count ;k++){
						actionScore = gs.getActionScore(targets[k], skills[j]);
						if(actionScore+passiveScore>bestScore){
							bestScore = actionScore+passiveScore;
							bestEmplacement = emplacements[i];
							bestTarget = targets[k];
							bestSkill.Id = 0;
							Debug.Log("Meilleur score trouvé : ("+passiveScore+"+"+actionScore+"="+bestScore+") - "+bestSkill.Id+" - ("+bestEmplacement.x+","+bestEmplacement.y+") - ("+bestTarget.x+","+bestTarget.y+")");
						}
					}
				}
				else{
					gs = GameSkills.instance.getSkill(skills[j].Id);
					if (gs.isLaunchable(emplacements[i], false).Length<3){
						List<Tile> targets = gs.getTargets(tempTile, false);
						if(gs.auto){
							if(skills[j].Id==130){
								for(int i2 = 0; i2 < 6 ; i2++){
									for(int i3 = 0; i3 < 8 ; i3++){
										targets.Add(new Tile(i2,i3));
									}
								}
							}
							else{
								targets.Add(new Tile(0,0));
							}
						}
						for (int k = 0 ; k < targets.Count ;k++){
							actionScore = gs.getActionScore(targets[k], skills[j]);
							if(actionScore+passiveScore>bestScore){
								bestScore = actionScore+passiveScore;
								bestEmplacement = emplacements[i];
								bestTarget = targets[k];
								bestSkill = skills[j];
								Debug.Log("Meilleur score trouvé : ("+passiveScore+"+"+actionScore+"="+bestScore+") - "+bestSkill.Id+" - ("+bestEmplacement.x+","+bestEmplacement.y+") - ("+bestTarget.x+","+bestTarget.y+")");
							}
						}
					}
				}
			}
		}
		Tile origine = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard());
		if(bestEmplacement.x!=origine.x || bestEmplacement.y!=origine.y){
			GameView.instance.dropCharacter(GameView.instance.getCurrentPlayingCard(), bestEmplacement, false, true);
			yield return new WaitForSeconds(2f);

			List<Tile> tempList = new List<Tile>();
			tempList.Add(bestTarget);
			GameController.instance.play(bestSkill.Id);

			if(bestSkill.Id == 131){
				tempList[0].x = GameSkills.instance.getSkill(bestSkill.Id).getBestChoice(new Tile(0,0), new Skill());
			}

			GameSkills.instance.getSkill(bestSkill.Id).resolve(tempList);
			yield return new WaitForSeconds(2f);

			if(passiveSkill.Id==141){
				bestScore = 0 ;
				bestEmplacement = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard());

				passiveSkill = GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).Skills[0];

				for (int i = 0 ; i < emplacements.Count ; i++){
					tempTile = emplacements[i];
					passiveScore = 0 ;
					if(passiveSkill.Id==138){
						passiveScore += (emplacements[i].y)*3;
					}
					else if(passiveSkill.Id==141){
						if(i<emplacements.Count-1){
							if((11-passiveSkill.Power)>=GameView.instance.getCurrentCard().getLife()){
								passiveScore-=500;
							}
							else{
								passiveScore -= (11-passiveSkill.Power);
							}
						}
					}

					if((emplacements[i].y==7 || emplacements[i].y==0)){
						if(GameView.instance.getCurrentCard().getLife()<=10*(GameView.instance.nbTurns+1)){
							passiveScore-=500;
						}
						else{
							passiveScore-=10*(GameView.instance.nbTurns+1);
						}
					}
					else if((emplacements[i].y==6 || emplacements[i].y==1)){
						if(GameView.instance.getCurrentCard().getLife()<=10*(GameView.instance.nbTurns)){
							passiveScore-=500;
						}
						else{
							passiveScore-=10*(GameView.instance.nbTurns);
						}
					}
					else if((emplacements[i].y==5 || emplacements[i].y==2)){
						if(GameView.instance.getCurrentCard().getLife()<=10*(GameView.instance.nbTurns-1)){
							passiveScore-=500;
						}
						else{
							passiveScore-=Mathf.Max(0,10*(GameView.instance.nbTurns-1));
						}
					}
				
					if(passiveSkill.Id!=141){
						passiveScore+=Mathf.RoundToInt((GameView.instance.getCurrentCard().getLife()-40)*(7-emplacements[i].y)/10f);
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
		else{
			if(bestScore>20){
				yield return new WaitForSeconds(2f);

				List<Tile> tempList = new List<Tile>();
				tempList.Add(bestTarget);
				GameController.instance.play(bestSkill.Id);
				GameSkills.instance.getSkill(bestSkill.Id).resolve(tempList);
			}
			yield return new WaitForSeconds(2f);

			this.updateDestinations();
			bestScore = 0 ;
			bestEmplacement = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard());

			for (int i = 0 ; i < emplacements.Count ; i++){
				tempTile = emplacements[i];
				passiveScore = 0 ;
				if(passiveSkill.Id==138){
					passiveScore += (emplacements[i].y)*3;
				}
				else if(passiveSkill.Id==141){
					if(i<emplacements.Count-1){
						if((11-passiveSkill.Power)>=GameView.instance.getCurrentCard().getLife()){
							passiveScore-=500;
						}
						else{
							passiveScore -= (11-passiveSkill.Power);
						}
					}
				}

				if((emplacements[i].y==7 || emplacements[i].y==0)){
					if(GameView.instance.getCurrentCard().getLife()<=10*(GameView.instance.nbTurns+1)){
						passiveScore-=500;
					}
					else{
						passiveScore-=10*(GameView.instance.nbTurns+1);
					}
				}
				else if((emplacements[i].y==6 || emplacements[i].y==1)){
					if(GameView.instance.getCurrentCard().getLife()<=10*(GameView.instance.nbTurns)){
						passiveScore-=500;
					}
					else{
						passiveScore-=10*(GameView.instance.nbTurns);
					}
				}
				else if((emplacements[i].y==5 || emplacements[i].y==2)){
					if(GameView.instance.getCurrentCard().getLife()<=10*(GameView.instance.nbTurns-1)){
						passiveScore-=500;
					}
					else{
						passiveScore-=Mathf.Max(0,10*(GameView.instance.nbTurns-1));
					}
				}
			
				if(passiveSkill.Id!=141){
					passiveScore+=Mathf.RoundToInt((GameView.instance.getCurrentCard().getLife()-40)*(7-emplacements[i].y)/10f);
				}

				if(passiveScore>bestScore){
					Debug.Log("Meilleur score déplacement : "+passiveScore+" - ("+bestEmplacement.x+","+bestEmplacement.y+")");
					bestScore = passiveScore ; 
					bestEmplacement = emplacements[i];
				}
			}

			if(bestEmplacement.x!=GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()).x||bestEmplacement.y!=GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()).y){
				GameView.instance.dropCharacter(GameView.instance.getCurrentPlayingCard(), bestEmplacement, false, true);
				yield return new WaitForSeconds(2f);
			}
		}

		GameView.instance.setNextPlayer();
		

		yield break;
	}



}

