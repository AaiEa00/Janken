using UnityEngine;

public class CPUHand : MonoBehaviour
{
    public void Onclick()
    {
        string[] hands = { "グー", "チョキ", "パー" };
        int randomIndex = Random.Range(0, hands.Length);
        string cpuHand = hands[randomIndex];
        Debug.Log("CPU：" + cpuHand);
    }
}
