using UnityEngine;

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
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé Sables Mouvants \n sur "
			+ amount +
			" case" +
			pluriel +
			" aléatoire" +
			pluriel);
		int targetX = args [0];
		int targetY = args [1];
		for (int i = 0; i < amount; i++)
		{
			int randX = (int)Mathf.Floor(Random.Range(0, GameController.instance.boardWidth));
			int randY = (int)Mathf.Floor(Random.Range(0, GameController.instance.boardHeight));
			GameController.instance.addTileModifier(2, amount, randX, randY);
		}

	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
