using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static Mino;
using static TettrisMino;

public class GhostTettrisMino : MonoBehaviour
{
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
    public List<GhostMino> Minos = new List<GhostMino>();
    private TettrisMinoType type = TettrisMino.TettrisMinoType.I;
    private int dir = 0;
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
    
    public async void init(TettrisMinoType type){
        for(int i=0; i<Minos.Count; i++){
            Minos[i].delete();
        }
        Minos.Clear();
        positionXID = 1;
        positionYID = 1;
        //形
        this.type = type;
        //向き
        this.dir = 0;
        while(Minos.Count < TETTRISMINO_FORM[(int)type, dir].Count){
            var handle = Addressables.LoadAssetAsync<GameObject>("GhostMino.prefab");
            await handle.Task;
            for(int i=0; i<TETTRISMINO_FORM[(int)type, dir].Count; i++){
                var temp = Instantiate(handle.Result, new Vector3(0, 0, 0), Quaternion.identity);
                temp.transform.parent = this.gameObject.transform;
                Minos.Add(temp.GetComponent<GhostMino>());
                Minos[i].init(positionXID + TETTRISMINO_FORM[(int)type, dir][i][0], positionYID + TETTRISMINO_FORM[(int)type, dir][i][1], (COLOR)(int)type);
            }
        }
    }

    public void set(int Px, int Py, int dr){
        positionXID = Px;
        positionYID = Py;
        this.dir = dr;
        AdjustMinos();
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

    public void Fall(){
        positionYID += 1;
        AdjustMinos();
    }

    public void LockDown(){
        for(int i=0; i<Minos.Count; i++){
            Minos[i].delete();
        }
        Minos.Clear();
    }
    private void AdjustMinos(){
        for(int i=0; i < Minos.Count; i++){
            Minos[i].PositionXID = positionXID + TETTRISMINO_FORM[(int)type, dir][i][0];
            Minos[i].PositionYID = positionYID + TETTRISMINO_FORM[(int)type, dir][i][1];
            Minos[i].PositionSet();
        }
    }
}
