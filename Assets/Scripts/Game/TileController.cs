using UnityEngine;

public class TileController : MonoBehaviour
{
//	public Texture2D cursorDragging;
//	public Texture2D cursorExchange;
//	public Texture2D cursorAttack;
//	public Texture2D cursorTarget;

	public int x ;
	public int y ;
	private int type ;
	public Texture2D[] backTile ;
	public Texture2D[] borderTile ;
	public TileView tileView ;
	private float scaleTile ;

	public bool isDestination ;
	//-1 : case vide ; 0 : case occupée par un personnage allié ; 1 : case occupée par un personnage ennemi


	void Awake()
	{
		this.tileView = gameObject.AddComponent <TileView>();
		this.isDestination = false ;
	}

	void Start () 
	{

	}

	public void setTile(int x, int y, int boardWidth, int boardHeight, int type, float scaleTile){
		this.x = x ;
		this.y = y ;
		this.type = type ;
		
		this.tileView.tileVM.scale = new Vector3(scaleTile,scaleTile,scaleTile);
		this.tileView.tileVM.position = new Vector3((x-boardWidth/2)*scaleTile*0.71f, (y-boardHeight/2)*scaleTile*0.81f, 0);
		this.tileView.tileVM.background = backTile[type];
		this.tileView.changeBackground();
		this.tileView.tileVM.border = borderTile[0];
		this.tileView.changeBorder();

		this.tileView.resize();
	}

	public void resize(int heightScreen){
	//
	}
	
	// Update is called once per frame
	void Update () {
		
	}

//	public void mouseEnter () {
//		if (this.isDestination == true && this.characterID==-1){
//			GameController.instance.moveCharacter(this.x, this.y);
//		}
//	}

	public void setDestination () {
		this.isDestination = true ;
		this.tileView.tileVM.background = this.backTile[5+this.type];
		this.tileView.changeBackground();
	}

	public void setBorderTile () {
		
	}

	public void hoverTile(){
		GameController.instance.hoverTileHandler(new Tile(this.x, this.y));
	}

	public void releaseClickTile(){
		GameController.instance.releaseClickTileHandler();
	}

	public void displayHover(){
		this.tileView.tileVM.border = this.borderTile[1];
		this.tileView.resize();
		this.tileView.changeBorder();
	}

	public void hideHover(){
		this.tileView.tileVM.border = this.borderTile[0];
		this.tileView.resize();
		this.tileView.changeBorder();
	}

	public void displaySelected(){
		this.tileView.tileVM.border = this.borderTile[2];
		this.tileView.resize();
		this.tileView.changeBorder();
	}
	
	public void hideSelected(){
		this.tileView.tileVM.border = this.borderTile[0];
		this.tileView.resize();
		this.tileView.changeBorder();
	}

	public void displayPlaying(){
		this.tileView.tileVM.border = this.borderTile[3];
		this.tileView.resize();
		this.tileView.changeBorder();
	}
	
	public void hidePlaying(){
		this.tileView.tileVM.border = this.borderTile[0];
		this.tileView.resize();
		this.tileView.changeBorder();
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

	public void setStandard () {
		this.isDestination = false ;
		this.tileView.tileVM.background = this.backTile[this.type];
		this.tileView.changeBackground();
	}
}

