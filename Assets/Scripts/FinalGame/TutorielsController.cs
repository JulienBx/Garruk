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
		NewGameController.instance.getTilesController().showAllColliders(true);
		this.step++;
		if(this.step==1){
			NewGameController.instance.loadPreGameDestinations();
		}
		if(this.step==2){
			NewGameController.instance.showStartButton();
		}
	}

	public void launchNextTutorial(){
		float scale = Mathf.Min(1f,NewGameController.instance.getScreenDimensions().getRealWidth()/6.05f);
		if(this.step==0){
			this.getTutorielController().setPosition(new Vector3(0f, scale, 0f));
			this.getTutorielController().setTexts(WordingGame.getText(45), WordingGame.getText(46), WordingGame.getText(142));
		}
		else if(this.step==1){
			this.getTutorielController().setPosition(new Vector3(0f, -0.6f*scale, 0f));
			this.getTutorielController().setTexts(WordingGame.getText(47), WordingGame.getText(48), WordingGame.getText(142));
		}
		NewGameController.instance.getTilesController().showAllColliders(false);
		this.getTutorielController().show(true);
	}
}