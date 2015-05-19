using UnityEngine;

public class Pass
{
	public void launch()
	{
		Debug.Log ("Je passe");
		GameController.instance.findNextPlayer();
	}
}
