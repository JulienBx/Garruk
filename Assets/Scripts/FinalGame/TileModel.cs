using System;

public class TileModel
{
	int x,y;

	public TileModel ()
	{
		this.x = -1;
		this.y = -1;
	}

	public void setX(int i){
		this.x = i;
	}

	public void setY(int i){
		this.y = i;
	}

	public void setXY(int i, int j){
		this.setX(i);
		this.setY(j);
	}

	public int getX(){
		return this.x;
	}

	public int getY(){
		return this.y;
	}

	public void randomize(int x, int y){
		this.x = UnityEngine.Random.Range(0,x);
		this.y = UnityEngine.Random.Range(1,y-1);
	}
}