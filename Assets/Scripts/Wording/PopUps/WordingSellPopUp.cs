using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSellPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingSellPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Révoquer cette unité vous rapportera ","Discharging the unit will pay you off "}); //0
		references.Add(new string[]{" cristaux). \n\n Attention cette unité sera perdue à jamais! \n"," cristals. This action can not be undone!"}); //1
		references.Add(new string[]{"Confirmer","Confirm"}); //2
	}
}