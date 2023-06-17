using _Project.Task2.Managers;
using UnityEngine;
using Zenject;

namespace _Project.Task2.Managers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private LevelManager levelManager;
        

        public override void InstallBindings()
        {

            Container.Bind<EventTriggers>().AsSingle().NonLazy();
            Container.Bind<GameManager>().FromInstance(gameManager);
            Container.Bind<UIManager>().FromInstance(uiManager);
            Container.Bind<LevelManager>().FromInstance(levelManager);
           
        }
    }

}
