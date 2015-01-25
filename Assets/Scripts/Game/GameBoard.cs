using UnityEngine;
using System.Collections;

public class GameBoard : MonoBehaviour {

	private Deck deck;
	private Deck opponentDeck;
	public GameObject[] locations;
	public GameObject Card;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ArrangeCards(bool leftSide)
	{
		if (leftSide)
		{
			locations = GameObject.FindGameObjectsWithTag("Column1");
		}
		else
		{
			locations = GameObject.FindGameObjectsWithTag("Column8");
		}
		Deck currentDeck;
		if ((Network.isServer && leftSide) || (Network.isClient && !leftSide))
		{
			currentDeck = deck;
		}
		else 
		{
			currentDeck = opponentDeck;
		}

		foreach (GameObject location in locations) 
		{
			int line = int.Parse(location.name.Substring(4));
			if (currentDeck.Cards.Count >= line)
			{
				GameObject instance = Network.Instantiate(Card, location.transform.position, location.transform.rotation * Quaternion.Euler(0, -90, 0), 0) as GameObject;
				GameCard gCard = instance.GetComponent<GameCard>();
				gCard.Card = currentDeck.Cards[line - 1];
				gCard.ShowFace();
				instance.transform.parent = location.transform;
			}
		}
	}

	public void AddCardToBoard()
	{
		//TODO Comportement à changer
		if (Network.isServer)
		{
			networkView.RPC("Proceed", RPCMode.AllBuffered, Network.isServer, 1);
		}
		else 
		{
			networkView.RPC("Proceed", RPCMode.AllBuffered, Network.isServer, 2);
		}
	}

	[RPC]
	IEnumerator Proceed(bool leftSide, int deckId)
	{
		if ((Network.isServer && leftSide) || (Network.isClient && !leftSide))
		{
			deck = new Deck(deckId);
			yield return StartCoroutine(deck.RetrieveCards());

		}
		else
		{
			opponentDeck = new Deck(deckId);
			yield return StartCoroutine(opponentDeck.RetrieveCards());
		}
		ArrangeCards(leftSide);
	}
}

