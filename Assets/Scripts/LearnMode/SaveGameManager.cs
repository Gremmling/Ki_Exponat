using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public class SaveGameManager: MonoBehaviour
{

	//Variables for the Save File name and the Path where to Save the File
	private static string fileName = "SaveAi.s";
	private static string path = Path.Combine(Application.dataPath, "Save");

	//Function to Save the data into the File
	public static void Save(SaveData saveD){
		BinaryFormatter formatter = new BinaryFormatter();
		//Folder missing = create Folder for Save File
		if(!Directory.Exists(path)){
			Directory.CreateDirectory(Path.Combine(path));
		}
		FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.OpenOrCreate, FileAccess.Write);
		formatter.Serialize(stream, saveD);
		stream.Close();
	}

	//Function to Load the Data
    public static SaveData Load(){
		//File Exists = get Data and save it into SaveData Object
		if(File.Exists(Path.Combine(path, fileName))){
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(Path.Combine(path, fileName), FileMode.Open);
			SaveData saveD = formatter.Deserialize(file) as SaveData;
			file.Close();
			Debug.Log("Array Loaded");
			return saveD;
		}
		//Create new Save file
		else{
			Debug.Log("No Saved Data");
			System.Console.WriteLine("No Save Data");
			return new SaveData();
		}
	}

	//Search for File if Exists delete File
	public static void ResetData(){
		if(File.Exists(Path.Combine(path, fileName))){
			File.Delete(Path.Combine(path, fileName));
			SaveData saveD = new SaveData();
		}
	}
}
