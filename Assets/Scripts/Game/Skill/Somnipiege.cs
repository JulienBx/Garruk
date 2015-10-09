using UnityEngine;
using System.Collections.Generic;

public class Somnipiege : GameSkill
{
	public Somnipiege(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentTileTargets();
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		GameController.instance.addTargetTile(targetsTile[0].x, targetsTile[0].y, 1);
		GameController.instance.play();
	}
	
	public override void applyOn(){
		Tile target ;
		string text ;
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		
		int amount ; 
		
		for(int i = 0 ; i < base.tileTargets.Count ; i++){
			target = base.tileTargets[i];	
			amount = base.skill.ManaCost;
			
			text="Piège posé";
			
			GameController.instance.addTileModifier(target, amount, ModifierType.Type_SleepingTrap, ModifierStat.Stat_No, -1, 2, "Piège endormissant", "Endort l'adversaire. "+amount+"% de chances de se réveiller chaque tour", "Permanent. Non visible du joueur adverse");
			GameView.instance.displaySkillEffect(GameController.instance.getCurrentPlayingCard(), text, 4);
		}
		if(!GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("pose un <b>somnipiège</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override void activateTrap(int[] targets, int[] args){
		GameController.instance.addCardModifier(targets[0], args[0], ModifierType.Type_Sleeping, ModifierStat.Stat_No, -1, 53, "ENDORMI", "Le héros est endormi et possède "+args[0]+"% de chances de se réveiller à chaque tour", "");
		GameView.instance.displaySkillEffect(targets[0], "PIEGE\nS'endort", 5);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentTileTargets();
	}
	
	public override string getTargetText(int i, Card c){
		
		int amount = base.skill.ManaCost;
		string s = "Pose un piège\nEndort";
		
		return s ;
	}
}
