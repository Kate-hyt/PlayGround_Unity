using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TettrisMino : MonoBehaviour
{
    //mino の相対位置を持つ
    public enum TettrisMinoType{
        I,
        O,
        Z,
        S,
        J,
        L,
        T
    }
    [SerializeField]
    private List<Mino> Minos = new List<Mino>(){};
    private static List<int[]>[,] TETTRISMINO_FORM = new List<int[]>[,]
    {
        //I
        {
            // dir : 0
            new List<int[]>(){new int[2]{0, 1}, new int[2]{1, 1}, new int[2]{2, 1}, new int[2]{3, 1}},
            // dir : 1
            new List<int[]>(){new int[2]{2, 0}, new int[2]{2, 1}, new int[2]{2, 2}, new int[2]{2, 3}},
            // dir : 2
            new List<int[]>(){new int[2]{3, 2}, new int[2]{2, 2}, new int[2]{1, 2}, new int[2]{0, 2}},
            // dir : 3
            new List<int[]>(){new int[2]{1, 3}, new int[2]{1, 2}, new int[2]{1, 1}, new int[2]{1, 0}}, 
        },
        //O
        {
            // dir : 0
            new List<int[]>(){new int[2]{1, 1}, new int[2]{2, 1}, new int[2]{1, 2}, new int[2]{2, 2}},
            // dir : 1
            new List<int[]>(){new int[2]{2, 1}, new int[2]{2, 2}, new int[2]{1, 1}, new int[2]{1, 2}},
            // dir : 2
            new List<int[]>(){new int[2]{2, 2}, new int[2]{1, 2}, new int[2]{2, 1}, new int[2]{1, 1}},
            // dir : 3
            new List<int[]>(){new int[2]{1, 2}, new int[2]{1, 1}, new int[2]{2, 2}, new int[2]{2, 1}}, 
        },
        //Z
        {
            // dir : 0
            new List<int[]>(){new int[2]{0, 1}, new int[2]{1, 1}, new int[2]{1, 2}, new int[2]{2, 2}},
            // dir : 1
            new List<int[]>(){new int[2]{2, 1}, new int[2]{2, 2}, new int[2]{1, 2}, new int[2]{1, 3}},
            // dir : 2
            new List<int[]>(){new int[2]{2, 3}, new int[2]{1, 3}, new int[2]{1, 2}, new int[2]{0, 2}},
            // dir : 3
            new List<int[]>(){new int[2]{0, 3}, new int[2]{0, 2}, new int[2]{1, 2}, new int[2]{1, 1}}, 
        },
        //S
        {
            // dir : 0
            new List<int[]>(){new int[2]{1, 1}, new int[2]{2, 1}, new int[2]{0, 2}, new int[2]{1, 2}},
            // dir : 1
            new List<int[]>(){new int[2]{2, 2}, new int[2]{2, 3}, new int[2]{1, 1}, new int[2]{1, 2}},
            // dir : 2
            new List<int[]>(){new int[2]{1, 3}, new int[2]{0, 3}, new int[2]{2, 2}, new int[2]{1, 2}},
            // dir : 3
            new List<int[]>(){new int[2]{0, 2}, new int[2]{0, 1}, new int[2]{1, 3}, new int[2]{1, 2}}, 
        },
        //J
        {
            // dir : 0
            new List<int[]>(){new int[2]{0, 1}, new int[2]{0, 2}, new int[2]{1, 2}, new int[2]{2, 2}},
            // dir : 1
            new List<int[]>(){new int[2]{2, 1}, new int[2]{1, 1}, new int[2]{1, 2}, new int[2]{1, 3}},
            // dir : 2
            new List<int[]>(){new int[2]{2, 3}, new int[2]{2, 2}, new int[2]{1, 2}, new int[2]{0, 2}},
            // dir : 3
            new List<int[]>(){new int[2]{0, 3}, new int[2]{1, 3}, new int[2]{1, 2}, new int[2]{1, 1}},
        },
        //L
        {
            // dir : 0
            new List<int[]>(){new int[2]{2, 1}, new int[2]{0, 2}, new int[2]{1, 2}, new int[2]{2, 2}},
            // dir : 1
            new List<int[]>(){new int[2]{2, 3}, new int[2]{1, 1}, new int[2]{1, 2}, new int[2]{1, 3}},
            // dir : 2
            new List<int[]>(){new int[2]{0, 3}, new int[2]{2, 2}, new int[2]{1, 2}, new int[2]{0, 2}},
            // dir : 3
            new List<int[]>(){new int[2]{0, 1}, new int[2]{1, 3}, new int[2]{1, 2}, new int[2]{1, 1}},
        },
        //T
        {
            // dir : 0
            new List<int[]>(){new int[2]{1, 1}, new int[2]{0, 2}, new int[2]{1, 2}, new int[2]{2, 2}},
            // dir : 1
            new List<int[]>(){new int[2]{2, 2}, new int[2]{1, 1}, new int[2]{1, 2}, new int[2]{1, 3}},
            // dir : 2
            new List<int[]>(){new int[2]{1, 3}, new int[2]{2, 2}, new int[2]{1, 2}, new int[2]{0, 2}},
            // dir : 3
            new List<int[]>(){new int[2]{0, 2}, new int[2]{1, 3}, new int[2]{1, 2}, new int[2]{1, 1}},
        }
    };
    private TettrisMinoType type = TettrisMino.TettrisMinoType.I;
    public TettrisMinoType Type{
        get{
            return type;
        }
    }
    private int dir = 0;
    public int Dir{
        get{
            return dir;
        }
    }
    private int positionXID = 3;
    private int positionYID = 1;
    public int PositionXID{
        get{
            return positionXID;
        }
    }

    public int PositionYID{
        get{
            return positionYID;
        }
    }
    
    public async void init(int[] IDList, int Px, TettrisMinoType type){
        //Minoの生成
        Minos.Clear();
        var handle = Addressables.LoadAssetAsync<GameObject>("Mino.prefab");
        await handle.Task;
        for(int i=0; i<TETTRISMINO_FORM[(int)type, dir].Count; i++){
            var temp = Instantiate(handle.Result, new Vector3(0, 0, 0), Quaternion.identity);
            temp.transform.parent = this.gameObject.transform;
            Minos.Add(temp.GetComponent<Mino>());
        }

        positionXID = Px;
        //形
        this.type = type;
        //向き
        this.dir = 0;
        if(type == TettrisMinoType.I){
            positionYID = 1;
        }
        else{
            positionYID = 0;
        }
        for(int i=0; i<Minos.Count; i++){
            Minos[i].init(IDList[i], positionXID + TETTRISMINO_FORM[(int)type, dir][i][0], positionYID + TETTRISMINO_FORM[(int)type, dir][i][1], (Mino.COLOR)(int)type);
        }
        
        this.gameObject.SetActive(true);
    }

    public bool checkPreparation(){
        return Minos.Count == TETTRISMINO_FORM[(int)type, dir].Count;
    }
    public static List<int[]> getGeneratePosition(TettrisMinoType type,int positionXID){
        List<int[]> temp = TETTRISMINO_FORM[(int)type, 0];
        List<int[]> MinoPosition = new List<int[]>();
        
        for(int i=0; i<temp.Count; i++){
            if(type != TettrisMinoType.I){
                MinoPosition.Add(new int[2]{temp[i][0]+positionXID, temp[i][1]+2});
            }
            else{
                MinoPosition.Add(new int[2]{temp[i][0]+positionXID, temp[i][1]+3});
            }
        }

        return MinoPosition;
    }
    public List<int[]> getFallPosition(){
        List<int[]> NowPosition = new List<int[]>();
        List<int[]> FallPosition = new List<int[]>();

        for(int i=0; i<Minos.Count; i++){
            NowPosition.Add(new int[2]{Minos[i].PositionXID, Minos[i].PositionYID});
        }

        for(int i=0; i<Minos.Count; i++){
            var temp1 = Minos[i].PositionXID;
            var temp2 = Minos[i].PositionYID+1;
            bool flag = false;
            for(int j=0; j<Minos.Count; j++){
                if(temp1 == NowPosition[j][0] && temp2 == NowPosition[j][1]){
                    flag = false;
                    break;
                }else{
                    flag = true;
                }
            }
            if(flag){
                FallPosition.Add(new int[2]{temp1, temp2});
            }
        }

        return FallPosition;
    }

    public List<int[]> getMovePosition(int dx, int dy){
        List<int[]> NowPosition = new List<int[]>();
        List<int[]> MovePosition = new List<int[]>();

        for(int i=0; i<Minos.Count; i++){
            NowPosition.Add(new int[2]{Minos[i].PositionXID, Minos[i].PositionYID});
        }
        for(int i=0; i<Minos.Count; i++){
            var temp1 = Minos[i].PositionXID+dx;
            var temp2 = Minos[i].PositionYID+dy;
            bool flag = false;
            for(int j=0; j<Minos.Count; j++){
                if(temp1 == NowPosition[j][0] && temp2 == NowPosition[j][1]){
                    flag = false;
                    break;
                }else{
                    flag = true;
                }
            }
            if(flag){
                MovePosition.Add(new int[2]{temp1, temp2});
            }
        }

        return MovePosition;
    }

    public List<int[]> getSuperRotationPosition(int dr, int pattern){
        List<int[]> SuperRotationPosition = new List<int[]>();
        int dx = 0;
        int dy = 0;
        var tempDir = dir;
        if(dr == 1){
            tempDir += 1;
            if(tempDir > 3){
                tempDir = 0;
            }
        }
        else if(dr == -1){
            tempDir -= 1;
            if(tempDir < 0){
                tempDir = 3;
            }
        }

        if(type == TettrisMinoType.O){
            for(int j=0; j<Minos.Count; j++){
                SuperRotationPosition.Add(new int[2]{positionXID + TETTRISMINO_FORM[(int)type, tempDir][j][0], positionYID + TETTRISMINO_FORM[(int)type, tempDir][j][1]});
            }
        }
        else if(type == TettrisMinoType.I){
            dx = 0;
            dy = 0;
            for(int i=0; i<=pattern; i++){
                if(i == 0){
                    for(int j=0; j<Minos.Count; j++){
                        SuperRotationPosition.Add(new int[2]{positionXID + TETTRISMINO_FORM[(int)type, tempDir][j][0], positionYID + TETTRISMINO_FORM[(int)type, tempDir][j][1]});
                    }
                }
                else if(i == 1){
                    if(tempDir == 0){
                        if(dir == 1){
                            dx += 2;
                        }
                        else{
                            dx += 1;
                        }
                    }
                    else if(tempDir == 1){
                        if(dir == 0){
                            dx -= 2;
                        }
                        else{
                            dx += 1;
                        }
                    }
                    else if(tempDir == 2){
                        if(dir == 1){
                            dx -= 1;
                        }
                        else{
                            dx -= 2;
                        }
                    }
                    else{
                        if(dir == 0){
                            dx -= 1;
                        }
                        else{
                            dx += 2;
                        }
                    }
                }
                else if(i == 2){
                    if(tempDir == 0){
                        dx -= 3;
                    }
                    else if(tempDir == 1 || tempDir == 3){
                        if(dir == 0){
                            dx += 3;
                        }
                        else{
                            dx -= 3;
                        }
                    }
                    else{
                        dx += 3;
                    }
                }
                else if(i == 3){
                    dx = 0;
                    dy = 0;
                    if(tempDir == 0){
                        if(dir == 1){
                            dx += 2;
                            dy -= 1;
                        }
                        else{
                            dx += 1;
                            dy += 2;
                        }
                    }
                    else if(tempDir == 1){
                        if(dir == 0){
                            dx -= 2;
                            dy += 1;
                        }
                        else{
                            dx += 1;
                            dy += 2;
                        }
                    }
                    else if(tempDir == 2){
                        if(dir == 1){
                            dx -= 1;
                            dy -= 2;
                        }
                        else{
                            dx -= 2;
                            dy += 1;
                        }
                    }
                    else{
                        if(dir == 0){
                            dx -= 1;
                            dy -= 2;
                        }
                        else{
                            dx += 2;
                            dy -= 1;
                        }
                    }
                }
                else if(i == 4){
                    if(tempDir == 0){
                        dx -= 3;
                    }
                    else if(tempDir == 1 || tempDir == 3){
                        if(dir == 0){
                            dx += 3;
                        }
                        else{
                            dx -= 3;
                        }
                    }
                    else{
                        dx += 3;
                    }
                    if(dir == 0 || dir == 2){
                        if(tempDir == 1){
                            dy -= 3;
                        }
                        else{
                            dy += 3;
                        }
                    }
                    else if(dir == 1){
                        dy += 3;
                    }
                    else{
                        dy -= 3;
                    }
                }
            }
        }
        else{
            dx = 0;
            dy = 0;
            for(int i=0; i<=pattern; i++){
                if(i == 0){
                    for(int j=0; j<Minos.Count; j++){
                        SuperRotationPosition.Add(new int[2]{positionXID + TETTRISMINO_FORM[(int)type, tempDir][j][0], positionYID + TETTRISMINO_FORM[(int)type, tempDir][j][1]});
                    }
                }
                else if(i == 1){
                    if(tempDir == 0){
                        if(dir == 1){
                            dx += 1;
                        }
                        else{
                            dx -= 1;
                        }
                    }
                    else if(tempDir == 1){
                        dx -= 1;
                    }
                    else if(tempDir == 2){
                        if(dir == 1){
                            dx += 1;
                        }
                        else{
                            dx -= 1;
                        }
                    }
                    else{
                        dx += 1;
                    }
                }
                else if(i == 2){
                    if(tempDir == 0 || tempDir == 2){
                        dy += 1;
                    }
                    else{
                        dy -= 1;
                    }
                }
                else if(i == 3){
                    dx = 0;
                    dy = 0;
                    if(tempDir == 0 || tempDir == 2){
                        dy -= 2;
                    }
                    else{
                        dy += 2;
                    }
                }
                else if(i == 4){
                    if(tempDir == 0){
                        if(dir == 1){
                            dx += 1;
                        }
                        else{
                            dx -= 1;
                        }
                    }
                    else if(tempDir == 1){
                        dx -= 1;
                    }
                    else if(tempDir == 2){
                        if(dir == 1){
                            dx += 1;
                        }
                        else{
                            dx -= 1;
                        }
                    }
                    else{
                        dx += 1;
                    }
                }
            }
        }

        for(int i=0; i<Minos.Count; i++){
            var temp1 = SuperRotationPosition[i][0] + dx;
            var temp2 = SuperRotationPosition[i][1] + dy;
            SuperRotationPosition[i] = new int[2]{temp1, temp2};
        }

        return SuperRotationPosition;
    }
    public TettrisMinoType getHoldMinoType(){
        var temp = type;
        return temp;
    }
    public List<Mino> getHoldMino(){
        var temp = new List<Mino>(Minos);
        return temp;
    }
    public void Fall(){
        positionYID += 1;
        AdjustMinos();
    }

    public List<Mino> LockDown(){
        return Minos;
    }
    public void Move(int dx, int dy){
        positionXID += dx;
        positionYID += dy;
        AdjustMinos();
    }

    public void SuperRotation(int dr, int pattern){
        int dx = 0;
        int dy = 0;
        var tempDir = dir;
        if(dr == 1){
            tempDir += 1;
            if(tempDir > 3){
                tempDir = 0;
            }
        }
        else if(dr == -1){
            tempDir -= 1;
            if(tempDir < 0){
                tempDir = 3;
            }
        }

        if(type == TettrisMinoType.I){
            dx = 0;
            dy = 0;
            for(int i=1; i<=pattern; i++){
                if(i == 1){
                    if(tempDir == 0){
                        if(dir == 1){
                            dx += 2;
                        }
                        else{
                            dx += 1;
                        }
                    }
                    else if(tempDir == 1){
                        if(dir == 0){
                            dx -= 2;
                        }
                        else{
                            dx += 1;
                        }
                    }
                    else if(tempDir == 2){
                        if(dir == 1){
                            dx -= 1;
                        }
                        else{
                            dx -= 2;
                        }
                    }
                    else{
                        if(dir == 0){
                            dx -= 1;
                        }
                        else{
                            dx += 2;
                        }
                    }
                }
                else if(i == 2){
                    if(tempDir == 0){
                        dx -= 3;
                    }
                    else if(tempDir == 1 || tempDir == 3){
                        if(dir == 0){
                            dx += 3;
                        }
                        else{
                            dx -= 3;
                        }
                    }
                    else{
                        dx += 3;
                    }
                }
                else if(i == 3){
                    dx = 0;
                    dy = 0;
                    if(tempDir == 0){
                        if(dir == 1){
                            dx += 2;
                            dy -= 1;
                        }
                        else{
                            dx += 1;
                            dy += 2;
                        }
                    }
                    else if(tempDir == 1){
                        if(dir == 0){
                            dx -= 2;
                            dy += 1;
                        }
                        else{
                            dx += 1;
                            dy += 2;
                        }
                    }
                    else if(tempDir == 2){
                        if(dir == 1){
                            dx -= 1;
                            dy -= 2;
                        }
                        else{
                            dx -= 2;
                            dy += 1;
                        }
                    }
                    else{
                        if(dir == 0){
                            dx -= 1;
                            dy -= 2;
                        }
                        else{
                            dx += 2;
                            dy -= 1;
                        }
                    }
                }
                else if(i == 4){
                    if(tempDir == 0){
                        dx -= 3;
                    }
                    else if(tempDir == 1 || tempDir == 3){
                        if(dir == 0){
                            dx += 3;
                        }
                        else{
                            dx -= 3;
                        }
                    }
                    else{
                        dx += 3;
                    }
                    if(dir == 0 || dir == 2){
                        if(tempDir == 1){
                            dy -= 3;
                        }
                        else{
                            dy += 3;
                        }
                    }
                    else if(dir == 1){
                        dy += 3;
                    }
                    else{
                        dy -= 3;
                    }
                }
            }
        }
        else{
            dx = 0;
            dy = 0;
            for(int i=1; i<=pattern; i++){
                if(i == 1){
                    if(tempDir == 0){
                        if(dir == 1){
                            dx += 1;
                        }
                        else{
                            dx -= 1;
                        }
                    }
                    else if(tempDir == 1){
                        dx -= 1;
                    }
                    else if(tempDir == 2){
                        if(dir == 1){
                            dx += 1;
                        }
                        else{
                            dx -= 1;
                        }
                    }
                    else{
                        dx += 1;
                    }
                }
                else if(i == 2){
                    if(tempDir == 0 || tempDir == 2){
                        dy += 1;
                    }
                    else{
                        dy -= 1;
                    }
                }
                else if(i == 3){
                    dx = 0;
                    dy = 0;
                    if(tempDir == 0 || tempDir == 2){
                        dy -= 2;
                    }
                    else{
                        dy += 2;
                    }
                }
                else if(i == 4){
                    if(tempDir == 0){
                        if(dir == 1){
                            dx += 1;
                        }
                        else{
                            dx -= 1;
                        }
                    }
                    else if(tempDir == 1){
                        dx -= 1;
                    }
                    else if(tempDir == 2){
                        if(dir == 1){
                            dx += 1;
                        }
                        else{
                            dx -= 1;
                        }
                    }
                    else{
                        dx += 1;
                    }
                }
            }
        }

        positionXID += dx;
        positionYID += dy;
        dir = tempDir;
        AdjustMinos();
    }
    
    public void createHoldMino(TettrisMinoType type, int Px, List<Mino> Minos){
        this.type = type;
        dir = 0;
        this.Minos = Minos;
        positionXID = Px;
        if(type == TettrisMinoType.I){
            positionYID = 1;
        }
        else{
            positionYID = 0;
        }
        for(int i=0; i<Minos.Count; i++){
            Minos[i].transform.gameObject.SetActive(true);
            Minos[i].transform.parent = this.transform;
        }
        AdjustMinos();
    }
    private void AdjustMinos(){
        for(int i=0; i < Minos.Count; i++){
            Minos[i].PositionXID = positionXID + TETTRISMINO_FORM[(int)type, dir][i][0];
            Minos[i].PositionYID = positionYID + TETTRISMINO_FORM[(int)type, dir][i][1];
        }
    }
}