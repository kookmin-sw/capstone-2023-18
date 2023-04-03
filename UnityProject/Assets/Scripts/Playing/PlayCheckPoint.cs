using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCheckPoint : MonoBehaviour
{
    public CP[] CP;
    public Transform Player;
    //돌아갈 유저 트랜스폼, 멀티플레이시 권한 설정하여 설정해 줄 예정.

    private void Awake()
    {
        init();
    }

    void init()
    {
        //씬에 존재하는 CP Component 저장.
        CP = GameObject.Find("=====CheckPoint======").GetComponentsInChildren<CP>();

        //씬에 존재하는 최대 CP 개수 저장.
        PlayManager.CP = 0;
        PlayManager.nextCP = new CP[] { CP[0] };
        PlayManager.MaxCP = CP.Length;

        //각 CP Index 부여
        for (int i=0; i<CP.Length; i++)
        {
            CP[i].CheckPointNum = i;
        }

        
    }
    
    public IEnumerator ReturnToCP()
    {
        PlayManager.isReturning = true;
        // 체크포인트로 돌아간 이후 카트의 움직임 제한.
        Rigidbody rb = Player.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;

        // 최근 체크포인트의 위치값 얻기.
        Transform ReturnPoint = CP[PlayManager.CP].GetReturnPoint();

        // 최근 체크포인트로 카트의 위치/회전 변경
        Player.position = ReturnPoint.position + new Vector3(0, 1, 0);
        Player.rotation = ReturnPoint.rotation;

        //0.3초 이후 다시 움직이기.
        yield return new WaitForSeconds(0.3f);
        rb.constraints = RigidbodyConstraints.None;
        PlayManager.isReturning = false;
    }

}
