using UnityEngine;

public class TileController : MonoBehaviour
{
//	public Texture2D cursorDragging;
//	public Texture2D cursorExchange;
//	public Texture2D cursorAttack;
//	public Texture2D cursorTarget;

	Tile tile ;
	private int type ;
	public Texture2D[] backTile ;
	public Texture2D[] borderTile ;
	public TileView tileView ;
	private float scaleTile ;

	public bool isDestination ;
	public int characterID = -1 ;

	void Awake()
	{
		this.tileView = gameObject.AddComponent <TileView>();
		this.isDestination = false ;
	}

	void Start () 
	{

	}

	public void setTile(int x, int y, int boardWidth, int boardHeight, int type, float scaleTile){
		this.tile = new Tile(x,y) ;
		this.type = type ;

		this.tileView.tileVM.background = backTile[type];
		this.tileView.changeBackground();
		this.tileView.tileVM.border = borderTile[0];
		this.tileView.changeBorder();

		this.resize(1f, 3, 4);
	}

	public void resize(float scaleTile, float offsetX, float offsetY){
		Vector3 position ;
		this.tileView.tileVM.scale = new Vector3(scaleTile,scaleTile,scaleTile);
		if (GameController.instance.isFirstPlayer){
			position = new Vector3(scaleTile * (-offsetX+0.5f+this.tile.x),scaleTile * (-offsetY+0.5f+this.tile.y), -1);
		}
		else{
			position = new Vector3(scaleTile * (offsetX-0.5f-this.tile.x),scaleTile * (offsetY-0.5f-this.tile.y), -1);
		}
		this.tileView.tileVM.position = position;

		this.tileView.resize();
	}

	public void setDestination () {
		this.isDestination = true ;
		this.tileView.tileVM.background = this.backTile[5+this.type];
		this.tileView.changeBackground();
		this.tileView.tileVM.border = this.borderTile[4];
		this.tileView.changeBorder();
	}

	public void setStandard () {
		this.isDestination = false ;
		this.tileView.tileVM.background = this.backTile[this.type];
		this.tileView.changeBackground();
		this.tileView.tileVM.border = this.borderTile[0];
		this.tileView.changeBorder();
	}

	public Vector3 getPosition () {
		return this.tileView.tileVM.position;
	}

	public void setBorderTile () {
		
	}

	public void hoverTile(){
		GameController.instance.hoverTileHandler(this.tile);
	}

	public void releaseClickTile(){
		GameController.instance.releaseClickTileHandler(this.tile);
	}

	public void displayHover(){
		this.tileView.tileVM.border = this.borderTile[1];
		this.tileView.changeBorder();
	}

	public void hideHover(){
		if (this.isDestination){
			this.tileView.tileVM.border = this.borderTile[4];
			this.tileView.changeBorder();
		}
		else{
			this.tileView.tileVM.border = this.borderTile[0];
			this.tileView.changeBorder();
		}
	}

	public void displaySelected(){
		this.tileView.tileVM.border = this.borderTile[2];
		this.tileView.changeBorder();
	}
	
	public void hideSelected(){
		if (this.isDestination){
			this.tileView.tileVM.border = this.borderTile[4];
			this.tileView.changeBorder();
		}
		else{
			this.tileView.tileVM.border = this.borderTile[0];
			this.tileView.changeBorder();
		}
	}

	public void displayPlaying(){
		this.tileView.tileVM.border = this.borderTile[3];
		this.tileView.changeBorder();
	}
	
	public void hidePlaying(){
		if (this.isDestination){
			this.tileView.tileVM.border = this.borderTile[4];
			this.tileView.changeBorder();
		}
		else{
			this.tileView.tileVM.border = this.borderTile[0];
			this.tileView.changeBorder();
		}
	}

//	public void drag(){
//		if (this.isMovable){
//			GameController.instance.setCharacterDragged(this.characterID);
//		}
//	}
//	
//	public void release(){
//		if (this.isMovable){
//			GameController.instance.dropCharacter();
//		}
//	}

}

