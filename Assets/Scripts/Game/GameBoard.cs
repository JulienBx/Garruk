using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBoard : Photon.MonoBehaviour 
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
	public int gridWidthInHexes = 5, gridHeightInHexes = 0;
	public int nbPlayer = 0;
	public int MyPlayerNumber {get {return nbPlayer;}}
	public int nbCardsPlayer1 = 0, nbCardsPlayer2 = 0;


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
		if (GameBoard.instance.nbPlayer == 1)
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
				int viewID = PhotonNetwork.AllocateViewID();
				photonView.RPC("SpawnCard", PhotonTargets.AllBuffered, viewID, deck.Cards[line - 1].Id, location.transform.position + new Vector3(0, 0, -1), location.transform.rotation * Quaternion.Euler(0, -90, 0));
			}
		}
	}

	public IEnumerator AddCardToBoard()
	{
		if (GameBoard.instance.nbPlayer == 1)
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
	IEnumerator SpawnCard(int viewID, int cardID, Vector3 location, Quaternion rotation)
	{
		GameObject clone;
		clone = Instantiate(Card, location, rotation) as GameObject;
		PhotonView nView;

		nView = clone.GetComponent<PhotonView>();
		nView.viewID = viewID;
		GameNetworkCard gCard = clone.GetComponent<GameNetworkCard>();
		if (gCard.photonView.isMine && GameBoard.instance.nbPlayer == 1 || !gCard.photonView.isMine && GameBoard.instance.nbPlayer == 2)
		{
			gCard.ownerNumber = 1;
			nbCardsPlayer1++;
		}
		else
		{
			gCard.ownerNumber = 2;
			nbCardsPlayer2++;
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

