using _Project.Task2.Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Task_2.UI
{
    public class ButtonNextLevel : MonoBehaviour
    {
        [Inject] 
        private EventTriggers _eventTriggers;
        
        [SerializeField] private GameObject parentPanel;
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClick_NextLevel);
        }
        

        private void OnClick_NextLevel()
        {
            
            _eventTriggers.OnNextLevel?.Invoke();
            parentPanel.SetActive(false);
        }
    }
}