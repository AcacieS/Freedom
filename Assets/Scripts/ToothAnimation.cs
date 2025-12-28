using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ToothAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] toothSprites;
    [SerializeField] private float[] speedAnimations;
    int index = 0;
    int index_tooth = 0;

    // public void NextSpeedAnimation()
    // {
    //     index++;
    // }
    public void AnimTooth()
    {
        GetComponent<Animator>().SetTrigger("eat");
        // GetComponent<Image>().sprite = toothSprites[index_tooth];
        // index_tooth = (index_tooth+1) % toothSprites.Length;
    }
    // public IEnumerator ToothAnim()
    // {
    //     while (true)
    //     {
    //         Debug.Log("speedAnimations[index]:"+speedAnimations[index]);
    //         yield return new WaitForSeconds(speedAnimations[index]);
    //         Debug.Log("Tooth Animation Index: "+index_tooth);
    //         GetComponent<Image>().sprite = toothSprites[index_tooth];
    //         index_tooth = (index_tooth+1) % toothSprites.Length;
    //     }
        
    // }
}
