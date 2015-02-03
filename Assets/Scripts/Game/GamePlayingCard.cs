using UnityEngine;
using System.Collections;

public class GamePlayingCard : MonoBehaviour {
	
	public static GamePlayingCard instance;
	public GameCard gameCard;

	Vector3 WorldNamePos;


	public int Damage = 0;
	public Texture2D bgImage; 
	public Texture2D fgImage;
	public GUIStyle progress_empty, progress_full;

	public void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {
		Vector3 GameCardPosition = transform.Find("Life/Life Bar").position;
		WorldNamePos = Camera.main.camera.WorldToScreenPoint(GameCardPosition);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

	void OnGUI()
	{
		if (!GameBoard.instance.TimeOfPositionning)
		{
			if (GameBoard.instance.MyPlayerNumber == gameCard.ownerNumber)
			{
				GameScript.instance.labelText = "A vous de jouer";
				if (GUI.Button(new Rect(37, 505, 67, 25), "Passer"))
				{
					networkView.RPC("forwardInTime", RPCMode.AllBuffered); 
				}
			} else
			{
				GameScript.instance.labelText = "Au joueur adverse de jouer";
			}
		}
		if (this.gameCard.Card != null)
		{



			GUI.BeginGroup(new Rect(WorldNamePos.x, Screen.height - WorldNamePos.y, 16, 50));
				GUI.Box(new Rect(0,0,16,50), bgImage, progress_empty);
				GUI.BeginGroup(new Rect(0, 0, 16, 50));
				GUI.Box (new Rect(0,0,8,50), fgImage, progress_full);
				GUI.EndGroup();
			GUI.EndGroup();
		}

	}
	public void ChangeCurrentCard(GameCard card)
	{
		gameCard.Card = card.Card;
		gameCard.ownerNumber = card.ownerNumber;
		changeStats();
		gameCard.ShowFace();
	}

	public void changeStats()
	{
		Transform attackText = transform.Find("Icons/Attack/Value");
		attackText.GetComponent<TextMesh>().text = this.gameCard.Card.Attack.ToString();

		Transform energyText = transform.Find("Icons/Energy/Value");
		energyText.GetComponent<TextMesh>().text = this.gameCard.Card.Energy.ToString();

		Transform moveText = transform.Find("Icons/Move/Value");
		moveText.GetComponent<TextMesh>().text = this.gameCard.Card.Move.ToString();

		Transform speedText = transform.Find("Icons/Speed/Value");
		speedText.GetComponent<TextMesh>().text = this.gameCard.Card.Speed.ToString();

		int i = 1;
		if (gameCard.Card.Skills != null)
		{
			foreach (Skill skill in gameCard.Card.Skills)
			{
				Transform skillText = transform.Find("Skills/Skill " + i++);
				skillText.GetComponent<TextMesh>().text = skill.Name;
			}
		}
	}

	[RPC]
	private void forwardInTime()
	{
		GameTimeLine.instance.forward();
	}
}
