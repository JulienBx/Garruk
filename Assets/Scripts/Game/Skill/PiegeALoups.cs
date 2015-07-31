using UnityEngine;
using System.Collections.Generic;

public class PiegeALoups : GameSkill
{
	public PiegeALoups(){
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
		int amount = base.skill.ManaCost;
		
		GameController.instance.addTileModifier(new Tile(targets[0], targets[1]), amount, ModifierType.Type_Wolftrap, ModifierStat.Stat_No, -1, 4, "Piège à loups", "Inflige "+amount+" dégats", "Permanent. Non visible du joueur adverse");
		GameView.instance.displaySkillEffect(GameController.instance.getCurrentPlayingCard(), "Piège posé", 4);
	}
	
	public override void activateTrap(int[] targets, int[] args){
		Debug.Log("J'active");
		GameController.instance.addCardModifier(targets[0], args[0], ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameView.instance.displaySkillEffect(targets[0], "HIT\n-"+args[0]+" PV", 5);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentTileTargets();
	}
	
	public override string getTargetText(Card c){
		
		int amount = base.skill.ManaCost;
		string s = "Pose un piège\n-"+amount+" PV";
		
		return s ;
	}
}
