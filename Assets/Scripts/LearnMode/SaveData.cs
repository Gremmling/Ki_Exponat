using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{

	//Lists for every Cup,
	public static List<int> Numbers_Cup_10;
	public static List<int> Numbers_Cup_9;
	public static List<int> Numbers_Cup_8;
	public static List<int> Numbers_Cup_7;
	public static List<int> Numbers_Cup_6;
	public static List<int> Numbers_Cup_5;
	public static List<int> Numbers_Cup_4;
	public static List<int> Numbers_Cup_3;
	public static List<int> Numbers_Cup_2;

	//List for the Cup Lists
	public List<List<int>> RemainingCups;

	public SaveData(){
		Numbers_Cup_10 = new List<int> { 1, 2, 3 };
		Numbers_Cup_9 = new List<int> { 1, 2, 3 };
		Numbers_Cup_8 = new List<int> { 1, 2, 3 };
		Numbers_Cup_7 = new List<int> { 1, 2, 3 };
		Numbers_Cup_6 = new List<int> { 1, 2, 3 };
		Numbers_Cup_5 = new List<int> { 1, 2, 3 };
		Numbers_Cup_4 = new List<int> { 1, 2, 3 };
		Numbers_Cup_3 = new List<int> { 1, 2, 3 };
		Numbers_Cup_2 = new List<int> { 1, 2 };

		RemainingCups = new List<List<int>> {
				Numbers_Cup_2,
				Numbers_Cup_3,
				Numbers_Cup_4,
				Numbers_Cup_5,
				Numbers_Cup_6,
				Numbers_Cup_7,
				Numbers_Cup_8,
				Numbers_Cup_9,
				Numbers_Cup_10
			};
	}
}
