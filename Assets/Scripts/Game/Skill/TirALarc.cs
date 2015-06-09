using UnityEngine;

public class TirALarc : GameSkill
{
	public override void launch()
	{
		Debug.Log("Je lance tir à l'arc");
		GameController.instance.lookForTarget("Choisir une cible pour le tir", "Lancer Tir à l'arc");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args [0];
		string message = GameController.instance.getCurrentCard().Title+" tire une flèche sur "+GameController.instance.getCard(targetID).Title;
		
		int attack = GameController.instance.getCurrentSkill().ManaCost;
		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive())
		{                             
			GameController.instance.addModifier(targetID, attack, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
			GameController.instance.useSkill();
			
			message+="\n"+"La flèche touche la cible et inflige "+attack+" degats";
		}
		else{
			message+="\n"+GameController.instance.getCard(targetID).Title+" esquive l'attaque";
		}
		
		GameController.instance.play(message);
	}
	
	public override bool isLaunchable(Skill s){
		return (s.nbLeft > 0) ;
	}
}
