using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using static TettrisMino;


public class HoldPanel : MonoBehaviour
{
    private Dictionary<TettrisMinoType, AsyncOperationHandle<Sprite>> minoPanelDist = new Dictionary<TettrisMinoType, AsyncOperationHandle<Sprite>>();
    private bool isHoldMino = false;
    private TettrisMinoType holdMino;
    [SerializeField]
    private GameObject image;

    public TettrisMinoType HoldMino{
        set{
            holdMino = value;
            isHoldMino = true;
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

        var tempImage = image.GetComponent<Image>();
        if(tempImage == null){
            tempImage = image.AddComponent<Image>();
        }
    }

    public void show(){
        var tempImage = image.GetComponent<Image>();
        if(isHoldMino == true){
            Sprite temp = Instantiate(minoPanelDist[holdMino].Result, new Vector3(0, 0, 0), Quaternion.identity);
            tempImage.sprite = temp;
        }
    }
}
