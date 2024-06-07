using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts;
using Assets.Scripts.GridSystem;
using Assets.Scripts.Inventory;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Entities;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.UI;
using BepInEx;
using HarmonyLib;
using UnityEngine;

/* Statineers Shortcuts plugin ads a short of quick shortcut key bindings for the 
 * Slots and Items in the character inventory for quick access.
 */

namespace StationeersShortcuts
{
    [BepInPlugin("net.ilo.plugins.ShortCutsPlugin", "ShortCuts Plug-In", "0.9.1.3")]
    public class ShortcutsPlugin : BaseUnityPlugin
    {

        // Backpack shortcuts
        public static KeyCode BPSC2;
        public static KeyCode BPSC3;
        public static KeyCode BPSC4;
        public static KeyCode BPSC5;

        // Suit shortcuts
        public static KeyCode UNSC1;
        public static KeyCode UNSC2;

        // ToolBelt shortcuts
        public static KeyCode TBSC1;
        public static KeyCode TBSC2;
        public static KeyCode TBSC3;
        public static KeyCode TBSC4;

        // Custom tools shortcuts
        public static KeyCode GrinderSC;
        public static KeyCode WelderSC;
        public static KeyCode DrillSC;
        public static KeyCode WrenchSC;
        public static KeyCode CuttersSC;
        public static KeyCode ScrewSC;
        public static KeyCode CrowbarSC;
        public static KeyCode AuthoringSC;

        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            try
            {
                var harmony = new Harmony("net.ilo.plugins.ShortcutsPlugin");
                harmony.PatchAll();
                UnityEngine.Debug.Log("Shortcuts patch succeeded");

                // We can now add an event handler for controls change, so we keep track of 
                // the current key selection 
                KeyManager.OnControlsChanged += new KeyManager.Event(ControlsChangedEvent);

            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("Shortcuts Patch Failed");
                UnityEngine.Debug.Log(e.ToString());
            }
        }

        /* Track current player keybinding selection, event trigger after any 
         * keybinding change.
         */
        private void ControlsChangedEvent()
        {
            UnityEngine.Debug.Log("Keybinding Controls changed");

            // Backpack keybindings
            BPSC2 = KeyManager.GetKey("Backpack 2");
            BPSC3 = KeyManager.GetKey("Backpack 3");
            BPSC4 = KeyManager.GetKey("Backpack 4");
            BPSC5 = KeyManager.GetKey("Backpack 5");

            // Uniform shortcuts 
            UNSC1 = KeyManager.GetKey("Uniform 1");
            UNSC2 = KeyManager.GetKey("Uniform 2");

            // Toolbelt keybindings
            TBSC1 = KeyManager.GetKey("Toolbelt 1");
            TBSC2 = KeyManager.GetKey("Toolbelt 2");
            TBSC3 = KeyManager.GetKey("Toolbelt 3");
            TBSC4 = KeyManager.GetKey("Toolbelt 4");

            // Tools keybindings
            GrinderSC = KeyManager.GetKey("Grinder");
            WelderSC = KeyManager.GetKey("Welder");
            DrillSC = KeyManager.GetKey("Hand Drill"); ;
            WrenchSC = KeyManager.GetKey("Wrench");
            CuttersSC = KeyManager.GetKey("Cable Cutters");
            ScrewSC = KeyManager.GetKey("Screwdriver");
            CrowbarSC = KeyManager.GetKey("Crowbar");
            AuthoringSC = KeyManager.GetKey("Authoring tool");
        }

