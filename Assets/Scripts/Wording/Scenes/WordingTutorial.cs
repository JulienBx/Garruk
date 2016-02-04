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
		references.Add(new string[]{"Après un long voyage de 17 années dans l'espace, vous voici enfin arrivé à Cristalia. La planète et ses satellites sont actuellement les proies de nombreuses convoitises. Vous êtes un colon missionné par votre pays pour amasser le plus de richesses et de connaissances sur ce fameux cristal. Cristal qui confèrerait aux habitants de la planète d'étranges capacités... Saurez-vous les mettre à profit pour triompher des colons ennemis ?",""}); //2
	}
}