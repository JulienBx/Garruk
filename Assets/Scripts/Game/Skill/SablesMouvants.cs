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
		GameController.instance.lookForValidation(true, "Sables Mouvants cible des cases aléatoires", "Lancer Sables Mouvants");
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		string pluriel = amount > 1 ? "s" : "";

		int targetX = args [0];
		int targetY = args [1];

		int EmptyPosition = 0;

		List<Point> EmptyPositions = new List<Point>();
		for (int i = 0; i < GameController.instance.boardWidth; i++)
		{
			for (int j = 0; j< GameController.instance.boardHeight; j++)
			{
				TileController tc = GameController.instance.getTile(i, j);
				if (tc.characterID == -1 && (tc.tileModification == TileModification.Void))
				{
					EmptyPositions.Add(new Point(i, j));
					EmptyPosition++;
				}
			}
		}

		if (EmptyPosition < amount)
		{
			amount = EmptyPosition;
		}

		for (int i = 0; i < amount; i++)
		{
			int randPosition = (int)Mathf.Floor(Random.Range(0, EmptyPosition - 1));
			EmptyPosition--;
			Point temp = EmptyPositions [randPosition];
			EmptyPositions.Remove(temp);
			TileController tc = GameController.instance.getTile(temp.X, temp.Y);
			GameController.instance.addTileModifier(2, amount, temp.X, temp.Y);
		}

		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé Sables Mouvants \n sur "
			+ amount +
			" case" +
			pluriel +
			" aléatoire" +
			pluriel);

	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
