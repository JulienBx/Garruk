using System;

public class ScreenDimensions
{
	int width, height ;

	public ScreenDimensions (int x, int y)
	{
		this.width = x;
		this.height = y;
	}

	public bool hasChanged (int x, int y){
		return (x!=this.width || y!=this.height);
	}

	public void setDimensions(int x, int y){
		this.width = x;
		this.height = y;
	}

	public float getRealWidth(){
		return 10f*this.width/this.height;
	}
}


