using UnityEngine;
using System.Collections.Generic;

public class SablesMouvants : GameSkill
{
	public SablesMouvants()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance Sables Mouvants");
		GameController.instance.lookForTileTarget("Choisir une cible pour Sables Mouvants", "Lancer Sables Mouvants");
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power;
		string pluriel = amount > 1 ? "s" : "";
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé Sables Mouvants \n sur "
			+ amount +
			" case" +
			pluriel +
			" aléatoire" +
			pluriel);
		for (int i = 0; i < amount; i++)
		{
			int randX = (int)Mathf.Floor(Random.Range(0, GameController.instance.boardWidth));
			int randY = (int)Mathf.Floor(Random.Range(0, GameController.instance.boardHeight));
			GameController.instance.getTile(randX, randY).addSable();
		}


	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
