using UnityEngine;
using System.Collections;

public class GameEventController : MonoBehaviour
{
	public GameEventView gameEventView;
	public GUIStyle textStyle;
	public GUIStyle backgroundStyle;

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
		transform.LookAt(camera.transform);
		Vector3 v3 = new Vector3(Screen.width * 0.05f, Screen.height * 3 / 2, 0);

		transform.position = camera.ScreenToWorldPoint(v3);

		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		transform.Translate((-transform.up + -transform.up * 0.1f) * count, Space.World);

		Vector3 reverse = camera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));

		Rect r = new Rect(reverse.x + v3.x, Screen.height - reverse.y, 100, 20);
		Debug.Log("julien" + r.y);
		setInfoRect(r);
	}

	public string getAction()
	{
		return gameEventView.gameEventVM.action;
	}

	public void setAction(string action)
	{
		gameEventView.gameEventVM.action = action;
	}

	public void addAction(string action)
	{
		gameEventView.gameEventVM.action += " puis " + action;
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
