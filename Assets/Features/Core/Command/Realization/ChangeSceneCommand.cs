using Assets.Features.Core.ServiceLocatorScript;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Features.Core.Command.Realization
{
    public class ChangeSceneCommand : ICommand
    {
        private int _sceneId;

        public ChangeSceneCommand(int sceneId)
        {
            _sceneId = sceneId;
        }

        public void Cancel()
        {
            
        }

        public void Dispose()
        {
            
        }

        public UniTask<CommandResult> Do()
        {
            SceneManager.LoadSceneAsync(_sceneId).GetAwaiter();

            return new UniTask<CommandResult>(new CommandResult() { Body = null, Status = CommandStatus.Success });
        }
    }
}
