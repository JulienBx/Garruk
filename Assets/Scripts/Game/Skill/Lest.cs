using UnityEngine;
using System.Collections.Generic;

public class Lest : GameSkill
{
	public Lest()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) < base.skill.proba){
			if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive()){                             
				GameController.instance.addTarget(target,1);
				if (base.card.isGenerous()){
					List<int> allys = GameView.instance.getAllys();
					if(allys.Count>1){
						allys.Remove(target);
						for (int i = 0 ; i < allys.Count ; i++){
							target = allys[Random.Range(0,allys.Count)];
							
							if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
							{
								GameController.instance.addTarget(target,3);
							}
							else{
								GameController.instance.addTarget(target,2);
							}
						}
					}
				}
			}
			else{
				GameController.instance.addTarget(target,0);
			}
		}
		else{
			GameController.instance.addTarget(target,4);
		}
		
		GameController.instance.play();
	}
	
	public override void applyOn(){
		Card targetCard ;
		int target ;
		string text ;
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		
		for(int i = 0 ; i < base.targets.Count ; i++){
			target = base.targets[i];
			targetCard = GameView.instance.getCard(target);
			receivers.Add (targetCard);
			if (base.results[i]==0){
				text = "Esquive";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else if (base.results[i]==2){
				text = "Bonus 'Généreux'\nEsquive";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else if (base.results[i]==4){
				text = "Echec";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else{
				if(base.results[i]==3){
					text = "Bonus Généreux\n";
				}
				else{
					text="";
				}
				
				if(base.skill.Level<=2){
					text+="-1 MOV pendant un tour";
					GameController.instance.addCardModifier(target, -1, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, 12, "LEST",  text, "");
				}
				else if(base.skill.Level<=4){
					text+="-2 MOV pendant un tour";
					GameController.instance.addCardModifier(target, -2, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, 12, "LEST",  text, "");
				}
				else if(base.skill.Level<=6){
					text+="-3 MOV pendant un tour";
					GameController.instance.addCardModifier(target, -3, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, 12, "LEST",  text, "");
				}
				else if(base.skill.Level<=8){
					text+="-3 MOV pendant deux tours";
					GameController.instance.addCardModifier(target, -3, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 2, 12, "LEST",  text, "");
				}
				else if(base.skill.Level<=10){
					text+="-4 MOV pendant deux tours";
					GameController.instance.addCardModifier(target, -4, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 2, 12, "LEST",  text, "");
				}
				
				receiversTexts.Add (text);
				
				GameView.instance.displaySkillEffect(target, text, 5);
			}	
		}		
		
		GameView.instance.setSkillPopUp("lance <b>Lest</b>...", base.card, receivers, receiversTexts);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchOpponentsTargets();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		int amount = base.skill.ManaCost;
		
		int baseD = targetCard.GetMove();
		int deplacement = Mathf.FloorToInt((amount)*baseD/100)+1;
		
		if (deplacement >= baseD){
			deplacement = baseD - 1 ;
		}
		
		string text = "MOV: "+baseD+"->"+(baseD-deplacement)+"\n";
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
