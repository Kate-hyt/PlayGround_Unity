using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using static TettrisMino;

public class NextPanels : MonoBehaviour
{
    private Dictionary<TettrisMinoType, AsyncOperationHandle<Sprite>> minoPanelDist = new Dictionary<TettrisMinoType, AsyncOperationHandle<Sprite>>();

    private List<TettrisMinoType> nextMinos = new List<TettrisMinoType>();

    [SerializeField]
    private List<GameObject> images = new List<GameObject>();

    public List<TettrisMinoType> NextMinos{
        set{
            if(value.Count == images.Count){
                nextMinos = value;
            }
            else{
                Debug.Log("nextの数があっていません");
            }
        }
    }
    
    async void Start(){
        var tempList = Enum.GetValues(typeof(TettrisMinoType)); 
        foreach(var temp in tempList){
            var tempType = (TettrisMinoType)temp;
            if(tempType == TettrisMinoType.I){
                var handle = Addressables.LoadAssetAsync<Sprite>("IminoPanel");
                await handle.Task;
                minoPanelDist[tempType] = handle;
            }
            else if(tempType == TettrisMinoType.O){
                var handle = Addressables.LoadAssetAsync<Sprite>("OminoPanel");
                await handle.Task;
                minoPanelDist[tempType] = handle;
            }
            else if(tempType == TettrisMinoType.Z){
                var handle = Addressables.LoadAssetAsync<Sprite>("ZminoPanel");
                await handle.Task;
                minoPanelDist[tempType] = handle;
            }
            else if(tempType == TettrisMinoType.S){
                var handle = Addressables.LoadAssetAsync<Sprite>("SminoPanel");
                await handle.Task;
                minoPanelDist[tempType] = handle;
            }
            else if(tempType == TettrisMinoType.J){
                var handle = Addressables.LoadAssetAsync<Sprite>("JminoPanel");
                await handle.Task;
                minoPanelDist[tempType] = handle;
            }
            else if(tempType == TettrisMinoType.L){
                var handle = Addressables.LoadAssetAsync<Sprite>("LminoPanel");
                await handle.Task;
                minoPanelDist[tempType] = handle;
            }
            else if(tempType == TettrisMinoType.T){
                var handle = Addressables.LoadAssetAsync<Sprite>("TminoPanel");
                await handle.Task;
                minoPanelDist[tempType] = handle;
            }          
            else{
                Debug.Log("HoldPanelの設定がありません");
                Debug.Log("Type" + tempType);
            }
        }

        for(int i=0; i<images.Count; i++){
            var tempImage = images[i].GetComponent<Image>();
            if(tempImage == null){
                images[i].AddComponent<Image>();
            }
        }
    }

    public void show(){
        for(int i=0; i<images.Count; i++){
            var tempImage = images[i].GetComponent<Image>();
            Sprite temp = Instantiate(minoPanelDist[nextMinos[i]].Result, new Vector3(0, 0, 0), Quaternion.identity);
            tempImage.sprite = temp;
        }
    }
}
