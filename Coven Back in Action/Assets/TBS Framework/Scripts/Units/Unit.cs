using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using TbsFramework.Cells;
using TbsFramework.Pathfinding.Algorithms;
using TbsFramework.Units.Highlighters;
using TbsFramework.Units.UnitStates;
using TbsFramework.Grid;
using TbsFramework.Players.AI.Actions;
using TbsFramework.Players.AI.Evaluators;
using UnityEngine.UI;
using System.Threading;
using TMPro;
using UnityEngine.EventSystems;

public enum eState {none, finished, normal, friendly, selected }

namespace TbsFramework.Units
{
    /// <summary>
    /// Base class for all units in the game.
    /// </summary>
    [ExecuteInEditMode]
    public class Unit : MonoBehaviour
    {
        Dictionary<Cell, List<Cell>> cachedPaths = null;
        /// <summary>
        /// UnitClicked event is invoked when user clicks the unit. 
        /// It requires a collider on the unit game object to work.
        /// </summary>
        public event EventHandler UnitClicked;
        /// <summary>
        /// UnitSelected event is invoked when user clicks on unit that belongs to him. 
        /// It requires a collider on the unit game object to work.
        /// </summary>
        public event EventHandler UnitSelected;
        /// <summary>
        /// UnitDeselected event is invoked when user click outside of currently selected unit's collider.
        /// It requires a collider on the unit game object to work.
        /// </summary>
        public event EventHandler UnitDeselected;
        /// <summary>
        /// UnitHighlighted event is invoked when user moves cursor over the unit. 
        /// It requires a collider on the unit game object to work.
        /// </summary>
        public event EventHandler UnitHighlighted;
        /// <summary>
        /// UnitDehighlighted event is invoked when cursor exits unit's collider. 
        /// It requires a collider on the unit game object to work.
        /// </summary>
        public event EventHandler UnitDehighlighted;
        /// <summary>
        /// UnitAttacked event is invoked when the unit is attacked.
        /// </summary>
        public event EventHandler<AttackEventArgs> UnitAttacked;
        /// <summary>
        /// UnitDestroyed event is invoked when unit's hitpoints drop below 0.
        /// </summary>
        public event EventHandler<AttackEventArgs> UnitDestroyed;
        /// <summary>
        /// UnitMoved event is invoked when unit moves from one cell to another.
        /// </summary>
        public event EventHandler<MovementEventArgs> UnitMoved;

        public UnitHighlighterAggregator UnitHighlighterAggregator;

        public bool Obstructable = true;

        public UnitState UnitState { get; set; }
        public void SetState(UnitState state)
        {
            Debug.Log("Set State = " + gameObject.name + " state: " + UnitState);
            UnitState.MakeTransition(state);
            Debug.Log("New State = " + gameObject.name + " state after: " + UnitState);
            //Debug.Log(string.Format("{0} - {1}", gameObject, UnitState));
        }


        public void SetStateUnit(eState _newState)
        {
            curState = _newState;
        }
        /// <summary>
        /// A list of buffs that are applied to the unit.
        /// </summary>
        private List<(Buff buff, int timeLeft)> Buffs;
        public void AddBuff(Buff buff)
        {
            buff.Apply(this);
            Buffs.Add((buff, buff.Duration));
        }

        [HideInInspector]
        public int TotalHitPoints;
        public float TotalMovementPoints { get; private set; }
        public float TotalActionPoints { get; private set; }

        /// <summary>
        /// Cell that the unit is currently occupying.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        private Cell cell;
        public Cell Cell
        {
            get
            {
                return cell;
            }
            set
            {
                cell = value;
            }
        }

        public eState curState;
        public ExperimentalUnit experimentalUnit;

        public Slider HealthSlider; // In game hit points
        public Slider UI_HealthSlider;  // Victory/Defeat hit points
        public Image HealthImage;
        public GameObject DamageIndicator;
        public int MaxHitPoints; // Starting Hit Points
        public int HitPoints; // Current Hit Points
        public int AttackRange;
        public int AttackFactor;
        public int DefenceFactor;
        public string Name;
        public int level;
        public int experience;
        public int totalExperience;
        public bool levelUp;
        [Header("Blood Lust")]
        public double BloodLust;
        public int bloodLust;
        public double BloodGainMin;
        public int bloodLustMin;
        public double BloodGainMax;
        public int bloodLustMax;
        public double BloodMultiplier;
        public Slider BloodLustSlider;
        public GameObject bullet;
         
