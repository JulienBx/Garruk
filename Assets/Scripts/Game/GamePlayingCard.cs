using UnityEngine;
using System.Collections;

public class GamePlayingCard : MonoBehaviour {
	
	public static GamePlayingCard instance;
	public GameCard gameCard;

	public void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

	void OnGUI()
	{
		if (GameBoard.instance.MyPlayerNumber == gameCard.ownerNumber)
		{
			if (GUI.Button(new Rect(37, 505, 67, 25), "Passer"))
			{
				networkView.RPC("forwardInTime", RPCMode.AllBuffered); 
			}
		}
	}
	public void ChangeCurrentCard(GameCard card)
	{
		gameCard.Card = card.Card;
		gameCard.ownerNumber = card.ownerNumber;
		gameCard.ShowFace();
	}

	[RPC]
	private void forwardInTime()
	{
		GameTimeLine.instance.forward();
	}
}
