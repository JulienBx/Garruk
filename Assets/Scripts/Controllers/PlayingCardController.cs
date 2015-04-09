using UnityEngine;
using System.Collections;

public class PlayingCardController : MonoBehaviour 
{
	public PlayingCardController instance;
	public bool isMine;
	public bool isLoaded;
	public bool hasPlayed = false;                                          	// seulement utile pour la timeline
	public Card card;

	public void Start()
	{
		instance = this;
	}

	public IEnumerator retrieveCard(int idCard)
	{
		yield return StartCoroutine(PlayingCardManager.instance.RetrieveCard(idCard, card));
	}
}
