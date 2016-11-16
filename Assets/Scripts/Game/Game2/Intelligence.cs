using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class Intelligence
{
	float startTime ;
	float timerStart ;
	bool starting ;

	float bestScore ;
	TileM bestDeplacement ;
	TileM bestTarget ;
	int bestSkill ;
	int bestIDSkill ;

	public Intelligence(){
		this.starting = false ;
	}

	public void placeCards(){
		int strategy = UnityEngine.Random.Range(1,11);
		TileM startingTile = new TileM(-1,-1) ;
		int tempInt ;

		int[,] tilesOccupancy = Game.instance.getBoard().getOpponentStartingTilesOccupancy();

		for(int i = 4 ; i < 8 ; i++){
			tempInt = UnityEngine.Random.Range(1,101);
			if(Game.instance.getCards().getCardC(i).getFaction()==0){
				if(tempInt<=25){
					startingTile = new TileM(3,0);
				}
				else if(tempInt<=50){
					startingTile = new TileM(3,1);
				}
				else if(tempInt<=75){
					startingTile = new TileM(2,0);
				}
				else if(tempInt<=100){
					startingTile = new TileM(2,1);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==1){
				if(tempInt<=25){
					startingTile = new TileM(0,0);
				}
				else if(tempInt<=50){
					startingTile = new TileM(5,0);
				}
				else if(tempInt<=75){
					startingTile = new TileM(1,0);
				}
				else if(tempInt<=100){
					startingTile = new TileM(4,0);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==2){
				if(tempInt<=25){
					startingTile = new TileM(2,0);
				}
				else if(tempInt<=50){
					startingTile = new TileM(1,0);
				}
				else if(tempInt<=75){
					startingTile = new TileM(3,0);
				}
				else if(tempInt<=100){
					startingTile = new TileM(4,0);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==3){
				if(tempInt<=25){
					startingTile = new TileM(0,1);
				}
				else if(tempInt<=50){
					startingTile = new TileM(1,1);
				}
				else if(tempInt<=75){
					startingTile = new TileM(4,1);
				}
				else if(tempInt<=100){
					startingTile = new TileM(5,1);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==4){
				if(tempInt<=25){
					startingTile = new TileM(1,1);
				}
				else if(tempInt<=50){
					startingTile = new TileM(3,1);
				}
				else if(tempInt<=75){
					startingTile = new TileM(5,1);
				}
				else if(tempInt<=100){
					startingTile = new TileM(4,0);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==5){
				if(tempInt<=25){
					startingTile = new TileM(1,0);
				}
				else if(tempInt<=50){
					startingTile = new TileM(2,0);
				}
				else if(tempInt<=75){
					startingTile = new TileM(3,0);
				}
				else if(tempInt<=100){
					startingTile = new TileM(4,0);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==6){
				if(tempInt<=25){
					startingTile = new TileM(2,0);
				}
				else if(tempInt<=50){
					startingTile = new TileM(3,0);
				}
				else if(tempInt<=75){
					startingTile = new TileM(3,1);
				}
				else if(tempInt<=100){
					startingTile = new TileM(2,1);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==7){
				if(tempInt<=25){
					startingTile = new TileM(1,1);
				}
				else if(tempInt<=50){
					startingTile = new TileM(0,1);
				}
				else if(tempInt<=75){
					startingTile = new TileM(5,1);
				}
				else if(tempInt<=100){
					startingTile = new TileM(4,1);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==8){
				if(tempInt<=25){
					startingTile = new TileM(0,0);
				}
				else if(tempInt<=50){
					startingTile = new TileM(1,0);
				}
				else if(tempInt<=75){
					startingTile = new TileM(4,0);
				}
				else if(tempInt<=100){
					startingTile = new TileM(5,0);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==9){
				if(tempInt<=25){
					startingTile = new TileM(1,1);
				}
				else if(tempInt<=50){
					startingTile = new TileM(4,1);
				}
				else if(tempInt<=75){
					startingTile = new TileM(1,0);
				}
				else if(tempInt<=100){
					startingTile = new TileM(4,0);
				}
			}

			while(tilesOccupancy[startingTile.x, startingTile.y]!=0){
				startingTile = new TileM(UnityEngine.Random.Range(0,6), UnityEngine.Random.Range(0,2));
			}

			tilesOccupancy[startingTile.x, startingTile.y]=1;
			Game.instance.moveOn(startingTile.x, startingTile.y+6, i);
		}
	}

	public void launch(){
		//this.startTime = UnityEngine.Random.Range(1,10);
		this.startTime = 0.5f;
		this.timerStart = 0f;
		this.starting = true;
	}

	public bool isStarting(){
		return this.starting;
	}

	public void addStartTime(float f){
		this.timerStart+=f;
		if(this.timerStart>this.startTime){
			this.starting = false ;
			Game.instance.addStartGame(false);
		}
	}

	public IEnumerator play(){
		this.choosePlay(!Game.instance.getCurrentCard().isParalized());

		if(this.bestSkill!=-1){
			if(this.bestDeplacement.x!=-1){
				yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f,3f));
				Game.instance.moveOn(this.bestDeplacement.x, this.bestDeplacement.y, Game.instance.getCurrentCardID());
			}

			yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f,3f));
			if(this.bestIDSkill==-1){
				Game.instance.getSkills().skills[this.bestSkill].resolve(this.bestTarget.x, this.bestTarget.y, new Skill());
			}
			else{
				Game.instance.getSkills().skills[this.bestSkill].resolve(this.bestTarget.x, this.bestTarget.y, Game.instance.getCurrentCard().getCardM().getSkill(this.bestIDSkill));
			}

			if(this.bestDeplacement.x==-1){
				this.choosePlay(false);
				if(this.bestDeplacement.x!=-1){
					yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f,3f));
					Game.instance.moveOn(this.bestDeplacement.x, this.bestDeplacement.y, Game.instance.getCurrentCardID());
				}
			}
		}
		else{
			if(this.bestDeplacement.x!=-1){
				yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f,3f));
				Game.instance.moveOn(this.bestDeplacement.x, this.bestDeplacement.y, Game.instance.getCurrentCardID());
			}
		}

		yield return new WaitForSeconds(2f);
	
		Game.instance.setLaunchNextTurn(true);
	}

	public void choosePlay(bool action){
		CardC card = Game.instance.getCurrentCard();
		int[,] board = Game.instance.getBoard().getCurrentBoard();
		bool[,] destinations = card.getDestinations();
		this.initBestScore();
		int[,] tempBoard = new int[Game.instance.getBoard().getBoardWidth(), Game.instance.getBoard().getBoardHeight()] ;
		TileM currentTile = Game.instance.getCurrentCard().getTileM();
		int currentID = Game.instance.getCurrentCardID();
		int activeScore ;
		int passiveScore ;
		List<TileM> targets;

		for(int x = 0 ; x < Game.instance.getBoard().getBoardWidth() ; x++){
			for(int y = 0 ; y < Game.instance.getBoard().getBoardHeight() ; y++){
				if(destinations[x,y] || (x==currentTile.x && y==currentTile.y)){
					for(int a = 0 ; a < Game.instance.getBoard().getBoardWidth() ; a++){
						for(int b = 0 ; b < Game.instance.getBoard().getBoardHeight() ; b++){
							if(currentTile.x==a && currentTile.y==b){
								if(x!=currentTile.x || y!=currentTile.y){
									tempBoard[a,b] = -1;
								}
							}
							else{
								if(a==x && b==y){
									tempBoard[a,b] = currentID;
								}
								else{
									tempBoard[a,b] = board[a,b];
								}
							}
						}
					}

					if(x==currentTile.x && y==currentTile.y){
						passiveScore = 0 ;
					}
					else{
						passiveScore = this.getPassiveScore(x,y,tempBoard,false);
					}

					if(action){
						for(int s = 1 ; s < card.getCardM().getNbActivatedSkill() ; s++){
							targets = Game.instance.getSkills().skills[card.getCardM().getSkill(s).Id].getTargetTiles(tempBoard, card, new TileM(x,y));
							for(int t = 0 ; t < targets.Count ; t++){
								activeScore = Game.instance.getSkills().skills[card.getCardM().getSkill(s).Id].getActionScore(targets[t], card.getCardM().getSkill(s),tempBoard);
								this.testBestScore(activeScore+passiveScore, x, y, targets[t], card.getCardM().getSkill(s).Id,s);
							}
						}
						targets = Game.instance.getSkills().skills[0].getTargetTiles(tempBoard, card, new TileM(x,y));
						for(int t = 0 ; t < targets.Count ; t++){
							activeScore = Game.instance.getSkills().skills[0].getActionScore(targets[t], new Skill(),tempBoard);
							this.testBestScore(activeScore+passiveScore, x, y, targets[t], 0,-1);
						}
					}
					this.testBestScore(passiveScore, x, y, new TileM(-1,-1), -1,-1);
				}
			}
		}
	}

	public void initBestScore(){
		this.bestScore = -1000 ;
		this.bestDeplacement = new TileM(-1,-1);
		this.bestTarget = new TileM(-1, -1);
		this.bestSkill = -1;
	}

	public void testBestScore(int s, int x, int y, TileM t, int b, int i){
		//Debug.Log("TESTSCORE "+s+",("+x+","+y+"),("+t.x+","+t.y+"),"+b);
		if(s>this.bestScore){
			Debug.Log("BESTSCORE "+s+",("+x+","+y+"),("+t.x+","+t.y+"),"+b);
			this.bestScore = s ;
			this.bestDeplacement = new TileM(x,y);
			this.bestTarget = new TileM(t.x, t.y);
			this.bestSkill = b ;
			this.bestIDSkill = i ;
		}
	}

	public int getPassiveScore(int x, int y, int[,] tempBoard, bool debug){
		int meteores = Game.instance.getIndexMeteores();
		int passiveScore = 0;
		CardC card = Game.instance.getCurrentCard();

		if(y==0 || y==7){
			passiveScore = -1*card.getDamageScore(card, Mathf.Max(0,5*meteores));
		}
		if(y==1 || y==6){
			passiveScore = -1*card.getDamageScore(card, Mathf.Max(0,5*(meteores-1)));
			passiveScore+=1;
		}
		if(y==2 || y==5){
			passiveScore = -1*card.getDamageScore(card, Mathf.Max(0,5*(meteores-2)));
			passiveScore+=2;
		}
		if(y==3 || y==4){
			passiveScore = -1*card.getDamageScore(card, Mathf.Max(0,5*(meteores-3)));
			passiveScore+=3;
		}
		if(debug){
			Debug.Log("- - - - - - - - - - - - - ");
			Debug.Log("SCORE METEORES "+passiveScore);
		}

		int distance ; 
		CardC target ;
		for(int i = 0 ; i < 4 ; i++){
			target = Game.instance.getCards().getCardC(i);
			if(!target.isDead()){
				distance = Mathf.Abs(x-target.getTileM().x)+Mathf.Abs(y-target.getTileM().y);
				if((distance-1)<=card.getMove()){
					passiveScore+=Mathf.RoundToInt(card.getDamageScore(target, card.getAttack())/2f);
					if(debug){
						Debug.Log("AJOUT "+Mathf.RoundToInt(card.getDamageScore(target, card.getAttack())/4f));
					}
				}
				if((distance-1)<=target.getMove()){
					passiveScore-=Mathf.RoundToInt(target.getDamageScore(card, target.getAttack())/2f);
					if(debug){
						Debug.Log("RETRAIT "+Mathf.RoundToInt(target.getDamageScore(card, target.getAttack())/2f));
					}
				}
			}
		}

		if(x==card.getTileM().x && y==card.getTileM().y){
			passiveScore+=1;
			if(debug){
				Debug.Log("AJOUT BONUS -1");
			}
		}
		if(debug){
			Debug.Log("SCORE FINAL "+passiveScore);
		}
		return passiveScore ;

	}

	public void move(){

	}

	public void act(){
		Game.instance.launchNextTurn();
	}
}

