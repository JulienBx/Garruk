using UnityEngine;

public class Trap
{
	int amount;
	int type;
	public bool isVisible;
	public bool isIAVisible;
	Tile tile;
	
	public string title;
	public string description;

	public Trap(int a, int t, bool v, string ti, string d, bool ia)
	{
		this.amount = a;
		this.type = t;
		this.isVisible = v;
		this.title = ti;
		this.description = d;
		this.isIAVisible = ia;
	}
	
	public Trap(Tile t, int a, int ty, bool v, string ti, string d, bool ia)
	{
		this.tile = t ;
		this.amount = a;
		this.type = ty;
		this.isVisible = v;
		this.title = ti;
		this.description = d;
		this.isIAVisible = ia;
	}
	
	public void setTile(Tile t){
		this.tile = t ;
	}
	
	public void setVisible(bool b){
		this.isVisible = b ;
	}

	public void setIAVisible(bool b){
		this.isIAVisible = b ;
	}
	
	public Tile getTile(){
		return this.tile ;
	}
	
	public bool getIsVisible(){
		return this.isVisible ;
	}

	public bool getIsIAVisible(){
		return this.isIAVisible ;
	}
	
	public int getType(){
		return this.type ;
	}
	
	public int getAmount(){
		return this.amount ;
	}
}
