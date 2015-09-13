using UnityEngine;
using TMPro;

public class TutorialGameBackgroundController : TutorialBackgroundController
{	
	void Awake()
	{
	}
	
	public override void resize (Rect rect, float clickableSectionXRatio, float clickableSectionYRatio)
	{
		base.resize (rect, clickableSectionXRatio, clickableSectionYRatio);	
	}
	public override void computeColliders()
	{
		BoxCollider[] colliders = gameObject.transform.FindChild("circleBackground").GetComponents<BoxCollider>();
		colliders [0].size = new Vector3( this.topColliderSize.x,this.topColliderSize.y,0.2f);
		colliders [0].center = new Vector3 (this.topColliderOffset.x, this.topColliderOffset.y, 0f);
		colliders [1].size = new Vector3 (this.bottomColliderSize.x, this.bottomColliderSize.y, 0.2f);
		colliders [1].center = new Vector3 (this.bottomColliderOffset.x, this.bottomColliderOffset.y, 0f);
		colliders [2].size = new Vector3( this.leftColliderSize.x,this.leftColliderSize.y,0.2f);
		colliders [2].center = new Vector3 (this.leftColliderOffset.x, this.leftColliderOffset.y, 0f);
		colliders [3].size = new Vector3( this.rightColliderSize.x,this.rightColliderSize.y,0.2f);
		colliders [3].center = new Vector3 (this.rightColliderOffset.x, this.rightColliderOffset.y, 0f);

	}
}

