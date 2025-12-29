using UnityEngine;

public class SwitchBG : MonoBehaviour
{
    [SerializeField] private Sprite[] bgs;
    private int index;
    public void SwitchNextBG()
    {
        if(index==bgs.Length) return;
        GetComponent<SpriteRenderer>().sprite = bgs[index];
        index++;
    }
}
