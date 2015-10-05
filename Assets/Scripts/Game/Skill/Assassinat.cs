using UnityEngine;
using System.Collections.Generic;

public class Assassinat : GameSkill
{
	public Assassinat(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameView.instance.getGC().initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		int successChances = base.skill.ManaCost;
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
		{                             
			if (Random.Range(1,101) <= successChances)
			{ 
				GameView.instance.getGC().addTarget(target,1);
			}
			else{
				GameView.instance.getGC().addTarget(target,2);
			}
		}
		else{
			GameView.instance.getGC().addTarget(target,0);
		}
		GameView.instance.getGC().play();
	}
	
	public override void applyOn(){
		
		Card targetCard ;
		int target ;
		string text ;
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		
		int amount ; 
		
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
				text = "Echec";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else{
				amount = targetCard.GetLife();
			
				text="MORT";
				receiversTexts.Add (text);
				
				GameView.instance.getGC().addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
			}	
		}
		if(!GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Assassinat</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		int chances = base.skill.ManaCost;
		
		int probaEsquive = targetCard.GetEsquive();
		int proba = (chances)*(100-probaEsquive)/100;
		string text = "HIT : ";
		if (probaEsquive!=0){
			text+=proba+"% : "+chances+"%(ASS) - "+probaEsquive+"%(ESQ)";
		}
		else{
			text+=proba+"% : ";
		}
		
		return text ;
	}
}
