using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingDates
{
	public static string[] dateFormat;

	public static string getDateFormat()
	{
		return dateFormat[ApplicationModel.idLanguage];
	}
	static WordingDates()
	{
		dateFormat = new string[]{"dd/MM/yyyy","MM/dd/yyyy"};
	}
}