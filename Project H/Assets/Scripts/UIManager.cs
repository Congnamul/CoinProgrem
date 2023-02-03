using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider timeSlider;
    public Text timeText;



    // Start is called before the first frame update
    void Start()
    {
        //timeSlider
    
    
    }

    // Update is called once per frame
    void Update()
    {

        timeSlider.value = GameManager.Instance.nowTime;
        switch ((int)GameManager.Instance.nowTime)
        {
            case 0:
                timeText.text = "»õº®";
                break;
            case 120:
                timeText.text = "¾ÆÄ§";
                break;
            case 240:
                timeText.text = "Á¡½É";
                break;
            case 360:
                timeText.text = "Àú³á";
                break;
            case 480:
                timeText.text = "¹ã";
                break;
        }

    }



}