        [Header("Movement")]
        /// <summary>
        /// Determines how far on the grid the unit can move.
        /// </summary>
        [SerializeField]
        private float movementPoints;

        [Header("Status Effects")]
        public bool Stun;
        public bool SupportFireTarget;
        [Header("Animation")]
        public Animator c_Animator;
        public AnimationScript animScript;
        public virtual float MovementPoints
        {
            get
            {
                return movementPoints;
            }
            set
            {
                movementPoints = value;
            }
        }
        /// <summary>
        /// Determines speed of movement animation.
        /// </summary>
        public float MovementAnimationSpeed;
        /// <summary>
        /// Determines how many attacks unit can perform in one turn.
        /// </summary>
        [SerializeField]
        private float actionPoints = 1;
        public float ActionPoints
        {
            get
            {
                return actionPoints;
            }
            set
            {
                actionPoints = value;
            }
        }

        /// <summary>
        /// Indicates the player that the unit belongs to. 
        /// Should correspoond with PlayerNumber variable on Player script.
        /// </summary>
        public int PlayerNumber;

        /// <summary>
        /// Indicates if movement animation is playing.
        /// </summary>
        public bool IsMoving { get; set; }
        [Header("Particles and Effects")]
        public Sprite characterPortrait;
        public ParticlePlayer particlePlayer;
        public CharacterSoundManager pSoundManager;
        private static DijkstraPathfinding _pathfinder = new DijkstraPathfinding();
        private static IPathfinding _fallbackPathfinder = new AStarPathfinding();

        /// <summary>
        /// Method called after object instantiation to initialize fields etc. 
        /// </summary>

        private void Awake()
        {
            experimentalUnit = FindObjectOfType<ExperimentalUnit>();
        }
        public virtual void Initialize()
        {
            Buffs = new List<(Buff, int)>();

            UnitState = new UnitStateNormal(this);
            c_Animator = gameObject.GetComponentInChildren<Animator>();
            animScript = gameObject.GetComponentInChildren<AnimationScript>();
            particlePlayer = gameObject.GetComponentInChildren<ParticlePlayer>();
            pSoundManager = gameObject.GetComponentInChildren<CharacterSoundManager>();
            //TotalHitPoints = HitPoints;
            TotalMovementPoints = MovementPoints;
            TotalActionPoints = ActionPoints;
            HealthSlider.maxValue = MaxHitPoints;
            bloodLustMax = (int)BloodGainMax;
            bloodLustMin = (int)BloodGainMin;
            bloodLust = (int)BloodLust;

        }

