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
		references.Add(new string[]{"Accepter","Next"}); //0
		references.Add(new string[]{"Terrien, bienvenue à Cristalia!","Welcome to Cristalia"}); //1
		references.Add(new string[]{"Votre arrivée sur Cristalia est imminente. De nombreux colonisateurs se disputent les richesses de la planète en s'affrontant dans des batailles rangées. Vous allez devoir recruter une équipe de combattants, et utiliser au mieux les pouvoirs étranges des habitants pour triompher de vos adversaires.\n\nA votre arrivée, un Cristalien chargé de l'accueil des colonisateurs vient vous rencontrer...",""}); //2
	}
}