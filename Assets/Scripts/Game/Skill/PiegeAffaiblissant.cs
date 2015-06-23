using UnityEngine;
using System.Collections.Generic;

public class PiegeAffaiblissant : GameSkill
{
	public PiegeAffaiblissant(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAdjacentTileTargets();
		GameController.instance.displayMyControls("Piège affaiblissant");
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		int[] targets = new int[2];
		targets[0] = targetsTile[0].x;
		targets[1] = targetsTile[0].y;
		GameController.instance.startPlayingSkill();
		GameController.instance.applyOn(targets);
		
		GameController.instance.playSkill();
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		
		GameController.instance.addTileModifier(new Tile(targets[0], targets[1]), amount, ModifierType.Type_WeakeningTrap, ModifierStat.Stat_No, -1, 1, "Piège affaiblissant", "Réduit de "+amount+"% l'attaque du héros touché pendant 2 tours", "Permanent. Non visible du joueur adverse");
		GameController.instance.displaySkillEffect(targets[0], "Piège posé", 3, 2);
	}
	
	public override void activateTrap(int[] targets, int[] args){
		int amount = args[0]*GameController.instance.getCard(targets[0]).GetAttack()/100;
		GameController.instance.addCardModifier(targets[0], -1*amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 2, 5, "Sape", "Attaque diminuée de "+amount, "Actif 2 tours");
		GameController.instance.displaySkillEffect(targets[0], "Déclenche le piège et diminue de "+amount+" l'attaque", 3, 1);
	}
	
	public override bool isLaunchable(Skill s){
		bool isLaunchable = false ;
		List<Tile> neighbourTiles = GameController.instance.getCurrentPCC().tile.getImmediateNeighbouringTiles();
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = GameController.instance.getTile(t.x, t.y).characterID;
			if (playerID == -1)
			{
				if (!GameController.instance.getTile(t.x, t.y).tile.isStatModifier)
				{
					isLaunchable = true ;
				}
			}
		}
		return isLaunchable ;
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
		
		int degats = GameController.instance.getCurrentSkill().ManaCost;
		int amount = degats*c.GetAttack()/100;
		
		h.addInfo("Pose un piège diminuant de "+degats+" l'attaque pendant 2 tours",0);
		
		return h ;
	}
	
	public override string getPlayText(){
		return "Piège affaiblissant" ;
	}
}