        public virtual void OnMouseDown()
        {
            if (UnitClicked != null)
            {
                UnitClicked.Invoke(this, new EventArgs());
            }
        }
        protected virtual void OnMouseEnter()
        {
            if (UnitHighlighted != null)
            {
                UnitHighlighted.Invoke(this, new EventArgs());
            }
        }
        protected virtual void OnMouseExit()
        {
            if (UnitDehighlighted != null)
            {
                UnitDehighlighted.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Method is called at the start of each turn.
        /// </summary>
        public virtual void OnTurnStart()
        {
            cachedPaths = null;

            Buffs.FindAll(b => b.timeLeft == 0).ForEach(b => { b.buff.Undo(this); });
            Buffs.RemoveAll(b => b.timeLeft == 0);
            var name = this.name;
            var state = UnitState;
            SetState(new UnitStateMarkedAsFriendly(this));
            curState = eState.friendly;
            if (Stun == true)
            {
                curState = eState.finished;
                Stun = false;
            }
        }
        /// <summary>
        /// Method is called at the end of each turn.
        /// </summary>
        public virtual void OnTurnEnd()
        {
            for (int i = 0; i < Buffs.Count; i++)
            {
                (Buff buff, int timeLeft) = Buffs[i];
                Buffs[i] = (buff, timeLeft - 1);
            }

            MovementPoints = TotalMovementPoints;
            ActionPoints = TotalActionPoints;

            SetState(new UnitStateNormal(this));
            curState = eState.normal;
        }
        /// <summary>
        /// Method is called when units HP drops below 1.
        /// </summary>
        

        protected virtual void OnDestroyed()
        {
            Cell.IsTaken = false;
            Cell.CurrentUnits.Remove(this);
            MarkAsDestroyed();
            Debug.Log(Name + " just freaking died, guys");
            Destroy(gameObject);
        }

        /// <summary>
        /// Method is called when unit is selected.
        /// </summary>
        public virtual void OnUnitSelected()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (FindObjectOfType<CellGrid>().GetCurrentPlayerUnits().Contains(this))
                {
                    SetState(new UnitStateMarkedAsSelected(this));
                    curState = eState.selected;
                }
                if (UnitSelected != null)
                {
                    UnitSelected.Invoke(this, new EventArgs());
                }
            }
        }
        /// <summary>
        /// Method is called when unit is deselected.
        /// </summary>
        public virtual void OnUnitDeselected()
        {
            if (FindObjectOfType<CellGrid>().GetCurrentPlayerUnits().Contains(this))
            {
                SetState(new UnitStateMarkedAsFriendly(this));
                curState = eState.friendly;
            }
            if (UnitDeselected != null)
            {
                UnitDeselected.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Method indicates if it is possible to attack a unit from given cell.
        /// </summary>
        /// <param name="other">Unit to attack</param>
        /// <param name="sourceCell">Cell to perform an attack from</param>
        /// <returns>Boolean value whether unit can be attacked or not</returns>
        public virtual bool IsUnitAttackable(Unit other, Cell sourceCell)
        {
            return IsUnitAttackable(other, other.Cell, sourceCell);
        }
        public virtual bool IsUnitAttackable(Unit other, Cell otherCell, Cell sourceCell)
        {
            return sourceCell.GetDistance(otherCell) <= AttackRange
                && other.PlayerNumber != PlayerNumber
                && ActionPoints >= 1;
        }

        public void GetAttackHandler()
        {
           

        }

        public void UnitLevelUP()
        {
            experience += AttackFactor;
            if (experience == totalExperience)
            {
                levelUp = true;
                if (levelUp == true)
                {
                    level++;
                }
            }

        }
        //xxy
        IEnumerator attackParticle()
        {
            yield return new WaitForSeconds((float)1f);
            particlePlayer.CallAttackParticle();
        }

        IEnumerator AttackSound()
        {
            yield return new WaitForSeconds(1f);
            pSoundManager.CallAttackAudio();
        }

        /// <summary>
        /// Method performs an attack on given unit.
        /// </summary>
        public void AttackHandler(Unit unitToAttack)
        {
            UnitLevelUP();


            Debug.Log("experience " + experience);
            transform.LookAt(unitToAttack.transform.position, Vector3.up);
            if (animScript != null)
                animScript.runAttackAnim();
            if (particlePlayer.attack != null)
                StartCoroutine("attackParticle");
            if (pSoundManager != null)
                StartCoroutine("AttackSound");
            if (AttackRange > 1 && bullet != null)
            {
                Debug.Log("Instantiating Bullet");
                Vector3 bulletPos = transform.position;
                bulletPos.y += 1;
                Vector3 target = unitToAttack.transform.position;
                target.y += 1;
                GameObject clone = Instantiate(bullet, bulletPos, transform.rotation);
                StartCoroutine(ProjectileMove(clone, target));
            }
            AttackAction attackAction = DealDamage(unitToAttack);
            MarkAsAttacking(unitToAttack);
            unitToAttack.DefendHandler(this, attackAction.Damage);
            AttackActionPerformed(attackAction.ActionCost);
        }
        /// <summary>
        /// Method for calculating damage and action points cost of attacking given unit
        /// </summary>
        /// <returns></returns>
        protected virtual AttackAction DealDamage(Unit unitToAttack)
        {
            return new AttackAction(AttackFactor, 1f);
        }
        /// <summary>
        /// Method called after unit performed an attack.
        /// </summary>
        /// <param name="actionCost">Action point cost of performed attack</param>
        protected virtual void AttackActionPerformed(float actionCost)
        {
            ActionPoints -= actionCost;
        }



        /// <summary>
        /// Handler method for defending against an attack.
        /// </summary>
        /// <param name="aggressor">Unit that performed the attack</param>
        /// <param name="damage">Amount of damge that the attack caused</param>
        public void DefendHandler(Unit aggressor, int damage)
        {
            
            transform.LookAt(aggressor.transform.position, Vector3.up);
            MarkAsDefending(aggressor);
            int damageTaken = Defend(aggressor, damage);
            HitPoints -= damageTaken;
            if (animScript != null)
            

            if (HealthSlider != null)
            {
                HealthSlider.value = HitPoints;
            }
            
            if (UI_HealthSlider != null)
            {
                UI_HealthSlider.value = HitPoints;
            }


            if (particlePlayer.takeDamage != null)
            {
                StartCoroutine("DamageWait");
            }

            DefenceActionPerformed();
            BloodDefendHandler(aggressor);
            if (UnitAttacked != null)
            {
                UnitAttacked.Invoke(this, new AttackEventArgs(aggressor, this, damage));
            }
            if (HitPoints <= 0)
            {
                if (animScript != null)
                    StartCoroutine("DeathWait");
                if (UnitDestroyed != null)
                {
                    StartCoroutine(DestroyUnit(aggressor, damage));
                    
                }
                StartCoroutine("RunOnDestroy");
            }
            if (DamageIndicator != null)
            {
                DamageIndicator.GetComponent<TextMeshProUGUI>().text = (damageTaken.ToString());
                DamageIndicator.SetActive(true);
                Invoke("HideDamageIndicator", 2f);
            }
            
            Debug.Log(Name + " took " + damageTaken + " damage. " + HitPoints + " Hit Points remain.");
            Debug.Log(Name + "'s Bloodlust is now " + BloodLust);
            if (BloodLustSlider != null)
            {
                BloodLustSlider.value = ((int)BloodLust);
            }
            if (GetComponent<Counter>() != null)
            {
                GetComponent<Counter>().AttackResponse(aggressor);
            }
            if (SupportFireTarget == true)
            {
                SupportFireTarget = false;
                GameObject CG = GameObject.Find("CellGrid");
                CellGrid cellGrid = CG.GetComponent<CellGrid>();
                foreach (Unit u in cellGrid.GetEnemyUnits(cellGrid.Players[GetComponent<Unit>().PlayerNumber]))
                {
                    if (u.GetComponentInChildren<SupportingFire>() != null)
                    {
                        u.GetComponentInChildren<SupportingFire>().AttackResponse(this);
                    }
                }
            }
        }

        public void LowerDefenseHandler(int lowerDefense)
        {
            DefenceFactor -= lowerDefense;
        }

        IEnumerator DeathWait()
        {
            yield return new WaitForSeconds((float)1.2);
            animScript.runDeathAnim();
            yield return new WaitForSeconds((float)2f);
        }

        IEnumerator DamageAudioWait()
        {
            yield return new WaitForSeconds(0.2f);
            if (pSoundManager != null)
            {
                pSoundManager.CallDamageAudio();
            }
        }
        IEnumerator DamageWait()
        {
            yield return new WaitForSeconds(1);
            animScript.runDamageAnim();
            StartCoroutine("DamageAudioWait");
            particlePlayer.CallDamageParticle();
        }
        IEnumerator DestroyUnit(Unit aggressor, int damage)
        {
            yield return new WaitForSeconds(3);
                UnitDestroyed.Invoke(this, new AttackEventArgs(aggressor, this, damage));
        }
        IEnumerator RunOnDestroy()
        {
            yield return new WaitForSeconds(3);
            OnDestroyed();
        }
        void HideDamageIndicator() 
        {
            DamageIndicator.SetActive(false);
        }


        public void BloodDefendHandler(Unit aggressor)
        {
            BloodLust += UnityEngine.Random.Range((float)aggressor.BloodGainMin, (float)aggressor.BloodGainMax) * BloodMultiplier;
            
            if (BloodLust >= 100)
            {
                if (PlayerNumber != 2)
                {
                    AttackFactor += 3;
                }

/*                
 *                ////Gotta rewrite this so it doesn't just keep healing the unit (Bloodlust units weren't dying)
 *                if (HitPoints < TotalHitPoints && PlayerNumber != 2)
                {
                    HitPoints = TotalHitPoints / 2;
                }
*/
                PlayerNumber = 2;

                
            }
        }
        /// <summary>
        /// Method for calculating actual damage taken by the unit.
        /// </summary>
        /// <param name="aggressor">Unit that performed the attack</param>
        /// <param name="damage">Amount of damge that the attack caused</param>
        /// <returns>Amount of damage that the unit has taken</returns>        
        protected virtual int Defend(Unit aggressor, int damage)
        {
            return Mathf.Clamp(damage - DefenceFactor, 1, damage);
        }
        /// <summary>
        /// Method callef after unit performed defence.
        /// </summary>
        protected virtual void DefenceActionPerformed() { }

        public int DryAttack(Unit other)
        {
            int damage = DealDamage(other).Damage;
            int realDamage = other.Defend(this, damage);

            return realDamage;
        }

        /// <summary>
        /// Handler method for moving the unit.
        /// </summary>
        /// <param name="destinationCell">Cell to move the unit to</param>
        /// <param name="path">A list of cells, path from source to destination cell</param>
        public virtual void Move(Cell destinationCell, List<Cell> path)
        {
            var totalMovementCost = path.Sum(h => h.MovementCost);
            MovementPoints -= totalMovementCost;

            Cell.IsTaken = false;
            Cell.CurrentUnits.Remove(this);
            Cell = destinationCell;
            destinationCell.IsTaken = true;
            destinationCell.CurrentUnits.Add(this);
            if (animScript != null)
            animScript.toggleMoveAnim();

            if (MovementAnimationSpeed > 0)
            {
                StartCoroutine(MovementAnimation(path));
            }
            else
            {
                transform.position = Cell.transform.position;
                OnMoveFinished();
            }

            if (UnitMoved != null)
            {
                UnitMoved.Invoke(this, new MovementEventArgs(Cell, destinationCell, path, this));
                
            }
            

        }
        protected virtual IEnumerator MovementAnimation(List<Cell> path)
        {
            IsMoving = true;
            for (int i = path.Count - 1; i >= 0; i--)
            {
                var currentCell = path[i];
                Vector3 destination_pos = FindObjectOfType<CellGrid>().Is2D ? new Vector3(currentCell.transform.localPosition.x, currentCell.transform.localPosition.y, transform.localPosition.z) : new Vector3(currentCell.transform.localPosition.x, transform.localPosition.y, currentCell.transform.localPosition.z);
                while (transform.localPosition != destination_pos)
                {
                    transform.LookAt(destination_pos, Vector3.up);
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination_pos, Time.deltaTime * MovementAnimationSpeed);
                    yield return 0;

                }


            }

            IsMoving = false;
            OnMoveFinished();
        }

        protected virtual IEnumerator ProjectileMove(GameObject projectile, Vector3 target)
        {
            while (projectile.transform.localPosition != target)
            {
                projectile.transform.LookAt(target, Vector3.up);
                projectile.transform.localPosition = Vector3.MoveTowards(projectile.transform.localPosition, target, Time.deltaTime*10);
                yield return 0;
            }
            Destroy(projectile);
            Debug.Log("Bullet Destroyed");
        }
        /// <summary>
        /// Method called after movement animation has finished.
        /// </summary>
        protected virtual void OnMoveFinished()
        {
            if(animScript != null)
            animScript.toggleMoveAnim();
        }

        ///<summary>
        /// Method indicates if unit is capable of moving to cell given as parameter.
        /// </summary>
        public virtual bool IsCellMovableTo(Cell cell)
        {
            return !cell.IsTaken;
        }
        /// <summary>
        /// Method indicates if unit is capable of moving through cell given as parameter.
        /// </summary>
        public virtual bool IsCellTraversable(Cell cell)
        {
            return !cell.IsTaken;
        }
        /// <summary>
        /// Method returns all cells that the unit is capable of moving to.
        /// </summary>
        public HashSet<Cell> GetAvailableDestinations(List<Cell> cells)
        {
            cachedPaths = new Dictionary<Cell, List<Cell>>();

            var paths = CachePaths(cells);
            foreach (var key in paths.Keys)
            {
                if (!IsCellMovableTo(key))
                {
                    continue;
                }
                var path = paths[key];

                var pathCost = path.Sum(c => c.MovementCost);
                if (pathCost <= MovementPoints)
                {
                    cachedPaths.Add(key, path);
                }
            }
            return new HashSet<Cell>(cachedPaths.Keys);
        }

        private Dictionary<Cell, List<Cell>> CachePaths(List<Cell> cells)
        {
            var edges = GetGraphEdges(cells);
            var paths = _pathfinder.findAllPaths(edges, Cell);
            return paths;
        }

        public List<Cell> FindPath(List<Cell> cells, Cell destination)
        {
            if (cachedPaths != null && cachedPaths.ContainsKey(destination))
            {
                return cachedPaths[destination];
            }
            else
            {
                return _fallbackPathfinder.FindPath(GetGraphEdges(cells), Cell, destination);
            }
        }
        /// <summary>
        /// Method returns graph representation of cell grid for pathfinding.
        /// </summary>
        protected virtual Dictionary<Cell, Dictionary<Cell, float>> GetGraphEdges(List<Cell> cells)
        {
            Dictionary<Cell, Dictionary<Cell, float>> ret = new Dictionary<Cell, Dictionary<Cell, float>>();
            foreach (var cell in cells)
            {
                if (IsCellTraversable(cell) || cell.Equals(Cell))
                {
                    ret[cell] = new Dictionary<Cell, float>();
                    foreach (var neighbour in cell.GetNeighbours(cells).FindAll(IsCellTraversable))
                    {
                        ret[cell][neighbour] = neighbour.MovementCost;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Gives visual indication that the unit is under attack.
        /// </summary>
        /// <param name="aggressor">
        /// Unit that is attacking.
        /// </param>
        public virtual void MarkAsDefending(Unit aggressor)
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsDefendingFn?.ForEach(o => o.Apply(this, aggressor));
            }
        }
        /// <summary>
        /// Gives visual indication that the unit is attacking.
        /// </summary>
        /// <param name="target">
        /// Unit that is under attack.
        /// </param>
        public virtual void MarkAsAttacking(Unit target)
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsAttackingFn?.ForEach(o => o.Apply(this, target));
            }
        }
        /// <summary>
        /// Gives visual indication that the unit is destroyed. It gets called right before the unit game object is
        /// destroyed.
        /// </summary>
        public virtual void MarkAsDestroyed()
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsDestroyedFn?.ForEach(o => o.Apply(this, null));
            }
        }

