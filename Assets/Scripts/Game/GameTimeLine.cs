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
			GameNetworkCard currentCardInTimeLine = GameObjects[playingCardPosition].GetComponent<GameNetworkCard>();
			GameObject go = GameObject.Find(currentCardInTimeLine.Card.Title + "-" + currentCardInTimeLine.ownerNumber);
			GameNetworkCard playingCard = null;
			if (go != null)
			{
				playingCard = go.GetComponent<GameNetworkCard>();
			}
			return playingCard; }
	}
	
	public GameObject PlayingCardObject
	{
		get { 
			GameNetworkCard currentCardInTimeLine = GameObjects[playingCardPosition].GetComponent<GameNetworkCard>();
			GameObject playingCard = GameObject.Find(currentCardInTimeLine.Card.Title + "-" + currentCardInTimeLine.ownerNumber);
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
		Arrange();
	}
	
	public void Arrange()
	{

		int countPlayedCards = playedCards.Count;

		for (int i = countPlayedCards - 1; countPlayedCards - i - 1 < playingCardPosition && i >= 0; i--)
		{
			GameNetworkCard gCard = GameObjects [playingCardPosition - (countPlayedCards - i)].GetComponent<GameNetworkCard>();
			gCard.Card = playedCards[i].Card;
			gCard.ownerNumber = playedCards[i].ownerNumber;
			GameOutline.instance.ToArrange = true;
			gCard.ShowFace();
		}
		int position = 0;

		for (int i = position; i < GameObjects.Count - playingCardPosition && i < GameCards.Count; i++)
		{
			GameNetworkCard gCard = GameObjects [i + playingCardPosition].GetComponent<GameNetworkCard>();
			gCard.Card = GameCards [i].Card;
			gCard.ownerNumber = GameCards [i].ownerNumber;
			GameOutline.instance.ToArrange = true;
			gCard.ShowFace();
		}
	
		GamePlayingCard.instance.ChangeCurrentCard(GameObjects [playingCardPosition].GetComponent<GameNetworkCard>());
		GameObject go = GameObject.Find(PlayingCard.Card.Title + "-" + WhosNext);
		go.GetComponent<GameNetworkCard>().FindNeighbors();
		
	}
	
	public void SortCardsBySpeed ()
	{
		GameCards = GameCards.OrderByDescending(e => e.Card.GetSpeed()).ThenBy(e => e.Card.Id).ToList();
	}

	public void SortCardsBySpeedAfterBuff ()
	{
		GameNetworkCard temp = GameCards[0];
		GameCards.RemoveAt(0);
		GameCards = GameCards.OrderBy(e => e.nbTurn).ThenByDescending(e => e.Card.GetSpeed()).ThenBy(e => e.Card.Id).ToList();
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
