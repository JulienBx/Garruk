using UnityEngine;
using System.Collections;

public class GameEventController : MonoBehaviour
{
	public GameEventView gameEventView;
	public GUIStyle textStyle;

	public void setCharacterName(string title)
	{
		gameEventView.gameEventVM.characterName = title;
	}

	public void setAction(string action)
	{
		gameEventView.gameEventVM.action = action;
	}

	public void setArt(Texture art)
	{
		gameEventView.gameEventVM.art = art;
	}

	void Awake()
	{
		gameEventView = gameObject.AddComponent <GameEventView>();
		gameEventView.gameEventVM.nameTextStyle = textStyle;
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
