using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingLobbyTutorial
{
	public static IList<string[]> helpContents;

	public static string getHelpContent(int idReference)
	{
		return helpContents[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingLobbyTutorial()
	{
		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Conquérir un satellite",""}); //0
		helpContents.Add(new string[]{"La conquete des satellites de Cristalia vous permettra de gagner de plus en plus de cristal. Pour conquérir un satellite, il vous faudra défier de nombreux colons et les vaincre. Vous pouvez ici consulter l'état de la guerre sur le satellite sur lequel vous vous trouvez",""}); //1
		helpContents.Add(new string[]{"Mes derniers combats",""}); //2
		helpContents.Add(new string[]{"Accédez ici à un compte-rendu de vos derniers combats sur le satellite",""}); //3
		helpContents.Add(new string[]{"Mon satellite",""}); //4
		helpContents.Add(new string[]{"La guerre fait rage sur les satellites de Cristalia. Plus vous deviendrez fort, plus vous pourrez accéder à des satellites riches en ressources. Consultez ici la richesse du satellite et les conditions d'accès au suivant",""}); //5
		helpContents.Add(new string[]{"Mes statistiques",""}); //6
		helpContents.Add(new string[]{"Ces statistiques vous permettront d'évaluer l'état de la conquete du satellite et votre niveau par rapport aux autres colons",""}); //7
	}
}