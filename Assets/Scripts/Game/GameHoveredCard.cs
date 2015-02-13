using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameHoveredCard : MonoBehaviour {
	
	public static GameHoveredCard instance;
	public GameNetworkCard gameCard;

	public void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

	void OnGUI()
	{
	}
	public void ChangeCard(GameNetworkCard card)
	{
		gameObject.SetActive(true);
		gameCard.Card = card.Card;
		gameCard.ownerNumber = card.ownerNumber;
		changeStats();
		gameCard.ShowFace();
	}

	public void hide()
	{
		gameObject.SetActive(false);
	}

	public void changeStats()
	{
		Transform attackText = transform.Find("Icons/Attack/Value");
		attackText.GetComponent<TextMesh>().text = this.gameCard.Card.Attack.ToString();

		//Transform energyText = transform.Find("Icons/Energy/Value");
		//energyText.GetComponent<TextMesh>().text = this.gameCard.Card.Energy.ToString();

		Transform moveText = transform.Find("Icons/Move/Value");
		moveText.GetComponent<TextMesh>().text = this.gameCard.Card.Move.ToString();

		Transform speedText = transform.Find("Icons/Speed/Value");
		speedText.GetComponent<TextMesh>().text = this.gameCard.Card.Speed.ToString();

		

		if (gameCard.Card.Skills != null)
		{
			for (int j = 1 ; j <= 5 ; j++)
			{
				Transform skillText = transform.Find("Skills/Skill " + j++);
				skillText.GetComponent<TextMesh>().text = "";
			}
			int i = 1;
			foreach (Skill skill in gameCard.Card.Skills)
			{
				Transform skillText = transform.Find("Skills/Skill " + i++);
				skillText.GetComponent<TextMesh>().text = skill.Name;
			}
		}
	}
}
