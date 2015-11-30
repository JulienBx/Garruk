using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class ScrollingController : MonoBehaviour 
{

	public float contentHeight;
	public float viewHeight;
	public float startPositionY;
	public float endPositionY;
	public float currentPositionY;

	public void setViewHeight(float viewHeight)
	{
		this.viewHeight = viewHeight;
	}
	public float getViewHeight()
	{
		return viewHeight;
	}
	public void setContentHeight(float contentHeight)
	{
		this.contentHeight = contentHeight;
	}
	public float getContentHeight()
	{
		return contentHeight;
	}
	public void setStartPositionY(float startPositionY)
	{
		this.startPositionY = startPositionY;
		this.currentPositionY = this.startPositionY;
		this.endPositionY = startPositionY - (contentHeight - viewHeight) ;
	}
	public float getStartPositionY()
	{
		return this.startPositionY;
	}
	public void reset()
	{
		this.setStartPositionY (this.startPositionY);
		Vector3 cameraPosition = gameObject.transform.position;
		cameraPosition.y = this.currentPositionY;
		gameObject.transform.position = cameraPosition;
	}
	public bool isEndPosition()
	{
		if(this.currentPositionY==this.endPositionY)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public bool isStartPosition()
	{
		if(this.currentPositionY==this.startPositionY)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool ScrollController()
	{
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if(scroll !=0)
		{
			float step = scroll * 0.5f;
			this.setCameraPosition(step);
			return true;
		}
		if (Input.touchCount == 1) 
		{
			Touch touch = Input.touches[0];
			if (touch.phase == TouchPhase.Moved)
			{
				float step = -touch.deltaPosition.y*0.005f;
				this.setCameraPosition(step);
				return true;
			}
		}
		return false;
	}


	private void setCameraPosition(float delta)
	{
		Vector3 cameraPosition = gameObject.transform.position;
		currentPositionY=cameraPosition.y;
		if(delta>0 && currentPositionY<startPositionY)
		{
			if(currentPositionY+delta<startPositionY)
			{
				currentPositionY=currentPositionY+delta;
			}
			else
			{
				currentPositionY=startPositionY;
			}
			cameraPosition.y=currentPositionY;
			gameObject.transform.position=cameraPosition;
		}
		else if(delta<0 && currentPositionY>endPositionY)
		{
			if(currentPositionY+delta>endPositionY)
			{
				currentPositionY=currentPositionY+delta;
			}
			else
			{
				currentPositionY=endPositionY;
			}
			cameraPosition.y=currentPositionY;
			gameObject.transform.position=cameraPosition;
		}
	}


//	public float contentOverflowY = 0f;
//	public float contentHeight = 0f;
//	public float viewHeight = 0f;
//	public float inertiaDuration = 0.95f;
//	
//	private Vector3 scrollPosition;
//	private float scrollVelocity = 0f;
//	private float timeTouchPhaseEnded = 0f;
//	private float elasticSpringBackTime = 0.5f;
//	static private float SCROLL_RESISTANCE_COEFFICIENT = 8f;
//	
//	public void setViewHeight(float viewHeight)
//	{
//		this.viewHeight = viewHeight;
//	}
//	public void setContentHeight(float contentHeight)
//	{
//		this.contentHeight = contentHeight;
//	}
//	private void setupBounds(){
//		
//		// contentOverflowY is calculated by subtracting the viewheight from the content height.
//		// this leaves us with the amount we need to be able to move the view.
//		if (contentOverflowY == 0f) {
//			contentOverflowY = viewHeight - contentHeight;
//		}
//		
//		// Finally, if our overflow is less than the viewHeight,
//		// we get strange scrolling behavior, since it snaps the
//		// bottom of the content to the bottom of the view.
//		if (contentOverflowY > 0) {
//			contentOverflowY = 0;
//		}
//	}
//	public void Reset() 
//	{
//		viewHeight = 0f;
//		contentHeight = 0f;
//		contentOverflowY = 0f;
//	}
//	
//	public bool ScrollController(){
//		
//		bool scrolling = false;
//		this.setupBounds ();
//		
//		scrollPosition = this.gameObject.transform.position;
//		
//		// Support the computer scrollwheel
//		float scroll = - Input.GetAxis("Mouse ScrollWheel");
//		if (0f != scroll) {
//			scrollPosition = this.gameObject.transform.position;
//			scrollPosition -= Vector3.down * scroll * 400f;
//
//			SpringToEdge();
//			//this.gameObject.transform.position = scrollPosition;
//			
//			// We are scrolling with the mouse scrollwheel,
//			// so we don't need to execute any more code here.
//			return true;
//		}
//		
//		if (Input.touchCount < 1) {
//			if ( scrollVelocity != 0.0f ) {
//				// slow down over time
//				float t = (Time.time - timeTouchPhaseEnded) / inertiaDuration;
//				float frameVelocity = Mathf.Lerp(scrollVelocity, 0f, t);
//				scrollPosition.y -= (frameVelocity * Time.deltaTime);
//				
//				// We finish after the time is up.
//				if (t >= inertiaDuration) scrollVelocity = 0.0f;
//				
//				SpringToEdge();
//				//this.gameObject.transform.position = scrollPosition;
//			}
//			return false;
//		}
//		
//		Touch touch = Input.touches[0];
//		float drag;
//		if (touch.phase == TouchPhase.Began){
//			scrollVelocity = 0.0f;
//			scrolling = false;
//		} else if (touch.phase == TouchPhase.Moved) {
//			float velocity;
//			// Figure out where we are starting from: 
//			drag = scrollPosition.y;
//			
//			// Add scroll resistance if we are past the bounds of the screen
//			// AKA elastic scrolling - doesn't work quite right.
//			// With micro-movements of the fingers, we get to a place where we
//			// start working our way back to the start.
//			if(scrollPosition.y < 0)
//			{
//				drag -= drag/SCROLL_RESISTANCE_COEFFICIENT;
//			}
//			else if(scrollPosition.y > contentOverflowY)
//			{
//				drag -= SCROLL_RESISTANCE_COEFFICIENT;
//
//			}
//				
//			drag += touch.deltaPosition.y;
//			scrollPosition.y = drag;
//
//			
//			
//			// Impart momentum to the scrolled view, using the velocity of the finger
//			// to compute the momentum of the scroll.
//			velocity = (touch.deltaPosition.y / touch.deltaTime);
//			
//			// Moving average filter to get rid of wild variations...
//			scrollVelocity = 0.8f * velocity + 0.2f * scrollVelocity;
//			
//			// If we move past a certain threshold, then we are scrolling
//			// otherwise, we are possibly trying to tap.  This allows us
//			// to detect when we are just trying to push a button.
//			scrolling = (Mathf.Abs(touch.deltaPosition.y) >= 3.0f) ? true : false;
//	
//		} else if (touch.phase == TouchPhase.Ended) {
//			
//			// track the time we ended so we can compute when to stop the scroll.
//			timeTouchPhaseEnded = Time.time;
//			
//			// Add the elastic bounce to the end if we're past bounds.
//			SpringToEdge();
//			scrolling = false;
//		}
//		
//		this.gameObject.transform.position = scrollPosition;
//		
//		return scrolling;
//	}
//	
//	private void SpringToEdge() {
//		TweenParms parms = new TweenParms();
//		
//		if(scrollPosition.y < 0) {
//			scrollVelocity = 0f;
//			parms.Prop("position", new Vector3(scrollPosition.x, 0, scrollPosition.z));
//			parms.Ease(EaseType.EaseOutCubic); // Easing type
//			HOTween.To(this.gameObject.transform, elasticSpringBackTime, parms);
//			return;
//		}
//		
//		if(scrollPosition.y > contentOverflowY) {
//			scrollVelocity = 0f;
//			parms.Prop("position", new Vector3(scrollPosition.x, contentOverflowY,scrollPosition.z));
//			parms.Ease(EaseType.EaseOutCubic); // Easing type
//			HOTween.To(this.gameObject.transform, elasticSpringBackTime, parms);
//			return;
//		}
//	}
}

