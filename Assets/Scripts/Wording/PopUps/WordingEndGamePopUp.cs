using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingEndGamePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingEndGamePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"BRAVO !\n\nPour gagner plus de cristaux, jouez en mode conquête (une fois l'entrainement terminé)","CONGRATULATIONS!\n\nNow try the conquest mode to earn more cristals!"}); //0
		references.Add(new string[]{"DOMMAGE !\n\nContinuez à vous entrainer ou essayez le mode conquête(débloqué une fois l'entrainement terminé) pour gagner plus de cristaux","WHAT A PITY !\n\nPractice in the arena or try the conquest mode to earn more cristals"}); //1
		references.Add(new string[]{"OK!","OK!"}); //2
	}
}