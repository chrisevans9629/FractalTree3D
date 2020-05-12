using UnityEngine;

namespace Assets
{
    public class HideShow : MonoBehaviour
    {
        public GameObject ShowGameObject;
        public void Show()
        {
            ShowGameObject.SetActive(!ShowGameObject.activeSelf);
        }
    }
}