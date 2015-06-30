﻿using UnityEngine;
using System.Collections;

public class GameEventController : MonoBehaviour
{
	public GameObject darkImage;
	public GameObject transparentImage;
	public GameEventView gameEventView;
	public GUIStyle textStyle;
	public GUIStyle backgroundStyle;
	public Texture2D[] borders;
	public int IDCharacter;

	public string getCharacterName()
	{
		return gameEventView.gameEventVM.characterName;
	}

	public void setCharacterName(string title)
	{
		gameEventView.gameEventVM.characterName = title;
	}

	public void setScreenPosition(int count, int boardWidth, int boardHeight, float scaleTile)
	{
		Camera camera = Camera.main;
		Vector3 currentScale = renderer.bounds.size;
		//Vector3 v3 = new Vector3((0.02f + scaleTile) * (- boardWidth / 2 - 0.5f), currentScale.y * GameController.instance.eventMax / 2 + 0.1f, -1f);
		//Vector3 newPosition = camera.ScreenToWorldPoint(v3);
		
		//transform.position = v3;
		transform.Translate((-transform.up * transform.localScale.y - transform.up * 0.1f) * (count - 1), Space.World);

		Vector3 reverse = Utils.getGOScreenPosition(new Vector3(transform.position.x + 0.54f, transform.position.y + 0.35f, transform.position.z));
		Rect r = new Rect(reverse.x, Screen.height - reverse.y, 200, 50);
		setInfoRect(r);
	}

	public string getAction()
	{
		return gameEventView.gameEventVM.action;
	}

	public Texture2D getArt()
	{
		return gameEventView.gameEventVM.art;
	}

	public void setAction(string action)
	{
		gameEventView.gameEventVM.action = action;
	}

	public void addAction(string action)
	{
		gameEventView.gameEventVM.action += " puis " + action;
	}

	public void setTarget(string target)
	{
		gameEventView.gameEventVM.action += " " + target;
	}

	public void setArt(Texture2D art)
	{
		gameEventView.gameEventVM.art = art;
	}

	public void setBorder(int isMine)
	{
		if (isMine == 1)
		{
			gameEventView.gameEventVM.border = borders [1];
		} else if (isMine == - 1)
		{
			gameEventView.gameEventVM.border = borders [2];
		} else
		{
			gameEventView.gameEventVM.border = borders [0];
		}
	}

	public void setMovement(GameObject origin, GameObject destination)
	{
		gameEventView.gameEventVM.origin = origin;
		gameEventView.gameEventVM.destination = destination;
	}

	public void setInfoRect(Rect r)
	{
		gameEventView.gameEventVM.infoRect = r;
	}

	public bool hasMouvementType()
	{
		return gameEventView.gameEventVM.origin != null;
	}

	public float getWidth()
	{
		return gameEventView.gameEventVM.width * gameEventView.gameEventVM.scale.x;
	}

	public float getHeight()
	{
		return gameEventView.gameEventVM.height * gameEventView.gameEventVM.scale.y;
	}

	public GameObject getOrigin()
	{
		return gameEventView.gameEventVM.origin;
	}

	public GameObject getDestination()
	{
		return gameEventView.gameEventVM.destination;
	}

	public GameObject[] getMovement()
	{
		return new GameObject[]{getOrigin(), getDestination()};
	}

	void Awake()
	{
		gameEventView = gameObject.AddComponent <GameEventView>();
		gameEventView.gameEventVM.nameTextStyle = textStyle;
		gameEventView.gameEventVM.backgroundStyle = backgroundStyle;
	}
	// Use this for initialization
	void Start()
	{

	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
}
