using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private float PUSH_INTERVAL = 0.1f;
    [SerializeField]
    private float PUSHDOWN_INTERVAL = 0.3f;
    [SerializeField]
    private float intervalTimer = 0.0f;
    [SerializeField]
    private float pushdownIntervalTimer = 0.0f;

    [SerializeField]
    private bool pushFlag = false;
    [SerializeField]
    private bool pushdownFlag = false;
    private bool leftMoveFlag = false;
    private bool rightMoveFlag = false;
    private bool leftRotateFlag = false;
    private bool rightRotateFlag = false;
    private bool hardDropFlag = false;
    private bool holdFlag = false;

    public bool LeftMoveFlag{
        get{
            var temp = leftMoveFlag;
            leftMoveFlag = false;
            return temp;
        }
    }

    public bool RightMoveFlag{
        get{
            var temp = rightMoveFlag;
            rightMoveFlag = false;
            return temp;
        }
    }

    public bool LeftRotateFlag{
        get{
            var temp = leftRotateFlag;
            leftRotateFlag = false;
            return temp;
        }
    }

    public bool RightRotateFlag{
        get{
            var temp = rightRotateFlag;
            rightRotateFlag = false;
            return temp;
        }
    }

    public bool HardDropFlag{
        get{
            var temp = hardDropFlag;
            hardDropFlag = false;
            return temp;
        }
    }

    public bool HoldFlag{
        get{
            var temp = holdFlag;
            holdFlag = false;
            return temp;
        }
    }
    void Update()
    {
        var deltaTime = Time.deltaTime;
        if(!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))){
            intervalTimer += deltaTime;
            pushdownIntervalTimer = 0.0f;
            if(intervalTimer >= PUSH_INTERVAL){
                pushFlag = true;
                intervalTimer = 0.0f;
            }
        }else{
            pushdownIntervalTimer += deltaTime;
            if(pushdownIntervalTimer >= PUSHDOWN_INTERVAL){
                pushdownFlag = true;
                pushdownIntervalTimer = 0.0f;
            }
        }
        if(pushFlag){
            if(Input.GetKeyDown(KeyCode.A)){
                leftMoveFlag = true;
                pushFlag = false;
            }
            else if(Input.GetKeyDown(KeyCode.D)){
                rightMoveFlag = true;
                pushFlag = false;
            }
            else if(Input.GetKeyDown(KeyCode.Q)){
                leftRotateFlag = true;
                pushFlag = false;
            }
            else if(Input.GetKeyDown(KeyCode.E)){
                rightRotateFlag = true;
                pushFlag = false;
            }
            else if(Input.GetKeyDown(KeyCode.S)){
                hardDropFlag = true;
                pushFlag = false;
            }
            else if(Input.GetKeyDown(KeyCode.W)){
                holdFlag = true;
                pushFlag = false;
            }            
        }
        else if(pushdownFlag){
            if(Input.GetKey(KeyCode.A)){
                leftMoveFlag = true;
                pushdownFlag = false;
            }
            else if(Input.GetKey(KeyCode.D)){
                rightMoveFlag = true;
                pushdownFlag = false;
            }
            else if(Input.GetKey(KeyCode.Q)){
                leftRotateFlag = true;
                pushdownFlag = false;
            }
            else if(Input.GetKey(KeyCode.E)){
                rightRotateFlag = true;
                pushdownFlag = false;
            }
            else if(Input.GetKey(KeyCode.S)){
                hardDropFlag = true;
                pushdownFlag = false;
            }
            else if(Input.GetKey(KeyCode.W)){
                holdFlag = true;
                pushdownFlag = false;
            }
        }
    }

    //すべてのFlagがfalseならtrueを返す
    private bool checkAllFlag(){
        return !(leftMoveFlag || rightMoveFlag || leftRotateFlag || rightRotateFlag || hardDropFlag);
    }
}
