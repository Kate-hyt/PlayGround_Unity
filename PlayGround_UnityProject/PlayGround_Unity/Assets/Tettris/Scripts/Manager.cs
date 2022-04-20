using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static TettrisMino;
using static Mino;
using static Field;

public class Manager : MonoBehaviour
{
    public float startTimer = 5;
    private enum GameState{
        GameStart,
        Prepare,
        Playing,
        GameOver,
    }

    private GameState gameState = GameState.GameStart;
    [SerializeField]
    private Controller controller;
    [SerializeField]
    private Field field;
    [SerializeField]
    private TettrisMino tettrisMino;
    [SerializeField]
    private GhostTettrisMino ghostTettrisMino;
    //落下関係
    private List<int[]> FallPosition = new List<int[]>();
    private bool fallFlag = true;
    //ミノが下にある状態でテトリミノが落ちるまでに許される操作回数
    private int LockDownCount = 0;
    //落ちたテトリミノの数
    private int MinoCount = 0;
    //Timer
    private float deltaTime = 0;
    private static float FALLINTERVAL_START = 0.5f;
    private float FALLINTERVAL = 0.5f;
    private static float LOCKDOWNINTERVAL = 0.5f;
    private float gameTimer = 0;
    private float fallIntervalTimer = 0;
    private float lockDownTimer = 0;
    //操作時のFLAG
    private bool leftMoveFlag = false;
    private bool rightMoveFlag = false;
    private bool leftRotateFlag = false;
    private bool rightRotateFlag = false;
    private bool hardDropFlag = false;
    private bool holdFlag = false;

    //Holdできるかどうか
    private bool canHoldFlag = true;
    [SerializeField]
    private TettrisMinoType holdType;
    [SerializeField]
    private List<Mino> holdMinos = new List<Mino>();

    //Tspin
    public enum TspinType{
        None,
        TspinMini,
        Tspin,
    }

    private TspinType tspinType = TspinType.None;
    //UI
    [SerializeField]
    private HoldPanel holdPanel;
    [SerializeField]
    private NextPanels nextPanels;
    [SerializeField]
    private TspinPanel tspinPanel;

