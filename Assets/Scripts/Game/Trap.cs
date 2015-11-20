using UnityEngine;

public class Trap
{
	int amount;
	int type;
	bool isVisible;
	Tile tile;
	
	public string title;
	public string description;

	public Trap(int a, int t, bool v, string ti, string d)
	{
		this.amount = a;
		this.type = t;
		this.isVisible = v;
		this.title = ti;
		this.description = d;
	}
	
	public Trap(Tile t, int a, int ty, bool v, string ti, string d)
	{
		this.tile = t ;
		this.amount = a;
		this.type = ty;
		this.isVisible = v;
		this.title = ti;
		this.description = d;
	}
	
	public void setTile(Tile t){
		this.tile = t ;
	}
	
	public void setVisible(bool b){
		this.isVisible = b ;
	}
	
	public Tile getTile(){
		return this.tile ;
	}
	
	public bool getIsVisible(){
		return this.isVisible ;
	}
	
	public int getType(){
		return this.type ;
	}
}
