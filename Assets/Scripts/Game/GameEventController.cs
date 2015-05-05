using UnityEngine;
using System.Collections;

public class GameEventController : MonoBehaviour
{
	public GameEventView gameEventView;
	public GUIStyle textStyle;

	public string getCharacterName()
	{
		return gameEventView.gameEventVM.characterName;
	}

	public void setCharacterName(string title)
	{
		gameEventView.gameEventVM.characterName = title;
	}

	public void setScreenPosition(int count)
	{
		Camera camera = Camera.main;
		Camera.main.WorldToScreenPoint(new Vector3(1f, 1f, 0f));
		
		//Vector3 v3 = new Vector3(Screen.width * 0.05f, Screen.height + height * count, 0);
		
		//transform.position = camera.ScreenToWorldPoint(v3);
		//transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		//Rect r = new Rect(v3.x, v3.y, getWidth(), getHeight());
		
		//setInfoRect(r);
	}

	public string getAction()
	{
		return gameEventView.gameEventVM.action;
	}

	public void setAction(string action)
	{
		gameEventView.gameEventVM.action = action;
	}

	public void setArt(Texture art)
	{
		gameEventView.gameEventVM.art = art;
	}

	public void setInfoRect(Rect r)
	{
		gameEventView.gameEventVM.infoRect = r;
	}

	public float getWidth()
	{
		return gameEventView.gameEventVM.width * gameEventView.gameEventVM.scale.x;
	}

	public float getHeight()
	{
		return gameEventView.gameEventVM.height * gameEventView.gameEventVM.scale.y;
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
