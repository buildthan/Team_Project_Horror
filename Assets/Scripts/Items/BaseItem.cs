using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{
    public void OnInteract();   // 어떤 효과를 발생시킬것인가
}


public abstract class BaseItem : MonoBehaviour, IInteractable
{
    /// <summary>
    /// 자식으로 형변환 하는거 생각하자
    /// 실제 오브젝트인 자식 오브젝트에 붙어있는 아이템종류DataBaseSO 스크립트를 통해서 
    /// BaseItemDataSO에 있는 변수들에 접근이 가능하니
    /// 최상위 부모에 BaseItemDataSO를 선언하는건 메모리 낭비가 될 수 있다
    /// </summary>
    //public BaseItemDataSO baseItemData;

    /// 추상 메서드를 만들어서 자식 클래스에서 실제 데이터를 반환하도록 강제하는 방식으로 해결한다
    /// 자식클래스에서 추상메서드를 구현하고, 부모클래스로 형변환하여 리턴하면 부모클래스에서 멤버변수 만들지 않아도 된다 
    public abstract BaseItemDataSO GetItemData();   /// 자식 클래스의 데이터를 부모 타입으로 가져온다

    public void OnInteract()
    {
        // 인벤토리에 추가한다
        /// 문제는 BaseItem스크립트에는 BaseItemDataSO를 선언하지 않을 것이기 때문이다
        /// 왜냐하면 BaseItemDataSO를 상속받은 자식의 오브젝트가 생성되기 때문이다
        /// 자식의 오브젝트에서는 부모의 자료형(선언형식)인 BaseItemDataSO의 변수에 접근할 수 있기 때문에
        /// BaseItem에 BaseItemDataSO를 선언하는건, 같은 변수를 2번 선언하는 것이니 변수를 선언하지는 않는다
        /// 대신에
        /// 자식(실형식)의 자료형을 알아서 그 자료형을 통해서 부모의 데이터를 가져와서 대입하는 방법을 쓴다

        /// 추상메서드를 사용하여 해결한다 
        /// 자식클래스에서 추상메서드를 구현하면서 부모클래스로 형변환하여 리턴하면 부모클래스에서 멤버변수 만들지 않아도 된다 
        /// 자식 클래스의 데이터를 부모 타입으로 가져옴
        BaseItemDataSO data = GetItemData();    
        CharacterManager.Instance.Player.BaseItemData = data;   // 플레이어의 itemData에 대입
        CharacterManager.Instance.Player.addItem?.Invoke(this); // addItem에 구독되어있는 함수가 있으면 실행      
    }
}
