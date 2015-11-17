using UnityEngine;
using System.Collections.Generic;

public class Antibiotique : GameSkill
{
	public Antibiotique()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
//	public override void launch()
//	{
//		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
//		GameView.instance.displayAllButMeModifiersTargets();
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
//			if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
//			{                             
//				GameController.instance.addTarget(target,1);
//				if (base.card.isGenerous()){
//					List<int> allys = GameView.instance.getEveryoneButMe();
//					if(allys.Count>1){
//						allys.Remove(target);
//						
//						target = allys[Random.Range(0,allys.Count)];
//						
//						if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
//						{
//							GameController.instance.addTarget(target,3);
//						}
//						else{
//							GameController.instance.addTarget(target,2);
//						}
//					}
//				}
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
//		for(int i = 0 ; i < base.targets.Count ; i++){
//			target = base.targets[i];
//			targetCard = GameView.instance.getCard(target);
//			receivers.Add (targetCard);
//			if (base.results[i]==0){
//				text = "Esquive";
//				GameView.instance.displaySkillEffect(target, text, 4);
//				receiversTexts.Add (text);
//			}
//			else if (base.results[i]==2){
//				text = "Bonus 'Généreux'\nEsquive";
//				GameView.instance.displaySkillEffect(target, text, 4);
//				receiversTexts.Add (text);
//			}
//			else if (base.results[i]==4){
//				text = "Echec";
//				GameView.instance.displaySkillEffect(target, text, 4);
//				receiversTexts.Add (text);
//			}
//			else{
//				if(base.results[i]==3){
//					text = "Bonus Généreux\n";
//				}
//				else{
//					text = "";
//				}
//				text+="Effets dissipés";
//				
//				receiversTexts.Add (text);
//				GameView.instance.getCard(target).emptyModifiers();
//				if(target!=GameController.instance.getCurrentPlayingCard()){
//					GameView.instance.show(target, true);
//				}
//				else{
//					GameView.instance.show(target, false);
//				}
//				GameView.instance.displaySkillEffect(target, text, 5);
//			}	
//		}
//		GameView.instance.setSkillPopUp(" lance <b>Antibiotique</b>...", base.card, receivers, receiversTexts);	
//	}
//	
//	public override string isLaunchable()
//	{
//		return GameView.instance.canLaunchAllButMeModifiersTargets();
//	}
//	
//	public override string getTargetText(int id, Card targetCard){
//		
//		int amount = base.skill.proba;
//		int attack = base.card.GetAttack();
//		string text;
//		
//		text = "Dissipe les effets\n";
//		
//		int probaEsquive = targetCard.GetMagicalEsquive();
//		int probaHit = Mathf.Max(0,(amount*(100-probaEsquive)/100)) ;
//	
//		text += "HIT% : "+probaHit;
//		
//		return text ;
//	}
}
