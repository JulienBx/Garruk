using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour {

	public float cameraSize;
	public int height;
	public int width;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		cameraSize = Screen.width * 0.108f;
		camera.orthographicSize = cameraSize;
		width = Screen.width;
		height = Screen.height;
	}
}
