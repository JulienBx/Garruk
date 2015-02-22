using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOutline : MonoBehaviour {

	public bool ToArrange = false;
	public List<GameObject> RedOutlines;
	public List<GameObject> GreenOutlines;
	public GameObject YellowOutlines;
	public static GameOutline instance;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (ToArrange)
		{
			int i = 0;
			foreach (GameObject go in GameTimeLine.instance.GameObjects)
			{
				if (i != 4)
				{
					GameNetworkCard gc = go.GetComponent<GameNetworkCard>();
					if (gc.ownerNumber == 1 && GameBoard.instance.nbPlayer == 1 || gc.ownerNumber == 2 && GameBoard.instance.nbPlayer == 2)
					{
						RedOutlines[i].renderer.enabled = false;
						GreenOutlines[i].renderer.enabled = true;
					}
					else
					{
						RedOutlines[i].renderer.enabled = true;
						GreenOutlines[i].renderer.enabled = false;
					}
				}
				i++;
			}
			ToArrange = false;
		}
	}
}
