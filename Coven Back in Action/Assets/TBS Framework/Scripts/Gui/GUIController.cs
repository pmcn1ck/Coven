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
        public GameObject defeat;
        public GameObject gameEndWin;
        public GameObject victory;
        public Transform tVictory;
       // public GameObject gameEndLossUI;
        public GameObject canvasHud;
        public GameObject w_CangeUint;
        public GameObject pause;
        public Transform tPause;
        public Transform tChangeUnit;
        public Transform tGameEnd;
        public Unit unit;
        public bool isGameWon;
        public bool isGameLoss;

        bool isSpawnChangeUnit;
        bool isSpawnEndGame;
        wGamEnd gameEnd;
        wChangeUnit changeUnit;
        


        void Awake()
        {
            unit = FindObjectOfType<Unit>();
            CellGrid.LevelLoading += OnLevelLoading;
            CellGrid.LevelLoadingDone += OnLevelLoadingDone;
            CellGrid.GameEnded += OnGameEnded;
            CellGrid.TurnEnded += OnTurnEnded;
            CellGrid.GameStarted += OnGameStarted;
        }

        IEnumerator DefeatUi()
        {
            defeat.SetActive(true);
            yield return new WaitForSeconds(2);
            defeat.SetActive(false);
            gameEnd= Instantiate(gameEndLoss, tGameEnd).GetComponent<wGamEnd>();
        }

        IEnumerator VictoryUi()
        {
            GameObject obj = Instantiate(victory, tVictory);
            yield return new WaitForSeconds(2);
            Destroy(obj);
            gameEnd = Instantiate(gameEndWin, tGameEnd).GetComponent<wGamEnd>();

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
                StartCoroutine(VictoryUi());
               //gameEnd=Instantiate(gameEndWin, tGameEnd). GetComponent<wGamEnd>();
                Debug.Log("player as won the game");
                unit = FindObjectOfType<Unit>();
            }
            else
            {
                StartCoroutine(DefeatUi());
                //Instantiate(gameEndLoss, tGameEnd);
                isGameLoss = true;
                GameManager.gm.rooms.RemoveAt(GameManager.gm.rooms.Count - 1);

            } 

        }

        public void SpawnChangeUnit()
        {
            if(isSpawnChangeUnit == false)
            {
                changeUnit = Instantiate(w_CangeUint, tChangeUnit).GetComponent<wChangeUnit>();
                isSpawnChangeUnit = true;
                
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
                StartCoroutine(DefeatUi());
                isGameLoss = true;
                //Instantiate(gameEndLoss, tGameEnd);
                
            }
           
            if(gameEnd.isChangeUnit == true)
            {
                SpawnChangeUnit();
                //gameEndLossUI.SetActive(false);
            }

            if(changeUnit.isBack == true)
            {
                if(isSpawnEndGame  == false)
                {
                    gameEnd = Instantiate(gameEndLoss, tGameEnd).GetComponent<wGamEnd>();
                    isSpawnEndGame = true;
                    gameEnd.isChangeUnit = true;
                }
                

            }

            

        }

        public void EndTurn()
        {
            CellGrid.EndTurn();

        }
    }
}