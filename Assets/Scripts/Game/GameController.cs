using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameController : Photon.MonoBehaviour
{	
	public static GameController instance;
	private float timePerTurn = 30 ;
	
	//URL pour les appels en BDD
	string URLStat = ApplicationModel.host + "updateResult.php";

	//Variable Photon
	const string roomNamePrefix = "GarrukGame";
	
	//Variables de gestion
	bool isReconnecting = false ;
	bool isFirstPlayer = false;
	bool bothPlayerLoaded = false ;
	int nbPlayers = 0 ;
	int nbPlayersReadyToFight = 0; 
	int currentPlayingCard = -1;
	int[] rankedPlayingCardsID; 
	TargetPCCHandler targetPCCHandler ;
	TargetTileHandler targetTileHandler ;
	bool isTutorialLaunched;

	void Awake()
	{
		instance = this;
		
		PhotonNetwork.autoCleanUpPlayerObjects = false;
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
		
//		if (ApplicationModel.launchGameTutorial)
//		{
//			this.tutorial = Instantiate(this.TutorialObject) as GameObject;
//			this.tutorial.AddComponent<GameTutorialController>();
//			this.tutorial.GetComponent<GameTutorialController>().launchSequence(0);
//			this.isTutorialLaunched = true;
//		}
	}

	public void resize(int w, int h)
	{
		
	}
	
	public void timeOut()
	{
	
	}
	
	public void playAlarm()
	{
		
	}

	

	

	public void hideHoveredTile()
	{
		//this.tiles [currentHoveredTile.x, currentHoveredTile.y].GetComponent<TileController>().hideHover();
		//this.isHovering = false;
	}

	public void hideHoveredPlayingCard()
	{
//		this.playingCards [this.hoveredPlayingCard].GetComponent<PlayingCardController>().hideHover();
//		this.isHovering = false;
//		this.hoveredPlayingCard = -1;
	}

	public void hoverTile(Tile t)
	{
//		this.tiles [t.x, t.y].GetComponent<TileController>().displayHover();
//		this.currentHoveredTile = t;
//		this.isHovering = true;
//		this.hoveredPlayingCard = -1;
	}

	public void hoverPlayingCard(int idPlayingCard)
	{
//		this.hoveredPlayingCard = idPlayingCard;
//		this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().displayHover();
//		this.currentHoveredTile = this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().tile;
//		this.isHovering = true;
	}

	public void activatePlayingCard(int idPlayingCard)
	{
//		if (this.hoveredPlayingCard != -1)
//		{
//			this.hideHoveredPlayingCard();
//		}
//		this.currentPlayingCard = idPlayingCard;
//
//		if (this.isFirstPlayer == (idPlayingCard < limitCharacterSide))
//		{
//			this.isDragging = true;
//			this.clickedPlayingCard = idPlayingCard;
//		} else
//		{
//			this.clickedOpponentPlayingCard = idPlayingCard;
//		}
//
//		this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().displayPlaying();
//		this.currentPlayingTile = this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().tile;
//		this.currentClickedTile = this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().tile;
//
//		if (this.isFirstPlayer == (idPlayingCard < limitCharacterSide))
//		{
//			this.showMyPlayingSkills(this.currentPlayingCard);
//		} else
//		{
//			this.showOpponentSkills(this.currentPlayingCard);
//		}
	}

	public void hideClickedTile()
	{
		//this.tiles [currentClickedTile.x, currentClickedTile.y].GetComponent<TileController>().hideSelected();
	}

	public void hoverPlayingCardHandler(int idPlayingCard)
	{
//		bool toHover = true;
//		bool toHide = false;
//
//		if (this.isHovering)
//		{
//			if (this.currentHoveredTile.x != this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().tile.x || this.currentHoveredTile.y != this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().tile.y)
//			{
//				toHide = true;
//			} else
//			{
//				toHover = false;
//			}
//		}
//		if (this.clickedPlayingCard != -1)
//		{
//			if (clickedPlayingCard == idPlayingCard)
//			{
//				toHover = false;
//			}
//		}
//		if (this.clickedOpponentPlayingCard != -1)
//		{
//			if (clickedOpponentPlayingCard == idPlayingCard)
//			{
//				toHover = false;
//			}
//		}
//		if (this.currentPlayingCard != -1)
//		{
//			if (currentPlayingCard == idPlayingCard)
//			{
//				toHover = false;
//			}
//		}
//
//		if (toHide)
//		{
//			if (this.hoveredPlayingCard == -1)
//			{
//				this.hideHoveredTile();
//			} else
//			{
//				this.hideHoveredPlayingCard();
//			}
//		}
//		if (toHover)
//		{
//			this.hoverPlayingCard(idPlayingCard);
//		}
	}

	public void hoverTileHandler(Tile t)
	{
//		bool toHover = true;
//		bool toHide = false;
//		if (this.isHovering)
//		{
//			if (t.x != this.currentHoveredTile.x || t.y != this.currentHoveredTile.y)
//			{
//				toHide = true;
//			} else
//			{
//				toHover = false;
//			}
//		}
//		if (this.currentPlayingCard != -1)
//		{
//
//		}
//
//		if (toHide)
//		{
//			if (this.hoveredPlayingCard == -1)
//			{
//				this.hideHoveredTile();
//			} else
//			{
//				this.hideHoveredPlayingCard();
//			}
//		}
//		if (toHover)
//		{
//			this.hoverTile(t);
//		}
	}

	public void hideClickedPlayingCard()
	{
//		this.hideMySkills();
//
//		this.playingCards [this.clickedPlayingCard].GetComponent<PlayingCardController>().hideSelected();
//		print("Je hide " + this.clickedPlayingCard);
//		this.clickedPlayingCard = -1;
	}

	public void hideOpponentClickedPlayingCard()
	{
//		this.hideOpponentSkills();
//
//		this.playingCards [this.clickedOpponentPlayingCard].GetComponent<PlayingCardController>().hideSelected();
//		this.clickedOpponentPlayingCard = -1;
	}

	public void clickPlayingCard(int idPlayingCard)
	{
//		if (this.hoveredPlayingCard != -1)
//		{
//			this.hideHoveredPlayingCard();
//		}
//		this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().displaySelected();
//		this.clickedPlayingCard = idPlayingCard;
//		this.currentClickedTile = this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().tile;
//		this.showMyPlayingSkills(this.clickedPlayingCard);
	}

	public void clickOpponentPlayingCard(int idPlayingCard)
	{
//		if (this.hoveredPlayingCard != -1)
//		{
//			this.hideHoveredPlayingCard();
//		}
//		this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().displayOpponentSelected();
//		this.clickedOpponentPlayingCard = idPlayingCard;
//		this.currentClickedTile = this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().tile;
//
//		this.showOpponentSkills(this.clickedOpponentPlayingCard);
	}
	
	public void changeZoneTargetTile(Tile t)
	{
//		int x = t.x;
//		int y = t.y;
//		for (int i = 0; i < this.boardWidth; i++)
//		{
//			for (int j = 0; j < this.boardHeight; j++)
//			{
//				this.tiles [i, j].GetComponent<TileController>().removeHalo();
//			}
//		}
//		
//		if (x > 0)
//		{
//			if (y > 0)
//			{
//				this.tiles [x - 1, y - 1].GetComponent<TileController>().activateEffectZoneHalo();
//			}
//			if (y < this.boardHeight - 1)
//			{
//				this.tiles [x - 1, y + 1].GetComponent<TileController>().activateEffectZoneHalo();
//			}
//			this.tiles [x - 1, y].GetComponent<TileController>().activateEffectZoneHalo();
//		}
//		if (x < this.boardWidth - 1)
//		{
//			if (y > 0)
//			{
//				this.tiles [x + 1, y - 1].GetComponent<TileController>().activateEffectZoneHalo();
//			}
//			if (y < this.boardHeight - 1)
//			{
//				this.tiles [x + 1, y + 1].GetComponent<TileController>().activateEffectZoneHalo();
//			}
//			this.tiles [x + 1, y].GetComponent<TileController>().activateEffectZoneHalo();
//		}
//		if (y > 0)
//		{
//			this.tiles [x, y - 1].GetComponent<TileController>().activateEffectZoneHalo();
//		}
//		if (y < this.boardHeight - 1)
//		{
//			this.tiles [x, y + 1].GetComponent<TileController>().activateEffectZoneHalo();
//		}
//		this.tiles [x, y].GetComponent<TileController>().activateEffectZoneHalo();
	}
	
	public void displayAdjacentOpponentsTargets()
	{
//		List<Tile> neighbourTiles = this.getCurrentPCC().tile.getImmediateNeighbouringTiles();
//		int playerID;
//		foreach (Tile t in neighbourTiles)
//		{
//			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().characterID;
//			if (playerID != -1)
//			{
//				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getPCC(playerID).isMine)
//				{
//					this.playingCards [playerID].GetComponent<PlayingCardController>().setTargetHalo(this.gameskills [this.getCurrentSkillID()].getTargetPCCText(this.getPCC(playerID).card));
//				}
//			}
//		}
	}
	
	public bool canLaunchAdjacentOpponents()
	{
//		bool isLaunchable = false;
//		
//		List<Tile> neighbourTiles = this.getCurrentPCC().tile.getImmediateNeighbouringTiles();
//		int playerID;
//		foreach (Tile t in neighbourTiles)
//		{
//			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().characterID;
//			if (playerID != -1)
//			{
//				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getPCC(playerID).isMine)
//				{
//					isLaunchable = true;
//				}
//			}
//		}
//		return isLaunchable;
	return true ;
	}
	
	public void displayAdjacentAllyTargets()
	{
//		List<Tile> neighbourTiles = this.getCurrentPCC().tile.getImmediateNeighbouringTiles();
//		int playerID;
//		foreach (Tile t in neighbourTiles)
//		{
//			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().characterID;
//			if (playerID != -1)
//			{
//				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && this.getPCC(playerID).isMine)
//				{
//					this.playingCards [playerID].GetComponent<PlayingCardController>().setTargetHalo(this.gameskills [this.getCurrentSkillID()].getTargetPCCText(this.getPCC(playerID).card));
//				}
//			}
//		}
	}
	
	public bool canLaunchAdjacentAllys()
	{
		bool isLaunchable = false;
		
//		List<Tile> neighbourTiles = this.getCurrentPCC().tile.getImmediateNeighbouringTiles();
//		int playerID;
//		foreach (Tile t in neighbourTiles)
//		{
//			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().characterID;
//			if (playerID != -1)
//			{
//				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && this.getPCC(playerID).isMine)
//				{
//					isLaunchable = true;
//				}
//			}
//		}
		return isLaunchable;
	}
	
	public void displayAdjacentTileTargets()
	{
		List<Tile> neighbourTiles = this.getCurrentPCC().tile.getImmediateNeighbouringTiles();
		int playerID;
//		foreach (Tile t in neighbourTiles)
//		{
//			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().characterID;
//			if (playerID == -1)
//			{
//				if (!this.tiles [t.x, t.y].GetComponent<TileController>().tile.isStatModifier && this.tiles [t.x, t.y].GetComponent<TileController>().type == 0)
//				{
//					this.tiles [t.x, t.y].GetComponent<TileController>().setTargetHalo(this.gameskills [this.getCurrentSkillID()].getTargetPCCText(new Card()));
//				}
//			}
//		}
	}
	
	public bool canLaunchAdjacentTiles()
	{
		bool isLaunchable = false;
		List<Tile> neighbourTiles = this.getCurrentPCC().tile.getImmediateNeighbouringTiles();
		int playerID;
//		foreach (Tile t in neighbourTiles)
//		{
//			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().characterID;
//			if (playerID == -1)
//			{
//				if (!this.tiles [t.x, t.y].GetComponent<TileController>().tile.isStatModifier && this.tiles [t.x, t.y].GetComponent<TileController>().type == 0)
//				{
//					isLaunchable = true;
//				}
//			}
//		}
		return isLaunchable;
	}
	
	public void displayAllTargets()
	{
//		PlayingCardController pcc;
//		for (int i = 0; i < this.playingCards.Length; i++)
//		{
//			pcc = this.getPCC(i);
//			if (pcc.canBeTargeted())
//			{
//				pcc.setTargetHalo(this.gameskills [this.getCurrentSkillID()].getTargetPCCText(pcc.card));
//			}
//		}		
	}
	
	public void displayAllButMeModifiersTargets()
	{
//		PlayingCardController pcc;
//		for (int i = 0; i < this.playingCards.Length; i++)
//		{
//			pcc = this.getPCC(i);
//			if (pcc.card.hasModifiers() && pcc.canBeTargeted() && i != this.currentPlayingCard)
//			{
//				pcc.setTargetHalo(this.gameskills [this.getCurrentSkillID()].getTargetPCCText(pcc.card));
//			}
//		}		
	}
	
	public bool canLaunchAllButMeModifiers()
	{
		bool isLaunchable = false;
		PlayingCardController pcc;
//		for (int i = 0; i < this.playingCards.Length; i++)
//		{
//			pcc = this.getPCC(i);
//			if (pcc.card.hasModifiers() && pcc.canBeTargeted() && i != this.currentPlayingCard)
//			{
//				isLaunchable = true;
//			}
//		}		
		return isLaunchable;
	}
	
	public void displayAllButMeTargets()
	{
		PlayingCardController pcc;
//		for (int i = 0; i < this.playingCards.Length; i++)
//		{
//			pcc = this.getPCC(i);
//			if (pcc.canBeTargeted() && i != this.currentPlayingCard)
//			{
//				pcc.setTargetHalo(this.gameskills [this.getCurrentSkillID()].getTargetPCCText(pcc.card));
//			}
//		}		
	}
	
	public bool canLaunchAllButMe()
	{
		bool isLaunchable = false;
		PlayingCardController pcc;
//		for (int i = 0; i < this.playingCards.Length; i++)
//		{
//			pcc = this.getPCC(i);
//			if (pcc.canBeTargeted() && i != this.currentPlayingCard)
//			{
//				isLaunchable = true;
//			}
//		}		
		return isLaunchable;
	}
	
	public void displayAllysButMeTargets()
	{
		PlayingCardController pcc;
		
//		for (int i = 0; i < this.playingCards.Length; i++)
//		{
//			pcc = this.getPCC(i);
//			if (pcc.isMine && pcc.canBeTargeted() && i != this.currentPlayingCard)
//			{
//				pcc.setTargetHalo(this.gameskills [this.getCurrentSkillID()].getTargetPCCText(pcc.card));
//			}
//		}		
	}
	
	public bool canLaunchAllysButMe()
	{
		PlayingCardController pcc;
		bool isLaunchable = false;
		
//		for (int i = 0; i < this.playingCards.Length && !isLaunchable; i++)
//		{
//			pcc = this.getPCC(i);
//			if (pcc.isMine && pcc.canBeTargeted() && i != this.currentPlayingCard)
//			{
//				isLaunchable = true;
//			}
//		}
		return isLaunchable; 	
	}
	
	public void displayOpponentsTargets()
	{
		PlayingCardController pcc;
		
//		for (int i = 0; i < this.playingCards.Length; i++)
//		{
//			pcc = this.getPCC(i);
//			if (!pcc.isMine && pcc.canBeTargeted())
//			{
//				pcc.setTargetHalo(this.gameskills [this.getCurrentSkillID()].getTargetPCCText(pcc.card));
//			}
//		}		
	}
	
	public bool canLaunchOpponents()
	{
		PlayingCardController pcc;
		bool isLaunchable = false;
		
//		for (int i = 0; i < this.playingCards.Length && !isLaunchable; i++)
//		{
//			pcc = this.getPCC(i);
//			if (!pcc.isMine && pcc.canBeTargeted())
//			{
//				isLaunchable = true;
//			}
//		}
		return isLaunchable; 	
	}
	
	public void displayMyControls(string s)
	{
		this.getCurrentPCC().setSkillControlSkillHandler(s);
	}
	
	public void hidePCCTargets()
	{
//		for (int i = 0; i < this.playingCards.Length; i++)
//		{
//			this.playingCards [i].GetComponent<PlayingCardController>().hideTargetHalo();
//		}
	}
	
	public void hideTileTargets()
	{
//		for (int i = 0; i < this.boardWidth; i++)
//		{
//			for (int j = 0; j < this.boardHeight; j++)
//			{	
//				this.tiles [i, j].GetComponent<TileController>().hideTargetHalo();
//			}
//		}
	}
	
	void initDeadsList()
	{
//		List<PlayingCardController> tempList = new List<PlayingCardController>();
//
//		foreach (GameObject g in playingCards)
//		{
//			PlayingCardController pcc = g.GetComponent<PlayingCardController>();
//			if (pcc.isDead)
//			{
//				tempList.Add(pcc);
//			}
//		}
//
//		GameObject go;
//		int i = 0;
//
//		foreach (PlayingCardController pcc in tempList)
//		{
//			go = (GameObject)Instantiate(this.playingCard);
//			PlayingCardController deadPcc = go.GetComponent<PlayingCardController>();
//
//			Camera camera = Camera.main;
//			Vector3 currentScale = go.renderer.bounds.size;
//			Vector3 v3 = new Vector3((0.02f + tileScale) * (boardWidth / 2 + 0.5f), 0, -1f);
//			
//			go.transform.position = v3;
//			go.tag = "deadCharacter";
//			go.transform.Translate((-transform.up * transform.localScale.y) * (-3.5f + i), Space.World);
//
//			deadPcc.setStyles(((pcc.IDCharacter < limitCharacterSide) == this.isFirstPlayer));
//			deadPcc.setCard(pcc.card);
//			deadPcc.setIDCharacter(pcc.IDCharacter);
//			
//			deadPcc.resizeDeadHalo(transform.localScale, i);
//			deadPcc.show();
//			
//			deads.Add(deadPcc);
//
//			i++;
//		}
	}

	public void deactivateMySkills()
	{
		for (int i = 0; i < 6; i++)
		{
			//this.skillsObjects [i].GetComponent<SkillObjectController>().setControlActive(false);
		}
	}

	public void cancelSkill()
	{
//		this.isRunningSkill = false;
//		this.playindCardHasPlayed = false;
//		for (int i = 0; i < playingCards.Length; i++)
//		{
//			this.getPCC(i).removeTargetHalo();	
//		}
//		for (int i = 0; i < this.boardWidth; i++)
//		{
//			for (int j = 0; j < this.boardHeight; j++)
//			{
//				this.tiles [i, j].GetComponent<TileController>().removePotentielTarget();
//				this.tiles [i, j].GetComponent<TileController>().removeHalo();
//			}
//		}
//		this.showMyPlayingSkills(this.currentPlayingCard);
	}

	public void hideActivatedPlayingCard()
	{
//		if (this.isFirstPlayer == (this.currentPlayingCard < limitCharacterSide))
//		{
//			this.hideMySkills();
//		} else
//		{
//			this.hideOpponentSkills();
//		}
//
//		this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().hidePlaying();
//		if (this.currentPlayingCard == this.clickedPlayingCard)
//		{
//			this.clickedPlayingCard = -1;
//		}
//		this.currentPlayingCard = -1;
//		this.isDragging = false;
	}

	public void showMyPlayingSkills(int idc)
	{
//		this.selectedPlayingCard.GetComponent<PlayingCardController>().setCard(this.playingCards [idc].GetComponent<PlayingCardController>().card);
//		this.selectedPlayingCard.GetComponent<PlayingCardController>().show();
//		this.selectedPlayingCard.GetComponent<PlayingCardController>().setActive(true);
//
//		List<Skill> skills = this.playingCards [idc].GetComponent<PlayingCardController>().card.Skills;
//		for (int i = 0; i < 4; i++)
//		{
//			if (i < skills.Count)
//			{
//				this.skillsObjects [i].GetComponent<SkillObjectController>().setSkill(skills [i]);
//				this.skillsObjects [i].GetComponent<SkillObjectController>().setActive(true);
//			} else
//			{
//				this.skillsObjects [i].GetComponent<SkillObjectController>().setActive(false);
//			}
//		}
//		
//		if (nbTurns != 0)
//		{
//			this.updateStatusMySkills(idc);
//		}
//		
//		bool isActive = !(nbTurns == 0) && (idc == this.currentPlayingCard);
//		this.skillsObjects [4].GetComponent<SkillObjectController>().setActive(isActive);
//		this.skillsObjects [5].GetComponent<SkillObjectController>().setActive(isActive);
		
	}
		
	public void updateStatusMySkills(int idc)
	{
//		bool controlActive;
//		bool isActive = !(nbTurns == 0) && (idc == this.currentPlayingCard);
//		List<Skill> skills = this.playingCards [idc].GetComponent<PlayingCardController>().card.Skills;
//		for (int i = 0; i < 4; i++)
//		{
//			if (i < skills.Count)
//			{
//				this.skillsObjects [i].GetComponent<SkillObjectController>().setActiveStatus(isActive);
//				controlActive = this.gameskills [skills [i].Id].isLaunchable(skills [i]) && !this.playindCardHasPlayed && !this.isRunningSkill;
//				this.skillsObjects [i].GetComponent<SkillObjectController>().setControlStatus(controlActive);
//				this.skillsObjects [i].GetComponent<SkillObjectController>().show();
//			}
//		}
//		this.skillsObjects [4].GetComponent<SkillObjectController>().setActive(isActive);
//		if (isActive)
//		{
//			this.skillsObjects [4].GetComponent<SkillObjectController>().setAttackValue(this.playingCards [idc].GetComponent<PlayingCardController>().card.GetAttack());
//		}
//		this.skillsObjects [4].GetComponent<SkillObjectController>().setActiveStatus(isActive);
//		controlActive = this.gameskills [0].isLaunchable(new Skill()) && !this.playindCardHasPlayed && !this.isRunningSkill;
//		this.skillsObjects [4].GetComponent<SkillObjectController>().setControlStatus(controlActive);
//		this.skillsObjects [4].GetComponent<SkillObjectController>().show();
//		this.skillsObjects [5].GetComponent<SkillObjectController>().setActive(isActive);
//		this.skillsObjects [5].GetComponent<SkillObjectController>().setActiveStatus(isActive);
//		this.skillsObjects [5].GetComponent<SkillObjectController>().setControlStatus(true);
//		this.skillsObjects [5].GetComponent<SkillObjectController>().show();
	}

	public void showOpponentSkills(int idc)
	{
//		this.selectedOpponentCard.GetComponent<PlayingCardController>().setCard(this.playingCards [idc].GetComponent<PlayingCardController>().card);
//		this.selectedOpponentCard.GetComponent<PlayingCardController>().show();
//		this.selectedOpponentCard.GetComponent<PlayingCardController>().setActive(true);
//		
//		List<Skill> skills = this.playingCards [idc].GetComponent<PlayingCardController>().card.Skills;
//		for (int i = 0; i < 4; i++)
//		{
//			if (i < skills.Count)
//			{
//				this.opponentSkillsObjects [i].GetComponent<SkillObjectController>().setSkill(skills [i]);
//				this.opponentSkillsObjects [i].SetActive(true);
//			} else
//			{
//				this.opponentSkillsObjects [i].SetActive(false);
//			}
//		}
//		this.opponentSkillsObjects [4].SetActive(false);
//		this.opponentSkillsObjects [5].SetActive(false);
	}

	public void hideMySkills()
	{
//		this.selectedPlayingCard.SetActive(false);
//		for (int i = 0; i < 6; i++)
//		{
//			this.skillsObjects [i].SetActive(false);
//		}
	}

	public void hideOpponentSkills()
	{
//		for (int i = 0; i < 6; i++)
//		{
//			this.selectedOpponentCard.SetActive(false);
//			this.opponentSkillsObjects [i].SetActive(false);
//		}
	}

	public void clickPlayingCardHandler(int idPlayingCard)
	{
//		bool toClick = false;
//		bool toClickOpponent = false;
//		bool toHideClick = false;
//		bool toHideOpponentClick = false;
//		bool toHidePlay = false;
//		bool toPlay = false;
//
//
//		if (idPlayingCard == this.currentPlayingCard)
//		{
//			if (this.nbTurns == 0)
//			{
//				toHidePlay = true;
//			} else if (this.isFirstPlayer == (idPlayingCard < limitCharacterSide))
//			{
//				if (this.clickedPlayingCard != idPlayingCard)
//				{
//					this.hideClickedPlayingCard();
//					this.clickedPlayingCard = idPlayingCard;
//					this.showMyPlayingSkills(idPlayingCard);
//				}
//			} else
//			{
//				if (this.clickedOpponentPlayingCard != idPlayingCard)
//				{
//					this.hideOpponentClickedPlayingCard();
//					this.clickedOpponentPlayingCard = idPlayingCard;
//					this.showOpponentSkills(idPlayingCard);
//				}
//			}
//		} else if (idPlayingCard == this.clickedPlayingCard)
//		{
//			toHideClick = true;
//		} else if (idPlayingCard == this.clickedOpponentPlayingCard)
//		{
//			toHideOpponentClick = true;
//		} else
//		{
//			if (this.nbTurns == 0)
//			{
//				if (this.isFirstPlayer == (idPlayingCard < limitCharacterSide))
//				{
//					if (this.currentPlayingCard != -1)
//					{
//						toHidePlay = true;
//					}
//					toPlay = true;
//				} else
//				{
//					if (clickedOpponentPlayingCard != -1)
//					{
//						toHideOpponentClick = true;
//					}
//					toClickOpponent = true;
//				}
//			} else
//			{
//				if (this.isFirstPlayer == (idPlayingCard < limitCharacterSide))
//				{
//					if (this.clickedPlayingCard != -1 && this.clickedPlayingCard != this.currentPlayingCard)
//					{
//						toHideClick = true;
//					}
//					toClick = true;
//				} else
//				{
//					if (clickedOpponentPlayingCard != -1 && this.clickedOpponentPlayingCard != this.currentPlayingCard)
//					{
//						toHideOpponentClick = true;
//					}
//					toClickOpponent = true;
//				}
//			}
//		}
//	
//		if (toHideClick)
//		{
//			this.hideClickedPlayingCard();
//		}
//		if (toHideOpponentClick)
//		{
//			this.hideOpponentClickedPlayingCard();
//		}
//		if (toHidePlay)
//		{
//			this.hideActivatedPlayingCard();
//		}
//		if (toClick)
//		{
//			this.clickPlayingCard(idPlayingCard);
//		}
//		if (toClickOpponent)
//		{
//			this.clickOpponentPlayingCard(idPlayingCard);
//		}
//		if (toPlay)
//		{
//			this.activatePlayingCard(idPlayingCard);
//		}
//		if (this.isTutorialLaunched)
//		{
//			this.tutorial.GetComponent<TutorialObjectController>().actionIsDone();
//		}
//		bool toClick = false;
//		bool toClickOpponent = false;
//		bool toHideClick = false;
//		bool toHideOpponentClick = false;
//		bool toHidePlay = false;
//		bool toPlay = false;
//
//
//		if (idPlayingCard == this.currentPlayingCard)
//		{
//			if (this.nbTurns == 0)
//			{
//				toHidePlay = true;
//			} else if (this.isFirstPlayer == (idPlayingCard < limitCharacterSide))
//			{
//				if (this.clickedPlayingCard != idPlayingCard)
//				{
//					this.hideClickedPlayingCard();
//					this.clickedPlayingCard = idPlayingCard;
//					this.showMyPlayingSkills(idPlayingCard);
//				}
//			} else
//			{
//				if (this.clickedOpponentPlayingCard != idPlayingCard)
//				{
//					this.hideOpponentClickedPlayingCard();
//					this.clickedOpponentPlayingCard = idPlayingCard;
//					this.showOpponentSkills(idPlayingCard);
//				}
//			}
//		} else if (idPlayingCard == this.clickedPlayingCard)
//		{
//			toHideClick = true;
//		} else if (idPlayingCard == this.clickedOpponentPlayingCard)
//		{
//			toHideOpponentClick = true;
//		} else
//		{
//			if (this.nbTurns == 0)
//			{
//				if (this.isFirstPlayer == (idPlayingCard < limitCharacterSide))
//				{
//					if (this.currentPlayingCard != -1)
//					{
//						toHidePlay = true;
//					}
//					toPlay = true;
//				} else
//				{
//					if (clickedOpponentPlayingCard != -1)
//					{
//						toHideOpponentClick = true;
//					}
//					toClickOpponent = true;
//				}
//			} else
//			{
//				if (this.isFirstPlayer == (idPlayingCard < limitCharacterSide))
//				{
//					if (this.clickedPlayingCard != -1 && this.clickedPlayingCard != this.currentPlayingCard)
//					{
//						toHideClick = true;
//					}
//					toClick = true;
//				} else
//				{
//					if (clickedOpponentPlayingCard != -1 && this.clickedOpponentPlayingCard != this.currentPlayingCard)
//					{
//						toHideOpponentClick = true;
//					}
//					toClickOpponent = true;
//				}
//			}
//		}
//	
//		if (toHideClick)
//		{
//			this.hideClickedPlayingCard();
//		}
//		if (toHideOpponentClick)
//		{
//			this.hideOpponentClickedPlayingCard();
//		}
//		if (toHidePlay)
//		{
//			this.hideActivatedPlayingCard();
//		}
//		if (toClick)
//		{
//			this.clickPlayingCard(idPlayingCard);
//		}
//		if (toClickOpponent)
//		{
//			this.clickOpponentPlayingCard(idPlayingCard);
//		}
//		if (toPlay)
//		{
//			this.activatePlayingCard(idPlayingCard);
//		}
//		if (this.isTutorialLaunched)
//		{
//			this.tutorial.GetComponent<GameTutorialController>().actionIsDone();
//		}
	}

	public void releaseClickPlayingCardHandler(int idPlayingCard)
	{
//		if (!this.isRunningSkill && isDragging)
//		{
//			if (this.isHovering && this.tiles [currentHoveredTile.x, currentHoveredTile.y].GetComponentInChildren<TileController>().characterID == -1 && this.tiles [currentHoveredTile.x, currentHoveredTile.y].GetComponentInChildren<TileController>().isDestination)
//			{
//				int x = currentHoveredTile.x;
//				int y = currentHoveredTile.y;
//				this.hideHoveredTile();
//				photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, x, y, this.currentPlayingCard, this.isFirstPlayer, false);
//			}
//		}
	}

	public void releaseClickTileHandler(Tile t)
	{
//		if (isDragging && !this.isRunningSkill)
//		{
//			if (this.tiles [t.x, t.y].GetComponentInChildren<TileController>().isDestination && this.tiles [currentHoveredTile.x, currentHoveredTile.y].GetComponentInChildren<TileController>().characterID == -1)
//			{
//				int x = currentHoveredTile.x;
//				int y = currentHoveredTile.y;
//				this.hideHoveredTile();
//				photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, x, y, this.currentPlayingCard, this.isFirstPlayer, false);
//				if (isTutorialLaunched)
//				{
//					this.tutorial.GetComponent<GameTutorialController>().actionIsDone();
//				}
//			}
//		}
	}

	public void clickSkillHandler(int ids)
	{
//		this.updateStatusMySkills(this.currentPlayingCard);
//		//this.clickedSkill = ids;
//		if (ids > 3)
//		{
//			if (ids == 4)
//			{
//				this.gameskills [0].launch();
//			} else
//			{
//				this.gameskills [1].launch();
//			}
//		} else
//		{
//			int idSkill = this.playingCards [this.currentPlayingCard].GetComponentInChildren<PlayingCardController>().card.Skills [ids].Id;
//			this.gameskills [idSkill].launch();
//		}
//		
//		if (isTutorialLaunched)
//		{
//			this.tutorial.GetComponent<GameTutorialController>().actionIsDone();
//		}
	}

	public void findNextPlayer()
	{
//		if (this.currentPlayingCard != -1)
//		{
//			this.playingCards [this.currentPlayingCard].GetComponentInChildren<PlayingCardController>().hasPlayed = true;
//		}
//		bool newTurn = true;
//		int nextPlayingCard = -1;
//		int i = 0;
//
//		while (i < playingCards.Length && newTurn == true)
//		{
//			if (!this.playingCards [rankedPlayingCardsID [i]].GetComponentInChildren<PlayingCardController>().hasPlayed)
//			{
//				nextPlayingCard = rankedPlayingCardsID [i];
//				newTurn = false;
//			}
//			i++;
//		}
//
//		if (newTurn)
//		{
//			for (i = 0; i < playingCards.Length; i++)
//			{
//				if (!this.playingCards [i].GetComponentInChildren<PlayingCardController>().isDead)
//				{
//					this.playingCards [i].GetComponentInChildren<PlayingCardController>().hasPlayed = false;
//				}
//			}
//			int j = 0;
//			while (j < playingCards.Length)
//			{
//				if (!this.playingCards [rankedPlayingCardsID [j]].GetComponentInChildren<PlayingCardController>().hasPlayed)
//				{
//					nextPlayingCard = rankedPlayingCardsID [j];
//					j = playingCards.Length + 1;
//				}
//				j++;
//			}
//		}
//		
//		photonView.RPC("initPlayer", PhotonTargets.AllBuffered, nextPlayingCard, newTurn, this.isFirstPlayer);
	}

	[RPC]
	public void initPlayer(int id, bool newTurn, bool isFirstP)
	{
//		print("Au tour de " + id);
//		if (newTurn)
//		{
//			for (int i = 0; i < playingCards.Length; i++)
//			{
//				if (!this.playingCards [i].GetComponentInChildren<PlayingCardController>().isDead)
//				{
//					this.playingCards [i].GetComponentInChildren<PlayingCardController>().hasPlayed = false;
//					this.reloadCard(i);
//					//reloadDestinationTiles();
//				}
//			}
//		}
//		if (this.currentPlayingCard != -1 && newTurn == false)
//		{
//			this.playingCards [this.currentPlayingCard].GetComponentInChildren<PlayingCardController>().hasPlayed = true;
//		}
//
//		if (this.currentPlayingCard != -1)
//		{
//			this.hideActivatedPlayingCard();
//		}
//		this.resetDestinations();
//
//		if (newTurn)
//		{
//			this.nbTurns++;
//		}
//
//		this.currentPlayingCard = id;
//		
//		if (this.getCurrentCard().isSleeping())
//		{
//			if ((this.currentPlayingCard < limitCharacterSide) == this.isFirstPlayer)
//			{
//				int percentage = this.getCurrentCard().getSleepingPercentage();
//				if (UnityEngine.Random.Range(1, 101) <= percentage)
//				{
//					this.wakeUp();
//				} else
//				{
//					this.displaySkillEffect(this.currentPlayingCard, "Ne se réveille pas", 3, 1);
//				}
//			}
//			//this.playindCardHasPlayed = true;
//			//this.playingCardHasMoved = true;
//		}
//		if (this.getCurrentCard().isParalyzed())
//		{
//			//this.playindCardHasPlayed = true;
//			//this.displayPopUpMessage(this.getCurrentCard().Title + " est paralysé", 3f);
//		} else
//		{
//			//this.playindCardHasPlayed = false;
//		}
//		
//		this.getCurrentPCC().activateSleepingModifiers();
//		
//		//this.playingCardHasMoved = false;
//
//		//this.currentPlayingTile = this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().tile;
//
//		this.activatePlayingCard(id);
//
//		if (this.isFirstPlayer == (id < limitCharacterSide))
//		{
//			this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().tile.setNeighbours(this.getCharacterTilesArray(), this.playingCards [id].GetComponentInChildren<PlayingCardController>().card.GetMove());
//			this.setDestinations(currentPlayingCard);
//			//this.isDragging = true;
//		}
//		//this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().card.changeModifiers();
//		if (newTurn)
//		{
//			foreach (GameObject pc in playingCards)
//			{
//				PlayingCardController temp = pc.GetComponent<PlayingCardController>();
//				loadTileModifierToCharacter(temp.tile.x, temp.tile.y, true);	
//			}
//		}
//
//		this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().show();
//
//		if ((currentPlayingCard < limitCharacterSide && this.isFirstPlayer) || (currentPlayingCard >= limitCharacterSide && !this.isFirstPlayer))
//		{
//			//displayPopUpMessage("A votre tour de jouer", 3f);
//			this.showMyPlayingSkills(this.currentPlayingCard);
//		} else
//		{
//			//displayPopUpMessage("Tour du joueur adverse", 3f);
//			this.showOpponentSkills(this.currentPlayingCard);
//		}
//		Card currentCard = getCurrentCard();
//		//leftStats.setCharacterStat(currentCard.Title, currentCard.GetMove(), currentCard.GetLife(), currentCard.GetAttack());
//		//leftStats.setSkill(currentCard.Skills [0].Name, currentCard.Skills [0].Description, currentCard.Skills [0].Id - 2);
	}

	public int[,] getCharacterTilesArray()
	{
//		int width = GameView.instance.boardWidth;
//		int height = GameView.instance.boardHeight;
//		int[,] characterTiles = new int[width, height];
		int[,] characterTiles = new int[1, 1];
//		for (int i = 0; i < width; i ++)
//		{
//			for (int j = 0; j < height; j ++)
//			{
//				if (this.getTile(i, j).type == 1)
//				{
//					characterTiles [i, j] = 9;
//				} else
//				{
//					characterTiles [i, j] = -1;
//				}	
//			}
//		}
//		int debut;
//		int fin;
//		if (this.isFirstPlayer)
//		{
//			debut = limitCharacterSide;
//			fin = playingCards.Length;
//		} else
//		{
//			debut = 0;
//			fin = limitCharacterSide;
//		}
//		for (int i = debut; i < fin; i++)
//		{
//			if (!this.playingCards [i].GetComponentInChildren<PlayingCardController>().isDead)
//			{
//				Tile tiletemp = this.playingCards [i].GetComponentInChildren<PlayingCardController>().tile;
//				characterTiles [tiletemp.x, tiletemp.y] = 8;
//			}
//		}
//
		return characterTiles;
	}

	public void setDestinations(int idPlayer)
	{
//		List<Tile> nt = this.playingCards [idPlayer].GetComponentInChildren<PlayingCardController>().tile.neighbours.tiles;
//		foreach (Tile t in nt)
//		{
//			if (!this.isTutorialLaunched)
//			{
//				this.tiles [t.x, t.y].GetComponentInChildren<TileController>().setDestination(true);
//			} else
//			{
//				this.tiles [t.x, t.y].GetComponent<TileController>().setGrey(true);
//				this.tiles [t.x, t.y].GetComponent<TileController>().setGreyBorder(true);
//			}
//		}
		//this.isDestinationDrawn = true;
	}

	public void resolvePass()
	{
		this.getCurrentPCC().hideControlSkillHandler();
		this.hidePCCTargets();
		this.hideTileTargets();
		photonView.RPC("checkModyfiersRPC", PhotonTargets.AllBuffered);
		
		findNextPlayer();
		photonView.RPC("timeRunsOut", PhotonTargets.AllBuffered, this.timePerTurn);
		photonView.RPC("addPassEvent", PhotonTargets.AllBuffered);
		photonView.RPC("loadTileModifierToCharacter", PhotonTargets.AllBuffered, getCurrentPCC().tile.x, getCurrentPCC().tile.y, false);
	}
	
	public void wakeUp()
	{
		photonView.RPC("wakeUpRPC", PhotonTargets.AllBuffered);
	}
	
	[RPC]
	public void wakeUpRPC()
	{
		//this.playindCardHasPlayed = false;
		//this.playingCardHasMoved = false;
		this.displaySkillEffect(this.currentPlayingCard, "Se réveille", 3, 2);
		this.getCurrentCard().removeSleeping();
		this.getCurrentPCC().show();
	}
	
	[RPC]
	public void checkModyfiersRPC()
	{
		this.getCurrentPCC().checkModyfiers();
	}

	[RPC]
	public void addPassEvent()
	{
		GameEventType ge = new PassType();
		addGameEvent(ge, "");
		//nbActionPlayed = 0;
		changeGameEvents();
		fillTimeline();
	}
	
	private IEnumerator returnToLobby()
	{
//		if (gameView.MyPlayerNumber == 1)
//		{
//			yield return new WaitForSeconds(5);
//		} 
//		else
//		{
//			yield return new WaitForSeconds(7);
//		}
//		PhotonNetwork.Disconnect();
		yield break;
	}
	
	IEnumerator sendStat(string user1, string user2)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick1", user1); 	                    // Pseudo de l'utilisateur victorieux
		form.AddField("myform_nick2", user2); 	                    // Pseudo de l'autre utilisateur
		form.AddField("myform_gametype", ApplicationModel.gameType);

		WWW w = new WWW(URLStat, form); 							// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null)
		{
			print(w.error); 										// donne l'erreur eventuelle
		} else
		{
			//print(w.text);
		}
		yield break;
	}

	void initGrid()
	{
		print("J'initialise le terrain de jeu");
		bool isRock = false;
		List<Tile> rocks = new List<Tile>();
		Tile t = new Tile(0,0) ;
		if (!this.isTutorialLaunched)
		{
			int nbRocksToAdd = UnityEngine.Random.Range(3, 6);
			int compteurRocks = 0;
			bool isOk = true;
			while (compteurRocks<nbRocksToAdd)
			{
				isOk = false;
				while (!isOk)
				{
					t = GameView.instance.getRandomRock(1);
					isOk = true;
					for (int a = 0; a < rocks.Count && isOk; a++)
					{
						if (rocks [a].x == t.x && rocks [a].y == t.y)
						{
							isOk = false;
						}
					}
				}
				rocks.Add(t);
				compteurRocks++;
			}
		} else
		{
			rocks.Add(new Tile(2, 3));
			rocks.Add(new Tile(3, 5));
			rocks.Add(new Tile(4, 3));
		}
		
		int w = GameView.instance.getBoardWidth();
		int h = GameView.instance.getBoardHeight();
		for (int x = 0; x < w; x++)
		{
			for (int y = 0; y < h; y++)
			{
				isRock = false;
				for (int z = 0; z < rocks.Count && !isRock; z++)
				{
					if (rocks [z].x == x && rocks [z].y == y)
					{
						isRock = true;
					}
				}
				if (!isRock)
				{
					photonView.RPC("AddTileToBoard", PhotonTargets.AllBuffered, x, y, 0);	
				}
				else{
					photonView.RPC("AddTileToBoard", PhotonTargets.AllBuffered, x, y, 1);
				}
			}
		}
	}

	public IEnumerator loadMyDeck(bool isFirstPlayer)
	{
		Deck myDeck = new Deck(ApplicationModel.username);
		yield return StartCoroutine(myDeck.LoadDeck());

		photonView.RPC("SpawnCharacter", PhotonTargets.AllBuffered, isFirstPlayer, myDeck.Id);
	}
	
	public IEnumerator loadTutorialDeck(bool isFirstPlayer, string name)
	{
		Deck tutorialDeck = new Deck(name);
		yield return StartCoroutine(tutorialDeck.LoadDeck());

		photonView.RPC("SpawnCharacter", PhotonTargets.AllBuffered, isFirstPlayer, tutorialDeck.Id);
	}
	
	[RPC]
	void AddPlayerToList(int id, string loginName)
	{
		print(loginName+" se connecte");

		if (ApplicationModel.username == loginName)
		{
			GameView.instance.setMyPlayerName(loginName);
		} else
		{
			GameView.instance.setHisPlayerName(loginName);
		}
		
		this.nbPlayers++;
		

		
//		if (ApplicationModel.username == loginName)
//		{
//			if (this.isFirstPlayer && !this.isTutorialLaunched)
//			{
//				for (int i = 0; i < this.nbFreeStartRows; i++)
//				{
//					for (int j = 0; j < this.boardWidth; j++)
//					{
//						this.tiles [j, i].GetComponent<TileController>().setDestination(true);
//					}
//				}
//			} else if (this.isTutorialLaunched)
//			{
//				for (int i = 0; i < this.nbFreeStartRows; i++)
//				{
//					for (int j = 0; j < this.boardWidth; j++)
//					{
//						this.tiles [j, i].GetComponent<TileController>().setGrey(true);
//						this.tiles [j, i].GetComponent<TileController>().setGreyBorder(true);
//					}
//				}
//				this.tutorial.GetComponent<GameTutorialController>().setNextButtonDisplaying(true);
//			} else
//			{
//				for (int i = this.boardHeight-1; i > this.boardHeight-1-this.nbFreeStartRows; i--)
//				{
//					for (int j = 0; j < this.boardWidth; j++)
//					{
//						this.tiles [j, i].GetComponent<TileController>().setDestination(true);
//					}
//				}
//			}
//		}
	}
				
	public void resetDestinations()
	{
//		if (isDestinationDrawn)
//		{
//			for (int i = 0; i < this.boardHeight; i++)
//			{
//				for (int j = 0; j < this.boardWidth; j++)
//				{
//					this.tiles [j, i].GetComponent<TileController>().setDestination(false);
//				}
//			}
//			this.isDestinationDrawn = false;
//		}
	}

	[RPC]
	void AddTileToBoard(int x, int y, int type)
	{
		GameView.instance.createTile(x, y, type);
	}

	[RPC]
	IEnumerator SpawnCharacter(bool isFirstP, int idDeck)
	{
//		Deck deck;
//		deck = new Deck(idDeck);
//		yield return StartCoroutine(deck.RetrieveCards());
//		int debut;
//		int hauteur;
//		int compteur = 0;
//
//		if (isFirstP)
//		{
//			debut = 0;
//			hauteur = 0;
//		} else
//		{
//			debut = this.limitCharacterSide;
//			hauteur = 7;
//		}
//		if (isFirstP == this.isFirstPlayer)
//		{
//			//this.myDeck = deck;
//		}
//		//a changer
//		for (int i = 0; i < ApplicationModel.nbCardsByDeck; i++)
//		{
//			//this.playingCards [debut + deck.Cards [i].deckOrder] = (GameObject)Instantiate(this.playingCard);
//			if (isFirstP != this.isFirstPlayer)
//			{
//				this.playingCards [debut + deck.Cards [i].deckOrder].GetComponentInChildren<PlayingCardController>().hideDisplay();
//			} else
//			{
//				
//			}
//			this.playingCards [debut + deck.Cards [i].deckOrder].GetComponentInChildren<PlayingCardController>().setStyles((isFirstP == this.isFirstPlayer));
//			this.playingCards [debut + deck.Cards [i].deckOrder].GetComponentInChildren<PlayingCardController>().setCard(deck.Cards [i]);
//			this.playingCards [debut + deck.Cards [i].deckOrder].GetComponentInChildren<PlayingCardController>().setIDCharacter(debut + deck.Cards [i].deckOrder);
//			this.playingCards [debut + deck.Cards [i].deckOrder].GetComponentInChildren<PlayingCardController>().setTile(new Tile(deck.Cards [i].deckOrder + 1, hauteur), tiles [deck.Cards [i].deckOrder + 1, hauteur].GetComponent<TileController>().tileView.tileVM.position, !isFirstP);
//			this.playingCards [debut + deck.Cards [i].deckOrder].GetComponentInChildren<PlayingCardController>().resize();
//			this.tiles [deck.Cards [i].deckOrder + 1, hauteur].GetComponent<TileController>().characterID = debut + deck.Cards [i].deckOrder;
//			this.playingCards [debut + deck.Cards [i].deckOrder].GetComponentInChildren<PlayingCardController>().show();
//			compteur++;
//		}
//		if (this.isTutorialLaunched && !isFirstP)
//		{
//			this.disableAllCharacters();
//		}
		yield break;
	}

	public void playerReady()
	{
		photonView.RPC("playerReadyRPC", PhotonTargets.AllBuffered, this.isFirstPlayer);
		if (isTutorialLaunched)
		{
			photonView.RPC("playerReadyRPC", PhotonTargets.AllBuffered, !this.isFirstPlayer);
			//this.tutorial.GetComponent<GameTutorialController>().actionIsDone();
		}
	}

	public void StartFight()
	{
		if (this.isFirstPlayer)
		{
			this.sortAllCards();
			photonView.RPC("timeRunsOut", PhotonTargets.AllBuffered, this.timePerTurn);
			this.findNextPlayer();
		}
		
	}
	
	public void reloadDestinationTiles()
	{
//		if (this.isFirstPlayer == (currentPlayingCard < limitCharacterSide))
//		{
//			resetDestinations();
//			//this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().tile.setNeighbours(this.getCharacterTilesArray(), this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().card.GetMove());
//			this.setDestinations(currentPlayingCard);
//		}
	}
	
	[RPC]
	public void playerReadyRPC(bool isFirst)
	{
//		if (isFirst == this.isFirstPlayer)
//		{
//			//this.gameView.gameScreenVM.startMyPlayer();
//		} else
//		{
//			//this.gameView.gameScreenVM.startOtherPlayer();
//		}
//
//		if (this.isFirstPlayer)
//		{
//			for (int i = 0; i < this.nbFreeStartRows; i++)
//			{
//				for (int j = 0; j < this.boardWidth; j++)
//				{
//					this.tiles [j, i].GetComponent<TileController>().setDestination(true);
//				}
//			}
//		} else
//		{
//			for (int i = this.boardHeight-1; i > this.boardHeight-1-this.nbFreeStartRows; i--)
//			{
//				for (int j = 0; j < this.boardWidth; j++)
//				{
//					this.tiles [j, i].GetComponent<TileController>().setDestination(true);
//				}
//			}
//		}
//		
//		nbPlayersReadyToFight++;
//		
//		if (nbPlayersReadyToFight == 2)
//		{
//			//this.gameView.gameScreenVM.toDisplayStartWindows = false;
//			//this.displayPopUpMessage("Le combat commence", 2);
//			this.resetDestinations();	
//			this.nbTurns = 1;
//			
//			if (this.currentPlayingCard != -1)
//			{
//				this.hideActivatedPlayingCard();
//			}
//			
//			if (this.isFirstPlayer)
//			{
//				this.StartFight();
//			}
//			
//			int debut = 0;
//			if (this.isFirstPlayer)
//			{
//				debut = this.limitCharacterSide;
//			}
//			
//			for (int i = debut; i < debut+limitCharacterSide; i++)
//			{
//				this.playingCards [i].GetComponentInChildren<PlayingCardController>().showDisplay();
//			}
//		}
	}

	public void reloadSortedList()
	{
		if (this.isFirstPlayer)
		{
			sortAllCards();
			photonView.RPC("reloadTimeline", PhotonTargets.AllBuffered);

		}
	}

	public void sortAllCards()
	{
//		List <int> cardsToRank = new List<int>();
//		List <int> quicknessesToRank = new List<int>();
//		int indexToRank;
//
//		for (int i = 0; i < playingCards.Length; i++)
//		{
//			cardsToRank.Add(i);	
//			print(i);
//			quicknessesToRank.Add(this.playingCards [i].GetComponentInChildren<PlayingCardController>().card.GetSpeed());
//		}
//		for (int i = 0; i < playingCards.Length; i++)
//		{
//			indexToRank = this.FindMaxQuicknessIndex(quicknessesToRank);
//			print("j'add " + cardsToRank [indexToRank] + " au rang " + i + " avec la vitesse " + quicknessesToRank [indexToRank]);
//
//			quicknessesToRank.RemoveAt(indexToRank);
//			photonView.RPC("addRankedCharacter", PhotonTargets.AllBuffered, cardsToRank [indexToRank], i);
//			cardsToRank.RemoveAt(indexToRank);
//		}
	}
	
	public void rankNext(int idToRank)
	{
//		int i = 0;
//		int rankPlayingCard = -1;
//		int rankCardToRank = -1;
//		while (i<this.playingCards.Length && (rankPlayingCard==-1||rankCardToRank==-1))
//		{
//			if (this.rankedPlayingCardsID [i] == this.currentPlayingCard)
//			{
//				rankPlayingCard = i;
//			}
//			if (this.rankedPlayingCardsID [i] == idToRank)
//			{
//				rankCardToRank = i;
//			}
//			i++;
//		}
//		
//		
//		int compteur = 0;
//		int[] tempRank = new int[this.playingCards.Length];
//		for (int j = 0; j < this.playingCards.Length; j++)
//		{
//			if (j != rankCardToRank)
//			{
//				tempRank [compteur] = this.rankedPlayingCardsID [j];
//				print("J'ajoute en position " + compteur + " : " + this.rankedPlayingCardsID [j]);
//				compteur++;
//			}
//			if (j == rankPlayingCard)
//			{
//				tempRank [compteur] = idToRank;
//				print("J'ajoute en position " + compteur + " : " + idToRank);
//				compteur++;
//			}
//		}
//		for (int j = 0; j < this.playingCards.Length; j++)
//		{
//			print("New " + j + " : " + tempRank [j]);
//			this.rankedPlayingCardsID [j] = tempRank [j];
//		}
//		this.getPCC(idToRank).hasPlayed = false;
	}
	
	public void rankBefore(int idToRank)
	{
//		int i = 0;
//		int rankPlayingCard = -1;
//		int rankCardToRank = -1;
//		while (i<this.playingCards.Length && (rankPlayingCard==-1||rankCardToRank==-1))
//		{
//			if (this.rankedPlayingCardsID [i] == this.currentPlayingCard)
//			{
//				rankPlayingCard = i;
//			}
//			if (this.rankedPlayingCardsID [i] == idToRank)
//			{
//				rankCardToRank = i;
//			}
//			i++;
//		}
//		
//		int compteur = 0;
//		int[] tempRank = new int[this.playingCards.Length];
//		for (int j = 0; j < this.playingCards.Length; j++)
//		{
//			if (j == rankPlayingCard)
//			{
//				tempRank [compteur] = idToRank;
//				compteur++;
//			}
//			if (j != rankCardToRank)
//			{
//				tempRank [compteur] = this.rankedPlayingCardsID [j];
//				compteur++;
//			}
//		}
//		for (int j = 0; j < this.playingCards.Length; j++)
//		{
//			print("New " + j + " : " + tempRank [j]);
//			this.rankedPlayingCardsID [j] = tempRank [j];
//		}
//		this.getPCC(idToRank).hasPlayed = true;
	}

	public int FindMaxQuicknessIndex(List<int> list)
	{
		if (list.Count == 0)
		{
			throw new InvalidOperationException("Liste vide !");
		}
		int max = -1;
		int index = -1;
		for (int i = 0; i < list.Count; i++)
		{
			if (list [i] > max)
			{
				max = list [i];
				index = i;
			}
		}
		return index;
	}
	
	[RPC]
	public void addRankedCharacter(int id, int rank)
	{
//		if (rank == 0)
//		{
//			this.rankedPlayingCardsID = new int[playingCards.Length];
//		}
//		this.rankedPlayingCardsID [rank] = id;
//		if (rank == playingCards.Length - 1 && !gameEventInitialized)
//		{
//			initGameEvents();
//		}
	}

	[RPC]
	public IEnumerator moveCharacterRPC(int x, int y, int c, bool isFirstP, bool isSwap)
	{
//		if (!this.isFirstPlayer)
//		{
//			while (!this.bothPlayerLoaded)
//			{
//				print("J'attends");
//				yield return new WaitForSeconds(1);
//			}
//		}
//		
//		if (nbTurns > 0)
//		{
//			addGameEvent(new MovementType(), "");
//		}
//		
//		if (!isSwap)
//		{
//			this.tiles [this.playingCards [c].GetComponentInChildren<PlayingCardController>().tile.x, this.playingCards [c].GetComponentInChildren<PlayingCardController>().tile.y].GetComponent<TileController>().characterID = -1;
//		}
//
//		getTile(x, y).characterID = c;
//		getTile(x, y).statModifierActive = true;
//
//		getPCC(c).changeTile(new Tile(x, y), getTile(x, y).getPosition());
//		getPCC(c).card.TileModifiers.Clear();
//		loadTileModifierToCharacter(x, y, false);
//
//		if (this.isFirstPlayer == isFirstP && nbTurns != 0)
//		{
//			this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().tile.setNeighbours(this.getCharacterTilesArray(), this.playingCards [this.currentPlayingCard].GetComponentInChildren<PlayingCardController>().card.GetMove());
//			//this.isDragging = false;
//			this.resetDestinations();
////			if (this.clickedPlayingCard != this.currentPlayingCard)
////			{
////				this.showMyPlayingSkills(this.currentPlayingCard);
////			} else
////			{
////				this.updateStatusMySkills(this.currentPlayingCard);
////			}
////			this.tiles [x, y].GetComponentInChildren<TileController>().checkTrap(this.currentPlayingCard);
////			if (this.playindCardHasPlayed)
////			{
////				this.gameskills [1].launch();
////			}
//		}
//		
//		//playingCardHasMoved = true;
	yield break ;
	}

	[RPC]
	public void inflictDamageRPC(int targetCharacter, bool isFisrtPlayerCharacter)
	{
		if (!photonView.isMine)
		{
			//displayPopUpMessage(this.playingCards [targetCharacter].GetComponentInChildren<PlayingCardController>().card.Title + " attaque", 2f);
		}
	}

	[RPC]
	public void timeRunsOut(float time)
	{
//		startTurn = true;
//		gameView.gameScreenVM.timer = time;
	}
	
	// Photon
	void OnJoinedLobby()
	{
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);    
		string sqlLobbyFilter = "C0 = " + ApplicationModel.gameType;
		PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	}
	
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room! - creating a new room");
		RoomOptions newRoomOptions = new RoomOptions();
		newRoomOptions.isOpen = true;
		newRoomOptions.isVisible = true;
		newRoomOptions.maxPlayers = 2;
		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", ApplicationModel.gameType } }; // CO pour une partie simple
		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
		
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);
		PhotonNetwork.CreateRoom(roomNamePrefix + Guid.NewGuid().ToString("N"), newRoomOptions, sqlLobby);
		this.isFirstPlayer = true;
	}
	
	void OnJoinedRoom()
	{
		Debug.Log("Connecté à Photon");
		if (!isReconnecting)
		{
			photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);
			if (this.isTutorialLaunched)
			{
				photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID + 1, "Garruk");
			}
		} 
		else
		{
			Debug.Log("Mode reconnection");
		}
		GameView.instance.createBackground();
		
		if (this.isFirstPlayer)
		{
			this.initGrid();
			//StartCoroutine(this.loadMyDeck(true));
		} 
		else if (!this.isFirstPlayer && nbPlayers == 2)
		{
			//StartCoroutine(this.loadMyDeck(false));
		} 
		else if (this.isTutorialLaunched && nbPlayers == 2)
		{
			//StartCoroutine(this.loadTutorialDeck(!this.isFirstPlayer, loginName));
		}		
	}
	
	void OnDisconnectedFromPhoton()
	{
		//Application.LoadLevel("Lobby");
	}

	public void quitGameHandler()
	{
		//StartCoroutine(this.quitGame());
	}
	
	public IEnumerator quitGame()
	{
//		if(isTutorialLaunched)
//		{
//			yield return (StartCoroutine(this.sendStat(this.users [0].Username, this.users [1].Username)));
//			photonView.RPC("quitGameRPC", PhotonTargets.AllBuffered, false);
//		}
//		else
//		{
//			if(isFirstPlayer)
//			{
//				yield return (StartCoroutine(this.sendStat(this.users [1].Username, this.users [0].Username)));
//			} 
//			else
//			{
//				yield return (StartCoroutine(this.sendStat(this.users [0].Username, this.users [1].Username)));
//			}
//			photonView.RPC("quitGameRPC", PhotonTargets.AllBuffered, this.isFirstPlayer);
//		}
	yield return 0;
	}
	
	[RPC]
	public void quitGameRPC(bool isFirstP)
	{
//		//gameView.gameScreenVM.toDisplayGameScreen = false;
//		for (int i = 0; i < this.playingCards.Length; i++)
//		{
//			this.playingCards [i].SetActive(false);	
//		}
//		for (int i = 0; i < 6; i++)
//		{
////			this.skillsObjects [i].SetActive(false);
////			this.opponentSkillsObjects [i].SetActive(false);
//		}
//		
//		if (isFirstP == this.isFirstPlayer)
//		{
//			//print("J'ai perdu comme un gros con");
//			EndSceneController.instance.displayEndScene(false);
//		} else
//		{
//			//print("Mon adversaire a lachement abandonné comme une merde");
//			EndSceneController.instance.displayEndScene(true);
//		}
	}

	public void addGameEvent(GameEventType type, string targetName)
	{
		setGameEvent(type);
		if (targetName != "")
		{
//			int midTimeline = (int)Math.Floor((double)eventMax / 2);
//			gameEvents [midTimeline].GetComponent<GameEventController>().setTarget(targetName);
		}
	}

	public void addGameEvent(string action, string targetName)
	{
		photonView.RPC("addGameEventRPC", PhotonTargets.AllBuffered, action, targetName);
	}
	[RPC]
	public void addGameEventRPC(string action, string targetName)
	{
		setGameEvent(new SkillType(action));
		if (targetName != "")
		{
//			int midTimeline = (int)Math.Floor((double)eventMax / 2);
//			gameEvents [midTimeline].GetComponent<GameEventController>().setTarget(targetName);
		}
	}

	public void addMovementEvent(GameObject origin, GameObject destination)
	{
		GameObject go = setGameEvent(new MovementType());

		go.GetComponent<GameEventController>().setMovement(origin, destination);
	}

	GameObject setGameEvent(GameEventType type)
	{
//		int midTimeline = (int)Math.Floor((double)eventMax / 2);
//		GameObject go;
//		if (nbActionPlayed == 0)
//		{ 
//			go = gameEvents [midTimeline];
//			go.GetComponent<GameEventController>().setAction(type.toString());
//			nbActionPlayed++;
//		} else if (nbActionPlayed < 2)
//		{
//			go = gameEvents [midTimeline];
//			go.GetComponent<GameEventController>().addAction(type.toString());
//			nbActionPlayed++;
//		} else
//		{
//			go = gameEvents [0];
//		}

		//return go;
		return null;
	}

	void fillTimeline()
	{
		int rankedPlayingCardID = 0;
		bool nextChara = true;
		bool findCharactersHaveNoAlreadyPlayed = false;

//		if (nextCharacterPositionTimeline < 7)
//		{
//			findCharactersHaveNoAlreadyPlayed = true;
//		}
//
//		while (nextChara)
//		{
//			rankedPlayingCardID = rankedPlayingCardsID [nextCharacterPositionTimeline];
//			PlayingCardController pcc = this.playingCards [rankedPlayingCardID] .GetComponentInChildren<PlayingCardController>();
//			if (!pcc.isDead && (!pcc.hasPlayed && !findCharactersHaveNoAlreadyPlayed || findCharactersHaveNoAlreadyPlayed))
//			{
//				nextChara = false;
//			}
//			if (++nextCharacterPositionTimeline > playingCards.Length - 1)
//			{
//				nextCharacterPositionTimeline = 0;
//				findCharactersHaveNoAlreadyPlayed = true;
//			}
//		}
		addCardEvent(rankedPlayingCardID, 0);
	}

	[RPC]
	public void reloadTimeline()
	{
//		bool findCharactersHaveNoAlreadyPlayed = false;
//		//nextCharacterPositionTimeline = 0;
//		for (int i = 4; i >= 0; i--)
//		{
//			int rankedPlayingCardID = 0;
//			bool nextChara = true;
//			while (nextChara)
//			{
//				//rankedPlayingCardID = rankedPlayingCardsID [nextCharacterPositionTimeline];
//				PlayingCardController pcc = this.playingCards [rankedPlayingCardID] .GetComponentInChildren<PlayingCardController>();
//				
//				if (!pcc.isDead && (!pcc.hasPlayed && !findCharactersHaveNoAlreadyPlayed || findCharactersHaveNoAlreadyPlayed) && rankedPlayingCardID != currentPlayingCard)
//				{
//					nextChara = false;
//				}
////				if (++nextCharacterPositionTimeline > playingCards.Length - 1)
////				{
////					nextCharacterPositionTimeline = 0;
////					findCharactersHaveNoAlreadyPlayed = true;
////				}
//			}
//			addCardEvent(rankedPlayingCardID, i);
//		}

	}

	void addCardEvent(int idCharacter, int position)
	{
		//GameObject go = gameEvents [position];
//		PlayingCardController pcc = playingCards [idCharacter].GetComponent<PlayingCardController>();
//		go.GetComponent<GameEventController>().setCharacterName(pcc.card.Title);
//		Texture t2 = playingCards [idCharacter].GetComponent<PlayingCardController>().getPicture();
//		Texture2D temp = getImageResized(t2 as Texture2D);
//		go.GetComponent<GameEventController>().IDCharacter = idCharacter;
//		go.GetComponent<GameEventController>().setAction("");
//		go.GetComponent<GameEventController>().setArt(temp);
//		go.GetComponent<GameEventController>().setBorder(isThisCharacterMine(idCharacter));
//		go.GetComponent<GameEventController>().gameEventView.show();
//		go.renderer.enabled = true;
	}

	void initGameEvents()
	{
//		//gameEventInitialized = true;
//		GameObject go;
//		int i = 1;
//		while (gameEvents.Count < eventMax)
//		{
//			go = (GameObject)Instantiate(gameEvent);
//			gameEvents.Add(go);
//			go.GetComponent<GameEventController>().setScreenPosition(gameEvents.Count, boardWidth, boardHeight, tileScale);
//			go.GetComponent<GameEventController>().setBorder(0);
//			if (i < 7)
//			{
//				GameObject child = (GameObject)Instantiate(go.GetComponent<GameEventController>().transparentImage);
//				child.name = "TransparentEvent";
//				child.transform.parent = go.transform;
//				child.transform.localPosition = new Vector3(0f, 0f, -5f);
//				child.transform.localScale = new Vector3(0.9f, 0.9f, 10f);
//			}
////			if (i == 6)
////			{
////				go.transform.localScale = new Vector3(0.95f, 0.95f, 10f);
////			} 
//
//			if (i > 6)
//			{
//				GameObject child = (GameObject)Instantiate(go.GetComponent<GameEventController>().darkImage);
//				child.name = "DarkEvent";
//				child.transform.parent = go.transform;
//				child.transform.localPosition = new Vector3(0f, 0f, -5f);
//				child.transform.localScale = new Vector3(0.9f, 0.9f, 10f);
//				child.renderer.enabled = false;
//			}
//			go.renderer.enabled = false;
//			i++;
//		}
//		for (i = 0; i < 6; i++)
//		{
//			addCardEvent(rankedPlayingCardsID [5 - i], i);
//		}
//		nextCharacterPositionTimeline = 6;
	}

	Texture2D getImageResized(Texture2D t)
	{
		int size;
		Color[] pix;
		if (t.width > t.height)
		{
			size = t.height;
			pix = t.GetPixels((t.width - size) / 2, 0, size, size);
		} else
		{
			size = t.width;
			pix = t.GetPixels(0, (t.height - size) / 2, size, size);
		}
		Texture2D temp = new Texture2D(size, size);
		temp.SetPixels(pix);
		temp.Apply();
		
		return temp;
	}
	
	public Card getCurrentCard()
	{
		//return this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().card;
		return new Card();
	}

	void changeGameEvents()
	{
//		for (int i = gameEvents.Count - 1; i > 0; i--)
//		{
//			int id = gameEvents [i - 1].GetComponent<GameEventController>().IDCharacter;
//			string title = gameEvents [i - 1].GetComponent<GameEventController>().getCharacterName();
//			string action = gameEvents [i - 1].GetComponent<GameEventController>().getAction();
//			GameObject[] movement = gameEvents [i - 1].GetComponent<GameEventController>().getMovement();
//			Texture2D t2 = gameEvents [i - 1].GetComponent<GameEventController>().getArt();
//			if (title != "" && i > 5)
//			{
//				gameEvents [i].renderer.enabled = true;
//				gameEvents [i].transform.Find("DarkEvent").renderer.enabled = true;
//			}
//
//			gameEvents [i].GetComponent<GameEventController>().IDCharacter = id;
//			gameEvents [i].GetComponent<GameEventController>().setCharacterName(title);
//			gameEvents [i].GetComponent<GameEventController>().setAction(action);
//			gameEvents [i].GetComponent<GameEventController>().setMovement(movement [0], movement [1]);
//			gameEvents [i].GetComponent<GameEventController>().setArt(t2);
//			if (i < 6)
//			{
//				gameEvents [i].GetComponent<GameEventController>().setBorder(isThisCharacterMine(id));
//			}
//
//			gameEvents [i].GetComponent<GameEventController>().gameEventView.show();
//			gameEvents [i - 1].GetComponent<GameEventController>().setMovement(null, null);
//		}
	}

	void recalculateGameEvents()
	{
		int i = 1;

//		foreach (GameObject go in gameEvents)
//		{
//			go.GetComponent<GameEventController>().setScreenPosition(i++, boardWidth, boardHeight, tileScale);
//		}
	}

	public void disconnect()
	{
		PhotonNetwork.Disconnect();
	}

	int isThisCharacterMine(int id)
	{
		//return (isFirstPlayer == (id < limitCharacterSide)) ? 1 : -1;
		return 0 ;
	}

	

	public void spawnMinion(string minionName, int targetX, int targetY, int amount, bool isFirstP)
	{
		photonView.RPC("spawnMinion", PhotonTargets.AllBuffered, this.isFirstPlayer, minionName, targetX, targetY, amount);
	}

	[RPC]
	public void spawnMinion(bool isFirstP, string minionName, int targetX, int targetY, int amount)
	{
//		//addGameEvent(new SkillType(getCurrentSkill().Action), "");
//		GameObject[] temp = new GameObject[playingCards.Length + 1];
//		int position;
//		if (isFirstP)
//		{
//			for (int i = 0; i < limitCharacterSide; i++)
//			{
//				temp [i] = playingCards [i];
//			}
//			for (int i = limitCharacterSide + 1; i < temp.Length; i++)
//			{
//				temp [i] = playingCards [i - 1];
//				PlayingCardController pcctemp = playingCards [i - 1].GetComponent<PlayingCardController>();
//				getTile(pcctemp.tile.x, pcctemp.tile.y).characterID = i;
//			}
//			position = limitCharacterSide;
//			limitCharacterSide++;
//		} else
//		{
//			for (int i = 0; i < playingCards.Length; i++)
//			{
//				temp [i] = playingCards [i];
//			}
//			position = playingCards.Length;
//		}
//
//		playingCards = temp;
//		//this.playingCards [position] = (GameObject)Instantiate(this.playingCard);
//		PlayingCardController pccTemp = this.playingCards [position].GetComponentInChildren<PlayingCardController>();
//		pccTemp.setStyles(isFirstP == this.isFirstPlayer);
//		Card minion;
//		switch (minionName)
//		{
//			case "Grizzly": 
//				minion = new Card(0, minionName, 60, 10, 50, 2, amount, new List<Skill>());
//				break;
//			case "Loup": 
//				minion = new Card(0, minionName, 40, 11, 100, 5, amount, new List<Skill>());
//				break;
//			default:
//				minion = new Card();
//				break;
//		}
//		
//		pccTemp.setCard(minion);
//		pccTemp.setIDCharacter(position);
//		pccTemp.setTile(new Tile(targetX, targetY), tiles [targetX, targetY].GetComponent<TileController>().tileView.tileVM.position, !isFirstP);
//		pccTemp.resize();
//		this.tiles [targetX, targetY].GetComponent<TileController>().characterID = position;
//		pccTemp.show();
//		reloadSortedList();
	}

	public PlayingCardController getCurrentPCC()
	{
		//return this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>();
		return null ;
	}

	public PlayingCardController getPCC(int id)
	{
		//return this.playingCards [id].GetComponent<PlayingCardController>();
		return null;
	}

	public TileController getTile(int x, int y)
	{
		//return this.tiles [x, y].GetComponent<TileController>();
		return null;
	}

	public Card getCard(int id)
	{
		//return this.playingCards [id].GetComponent<PlayingCardController>().card;
		return new Card();
	}

	public Skill getCurrentSkill()
	{
		//return this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().card.Skills [this.clickedSkill];
		return null;
	}

	public int getCurrentSkillID()
	{
//		if (this.clickedSkill == 4)
//		{
//			return 0;
//		} else if (this.clickedSkill == 5)
//		{
//			return 1;
//		}
//		return this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().card.Skills [this.clickedSkill].Id;
		return 0;
	}

	public void reloadCard(int id)
	{
		//this.playingCards [id].GetComponent<PlayingCardController>().show();
	}

	public void play()
	{
		this.getCurrentPCC().hideControlSkillHandler();
//		
//		if (this.clickedPlayingCard != this.currentPlayingCard && this.clickedPlayingCard != -1)
//		{
//			this.showMyPlayingSkills(this.clickedPlayingCard);
//		} else
//		{
//			this.updateStatusMySkills(this.currentPlayingCard);
//		}
//		
//		if (this.playingCardHasMoved || this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().isDead)
//		{
//			this.gameskills [1].launch();
//		}
	}
	
	public void playSkill(int isSuccess)
	{
		photonView.RPC("playSkillRPC", PhotonTargets.AllBuffered, isSuccess);
	}
	
	[RPC]
	public void playSkillRPC(int isSuccess)
	{
		if (isSuccess == 1)
		{
			this.displaySkillEffect(this.currentPlayingCard, this.getCurrentGameSkill().getSuccessText(), 5, isSuccess);
			this.addGameEvent(this.getCurrentGameSkill().getSuccessText(), "");
		} else if (isSuccess == 0)
		{
			this.displaySkillEffect(this.currentPlayingCard, this.getCurrentGameSkill().getFailureText(), 5, isSuccess);
			this.addGameEvent(this.getCurrentGameSkill().getFailureText(), "");
		}
	}
	
	public void startPlayingSkill()
	{
		//photonView.RPC("startPlayingSkillRPC", PhotonTargets.AllBuffered, this.clickedSkill);
	}
	
	[RPC]
	public void startPlayingSkillRPC(int idskill)
	{
		//this.clickedSkill = idskill;
	}
	
	public void displaySkillEffect(int target, string s, float timer, int colorindex)
	{
		this.getPCC(target).addSkillResult(s, timer, colorindex);
	}
	
	public void emptyTile(int x, int y)
	{
		//this.tiles [x, y].GetComponent<TileController>().characterID = -1;
		this.areMyHeroesDead();
	}
	
	public void areMyHeroesDead()
	{
//		bool areTheyAllDead = true;
//		int debut;
//		int i = 0;
//		if (this.isFirstPlayer)
//		{
//			debut = 0;
//		} else
//		{
//			debut = this.limitCharacterSide;
//		}
//		while (areTheyAllDead && i<limitCharacterSide)
//		{
//			if (!this.getPCC(debut + i).isDead)
//			{
//				areTheyAllDead = false;
//			}
//			i++;
//		}
//		if (areTheyAllDead)
//		{
//			//StartCoroutine(this.quitGame());
//		}
	}
	
	public void addCardModifier(int target, int amount, ModifierType type, ModifierStat stat, int duration, int idIcon, string t, string d, string a)
	{ 
		this.getCard(target).addModifier(amount, type, stat, duration, idIcon, t, d, a);
		this.getPCC(target).show();
		if (stat == ModifierStat.Stat_Dommage)
		{
			if (!this.getPCC(target).isDead && this.getCard(target).GetLife() <= 0)
			{
				this.getPCC(target).kill();
			}
		}
	}
	
	public void addTileModifier(Tile tile, int amount, ModifierType type, ModifierStat stat, int duration, int idIcon, string t, string d, string a)
	{ 
//		bool b = true;
//		if (this.currentPlayingCard < this.limitCharacterSide != this.isFirstPlayer)
//		{
//			b = false;
//		}
//		this.tiles [tile.x, tile.y].GetComponent<TileController>().tile.setModifier(amount, type, stat, duration, idIcon, t, d, a, b);
//		this.tiles [tile.x, tile.y].GetComponent<TileController>().show();
	}
	
	public IEnumerator kill(int target)
	{
		yield return new WaitForSeconds(2f);
		//this.tiles [this.getPCC(target).tile.x, this.getPCC(target).tile.x].GetComponent<TileController>().characterID = -1;
		this.getPCC(target).kill();
	}

	public void addTileModifier(int modifierType, int amount, int tileX, int tileY)
	{
		photonView.RPC("addTileModifierRPC", PhotonTargets.AllBuffered, modifierType, amount, tileX, tileY, this.isFirstPlayer);
	}

	public void addCardModifier(int amount, int targetID, int modifierType, int modifierStat, int duration)
	{
		photonView.RPC("addCardModifierRPC", PhotonTargets.AllBuffered, amount, targetID, modifierType, modifierStat, duration, this.isFirstPlayer);
	}

	[RPC]
	public void addTileModifierRPC(int modifierType, int amount, int tileX, int tileY, bool isFirstP)
	{
//		int c = getTile(tileX, tileY).characterID;
//		if (c != -1)
//		{
//			getPCC(c).card.TileModifiers.Clear();
//		}
//		switch (modifierType)
//		{
//			case 0:
//				this.tiles [tileX, tileY].GetComponent<TileController>().addTemple(amount);
//				loadTileModifierToCharacter(tileX, tileY, false);
//				break;
//			case 1:
//				TileController tileController1 = this.tiles [tileX, tileY].GetComponent<TileController>();
//				tileController1.addForetIcon(amount);
//				loadTileModifierToCharacter(tileX, tileY, false);
//				foreach (Tile til in tileController1.tile.getImmediateNeighbouringTiles())
//				{
//					this.getTile(til.x, til.y).addForetIcon(amount);
//					loadTileModifierToCharacter(til.x, til.y, false);
//					reloadDestinationTiles();
//				}
//				break;
//			case 2: 
//				getTile(tileX, tileY).addSable((this.isFirstPlayer == isFirstP));
//				loadTileModifierToCharacter(tileX, tileY, false);
//				break;
//			case 3:
//				getTile(tileX, tileY).addFontaine(amount);
//				loadTileModifierToCharacter(tileX, tileY, false);
//				break;
//			default:
//				break;
//		}
//		//addGameEvent(new SkillType(getCurrentSkill().Action), "");
	}

	[RPC]
	public void addCardModifierRPC(int amount, int targetID, int modifierType, int modifierStat, int duration, bool isFirstP)
	{
		getCard(targetID).modifiers.Add(new StatModifier(amount, (ModifierType)modifierType, (ModifierStat)modifierStat, duration, 0, "", "", ""));
		reloadCard(targetID);
	}

	public void relive(int id, int x, int y)
	{
		photonView.RPC("reliveRPC", PhotonTargets.AllBuffered, id, x, y, this.isFirstPlayer);
	}

	public void clearDeads()
	{
		//deads.Clear();
		foreach (GameObject goTag in GameObject.FindGameObjectsWithTag("deadCharacter"))
		{
			Destroy(goTag);
		}
	}

	[RPC]
	public void reliveRPC(int id, int x, int y, bool isFirstP)
	{
		PlayingCardController pcc = GameController.instance.getPCC(id).GetComponent<PlayingCardController>();
		//pcc.setTile(new Tile(x, y), getTile(x, y).GetComponent<TileController>().tileView.tileVM.position, (id < limitCharacterSide) != isFirstP);
		pcc.card.Life = 1;
		pcc.relive();
		pcc.show();
		//addGameEvent(new SkillType(getCurrentSkill().Action), pcc.card.Title);
	}

	[RPC]
	public void loadTileModifierToCharacter(int x, int y, bool newTurn)
	{
		TileController tileController = this.getTile(x, y).GetComponent<TileController>();
//		if (tileController.characterID != -1)
//		{
//			if (tileController.statModifierActive == true && newTurn == tileController.statModifierNewTurn && tileController.tileModification != TileModification.Foret_de_Lianes)
//			{
////				foreach (StatModifier sm in tileController.tile.StatModifier)
////				{
////					this.getPCC(tileController.characterID).card.TileModifiers.Add(sm);
////				}
////				if (tileController.tileModification == TileModification.Sables_Mouvants)
////				{
////					tileController.tileView.tileVM.toDisplayIcon = true;
////					//playRPC(this.getPCC(tileController.characterID).card.Title + " est pris dans un sable mouvant");
////				}
////				if (!tileController.statModifierEachTurn)
////				{
////					tileController.statModifierActive = false;
////				}
//				//reloadCard(tileController.characterID);
//			}
//		}
	}
	
	public void useSkill()
	{
		//photonView.RPC("useSkillRPC", PhotonTargets.AllBuffered, this.currentPlayingCard, this.clickedSkill);
	}
	
	[RPC]
	public void useSkillRPC(int target, int nbSkill)
	{
		this.getCard(target).Skills [nbSkill].lowerNbLeft();
	}
	
	public void initPCCTargetHandler(int numberOfExpectedTargets)
	{
		this.targetPCCHandler = new TargetPCCHandler(numberOfExpectedTargets);
	}
	
	public void initTileTargetHandler(int numberOfExpectedTargets)
	{
		this.targetTileHandler = new TargetTileHandler(numberOfExpectedTargets);
	}
	
	public GameSkill getCurrentGameSkill()
	{
		//return this.gameskills [this.getCurrentSkillID()];
		return null;
	}
	
	public void applyOn()
	{
		photonView.RPC("applyOnRPC5", PhotonTargets.AllBuffered);
	}
	
	[RPC]
	public void applyOnRPC5()
	{
		this.getCurrentGameSkill().applyOn();
	}
	
	public void applyOn(int[] targets)
	{
		photonView.RPC("applyOnRPC", PhotonTargets.AllBuffered, targets);
	}
	
	[RPC]
	public void applyOnRPC(int[] targets)
	{
		this.getCurrentGameSkill().applyOn(targets);
	}
	
	public void applyOn(int target)
	{
		photonView.RPC("applyOnRPC4", PhotonTargets.AllBuffered, target);
	}
	
	[RPC]
	public void applyOnRPC4(int target)
	{
		this.getCurrentGameSkill().applyOn(target);
	}
	
	public void applyOn(int target, int arg)
	{
		photonView.RPC("applyOnRPC3", PhotonTargets.AllBuffered, target, arg);
	}
	
	[RPC]
	public void applyOnRPC3(int target, int arg)
	{
		this.getCurrentGameSkill().applyOn(target, arg);
	}
	
	public void applyOn(int[] targets, int[] args)
	{
		photonView.RPC("applyOnRPC2", PhotonTargets.AllBuffered, targets, args);
	}
	
	[RPC]
	public void applyOnRPC2(int[] targets, int[] args)
	{
		this.getCurrentGameSkill().applyOn(targets, args);
	}
	
	public void activateTrap(int idSkill, int[] targets, int[] args)
	{
		photonView.RPC("activateTrapRPC", PhotonTargets.AllBuffered, idSkill, targets, args);
	}
	
	[RPC]
	public void activateTrapRPC(int idSkill, int[] targets, int[] args)
	{
		//this.gameskills [idSkill].activateTrap(targets, args);
	}
	
	public void hideTrap(int[] targets)
	{
		photonView.RPC("hideTrapRPC", PhotonTargets.AllBuffered, targets);
	}
	
	[RPC]
	public void hideTrapRPC(int[] targets)
	{
		//this.tiles [targets [0], targets [1]].GetComponent<TileController>().hideIcon();
	}
	
	public void failedToCastOnSkill(int[] targets, int[] failures)
	{
		photonView.RPC("failedToCastOnSkillRPC", PhotonTargets.AllBuffered, targets, failures);
	}
	
	[RPC]
	public void failedToCastOnSkillRPC(int[] targets, int[] failures)
	{
		this.getCurrentGameSkill().failedToCastOn(targets, failures);
	}
	
	public void failedToCastOnSkill(int target, int failure)
	{
		photonView.RPC("failedToCastOnSkillRPC", PhotonTargets.AllBuffered, target, failure);
	}
	
	[RPC]
	public void failedToCastOnSkillRPC(int target, int failure)
	{
		this.getCurrentGameSkill().failedToCastOn(target, failure);
	}
	
	public int nbMyPlayersAlive()
	{
		int compteur = 0;
//		for (int i = 0; i < this.playingCards.Length; i++)
//		{
//			if (!this.getPCC(i).isDead && i != this.currentPlayingCard && this.getPCC(i).isMine)
//			{
//				compteur++;
//			}
//		}
		return compteur;
	}
	
	public int nbOtherPlayersAlive()
	{
		int compteur = 0;
//		for (int i = 0; i < this.playingCards.Length; i++)
//		{
//			if (!this.getPCC(i).isDead && i != this.currentPlayingCard && !this.getPCC(i).isMine)
//			{
//				compteur++;
//			}
//		}
		return compteur;
	}
	
	public int getNbPlayingCards()
	{
		//return (this.playingCards.Length);
		return 0;
	}

	// Méthodes pour le tutoriel

	public void setButtonsGUI(bool value)
	{
//		for (int i =0; i<gameView.gameScreenVM.buttonsEnabled.Length; i++)
//		{
//			gameView.gameScreenVM.buttonsEnabled [i] = value;
//		}
	}
	public void setButtonGUI(int index, bool value)
	{
		//gameView.gameScreenVM.buttonsEnabled [index] = value;
	}
	public void activeSingleCharacter(int index)
	{
		//this.playingCards [index].GetComponent<PlayingCardController>().hideTargetHalo();
	}
	public void activeTargetingOnCharacter(int index)
	{
		//this.playingCards [index].GetComponent<PlayingCardController>().setIsDisable(false);
	}
	public void activeAllCharacters()
	{
//		for (int i=0; i<this.limitCharacterSide; i++)
//		{
//			//this.playingCards [i].GetComponent<PlayingCardController>().hideTargetHalo();
//		}
	}
	public void disableAllCharacters()
	{
//		for (int i=0; i<this.playingCards.Length; i++)
//		{
//			this.playingCards [i].GetComponent<PlayingCardController>().setTargetHalo(new HaloTarget(1), true);
//		}
	}
	public Vector2 getPlayingCardsPosition(int index)
	{
		//return this.playingCards [index].GetComponent<PlayingCardController>().getGOScreenPosition(this.playingCards [index]);
		return new Vector2();
	}
	
	public Vector2 getPlayingCardsSize(int index)
	{
		//return this.playingCards [index].GetComponent<PlayingCardController>().getGOScreenSize(this.playingCards [index]);
		return new Vector2();
	}
	
	public Vector2 getTilesPosition(int x, int y)
	{
		//return this.tiles [x, y].GetComponent<TileController>().getGOScreenPosition(this.tiles [x, y]);
		return new Vector2();
	}
	
	public Vector2 getTilesSize(int x, int y)
	{
		//return this.tiles [x, y].GetComponent<TileController>().getGOScreenSize(this.tiles [x, y]);
		return new Vector2();
	}
	
	public void setDestination(int x, int y, bool value)
	{
		//this.tiles [x, y].GetComponent<TileController>().setDestination(value);
	}
	
	public void addTileHalo(int x, int y, int haloIndex, bool isHaloDisabled)
	{
		//this.tiles [x, y].GetComponent<TileController>().setTargetHalo(new HaloTarget(haloIndex), isHaloDisabled);
	}
	public void hideTileHalo(int x, int y)
	{
		//this.tiles [x, y].GetComponent<TileController>().hideTargetHalo();
	}
	public void disableAllSkillObjects()
	{
//		for (int i=0; i<this.skillsObjects.Length; i++)
//		{
//			this.skillsObjects [i].GetComponent<SkillObjectController>().setControlActive(false);
//		}
	}
	public void setAllSkillObjects(bool value)
	{
//		for (int i=0; i<this.skillsObjects.Length; i++)
//		{
//			this.skillsObjects [i].GetComponent<SkillObjectController>().setActive(value);
//		}
	}
	public void activeSingleSkillObjects(int index)
	{
		//this.skillsObjects [index].GetComponent<SkillObjectController>().setControlActive(true);
	}
	public Vector2 getSkillObjectsPosition(int index)
	{
		//return this.skillsObjects [index].GetComponent<SkillObjectController>().getGOScreenPosition(this.skillsObjects [index]);
		return new Vector2();
	}
	public Vector2 getSkillObjectsSize(int index)
	{
		//return this.skillsObjects [index].GetComponent<SkillObjectController>().getGOScreenSize(this.skillsObjects [index]);
		return new Vector2();
	}
	public void launchSkill(int clickedSkill, List<int> pcc)
	{
		//this.clickedSkill = clickedSkill;
		//this.gameskills [getCurrentSkillID()].resolve(pcc);
	}
	public void callTutorial()
	{
		if (isTutorialLaunched)
		{
			//this.tutorial.GetComponent<GameTutorialController>().actionIsDone();
		}
	}
	public bool getIsTutorialLaunched()
	{
		return this.isTutorialLaunched;
	}
	public void setEndSceneControllerGUI(bool value)
	{
		EndSceneController.instance.setGUI (value);
	}
	public IEnumerator endTutorial()
	{
//		this.setEndSceneControllerGUI (false);
//		//yield return StartCoroutine (this.users[0].setTutorialStep (5));
//		ApplicationModel.launchGameTutorial = false;
//		Application.LoadLevel ("EndGame");
	yield return 0 ;
	}
	
}

