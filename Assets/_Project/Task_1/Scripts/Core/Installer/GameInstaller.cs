using UnityEngine;
using Zenject;

namespace Project.Task_1
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GridManager gridManager;
        [SerializeField] private UIManager uıManager;
        [SerializeField] private CameraManager cameraManager;

        
        // install all bindings to container
        public override void InstallBindings()
        {
            Container.BindInstance(gridManager);
            Container.BindInstance(cameraManager);
            Container.BindInstance(uıManager);
        
        }
    
    }


}
