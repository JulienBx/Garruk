using UnityEngine;
using TMPro;

public class HelpBackgroundGameController : MonoBehaviour 
{

	public Sprite[] backgrounds;

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

	public void setSprite(int id)
	{
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = this.backgrounds [id];
	}

	public virtual void resize(Rect rect, float clickableSectionXRatio, float clickableSectionYRatio)
	{
		rect.width = rect.width * 1.025f;
		rect.height = rect.height * 1.025f;

		int backgroundSizeX = 5000;
		int backgroundSizeY = 5000;
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

		gameObject.transform.localPosition=new Vector3(rect.x,rect.y,-9f);
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

	public virtual void computeColliders()
	{
		BoxCollider[] colliders = gameObject.transform.GetComponents<BoxCollider>();
		colliders [0].size = this.topColliderSize;
		colliders [0].center = this.topColliderOffset;
		colliders [1].size = this.bottomColliderSize;
		colliders [1].center = this.bottomColliderOffset;
		colliders [2].size = this.leftColliderSize;
		colliders [2].center = this.leftColliderOffset;
		colliders [3].size = this.rightColliderSize;
		colliders [3].center = this.rightColliderOffset;
	}

}