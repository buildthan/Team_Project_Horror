using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{
    //public string GetInteractPrompt();  // 화면에 띄울 prompt 관련 함수
    public void OnInteract();   // 어떤 효과를 발생시킬것인가
}



public class BaseItem : MonoBehaviour, IInteractable
{
    /// <summary>
    /// 자식으로 형변환 하는거 생각하자
    /// 실제 오브젝트인 자식 오브젝트에 붙어있는 아이템종류DataBaseSO 스크립트를 통해서 
    /// BaseItemDataSO에 있는 변수들에 접근이 가능하니
    /// 최상위 부모에 BaseItemDataSO를 선언하는건 메모리 낭비가 될 수 있다
    /// 
    /// 예를들어 NormalBullet은 NormalBulletDataSO 변수를 가지고 있는데,
    /// NormalBulletDataSO 변수는 BaseItemDataSO의 상속을 받고 있다
    /// 그러니 실제 자료형을 이용해 부모의 변수에 접근하면 되니까
    /// 부모에 굳이 변수 선언할 필요가 없다
    /// </summary>
    //public BaseItemDataSO baseItemData;

    //public string name;

    // Start is called before the first frame update
    void Start()
    {
        //baseItemData.id = 0;
    }
    public void OnInteract()
    {
        // 인벤토리에 추가한다
        //Player 스크립트 먼저 수정
        //CharacterManager.Instance.Player.itemData = data;   // 플레이어의 itemData에 대입
        CharacterManager.Instance.Player.addItem?.Invoke(); // addItem에 구독되어있는 함수가 있으면 실행
        Destroy(gameObject);    // 인벤토리로 이동한 아이템은 씬에서 삭제
    }

}
