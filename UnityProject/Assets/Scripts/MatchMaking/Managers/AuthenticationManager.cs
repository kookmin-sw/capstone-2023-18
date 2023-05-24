using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthenticationManager : MonoBehaviour {

    public void Awake()
    {
        GameManager.Sound.BGMPlay(GameManager.Sound.BGMList[(int)BGMLIST.AUTH]);
    }
    public async void LoginAnonymously() {

            using (new Load("Logging you in..."))
            {

                await Authentication.Login();
                SceneManager.LoadSceneAsync("Lobby");

            }

    }
}