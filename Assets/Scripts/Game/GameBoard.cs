﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBoard : MonoBehaviour 
{
	public GameObject[] locations;
	public GameObject Card;
	public bool isDragging = false;         // Pendant la phase de positionnement
	public bool isMoving = false;           // Pendant la phase de combat
	public bool droppedCard = false;        
	public bool TimeOfPositionning;         // false : phase de positionnement, true : phase de combat
	public GameNetworkCard CardSelected;           // Carte sélectionnée dans la phase de positionnement et la phase de combat 
	public GameCard CardHovered;
	public static GameBoard instance = null;
	public int gridWidthInHexes = 5;
	public int gridHeightInHexes = 8;
	public int MyPlayerNumber {get {return (Network.isServer)?1:2;}}

	private Deck deck;
	private int nbPlayerReadyToFight = 0;
	public Dictionary<Point, Tile> board = new Dictionary<Point, Tile>();

	void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ArrangeCards()
	{
		if (Network.isServer)
		{
			locations = GameObject.FindGameObjectsWithTag("Column1");
		}
		else
		{
			locations = GameObject.FindGameObjectsWithTag("Column8");
		}

		foreach (GameObject location in locations) 
		{
			int line = int.Parse(location.name.Substring(6)) + 1;
			if (deck.Cards.Count >= line)
			{
				NetworkViewID viewID = Network.AllocateViewID();
				networkView.RPC("SpawnCard", RPCMode.AllBuffered, viewID, deck.Cards[line - 1].Id, location.transform.position + new Vector3(0, 0, -1), location.transform.rotation * Quaternion.Euler(0, -90, 0));
			}
		}
	}

	public IEnumerator AddCardToBoard()
	{
		if (Network.isServer)
		{
			this.deck = new Deck(2);
		}
		else 
		{
			this.deck = new Deck(1);
		}

		yield return StartCoroutine(deck.RetrieveCards());

		ArrangeCards();
	}

	public void StartFight()
	{
		nbPlayerReadyToFight++;

		if (nbPlayerReadyToFight == 2)
		{  
			TimeOfPositionning = false;
			if (GameTimeLine.instance.PlayingCard.ownerNumber == MyPlayerNumber)
			{
				GameScript.instance.labelText = "A vous de jouer";
			}
			else
			{
				GameScript.instance.labelText = "Au joueur adverse de jouer";
			}
		}
	}

	[RPC]
	IEnumerator SpawnCard(NetworkViewID viewID, int cardID, Vector3 location, Quaternion rotation)
	{
		GameObject clone;
		clone = Instantiate(Card, location, rotation) as GameObject;
		NetworkView nView;

		nView = clone.GetComponent<NetworkView>();
		nView.viewID = viewID;
		GameNetworkCard gCard = clone.GetComponent<GameNetworkCard>();
		if (gCard.networkView.isMine && Network.isServer || !gCard.networkView.isMine && Network.isClient)
		{
			gCard.ownerNumber = 1;
		}
		else
		{
			gCard.ownerNumber = 2;
		}
		yield return StartCoroutine(gCard.RetrieveCard(cardID));
		clone.name = gCard.Card.Title + "-" + gCard.ownerNumber;
		clone.tag = "PlayableCard";
		GameTimeLine.instance.GameCards.Add(gCard);
		GameTimeLine.instance.SortCardsBySpeed();
		GameTimeLine.instance.Arrange();
		gCard.ShowFace();
	}
}

