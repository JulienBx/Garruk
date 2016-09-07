using System;
using UnityEngine;
using System.Collections.Generic;

public class Gamecards
{
	GameObject[] cards ;
	int nbCards ;

	public Gamecards ()
	{
		this.cards = new GameObject[8];
		this.nbCards = 0 ;
	}

	public CardC getCardC(int i){
		return this.cards[i].GetComponent<CardC>();
	}

	public int getNumberOfCards(){
		return this.nbCards;
	}

	public bool isLoaded(){
		return (this.nbCards==cards.Length) ;
	}

	public void GenerateIADeck(){
    	ApplicationModel.opponentDeck=new Deck();
        ApplicationModel.opponentDeck.cards=new List<Card>();

        int fixedIDType = -1;
        int cardType ;
        List<Skill> skills ;
        int nbSkills ;
        int compteurSkills;
        int idSkill=-1;
        bool hasFoundSkill ;
        int difficultyLevel = -1 ; 
        int randomTest ;
        List<int> passive = new List<int>() ;

        int[,] passiveSkills = new int[10,4];
        passiveSkills[0,0]=72;
        passiveSkills[0,1]=73;
        passiveSkills[0,2]=75;
        passiveSkills[0,3]=76;
        passiveSkills[1,0]=64;
        passiveSkills[1,1]=65;
        passiveSkills[1,2]=66;
        passiveSkills[1,3]=67;
        passiveSkills[2,0]=68;
        passiveSkills[2,1]=69;
        passiveSkills[2,2]=70;
        passiveSkills[2,3]=71;
        passiveSkills[3,0]=32;
        passiveSkills[3,1]=33;
        passiveSkills[3,2]=34;
        passiveSkills[3,3]=35;
        passiveSkills[4,0]=51;
        passiveSkills[4,1]=0;
        passiveSkills[4,2]=0;
        passiveSkills[4,3]=0;
        passiveSkills[5,0]=0;
        passiveSkills[5,1]=0;
        passiveSkills[5,2]=0;
        passiveSkills[5,3]=0;
        passiveSkills[6,0]=138;
        passiveSkills[6,1]=139;
        passiveSkills[6,2]=140;
        passiveSkills[6,3]=141;
        passiveSkills[7,0]=110;
        passiveSkills[7,1]=111;
        passiveSkills[7,2]=112;
        passiveSkills[7,3]=113;
        passiveSkills[8,0]=0;
        passiveSkills[8,1]=0;
        passiveSkills[8,2]=0;
        passiveSkills[8,3]=0;
        passiveSkills[9,0]=43;
        passiveSkills[9,1]=0;
        passiveSkills[9,2]=0;
        passiveSkills[9,3]=0;

        int[,] activeSkills = new int[10,10]{{2,3,4,5,6,7,39,56,57,94},{8,9,10,11,12,13,14,15,58,59},{16,17,18,19,20,21,63,91,92,93},{22,23,24,25,26,27,28,29,30,31},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0},{40,41,42,105,128,129,130,131,132,133},{95,101,100,102,103,104,106,107,108,109},{0,0,0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0,0,0}};
		randomTest = UnityEngine.Random.Range(1,11);

        if(ApplicationModel.player.ChosenGameType>0 && ApplicationModel.player.ChosenGameType<11){
            fixedIDType = ApplicationModel.player.ChosenGameType-1;
			difficultyLevel = -1 ; 
        }
		else if(ApplicationModel.player.ChosenGameType==0){
			if(randomTest<6){
           		difficultyLevel = 1 ; 
           	}
			else if(randomTest<9){
           		difficultyLevel = 2 ; 
           	}
           	else{
				difficultyLevel = 3 ; 
           	}
        }
		else if(ApplicationModel.player.ChosenGameType<21){
			if(randomTest<ApplicationModel.player.ChosenGameType-5){
				difficultyLevel=1;
			}
			else if(randomTest<ApplicationModel.player.ChosenGameType-10){
				difficultyLevel=2;
			}
			else{
				difficultyLevel=3;
			}
        }
         
