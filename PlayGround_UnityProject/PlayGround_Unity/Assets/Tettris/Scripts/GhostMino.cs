using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static Mino;

public class GhostMino : MonoBehaviour
{
    private int[] positionID = new int[2] {1, 1};

    [SerializeField]
    public COLOR color = Mino.COLOR.Skyblue;
    private GameObject minoGameObject;
    
    //左上が(1, 1)
    //右下が(10, 25)

    public int PositionXID{
        set{
            if(value > 0 && value <= Field.XSIZE){
                this.positionID[0] = value;
            }else{
                Debug.Log("MinoのPositionXIDが間違えています");
                if(value <= 0){
                    this.positionID[0] = 1;
                }else if(value > Field.XSIZE){
                    this.positionID[0] = Field.XSIZE;
                }
            }
        }
        get{
            return this.positionID[0];
        }
    }
    public int PositionYID{
        set{
            if(value > 0 && value <= Field.YSIZE){
                this.positionID[1] = value;
            }else{
                Debug.Log("MinoのPositionYIDが間違えています");
                if(value <= 0){
                    this.positionID[0] = 1;
                }else if(value > Field.YSIZE){
                    this.positionID[0] = Field.YSIZE;
                }
            }
        }
        get{
            return this.positionID[1];
        }
    }
    public COLOR Color{
        set{
            if(value == Mino.COLOR.Skyblue){
                color = Mino.COLOR.Skyblue;
            }
            else if(value == Mino.COLOR.Yellow){
                color = Mino.COLOR.Yellow;
            }
            else if(value == Mino.COLOR.Green){
                color = Mino.COLOR.Green;
            }
            else if(value == Mino.COLOR.Red){
                color = Mino.COLOR.Red;
            }
            else if(value == Mino.COLOR.Blue){
                color = Mino.COLOR.Blue;
            }
            else if(value == Mino.COLOR.Orange){
                color = Mino.COLOR.Orange;
            }
            else if(value == Mino.COLOR.Purple){
                color = Mino.COLOR.Purple;
            }
            else{
                color = Mino.COLOR.Skyblue;
            }
            MaterialSet();
        }
    }
    public void init(int px, int py, Mino.COLOR color){
        PositionXID = px;
        PositionYID = py;
        Color = color;
        PositionSet();
        MaterialSet();
        this.gameObject.SetActive(true);
    }

     public void PositionSet(){
        float PositionX_Default = -27.5f;
        float PositionY_Default = 57.5f;

        if(minoGameObject == null){
            minoGameObject = this.gameObject;
        }
        minoGameObject.transform.position = new Vector3(PositionX_Default + 5.0f * PositionXID, PositionY_Default - 5.0f * PositionYID, 0);
    }

    public void MaterialSet(){
        if(minoGameObject == null){
            minoGameObject = this.gameObject;
        }

        byte alpha = 255 - 100;
        if(this.color == Mino.COLOR.Skyblue){
//            Debug.Log("水色");
            Addressables.LoadAssetAsync<Material>("SkyblueMino.mat").Completed += m =>
            {
                minoGameObject.GetComponent<Renderer>().material = m.Result;
                minoGameObject.GetComponent<Renderer>().material.color = minoGameObject.GetComponent<Renderer>().material.color - new Color32(0, 0, 0, alpha);
            };
        }else if(this.color == Mino.COLOR.Yellow){
//            Debug.Log("黄色");
            Addressables.LoadAssetAsync<Material>("YellowMino.mat").Completed += m =>
            {
                minoGameObject.GetComponent<Renderer>().material = m.Result;
                minoGameObject.GetComponent<Renderer>().material.color = minoGameObject.GetComponent<Renderer>().material.color - new Color32(0, 0, 0, alpha);
            };
        }else if(this.color == Mino.COLOR.Green){
//            Debug.Log("緑色");
            Addressables.LoadAssetAsync<Material>("GreenMino.mat").Completed += m =>
            {
                minoGameObject.GetComponent<Renderer>().material = m.Result;
                minoGameObject.GetComponent<Renderer>().material.color = minoGameObject.GetComponent<Renderer>().material.color - new Color32(0, 0, 0, alpha);
            };
        }else if(this.color == Mino.COLOR.Red){
//            Debug.Log("赤色");
            Addressables.LoadAssetAsync<Material>("RedMino.mat").Completed += m =>
            {
                minoGameObject.GetComponent<Renderer>().material = m.Result;
                minoGameObject.GetComponent<Renderer>().material.color = minoGameObject.GetComponent<Renderer>().material.color - new Color32(0, 0, 0, alpha);
            };
        }else if(this.color == Mino.COLOR.Blue){
//            Debug.Log("青色");
            Addressables.LoadAssetAsync<Material>("BlueMino.mat").Completed += m =>
            {
                minoGameObject.GetComponent<Renderer>().material = m.Result;
                minoGameObject.GetComponent<Renderer>().material.color = minoGameObject.GetComponent<Renderer>().material.color - new Color32(0, 0, 0, alpha);
            };
        }else if(this.color == Mino.COLOR.Orange){
//            Debug.Log("橙色");
            Addressables.LoadAssetAsync<Material>("OrangeMino.mat").Completed += m =>
            {
                minoGameObject.GetComponent<Renderer>().material = m.Result;
                minoGameObject.GetComponent<Renderer>().material.color = minoGameObject.GetComponent<Renderer>().material.color - new Color32(0, 0, 0, alpha);
            };
        }else if(this.color == Mino.COLOR.Purple){
//            Debug.Log("紫色");
            Addressables.LoadAssetAsync<Material>("PurpleMino.mat").Completed += m =>
            {
                minoGameObject.GetComponent<Renderer>().material = m.Result;
                minoGameObject.GetComponent<Renderer>().material.color = minoGameObject.GetComponent<Renderer>().material.color - new Color32(0, 0, 0, alpha);
            };
        }
    }

    public void delete(){
        Destroy(this.gameObject);
        Destroy(this);
    }
}
