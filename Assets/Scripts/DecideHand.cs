using UnityEngine;

public class DecideHand : MonoBehaviour
{
    public void OnClick(string hand)
    {
        Debug.Log("プレイヤー：" + hand);
    }
}
