using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingCardTypes
{
	public static IList<string[]> descriptions;
	public static IList<string[]> names;

	public static string getName(int idCardType)
	{
		return names[idCardType][ApplicationModel.player.IdLanguage];
	}
	public static string getDescription(int idCardType)
	{
		return descriptions[idCardType][ApplicationModel.player.IdLanguage];
	}
	static WordingCardTypes()
	{
		names=new List<string[]>();
		names.Add(new string[]{"Medic","Medic"});
		names.Add(new string[]{"Assassin","Assassin"});
		names.Add(new string[]{"Prédateur","Predator"});
		names.Add(new string[]{"Trooper","Trooper"});
		names.Add(new string[]{"Mentaliste","Mentalist"});
		names.Add(new string[]{"Robot","Robot"});
		names.Add(new string[]{"Cristoide","Cristoid"});
		names.Add(new string[]{"Mystique","Zealot"});
		names.Add(new string[]{"Scientifique","Scientist"});
		names.Add(new string[]{"Terraformeur","Terraformer"});

		descriptions=new List<string[]>();
		descriptions.Add(new string[]{"Les MEDICS peuvent aider leurs alliés sur le champ de bataille grâce à leurs compétences uniques.","Medic units can heal or boost their allys with their unique set of skills"});
		descriptions.Add(new string[]{"Les ASSASSINS sont des unités mobiles capable d'infliger de lourds dégats ou de piéger le terrain.","Assassins are fast units but do not have much HP. They can use a wide set of skills to inflict average damages"});
		descriptions.Add(new string[]{"Les PREDATEURS sont les unités les plus puissantes de Cristalia mais ne sont efficaces qu'au corps à corps","Predators are the strongest units, but they are slow and need to come closer to their ennemies to attack"});
		descriptions.Add(new string[]{"Les TROOPERS préfèrent attaquer à distance et sont très peu résistants. Protégez-les au maximum!","The trooper can use his long range weapons to attack ennemies. Troopers are difficult to protect as they do not have much HP"});
		descriptions.Add(new string[]{"Les MENTALISTES disposent de compétences unques pour perturber les stratégies et unités ennemies.","Mentalist master illusions and can get to their ennemies mind to confuse or use them"});
		descriptions.Add(new string[]{"Les ROBOTS sont des unités très résistantes mais peu puissantes, à moins qu'un SCIENTIFIQUE ne les aide","ROBOTS are strenghtful, powerful, but slow... unless SCIENTISTS help them"});
		descriptions.Add(new string[]{"Les CRISTOIDES peuvent se servir des cristaux, et se renforcent mutuellement sur le champ de bataille.","CRISTOIDS can use cristals on the battlefield, and help each other during fights"});
		descriptions.Add(new string[]{"Les MYSTIQUES disposent de compétences puissantes mais qui peuvent également pénaliser vos unités.","MYSTICS use powerful skills but these can also affect your own units"});
		descriptions.Add(new string[]{"Les SCIENTIFIQUES peuvent aider leurs alliés en façonnant des armures ou des batiments de soutien","SCIENTISTS can use their skills to create weapons or build facilities for their team."});
		descriptions.Add(new string[]{"Les TERRAFORMEURS sont les maitres du champ de bataille, et peuvent utiliser celui-ci pour triompher!","TERRAFORMERS can use the battlefield in many ways to turn the tide of the fight"});
	}
}