using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TspinPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tspinText;

    [SerializeField]
    public static float DELETE_TIME = 5.0f;
    private float showTime = 0.0f;
    private bool showFlag = false;
    void Update(){
        if(showFlag){
            var dt = Time.deltaTime;
            showTime += dt;
            if(showTime >= DELETE_TIME){
                tspinText.text = "";
                showFlag = false;
            }
        }
    }
    public void show(Manager.TspinType type, int lineCount){
        if(tspinText != null){
            if(type == Manager.TspinType.None){
                tspinText.text = "";
            }
            else if(type == Manager.TspinType.TspinMini){
                tspinText.text = "Tspin \n Mini";
                showTime = 0.0f;
                showFlag = true;
            }
            else if(type == Manager.TspinType.Tspin){
                if(lineCount == 1){
                    tspinText.text = "Tspin\n Single";
                    showTime = 0.0f;
                    showFlag = true;
                }
                else if(lineCount == 2){
                    tspinText.text = "Tspin\n Double!";
                    showTime = 0.0f;
                    showFlag = true;
                }
                else if(lineCount == 3){
                    tspinText.text = "Tspin\n Tripple!!!";
                    showTime = 0.0f;
                    showFlag = true;
                }
                else{
                    tspinText.text = "";
                    showTime = 0.0f;
                    showFlag = true;
                }
            }
        }
    }
}
