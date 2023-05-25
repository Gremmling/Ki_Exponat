using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour
{

	[SerializeField] private Slider _slider;
	[SerializeField] private TextMeshProUGUI _sliderText;
	public static int valueSlider;
	// Start is called before the first frame update
	void Start()
    {
		_slider.onValueChanged.AddListener((v) =>
		{
			_sliderText.text = v.ToString("0");
			valueSlider = (int)v;
		});
	}

    // Update is called once per frame
    void Update()
    {

    }
}
