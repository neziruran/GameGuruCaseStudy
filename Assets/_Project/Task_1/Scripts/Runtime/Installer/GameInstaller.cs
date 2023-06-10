using System.Collections;
using System.Collections.Generic;
using _Project.Task_1.Runtime.Game;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private UIManager uıManager;
    [SerializeField] private CameraManager cameraManager;

    public override void InstallBindings()
    {
        Container.BindInstance(gridManager);
        Container.BindInstance(cameraManager);
        Container.BindInstance(uıManager);
        
    }
    
}

