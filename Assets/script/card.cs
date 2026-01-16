using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class card : MonoBehaviour, IPointerClickHandler
{
    [Header("Card Child Image")]
    public Image childImage;   
    void Start()
    {
        //child image is OFF by default
        if (childImage != null)
        {
            childImage.gameObject.SetActive(false);
        }
    }

    // child image turned on by clicking
    public void OnPointerClick(PointerEventData eventData)
    {
        if (childImage != null)
        {
            childImage.gameObject.SetActive(true);
        }
    }
}
