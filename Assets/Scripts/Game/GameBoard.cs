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
	public static Deck deck;
	public int nbTurn;

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
				photonView.RPC("SpawnCard", PhotonTargets.AllBuffered, viewID, deck.Cards[line - 1].Id, location.transform.position + new Vector3(0, 0, -1));
			}
		}
	}

	public IEnumerator AddCardToBoard()
	{
		GameBoard.deck = new Deck(ApplicationModel.username);
		yield return StartCoroutine(GameBoard.deck.LoadSelectedDeck());
		yield return StartCoroutine(deck.RetrieveCards());

		ArrangeCards();
	}

	public void StartFight()
	{
		nbPlayerReadyToFight++;

		if (nbPlayerReadyToFight == 2)
		{  
			nbTurn = 1;
			TimeOfPositionning = false;
			GameTimeLine.instance.PlayingCard.transform.Find("Yellow Outline").renderer.enabled = true;
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
	IEnumerator SpawnCard(int viewID, int cardID, Vector3 location)
	{
		GameObject clone;
		clone = Instantiate(Card, location, Quaternion.identity) as GameObject;
		PhotonView nView;

		nView = clone.GetComponent<PhotonView>();
		nView.viewID = viewID;
		GameCard gCard = clone.GetComponent<GameCard>();
		GameNetworkCard gnCard = clone.GetComponent<GameNetworkCard>();
		clone.transform.localScale = new Vector3(10, 10, 1);
		if (gCard.photonView.isMine && GameBoard.instance.nbPlayer == 1 || !gCard.photonView.isMine && GameBoard.instance.nbPlayer == 2)
		{
			gnCard.ownerNumber = 1;
			nbCardsPlayer1++;
		}
		else
		{
			gnCard.ownerNumber = 2;
			nbCardsPlayer2++;
		}
		if (gCard.photonView.isMine)
		{
			clone.transform.Find("Green Outline").renderer.enabled = true;
		} 
		else
		{
			clone.transform.Find("Red Outline").renderer.enabled = true;
		}
		yield return StartCoroutine(gCard.RetrieveCard(cardID));
		clone.name = gCard.Card.Title + "-" + gnCard.ownerNumber;
		clone.tag = "PlayableCard";
		gCard.ShowFace();
		GameTimeLine.instance.GameCards.Add(gnCard);
		GameTimeLine.instance.SortCardsBySpeed();
		GameTimeLine.instance.removeBarLife();
		GameTimeLine.instance.Arrange();
	}
}

