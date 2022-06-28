using System;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Players;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TbsFramework.Units;
using System.Collections;

namespace TbsFramework.Gui
{
    public class GUIController : MonoBehaviour
    {
        public CellGrid CellGrid;
        public Button EndTurnButton;
        public Image iTurnBanner;
        public Text tTurnBanner;
        public GameObject gameEndLoss;
        public GameObject gameEndWin;
        public Transform tGameEnd;
        public Unit unit;
        public bool isGameWon;

        void Awake()
        {
            unit = FindObjectOfType<Unit>();
            CellGrid.LevelLoading += OnLevelLoading;
            CellGrid.LevelLoadingDone += OnLevelLoadingDone;
            CellGrid.GameEnded += OnGameEnded;
            CellGrid.TurnEnded += OnTurnEnded;
            CellGrid.GameStarted += OnGameStarted;
        }

        private void OnGameStarted(object sender, EventArgs e)
        {
            if (EndTurnButton != null)
            {
                EndTurnButton.interactable = CellGrid.CurrentPlayer is HumanPlayer;
            }
        }

        private void OnTurnEnded(object sender, EventArgs e)
        {
            if (EndTurnButton != null)
            {
                EndTurnButton.interactable = CellGrid.CurrentPlayer is HumanPlayer;
            }
        }

        private void OnGameEnded(object sender, GameEndedArgs e)
        {
            //Debug.Log(string.Format("Player{0} wins!", e.gameResult.WinningPlayers[0]));

            bool isWinningGame = false;
            for (int i = 0; i < e.gameResult.WinningPlayers.Count; i++)
            {
                if (e.gameResult.WinningPlayers[i] == 0)
                {
                    isWinningGame = true;
                    Debug.Log("Is Winning Game" + isWinningGame);
                }
                //Debug.Log(e.gameResult.WinningPlayers[0]);
            }

            

            
            if(EndTurnButton != null)
            {
                EndTurnButton.interactable = false;
            }

            if (isWinningGame)
            {
                Instantiate(gameEndWin, tGameEnd);
                Debug.Log("player as won the game");
                unit = FindObjectOfType<Unit>();
            }
            else
            {
                Instantiate(gameEndLoss, tGameEnd);
                GameManager.gm.rooms.RemoveAt(GameManager.gm.rooms.Count - 1);

            } 

        }

      

        private void OnLevelLoading(object sender, EventArgs e)
        {
            Debug.Log("Level is loading");
        }

        private void OnLevelLoadingDone(object sender, EventArgs e)
        {
            Debug.Log("Level loading done");
            Debug.Log("Press 'm' to end turn");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M) && !(CellGrid.CellGridState is CellGridStateAITurn))
            {
                EndTurn();//User ends his turn by pressing "m" on keyboard.
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                Units.Unit u = CellGrid.GetCurrentSelectedUnit();
                Player p = CellGrid.Players[0];
                foreach(var item in CellGrid.GetEnemyUnits(p))
                {
                    item.DefendHandler(u, 10000);
                    unit.levelUp = true;
                    unit.level++;
                }
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                /* Player p = CellGrid.Players[0];
                 Units.Unit u = CellGrid.GetEnemyUnits(p)[0];

                 foreach (var item in CellGrid.Units)
                 {
                     if (item != u)
                     {
                         item.DefendHandler(u, 10000);
                     }
                 }*/
                Instantiate(gameEndLoss, tGameEnd);
            }
        }

        public void EndTurn()
        {
            CellGrid.EndTurn();

        }
    }
}