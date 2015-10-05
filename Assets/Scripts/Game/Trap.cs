using UnityEngine;

public class Trap
{
	public int amount;
	public int type; //0 = piège à loups
	public bool isVisible;

	public Trap(int a, int t, bool v)
	{
		this.amount = a;
		this.type = t;
		this.isVisible = v;
	}

	public void activate(int target)
	{
		if (type==0){
			string message = GameView.instance.getCard(target).Title+" déclenche piège à loups";
			message += "\n"+GameView.instance.getCard(target).Title+" subit "+amount+" dégats";
			
			//GameController.instance.addModifier(target, amount, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);	
			GameView.instance.getGC().play();	
		}
	}
}
