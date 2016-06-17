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
		helpContents.Add(new string[]{"Les Cristaliens ont développé au contact du Cristal des compétences spécifiques. Les connaitre est indispensable pour gagner des combats. La Cristalopedia est offerte à tous les nouveaux arrivants pour les former aux compétences cristaliennes","Cristalia inhabitants have been developing strange skills due to Cristal exposure. Knowing these skills will help you winning fights. Cristalopedia is offered to every newcomer on Cristalia to give them an accelerated training!"}); //1
		helpContents.Add(new string[]{"140 compétences ont été découvertes sur Cristalia, utilisez les filtres pour les retrouver!","140 skills have discovered yet, use filters to find the one you are looking for"}); //3
	}
}