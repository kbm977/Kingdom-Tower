using Runtime.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals.Game
{
    public class GameSignals : MonoSingleton<GameSignals>
    {
        //Adjusts what needs to be adjusted based on the phase
        public UnityAction<GameState> onChangeGameState = delegate { };
        //This can be called to either increase or decrease orbs
        public UnityAction<int> onAddOrbs = delegate { };
        public UnityAction<int> onUseOrbs = delegate { };

        public UnityAction onNextLevel = delegate { };

        public UnityAction<bool> onToggleGridTiles = delegate { };

        public UnityAction<bool, Texture2D> onCursorChange = delegate { };

        public UnityAction onLose = delegate { };
        public UnityAction onWin = delegate { };
    }
}
