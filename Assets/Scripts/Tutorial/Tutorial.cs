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
	private int countSteps = 0;

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
