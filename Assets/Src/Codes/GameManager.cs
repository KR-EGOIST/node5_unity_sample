using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public int targetFrameRate;
    public string version = "1.0.0";
    public int latency = 2;

    [Header("# Player Info")]
    public uint playerId;
    public string deviceId;

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public GameObject hud;
    public GameObject GameStartUI;

    void Awake() {
        instance = this;
        Application.targetFrameRate = targetFrameRate;
    }

    public void GameStart(float x, float y) {
        playerId = (uint)Random.Range(0, 4);
        player.deviceId = deviceId;
        player.gameObject.SetActive(true);
        hud.SetActive(true);
        GameStartUI.SetActive(false);
        isLive = true;
        player.InitLocation(x, y);

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver() {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine() {
        isLive = false;
        yield return new WaitForSeconds(0.5f);

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameRetry() {
        SceneManager.LoadScene(0);
    }

    public void GameQuit() {
        Application.Quit();
    }

    void Update()
    {
        if (!isLive) {
            return;
        }
        gameTime += Time.deltaTime;
    }
}
