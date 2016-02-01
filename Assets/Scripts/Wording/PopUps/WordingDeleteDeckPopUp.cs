using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingDeleteDeckPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingDeleteDeckPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Confirmez vous la suppression de l'équipe ",""}); //0
		references.Add(new string[]{".\n\n Attention cette action est irréversible ! \n Vous perdrez définitivement votre deck mais vous conserverez vos unités.\n",""}); //1
		references.Add(new string[]{"Confirmer",""}); //2
	}
}