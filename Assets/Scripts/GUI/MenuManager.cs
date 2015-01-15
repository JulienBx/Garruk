using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {
	
	public void homeLink()
	{
		Application.LoadLevel("HomePage");
	}
	
	public void myCardsLink()
	{
		Application.LoadLevel("MyCardsPage");
	}

	public void myDecksLink()
	{
		Application.LoadLevel("MyDecksPage");
	}
	
	public void magasinLink()
	{
		Application.LoadLevel("BuyCards");
	}

	public void logOutLink() 
	{
		ApplicationModel.username = "";
		Application.LoadLevel("ConnectionPage");
	}
}