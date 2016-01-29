using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSocial
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingSocial()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Défier",""}); //0
		references.Add(new string[]{"Oui",""}); //1
		references.Add(new string[]{"Non",""}); //2
		references.Add(new string[]{"n'est pas en ligne",""}); //3
		references.Add(new string[]{"est disponible pour un défi",""}); //4
		references.Add(new string[]{"est entrain de jouer",""}); //5
		references.Add(new string[]{"En attente de la réponse de votre ami ...",""}); //6
		references.Add(new string[]{"Votre ami a annulé le défi",""});//7
	}
}