using UnityEngine;
using System.Collections;

public class GameNetworkCard : GameCard {

	public int ownerNumber;											// joueur 1 ou joueur 2
	public GameTile currentTile;
	Vector3 WorldNamePos;

	public int Damage = 0;

	public Texture2D bgImage; 
	public Texture2D fgImage;
	public GUIStyle progress_empty, progress_full;

	void Start()
	{
		UpdatePosition();
	}
	void Update()
	{
		UpdatePosition();
	}

	void OnGUI()
	{
		if (Card != null)
		{
			GUI.BeginGroup(new Rect(WorldNamePos.x, Screen.height - WorldNamePos.y, 16, 50));
			GUI.Box(new Rect(0,0,16,50), bgImage, progress_empty);
			GUI.BeginGroup(new Rect(0, 0, 16, 50));
			GUI.Box (new Rect(0,0,8,50), fgImage, progress_full);
			GUI.EndGroup();
			GUI.EndGroup();
		}
	}

	public Vector2 CalcGridPos()
	{
		float x = (transform.position.x / (GameTile.instance.hexWidth * 1.5f/2) + (GameBoard.instance.gridWidthInHexes / 2f) + 1);
		float y = Mathf.Floor(-(transform.position.y / (GameTile.instance.hexHeight) - (GameBoard.instance.gridHeightInHexes / 2f) + 1.5f));
		
		return new Vector2(x, y);
	}
	
	void OnMouseDown() 
	{
		if (networkView.isMine)
		{
			if (GameBoard.instance.TimeOfPositionning)
			{
				
				Debug.Log("card : " + Card.Id);
				GameBoard.instance.CardSelected = this;
				GameBoard.instance.isDragging = true;
				GameTile.instance.SetCursorToDrag();
			} 
			else
			{
				if (GameTimeLine.instance.PlayingCard.Card.Equals(Card) && !GamePlayingCard.instance.hasMoved) 
				{
					GameBoard.instance.CardSelected = this;
					GameBoard.instance.isMoving = true;
					//					offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1));
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					
					if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << GameBoardGenerator.instance.GridLayerMask))
					{
						currentTile = hit.transform.gameObject.GetComponent<GameTile>();
					}
				}
				Vector2 gridPosition = this.CalcGridPos();
				if (currentTile != null)
				{
					currentTile.Passable = true;
				}
				Tile tile = GameBoard.instance.board[new Point((int)gridPosition.x, (int)gridPosition.y)];
				colorAndMarkNeighboringTiles(tile.AllNeighbours, Card.Move, Color.gray);
			}                         
		}
	}
	
	void OnMouseEnter()
	{
		if (GameBoard.instance.TimeOfPositionning)
		{
			if (networkView.isMine)
			{
				GameBoard.instance.CardHovered = this;
				if (GameBoard.instance.isDragging)
				{
					if (!this.Equals(GameBoard.instance.CardSelected))
					{
						GameTile.instance.SetCursorToExchange();
					} else
					{
						GameTile.instance.SetCursorToDrag();
					}
				}
			}
		}
		GameHoveredCard.instance.ChangeCard(this);
	}
	void OnMouseExit()
	{
		if (GameBoard.instance.TimeOfPositionning)
		{
			if (networkView.isMine)
			{
				if (GameBoard.instance.isDragging)
				{
					GameBoard.instance.CardHovered = null;
				}
			}
		}
		GameHoveredCard.instance.hide();
	}
	void OnMouseUp()
	{
		if (GameBoard.instance.TimeOfPositionning)
		{
			if (networkView.isMine)
			{
				if (GameBoard.instance.isDragging)
				{
					if (!this.Equals(GameBoard.instance.CardHovered) && GameBoard.instance.CardHovered)
					{
						Vector3 temp = this.transform.position;
						this.transform.position = GameBoard.instance.CardHovered.transform.position;
						GameBoard.instance.CardHovered.transform.position = temp;
					}
					if (GameBoard.instance.CardHovered == null)
					{
						GameBoard.instance.droppedCard = true;
					}
					GameBoard.instance.isDragging = false;
					GameTile.instance.SetCursorToDefault();
				}
			}
		}
		else
		{
			foreach(Transform go in GameBoard.instance.gameObject.transform)
			{
				if (!go.gameObject.name.Equals("Game Board"))
				{
					go.renderer.material = GameTile.instance.DefaultMaterial;
					go.GetComponent<GameTile>().Passable = false;
				}
			}
			if (networkView.isMine)
			{
				GameBoard.instance.isMoving = false;
				GameBoard.instance.CardSelected = null;
			}
		}
	}
	
	void OnMouseDrag()
	{
		//		if (networkView.isMine)
		//		{
		//			if (GameTimeLine.instance.PlayingCard.Card.Equals(Card)) 
		//			{
		//				Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1);
		//				Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		//				curPosition.z = -1;
		//				transform.position = curPosition;
		//
		//
		//			}
		//		}
	}
	
	void colorAndMarkNeighboringTiles(IEnumerable allNeighbours, int i, Color color)
	{
		if (i-- == 0)
		{
			return;
		}
		foreach (Tile tile in allNeighbours)
		{
			GameTile gTile = GameObject.Find("hex " + tile.X + "-" + tile.Y).GetComponent<GameTile>();

			Vector3 pos = gTile.transform.TransformPoint(Vector3.zero) + new Vector3(0, 0, -2); // les colliders fonctionnent que d'un coté sur les planes, on va donc reculer et regarder ensuite en avant

			RaycastHit hit;

			bool hasCard = false;
			if (Physics.Raycast(pos, Vector3.forward, out hit))
			{
				if (hit.transform.gameObject.name.Equals("CardNetwork(Clone)"))
				{
					hasCard = true;
				}
			}
			
			if (!hasCard)
			{
				colorAndMarkNeighboringTiles(tile.AllNeighbours, i, color);
				gTile.Passable = true;
				gTile.changeColor(color);
			}
		}
	}

	public void UpdatePosition()
	{
		if (!gameObject.tag.Equals("NoPlayableCard"))
		{
			Vector3 GameCardPosition = transform.Find("Life/Life Bar").position;
			WorldNamePos = Camera.main.camera.WorldToScreenPoint(GameCardPosition);
		}
	}
}
