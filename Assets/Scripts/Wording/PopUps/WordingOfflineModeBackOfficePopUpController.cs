using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingOfflineModeBackOfficePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingOfflineModeBackOfficePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Oui","Yes"}); //0
		references.Add(new string[]{"Non","No"}); //1
		references.Add(new string[]{"Souhaitez-vous, vous reconnecter ?","Do you want to reconnect ?"}); //2
		references.Add(new string[]{"Pour accéder à cette action, vous devez vous connecter. Souhaitez-vous, vous reconnecter ?","To access this content, you must be online! Do you want to reconnect ?"}); //2
	}
}