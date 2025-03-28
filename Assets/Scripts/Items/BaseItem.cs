using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public BaseItemDataSO baseItemData;



    //public string name;

    // Start is called before the first frame update
    void Start()
    {
        baseItemData.id = 0;
    }

}