        /// <summary>
        /// Method marks unit as current players unit.
        /// </summary>
        public virtual void MarkAsFriendly()
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsFriendlyFn?.ForEach(o => o.Apply(this, null));
            }
        }
        /// <summary>
        /// Method mark units to indicate user that the unit is in range and can be attacked.
        /// </summary>
        public virtual void MarkAsReachableEnemy()
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsReachableEnemyFn?.ForEach(o => o.Apply(this, null));
            }
        }
        /// <summary>
        /// Method marks unit as currently selected, to distinguish it from other units.
        /// </summary>
        public virtual void MarkAsSelected()
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsSelectedFn?.ForEach(o => o.Apply(this, null));
            }
        }
        /// <summary>
        /// Method marks unit to indicate user that he can't do anything more with it this turn.
        /// </summary>
        public virtual void MarkAsFinished()
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.MarkAsFinishedFn?.ForEach(o => o.Apply(this, null));
            }
        }
        /// <summary>
        /// Method returns the unit to its base appearance
        /// </summary>
        public virtual void UnMark()
        {
            if (UnitHighlighterAggregator != null)
            {
                UnitHighlighterAggregator.UnMarkFn?.ForEach(o => o.Apply(this, null));
            }
        }
        public virtual void SetColor(Color color) { }

        [ExecuteInEditMode]
        public void OnDestroy()
        {
            if (Cell != null)
            {
                Cell.IsTaken = false;
            }
        }

        private void Reset()
        {
            if (GetComponent<Abilities.AttackAbility>() == null)
            {
                gameObject.AddComponent<Abilities.AttackAbility>();
            }
            if (GetComponent<Abilities.MoveAbility>() == null)
            {
                gameObject.AddComponent<Abilities.MoveAbility>();
            }

            GameObject brain = new GameObject("Brain");
            brain.transform.parent = transform;

            brain.AddComponent<MoveToPositionAIAction>();
            brain.AddComponent<AttackAIAction>();

            brain.AddComponent<DamageCellEvaluator>();
            brain.AddComponent<DamageUnitEvaluator>();
        }

        //this controls level ups
        public void LevelAdvancement()
        {
            if (experience >= 100)
            {
                levelUp = true;
            }
        }


    }

    public class AttackAction
    {
        public readonly int Damage;
        public readonly float ActionCost;

        public AttackAction(int damage, float actionCost)
        {
            //Make damage semi random, max range min range situation
            Damage = damage;
            //Calculate BloodLust Increase (something semi random but based off damage) (add a check for to make sure it's a human player player)
            ActionCost = actionCost;
        }
    }

    public class MovementEventArgs : EventArgs
    {
        public Cell OriginCell;
        public Cell DestinationCell;
        public List<Cell> Path;
        public Unit Unit;

        public MovementEventArgs(Cell sourceCell, Cell destinationCell, List<Cell> path, Unit unit)
        {
            OriginCell = sourceCell;
            DestinationCell = destinationCell;
            Path = path;
            Unit = unit;
        }
    }
    public class AttackEventArgs : EventArgs
    {
        public Unit Attacker;
        public Unit Defender;

        public int Damage;

        public AttackEventArgs(Unit attacker, Unit defender, int damage)
        {
            Attacker = attacker;
            Defender = defender;

            Damage = damage;
        }
    }
    public class UnitCreatedEventArgs : EventArgs
    {
        public Transform unit;

        public UnitCreatedEventArgs(Transform unit)
        {
            this.unit = unit;
        }
    }

}
