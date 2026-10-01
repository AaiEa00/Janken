using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum Hands
    {
        グー,
        チョキ,
        パー
    }

    static int handCount = System.Enum.GetValues(typeof(Hands)).Length;

    Hands playerHand;
    Hands cpuHand;

    [SerializeField] GameObject[] hands = new GameObject[handCount];
    [SerializeField] GameObject[] results = new GameObject[2];
    [SerializeField] Sprite[] handSprites = new Sprite[handCount];
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] Button replayButton;
    [SerializeField] GameObject navigation;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip replayClip;
    [SerializeField] AudioClip chooseClip;
    [SerializeField] Button soundButton;

    bool isMuted = false;

    private void Awake()
    {
        foreach (var res in results)
        {
            res.SetActive(false);
        }

        resultText.gameObject.SetActive(false);
        resultText.text = "";
        replayButton.gameObject.SetActive(false);

        // 音声の初期設定
        audioSource.mute = isMuted;
        soundButton.image.color = Color.white;
        soundButton.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
    }

    public void OnClick(string hand)
    {
        audioSource.PlayOneShot(chooseClip);

        playerHand = (Hands)System.Enum.Parse(typeof(Hands), hand);

        cpuHand = (Hands)Random.Range(0, handCount);

        Result(playerHand, cpuHand);
    }

    void Result(Hands player, Hands cpu)
    {
        // ボタンを押したときに手の画像を非表示にして、結果のUIを表示する
        foreach (var hand in hands)
        {
            hand.gameObject.SetActive(false);
        }
        foreach (var res in results)
        {
            res.SetActive(true);
        }

        Image playerImage = results[0].GetComponentInChildren<Image>();
        Image cpuImage = results[1].GetComponentInChildren<Image>();

        TextMeshProUGUI playerText = playerImage.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI cpuText = cpuImage.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        playerImage.sprite = handSprites[(int)player];
        cpuImage.sprite = handSprites[(int)cpu];

        playerText.text = player.ToString();
        cpuText.text = cpu.ToString();

        ExecuteJanken(player, cpu);
    }

    void ExecuteJanken(Hands player, Hands cpu)
    {
        int result = ((int)player - (int)cpu + handCount) % handCount;
        if (result == 0)
        {
            resultText.text = "引き分け";
        }
        else if (result == 2)
        {
            resultText.text = "あなたの勝ち";
            audioSource.PlayOneShot(winClip);
        }
        else
        {
            resultText.text = "あなたの負け";
        }
        resultText.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
        navigation.SetActive(false);
    }

    public void OnReplayClick()
    {
        foreach (var hand in hands)
        {
            hand.gameObject.SetActive(true);
        }
        foreach (var res in results)
        {
            res.SetActive(false);
        }
        resultText.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        navigation.SetActive(true);
        audioSource.PlayOneShot(replayClip);
    }

    public void OnSoundClick()
    {
        audioSource.mute = !audioSource.mute;
        // 音声のON/OFFに応じてボタンの色とテキストを変更
        if (audioSource.mute)
        {
            soundButton.image.color = Color.gray;
            soundButton.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
        }
        else
        {
            soundButton.image.color = Color.white;
            soundButton.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
        }
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
