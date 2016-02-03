using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingTutorial
{
	public static IList<string[]> references;
	public static IList<string[]> tutorialContents;
	public static IList<string[]> helpContents;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	public static string getTutorialContent(int idReference)
	{
		return tutorialContents[idReference][ApplicationModel.player.IdLanguage];
	}
	public static string getHelpContent(int idReference)
	{
		return helpContents[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingTutorial()
	{

		references=new List<string[]>();
		references.Add(new string[]{"Quitter le tutoriel",""}); //0
		references.Add(new string[]{"Quitter l'aide",""}); //1
		references.Add(new string[]{"Vous êtes encore en apprentissage !  \n Attendez d'avoir disputé votre premier match \npour pouvoir ensuite réaliser cette action",""}); //2
		references.Add(new string[]{"Continuer",""}); //3

		tutorialContents=new List<string[]>();
		tutorialContents.Add(new string[]{"Une équipe, vite !",""}); //0
		tutorialContents.Add(new string[]{"Avant de pouvoir aller plus loin, il vous faut créer une équipe de combattants prete à vous défendre dans cet environnement hostile",""}); //1
		tutorialContents.Add(new string[]{"Premier combat",""}); //2
		tutorialContents.Add(new string[]{"Il est temps de participer à votre premier combat, votre adversaire vous attend dans l'arène",""}); //3

		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Classe d'unité",""}); //0
		helpContents.Add(new string[]{"Les cristaliens se divisent en dix classes d'unités, chacune possédant ses propres compétences. \n\nLa première compétence de l'unité est sa compétence passive, lui conférant des bonus permanents.Enfin l'expérience de l'unité lui permet d'acquérir de nouvelles compétences et de faire progresser ses caractéristiques",""}); //1
		helpContents.Add(new string[]{"Compétences",""}); //2
		helpContents.Add(new string[]{"Chaque Cristalien a développé des compétences uniques au contact du Cristal (plus de 150 découvertes à ce jour). Chaque cristalien peut posséder 3 compétences en plus de sa compétence passive.",""}); //3
		helpContents.Add(new string[]{"Caractéristiques",""}); //4
		helpContents.Add(new string[]{"Les caractéristiques déterminent la force et la santé de l'unité. Dépendantes de la classe, elles peuvent etre améliorées avec l'expérience. La santé se régènère à la fin de chaque combat, les blessures létales ayant été abolies il y a quelques années",""}); //5
		helpContents.Add(new string[]{"Compétence",""}); //6
		helpContents.Add(new string[]{"Le détail d'une compétence... bla bla bla bla",""}); //7


	}
}