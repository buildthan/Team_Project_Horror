using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

// 카메라에서 ray를 쏜다
public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // ray를 쏘는 시간간격
    private float lastCheckTime;    // 마지막 ray를 쏜 시각
    public float maxCheckDistance;  // 얼마나 멀리까지 체크할것인가
    public LayerMask layerMask;     // 어떤 layer가 달려있는 게임오브젝트를 검출할것인가

    /// <summary>
    /// ray로 검출된 게임오브젝트의 정보를 담는다
    /// </summary>
    public GameObject curInteractGameObject;    // 검출 성공했다면, interaction하는 게임오브젝트 정보를 저장
    private IInteractable curInteractable;      /// ★검출된 정보를 인터페이스로 캐싱한다 
    private IDamageable curDamageable;

    public int attackDamage = 10;

    public Weapon currentWeapon;

    // 검출한 아이템 정보를 promptText에 띄운다
    public TextMeshProUGUI promptText;  /// 일단 분리하지는 않지만, 개인과제할때는 UI를 분리해서 drag and drop 안하고 사용하는 방법을 찾아서 리팩토링 해봐라
    private Camera camera;  // 카메라

    void Start()
    {
        camera = Camera.main;
        Debug.Log("[Start] Interaction 초기화 완료");
    }

    // Update is called once per frame
    void Update()
    {
        // checkRate 간격으로 ray를 만들어야한다
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Debug.Log("[Raycast] 시도 중...");

            // 카메라에서 화면 중앙에 ray 발사
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));    // 카메라가 찍고 있는 방향이 있기 때문에, 시작점만 정해주면 된다
            RaycastHit hit; // ray에 부딪힌 게임오브젝트의 정보가 들어간다

            Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red, checkRate);

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) // ray에 검출된 오브젝트가 있다
            {

                Debug.Log("[Raycast] Hit 감지됨: " + hit.collider.name);
                if (hit.collider.gameObject != curInteractGameObject)   // ray에 충돌한 게임오브젝트가 현재 상호작용하는 게임오브젝트가 아니라면
                {

                    Debug.Log(hit.collider.gameObject);

                    Debug.Log(hit.collider.gameObject);
                    curInteractGameObject = hit.collider.gameObject;    // 새로운 정보로 바꿔


                    /// BaseItem에 붙어있는 것은 IInteractable이 아니라 BaseItem이므로
                    /// BaseItem으로 받아서 IInteractable으로 캐스팅한다
                    curInteractable = hit.collider.GetComponentInParent<BaseItem>() as IInteractable;


                    curDamageable = hit.collider.GetComponent<IDamageable>();   //공격가능한 대상을 캐싱
                    Debug.Log("curDamageable: " + curDamageable);
                    //SetPromptText();    // promptText에 출력해라
                }
            }
            else // 빈공간에 ray를 쏜 경우
            {
                Debug.Log("[Raycast] 아무것도 맞지 않음");
                //Debug.Log("아무것도 없습니다");
                // 모든 정보를 없애라
                curInteractGameObject = null;
                curInteractable = null;
                curDamageable = null;
                //promptText.gameObject.SetActive(false);
            }
        }
    }
    // promptText에 정보를 세팅
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);  // text를 활성화
        //promptText.text = curInteractable.GetInteractPrompt();  /// ★인터페이스에서 정보를 가져와 정보를 출력
    }
    // E키 눌렀을 때 상호작용
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        /// E를 눌렀을 때, aim이 아이템을 바라보고 있을 때(인터페이스로 캐싱하고 있는 정보가 있을 때)
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            //Debug.Log($"상호작용하는 오브젝트의 ID: {curInteractGameObject.GetInstanceID()}");

            curInteractable.OnInteract();   // 상호작용 끝나고 인벤토리로 이동한 아이템은 Destroy까지 해준다
            // 상호작용을 끝냈으니 모두 null, 비활성화
            curInteractGameObject = null;
            curInteractable = null;
            //promptText.gameObject.SetActive(false);
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("[Input] 우클릭 입력 감지됨");

            if (curDamageable != null && currentWeapon != null)
            {
                if (currentWeapon.CanFire())  // 연사 속도 체크
                {
                    int damage = currentWeapon.GetDamage();
                    Debug.Log($"[Attack] {damage} 데미지 전달 → 대상: {curDamageable}");
                    currentWeapon.Fire(); // 마지막 발사 시간 갱신
                    curDamageable.TakeDamage(damage);
                }
                else
                {
                    Debug.Log("[Attack] 쿨타임 중입니다. 발사 불가");
                }
            }
            else
            {
                Debug.Log("[Input] 공격 실패: 대상 또는 무기 없음");
            }
        }
    }
}