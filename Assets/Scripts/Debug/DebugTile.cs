using UnityEngine;
using System.Collections;

public class DebugTile : MonoBehaviour {

	GameTile gTile;

	// Use this for initialization
	void Start () {
		gTile = GetComponent<GameTile>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter()
	{
		Debug.Log(gTile.tile.ToString());
	}
}
