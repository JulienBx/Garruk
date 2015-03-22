using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {
	
	public void link1()
	{
		Application.LoadLevel("HomePage");
	}
	
	public void link2()
	{
		Application.LoadLevel("MyGame");
	}

	public void link3()
	{
		Application.LoadLevel("BuyCards");
	}
	
	public void link4()
	{
		Application.LoadLevel("Market");
	}

	public void logOutLink() 
	{
		ApplicationModel.username = "";
		ApplicationModel.toDeconnect = true;
		Application.LoadLevel("ConnectionPage");
	}

	public void profileLink() 
	{
		Application.LoadLevel("Profile");
	}

	public void homePageLink() 
	{
		Application.LoadLevel("HomePage");
	}

	public void link5()
	{
		Application.LoadLevel("LobbyPage");
	}
}