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
		valueSlider = 0;
		_slider.onValueChanged.AddListener((v) =>
		{
			double finalValue = v * 5.882352941176471;
			if(v == 0){
				_sliderText.text = v.ToString();
			}
			else if(v == 17){
				_sliderText.text = finalValue.ToString();
			}
			else{
				//_sliderText.text = finalValue.ToString("#.0000");
				_sliderText.text = finalValue.ToString("#.0");
			}
			valueSlider = (int)v;
		});
	}

    // Update is called once per frame
    void Update()
    {

    }
}
