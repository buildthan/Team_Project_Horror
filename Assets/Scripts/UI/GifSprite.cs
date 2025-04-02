using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GifSprite : MonoBehaviour
{

    //GIF효과를 주기 위한 이미지의 각 프레임들의 스프라이트가 들어가는 공간
    [SerializeField] private Sprite[] mSprites;
    //GIF효과에서 다음 이미지까지의 딜레이 시간을 설정
    [SerializeField] private float mImageDelay;
    //다음 이미지까지 남은 딜레이 시간
    private float mCurrentDelay;
    //현재 mSprites에서 현재 이미지의 인덱스 번호
    private int mCurrentImageID;

    //게임 오브젝트의 스프라이트 렌더러 컴포넌트
    private SpriteRenderer mSpriteComp;
    //게임 오브젝트의 스프라이트 이미지 컴포넌트. UI에서 사용할 때 이용한다.
    private Image mImageComp;

    //스프라이트 렌더러를 가져와 저장한다.
    private void Awake()
    {
        this.mSpriteComp = GetComponent<SpriteRenderer>();

        if (mSpriteComp == null)
        {
            this.mImageComp = GetComponent<Image>();
        }
    }

    //오브젝트가 활성화 되면 첫 프레임부터 시작하도록 한다.
    private void OnEnable()
    {
        mCurrentImageID = 0;
        mCurrentDelay = mImageDelay;

        if (mSpriteComp != null)
        {
            mSpriteComp.sprite = mSprites[mCurrentImageID];
        }
        else
        {
            mImageComp.sprite = mSprites[mCurrentImageID];
        }
    }

    //오브젝트가 활성화되어 Update()가 매 프레임 호출되면 mSprites의 0부터 length-1까지 loop를 돌도록 한다.
    private void Update()
    {
        mCurrentDelay -= Time.deltaTime;

        if (mCurrentDelay < 0)
        {
            ++mCurrentImageID;

            if (mCurrentImageID == mSprites.Length)
            {
                mCurrentImageID = 0;
            }

            if (mSpriteComp != null)
            {
                mSpriteComp.sprite = mSprites[mCurrentImageID];
            }
            else
            {
                mImageComp.sprite = mSprites[mCurrentImageID];
            }

            mCurrentDelay = mImageDelay;
        }
    }
}
