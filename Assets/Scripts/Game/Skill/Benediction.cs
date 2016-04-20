using UnityEngine;
using System.Collections.Generic;

public class Benediction : GameSkill
{
	public Benediction()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Benediction";
		base.ciblage = 12 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().setTexts("Malédiction", "Choisis une faction. Les unités de cette faction recevront un malus d'attaque");
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().displayAllAllyTypes();
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(true);
		GameController.instance.play(GameView.instance.runningSkill);
	}

	public override void resolve(List<Tile> targets)
	{	                     
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(false);

		List<int> characters = GameView.instance.getEveryone();
		for(int i = 0 ; i < characters.Count ; i++){
			if(GameView.instance.getCard(characters[i]).CardType.Id == targets[0].x){
				GameController.instance.applyOn(characters[i]);
			}
		}
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
			
		int level = GameView.instance.getCurrentSkill().Power*5+10;
		GameCard targetCard = GameView.instance.getCard(target);
		int malus = Mathf.Max(1,Mathf.RoundToInt(targetCard.getAttack()*level/100f));

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(malus, 1, 103, base.name, ". Actif 1 tour"));
		GameView.instance.displaySkillEffect(target, "+"+malus+"ATK", 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 103);
	}
}
