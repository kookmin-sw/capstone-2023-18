using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    public List<Rigidbody> UserRB;

    private void Awake()
    {
        UserRB = new List<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {

        //StartCoroutine(StartGame());
    }

    void FindKart()
    {
        GameObject[] karts = GameObject.FindGameObjectsWithTag("Kart");
        foreach (GameObject kart in karts)
        {
            Debug.Log(kart.name);
            UserRB.Add(kart.transform.GetComponent<Rigidbody>());
        }
    }

    public IEnumerator StartGame()
    {
        Debug.Log("Start Game");
        PlayUI UI = GameObject.Find("@PlayManager").GetComponent<PlayUI>();
        Debug.Log(UI.gameObject.name);
        FindKart();
        while(UserRB.Count == 0)
        {
            yield return null;
        }

        foreach (Rigidbody rb in UserRB)
        {
            //시작 전 모든 유저 동결
            Debug.Log("Check");
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        for (int i = 3; i > 0; i--)
        {
            Debug.Log(i);
            UI.Count.gameObject.SetActive(true);
            UI.Count.text = i.ToString();
            yield return new WaitForSecondsRealtime(0.2f);
            UI.Count.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.8f);
        }

        PlayManager.isStart = true;
        foreach (Rigidbody rb in UserRB)
        {
            //시작 전 모든 유저 동결
            rb.constraints = RigidbodyConstraints.None;
        }

        UI.Count.gameObject.SetActive(true);
        UI.Count.text = "GO";
        yield return new WaitForSecondsRealtime(0.2f);
        UI.Count.gameObject.SetActive(false);
    }
}
