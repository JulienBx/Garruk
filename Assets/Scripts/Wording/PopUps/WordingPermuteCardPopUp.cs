using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingPermuteCardPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingPermuteCardPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Le code de guerre de Cristalia impose de ne pas sélectionner deux unités identiques au sein d'une équipe, souhaitez-vous remplacer l'unité de votre équipe par cette unité ?","Cristalian War Code, article 3 : You shall not select two units of the same type in your team. Do you want to replace your deck unit with this new one ? "}); //0
		references.Add(new string[]{"Remplacer","Replace"}); //1
		references.Add(new string[]{"Annuler","Cancel"}); //2
	}
}