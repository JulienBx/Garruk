using UnityEngine;

public static class Utils
{
	public static Vector3 getGOScreenPosition(Vector3 pos)
	{
		float worldHeight;
		if (Camera.main.camera.isOrthoGraphic)
		{
			worldHeight = 2f * Camera.main.camera.orthographicSize;
		} else
		{
			float distance = Mathf.Abs(Camera.main.camera.transform.position.z);
			float radians = (Camera.main.camera.fieldOfView / 2f) * (Mathf.PI / 180f);
			worldHeight = 2f * (distance * Mathf.Tan(radians));
		}
		float worldWidth = ((float)Screen.width / (float)Screen.height) * worldHeight;
		return new Vector3((worldWidth / 2f + pos.x) * (float)Screen.width / worldWidth, (worldHeight / 2f + pos.y) * (float)Screen.height / worldHeight, 0);
	}
}
