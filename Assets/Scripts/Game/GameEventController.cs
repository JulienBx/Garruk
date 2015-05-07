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
		Vector3 v3 = new Vector3(Screen.width * 0.05f, Screen.height, 0);
		Ray ray = camera.ScreenPointToRay(v3);
		Vector3 newPosition = ray.GetPoint(Vector3.Distance(Vector3.zero, camera.transform.position));

		transform.LookAt(camera.transform);
		transform.position = newPosition;
		transform.Translate((-transform.up + -transform.up * 0.1f) * count, Space.World);

		Vector3 reverse = camera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));
		Rect r = new Rect(reverse.x + v3.x, Screen.height - reverse.y, 200, 50);
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
