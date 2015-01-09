using UnityEngine;
using System.Collections;

public class GameCard : MonoBehaviour {

	public GameObject front;
	public Card Card;


	public GameCard(GameObject front, Card card) {
		this.front = front;
		this.Card = card;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
