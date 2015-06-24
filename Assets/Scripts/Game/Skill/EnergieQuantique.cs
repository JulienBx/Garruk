using UnityEngine;
using System.Collections.Generic;

public class EnergieQuantique : GameSkill
{
	public EnergieQuantique()
	{
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int[] targets = new int[1];
		GameController.instance.startPlayingSkill();
		int debut = 0 ; 
		if(GameController.instance.isFirstPlayer){
			debut = GameController.instance.limitCharacterSide;
		}
		int index = Random.Range(1,GameController.instance.nbOtherPlayersAlive()+1);
		int compteurDead = 0;
		bool hasFound = false;
		while (!hasFound && debut<20){
			if (!GameController.instance.getPCC(debut).isDead){
				if(compteurDead==index){
					targets[0] = debut;
					hasFound = true ;
				}
				else{
					compteurDead++;
				}
			}
			debut++;
		}
		
		if (Random.Range(1,101) > GameController.instance.getCard(targets[0]).GetEsquive())
		{ 
			GameController.instance.applyOn(targets);
		}
		else{
			GameController.instance.failedToCastOnSkill(targets);
		}
		
		GameController.instance.playSkill();
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets, int[] args){
		
		Card targetCard = GameController.instance.getCard(targets[0]);
		int currentLife = targetCard.GetLife();
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		amount = Mathf.Min(currentLife,amount);
		
		GameController.instance.addCardModifier(targets[0], amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		
		GameController.instance.displaySkillEffect(targets[0], "Inflige "+amount+" dégats", 3, 1);
	}
	
	public override void failedToCastOn(int[] targets){
		for (int i = 0 ; i < targets.Length ; i++){
			GameController.instance.displaySkillEffect(targets[i], "Echec", 3, 1);
		}
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
	
	public override string getPlayText(){
		return "Energie quantique" ;
	}
}
