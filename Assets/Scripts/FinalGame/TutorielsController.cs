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
	}
}