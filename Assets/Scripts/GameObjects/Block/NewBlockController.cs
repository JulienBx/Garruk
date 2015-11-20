
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class NewBlockController : MonoBehaviour 
{
	
	public void resize(float leftMargin, float upMargin, float width, float height)
	{
		Vector2 blockOrigin = new Vector3 (-ApplicationDesignRules.worldWidth/2f+leftMargin+width/2f, ApplicationDesignRules.worldHeight / 2f - upMargin - height / 2,0f);

		float blockSize = 100f;
		float blockWorldSize = 100f / (ApplicationDesignRules.pixelPerUnit);
		float blockScaleX = width / (blockWorldSize);
		float blockScaleY = height / (blockWorldSize);

		gameObject.transform.localScale=new Vector3(blockScaleX,blockScaleY,1f);
		gameObject.transform.position = new Vector3 (blockOrigin.x, blockOrigin.y);
	}
	public Vector3 getUpperLeftCornerPosition()
	{
		Vector3 upperLeftCornerPosition = new Vector3 ();
		upperLeftCornerPosition.x = gameObject.transform.GetComponent<SpriteRenderer> ().bounds.min.x;
		upperLeftCornerPosition.y = gameObject.transform.GetComponent<SpriteRenderer> ().bounds.max.y;
		upperLeftCornerPosition.z = gameObject.transform.position.z;
		return upperLeftCornerPosition;
	}
	public Vector3 getUpperRightCornerPosition()
	{
		Vector3 upperRightCornerPosition = new Vector3 ();
		upperRightCornerPosition.x = gameObject.transform.GetComponent<SpriteRenderer> ().bounds.max.x;
		upperRightCornerPosition.y = gameObject.transform.GetComponent<SpriteRenderer> ().bounds.max.y;
		upperRightCornerPosition.z = gameObject.transform.position.z;
		return upperRightCornerPosition;
	}
	public Vector3 getLowerRightCornerPosition()
	{
		Vector3 lowerRightCornerPosition = new Vector3 ();
		lowerRightCornerPosition.x = gameObject.transform.GetComponent<SpriteRenderer> ().bounds.max.x;
		lowerRightCornerPosition.y = gameObject.transform.GetComponent<SpriteRenderer> ().bounds.min.y;
		lowerRightCornerPosition.z = gameObject.transform.position.z;
		return lowerRightCornerPosition;
	}
	public Vector3 getLowerLeftCornerPosition()
	{
		Vector3 lowerLeftCornerPosition = new Vector3 ();
		lowerLeftCornerPosition.x = gameObject.transform.GetComponent<SpriteRenderer> ().bounds.min.x;
		lowerLeftCornerPosition.y = gameObject.transform.GetComponent<SpriteRenderer> ().bounds.min.y;
		lowerLeftCornerPosition.z = gameObject.transform.position.z;
		return lowerLeftCornerPosition;
	}
	public Vector3 getOriginPosition()
	{
		return gameObject.transform.position;
	}
	public Vector2 getSize()
	{
		Vector2 size = new Vector2 ();
		size.x = gameObject.transform.GetComponent<SpriteRenderer> ().bounds.size.x;
		size.y = gameObject.transform.GetComponent<SpriteRenderer> ().bounds.size.y;
		return size;
	}
}