    void Update(){
        deltaTime = Time.deltaTime;
        gameTimer += deltaTime;

        if(gameState == GameState.GameStart){
            startTimer -= deltaTime;
            if(startTimer < 0){
                createTettrisMino(decisionMinoType());
                gameState = GameState.Prepare;
            }
        }
        else if(gameState == GameState.Prepare){
            if(tettrisMino.checkPreparation()){
                FallPosition = tettrisMino.getFallPosition();
                fallFlag = field.checkPosition(FallPosition);
                LockDownCount = 0;
                gameState = GameState.Playing;
            }
        }
        else if(gameState == GameState.Playing){
            leftMoveFlag = controller.LeftMoveFlag;
            rightMoveFlag = controller.RightMoveFlag;
            leftRotateFlag = controller.LeftRotateFlag;
            rightRotateFlag = controller.RightRotateFlag;
            hardDropFlag = controller.HardDropFlag;
            holdFlag = controller.HoldFlag;
            fallFlag = field.checkPosition(FallPosition);
            setHardDropPosition();

            if(fallFlag){
                fallIntervalTimer += deltaTime;
                if(fallIntervalTimer >= FALLINTERVAL){
                    Fall();
                    fallIntervalTimer = 0.0f;
                    lockDownTimer = 0.0f;
                }
                else if(leftMoveFlag){
                    Move(-1, 0);
                }
                else if(rightMoveFlag){
                    Move(1, 0);
                }
                else if(leftRotateFlag){
                    SuperRotation(-1);
                }
                else if(rightRotateFlag){
                    SuperRotation(1);
                }
                else if(hardDropFlag){
                    HardDrop();
                    fallIntervalTimer = 0.0f;
                    lockDownTimer = 0.0f;
                    LockDownCount = 0;
                    createTettrisMino(decisionMinoType());
                    gameState = GameState.Prepare;
                }
                else if(holdFlag){
                    if(canHoldFlag){
                        Hold();
                        fallIntervalTimer = 0.0f;
                        lockDownTimer = 0.0f;
                        LockDownCount = 0;
                        gameState = GameState.Prepare;
                    }
                }
            }
            else{
                lockDownTimer += deltaTime;
                if(lockDownTimer >= LOCKDOWNINTERVAL){
                    LockDown();
                    fallIntervalTimer = 0.0f;
                    lockDownTimer = 0.0f;
                    LockDownCount = 0;
                    createTettrisMino(decisionMinoType());
                    gameState = GameState.Prepare;
                }
                else if(leftMoveFlag){
                    Move(-1, 0);
                    if(LockDownCount <= 15){
                        lockDownTimer = 0.0f;
                        LockDownCount += 1;
                    }
                }
                else if(rightMoveFlag){
                    Move(1, 0);
                    if(LockDownCount <= 15){
                        lockDownTimer = 0.0f;
                        LockDownCount += 1;
                    }                
                }
                else if(leftRotateFlag){
                    SuperRotation(-1);
                    if(LockDownCount <= 15){
                        lockDownTimer = 0.0f;
                        LockDownCount += 1;
                    }
                }
                else if(rightRotateFlag){
                    SuperRotation(1);
                    if(LockDownCount <= 15){
                        lockDownTimer = 0.0f;
                        LockDownCount += 1;
                    }
                }
                else if(hardDropFlag){
                    HardDrop();
                    fallIntervalTimer = 0.0f;
                    lockDownTimer = 0.0f;
                    LockDownCount = 0;
                    createTettrisMino(decisionMinoType());
                    gameState = GameState.Prepare;
                }
                else if(holdFlag){
                    if(canHoldFlag){
                        Hold();
                        fallIntervalTimer = 0.0f;
                        lockDownTimer = 0.0f;
                        LockDownCount = 0;
                        gameState = GameState.Prepare;
                    }
                }
            }
        }
    }
    private TettrisMinoType decisionMinoType(){
        return field.getNextTettrisMino(1)[0];
    }
    private void createTettrisMino(TettrisMinoType type){
        if(field.canCreate(type)){
            MinoCount += 1;
            setFallInterval(MinoCount);
            tettrisMino.init(field.nextID(), Field.GENERATE_POSITIONX, type);
            tettrisMino.gameObject.SetActive(true);
            field.proceedNextTettrisMino();
            ghostTettrisMino.init(type);
            ghostTettrisMino.gameObject.SetActive(true);
            nextPanels.NextMinos = field.getNextTettrisMino(6);
            nextPanels.show();
        }else{
            GameOver();
        }
    }

    private void Fall(){
        tettrisMino.Fall();
        FallPosition = tettrisMino.getFallPosition();
        tspinType = TspinType.None;
    }

    private void LockDown(){
        field.addMinos(tettrisMino.LockDown());
        ghostTettrisMino.LockDown();
        var lineCount = field.deleteLines();
        if(tspinPanel != null){
            tspinPanel.show(tspinType, lineCount);
            Debug.Log("?");
        }
        canHoldFlag = true;
    }

    private void Move(int dx, int dy){
        List<int[]> temp = tettrisMino.getMovePosition(dx, dy);
        bool flag = field.checkPosition(temp);
        if(flag){
//            Debug.Log("Move!");
            tettrisMino.Move(dx, dy);
        }
        FallPosition = tettrisMino.getFallPosition();
        tspinType = TspinType.None;
    }

