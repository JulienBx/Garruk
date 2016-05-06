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
		references.Add(new string[]{"Liste des compétences","Skills"}); //0
		references.Add(new string[]{"Cristalopedia","Cristalopedia"}); //1
		references.Add(new string[]{"Ma collection","Stats"}); //2
		references.Add(new string[]{"Disciplines","Skill types"}); //3
		references.Add(new string[]{"Factions","Factions"}); //4
		references.Add(new string[]{"Vous avez découvert la compétence","Discovered skill"}); //5
		references.Add(new string[]{"Les compétences passives sont affichées en noir","Discovered passive skill"}); //6
		references.Add(new string[]{"Vous n'avez pas découvert ces compétences","Undiscovered skill"}); //7
		references.Add(new string[]{"Compétences découvertes","Discovered skills"}); //8
		references.Add(new string[]{"Niveau de collection","Collection progress"}); //9
		references.Add(new string[]{"Points de collectionneur","Collection points"}); //10
		references.Add(new string[]{"Classement Collectionneur","Collection ranking"}); //11
		references.Add(new string[]{"La Cristalopédia rassemble l'ensemble des connaissances que vous possédez sur les unités et compétences cristaliennes.","The Cristalopedia gathers all your knowledge about Cristalian units and their skills"}); //12
		references.Add(new string[]{"Découvrir l'ensemble des compétences de Cristalia facilitera vos combats et vous permettra de progresser dans votre classement de collectionneur. Les meilleurs collectionneurs seront récompensés chaque mois.","These stats will help you understand how much information you have gathered about Cristalia. The more information you have, the more wins you will get during fights"}); //13
		references.Add(new string[]{"Non découverte","Undiscovered"}); //14
		references.Add(new string[]{"Découverte (niveau ","Discovered (level "}); //15
		references.Add(new string[]{")",")"}); //16
		references.Add(new string[]{"Compétence",""}); //17
		references.Add(new string[]{"Accédez au détail des compétences pour voir la liste de leurs effets selon le niveau atteint",""}); //18
		references.Add(new string[]{"Niveau",""}); //19
		references.Add(new string[]{"Plus le niveau d'une compétence est élevée, plus son impact est grand sur le champ de bataille. Au fil des combats, vos unités progressent et le niveau de leurs compétences augmente.",""}); //20
	}
}