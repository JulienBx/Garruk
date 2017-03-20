using System;
using UnityEngine;

public class TutorielsController
{
	GameObject tutorielController ;
	int step = 0;

	public TutorielsController (GameObject g)
	{
		this.tutorielController = g;
		this.step = 0;
	}

	public TutorielController getTutorielController(){
		return this.tutorielController.GetComponent<TutorielController>();
	}

	public void hitTutorial(){
		this.getTutorielController().show(false);
		this.step++;
		if(this.step==1){
			NewGameController.instance.getTilesController().loadPreGameDestinations(NewGameController.instance.isFirstPlayer());
		}
	}

	public void launchNextTutorial(){
		if(this.step==0){
			this.getTutorielController().setTexts(WordingGame.getText(45), WordingGame.getText(46), WordingGame.getText(142));
			this.getTutorielController().show(true);
		}
	}
}