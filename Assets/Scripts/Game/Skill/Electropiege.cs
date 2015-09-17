using UnityEngine;
using System.Collections.Generic;

public class Electropiege : GameSkill
{
	public Electropiege(){
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
		
		int[] targets = new int[2];
		targets[0] = targetsTile[0].x;
		targets[1] = targetsTile[0].y;
		GameController.instance.applyOn(targets);
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
		if(targets[2]==0){
			int amount = base.skill.ManaCost;
			GameController.instance.addTileModifier(new Tile(targets[0], targets[1]), amount, ModifierType.Type_Wolftrap, ModifierStat.Stat_No, -1, 4, "Electropiege", "Inflige "+amount+" dégats", "Permanent. Non visible du joueur adverse");
			GameView.instance.displaySkillEffect(GameController.instance.getCurrentPlayingCard(), "Piège posé", 4);
		}
		else{
			int amount = targets[3];
			GameController.instance.addTileModifier(new Tile(targets[0], targets[1]), amount, ModifierType.Type_Wolftrap, ModifierStat.Stat_No, -1, 4, "Electropiege", "Inflige "+amount+" dégats", "Permanent. Non visible du joueur adverse");
		}
	}
	
	public override void activateTrap(int[] targets, int[] args){
		GameController.instance.addCardModifier(targets[0], args[0], ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		if(GameView.instance.getCard(targets[0]).GetLife()>0){
			GameView.instance.displaySkillEffect(targets[0], "HIT\n-"+args[0]+" PV", 5);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentTileTargets();
	}
	
	public override string getTargetText(int i, Card c){
		
		int amount = base.skill.ManaCost;
		string s = "Pose un piège\n-"+amount+" PV";
		
		return s ;
	}
}
