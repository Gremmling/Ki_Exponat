using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonHover : MonoBehaviour
{

	public Button BTN_Button;
	public Sprite whileHover;
	public Sprite bevorHover;

	public Boolean smaller;


	public Vector2 whileHoverVector;
	public Vector2 bevorHoverVector;

    void OnMouseEnter(){
		BTN_Button.image.sprite = whileHover;
    	if(smaller){
			BTN_Button.GetComponent<RectTransform>().sizeDelta = whileHoverVector;
			BTN_Button.GetComponent<BoxCollider>().size = whileHoverVector;
		}
	}

    void OnMouseExit(){
		BTN_Button.image.sprite = bevorHover;
    if(smaller){
    	BTN_Button.GetComponent<RectTransform>().sizeDelta = bevorHoverVector;
		BTN_Button.GetComponent<BoxCollider>().size = bevorHoverVector;
    }
    }
}
