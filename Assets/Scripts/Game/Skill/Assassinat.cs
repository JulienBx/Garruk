using UnityEngine;
using System.Collections.Generic;

public class Assassinat : GameSkill
{
	public Assassinat(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
//	public override void launch()
//	{
//		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
//		GameView.instance.displayAdjacentOpponentsTargets();
//	}
//	
//	public override void resolve(List<int> targetsPCC)
//	{	
//		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
//			GameView.instance.hideTargets();
//		}
//		
//		int target = targetsPCC[0];
//		
//		if (Random.Range(1,101) < base.skill.proba){
//			if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive()){                             
//				GameController.instance.addTarget(target,1);
//			}
//			else{
//				GameController.instance.addTarget(target,0);
//			}
//		}
//		else{
//			GameController.instance.addTarget(target,4);
//		}
//		
//		GameController.instance.play();
//	}
//	
//	public override void applyOn(){
//		
//		Card targetCard ;
//		int target ;
//		string text ;
//		List<Card> receivers =  new List<Card>();
//		List<string> receiversTexts =  new List<string>();
//		
//		int amount ; 
//		
//		for(int i = 0 ; i < base.targets.Count ; i++){
//			target = base.targets[i];
//			targetCard = GameView.instance.getCard(target);
//			receivers.Add (targetCard);
//			if (base.results[i]==0){
//				text = "Esquive";
//				GameView.instance.displaySkillEffect(target, text, 4);
//				receiversTexts.Add (text);
//			}
//			else if (base.results[i]==4){
//				text = "Echec";
//				GameView.instance.displaySkillEffect(target, text, 4);
//				receiversTexts.Add (text);
//			}
//			else{
//				amount = targetCard.GetLife();
//			
//				text="MORT";
//				receiversTexts.Add (text);
//				
//				GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//			}	
//		}
//		GameView.instance.setSkillPopUp("lance <b>Assassinat</b>...", base.card, receivers, receiversTexts);
//	}
//	
//	public override string isLaunchable(){
//		return GameView.instance.canLaunchAdjacentOpponents();
//	}
//	
//	public override string getTargetText(int id, Card targetCard){
//		
//		int chances = base.skill.proba;
//		
//		int probaEsquive = targetCard.GetEsquive();
//		int proba = (chances)*(100-probaEsquive)/100;
//		string text = "HIT : ";
//		if (probaEsquive!=0){
//			text+=proba+"% : "+chances+"%(ASS) - "+probaEsquive+"%(ESQ)";
//		}
//		else{
//			text+=proba+"% : ";
//		}
//		
//		return text ;
//	}
}
