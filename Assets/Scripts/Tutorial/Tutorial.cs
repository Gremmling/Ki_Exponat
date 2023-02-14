using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

public class Tutorial : MonoBehaviour
{

	//Buttons for the Tutorial
	public Button BTN_Exit;
    public Button BTN_Left;
	public Button BTN_Right;

	//Arrays for every Text and Image
	public String[] AllTexts;
	public Sprite[] AllSprites;

	//Objects to manage the Sprites and Textes
	public TextMeshProUGUI tutorialText;
	public Image ImageForTutorial;

	//Counter to go right or left in the Array
	private static int countSteps = 0;

	//enum for a step to the right or left
	private enum Direction
	{
		Right = 0,
		Left = 1
	};

	// Start is called before the first frame update
	void Start()
    {
		//add on Click Function to Buttons
		BTN_Exit.onClick.AddListener(() => CloseTutorial());
		BTN_Right.onClick.AddListener(() => ChangeTutorial(Direction.Right));
        BTN_Left.onClick.AddListener(() => ChangeTutorial(Direction.Left));
		//reset Counter
		countSteps = 0;
		//call Function to set first text and Img
		SetTutorial();


	}

	//set first img and text
    private void SetTutorial(){
		string adjustedText = SplitString(AllTexts[0]);
		tutorialText.text = adjustedText;
		ImageForTutorial.sprite = AllSprites[0];
	}

	//set the new text and img in the right Direction
	private void ChangeTutorial(Direction direction){
		// if(direction == Direction.Right){

		//     if(countSteps == AllTexts.Length - 1)//if the counter is on the last position reset to start from step 0
		// 		countSteps = 0;
		// 	else
		// 		countSteps++;
		// 	string adjustedText = SplitString(AllTexts[countSteps]);
		// 	tutorialText.text = adjustedText;
		// 	ImageForTutorial.sprite = AllSprites[countSteps];
		// }
		// else if(direction == Direction.Left){
		//     if(countSteps == 0)//if the counter is on the first position set counter to last position to go left
		// 		countSteps = AllTexts.Length - 1;
		// 	else
		// 		countSteps--;
		// 	string adjustedText = SplitString(AllTexts[countSteps]);
		// 	tutorialText.text = adjustedText;
		// 	ImageForTutorial.sprite = AllSprites[countSteps];
		// }
		if(direction == Direction.Left && countSteps == 0)
			countSteps = AllTexts.Length;
		countSteps = (countSteps + (direction == Direction.Right ? 1 : -1)) % AllTexts.Length;
		tutorialText.text = SplitString(AllTexts[countSteps]);
		ImageForTutorial.sprite = AllSprites[countSteps];

    }

	private string SplitString(string text){
		string finalText = "";
		string[] lines = text.Split('.');
		for (int i = 0; i < lines.Length; i++){
			if(i != lines.Length - 1)
				finalText += lines[i] + "." + "\n";
		}
		Debug.Log(finalText);
		return finalText;
	}

	private void CloseTutorial(){
        SceneManager.LoadScene("MainMenu");
    }
}
