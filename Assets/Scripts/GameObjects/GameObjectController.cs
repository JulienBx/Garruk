using UnityEngine;

public class GameObjectController : MonoBehaviour {
	
	public Vector2 GOPosition;
	public Vector2 GOSize;

	public void getGOCoordinates(GameObject gameobject)
	{
		this.getGOScreenSize(gameobject);
		this.getGOScreenPosition(gameobject);
	}
	public void getGOScreenPosition(GameObject gameobject)
	{
		Vector2 position = new Vector2 (gameobject.transform.position.x,gameobject.transform.position.y);
		float worldHeight = 2f*Camera.main.camera.orthographicSize;
		float worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		GOPosition.x = (worldWidth / 2f + position.x) * (float)Screen.width / worldWidth;
		GOPosition.y = (worldHeight / 2f + position.y) * (float)Screen.height / worldHeight;
	}
	public void getGOScreenSize(GameObject gameobject)
	{
		Vector2 size = new Vector2 (gameobject.GetComponent<Renderer> ().bounds.size.x,gameobject.GetComponent<Renderer> ().bounds.size.y);
		float worldHeight = 2f*Camera.main.camera.orthographicSize;
		float worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		GOSize.x = (size.x / worldWidth) * (float)Screen.width;
		GOSize.y = (size.y / worldHeight) * (float)Screen.height;
	}
}