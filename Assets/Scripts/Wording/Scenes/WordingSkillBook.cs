using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSkillBook
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingSkillBook()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Compétences","Skills"}); //0
		references.Add(new string[]{"Cristalopedia","Cristalopedia"}); //1
		references.Add(new string[]{"Statistiques","Stats"}); //2
		references.Add(new string[]{"Skills","Skill types"}); //3
		references.Add(new string[]{"Factions","Factions"}); //4
		references.Add(new string[]{"Compétence découverte","Discovered skill"}); //5
		references.Add(new string[]{"Compétence passive découverte","Discovered passive skill"}); //6
		references.Add(new string[]{"Compétence non découverte","Undiscovered skill"}); //7
		references.Add(new string[]{"Compétences découvertes","Discovered skills"}); //8
		references.Add(new string[]{"Avancement de la collection ","Collection progress"}); //9
		references.Add(new string[]{"Points de collection","Collection points"}); //10
		references.Add(new string[]{"Classement collection","Collection ranking"}); //11
		references.Add(new string[]{"La Cristalopédia rassemble l'ensemble des connaissances que vous possédez sur les unités et compétences cristaliennes.","The Cristalopedia gathers all your knowledge about Cristalian units and their skills"}); //12
		references.Add(new string[]{"Ces statistiques votre degré de connaissance de Cristalia. Mieux vous maitriserez les compétences, plus les combats vous sembleront faciles","These stats will help you understand how much information you have gathered about Cristalia. The more information you have, the more wins you will get during fights"}); //13
		references.Add(new string[]{"Non découverte","Undiscovered"}); //14
		references.Add(new string[]{"Découverte (niveau ","Discovered (level "}); //15
		references.Add(new string[]{")",")"}); //16
		references.Add(new string[]{"Compétence",""}); //17
		references.Add(new string[]{"Cliquez sur une compétence pour avoir le détail",""}); //18
	}
}