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
		references.Add(new string[]{"Votre arrivée sur Cristalia est imminente. Cette planète, fameuse pour son minerai unique dans l'univers connu, le Cristal, attire de plus en plus de colons chaque année. Depuis la guerre ayant opposé les premiers colons aux habitants de la planète, les Cristoïdes, un gouvernement a été créé pour réguler l'arrivée de nouveaux habitants sur la planète. Les colons s'affrontent désormais au sein d'arènes sécurisées, selon les règles du code de guerre cristalien! Vous êtes missionné par la planète Terre pour collecter des Cristaux et découvrir leurs propriétés étranges.\nDès l'atterrissage de votre vaisseau, un Cristalien du département d'accueil et d'intégration des nouveaux colons demande à vous rencontrer.",""}); //2
	}
}