using System.Collections.Generic;
using UnityEngine;

namespace TbsFramework.Grid.GameResolvers
{
    [System.Serializable]
    public class GameResult
    {
        public GameResult(bool isFinished, List<int> winningPlayers, List<int> loosingPlayers)
        {
            IsFinished = isFinished;
            WinningPlayers = winningPlayers;
            LoosingPlayers = loosingPlayers;
            Debug.Log("You did it!");
        }

        public bool IsFinished { get; private set; }

        [SerializeField]
        public List<int> WinningPlayers { get; private set; }
        [SerializeField]
        public List<int> LoosingPlayers { get; private set; }
    }
}