        for (int i = 0 ; i < 4 ; i++){
            if(fixedIDType==-1){
                cardType = UnityEngine.Random.Range(0,10);
                while(cardType==4 || cardType==5 || cardType==8 || cardType==9){
                    cardType = UnityEngine.Random.Range(0,10);
                }
            }
            else{
                cardType = fixedIDType;
            }

            skills = new List<Skill>();
            hasFoundSkill = true ;
            while (hasFoundSkill){
                idSkill = passiveSkills[cardType,UnityEngine.Random.Range(0,4)];
                hasFoundSkill = false ;
                for(int l = 0 ; l < passive.Count ; l++){
                    if(idSkill==passive[l]){
                        hasFoundSkill = true ;
                    }
                }
            }

            passive.Add(idSkill);
            skills.Add(new Skill(idSkill, this.getRandomInt10(difficultyLevel)));

            nbSkills = 0 ;
            if(difficultyLevel==1){
            	nbSkills = UnityEngine.Random.Range(1,4);
            }
			else if(difficultyLevel==2){
            	nbSkills = UnityEngine.Random.Range(2,4);
            }
			else if(difficultyLevel==3){
            	nbSkills = 3;
            }
			else if(difficultyLevel==-1){
				nbSkills = 1;
            }

            compteurSkills=0 ; 

            for(int j = 0 ; j < nbSkills ; j++){
                hasFoundSkill = true ;
                while (hasFoundSkill){
                    idSkill = activeSkills[cardType, UnityEngine.Random.Range(0,10)];
                    hasFoundSkill = false ;
                    if(idSkill!=0){
                        for (int k = 0 ; k < compteurSkills ; k++){
                            if(skills[1+k].Id==idSkill){
                                hasFoundSkill = true ;
                            }
                        }
                    }
                }

				skills.Add(new Skill(idSkill, this.getRandomInt10(difficultyLevel)));
                compteurSkills++;
            }
			ApplicationModel.opponentDeck.cards.Add(new GameCard(WordingCardName.getName(skills[0].Id), this.getRandomLife(cardType, difficultyLevel), cardType, this.getRandomMove(cardType), this.getRandomAttack(cardType, difficultyLevel), skills,i));
        }
    }

	private int getRandomMove(int cardType){
        if(cardType==0){
            return 3;
        }
        else if(cardType==1){
            return 5;
        }
        else if(cardType==2){
            return 2;
        }
        else if(cardType==3){
            return 4;
        }
        else if(cardType==4){
            return 5;
        }
        else if(cardType==5){
            return 2;
        }
        else if(cardType==6){
            return 3;
        }
        else if(cardType==7){
            return 3;
        }
        else if(cardType==8){
            return 2;
        }
        else{
            return 4;
        }
    }

    public int getRandomInt10(int difficulty){
		int randomTest = UnityEngine.Random.Range(1,101);
		if(difficulty==1){
			if(randomTest>90){
				randomTest = 10;
			}
			else if(randomTest>80){
				randomTest = 9;
			}
			else if(randomTest>70){
				randomTest = 8;
			}
			else if(randomTest>60){
				randomTest = 7;
			}
			else if(randomTest>50){
				randomTest = 6;
			}
			else if(randomTest>40){
				randomTest = 5;
			}
			else if(randomTest>30){
				randomTest = 4;
			}
			else if(randomTest>20){
				randomTest = 3;
			}
			else if(randomTest>10){
				randomTest = 2;
			}
			else {
				randomTest = 1;
			}
		}
		else if(difficulty==2){
			if(randomTest>85){
				randomTest = 10;
			}
			else if(randomTest>71){
				randomTest = 9;
			}
			else if(randomTest>58){
				randomTest = 8;
			}
			else if(randomTest>46){
				randomTest = 7;
			}
			else if(randomTest>35){
				randomTest = 6;
			}
			else if(randomTest>25){
				randomTest = 5;
			}
			else if(randomTest>16){
				randomTest = 4;
			}
			else if(randomTest>8){
				randomTest = 3;
			}
			else if(randomTest>3){
				randomTest = 2;
			}
			else {
				randomTest = 1;
			}
        }
		else if(difficulty==3){
			if(randomTest>75){
				randomTest = 10;
			}
			else if(randomTest>60){
				randomTest = 9;
			}
			else if(randomTest>42){
				randomTest = 8;
			}
			else if(randomTest>32){
				randomTest = 7;
			}
			else if(randomTest>24){
				randomTest = 6;
			}
			else if(randomTest>18){
				randomTest = 5;
			}
			else if(randomTest>13){
				randomTest = 4;
			}
			else if(randomTest>9){
				randomTest = 3;
			}
			else if(randomTest>4){
				randomTest = 2;
			}
			else {
				randomTest = 1;
			}
        }
		else if(difficulty==-1){
			randomTest = 1;
        }
        return randomTest;
    }

	public int getRandomLife(int cardType, int difficultyLevel){
    	int randomTest = -1;
		randomTest = 10*this.getRandomInt10(difficultyLevel);

		if(cardType==0){
			return (40+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==1){
            return (30+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==2){
			return (50+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==3){
			return (30+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==4){
			return (40+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==5){
			return (60+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==6){
			return (30+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==7){
			return (30+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else if(cardType==8){
			return (50+Mathf.RoundToInt(20*(randomTest)/100f));
        }
        else{
			return (40+Mathf.RoundToInt(20*(randomTest)/100f));
        }
    }

	public int getRandomAttack(int cardType, int difficultyLevel){
    	int randomTest = -1;
		randomTest = 10*this.getRandomInt10(difficultyLevel);

		if(cardType==0){
			return (10+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==1){
			return (10+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==2){
			return (15+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==3){
			return (5+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==4){
			return (5+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==5){
			return (15+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==6){
			return (5+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==7){
			return (10+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else if(cardType==8){
			return (10+Mathf.RoundToInt(15*(randomTest)/100f));
        }
        else{
			return (15+Mathf.RoundToInt(15*(randomTest)/100f));
        }
    }

	public void createPlayingCard(int i, CardM c, bool mine, GameObject cardModel, int deckOrder)
	{
		this.cards[i] = cardModel;
		this.getCardC(i).setCard(c, mine, i);

		if (Game.instance.isFirstPlayer()==mine){
			this.getCardC(i).setTile(new TileM(1+deckOrder, 0));
			Game.instance.getBoard().getTileC(1+deckOrder, 0).setCharacterID(i);
		}
		else{
			this.getCardC(i).setTile(new TileM(4-deckOrder, 7));
			Game.instance.getBoard().getTileC(4-deckOrder, 7).setCharacterID(i);
		}

		this.getCardC(i).setBackTile(mine);
		this.getCardC(i).checkPassiveSkills();

		this.getCardC(i).show(mine);
		this.nbCards++;
	}
}

