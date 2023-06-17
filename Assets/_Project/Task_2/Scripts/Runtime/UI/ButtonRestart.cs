using UnityEngine;
using Zenject;
using _Project.Task2.Managers;
using UnityEngine.UI;

namespace _Project.Task_2.UI
{
    public class ButtonRestart : MonoBehaviour
    {
        [Inject] 
        private EventTriggers _eventTriggers;
        
        [SerializeField] private GameObject parentPanel;
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClick_ResLevel);
        }

        private void OnClick_ResLevel()
        {

            _eventTriggers.OnRestartLevel?.Invoke();
            parentPanel.SetActive(false);
        }   
    }
}