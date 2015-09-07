using UnityEngine;
using TMPro;

public class TutorialBackgroundController : MonoBehaviour 
{
	
	void Awake()
	{
	}

	public void resize (Rect rect, float clickableSectionXRatio, float clickableSectionYRatio)
	{

		float worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		float worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;

		int backgroundSizeX = 1920;
		int backgroundSizeY = 1080;
		int circleBackgroundSizeX = 1000;
		int circleBackgroundSizeY = 1000;

		int pixelPerUnit = 108;

		float backgroundWorldSizeX = (float)backgroundSizeX / (float)pixelPerUnit;
		float backgroundWorldSizeY = (float)backgroundSizeY / (float)pixelPerUnit;

		float circleBackgroundWorldSizeX=(float)circleBackgroundSizeX/(float)pixelPerUnit;
		float circleBackgroundWorldSizeY=(float)circleBackgroundSizeY/(float)pixelPerUnit;

		float clickableSectionX = circleBackgroundWorldSizeX * clickableSectionXRatio;
		float clickableSectionY = circleBackgroundWorldSizeY * clickableSectionYRatio;

		float XOriginDistance = Mathf.Abs (-worldWidth / 2 - rect.x);
		float YOriginDistance = Mathf.Abs (worldHeight / 2 - rect.y);

		float leftBackgroundWidth = XOriginDistance - rect.width / 2;
		float leftBackgroundScaleX = leftBackgroundWidth / backgroundWorldSizeX;
		float leftBackgroundHeight = worldHeight;
		float leftBackgroundX = -worldWidth / 2f + leftBackgroundWidth/2f;
		float leftBackgroundY = 0f;

		float rightBackgroundWidth = (worldWidth - XOriginDistance) - rect.width / 2f;
		float rightBackgroundScaleX = rightBackgroundWidth / backgroundWorldSizeX;
		float rightBackgroundHeight = worldHeight;
		float rightBackgroundX = worldWidth / 2f - rightBackgroundWidth/2f;
		float rightBackgroundY = 0f;

		float upperBackgroundWidth = rect.width;
		float upperBackgroundScaleX = upperBackgroundWidth / backgroundWorldSizeX;
		float upperBackgroundHeight = YOriginDistance - rect.height / 2f;
		float upperBackgroundX = rect.x;
		float upperBackgroundY = worldHeight / 2f - upperBackgroundHeight / 2f;

		float lowerBackgroundWidth = rect.width;
		float lowerBackgroundScaleX = lowerBackgroundWidth / backgroundWorldSizeX;
		float lowerBackgroundHeight = worldHeight - upperBackgroundHeight - rect.height;
		float lowerBackgroundX = rect.x;
		float lowerBackgroundY = -worldHeight / 2f + lowerBackgroundHeight / 2f;

		float circleBackgroundWidth = rect.width;
		float circleBackgroundHeight = rect.height;
		float circleBackgroundX = rect.x;
		float circleBackgroundY = rect.y;

		gameObject.transform.FindChild("leftBackground").transform.position=new Vector3(leftBackgroundX,leftBackgroundY,-3f);
		gameObject.transform.FindChild ("leftBackground").transform.localScale = new Vector3 (leftBackgroundScaleX, leftBackgroundHeight / backgroundWorldSizeY, 1f);

		gameObject.transform.FindChild("rightBackground").transform.position=new Vector3(rightBackgroundX,rightBackgroundY,-3f);
		gameObject.transform.FindChild ("rightBackground").transform.localScale = new Vector3 (rightBackgroundScaleX, rightBackgroundHeight / backgroundWorldSizeY, 1f);

		gameObject.transform.FindChild("upperBackground").transform.position=new Vector3(upperBackgroundX,upperBackgroundY,-3f);
		gameObject.transform.FindChild ("upperBackground").transform.localScale = new Vector3 (upperBackgroundScaleX, upperBackgroundHeight / backgroundWorldSizeY, 1f);

		gameObject.transform.FindChild("lowerBackground").transform.position=new Vector3(lowerBackgroundX,lowerBackgroundY,-3f);
		gameObject.transform.FindChild ("lowerBackground").transform.localScale = new Vector3 (lowerBackgroundScaleX, lowerBackgroundHeight / backgroundWorldSizeY, 1f);

		gameObject.transform.FindChild ("circleBackground").transform.position = new Vector3 (circleBackgroundX, circleBackgroundY, -3f);
		gameObject.transform.FindChild ("circleBackground").transform.localScale = new Vector3 (circleBackgroundWidth / circleBackgroundWorldSizeX, circleBackgroundHeight/circleBackgroundWorldSizeY, 1f);

		BoxCollider2D[] colliders = gameObject.transform.FindChild("circleBackground").GetComponents<BoxCollider2D>();

		Vector2 upperColliderWorldSize = new Vector2 (circleBackgroundWorldSizeX, (circleBackgroundWorldSizeY - clickableSectionY) / 2f);
		Vector2 upperColliderWorldOffset = new Vector2 (0f,clickableSectionY/2f+upperColliderWorldSize.y/2f);

		Vector2 lowerColliderWorldSize = new Vector2 (circleBackgroundWorldSizeX, (circleBackgroundWorldSizeY - clickableSectionY) / 2f);
		Vector2 lowerColliderWorldOffset = new Vector2 (0f,-clickableSectionY/2f-lowerColliderWorldSize.y/2f);

		Vector2 leftColliderWorldSize = new Vector2 ((circleBackgroundWorldSizeX-clickableSectionX)/2f,clickableSectionY);
		Vector2 leftColliderWorldOffset = new Vector2 (-clickableSectionX/2f-leftColliderWorldSize.x/2f,0f);

		Vector2 rightColliderWorldSize = new Vector2 ((circleBackgroundWorldSizeX-clickableSectionX)/2f,clickableSectionY);
		Vector2 rightColliderWorldOffset = new Vector2 (clickableSectionX/2f+rightColliderWorldSize.x/2f,0f);

		colliders[0].size = new Vector2 ((upperColliderWorldSize.x / circleBackgroundWorldSizeX) * ((float)circleBackgroundSizeX / (float)pixelPerUnit), (upperColliderWorldSize.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));
		colliders[0].offset = new Vector2 ((upperColliderWorldOffset.x / circleBackgroundWorldSizeX) * (circleBackgroundSizeX / (float)pixelPerUnit), (upperColliderWorldOffset.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));

		colliders[1].size = new Vector2 ((lowerColliderWorldSize.x / circleBackgroundWorldSizeX) * ((float)circleBackgroundSizeX / (float)pixelPerUnit), (lowerColliderWorldSize.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));
		colliders[1].offset = new Vector2 ((lowerColliderWorldOffset.x / circleBackgroundWorldSizeX) * (circleBackgroundSizeX / (float)pixelPerUnit), (lowerColliderWorldOffset.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));

		colliders[2].size = new Vector2 ((leftColliderWorldSize.x / circleBackgroundWorldSizeX) * ((float)circleBackgroundSizeX / (float)pixelPerUnit), (leftColliderWorldSize.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));
		colliders[2].offset = new Vector2 ((leftColliderWorldOffset.x / circleBackgroundWorldSizeX) * (circleBackgroundSizeX / (float)pixelPerUnit), (leftColliderWorldOffset.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));

		colliders[3].size = new Vector2 ((rightColliderWorldSize.x / circleBackgroundWorldSizeX) * ((float)circleBackgroundSizeX / (float)pixelPerUnit), (rightColliderWorldSize.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));
		colliders[3].offset = new Vector2 ((rightColliderWorldOffset.x / circleBackgroundWorldSizeX) * ((float)circleBackgroundSizeX / (float)pixelPerUnit), (rightColliderWorldOffset.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));
	}
}

