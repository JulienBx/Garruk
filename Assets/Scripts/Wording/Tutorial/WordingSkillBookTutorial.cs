using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSkillBookTutorial
{
	public static IList<string[]> helpContents;

	public static string getHelpContent(int idReference)
	{
		return helpContents[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingSkillBookTutorial()
	{
		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Les compétences",""}); //0
		helpContents.Add(new string[]{"Le cristal a entrainé des mutations inhabituelles sur les habitants de la planète, conférant à certains d'entre eux des compétences spécifiques. Bien connaitre ses compétences est la clé du succès dans ce monde, et la Cristalopedia vous permettra de vous documenter sur celles-ci.",""}); //1
		helpContents.Add(new string[]{"Rechercher une compétence",""}); //2
		helpContents.Add(new string[]{"Plus de 150 compétences étant disponibles, des filtres sont à votre disposition pour vous permettre de rechercher des compétences précises",""}); //3
		helpContents.Add(new string[]{"Compétence",""}); //4
		helpContents.Add(new string[]{"Le détail d'une compétence... bla bla bla bla",""}); //5
	}
}