using UnityEngine;

public class GameObjectController : MonoBehaviour 
{
	public Vector2 GOPosition;
	public Vector2 GOSize;

	public void setGOCoordinates(GameObject gameobject)
	{
		this.setGOScreenSize(gameobject);
		this.setGOScreenPosition(gameobject);
	}
	public void setGOScreenPosition(GameObject gameobject)
	{
		Vector2 position = this.getGOScreenPosition(gameobject);
		GOPosition.x = position.x;
		GOPosition.y = position.y;
	}
	public void setGOScreenSize(GameObject gameobject)
	{
		Vector2 size = this.getGOScreenSize (gameobject);
		GOSize.x = size.x;
		GOSize.y = size.y;
	}
	public Vector2 getGOScreenPosition(GameObject gameobject)
	{
		Vector2 position = new Vector2 (gameobject.transform.position.x,gameobject.transform.position.y);
		float worldHeight;
		if(Camera.main.GetComponent<Camera>().orthographic)
		{
			worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		}
		else
		{
			float distance = Mathf.Abs(Camera.main.GetComponent<Camera>().transform.position.z-gameobject.transform.position.z);
			float radians = (Camera.main.GetComponent<Camera>().fieldOfView/2f) * (Mathf.PI/180f);
			worldHeight=2f*(distance*Mathf.Tan(radians));
		}
		float worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		position.x = (worldWidth / 2f + position.x) * (float)Screen.width / worldWidth;
		position.y = (worldHeight / 2f + position.y) * (float)Screen.height / worldHeight;
		return position;
	}
	public Vector2 getGOScreenSize(GameObject gameobject)
	{
		Vector2 size = new Vector2 (gameobject.GetComponent<Renderer> ().bounds.size.x,gameobject.GetComponent<Renderer> ().bounds.size.y);
		float worldHeight;
		if(Camera.main.GetComponent<Camera>().orthographic)
		{
			worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		}
		else
		{
			float distance = Mathf.Abs(Camera.main.GetComponent<Camera>().transform.position.z-gameobject.transform.position.z);
			float radians = (Camera.main.GetComponent<Camera>().fieldOfView/2f) * (Mathf.PI/180f);
			worldHeight=2f*(distance*Mathf.Tan(radians));
		}
		float worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		size.x = (size.x / worldWidth) * (float)Screen.width;
		size.y = (size.y / worldHeight) * (float)Screen.height;
		return size;
	}
}