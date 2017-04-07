using System;

public class IconModel
{
	bool displayed;

	public IconModel ()
	{
		this.displayed = false;
	}

	public bool isDisplayed(){
		return displayed;
	}

	public void setDisplayed(bool b){
		this.displayed=b;
	}


}