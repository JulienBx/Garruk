using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameTimeLine : MonoBehaviour {

	public static GameTimeLine instance;
	public List<GameObject> GameObjects;
	public List<GameNetworkCard> GameCards;

	private int startPosition = 4;
	private int position = 0;
	private int playingCardPosition = 4;

	public GameNetworkCard PlayingCard
	{
		get { 
			GameNetworkCard currentCardInTimeLine = GameObjects[playingCardPosition].GetComponent<GameNetworkCard>();
			GameNetworkCard playingCard = GameObject.Find(currentCardInTimeLine.Card.Title + "-" + currentCardInTimeLine.ownerNumber).GetComponent<GameNetworkCard>();
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
		if (startPosition > 0)
		{
			startPosition--;
		} else
		{
			GameCards.Add(GameCards[0]);
			GameCards.RemoveAt(0);
		}
		Arrange();
	}

	public void Arrange()
	{
		position = 0;
		for (int i = startPosition ; i < GameObjects.Count ; i++) 
		{
			GameNetworkCard gCard = GameObjects[i].GetComponent<GameNetworkCard>();
			gCard.Card = GameCards[position].Card;
			gCard.ownerNumber = GameCards[position].ownerNumber;
			position++;
			if (position + 1 > GameCards.Count)
			{
				position = 0;
			}

			GameOutline.instance.ToArrange = true;
			gCard.ShowFace();
		}
		GamePlayingCard.instance.ChangeCurrentCard(GameObjects[playingCardPosition].GetComponent<GameNetworkCard>());
		GameObject go = GameObject.Find(PlayingCard.Card.Title + "-" + WhosNext);
		go.GetComponent<GameNetworkCard>().FindNeighbors();
	}

	public void SortCardsBySpeed ()
	{
		GameCards = GameCards.OrderBy(e => e.Card.Speed).ThenBy(e => e.Card.Id).ToList();
	}
}