        // Tracking of user inputs to perform slot swapping.
        
//Kastuk: original ShortcutsPlugin.Update is not running.  
//Supposedly instance got destroyed after Awake got finished, may it need to use DontDestroyOnLoad or something. FOr now I need injection into any other regular game Update to check keys.

    }
        [HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]//, new Type[] { typeof(bool) })]
        class CheckKeysPressed
        {
        static bool warn = true;
        static bool warn2 = true;

        static void Postfix()
        { 

            // Usual suspects, don't check for key pressing in any of these cases
            if (warn) Debug.Log("Update of keys tracker is works"); warn = false;


            if (GameManager.GameState != GameState.Running || (UnityEngine.Object)InventoryManager.ParentBrain == (UnityEngine.Object)null || WorldManager.IsGamePaused)
                return;

            if (warn2) Debug.Log("Update of keys tracker isovercome initial checks"); warn2 = false;

            // this will be the slot from any of the inventory items.
            Slot inventory = null;

            // Backpack slots
            if (KeyManager.GetButtonDown(ShortcutsPlugin.BPSC2))
            {
                UnityEngine.Debug.Log("Backpack 2 selected");
                inventory = FindSlotIdInBackPack(2);
            }
            if (KeyManager.GetButtonDown(ShortcutsPlugin.BPSC3))
            {
                UnityEngine.Debug.Log("Backpack 2 selected");
                inventory = FindSlotIdInBackPack(3);
            }
            if (KeyManager.GetButtonDown(ShortcutsPlugin.BPSC4))
            {
                UnityEngine.Debug.Log("Backpack 4 selected");
                inventory = FindSlotIdInBackPack(4);
            }
            if (KeyManager.GetButtonDown(ShortcutsPlugin.BPSC5))
            {
                UnityEngine.Debug.Log("Backpack 5 selected");
                inventory = FindSlotIdInBackPack(5);
            }

            // Uniform slots
            if (KeyManager.GetButtonDown(ShortcutsPlugin.UNSC1))
            {
                UnityEngine.Debug.Log("Uniform 1 selected");
                inventory = FindSlotIdInUniform(1);
            }
            if (KeyManager.GetButtonDown(ShortcutsPlugin.UNSC2))
            {
                UnityEngine.Debug.Log("Uniform 2 selected");
                inventory = FindSlotIdInUniform(2);
            }


            // toolbelt slots
            if (KeyManager.GetButtonDown(ShortcutsPlugin.TBSC1))
            {
                UnityEngine.Debug.Log("Toolbelt 1 selected");
                inventory = FindSlotIdInToolBelt(1);
            }
            if (KeyManager.GetButtonDown(ShortcutsPlugin.TBSC2))
            {
                UnityEngine.Debug.Log("Toolbelt 2 selected");
                inventory = FindSlotIdInToolBelt(2);
            }
            if (KeyManager.GetButtonDown(ShortcutsPlugin.TBSC3))
            {
                UnityEngine.Debug.Log("Toolbelt 3 selected");
                inventory = FindSlotIdInToolBelt(3);
            }
            if (KeyManager.GetButtonDown(ShortcutsPlugin.TBSC4))
            {
                UnityEngine.Debug.Log("Toolbelt 4 selected");
                inventory = FindSlotIdInToolBelt(4);
            }


            // Custom tool keybindings
            if (KeyManager.GetButtonDown(ShortcutsPlugin.GrinderSC))
            {
                UnityEngine.Debug.Log("Grinder selected");
                inventory = findBeltSlotWithHash(201215010); // grinder
                if (inventory == null)
                    inventory = findBeltSlotWithHash(240174650); // mk2 grinder
            }
            if (KeyManager.GetButtonDown(ShortcutsPlugin.WelderSC))
            {
                UnityEngine.Debug.Log("Welder selected");
                inventory = findBeltSlotWithHash(1385062886); //arc welder
                if (inventory == null)
                    inventory = findBeltSlotWithHash(-2061979347); // mk2 welder
            }

            if (KeyManager.GetButtonDown(ShortcutsPlugin.DrillSC))
            {
                UnityEngine.Debug.Log("Drill selected");
                inventory = findBeltSlotWithHash(2009673399); // hand drill
                if (inventory == null)
                    inventory = findBeltSlotWithHash(324791548); // mk2 drill 
            }

            if (KeyManager.GetButtonDown(ShortcutsPlugin.WrenchSC))
            {
                UnityEngine.Debug.Log("Wrench selected");
                inventory = findBeltSlotWithHash(-1886261558); // wrench
                if (inventory == null)
                    inventory = findBeltSlotWithHash(1862001680); // mk2 wrench 
            }

            if (KeyManager.GetButtonDown(ShortcutsPlugin.CuttersSC))
            {
                UnityEngine.Debug.Log("Cutters selected");
                inventory = findBeltSlotWithHash(1535854074);
                if (inventory == null)
                    inventory = findBeltSlotWithHash(-178893251); // mk2 cutters
            }

            if (KeyManager.GetButtonDown(ShortcutsPlugin.ScrewSC))
            {
                UnityEngine.Debug.Log("Screwdriver selected");
                inventory = findBeltSlotWithHash(687940869);
                if (inventory == null)
                    inventory = findBeltSlotWithHash(-2015613246); // mk2 screwdriver
            }

            if (KeyManager.GetButtonDown(ShortcutsPlugin.CrowbarSC))
            {
                UnityEngine.Debug.Log("Crowbar selected");
                inventory = findBeltSlotWithHash(856108234);
                if (inventory == null)
                    inventory = findBeltSlotWithHash(1440775434); // mk2 crowbar
            }

            if (KeyManager.GetButtonDown(ShortcutsPlugin.AuthoringSC))
            {
                UnityEngine.Debug.Log("Authoring tool selected");
                inventory = findBeltSlotWithHash(789015045);
            }

            // Get current active hand slot
            Slot hand = InventoryManager.ActiveHandSlot;

            // Try whether it is possible 
            switch (IsValid(inventory, hand))
            {
                case KeyResult.Swap:
                    hand.PlayerSwapToSlot(inventory);
                    break;
                case KeyResult.HandToSlot:
                    inventory.PlayerMoveToSlot(hand.Occupant);
                    break;
                case KeyResult.SlotToHand:
                    hand.PlayerMoveToSlot(inventory.Occupant);
                    break;
                case KeyResult.Merge:
                    hand.PlayerMergeToSlot(inventory.Occupant as Stackable);
                    break;
            }

        }

        static Slot FindSlotIdInBackPack(int index)
        {
            Human Character = InventoryManager.ParentHuman;
            if (Character.BackpackSlot.Occupant != null && Character.BackpackSlot.Occupant.Slots.Count > index) 
            {
                return Character.BackpackSlot.Occupant.Slots[index - 1];
            }
            return (Slot)null;
        }

        static Slot FindSlotIdInUniform(int index)
        {
            Human Character = InventoryManager.ParentHuman;
            if (Character.UniformSlot.Occupant != null && Character.UniformSlot.Occupant.Slots.Count > index)
            {
                return Character.UniformSlot.Occupant.Slots[index - 1];
            }
            return (Slot)null;
        }


        static Slot FindSlotIdInToolBelt(int index)
        {
            Human Character = InventoryManager.ParentHuman;
            if (Character.ToolbeltSlot.Occupant != null && Character.ToolbeltSlot.Occupant.Slots.Count > index)
            {
                return Character.ToolbeltSlot.Occupant.Slots[index - 1];
            }
            return (Slot)null;
        }


        static Slot findBeltSlotWithHash(int prefabHash)
        {
            Human Character = InventoryManager.ParentHuman;
            if (Character.ToolbeltSlot.Occupant != null)
            {
                foreach (Slot tool in Character.ToolbeltSlot.Occupant.Slots)
                {
                    if (tool.Occupant != null && tool.Occupant.GetPrefabHash() == prefabHash)
                    {
                        return tool;
                    }
                }
            }
            return (Slot)null;
        }

        static KeyResult IsValid(Slot inventory, Slot hand)
        {
            // No inventory slot selected so nothing to do
            if (InventoryManager.Instance.IsUsingSmartTool || inventory == null)
                return KeyResult.Invalid;

            // Items of both places, swap or merge
            if (inventory.Occupant != null && hand.Occupant != null)
            {
                if (Slot.CanMerge(inventory.Occupant, hand))
                {
                    return KeyResult.Merge;
                }
                return !Slot.AllowSwap(inventory, hand) ? KeyResult.Invalid : KeyResult.Swap;
            }

            // No item in hand, place
            if (inventory.Occupant != null && hand.Occupant == null)
            {
                return !Slot.AllowSwap(hand, inventory) ? KeyResult.Invalid : KeyResult.SlotToHand;
            }

            // No item in inventory, place
            return !Slot.AllowSwap(hand, inventory) ? KeyResult.Invalid : KeyResult.HandToSlot;

        }

    }
}
