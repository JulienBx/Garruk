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
		Application.LoadLevel("myCardsPage");
	}
	
	public void magasinLink()
	{
		Application.LoadLevel("BuyCards");
	}
}