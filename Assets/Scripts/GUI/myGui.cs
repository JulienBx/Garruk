using UnityEngine;

public static class MyGUI {
	
	#region Implementation
	
	private static GUIStyle backgroundStyle = "Box";
	private static GUIStyle thumbStyle = "Box";
	private static GUIStyle minThumbStyle = "Button";
	private static GUIStyle maxThumbStyle = "Button";
	
	private static void DoMinMaxSlider(Rect position, ref float minValue, ref float maxValue, float minLimit, float maxLimit) {
		int controlID = GUIUtility.GetControlID(FocusType.Passive);
		int minThumbControlID = GUIUtility.GetControlID(FocusType.Passive);
		int maxThumbControlID = GUIUtility.GetControlID(FocusType.Passive);
		
		// Do not proceed for layout event.
		if (Event.current.type == EventType.Layout)
			return;
		
		// Clamp current state of values.
		minValue = Mathf.Clamp(minValue, minLimit, maxLimit);
		maxValue = Mathf.Max(minValue, Mathf.Clamp(maxValue, minLimit, maxLimit));
		
		// Calculate normalized version of values.
		float range = Mathf.Abs(maxLimit - minLimit);
		float normalizedMinValue = (minValue - minLimit) / range;
		float normalizedMaxValue = (maxValue - minLimit) / range;
		
		// Calculate visual version of values.
		float minValueX = position.x + normalizedMinValue * position.width;
		float maxValueX = position.x + normalizedMaxValue * position.width;
		
		Rect minThumbPosition = new Rect(minValueX, position.y, 7, position.height);
		Rect maxThumbPosition = new Rect(maxValueX-7, position.y, 7, position.height);
		
		float normalizedMousePosition;
		
		switch (Event.current.GetTypeForControl(controlID)) {
		case EventType.MouseDown:
			// Mouse pressed down on minimum thumb position?
			if (minThumbPosition.Contains(Event.current.mousePosition)) {
				GUIUtility.hotControl = minThumbControlID;
				Event.current.Use();
			}
			// Mouse pressed down on maximum thumb position?
			else if (maxThumbPosition.Contains(Event.current.mousePosition)) {
				GUIUtility.hotControl = maxThumbControlID;
				Event.current.Use();
			}
			break;
			
		case EventType.MouseUp:
			// Extremely important!!
			if (GUIUtility.hotControl == minThumbControlID || GUIUtility.hotControl == maxThumbControlID || GUIUtility.hotControl == controlID)
				GUIUtility.hotControl = 0;
			break;
			
		case EventType.MouseDrag:
			normalizedMousePosition = (Event.current.mousePosition.x - position.x) / position.width;
			
			// Process mouse movement?
			if (GUIUtility.hotControl == minThumbControlID) {
				float newMinValue = Mathf.Clamp(normalizedMousePosition * range + minLimit, minLimit, maxValue);
				if (newMinValue != minValue) {
					minValue = newMinValue;
					GUI.changed = true;
				}
				Event.current.Use();
			}
			else if (GUIUtility.hotControl == maxThumbControlID) {
				float newMaxValue = Mathf.Clamp(normalizedMousePosition * range + minLimit, minValue, maxLimit);
				if (newMaxValue != maxValue) {
					maxValue = newMaxValue;
					GUI.changed = true;
				}
				Event.current.Use();
			}
			break;
			
		case EventType.Repaint:
			// Draw background of slider control.
			backgroundStyle.Draw(new Rect(position.x, position.y + 5, position.width, position.height - 10), GUIContent.none, controlID);
			// Draw background of thumb range.
			thumbStyle.Draw(new Rect(minValueX, position.y + 9, maxValueX - minValueX, 5), GUIContent.none, false, false, false, false);
			// Draw minimum thumb button.
			minThumbStyle.Draw(minThumbPosition, GUIContent.none, minThumbControlID);
			// Draw maximum thumb button.
			maxThumbStyle.Draw(maxThumbPosition, GUIContent.none, maxThumbControlID);
			break;
		}
	}
	
	#endregion
	
	#region Absolute Version
	
	public static void MinMaxSlider(Rect position, ref float minValue, ref float maxValue, float minLimit, float maxLimit) {
		DoMinMaxSlider(position, ref minValue, ref maxValue, minLimit, maxLimit);
	}
	
	#endregion
	
	#region Layout-enabled Version
	
	public static void MinMaxSlider(ref float minValue, ref float maxValue, float minLimit, float maxLimit) {
		Rect position = GUILayoutUtility.GetRect(GUIContent.none, backgroundStyle);
		DoMinMaxSlider(position, ref minValue, ref maxValue, minLimit, maxLimit);
	}
	
	#endregion
	
}
