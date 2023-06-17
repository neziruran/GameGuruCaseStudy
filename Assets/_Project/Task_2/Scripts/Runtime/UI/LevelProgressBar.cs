using UnityEngine;
using Zenject;
using _Project.Task2.Managers;
using TMPro;
using UnityEngine.UI;

namespace _Project.Task_2.UI
{
    public class LevelProgressBar : MonoBehaviour
    {
        [Inject] 
        private EventTriggers _eventTriggers;


        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private GameObject startPos;
        [SerializeField] private Image image;

        private float _distance;        
        private float _endPos;

        private void OnEnable()
        {
            _eventTriggers.OnProgressBarSet += SetValues;
        }
        private void OnDisable()
        {
            _eventTriggers.OnProgressBarSet -= SetValues;
            levelText.text = " ";
            image.fillAmount = 0;
            startPos = null;
        }

        void Update()
        {
            if (startPos)
            {               
                image.fillAmount = 1 - ((_endPos - startPos.transform.position.z) / _distance);
            }
        }

        private void SetValues(GameObject startBlock, float endPosValue, int levelValue)
        {
            startPos = startBlock;
            _endPos = endPosValue;
            levelText.text = "LEVEL " + levelValue;
            _distance = _endPos - startPos.transform.position.z;
        }
    }
}