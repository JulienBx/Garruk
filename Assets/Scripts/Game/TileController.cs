using UnityEngine;

public class TileController : MonoBehaviour
{
//	public Texture2D cursorDragging;
//	public Texture2D cursorExchange;
//	public Texture2D cursorAttack;
//	public Texture2D cursorTarget;

	private int x ;
	private int y ;
	private int type ;
	private float scaleTile ;
	public Texture2D[] backTile ;
	public TileView tileView ;

	public bool isDestination ;
	//-1 : case vide ; 0 : case occupée par un personnage allié ; 1 : case occupée par un personnage ennemi
	public int occupationType ;

	void Awake()
	{
		this.tileView = gameObject.AddComponent <TileView>();
		this.isDestination = false ;
		this.occupationType = -1 ;
	}

	void Start () 
	{

	}

	public void setTile(int x, int y, int boardWidth, int boardHeight, int type, float scaleTile){
		this.x = x ;
		this.y = y ;
		this.type = type ;
		int decalage ;
		if ((boardWidth-x)%2==0){
			decalage = 1;
		}
		else{
			decalage = 0;
		}

		this.tileView.tileVM.scale = new Vector3(scaleTile,scaleTile,scaleTile);
		this.tileView.tileVM.position = new Vector3((x-boardWidth/2)*scaleTile*0.71f, (y-boardHeight/2+decalage/2f)*scaleTile*0.81f, 0);
		this.tileView.tileVM.background = backTile[type];
		this.tileView.resize();
		this.tileView.ShowFace();
	}

	public void setOccupationType(int i){
		this.occupationType = i ;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void mouseEnter () {
		if (this.isDestination == true && this.occupationType==-1){
			GameController.instance.moveCharacter(this.x, this.y);
		}
	}

	public void setDestination () {
		this.isDestination = true ;
		this.tileView.tileVM.background = this.backTile[5+this.type];
		this.tileView.ShowFace();
	}
	
	//	void OnMouseEnter()
	//	{
	//		if (GameView.instance.TimeOfPositionning)
	//		{
	//			if (GameView.instance.isDragging && AvailableStartingColumns.Any(e => e.Contains(this.tag)))
	//			{
	//				SetCursorToDrag();
	//			} else
	//			{
	//				SetCursorToDefault();
	//			}
	//		} else
	//		{
	//			if (GameView.instance.isMoving && this.Passable)
	//			{
	//				Vector3 newPosition = this.transform.position;
	//				newPosition.z = -1;
	////				GameView.instance.CardSelected.transform.position = newPosition;
	////				if (GameView.instance.CardSelected.currentTile.Equals(this))
	////				{
	////					GamePlayingCard.instance.attemptToMoveTo = null;
	////				}
	////				else
	////				{
	////					GamePlayingCard.instance.attemptToMoveTo = this;
	////				}
	//			}
	//			else
	//			{
	//				GamePlayingCard.instance.attemptToMoveTo = null;
	//			}
	//		}
	//	}
	
	//	void OnMouseOver()
	//	{
	//		if (GameView.instance.droppedCard && AvailableStartingColumns.Any(e => e.Contains(this.tag)))
	//		{
	//			Vector3 pos = transform.TransformPoint(Vector3.zero) + new Vector3(0, 0, -2);
	//			RaycastHit hit;
	//			
	//			if (Physics.Raycast(pos, Vector3.forward, out hit))
	//			{
	//				if (hit.transform.gameObject.tag != "PlayableCard")
	//				{
	//					//GameView.instance.CardSelected.transform.position = this.transform.position + new Vector3(0, 0, -1);
	//					GameView.instance.droppedCard = false;
	//					GameView.instance.isDragging = false;
	//				}
	//			}
	//			
	//		}
	//	}

	public void changeColor(Color color)
	{
		if (color.a == 1)
			color.a = 130f / 255f;
		//renderer.material = OpaqueMaterial;
		renderer.material.color = color;
	}
	
	public static void RemovePassableTile()
	{
		//		foreach(Transform go in GameView.instance.gameObject.transform)
		//		{
		//			if (!go.gameObject.name.Equals("Game Board"))
		//			{
		//				go.renderer.material = GameTile.instance.DefaultMaterial;
		//				go.GetComponent<GameTile>().Passable = false;
		//			}
		//		}
	}
	
	public static void InitIndexPathTile()
	{
		//		foreach(Transform go in GameView.instance.gameObject.transform)
		//		{
		//			if (!go.gameObject.name.Equals("Game Board"))
		//			{
		//				go.GetComponent<GameTile>().pathIndex = -1;
		//			}
		//		}
	}
	
	public void SetCursorToDrag()
	{
		//Cursor.SetCursor(cursorDragging, cursorHotspot, CursorMode.Auto);
	}
	public void SetCursorToExchange()
	{
		//Cursor.SetCursor(cursorExchange, cursorHotspot, CursorMode.Auto);
	}
	public void SetCursorToDefault()
	{
		//Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
	public void SetCursorToAttack()
	{
		//Cursor.SetCursor(cursorAttack, Vector2.zero, CursorMode.Auto);
	}
	public void SetCursorToTarget()
	{
		//Cursor.SetCursor(cursorTarget, Vector2.zero, CursorMode.Auto);
	}
}

