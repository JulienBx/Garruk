using UnityEngine;
using System.Collections;

public class GameBoardGenerator : MonoBehaviour 
{
	public GameObject Hex;
	public int gridWidthInHexes;
	public int gridHeightInHexes;
	public static GameBoardGenerator instance = null;

	private float hexWidth;
	private float hexHeight;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		setSizes();
		createGrid();
	}

	void setSizes()
	{
		hexWidth = Hex.renderer.bounds.size.x;
		hexHeight = Hex.renderer.bounds.size.z;
	}

	Vector3 calcInitPos()
	{
		Vector3 initPos;
		initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2, 0,
		                      gridHeightInHexes / 2f * hexHeight - hexHeight / 2);
		
		return initPos;
	}

	public Vector3 calcWorldCoord(Vector2 gridPos)
	{
		Vector3 initPos = calcInitPos();
		float offset = 0;
		if (gridPos.y % 2 != 0)
			offset = hexWidth / 2;
		
		float x =  initPos.x + offset - gridPos.x * hexWidth;
		float z = initPos.z - gridPos.y * hexHeight * 0.75f;
		return new Vector3(x, 0, z);
	}

	void createGrid()
	{
		GameObject hexGridGO = new GameObject("Game Board");
		
		for (float y = 0; y < gridHeightInHexes; y++)
		{
			for (float x = 0; x < gridWidthInHexes; x++)
			{
				GameObject hex = (GameObject)Instantiate(Hex);
				if (y == 0 || y == 1 || y == 6 || y == 7)
				{
					hex.transform.tag = "Column" + (y + 1);
				}
				hex.name = "hex " + (x + 1);
				Vector2 gridPos = new Vector2(x, y);
				hex.transform.position = calcWorldCoord(gridPos);
				hex.transform.parent = hexGridGO.transform;
			}
		}
		hexGridGO.transform.Rotate(new Vector3(0, -90, 90));
		hexGridGO.transform.localScale = new Vector3(40, 1, 40);
	}
}
