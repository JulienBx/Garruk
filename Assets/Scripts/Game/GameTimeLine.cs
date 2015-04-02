using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameTimeLine : MonoBehaviour {
	
	public static GameTimeLine instance;
	public List<GameObject> GameObjects;
	public List<GameNetworkCard> GameCards;
	public List<GameNetworkCard> playedCards;
	
	private int startPosition = 4;
	private int position = 0;
	private int playingCardPosition = 4;
	
	public GameNetworkCard PlayingCard
	{
		get { 
//			GameNetworkCard currentCardInTimeLine = GameObjects[playingCardPosition].GetComponent<GameNetworkCard>();
//			GameObject go = GameObject.Find(currentCardInTimeLine.gameCard.card.Title + "-" + currentCardInTimeLine.ownerNumber);
//			GameNetworkCard playingCard = null;
//			if (go != null)
//			{
//				playingCard = go.GetComponent<GameNetworkCard>();
//			}
//			return playingCard;
			return null;
		}
	}
	
	public GameObject PlayingCardObject
	{
		get { 
			GameNetworkCard currentCardInTimeLine = GameObjects[playingCardPosition].GetComponent<GameNetworkCard>();
			GameObject playingCard = GameObject.Find(currentCardInTimeLine.gameCard.card.Title + "-" + currentCardInTimeLine.ownerNumber);
			return playingCard; }
	}
	
	public int WhosNext
	{
		get { return GameObjects[playingCardPosition].GetComponent<GameNetworkCard>().ownerNumber; }
	}
	
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
	
	public void forward()
	{
		playedCards.Add(GameCards[0]);
		GameCards[0].nbTurn++;
		
		GameCards.Add(GameCards[0]);
		GameCards.RemoveAt(0);
		GameBoard.instance.nbTurn = GameCards[0].nbTurn;
		//Arrange();
	}
	
	public void Arrange()
	{

		int countPlayedCards = playedCards.Count;
		GameObject go;
		for (int i = countPlayedCards - 1; countPlayedCards - i - 1 < playingCardPosition && i >= 0; i--)
		{
			go = GameObjects [playingCardPosition - (countPlayedCards - i)];
			GameCard gCard = go.GetComponent<GameCard>();
			GameNetworkCard gnCard = go.GetComponent<GameNetworkCard>();

			gCard.Card = playedCards[i].gameCard.card;
			gnCard.ownerNumber = playedCards[i].ownerNumber;
			gnCard.DiscoveryFeature = playedCards[i].DiscoveryFeature;

			GameOutline.instance.ToArrange = true;
			gCard.ShowFace(gnCard.ownerNumber == GameBoard.instance.MyPlayerNumber, gnCard.DiscoveryFeature);
		}
		int position = 0;

		for (int i = position; i < GameObjects.Count - playingCardPosition && i < GameCards.Count; i++)
		{
			go = GameObjects[i + playingCardPosition];
			GameCard gCard = go.GetComponent<GameCard>();
			GameNetworkCard gnCard = go.GetComponent<GameNetworkCard>();

			gCard.Card = GameCards[i].gameCard.card;
			gnCard.ownerNumber = GameCards[i].ownerNumber;
			gnCard.DiscoveryFeature = GameCards[i].DiscoveryFeature;
			GameOutline.instance.ToArrange = true;
			gCard.ShowFace(gnCard.ownerNumber == GameBoard.instance.MyPlayerNumber, gnCard.DiscoveryFeature);
		}
	

		GamePlayingCard.instance.ChangeCurrentCard(GameObjects [playingCardPosition].GetComponent<GameNetworkCard>());
		go = GameObject.Find(PlayingCard.gameCard.card.Title + "-" + WhosNext);
		go.GetComponent<GameNetworkCard>().FindNeighbors();
		
	}
	
	public void SortCardsBySpeed ()
	{
		GameCards = GameCards.OrderByDescending(e => e.gameCard.card.GetSpeed()).ThenBy(e => e.gameCard.card.Id).ToList();
	}

	public void SortCardsBySpeedAfterBuff ()
	{
		GameNetworkCard temp = GameCards[0];
		GameCards.RemoveAt(0);
		GameCards = GameCards.OrderBy(e => e.nbTurn).ThenByDescending(e => e.gameCard.card.GetSpeed()).ThenBy(e => e.gameCard.card.Id).ToList();
		GameCards.Insert(0, temp);
	}
	
	public void removeBarLife()
	{
		foreach (Transform child in transform)
		{
			Transform go = child.Find("Life");
			if (go != null)
			{
				Destroy(go.gameObject);
			}
		}
	}
}
