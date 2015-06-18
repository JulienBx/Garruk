using UnityEngine;
using System.Collections.Generic;

public class EauBenite : GameSkill
{
	public EauBenite()
	{
	}
	
	public override void launch()
	{
		GameController.instance.lookForValidation(true, "Eau Bénite cible tous les alliés", "Lancer Eau Bénite");
	}

	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		
		int debut;
		int fin;
		if (GameController.instance.isFirstPlayer)
		{
			debut = GameController.instance.limitCharacterSide;
			fin = GameController.instance.playingCards.Length;
		} else
		{
			debut = 0;
			fin = GameController.instance.limitCharacterSide;
		}
		for (int i = debut; i < fin; i++)
		{
			PlayingCardController pcc = GameController.instance.getPCC(i);
			if (!pcc.isDead)
			{
				GameController.instance.addCardModifier(-amount, pcc.IDCharacter, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage, -1);
			}
		}

		GameController.instance.addGameEvent(GameController.instance.getCurrentSkill().Action, "");
		
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé Eau Bénite \n " 
			+ convertStatToString(ModifierStat.Stat_Heal)
			+ " "
			+ amount 
			+ " " 
			+ convertStatToString(ModifierStat.Stat_Life));
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
