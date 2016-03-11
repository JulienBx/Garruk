using UnityEngine;
using System.Collections;

public class ResizeToBiggestChild : MonoBehaviour {

	// Use this for initialization
	void Start () {

        RectTransform currenRectTransform = GetComponent<RectTransform> ();
        RectTransform childRectTransform = GetComponentInChildren<RectTransform> ();
		currenRectTransform.offsetMin = new Vector2 (currenRectTransform.offsetMin.x, currenRectTransform.offsetMax.y - childRectTransform.rect.height);
		currenRectTransform.offsetMax = new Vector2 (currenRectTransform.offsetMin.x, currenRectTransform.offsetMax.y);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
