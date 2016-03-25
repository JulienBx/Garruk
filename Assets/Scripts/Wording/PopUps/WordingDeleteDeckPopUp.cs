using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingDeleteDeckPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingDeleteDeckPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Voulez-vous vraiment supprimer cette équipe ? \n","Do you really want to suppress this team ? \n"}); //0
		references.Add(new string[]{".\n\n Attention cette action est irréversible ! \n Vos unités ne seront en revanche pas supprimées.\n",".\n\n This action can not be undone. You will not lose your units by destroying the team.\n"}); //1
		references.Add(new string[]{"Oui!","Yes!"}); //2
	}
}