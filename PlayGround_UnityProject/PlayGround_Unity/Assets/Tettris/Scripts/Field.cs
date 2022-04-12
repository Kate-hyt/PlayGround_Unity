using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TettrisMino;
using static Mino;

public class Field : MonoBehaviour
{
    public static int GENERATE_POSITIONX = 4;
    public static int XSIZE = 10;
    public static int YSIZE = 22;
    private List<int> nextIDList = new List<int>();
    private List<TettrisMinoType> nextTettrisMinoList = new List<TettrisMinoType>();
    private int[,] fieldMinoID = new int[Field.XSIZE+1, Field.YSIZE+1];
    private Dictionary<int, Mino> MinoDict = new Dictionary<int, Mino>();

    public void Awake(){
        for(int i=1; i<= 194; i++){
            nextIDList.Add(i);
        }
    }

    public int[] nextID(){
        int[] ReturnList = {0, 0, 0, 0};
        nextIDList.Sort();
        for(int i=0; i<4; i++){
            ReturnList[i] = nextIDList[0];
            nextIDList.RemoveAt(0);
        }

        return ReturnList;
    }

    public List<TettrisMinoType> getNextTettrisMino(int range){
        var typeNumber = Enum.GetNames(typeof(TettrisMinoType)).Length;
        if(nextTettrisMinoList.Count < typeNumber){
            List<TettrisMinoType> tempList = new List<TettrisMinoType>();
            for(int i=0; i<typeNumber; i++){
                tempList.Add((TettrisMinoType)i);
            }
            //Guid.NewGuid()でランダムなバイト列を取得 それをkeyにソート
            tempList = tempList.OrderBy(x => Guid.NewGuid()).ToList();
            nextTettrisMinoList = nextTettrisMinoList.Concat(tempList).ToList();
        }
        return nextTettrisMinoList.GetRange(0, range).ToList();
    }
    public void proceedNextTettrisMino(){
        nextTettrisMinoList.RemoveAt(0);
    }

    public bool canCreate(TettrisMinoType type){
        bool result = true;
        List<int[]> GeneratePosition = TettrisMino.getGeneratePosition(type, GENERATE_POSITIONX);
        result = checkPosition(GeneratePosition);
        return result;
    }

    public int deleteLines(){
        List<int> rowList = new List<int>();
        int temp = 0;
        int deleteRowCount = 0;
        bool flag = false;

        for(int i=1; i<=YSIZE; i++){
            flag = false;
            for(int j=1; j<=XSIZE; j++){
                if(fieldMinoID[j, i] != 0){
                    flag = true;
                }else{
                    flag = false;
                    break;
                }
            }
            if(flag){
                for(int j=1; j<=XSIZE; j++){
                    temp = fieldMinoID[j, i];
                    fieldMinoID[j, i] = 0;
                    deleteMino(temp);
                }
                deleteRowCount += 1;
            }
            else{
                rowList.Add(i);
            }
        }
        while(rowList.Count < YSIZE){
            rowList.Insert(0, 0);
        }
        
        if(rowList[0] == 0){
            for(int i=YSIZE; i>0; i--){
                var rowNum = rowList[i-1];
                if(rowNum != 0 && i != rowNum){
                    for(int j=1; j<=XSIZE; j++){
                        if(fieldMinoID[j, rowNum] != 0){
                            Mino mino = MinoDict[fieldMinoID[j, rowNum]];
                            mino.PositionXID = j;
                            mino.PositionYID = i;
                            mino.PositionSet();
                            fieldMinoID[j, i] = mino.MinoID;
                            fieldMinoID[j, rowNum] = 0;
                        }
                    }
                }
                else if(rowNum == 0){
                    for(int j=1; j<=XSIZE; j++){
                        fieldMinoID[j, i] = 0;
                    }
                }
            }
        }

        return deleteRowCount;
    }

    private void deleteMino(int MinoID){
        Mino temp;
        if(MinoDict.ContainsKey(MinoID)){
            temp = MinoDict[MinoID];
            MinoDict.Remove(MinoID);
            nextIDList.Add(MinoID);
            temp.delete();
        }else{
            Debug.Log("Field.deleteMino : MinoID error");
        }
    }
    public void addMinos(List<Mino> Minos){
        for(int i=0; i<Minos.Count; i++){
            addMino(Minos[i]);
        }
    }
    private void addMino(Mino Mino){
        Mino.PositionSet();
        int x = Mino.PositionXID;
        int y = Mino.PositionYID;
        int MinoID = Mino.MinoID;
        
        fieldMinoID[x, y] = MinoID;
        Mino.transform.parent = this.transform;
        MinoDict[MinoID] = Mino;
    }
    public bool checkPosition(List<int[]> PositionList){
        bool result = true;
        for(int i=0; i<PositionList.Count; i++){
            result = result && checkPosition(PositionList[i][0], PositionList[i][1]);
        }
        return result;
    }
    private bool checkPosition(int PositionXID, int PositionYID){
        bool flag = false;
        if(PositionXID < 1 || PositionXID > Field.XSIZE){
            flag = false;
        }else if(PositionYID < 1 || PositionYID > Field.YSIZE){
            flag = false;
        }else{
            if(fieldMinoID[PositionXID, PositionYID] == 0){
                flag = true;
            }else{
                flag = false;
            }
        }
        return flag;
    }

}
