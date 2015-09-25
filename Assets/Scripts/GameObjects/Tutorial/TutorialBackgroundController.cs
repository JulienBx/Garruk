using UnityEngine;
using TMPro;

public class TutorialBackgroundController : MonoBehaviour 
{

	public Vector2 topColliderSize;
	public Vector2 topColliderOffset;

	public Vector2 bottomColliderSize;
	public Vector2 bottomColliderOffset;

	public Vector2 leftColliderSize;
	public Vector2 leftColliderOffset;

	public Vector2 rightColliderSize;
	public Vector2 rightColliderOffset;


	void Awake()
	{
	}


	public virtual void resize(Rect rect, float clickableSectionXRatio, float clickableSectionYRatio)
	{
		float worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		float worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		int backgroundSizeX = 4000;
		int backgroundSizeY = 3000;
		int circleBackgroundSizeX = 200;
		int circleBackgroundSizeY = 200;
		int pixelPerUnit = 108;
		float backgroundWorldSizeX = (float)backgroundSizeX / (float)pixelPerUnit;
		float backgroundWorldSizeY = (float)backgroundSizeY / (float)pixelPerUnit;
		float circleBackgroundWorldSizeX=(float)circleBackgroundSizeX/(float)pixelPerUnit;
		float circleBackgroundWorldSizeY=(float)circleBackgroundSizeY/(float)pixelPerUnit;

		float clickableSectionX = circleBackgroundWorldSizeX * clickableSectionXRatio;
		float clickableSectionY = circleBackgroundWorldSizeY * clickableSectionYRatio;

		Vector3 scale = new Vector3 (rect.width / circleBackgroundWorldSizeX, rect.height / circleBackgroundWorldSizeY, 1f);

		gameObject.transform.position=new Vector3(rect.x,rect.y,-9f);
		gameObject.transform.localScale = scale;


		Vector2  upperColliderWorldSize = new Vector2 (backgroundWorldSizeX, (backgroundWorldSizeX - clickableSectionY) / 2f);
		Vector2  upperColliderWorldOffset = new Vector2 (0f,clickableSectionY/2f+upperColliderWorldSize.y/2f);

		Vector2 lowerColliderWorldSize = new Vector2 (backgroundWorldSizeX, (backgroundWorldSizeX - clickableSectionY) / 2f);
		Vector2 lowerColliderWorldOffset = new Vector2 (0f,-clickableSectionY/2f-lowerColliderWorldSize.y/2f);

		Vector2 leftColliderWorldSize = new Vector2 ((backgroundWorldSizeX-clickableSectionX)/2f,clickableSectionY);
		Vector2 leftColliderWorldOffset = new Vector2 (-clickableSectionX/2f-leftColliderWorldSize.x/2f,0f);

		Vector2 rightColliderWorldSize = new Vector2 ((backgroundWorldSizeX-clickableSectionX)/2f,clickableSectionY);
		Vector2 rightColliderWorldOffset = new Vector2 (clickableSectionX/2f+rightColliderWorldSize.x/2f,0f);

		this.topColliderSize = new Vector2 ((upperColliderWorldSize.x / backgroundWorldSizeX) * ((float)backgroundSizeX / (float)pixelPerUnit), (upperColliderWorldSize.y / backgroundWorldSizeY) * ((float)backgroundSizeY / (float)pixelPerUnit));
		this.topColliderOffset = new Vector2 ((upperColliderWorldOffset.x / backgroundWorldSizeX) * (backgroundSizeX / (float)pixelPerUnit), (upperColliderWorldOffset.y / backgroundWorldSizeY) * ((float)backgroundSizeY / (float)pixelPerUnit));

		this.bottomColliderSize = new Vector2 ((lowerColliderWorldSize.x / backgroundWorldSizeX) * ((float)backgroundSizeX / (float)pixelPerUnit), (lowerColliderWorldSize.y / backgroundWorldSizeY) * ((float)backgroundSizeY / (float)pixelPerUnit));				
		this.bottomColliderOffset = new Vector2 ((lowerColliderWorldOffset.x / backgroundWorldSizeX) * (backgroundSizeX / (float)pixelPerUnit), (lowerColliderWorldOffset.y / backgroundWorldSizeY) * ((float)backgroundSizeY / (float)pixelPerUnit));
		
		this.leftColliderSize = new Vector2 ((leftColliderWorldSize.x / backgroundWorldSizeX) * ((float)backgroundSizeX / (float)pixelPerUnit), (leftColliderWorldSize.y / backgroundWorldSizeY) * ((float)backgroundSizeY / (float)pixelPerUnit));
		this.leftColliderOffset = new Vector2 ((leftColliderWorldOffset.x / backgroundWorldSizeX) * (backgroundSizeX / (float)pixelPerUnit), (leftColliderWorldOffset.y / backgroundWorldSizeY) * ((float)backgroundSizeY/ (float)pixelPerUnit));
				
		this.rightColliderSize = new Vector2 ((rightColliderWorldSize.x / backgroundWorldSizeX) * ((float)backgroundSizeX / (float)pixelPerUnit), (rightColliderWorldSize.y / backgroundWorldSizeY) * ((float)backgroundSizeY / (float)pixelPerUnit));
		this.rightColliderOffset = new Vector2 ((rightColliderWorldOffset.x / backgroundWorldSizeX) * ((float)backgroundSizeX / (float)pixelPerUnit), (rightColliderWorldOffset.y / backgroundWorldSizeY) * ((float)backgroundSizeY / (float)pixelPerUnit));
		
		this.computeColliders ();

	}


//	public virtual void resize (Rect rect, float clickableSectionXRatio, float clickableSectionYRatio)
//	{
//		float worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
//		float worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
//
//		int backgroundSizeX = 1920;
//		int backgroundSizeY = 1080;
//		int circleBackgroundSizeX = 1000;
//		int circleBackgroundSizeY = 1000;
//
//		int pixelPerUnit = 108;
//
//		float backgroundWorldSizeX = (float)backgroundSizeX / (float)pixelPerUnit;
//		float backgroundWorldSizeY = (float)backgroundSizeY / (float)pixelPerUnit;
//
//		float circleBackgroundWorldSizeX=(float)circleBackgroundSizeX/(float)pixelPerUnit;
//		float circleBackgroundWorldSizeY=(float)circleBackgroundSizeY/(float)pixelPerUnit;
//
//		float clickableSectionX = circleBackgroundWorldSizeX * clickableSectionXRatio;
//		float clickableSectionY = circleBackgroundWorldSizeY * clickableSectionYRatio;
//
//		float XOriginDistance = Mathf.Abs (-worldWidth / 2 - rect.x);
//		float YOriginDistance = Mathf.Abs (worldHeight / 2 - rect.y);
//
//		float leftBackgroundWidth = XOriginDistance - rect.width / 2;
//		float leftBackgroundScaleX = leftBackgroundWidth / backgroundWorldSizeX;
//		float leftBackgroundHeight = worldHeight;
//		float leftBackgroundX = -worldWidth / 2f + leftBackgroundWidth/2f;
//		float leftBackgroundY = 0f;
//
//		float rightBackgroundWidth = (worldWidth - XOriginDistance) - rect.width / 2f;
//		float rightBackgroundScaleX = rightBackgroundWidth / backgroundWorldSizeX;
//		float rightBackgroundHeight = worldHeight;
//		float rightBackgroundX = worldWidth / 2f - rightBackgroundWidth/2f;
//		float rightBackgroundY = 0f;
//
//		float upperBackgroundWidth = rect.width;
//		float upperBackgroundScaleX = upperBackgroundWidth / backgroundWorldSizeX;
//		float upperBackgroundHeight = YOriginDistance - rect.height / 2f;
//		float upperBackgroundX = rect.x;
//		float upperBackgroundY = worldHeight / 2f - upperBackgroundHeight / 2f;
//
//		float lowerBackgroundWidth = rect.width;
//		float lowerBackgroundScaleX = lowerBackgroundWidth / backgroundWorldSizeX;
//		float lowerBackgroundHeight = worldHeight - upperBackgroundHeight - rect.height;
//		float lowerBackgroundX = rect.x;
//		float lowerBackgroundY = -worldHeight / 2f + lowerBackgroundHeight / 2f;
//
//		float circleBackgroundWidth = rect.width;
//		float circleBackgroundHeight = rect.height;
//		float circleBackgroundX = rect.x;
//		float circleBackgroundY = rect.y;
//
//		gameObject.transform.FindChild("leftBackground").transform.position=new Vector3(leftBackgroundX,leftBackgroundY,-9f);
//		gameObject.transform.FindChild ("leftBackground").transform.localScale = new Vector3 (leftBackgroundScaleX, leftBackgroundHeight / backgroundWorldSizeY, 1f);
//
//		gameObject.transform.FindChild("rightBackground").transform.position=new Vector3(rightBackgroundX,rightBackgroundY,-9f);
//		gameObject.transform.FindChild ("rightBackground").transform.localScale = new Vector3 (rightBackgroundScaleX, rightBackgroundHeight / backgroundWorldSizeY, 1f);
//
//		gameObject.transform.FindChild("upperBackground").transform.position=new Vector3(upperBackgroundX,upperBackgroundY,-9f);
//		gameObject.transform.FindChild ("upperBackground").transform.localScale = new Vector3 (upperBackgroundScaleX, upperBackgroundHeight / backgroundWorldSizeY, 1f);
//
//		gameObject.transform.FindChild("lowerBackground").transform.position=new Vector3(lowerBackgroundX,lowerBackgroundY,-9f);
//		gameObject.transform.FindChild ("lowerBackground").transform.localScale = new Vector3 (lowerBackgroundScaleX, lowerBackgroundHeight / backgroundWorldSizeY, 1f);
//
//		gameObject.transform.FindChild ("circleBackground").transform.position = new Vector3 (circleBackgroundX, circleBackgroundY, -9f);
//		gameObject.transform.FindChild ("circleBackground").transform.localScale = new Vector3 (circleBackgroundWidth / circleBackgroundWorldSizeX, circleBackgroundHeight/circleBackgroundWorldSizeY, 1f);
//
//
//
//		Vector2  upperColliderWorldSize = new Vector2 (circleBackgroundWorldSizeX, (circleBackgroundWorldSizeY - clickableSectionY) / 2f);
//		Vector2 upperColliderWorldOffset = new Vector2 (0f,clickableSectionY/2f+upperColliderWorldSize.y/2f);
//
//		Vector2 lowerColliderWorldSize = new Vector2 (circleBackgroundWorldSizeX, (circleBackgroundWorldSizeY - clickableSectionY) / 2f);
//		Vector2 lowerColliderWorldOffset = new Vector2 (0f,-clickableSectionY/2f-lowerColliderWorldSize.y/2f);
//
//		Vector2 leftColliderWorldSize = new Vector2 ((circleBackgroundWorldSizeX-clickableSectionX)/2f,clickableSectionY);
//		Vector2 leftColliderWorldOffset = new Vector2 (-clickableSectionX/2f-leftColliderWorldSize.x/2f,0f);
//
//		Vector2 rightColliderWorldSize = new Vector2 ((circleBackgroundWorldSizeX-clickableSectionX)/2f,clickableSectionY);
//		Vector2 rightColliderWorldOffset = new Vector2 (clickableSectionX/2f+rightColliderWorldSize.x/2f,0f);
//
//		this.topColliderSize = new Vector2 ((upperColliderWorldSize.x / circleBackgroundWorldSizeX) * ((float)circleBackgroundSizeX / (float)pixelPerUnit), (upperColliderWorldSize.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));
//		this.topColliderOffset = new Vector2 ((upperColliderWorldOffset.x / circleBackgroundWorldSizeX) * (circleBackgroundSizeX / (float)pixelPerUnit), (upperColliderWorldOffset.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));
//
//		this.bottomColliderSize = new Vector2 ((lowerColliderWorldSize.x / circleBackgroundWorldSizeX) * ((float)circleBackgroundSizeX / (float)pixelPerUnit), (lowerColliderWorldSize.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));
//		this.bottomColliderOffset = new Vector2 ((lowerColliderWorldOffset.x / circleBackgroundWorldSizeX) * (circleBackgroundSizeX / (float)pixelPerUnit), (lowerColliderWorldOffset.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));
//
//		this.leftColliderSize = new Vector2 ((leftColliderWorldSize.x / circleBackgroundWorldSizeX) * ((float)circleBackgroundSizeX / (float)pixelPerUnit), (leftColliderWorldSize.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));
//		this.leftColliderOffset = new Vector2 ((leftColliderWorldOffset.x / circleBackgroundWorldSizeX) * (circleBackgroundSizeX / (float)pixelPerUnit), (leftColliderWorldOffset.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));
//		
//		this.rightColliderSize = new Vector2 ((rightColliderWorldSize.x / circleBackgroundWorldSizeX) * ((float)circleBackgroundSizeX / (float)pixelPerUnit), (rightColliderWorldSize.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));
//		this.rightColliderOffset = new Vector2 ((rightColliderWorldOffset.x / circleBackgroundWorldSizeX) * ((float)circleBackgroundSizeX / (float)pixelPerUnit), (rightColliderWorldOffset.y / circleBackgroundWorldSizeY) * ((float)circleBackgroundSizeY / (float)pixelPerUnit));
//
//		this.computeColliders ();
//	}

	public virtual void computeColliders()
	{
		BoxCollider2D[] colliders = gameObject.transform.GetComponents<BoxCollider2D>();
		colliders [0].size = this.topColliderSize;
		colliders [0].offset = this.topColliderOffset;
		colliders [1].size = this.bottomColliderSize;
		colliders [1].offset = this.bottomColliderOffset;
		colliders [2].size = this.leftColliderSize;
		colliders [2].offset = this.leftColliderOffset;
		colliders [3].size = this.rightColliderSize;
		colliders [3].offset = this.rightColliderOffset;
	}

}

