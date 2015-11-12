using UnityEngine;
using System.Collections.Generic;

public class Excitant : GameSkill
{
	public Excitant()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAllysButMeTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) < base.skill.proba){
			if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
			{                             
				GameController.instance.addTarget(target,1);
				if (base.card.isGenerous()){
					List<int> allys = GameView.instance.getAllys();
					if(allys.Count>1){
						allys.Remove(target);
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
		List<int> status =  new List<int>();
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		
		for(int i = 0 ; i < base.targets.Count ; i++){
			target = base.targets[i];
			targetCard = GameView.instance.getCard(target);
			receivers.Add (targetCard);
			if (base.results[i]==0){
				GameView.instance.displaySkillEffect(target, "Esquive", 4);
				status.Add (2);
				//receiversTexts.Add (text);
			}
			else if (base.results[i]==2){
				GameView.instance.displaySkillEffect(target, "Esquive", 4);
				//receiversTexts.Add (text);
				status.Add (3);
			}
			else if (base.results[i]==4){
				text = "Echec";
				GameView.instance.displaySkillEffect(target, "Echec", 4);
				//receiversTexts.Add (text);
			}
			else{
				if(base.results[i]==3){
					text = "Bonus Généreux\n";
				}
				else{
					text="";
				}
				
				text+="Jouera au prochain tour";
				
				//receiversTexts.Add (text);
				
				GameController.instance.advanceTurns(target);
				
				GameView.instance.displaySkillEffect(target, text, 4);
			}	
		}
		//GameView.instance.setSkillPopUp("Excitant", base.card, receivers, List<string> textsUpReceivers, List<string> textsDownReceivers);
	}

	public override string isLaunchable(){
		return GameView.instance.canLaunchOpponentsTargets();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		int amount = base.skill.proba;
		string text;
		
		text = "Joue au prochain tour\n";
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
