using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSkillBookHelp
{
	public static IList<string[]> helpContents;

	public static string getHelpContent(int idReference)
	{
		return helpContents[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingSkillBookHelp()
	{
		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Les Cristaliens ont développé au contact du Cristal des compétences spécifiques, causées par des mutations génétiques. La connaissance de ces compétences est indispensable pour réussir dans l'arène, et la Cristalopedia offerte à chaque colon permet de vous renseigner sur celles-ci",""}); //1
		helpContents.Add(new string[]{"140 compétences ont été découvertes sur Cristalia, utilisez les filtres pour les retrouver!",""}); //3
		helpContents.Add(new string[]{"Chaque ccompéter",""}); //4
		helpContents.Add(new string[]{"Le détail d'une compétence... bla bla bla bla",""}); //5
	}
}