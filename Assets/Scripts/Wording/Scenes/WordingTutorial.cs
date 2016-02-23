using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingTutorialScene
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingTutorialScene()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Suivant",""}); //0
		references.Add(new string[]{"Bienvenue à Cristalia !",""}); //1
		references.Add(new string[]{"Après un long voyage de 17 années dans l'espace, vous voici enfin arrivé à Cristalia. Votre planète de naissance vous a missionné d'en apprendre plus sur le Cristal, cette ressource dont seule Cristalia dispose dans l'univers connu. Ce minéral confèrerait aux habitants de la planète d'étranges pouvoirs, et de nombreux colons ont afflué récemment pour tenter de s'en emparer!",""}); //2
	}
}