    private void SuperRotation(int dr){
        int pattern = 0;
        int positionXID = 0;
        int positionYID = 0;
        int dir = 0;
        
        for(int i=0; i<5; i++){
            List<int[]> temp = tettrisMino.getSuperRotationPosition(dr, i);
            bool flag = field.checkPosition(temp);
            if(flag){
                tettrisMino.SuperRotation(dr, i);
                if(tettrisMino.Type == TettrisMinoType.T){
                    pattern = i;
                    positionXID = tettrisMino.PositionXID;
                    positionYID = tettrisMino.PositionYID;
                    dir = tettrisMino.Dir;
                }
                break;
            }
        }
        FallPosition = tettrisMino.getFallPosition();
        if(tettrisMino.Type == TettrisMinoType.T && pattern > 0){
            tspinType = checkTspin(positionXID, positionYID, dir, pattern);
        }
        else{
            tspinType = TspinType.None;
        }
    }
    private TspinType checkTspin(int px, int py, int dir, int pattern){
        TspinType resultType = TspinType.None;
        var temp = tettrisMino.getTspinCheckPosition(dir);
        int[] pa = new int[2]{temp[0][0] + px, temp[0][1] + py};
        int[] pb = new int[2]{temp[1][0] + px, temp[1][1] + py};
        int[] pc = new int[2]{temp[2][0] + px, temp[2][1] + py};
        int[] pd = new int[2]{temp[3][0] + px, temp[3][1] + py};
        int count = 0;
        bool tspinMiniFlag = false;

        if((0 < pa[0] && pa[0] <= Field.XSIZE) && (0 < pa[1] && pa[1] <= Field.YSIZE)){
            if(field.checkPosition(new List<int[]>{pa})){
                tspinMiniFlag = true;
            }
            else{
                count += 1;
            }
        }
        else{
            //壁か床
            count += 1;
        }
        if((0 < pb[0] && pb[0] <= Field.XSIZE) && (0 < pb[1] && pb[1] <= Field.YSIZE)){
            if(field.checkPosition(new List<int[]>{pb})){
                tspinMiniFlag = true;
            }
            else{
                count += 1;
            }
        }
        else{
            //壁か床
            count += 1;
        }
        //背面
        if((0 < pc[0] && pc[0] <= Field.XSIZE) && (0 < pc[1] && pc[1] <= Field.YSIZE)){
            if(!field.checkPosition(new List<int[]>{pc})){
                count += 1;
            }
        }
        else{
            //壁か床
            count += 1;
        }
        if((0 < pd[0] && pd[0] <= Field.XSIZE) && (0 < pd[1] && pd[1] <= Field.YSIZE)){
            if(!field.checkPosition(new List<int[]>{pd})){
                count += 1;
            }
        }
        else{
            //壁か床
            count += 1;
        }

        if(count >= 3){
            resultType = TspinType.Tspin;
            if(tspinMiniFlag && pattern != 4){
                resultType = TspinType.TspinMini;
            }
        }

        return resultType;
    }
    private void setHardDropPosition(){
        ghostTettrisMino.set(tettrisMino.PositionXID, tettrisMino.PositionYID, tettrisMino.Dir);
        var temp = ghostTettrisMino.getFallPosition();
        while(field.checkPosition(temp)){
            ghostTettrisMino.Fall();
            temp = ghostTettrisMino.getFallPosition();
        }
    }
    
    private void HardDrop(){
        Move(0, ghostTettrisMino.PositionYID - tettrisMino.PositionYID);
        LockDown();
    }
    private void Hold(){
        if(holdMinos.Count == 0){
            holdType = tettrisMino.getHoldMinoType();
            holdMinos = tettrisMino.getHoldMino();
            for(int i=0; i<holdMinos.Count; i++){
                holdMinos[i].transform.parent = this.transform;
                holdMinos[i].transform.gameObject.SetActive(false);
            }
            createTettrisMino(decisionMinoType());
        }
        else{
            var temp1 = holdType;
            var temp2 = holdMinos;
            holdType = tettrisMino.getHoldMinoType();
            holdMinos = tettrisMino.getHoldMino();
            for(int i=0; i<holdMinos.Count; i++){
                holdMinos[i].transform.parent = this.transform;
                holdMinos[i].transform.gameObject.SetActive(false);
            }
            tettrisMino.createHoldMino(temp1, Field.GENERATE_POSITIONX, temp2);
            ghostTettrisMino.init(temp1);
        }

        if(holdPanel != null){
            holdPanel.HoldMino = holdType;
            holdPanel.show();
        }
        else{
            Debug.Log("HoldPanelが設定されていません。");
        }
        canHoldFlag = false;
        tspinType = TspinType.None;
    }
    private void setFallInterval(int count){
        int temp = 0;
        if(count <= 5){
            temp = 0;
        }else if(count <= 10){
            temp = 1;
        }else if(count <= 25){
            temp = 2;
        }else if(count <= 50){
            temp = 3;
        }else{
            temp = 4;
        }
        FALLINTERVAL = FALLINTERVAL_START - FALLINTERVAL_START / 5 * temp;
    }

    private void GameOver(){
        gameState = GameState.GameOver;
        Debug.Log("GameOver!");
    }
}
