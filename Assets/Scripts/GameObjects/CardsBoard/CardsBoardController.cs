using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CardsBoardController : MonoBehaviour 
{
	
	public void resize(float width, float height, Vector3 origin)
	{
		this.gameObject.transform.position = origin;

		float cornerSize = 56f;
		float correction = 16.5f;
		float correctionRatio = cornerSize / correction;
		float pixelsPerUnit = 108f;
		float lineSize = 800f;
		float cornerWorldSize = cornerSize / pixelsPerUnit;
		float lineWorldSize = lineSize / pixelsPerUnit;

		float verticalLineSize = height - 2 * cornerWorldSize;
		float verticalLineScale = verticalLineSize / lineWorldSize;
		float horizontalLineSize = width - 2 * cornerWorldSize;
		float horizontalLineScale = horizontalLineSize / lineWorldSize;

		Vector3 topLeftCornerPosition = new Vector3 (-width / 2f + cornerWorldSize / correctionRatio, height / 2f - cornerWorldSize / correctionRatio, 0);
		Vector3 topRightCornerPosition = new Vector3 (width / 2f - cornerWorldSize / correctionRatio, height / 2f - cornerWorldSize / correctionRatio, 0);
		Vector3 bottomLeftCornerPosition = new Vector3 (-width / 2f + cornerWorldSize / correctionRatio, -height / 2f + cornerWorldSize / correctionRatio, 0);
		Vector3 bottomRightCornerPosition = new Vector3 (width / 2f - cornerWorldSize / correctionRatio, -height / 2f + cornerWorldSize / correctionRatio, 0);

		Vector3 leftLinePosition = new Vector3 (topLeftCornerPosition.x, 0, 0);
		Vector3 rightLinePosition = new Vector3 (topRightCornerPosition.x, 0, 0);
		Vector3 topLinePosition = new Vector3 (0, topLeftCornerPosition.y, 0);
		Vector3 bottomLinePosition = new Vector3 (0, bottomLeftCornerPosition.y, 0);

		Vector3 horizontalLinesScale = new Vector3 (1f,horizontalLineScale, 1f);
		Vector3 verticalLinesScale = new Vector3 (1f, verticalLineScale, 1f);

		this.gameObject.transform.FindChild ("topLeftCorner").localPosition = topLeftCornerPosition;
		this.gameObject.transform.FindChild ("topRightCorner").localPosition = topRightCornerPosition;
		this.gameObject.transform.FindChild ("bottomLeftCorner").localPosition = bottomLeftCornerPosition;
		this.gameObject.transform.FindChild ("bottomRightCorner").localPosition = bottomRightCornerPosition;
		this.gameObject.transform.FindChild ("leftLine").localPosition = leftLinePosition;
		this.gameObject.transform.FindChild ("rightLine").localPosition = rightLinePosition;
		this.gameObject.transform.FindChild ("topLine").localPosition = topLinePosition;
		this.gameObject.transform.FindChild ("bottomLine").localPosition = bottomLinePosition;
		this.gameObject.transform.FindChild ("leftLine").localScale = verticalLinesScale;
		this.gameObject.transform.FindChild ("rightLine").localScale = verticalLinesScale;
		this.gameObject.transform.FindChild ("topLine").localScale = horizontalLinesScale;
		this.gameObject.transform.FindChild ("bottomLine").localScale = horizontalLinesScale;

	}
}

