using UnityEngine;
using System.Collections.Generic;

public class TirALarc : GameSkill
{
	public TirALarc()
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
		int nbHitMax = Random.Range(1,3);
		int arg = 0;
		
		for (int i = 0 ; i < nbHitMax ; i++){
			if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
			{                             
				arg++;
			}
		}
		
		if (arg!=0){
			GameController.instance.addTarget(target,1,arg);
		}
		else{
			GameController.instance.addTarget(target,0,0);
		}
		GameController.instance.play();
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
			else{
				amount = Mathf.Min(targetCard.GetLife(),base.values[i]*base.skill.ManaCost*(1-(targetCard.GetBouclier()/100)));
				
				text="Hits X"+base.values[i]+"\n-"+amount+" PV";
				
				if(targetCard.GetLife()==amount){
					text+="\nMORT";
				}
				receiversTexts.Add (text);
				
				GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
				
				GameView.instance.displaySkillEffect(target, text, 5);
			}	
		}
		if(!GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Tir à l'arc</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchOpponentsTargets();
	}
	
	public override string getTargetText(int i, Card targetCard){
		
		int proba;
		int probaEsquive = targetCard.GetEsquive();
		int currentLife = targetCard.GetLife();
		int bouclier = targetCard.GetBouclier();
		
		int damageBonusPercentage = GameView.instance.getCard(GameController.instance.getCurrentPlayingCard()).GetDamagesPercentageBonus(targetCard);
		int amount = this.skill.ManaCost*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		string text ;
		
		if(currentLife-Mathf.Min(currentLife,amount)==0){
			text = "PV : "+currentLife+"->0";
		}
		else{
			text = "PV : "+currentLife+"->"+(currentLife-Mathf.Min(currentLife,amount))+"-"+(currentLife-Mathf.Min(currentLife,(2*amount)));
		}
		
		text += "\nHIT : ";
		if (probaEsquive!=0){
			proba = 100-probaEsquive;
			text+=proba+"% : "+100+"%(ATT) - "+probaEsquive+"%(ESQ)";
		}
		else{
			proba = 100;
			text+=proba+"%";
		}
		
		return text ;
	}
}
