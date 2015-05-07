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
	private float scaleTile ;
	public Texture2D[] backTile ;
	public Texture2D[] borderTile ;
	public TileView tileView ;

	public bool isDestination ;
	//-1 : case vide ; 0 : case occupée par un personnage allié ; 1 : case occupée par un personnage ennemi
	public int characterID ; //-1 si personne


	void Awake()
	{
		this.tileView = gameObject.AddComponent <TileView>();
		this.isDestination = false ;
		this.characterID = -1 ;
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
		this.tileView.tileVM.border = borderTile[0];
		this.tileView.resize();
		this.tileView.ShowFace();
	}

	public void setCharacterID(int i){
		this.characterID = i ;
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
		this.tileView.ShowFace();
	}

	public void setBorderTile () {
		
	}

	public void hoverTile(){
		GameController.instance.hoverTileHandler(this.x, this.y, this.characterID, this.isDestination);
	}

	public void clickTile(){
		if(this.characterID!=-1){
			GameController.instance.clickTileHandler(this.x, this.y, this.characterID);
		}
	}

	public void releaseClickTile(){
		GameController.instance.releaseClick();
	}

	public void displayHover(){
		this.tileView.tileVM.border = this.borderTile[1];
		this.tileView.tileVM.raiseTile();
		this.tileView.resize();
		this.tileView.ShowFace();
	}

	public void hideHover(){
		this.tileView.tileVM.border = this.borderTile[0];
		this.tileView.tileVM.lowerTile();
		this.tileView.resize();
		this.tileView.ShowFace();
	}

	public void displaySelected(){
		this.tileView.tileVM.border = this.borderTile[2];
		this.tileView.resize();
		this.tileView.ShowFace();
	}
	
	public void hideSelected(){
		this.tileView.tileVM.border = this.borderTile[0];
		this.tileView.tileVM.lowerTile();
		this.tileView.resize();
		this.tileView.ShowFace();
	}

	public void displayPlaying(){
		this.tileView.tileVM.border = this.borderTile[3];
		this.tileView.resize();
		this.tileView.ShowFace();
	}
	
	public void hidePlaying(){
		this.tileView.tileVM.border = this.borderTile[0];
		this.tileView.tileVM.lowerTile();
		this.tileView.resize();
		this.tileView.ShowFace();
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
		this.tileView.ShowFace();
	}
}

