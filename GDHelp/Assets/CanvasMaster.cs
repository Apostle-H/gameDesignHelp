using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class CanvasMaster : MonoBehaviour
    {
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private TextMeshProUGUI infoText;

        [SerializeField] private GameObject winPanel;
        [SerializeField] private TextMeshProUGUI winMistakesText;
        [SerializeField] private TextMeshProUGUI winTimeText;
        [SerializeField] private GameObject losePanel;
        [SerializeField] private TextMeshProUGUI loseMistakesText;
        [SerializeField] private TextMeshProUGUI loseTimeText;
        
        [SerializeField] private Slider healthBar;

        public void ToScene(int index)
        {
            SceneManager.LoadScene(index);
        }
        
        public void ShowObjectInfo(bool onOff, string info)
        {
            infoPanel.SetActive(onOff);
            infoText.text = info;
        }

        public void Die(bool winLose, int mistakes, float time)
        {
            if (winLose)
            {
                winPanel.SetActive(true);
                winMistakesText.text = mistakes.ToString();
                winTimeText.text = time.ToString();
            }
            else
            {
                losePanel.SetActive(true);
                loseMistakesText.text = mistakes.ToString();
                loseTimeText.text = time.ToString();
            }
        }

        public void HealthBar(float newValue)
        {
            healthBar.value = newValue;
        }
    }
}