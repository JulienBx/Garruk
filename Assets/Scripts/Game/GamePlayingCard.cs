﻿using UnityEngine;
using System.Collections;

public class GamePlayingCard : Photon.MonoBehaviour {
	
	public static GamePlayingCard instance;
	public GameNetworkCard gameCard;
	public int nbSkillToBeCast;
	
	public GameTile attemptToMoveTo;
	public bool hasMoved = false;
	public bool attemptToAttack = false;
	public bool hasAttacked = false;
	public bool attemptToCast = false;
	public int SkillCasted;
	
	Vector3 WorldNamePos;
	
	
	public int Damage = 0;
	public Texture2D bgImage; 
	public Texture2D fgImage;
	public GUIStyle progress_empty, progress_full;
	
	public void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {
		Vector3 GameCardPosition = transform.Find("Life/Life Bar").position;
		WorldNamePos = Camera.main.camera.WorldToScreenPoint(GameCardPosition);
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnGUI()
	{
		if (!GameBoard.instance.TimeOfPositionning && !GameScript.instance.gameOver)
		{
			if (GameBoard.instance.MyPlayerNumber == gameCard.ownerNumber)
			{
				GameScript.instance.labelText = "A vous de jouer";
				Vector3 pos = transform.position;
				pos = Camera.main.camera.WorldToScreenPoint(pos);
				if (GUI.Button(new Rect(pos.x - 35, Screen.height - pos.y - 110, 67, 25), "Passer"))
				{
					Pass();
				}
				if (GameTimeLine.instance.PlayingCard.hasNeighbor() && !hasAttacked && !attemptToAttack)
				{
					if (GUI.Button(new Rect(pos.x - 35, Screen.height - pos.y - 150, 67, 25), "Attaquer!"))
					{
						GameTile.instance.SetCursorToAttack();
						attemptToAttack = true;
					}
				}
			} else
			{
				GameScript.instance.labelText = "Au joueur adverse de jouer";
			}
		}
		if (this.gameCard.Card != null)
		{
			GUI.BeginGroup(new Rect(WorldNamePos.x, Screen.height - WorldNamePos.y, 16, 50));
			GUI.Box(new Rect(0,0,16,50), bgImage, progress_empty);
			GUI.BeginGroup(new Rect(0, 0, 16, 50));
			GUI.Box (new Rect(0,0,8,50), fgImage, progress_full);
			GUI.EndGroup();
			GUI.EndGroup();
		}
		
	}
	
	public void Pass()
	{
		GamePlayingCard.instance.attemptToAttack = false;
		GamePlayingCard.instance.attemptToCast = false;
		GamePlayingCard.instance.hasAttacked = true;
		GameTile.instance.SetCursorToDefault();
		photonView.RPC("ForwardInTime", PhotonTargets.AllBuffered);
	}
	public void ChangeCurrentCard(GameNetworkCard card)
	{
		hasMoved = false;
		hasAttacked = false;
		attemptToMoveTo = null;
		gameCard.Card = card.Card;
		gameCard.ownerNumber = card.ownerNumber;
		changeStats();
	}
	
	public void changeStats()
	{
		Transform attackText = transform.Find("Icons/Attack/Value");
		attackText.GetComponent<TextMesh>().text = this.gameCard.Card.GetAttack().ToString();
		
		//Transform energyText = transform.Find("Icons/Energy/Value");
		//energyText.GetComponent<TextMesh>().text = this.gameCard.Card.Energy.ToString();
		
		Transform moveText = transform.Find("Icons/Move/Value");
		moveText.GetComponent<TextMesh>().text = this.gameCard.Card.GetMove().ToString();
		
		Transform speedText = transform.Find("Icons/Speed/Value");
		speedText.GetComponent<TextMesh>().text = this.gameCard.Card.GetSpeed().ToString();
		
		int i = 1;
		if (gameCard.Card.Skills != null)
		{
			foreach (Skill skill in gameCard.Card.Skills)
			{
				Transform skillText = transform.Find("Skills/Skill " + i++);
				skillText.GetComponent<TextMesh>().text = skill.Name;
			}
		}
		gameCard.ShowFace();
	}
	
	[RPC]
	private void ForwardInTime()
	{
		GameTimeLine.instance.PlayingCard.transform.Find("Yellow Outline").renderer.enabled = false;
		GameTimeLine.instance.forward();
		GameTimeLine.instance.PlayingCard.transform.Find("Yellow Outline").renderer.enabled = true;
	}
}
