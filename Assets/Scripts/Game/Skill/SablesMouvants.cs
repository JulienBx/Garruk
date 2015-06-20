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
		//GameController.instance.lookForValidation(true, "Sables Mouvants cible des cases aléatoires", "Lancer Sables Mouvants");
	}
	
	public override void resolve(List<int> targetsPCC)
	{
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		string pluriel = amount > 1 ? "s" : "";
		GameController.instance.play();
//		int targetX = args [0];
//		int targetY = args [1];
//		int[] posx = new int[amount];
//		int[] posy = new int[amount];
//		for (int i = 0; i < amount; i++)
//		{
//			int randX = (int)Mathf.Floor(Random.Range(0, GameController.instance.boardWidth));
//			int randY = (int)Mathf.Floor(Random.Range(0, GameController.instance.boardHeight));
//			bool alreadyExist = false;
//			do
//			{
//				alreadyExist = false;
//				for (int j = 0; j < posx.Length; j++)
//				{
//					if (posx [j] == randX && posy [j] == randY)
//					{
//						alreadyExist = true;
//						randX = (int)Mathf.Floor(Random.Range(0, GameController.instance.boardWidth));
//						randY = (int)Mathf.Floor(Random.Range(0, GameController.instance.boardHeight));
//					}
//				}
//			} while(alreadyExist);
//			posx [i] = randX;
//			posy [i] = randY;
//			GameController.instance.addTileModifier(2, amount, randX, randY);
//		}

	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
