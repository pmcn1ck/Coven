﻿using System.Collections.Generic;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Units.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace TbsFramework.HOMMExample
{
    public class SpellCastingAbility : Ability
    {
        public GameObject SpellBookPanel;
        public GameObject SpellPanel;
        public GameObject CancelButton;
        public List<SpellAbility> Spells;
        public List<GameObject> SpellPanels;
        public bool ShouldBeActive = false;

        public int MaxMana;
        public int CurrentMana { get; set; }
        public int ManaRecoveryRate;


        protected void Start()
        {
            CurrentMana = MaxMana;
            SpellPanels = new List<GameObject>();
            foreach (var spell in Spells)
            {
                spell.UnitReference = UnitReference;
            }
        }

        public override void Display(CellGrid cellGrid)
        {
            if (ShouldBeActive == true)
            {
            SpellBookPanel.SetActive(true);
            SpellBookPanel.transform.Find("TotalMana").GetComponent<Text>().text = string.Format("{0} Mana", CurrentMana);

            foreach (var spell in Spells)
            {
                var spellPanelInstance = Instantiate(SpellPanel);
                spellPanelInstance.transform.parent = SpellPanel.transform.parent;
                spellPanelInstance.transform.Find("SpellName").GetComponent<Text>().text = spell.GetComponent<SpellAbility>().SpellName;
                spellPanelInstance.transform.Find("Details").GetComponent<Text>().text = spell.GetComponent<SpellAbility>().GetDetails();
                spellPanelInstance.GetComponent<SpellDetails>().Spell = spell;
                spellPanelInstance.GetComponentInChildren<Image>().sprite = spell.GetComponent<SpellAbility>().Image;
                spellPanelInstance.GetComponentInChildren<Image>().color = spell.GetComponent<SpellAbility>().ManaCost <= CurrentMana ? Color.white : Color.gray;
                spellPanelInstance.SetActive(true);
                spellPanelInstance.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    if (CurrentMana >= spell.ManaCost)
                    {
                        if (spell.playerPicksTarget == true)
                        {
                            Debug.Log("Seeking a target in SpellCastingAbility");
                            CancelButton.SetActive(true);
                            cellGrid.CellGridState = new CellGridStateAbilitySelected(cellGrid, UnitReference, new List<Ability>() { spell });
                            
                        }
                        else
                        {
                            Debug.Log("Activating an ability without selected target from SpellCastingAbility");
                            spell.Activate(cellGrid);
                            SpellBookPanel.SetActive(false);
                        }
                    }
                });

                SpellPanels.Add(spellPanelInstance);
            }
            RemoveUI();
            }

        }

        public override void CleanUp(CellGrid cellGrid)
        {
            foreach (var panel in SpellPanels)
            {
                Destroy(panel);
            }

            SpellBookPanel.SetActive(false);
            SpellPanels = new List<GameObject>();
            ShouldBeActive = false;
        }

        public override void OnTurnStart(CellGrid cellGrid)
        {
            CurrentMana = Mathf.Min(CurrentMana + ManaRecoveryRate, MaxMana);
            foreach (var spell in Spells)
            {
                spell.OnTurnStart(cellGrid);
            }
        }

        public override void OnTurnEnd(CellGrid cellGrid)
        {
            foreach (var spell in Spells)
            {
                spell.OnTurnEnd(cellGrid);
            }
        }
        public void RemoveUI()
        {
            var cellGrid = FindObjectOfType<CellGrid>();
            //cellGrid.CellGridState = new CellGridStateWaitingForInput(cellGrid);
            CancelButton.SetActive(false);
            //SpellBookPanel.SetActive(false);
            ShouldBeActive = false;
        }

        public void CancelCasting()
        {
            var cellGrid = FindObjectOfType<CellGrid>();
            cellGrid.CellGridState = new CellGridStateWaitingForInput(cellGrid);
            CancelButton.SetActive(false);
            SpellBookPanel.SetActive(false);
            ShouldBeActive = false;
        }
    }
}