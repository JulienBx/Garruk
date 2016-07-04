using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingCardTypes
{
	public static IList<string[]> descriptions;
	public static IList<string[]> names;
	private static IList<int> idCardTypes;

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

		idCardTypes=new List<int>();
		names=new List<string[]>();
		descriptions=new List<string[]>();

		idCardTypes.Add(0);
		names.Add(new string[]{"Medic","Medic"});
		descriptions.Add(new string[]{"Les MEDICS peuvent aider leurs alliés sur le champ de bataille grâce à leurs compétences uniques.","Medic units can heal or boost their allys with their unique set of skills"});
		idCardTypes.Add(1);
		names.Add(new string[]{"Assassin","Assassin"});
		descriptions.Add(new string[]{"Les ASSASSINS sont des unités mobiles capable d'infliger de lourds dégats ou de piéger le terrain.","Assassins are fast units but do not have much HP. They can use a wide set of skills to inflict average damages"});
		idCardTypes.Add(2);
		names.Add(new string[]{"Prédateur","Predator"});
		descriptions.Add(new string[]{"Les PREDATEURS sont les unités les plus puissantes de Cristalia mais ne sont efficaces qu'au corps à corps","Predators are the strongest units, but they are slow and need to come closer to their ennemies to attack"});
		idCardTypes.Add(3);
		names.Add(new string[]{"Trooper","Trooper"});
		descriptions.Add(new string[]{"Les TROOPERS préfèrent attaquer à distance et sont très peu résistants. Protégez-les au maximum!","The trooper can use his long range weapons to attack ennemies. Troopers are difficult to protect as they do not have much HP"});
		idCardTypes.Add(4);
		names.Add(new string[]{"Mentaliste","Mentalist"});
		descriptions.Add(new string[]{"Les MENTALISTES disposent de compétences unques pour perturber les stratégies et unités ennemies.","Mentalist master illusions and can get to their ennemies mind to confuse or use them"});
		idCardTypes.Add(5);
		names.Add(new string[]{"Robot","Robot"});
		descriptions.Add(new string[]{"Les ROBOTS sont des unités très résistantes mais peu puissantes, à moins qu'un SCIENTIFIQUE ne les aide","ROBOTS are strenghtful, powerful, but slow... unless SCIENTISTS help them"});
		idCardTypes.Add(6);
		names.Add(new string[]{"Cristoide","Cristoid"});
		descriptions.Add(new string[]{"Les CRISTOIDES peuvent se servir des cristaux, et se renforcent mutuellement sur le champ de bataille.","CRISTOIDS can use cristals on the battlefield, and help each other during fights"});
		idCardTypes.Add(7);
		names.Add(new string[]{"Mystique","Zealot"});
		descriptions.Add(new string[]{"Les MYSTIQUES disposent de compétences puissantes mais qui peuvent également pénaliser vos unités.","MYSTICS use powerful skills but these can also affect your own units"});
		idCardTypes.Add(8);
		names.Add(new string[]{"Scientifique","Scientist"});
		descriptions.Add(new string[]{"Les SCIENTIFIQUES peuvent aider leurs alliés en façonnant des armures ou des batiments de soutien","SCIENTISTS can use their skills to create weapons or build facilities for their team."});
		idCardTypes.Add(9);
		names.Add(new string[]{"Terraformeur","Terraformer"});
		descriptions.Add(new string[]{"Les TERRAFORMEURS sont les maitres du champ de bataille, et peuvent utiliser celui-ci pour triompher!","TERRAFORMERS can use the battlefield in many ways to turn the tide of the fight"});
	}
}