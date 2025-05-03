using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Il2CppScheduleOne;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.PlayerScripts.Health;
using static Il2CppScheduleOne.PlayerScripts.PlayerCrimeData;
using Il2CppScheduleOne.NPCs;
using static Il2CppScheduleOne.Console;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Product;
using Il2CppScheduleOne.Employees;
using Il2CppScheduleOne.NPCs.Relation;
using Il2CppScheduleOne.Property;
using Il2CppScheduleOne.UI.Shop;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.Storage;
using Il2CppScheduleOne.GameTime;
using Il2CppScheduleOne.ObjectScripts;

namespace NastyMod_v2.Core
{
    /**
     * Mod
     * 
     * This is the mod class for the NastyMod project.
     * 
     * Author: nastycodes
     * Version: 1.0.0
     */
    public class Mod
    {
        // Singleton instance reference
        public static Mod Instance { get; private set; }

        // Helper instance
        public Helper HelperInstance { get; private set; } = new Helper();

        // Renderer instance
        public UI.Renderer RendererInstance { get; private set; } = new UI.Renderer();

        // Player height
        public float PlayerHeight = 1.9f;

        // Player variables
        public Transform PlayerLocal;
        public bool PlayerInfiniteHealth = Properties.Settings.Default.PlayerInfiniteHealth;
        public bool PlayerInfiniteStamina = Properties.Settings.Default.PlayerInfiniteStamina;
        public bool PlayerNoClip = Properties.Settings.Default.PlayerNoClip;
        public bool PlayerNeverWanted = Properties.Settings.Default.PlayerNeverWanted;
        public float PlayerMoveSpeedMultiplier = Properties.Settings.Default.PlayerMoveSpeedMultiplier;
        public float PlayerDefaultMoveSpeedMultiplier = 1f;
        public float PlayerCrouchSpeedMultiplier = Properties.Settings.Default.PlayerCrouchSpeedMultiplier;
        public float PlayerDefaultCrouchSpeedMultiplier = 0.6f;
        public float PlayerJumpMultiplier = Properties.Settings.Default.PlayerJumpMultiplier;
        public float PlayerDefaultJumpMultiplier = 1f;
        public int PlayerExpAmount = 1000;
        public int PlayerCashAmount = 1000;
        public int PlayerBalanceAmount = 1000;

        // World variables
        public bool WorldNpcEsp = Properties.Settings.Default.WorldNpcEsp;
        public bool WorldPlayerEsp = Properties.Settings.Default.WorldPlayerEsp;
        public float WorldEspRange = Properties.Settings.Default.WorldEspRange;
        public float WorldTimeScale = Properties.Settings.Default.WorldTimeScale;
        public string WorldTime = Properties.Settings.Default.WorldTime;
        public bool WorldFreezeTime = Properties.Settings.Default.WorldFreezeTime;

        // Spawner variables
        public Dictionary<string, Dictionary<string, string>> SpawnerItems = new Dictionary<string, Dictionary<string, string>>();
        public Dictionary<string, List<string>> SpawnerItemsCache = new Dictionary<string, List<string>>();
        public Dictionary<string, bool> SpawnerItemSupportsQualityCache = new Dictionary<string, bool>();
        public Dictionary<string, List<string>> SpawnerItemQualityCache = new Dictionary<string, List<string>>();
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> SpawnerFilterCache = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
        public string SpawnerSelectedCategory = Properties.Settings.Default.SpawnerSelectedCategory;
        public int SpawnerItemAmount = Properties.Settings.Default.SpawnerItemAmount;
        public string SpawnerItemFilter = "";

        // Misc variables
        public Dictionary<string, Supplier> MiscSuppliers = new Dictionary<string, Supplier>();
        public bool MiscUseStackSize = Properties.Settings.Default.MiscUseStackSize;
        public int MiscStackSize = Properties.Settings.Default.MiscStackSize;
        public Dictionary<string, int> MiscStackSizeDefaults = new Dictionary<string, int>();
        public bool MiscUseDealSuccessChance = Properties.Settings.Default.MiscUseDealSuccessChance;
        public float MiscDealSuccessChance = Properties.Settings.Default.MiscDealSuccessChance;
        public bool MiscUseTrashGrabberCapacity = Properties.Settings.Default.MiscUseTrashGrabberCapacity;
        public int MiscTrashGrabberCapacity = Properties.Settings.Default.MiscTrashGrabberCapacity;
        public bool MiscUsePlantGrowSpeedMultiplier = Properties.Settings.Default.MiscUsePlantGrowSpeedMultiplier;
        public float MiscPlantGrowSpeedMultiplier = Properties.Settings.Default.MiscPlantGrowSpeedMultiplier;
        public bool MiscInstantDeadDrop = Properties.Settings.Default.MiscInstantDeadDrop;
        public float LastDeadDropCheck = 0f;
        public bool MiscInstantLaundering = Properties.Settings.Default.MiscInstantLaundering;
        public float LastLaunderingCheck = 0f;
        public bool MiscInstantMixing = Properties.Settings.Default.MiscInstantMixing;
        public float LastMixingCheck = 0f;

        // Employees variables
        public Dictionary<string, Dictionary<string, List<Employee>>> Employees = new Dictionary<string, Dictionary<string, List<Employee>>>();
        public Dictionary<Il2CppSystem.Guid, Botanist> BotanistEmployees = new Dictionary<Il2CppSystem.Guid, Botanist>();
        public Dictionary<Il2CppSystem.Guid, Packager> PackagerEmployees = new Dictionary<Il2CppSystem.Guid, Packager>();
        public Dictionary<Il2CppSystem.Guid, Dictionary<string, float>> EmployeesSpeed = new Dictionary<Il2CppSystem.Guid, Dictionary<string, float>>();
        public Dictionary<string, float> EmployeesDefaultSpeed = new Dictionary<string, float>
        {
            { "Soil Pour Time", 10f },
            { "Water Pour Time", 10f },
            { "Additive Pour Time", 10f },
            { "Seed Sow Time", 15f },
            { "Harvest Time", 15f }
        };
        public string EmployeesSelectedProperty = Properties.Settings.Default.EmployeesSelectedProperty;
        public string EmployeesFilter = "";

        // Teleport variables
        public Dictionary<string, Dictionary<string, Vector3>> TeleportLocations = new Dictionary<string, Dictionary<string, Vector3>>();
        public Dictionary<string, Dictionary<string, Dictionary<string, Vector3>>> TeleportFilterCache = new Dictionary<string, Dictionary<string, Dictionary<string, Vector3>>>();
        public string TeleportSelectedCategory = Properties.Settings.Default.TeleportSelectedCategory;
        public string TeleportLocationFilter = "";

        /**
         * SetInstanceReference
         * 
         * Sets the singleton instance reference.
         * 
         * @return void
         */
        public void SetInstanceReference()
        {
            if (Instance == null)
            {
                Instance = this;
                HelperInstance.SendLoggerMsg("Mod instance reference set!");
            }
            else
            {
                HelperInstance.SendLoggerMsg("Mod instance reference already set!");
            }
        }

        /**
         * FindGameObjectByPath
         * 
         * Finds a game object by its path.
         * 
         * @param path The path to the game object.
         * @return The found game object or null if not found.
         */
        private GameObject FindGameObjectByPath(string path)
        {
            Transform transform = null;
            string[] array = path.Split('/');
            foreach (string text in array)
            {
                if (transform == null)
                {
                    GameObject gameObject = GameObject.Find(text);
                    if (gameObject == null)
                    {
                        return null;
                    }

                    transform = gameObject.transform;
                }
                else
                {
                    transform = transform.Find(text);
                    if (transform == null)
                    {
                        return null;
                    }
                }
            }

            return transform?.gameObject;
        }

        #region Player mods
        /**
         * CheckPlayerMods
         * 
         * Checks all player mods and applies them if necessary.
         * 
         * @return void
         */
        public void CheckPlayerMods()
        {
            if (!PlayerMovement.InstanceExists)
            {
                return;
            }

            if (PlayerInfiniteHealth)
            {
                if (Player.Local.Health.CurrentHealth < PlayerHealth.MAX_HEALTH)
                {
                    Player.Local.Health.RecoverHealth(PlayerHealth.MAX_HEALTH - Player.Local.Health.CurrentHealth);
                }
            }

            if (PlayerInfiniteStamina)
            {
                if (Player.Local.Energy.CurrentEnergy < PlayerEnergy.MAX_ENERGY)
                {
                    Player.Local.Energy.SetEnergy(PlayerEnergy.MAX_ENERGY);
                }

                if (PlayerMovement.Instance.CurrentStaminaReserve < PlayerMovement.StaminaReserveMax)
                {
                    PlayerMovement.Instance.SetStamina(PlayerMovement.StaminaReserveMax);
                }
            }

            if (PlayerNeverWanted)
            {
                if (Player.Local.CrimeData.CurrentPursuitLevel != EPursuitLevel.None)
                {
                    Player.Local.CrimeData.SetPursuitLevel(EPursuitLevel.None);
                }
            }

            if (PlayerNoClip)
            {
                if (Player.Local.CharacterController.enabled) Player.Local.CharacterController.enabled = false;

                Vector3 move = Vector3.zero;

                if (Input.GetKey(KeyCode.W)) move += Vector3.forward;
                if (Input.GetKey(KeyCode.S)) move += Vector3.back;
                if (Input.GetKey(KeyCode.A)) move += Vector3.left;
                if (Input.GetKey(KeyCode.D)) move += Vector3.right;
                if (Input.GetKey(KeyCode.Space)) move += Vector3.up;
                if (Input.GetKey(KeyCode.LeftControl)) move += Vector3.down;

                if (move != Vector3.zero)
                {
                    // Bewegung anhand der horizontalen Blickrichtung
                    Vector3 camForward = Camera.main.transform.forward;
                    Vector3 camRight = Camera.main.transform.right;
                    Vector3 camUp = Vector3.up;

                    Vector3 direction =
                        move.z * Vector3.ProjectOnPlane(camForward, Vector3.up).normalized +
                        move.x * Vector3.ProjectOnPlane(camRight, Vector3.up).normalized +
                        move.y * camUp;

                    Player.Local.transform.position += direction.normalized * 10f * Time.deltaTime;
                }
            }
        }

        /**
         * TogglePlayerInfiniteHealth
         * 
         * Toggles the player infinite health state.
         * 
         * @return void
         */
        public void TogglePlayerInfiniteHealth()
        {
            PlayerInfiniteHealth = !PlayerInfiniteHealth;
            Properties.Settings.Default.PlayerInfiniteHealth = PlayerInfiniteHealth;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"Player - Infinite Health toggled! (now {PlayerInfiniteHealth})");
        }

        /**
         * TogglePlayerInfiniteStamina
         * 
         * Toggles the player infinite stamina state.
         * 
         * @return void
         */
        public void TogglePlayerInfiniteStamina()
        {
            PlayerInfiniteStamina = !PlayerInfiniteStamina;
            Properties.Settings.Default.PlayerInfiniteStamina = PlayerInfiniteStamina;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"Player - Infinite Stamina toggled! (now {PlayerInfiniteStamina})");
        }

        /**
         * TogglePlayerNeverWanted
         * 
         * Toggles the player never wanted state.
         * 
         * @return void
         */
        public void TogglePlayerNeverWanted()
        {
            PlayerNeverWanted = !PlayerNeverWanted;
            Properties.Settings.Default.PlayerNeverWanted = PlayerNeverWanted;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"Player - Never Wanted toggled! (now {PlayerNeverWanted})");
        }

        /**
         * TogglePlayerNoClip
         * 
         * Toggles the player no clip state.
         * 
         * @return void
         */
        public void TogglePlayerNoClip()
        {
            PlayerNoClip = !PlayerNoClip;

            Properties.Settings.Default.PlayerNoClip = PlayerNoClip;
            Properties.Settings.Default.Save();

            Player.Local.CharacterController.enabled = !PlayerNoClip;

            HelperInstance.SendLoggerMsg($"Player - No Clip toggled! (now {PlayerNoClip})");
        }

        /**
         * SetPlayerMoveSpeedMultiplier
         * 
         * Sets the player move speed multiplier.
         * 
         * @return void
         */
        public void SetPlayerMoveSpeedMultiplier()
        {
            if (PlayerMoveSpeedMultiplier != Properties.Settings.Default.PlayerMoveSpeedMultiplier)
            {
                Properties.Settings.Default.PlayerMoveSpeedMultiplier = PlayerMoveSpeedMultiplier;
                Properties.Settings.Default.Save();
            }

            if (PlayerMovement.Instance.MoveSpeedMultiplier != PlayerMoveSpeedMultiplier)
            {
                PlayerMovement.Instance.MoveSpeedMultiplier = PlayerMoveSpeedMultiplier;
                HelperInstance.SendLoggerMsg($"Player - Move Speed Multiplier set to {PlayerMoveSpeedMultiplier}");
            }
        }

        /**
         * ResetPlayerMoveSpeedMultiplier
         * 
         * Resets the player move speed multiplier to default.
         * 
         * @return float Move speed multiplier
         */
        public float ResetPlayerMoveSpeedMultiplier()
        {
            PlayerMoveSpeedMultiplier = 1f;

            SetPlayerMoveSpeedMultiplier();

            HelperInstance.SendLoggerMsg($"Player - Move Speed Multiplier reset to {PlayerMoveSpeedMultiplier}");

            return PlayerMoveSpeedMultiplier;
        }

        /**
         * SetPlayerCrouchSpeedMultiplier
         * 
         * Sets the player crouch speed multiplier.
         * 
         * @return void
         */
        public void SetPlayerCrouchSpeedMultiplier()
        {
            if (PlayerCrouchSpeedMultiplier != Properties.Settings.Default.PlayerCrouchSpeedMultiplier)
            {
                Properties.Settings.Default.PlayerCrouchSpeedMultiplier = PlayerCrouchSpeedMultiplier;
                Properties.Settings.Default.Save();
            }

            if (PlayerMovement.Instance.crouchSpeedMultipler != PlayerCrouchSpeedMultiplier)
            {
                PlayerMovement.Instance.crouchSpeedMultipler = PlayerCrouchSpeedMultiplier;
                HelperInstance.SendLoggerMsg($"Player - Crouch Speed Multiplier set to {PlayerCrouchSpeedMultiplier}");
            }
        }

        /**
         * ResetPlayerCrouchSpeedMultiplier
         * 
         * Resets the player crouch speed multiplier to default.
         * 
         * @return float Crouch speed multiplier
         */
        public float ResetPlayerCrouchSpeedMultiplier()
        {
            PlayerCrouchSpeedMultiplier = 0.6f;

            SetPlayerCrouchSpeedMultiplier();

            HelperInstance.SendLoggerMsg($"Player - Crouch Speed Multiplier reset to {PlayerCrouchSpeedMultiplier}");

            return PlayerCrouchSpeedMultiplier;
        }

        /**
         * SetPlayerJumpMultiplier
         * 
         * Sets the player jump multiplier.
         * 
         * @return void
         */
        public void SetPlayerJumpMultiplier()
        {
            if (PlayerJumpMultiplier != Properties.Settings.Default.PlayerJumpMultiplier)
            {
                Properties.Settings.Default.PlayerJumpMultiplier = PlayerJumpMultiplier;
                Properties.Settings.Default.Save();
            }

            if (PlayerMovement.JumpMultiplier != PlayerJumpMultiplier)
            {
                PlayerMovement.JumpMultiplier = PlayerJumpMultiplier;
                HelperInstance.SendLoggerMsg($"Player - Jump Multiplier set to {PlayerJumpMultiplier}");
            }
        }

        /**
         * ResetPlayerJumpMultiplier
         * 
         * Resets the player jump multiplier to default.
         * 
         * @return float Player jump multiplier
         */
        public float ResetPlayerJumpMultiplier()
        {
            PlayerJumpMultiplier = 1f;

            SetPlayerJumpMultiplier();

            HelperInstance.SendLoggerMsg($"Player - Jump Multiplier reset to {PlayerJumpMultiplier}");

            return PlayerJumpMultiplier;
        }

        /**
         * AddPlayerExp
         * 
         * Adds a previously set amount the player exp by a previously set amount.
         * 
         * @param Type The type of change to apply. (add, remove)
         * @return void
         */
        public void AddPlayerExp()
        {
            if (PlayerExpAmount > 0)
            {
                GiveXP GiveExpCommand = new GiveXP();
                Il2CppSystem.Collections.Generic.List<string> ExpCommandArguments = new Il2CppSystem.Collections.Generic.List<string>();
                ExpCommandArguments.Add(PlayerExpAmount.ToString());
                GiveExpCommand.Execute(ExpCommandArguments);

                HelperInstance.SendLoggerMsg($"Player - Added {PlayerExpAmount} exp");
            }
        }

        /**
         * ChangePlayerCash
         * 
         * Changes the player cash by a previously set amount.
         * 
         * @param Type The type of change to apply. (add, remove)
         * @return void
         */
        public void ChangePlayerCash(string Type = "add")
        {
            if (PlayerCashAmount > 0)
            {
                string Amount = Type == "add" ? PlayerCashAmount.ToString() : "-" + PlayerCashAmount.ToString();

                ChangeCashCommand ChangeCashCommand = new ChangeCashCommand();
                Il2CppSystem.Collections.Generic.List<string> CashCommandArguments = new Il2CppSystem.Collections.Generic.List<string>();
                CashCommandArguments.Add(Amount);
                ChangeCashCommand.Execute(CashCommandArguments);

                HelperInstance.SendLoggerMsg($"Player - Changed cash by {PlayerCashAmount}");
            }
        }

        /**
         * AddPlayerCash
         * 
         * Adds a previously set amount to the player cash.
         * 
         * @return void
         */
        public void AddPlayerCash()
        {
            ChangePlayerCash();
        }

        /**
         * RemovePlayerCash
         * 
         * Removes a previously set amount from the player cash.
         * 
         * @return void
         */
        public void RemovePlayerCash()
        {
            ChangePlayerCash("!");
        }

        /**
         * ChangePlayerBalance
         * 
         * Changes the online balance by a previously set amount.
         * 
         * @param Type The type of change to apply. (add, remove)
         * @return void
         */
        public void ChangePlayerBalance(string Type = "add")
        {
            if (PlayerBalanceAmount > 0)
            {
                string Amount = Type == "add" ? PlayerBalanceAmount.ToString() : "-" + PlayerBalanceAmount.ToString();

                ChangeOnlineBalanceCommand ChangeOnlineBalanceCommand = new ChangeOnlineBalanceCommand();
                Il2CppSystem.Collections.Generic.List<string> ChangeOnlineBalanceCommandArguments = new Il2CppSystem.Collections.Generic.List<string>();
                ChangeOnlineBalanceCommandArguments.Add(Amount);
                ChangeOnlineBalanceCommand.Execute(ChangeOnlineBalanceCommandArguments);

                HelperInstance.SendLoggerMsg($"Player - Changed balance by {PlayerBalanceAmount}");
            }
        }

        /**
         * AddPlayerBalance
         * 
         * Adds a previously set amount to the online balance.
         * 
         * @return void
         */
        public void AddPlayerBalance()
        {
            ChangePlayerBalance();
        }

        /**
         * RemovePlayerBalance
         * 
         * Removes a previously set amount from the online balance.
         * 
         * @return void
         */
        public void RemovePlayerBalance()
        {
            ChangePlayerBalance("!");
        }
        #endregion

        #region World mods
        /**
         * CheckWorldGuiMods
         * 
         * Checks all world GUI mods and applies them if necessary.
         * 
         * @return void
         */
        public void CheckWorldGuiMods()
        {
            RenderWorldNpcEsp();
        }

        /**
         * ToggleWorldNpcEsp
         * 
         * Toggles the NPC ESP.
         * 
         * @return void
         */
        public void ToggleWorldNpcEsp()
        {
            WorldNpcEsp = !WorldNpcEsp;

            Properties.Settings.Default.WorldNpcEsp = WorldNpcEsp;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"World - NPC ESP toggled! (now {WorldNpcEsp})");
        }

        /**
         * RenderWorldNpcEsp
         * 
         * Renders the NPC ESP.
         * 
         * @return void
         */
        public void RenderWorldNpcEsp()
        {
            if (WorldNpcEsp)
            {
                foreach (NPC npc in NPCManager.NPCRegistry)
                {
                    // Get NPC height from mesh bounds
                    MeshRenderer meshRenderer = npc.GetComponent<MeshRenderer>();
                    float npcHeight = meshRenderer != null ? meshRenderer.bounds.size.y : 1.9f;

                    Vector3 pivotPos = npc.gameObject.transform.position;
                    Vector3 playerHeadPos = pivotPos;
                    playerHeadPos.y += npcHeight;

                    Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);
                    Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(pivotPos);

                    if (w2s_footpos.x < 0 || w2s_footpos.x > Screen.width ||
                        w2s_footpos.y < 0 || w2s_footpos.y > Screen.height ||
                        w2s_headpos.x < 0 || w2s_headpos.x > Screen.width ||
                        w2s_headpos.y < 0 || w2s_headpos.y > Screen.height)
                    {
                        continue;
                    }

                    if (w2s_footpos.z > 0f && w2s_footpos.z < WorldEspRange)
                    {
                        RendererInstance.DrawSkewedBox(w2s_footpos, w2s_headpos, Color.red);
                    }
                }
            }
        }

        /**
         * ToggleWorldPlayerEsp
         * 
         * Toggles the Player ESP.
         * 
         * @return void
         */
        public void ToggleWorldPlayerEsp()
        {
            WorldPlayerEsp = !WorldPlayerEsp;

            Properties.Settings.Default.WorldPlayerEsp = WorldPlayerEsp;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"World - Player ESP toggled! (now {WorldPlayerEsp})");
        }

        /**
         * RenderWorldPlayerEsp
         * 
         * Renders the Player ESP.
         * 
         * @return void
         */
        public void RenderWorldPlayerEsp()
        {
            if (WorldPlayerEsp)
            {
                foreach (Player player in Player.PlayerList)
                {
                    // Get Player height from mesh bounds
                    MeshRenderer meshRenderer = player.GetComponent<MeshRenderer>();
                    float playerHeight = meshRenderer != null ? meshRenderer.bounds.size.y : 1.9f;

                    Vector3 pivotPos = player.gameObject.transform.position;
                    Vector3 playerHeadPos = pivotPos;
                    playerHeadPos.y += playerHeight;

                    Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(pivotPos);
                    Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);

                    if (w2s_footpos.x < 0 || w2s_footpos.x > Screen.width ||
                        w2s_footpos.y < 0 || w2s_footpos.y > Screen.height ||
                        w2s_headpos.x < 0 || w2s_headpos.x > Screen.width ||
                        w2s_headpos.y < 0 || w2s_headpos.y > Screen.height)
                    {
                        continue;
                    }

                    if (w2s_footpos.z > 0f && w2s_footpos.z < WorldEspRange)
                    {
                        RendererInstance.DrawSkewedBox(w2s_footpos, w2s_headpos, Color.blue);
                    }
                }
            }
        }

        /**
         * SetWorldEspRange
         * 
         * Sets the ESP range.
         * 
         * @return void
         */
        public void SetWorldEspRange()
        {
            if (WorldEspRange != Properties.Settings.Default.WorldEspRange)
            {
                Properties.Settings.Default.WorldEspRange = WorldEspRange;
                Properties.Settings.Default.Save();
            }

            HelperInstance.SendLoggerMsg($"World - ESP Range set to {WorldEspRange}");
        }

        /**
         * ResetWorldEspRange
         * 
         * Resets the ESP range to default.
         * 
         * @return float ESP range
         */
        public float ResetWorldEspRange()
        {
            WorldEspRange = 100f;

            Properties.Settings.Default.WorldEspRange = WorldEspRange;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"World - ESP Range reset to {WorldEspRange}");

            return WorldEspRange;
        }

        /**
         * SetWorldTimeScale
         * 
         * Sets the world time scale.
         * 
         * @return void
         */
        public void SetWorldTimeScale()
        {
            var CommandArguments = new Il2CppSystem.Collections.Generic.List<string>();
            CommandArguments.Add(WorldTimeScale.ToString());

            var Command = new SetTimeScale();
            Command.Execute(CommandArguments);

            HelperInstance.SendLoggerMsg($"World - Time Scale set to {WorldTimeScale}");
        }

        /**
         * ResetWorldTimeScale
         * 
         * Resets the world time scale to default.
         * 
         * @return float World time scale
         */
        public float ResetWorldTimeScale()
        {
            var CommandArguments = new Il2CppSystem.Collections.Generic.List<string>();
            CommandArguments.Add("1");

            var Command = new SetTimeScale();
            Command.Execute(CommandArguments);

            HelperInstance.SendLoggerMsg($"World - Time Scale reset to 1");

            return 1f;
        }

        /**
         * ToggleWorldFreezeTime
         * 
         * Toggles the world freeze time state.
         * 
         * @return void
         */
        public void ToggleWorldFreezeTime()
        {
            WorldFreezeTime = !WorldFreezeTime;

            Properties.Settings.Default.WorldFreezeTime = WorldFreezeTime;
            Properties.Settings.Default.Save();

            if (TimeManager.InstanceExists) TimeManager.Instance.TimeProgressionMultiplier = WorldFreezeTime ? 0f : 1f;

            HelperInstance.SendLoggerMsg($"World - Freeze Time toggled! (now {WorldFreezeTime})");
        }

        /**
         * SetWorldTime
         * 
         * Sets the world time.
         * 
         * @return void
         */
        public void SetWorldTime()
        {
            var CommandArguments = new Il2CppSystem.Collections.Generic.List<string>();
            CommandArguments.Add(WorldTime.ToString());

            var Command = new SetTimeCommand();
            Command.Execute(CommandArguments);

            HelperInstance.SendLoggerMsg($"World - Time set to {WorldTime}");
        }

        /**
         * WorldTimeForwardHour
         * 
         * Forwards the world time by one hour.
         * 
         * @return void
         */
        public void WorldTimeForwardHour()
        {
            if (!TimeManager.InstanceExists) return;

            var CurrentTime = TimeManager.Instance.CurrentTime;
            var NewTime = TimeManager.AddMinutesTo24HourTime(CurrentTime, 60);
            TimeManager.Instance.SetTime(NewTime);
        }

        /**
         * WorldTimeForwardHour
         * 
         * Forwards the world time by one hour.
         * 
         * @return void
         */
        public void WorldTimeBackwardHour()
        {
            if (!TimeManager.InstanceExists) return;

            var CurrentTime = TimeManager.Instance.CurrentTime;
            var NewTime = TimeManager.AddMinutesTo24HourTime(CurrentTime, -60);
            TimeManager.Instance.SetTime(NewTime);
        }
        #endregion

        #region Spawner mods
        /**
         * CacheGameItems
         * 
         * Caches all game items and their properties.
         * 
         * @return void
         */
        public void CacheGameItems()
        {
            try
            {
                // Create dictionaries to store discovered items
                var _SpawnerItems = new Dictionary<string, Dictionary<string, string>>();
                var _SpawnerItemSupportsQualityCache = new Dictionary<string, bool>();
                var _SpawnerItemQualityCache = new Dictionary<string, List<string>>();

                // Get the registry
                var _Registry = Registry.Instance;
                if (_Registry == null)
                {
                    HelperInstance.SendLoggerMsg("Registry instance is NULL - aborting CacheGameItems");
                    return;
                }

                // Get quality names for reference
                var QualityNames = Enum.GetNames(typeof(EQuality)).ToList();

                // Get ProductManager instance to access drug product definitions
                var _ProductManager = ProductManager.Instance;
                if (_ProductManager == null)
                {
                    HelperInstance.SendLoggerMsg("ProductManager instance is NULL");
                }

                // Create a managed list to store all products
                var AllProducts = new List<ProductDefinition>();
                if (_ProductManager != null)
                {
                    // Add all products from different sources to our local list
                    // Handle Il2Cpp collections properly
                    if (_ProductManager.AllProducts != null)
                    {
                        for (int i = 0; i < _ProductManager.AllProducts.Count; i++)
                        {
                            AllProducts.Add(_ProductManager.AllProducts[i]);
                        }
                    }

                    // Get discovered products
                    if (ProductManager.DiscoveredProducts != null)
                    {
                        for (int i = 0; i < ProductManager.DiscoveredProducts.Count; i++)
                        {
                            AllProducts.Add(ProductManager.DiscoveredProducts[i]);
                        }
                    }

                    // Get default known products
                    if (_ProductManager.DefaultKnownProducts != null)
                    {
                        for (int i = 0; i < _ProductManager.DefaultKnownProducts.Count; i++)
                        {
                            AllProducts.Add(_ProductManager.DefaultKnownProducts[i]);
                        }
                    }

                    // Remove duplicates - using a dictionary to track unique items by ID
                    var uniqueProducts = new Dictionary<string, ProductDefinition>();
                    foreach (var product in AllProducts)
                    {
                        if (!uniqueProducts.ContainsKey(product.ID))
                        {
                            uniqueProducts[product.ID] = product;
                        }
                    }

                    AllProducts = uniqueProducts.Values.ToList();
                    HelperInstance.SendLoggerMsg($"Found {AllProducts.Count} product definitions from ProductManager");
                }

                _SpawnerItems["All"] = new Dictionary<string, string>();

                // Enumerate all items in the registry
                foreach (var entry in _Registry.ItemDictionary)
                {
                    try
                    {
                        var itemDefinition = entry.Value.Definition;

                        // Skip null definitions
                        if (itemDefinition == null) continue;

                        // Try to get item ID and name
                        string ItemId = itemDefinition.ID;
                        string ItemName = itemDefinition.Name;
                        string ItemCategory = itemDefinition.Category.ToString() ?? "Uncategorized";

                        // Skip items that belong to the "Cash" category
                        if (ItemCategory == "Cash") continue;

                        // Fill the category dictionary if it doesn't exist
                        if (!_SpawnerItems.ContainsKey(ItemCategory))
                        {
                            _SpawnerItems[ItemCategory] = new Dictionary<string, string>();
                        }

                        // Determine if this is a quality item
                        bool IsQualityItem = false;
                        List<string> SupportedQualities = null;

                        // Check 1: Explicit QualityItemDefinition type
                        var QualityDefinition = itemDefinition as QualityItemDefinition;
                        if (QualityDefinition != null)
                        {
                            IsQualityItem = true;
                            SupportedQualities = QualityNames;
                        }

                        // Check 2: Check if this is a drug product by comparing with ProductManager items
                        if (!IsQualityItem && _ProductManager != null)
                        {
                            // Try to find matching product in the product list
                            ProductDefinition MatchingProduct = null;
                            foreach (var product in AllProducts)
                            {
                                if (product.ID.Equals(ItemId, StringComparison.OrdinalIgnoreCase) ||
                                    product.Name.Equals(ItemName, StringComparison.OrdinalIgnoreCase))
                                {
                                    MatchingProduct = product;
                                    break;
                                }
                            }

                            if (MatchingProduct != null)
                            {
                                IsQualityItem = true;

                                // Determine drug type and available qualities based on product type
                                string drugType = MatchingProduct.DrugType.ToString();

                                // Log product properties if available
                                if (MatchingProduct.Properties != null && MatchingProduct.Properties.Count > 0)
                                {
                                    // Handle Il2Cpp List without using LINQ
                                    var PropertyNames = new List<string>();
                                    for (int i = 0; i < MatchingProduct.Properties.Count; i++)
                                    {
                                        var Property = MatchingProduct.Properties[i];
                                        PropertyNames.Add(Property.Name);
                                    }

                                }

                                // For now, we'll use all quality levels, but this could be refined based on the product type
                                SupportedQualities = QualityNames;
                            }
                        }

                        if (!string.IsNullOrEmpty(ItemId) && !string.IsNullOrEmpty(ItemName))
                        {
                            // Add to main dictionary (display name => ID)
                            _SpawnerItems[ItemCategory][ItemName] = ItemId;
                            _SpawnerItems["All"][ItemName] = ItemId;

                            // Track quality support
                            _SpawnerItemSupportsQualityCache[ItemId] = IsQualityItem;

                            // If it's a quality item, cache quality levels
                            if (IsQualityItem)
                            {
                                _SpawnerItemQualityCache[ItemId] = SupportedQualities ?? QualityNames;
                            }
                        }
                    }
                    catch (Exception innerEx)
                    {
                        HelperInstance.SendLoggerMsg($"Error processing item entry: {innerEx.Message}");
                    }
                }

                // Update class-level caches
                SpawnerItems = _SpawnerItems;
                SpawnerItemSupportsQualityCache = _SpawnerItemSupportsQualityCache;
                SpawnerItemQualityCache = _SpawnerItemQualityCache;

                // Prepare standard caches
                SpawnerItemsCache["Qualities"] = QualityNames;
                SpawnerItemsCache["Items"] = SpawnerItems.Keys.OrderBy(k => k).ToList();
                SpawnerItemsCache["Slots"] = Enumerable.Range(1, 9).Select(x => x.ToString()).ToList();

                // Count quality items using standard .NET method
                int qualityItemCount = 0;
                foreach (var kvp in SpawnerItemSupportsQualityCache)
                {
                    if (kvp.Value) qualityItemCount++;
                }

                HelperInstance.SendLoggerMsg($"Cached all game items: {SpawnerItems.Count} total, {qualityItemCount} supporting quality");
            }
            catch (Exception ex)
            {
                HelperInstance.SendLoggerMsg($"Critical error in item discovery: {ex}");
            }
        }

        /**
         * GetSpawnerCategories
         * 
         * Returns a list of all spawner categories.
         * 
         * @return List<string> A list of all spawner categories.
         */
        public List<string> GetSpawnerCategories()
        {
            return SpawnerItems.Keys.ToList();
        }

        /**
         * GetSpawnerCategoryItems
         * 
         * Returns a list of all items in a given spawner category.
         * 
         * @param Category The category to get items from.
         */
        public Dictionary<string, string> GetSpawnerCategoryItems(string category)
        {
            if (SpawnerItems.ContainsKey(category))
            {
                return SpawnerItems[category];
            }
            else
            {
                HelperInstance.SendLoggerMsg($"Category '{category}' not found in SpawnerItems.");
                return new Dictionary<string, string>();
            }
        }

        /**
         * FilterSpawnerItems
         * 
         * Filters the spawner items based on a given filter string.
         * 
         * @param Category The category to filter items from.
         * @param Filter Filter string to use.
         * @param Items Items to filter from.
         * @return Dictionary<string, string> Dictionary of filtered items.
         */
        public Dictionary<string, string> FilterSpawnerItems(string Category, string Filter, Dictionary<string, string> Items)
        {
            // Check if the category exists in the SpawnerFilterCache
            if (!SpawnerFilterCache.ContainsKey(Category)) SpawnerFilterCache[Category] = new Dictionary<string, Dictionary<string, string>>();

            // Check and return the filter if it exists
            if (SpawnerFilterCache[Category].ContainsKey(Filter))return SpawnerFilterCache[Category][Filter];

            // Create a new dictionary for the filtered items
            SpawnerFilterCache[Category][Filter] = new Dictionary<string, string>();
            Dictionary<string, string> FilteredItems = new Dictionary<string, string>();

            // Filter the items based on the filter string
            foreach (var Item in Items)
            {
                var FilterMatch = Item.Value.ToLower().Trim().Contains(Filter.ToLower().Trim());
                // MelonLogger.Msg($"Checking if item \"{Item.Value.ToLower()}\" contains \"{Filter.ToLower()}\"");

                if (FilterMatch) FilteredItems[Item.Key] = Item.Value;
            }

            // Cache the filtered items
            SpawnerFilterCache[Category][Filter] = FilteredItems;

            return FilteredItems;
        }

        /**
         * SpawnItem
         * 
         * Spawns an item in the game.
         * 
         * @param ItemId The ID of the item to spawn.
         * @param ItemAmount The amount of the item to spawn.
         * @return void
         */
        public void SpawnItem(string ItemId, int ItemAmount)
        {
            AddItemToInventoryCommand AddItemCommand = new AddItemToInventoryCommand();
            Il2CppSystem.Collections.Generic.List<string> AddItemArguments = new Il2CppSystem.Collections.Generic.List<string>();
            AddItemArguments.Add(ItemId);
            AddItemArguments.Add(ItemAmount.ToString());
            AddItemCommand.Execute(AddItemArguments);

            HelperInstance.SendLoggerMsg($"Spawned {ItemAmount} of item {ItemId}");
        }
        #endregion

        #region Misc mods
        /**
         * CheckMiscMods
         * 
         * Checks all misc mods and applies them if necessary.
         * 
         * @return void
         */
        public void CheckMiscMods()
        {
            if (MiscInstantDeadDrop)
            {
                if (Time.time - LastDeadDropCheck > 5f)
                {
                    LastDeadDropCheck = Time.time;

                    var Suppliers = GameObject.FindObjectsOfType<Supplier>();
                    foreach (var Supplier in Suppliers)
                    {
                        if (Supplier != null && Supplier._minsUntilDeaddropReady_k__BackingField != -1)
                        {
                            Supplier._minsUntilDeaddropReady_k__BackingField = -1;
                            HelperInstance.SendLoggerMsg($"Supplier {Supplier.fullName} - Dead drop ready");
                        }
                    }
                }
            }

            if (MiscInstantLaundering)
            {
                if (Time.time - LastLaunderingCheck > 5f)
                {
                    LastLaunderingCheck = Time.time;
                    var Businesses = BusinessManager.FindObjectsOfType<Business>();
                    foreach (var Business in Businesses)
                    {
                        var LaunderingOperations = Business.LaunderingOperations;
                        foreach (var LaunderingOperation in LaunderingOperations)
                        {
                            if (LaunderingOperation != null)
                            {
                                LaunderingOperation.minutesSinceStarted = LaunderingOperation.completionTime_Minutes;
                                HelperInstance.SendLoggerMsg($"Business {Business.PropertyName} - Laundering operation completed");
                            }
                        }
                    }
                }
            }

            if (MiscInstantMixing)
            {
                if (Time.time - LastMixingCheck > 5f)
                {
                    LastMixingCheck = Time.time;
                    
                    var MixingStations = GameObject.FindObjectsOfType<MixingStation>();
                    foreach (var MixingStation in MixingStations)
                    {
                        if (MixingStation._CurrentMixOperation_k__BackingField != null)
                        {
                            MixingStation.TimeSkipped(240);
                            HelperInstance.SendLoggerMsg($"Mixing Station - Mixing done");
                        }
                    }
                }
            }
        }

        /**
         * CacheStackSizes
         * 
         * Caches the stack sizes for all items in the game.
         * 
         * @return void
         */
        public void CacheStackSizes()
        {
            ItemDefinition[] array = Resources.FindObjectsOfTypeAll<ItemDefinition>();
            foreach (ItemDefinition itemDefinition in array)
            {
                if (itemDefinition != null && itemDefinition.StackLimit < 255)
                {
                    // Check if the item is already in the dictionary
                    if (!MiscStackSizeDefaults.ContainsKey(itemDefinition.ID))
                    {
                        // Add the item to the dictionary with its stack size
                        MiscStackSizeDefaults[itemDefinition.ID] = itemDefinition.StackLimit;
                    }
                }
            }

            HelperInstance.SendLoggerMsg($"Cached stack sizes for {MiscStackSizeDefaults.Count} items");
        }

        /**
        * SetMiscStackSize
        * 
        * Sets the stack size for items in the game.
        *
        * @return void
        */
        public void SetMiscStackSize() {
            if (MiscStackSize != Properties.Settings.Default.MiscStackSize)
            {
                Properties.Settings.Default.MiscStackSize = MiscStackSize;
                Properties.Settings.Default.Save();

                if (MiscUseStackSize)
                {
                    // Set the stack size for all items
                    ItemDefinition[] array = Resources.FindObjectsOfTypeAll<ItemDefinition>();
                    foreach (ItemDefinition itemDefinition in array)
                    {
                        if (itemDefinition != null)
                        {
                            itemDefinition.StackLimit = MiscStackSize;
                        }
                    }
                }

                HelperInstance.SendLoggerMsg($"Misc - Stack Size set to {MiscStackSize}");
            }
        }

        /**
         * ResetMiscStackSize
         * 
         * Resets the stack size for items in the game to default.
         *
         * @return void
         */
        public void ResetMiscStackSize() {
            var Value = GetDefaultMiscStackSize();

            if (Properties.Settings.Default.MiscStackSize != Value)
            {
                Properties.Settings.Default.MiscStackSize = Value;
                Properties.Settings.Default.Save();

                HelperInstance.SendLoggerMsg($"Misc - Stack Size reset to {MiscStackSize}");
            }
        }

        /**
         * GetDefaultMiscStackSize
         * 
         * Gets the default stack size.
         *
         * @return int The default stack size.
         */
        public int GetDefaultMiscStackSize() {
            return 20;
        }

        /**
         * ToggleMiscStackSize
         * 
         * Toggles the stack size for items in the game.
         * 
         * @return void
         */
        public void ToggleMiscStackSize()
        {
            MiscUseStackSize = !MiscUseStackSize;

            // Reset the stack size for all items
            ItemDefinition[] array = Resources.FindObjectsOfTypeAll<ItemDefinition>();
            foreach (ItemDefinition _ItemDefinition in array)
            {
                if (_ItemDefinition != null)
                {
                    if (MiscUseStackSize)
                    {
                        // Set the stack size to the default value
                        _ItemDefinition.StackLimit = MiscStackSize;
                    }
                    else
                    {
                        // Reset the stack size to the original value
                        if (MiscStackSizeDefaults.ContainsKey(_ItemDefinition.ID))
                            _ItemDefinition.StackLimit = MiscStackSizeDefaults[_ItemDefinition.ID];
                    }
                }
            }

            HelperInstance.SendLoggerMsg($"Misc - Stack Size toggled! (now {MiscUseStackSize})");
        }

        /**
         * SetMiscDealSuccessChance
         * 
         * Sets the deal success chance for items in the game.
         *
         * @return void
         */
        public void SetMiscDealSuccessChance() {
            if (Properties.Settings.Default.MiscDealSuccessChance != MiscDealSuccessChance)
            {
                Properties.Settings.Default.MiscDealSuccessChance = MiscDealSuccessChance;
                Properties.Settings.Default.Save();

                HelperInstance.SendLoggerMsg($"Misc - Deal Success Chance set to {MiscDealSuccessChance}");
            }
        }

        /**
         * ResetMiscDealSuccessChance
         * 
         * Resets the deal success chance for items in the game to default.
         *
         * @return void
         */
        public void ResetMiscDealSuccessChance() {
            var Value = GetDefaultMiscDealSuccessChance();

            if (Properties.Settings.Default.MiscDealSuccessChance != Value)
            {
                Properties.Settings.Default.MiscDealSuccessChance = Value;
                Properties.Settings.Default.Save();

                HelperInstance.SendLoggerMsg($"Misc - Deal Success Chance reset to {MiscDealSuccessChance}");
            }
        }

        /**
         * GetDefaultMiscDealSuccessChance
         * 
         * Gets the default deal success chance.
         *
         * @return float The default deal success chance.
         */
        public float GetDefaultMiscDealSuccessChance() {
            return 75f;
        }

        /**
         * ToggleMiscDealSuccessChance
         * 
         * Toggles the deal success chance for items in the game.
         * 
         * @return void
         */
        public void ToggleMiscDealSuccessChance()
        {
            MiscUseDealSuccessChance = !MiscUseDealSuccessChance;

            HelperInstance.SendLoggerMsg($"Misc - Deal Success Chance toggled! (now {MiscUseDealSuccessChance})");
        }

        /**
         * MiscDealSuccessChancePatch
         * 
         * HarmonyPatch - Patches the deal success chance for items in the game.
         * 
         * @return void
         */
        [HarmonyPatch]
        public class MiscDealSuccessChancePatch
        {
            [HarmonyPatch(typeof(Customer), "GetOfferSuccessChance")]
            [HarmonyPostfix]
            public static void Postfix(ref float __result, List<ItemInstance> items, float askingPrice)
            {
                if (Mod.Instance != null && Mod.Instance.MiscUseDealSuccessChance)
                {
                    __result = Mod.Instance.MiscDealSuccessChance / 100;
                }
            }
        }

        /**
         * SetMiscPlantGrowSpeed
         * 
         * Sets the deal success chance for items in the game.
         *
         * @return void
         */
        public void SetMiscPlantGrowSpeed()
        {
            if (Properties.Settings.Default.MiscPlantGrowSpeedMultiplier != MiscPlantGrowSpeedMultiplier)
            {
                Properties.Settings.Default.MiscPlantGrowSpeedMultiplier = MiscPlantGrowSpeedMultiplier;
                Properties.Settings.Default.Save();

                HelperInstance.SendLoggerMsg($"Misc - Plant Grow Speed set to {MiscPlantGrowSpeedMultiplier}");
            }
        }

        /**
         * ResetMiscPlantGrowSpeed
         * 
         * Resets the deal success chance for items in the game to default.
         *
         * @return float
         */
        public float ResetMiscPlantGrowSpeed()
        {
            var Value = 1f;

            if (Properties.Settings.Default.MiscPlantGrowSpeedMultiplier != Value)
            {
                Properties.Settings.Default.MiscPlantGrowSpeedMultiplier = Value;
                Properties.Settings.Default.Save();

                HelperInstance.SendLoggerMsg($"Misc - Deal Success Chance reset to {MiscPlantGrowSpeedMultiplier}");
            }

            return Value;
        }

        /**
         * ToggleMiscPlantGrowSpeed
         * 
         * Toggles the deal success chance for items in the game.
         * 
         * @return void
         */
        public void ToggleMiscPlantGrowSpeed()
        {
            MiscUsePlantGrowSpeedMultiplier = !MiscUsePlantGrowSpeedMultiplier;

            HelperInstance.SendLoggerMsg($"Misc - Growth speed toggled! (now {MiscUsePlantGrowSpeedMultiplier})");
        }

        /**
         * MiscGrowthSpeedPatch
         * 
         * HarmonyPatch - Patches the growth speed for items in the game.
         * 
         * @return void
         */
        [HarmonyPatch(typeof(Pot), "GetAdditiveGrowthMultiplier")]
        public class PlantGetAdditiveGrowthMultiplierPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref float __result)
            {
                if (Mod.Instance != null && Mod.Instance.MiscUsePlantGrowSpeedMultiplier)
                {
                    __result *= Mod.Instance.MiscPlantGrowSpeedMultiplier;
                }
            }
        }

        /**
         * ToggleMiscInstantDeadDrop
         * 
         * Toggles the instant dead drop feature.
         * 
         * @return void
         */
        public void ToggleMiscInstantDeadDrop()
        {
            MiscInstantDeadDrop = !MiscInstantDeadDrop;

            Properties.Settings.Default.MiscInstantDeadDrop = MiscInstantDeadDrop;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"Misc - Instant Dead Drop toggled! (now {MiscInstantDeadDrop})");
        }

        /**
         * ToggleMiscInstantLaundering
         * 
         * Toggles the instant laundering feature.
         * 
         * @return void
         */
        public void ToggleMiscInstantLaundering()
        {
            MiscInstantLaundering = !MiscInstantLaundering;

            Properties.Settings.Default.MiscInstantLaundering = MiscInstantLaundering;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"Misc - Instant Laundering toggled! (now {MiscInstantLaundering})");
        }

        /**
         * ToggleMiscInstantMixing
         * 
         * Toggles the instant mixing feature.
         * 
         * @return void
         */
        public void ToggleMiscInstantMixing()
        {
            MiscInstantMixing = !MiscInstantMixing;

            Properties.Settings.Default.MiscInstantMixing = MiscInstantMixing;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"Misc - Instant Mixing toggled! (now {MiscInstantMixing})");
        }

        /**
         * GetProductQualities
         * 
         * Gets all product qualities.
         * 
         * @return List<string> A list of all product qualities.
         */
        public List<string> GetProductQualities()
        {
            var Qualities = new List<string>();
            foreach (var Quality in Enum.GetValues(typeof(EQuality)))
            {
                Qualities.Add(Quality.ToString());
            }
            return Qualities;
        }

        /**
         * GetProductPackagings
         * 
         * Gets all product packagings.
         * 
         * @return List<string> A list of all product packagings.
         */
        public List<string> GetProductPackagings()
        {
            var Packagings = new List<string>();
            foreach (var Category in SpawnerItems)
            {
                if (Category.Key == "Packaging")
                {
                    foreach (var Packaging in Category.Value)
                    {
                        Packagings.Add(Packaging.Key);
                    }
                }
            }

            return Packagings;
        }

        /**
         * SetMiscEquippedProductQuality
         * 
         * Sets the equipped product quality.
         * 
         * @param Quality The quality to set.
         * @return void
         */
        public void SetMiscEquippedProductQuality(string Quality)
        {

            SetQuality command = new SetQuality();
            Il2CppSystem.Collections.Generic.List<string> args = new Il2CppSystem.Collections.Generic.List<string>();
            args.Add(Quality);
            command.Execute(args);

            HelperInstance.SendLoggerMsg($"Misc - Equipped Product Quality set to {Quality}");
        }

        /**
         * SetMiscEquippedProductPackaging
         * 
         * Sets the equipped product packaging.
         * 
         * @param Packaging The packaging to set.
         * @return void
         */
        public void SetMiscEquippedProductPackaging(string Packaging)
        {
            PackageProduct command = new PackageProduct();
            Il2CppSystem.Collections.Generic.List<string> args = new Il2CppSystem.Collections.Generic.List<string>();
            args.Add(Packaging);
            command.Execute(args);
            HelperInstance.SendLoggerMsg($"Misc - Equipped Product Packaging set to {Packaging}");
        }

        /**
         * MiscUnlockAllNpcs
         * 
         * Unlocks all NPCs in the game.
         * 
         * @return void
         */
        public void MiscUnlockAllNpcs()
        {
            var UnlockedNpcCount = 0;
            foreach (NPC npc in NPCManager.NPCRegistry)
            {
                try
                {
                    NPCRelationData relation = npc.RelationData;
                    if (relation != null)
                    {
                        relation.Unlock(NPCRelationData.EUnlockType.Recommendation, true);
                        UnlockedNpcCount++;

                        // MelonLogger.Msg($"Unlocked NPC: {npc.FirstName ?? npc.ID} {npc.LastName ?? ""}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception if needed
                    HelperInstance.SendLoggerMsg($"Error unlocking NPC: {npc.FirstName ?? npc.ID} {npc.LastName ?? ""} - {ex.Message}");
                }
            }

            // Log the number of unlocked NPCs
            HelperInstance.SendLoggerMsg($"Unlocked {UnlockedNpcCount} NPCs in the game.");
        }

        /**
         * MiscUnlockAllProperties
         * 
         * Unlocks all properties in the game.
         *
         * @return void
         */
        public void MiscUnlockAllProperties()
        {
            var UnlockedPropertiesCount = 0;
            foreach (var Property in PropertyManager.FindObjectsOfType<Property>())
            {
                try
                {
                    // Set the property to owned
                    Property.SetOwned();
                    UnlockedPropertiesCount++;
                }
                catch (Exception ex)
                {
                    // Log the error
                    HelperInstance.SendLoggerMsg($"Error unlocking property: {Property.propertyName ?? Property.propertyCode} - {ex.Message}");
                }
            }

            // Log the number of unlocked properties
            HelperInstance.SendLoggerMsg($"Unlocked {UnlockedPropertiesCount} Properties in the game.");
        }

        /**
         * MiscUnlockAllAchievements
         *
         * Unlocks all achievements in the game.
         *
         * @return void
         */
        public void MiscUnlockAllAchievements()
        {
            var UnlockedAchievementsCount = 0;
            foreach (var achievement in Enum.GetValues(typeof(AchievementManager.EAchievement)))
            {
                try
                {
                    // Unlock the achievement
                    AchievementManager.Instance.UnlockAchievement((AchievementManager.EAchievement)achievement);
                    UnlockedAchievementsCount++;
                }
                catch (Exception ex)
                {
                    // Log the error
                    HelperInstance.SendLoggerMsg($"Error unlocking achievement: {achievement} - {ex.Message}");
                }
            }

            // Log the number of unlocked achievements
            HelperInstance.SendLoggerMsg($"Unlocked {UnlockedAchievementsCount} Achievements in the game.");
        }
        #endregion

        #region Teleport mods
        /**
         * CacheTeleports
         * 
         * Caches all teleport items and their properties.
         * 
         * @return void
         */
        public void CacheTeleports()
        {
            try
            {
                // Create a dictionary to store teleport items
                var _TeleportLocations = new Dictionary<string, Dictionary<string, Vector3>>();

                // Get all properties and businesses
                var Properties = PropertyManager.FindObjectsOfType<Property>();
                foreach (var Property in Properties)
                {
                    // Get the property name and ID
                    string PropertyName = Property.propertyName;
                    string PropertyId = Property.propertyCode;
                    string PropertySaveFileName = Property.SaveFileName;
                    // Get the property position
                    Vector3 PropertyPosition = Property.transform.position;
                    PropertyPosition.y += (PlayerHeight / 2);

                    if (PropertySaveFileName == "Property")
                    {
                        // Add the property to the teleport dictionary
                        if (!_TeleportLocations.ContainsKey("Properties"))
                        {
                            _TeleportLocations["Properties"] = new Dictionary<string, Vector3>();
                        }
                        _TeleportLocations["Properties"][PropertyName] = PropertyPosition;
                    }
                    else if (PropertySaveFileName == "Business")
                    {
                        // Add the business to the teleport dictionary
                        if (!_TeleportLocations.ContainsKey("Businesses"))
                        {
                            _TeleportLocations["Businesses"] = new Dictionary<string, Vector3>();
                        }
                        _TeleportLocations["Businesses"][PropertyName] = PropertyPosition;
                    }
                }

                // Get all npcs and their stores
                var Npcs = NPCManager.FindObjectsOfType<NPC>();
                foreach (var Npc in Npcs)
                {
                    // Get the npc name and ID
                    string NpcName = Npc.FirstName;
                    string NpcId = Npc.ID;
                    // Get the npc position
                    Vector3 NpcPosition = Npc.transform.position;
                    NpcPosition.y += (PlayerHeight / 2);

                    // Check if the npc has a ShopInterface field
                    var ShopInterface = Npc.GetComponent<ShopInterface>();
                    if (ShopInterface != null)
                    {
                        if (!_TeleportLocations.ContainsKey("Shops"))
                        {
                            _TeleportLocations["Shops"] = new Dictionary<string, Vector3>();
                        }
                        _TeleportLocations["Shops"][ShopInterface.ShopName] = NpcPosition;
                    }
                    else
                    {
                        // Add the npc to the teleport dictionary
                        if (!_TeleportLocations.ContainsKey("NPCs"))
                        {
                            _TeleportLocations["NPCs"] = new Dictionary<string, Vector3>();
                        }
                        _TeleportLocations["NPCs"][NpcName] = NpcPosition;
                    }
                }

                // Get all supplier stashes
                var SupplierStashes = StorageManager.FindObjectsOfType<SupplierStash>();
                foreach (var Stash in SupplierStashes)
                {
                    // Get the stash name
                    string StashName = Stash.name;
                    // Get the stash position
                    Vector3 StashPosition = Stash.transform.position;
                    StashPosition.y += (PlayerHeight / 2);
                    // Add the stash to the teleport dictionary
                    if (!_TeleportLocations.ContainsKey("Supplier Stashes"))
                    {
                        _TeleportLocations["Supplier Stashes"] = new Dictionary<string, Vector3>();
                    }
                    _TeleportLocations["Supplier Stashes"][StashName] = StashPosition;
                }

                // Get all supplier locations
                var SupplierLocations = StorageManager.FindObjectsOfType<SupplierLocation>();
                foreach (var Location in SupplierLocations)
                {
                    // Get the location name
                    string LocationName = Location.name;
                    // Get the location position
                    Vector3 LocationPosition = Location.transform.position;
                    LocationPosition.y += (PlayerHeight / 2);
                    // Add the location to the teleport dictionary
                    if (!_TeleportLocations.ContainsKey("Supplier Locations"))
                    {
                        _TeleportLocations["Supplier Locations"] = new Dictionary<string, Vector3>();
                    }
                    _TeleportLocations["Supplier Locations"][LocationName] = LocationPosition;
                }

                // Get all delivery locations
                var DeliveryLocations = StorageManager.FindObjectsOfType<DeliveryLocation>();
                foreach (var Location in DeliveryLocations)
                {
                    // Get the location name
                    string LocationName = Location.name;
                    // Get the location position
                    Vector3 LocationPosition = Location.transform.position;
                    LocationPosition.y += (PlayerHeight / 2);
                    // Add the location to the teleport dictionary
                    if (!_TeleportLocations.ContainsKey("Delivery Locations"))
                    {
                        _TeleportLocations["Delivery Locations"] = new Dictionary<string, Vector3>();
                    }
                    _TeleportLocations["Delivery Locations"][LocationName] = LocationPosition;
                }

                // Get all dead drops
                var DeadDrops = StorageManager.FindObjectsOfType<DeadDrop>();
                foreach (var Drop in DeadDrops)
                {
                    // Get the drop name
                    string DropName = Drop.name;
                    // Get the drop position
                    Vector3 DropPosition = Drop.transform.position;
                    DropPosition.y += (PlayerHeight / 2);
                    // Add the drop to the teleport dictionary
                    if (!_TeleportLocations.ContainsKey("Dead Drops"))
                    {
                        _TeleportLocations["Dead Drops"] = new Dictionary<string, Vector3>();
                    }
                    _TeleportLocations["Dead Drops"][DropName] = DropPosition;
                }

                // Get all players
                var Players = Player.PlayerList;
                if (Players.Count > 1) { 
                    foreach (var Player in Players)
                    {
                        // Get the player name
                        string PlayerName = Player.PlayerName;
                        // Get the player position
                        Vector3 PlayerPosition = Player.transform.position;
                        PlayerPosition.y += (PlayerHeight / 2);
                        // Add the player to the teleport dictionary
                        if (!_TeleportLocations.ContainsKey("Players"))
                        {
                            _TeleportLocations["Players"] = new Dictionary<string, Vector3>();
                        }
                        _TeleportLocations["Players"][PlayerName] = PlayerPosition;
                    }
                }

                // Add the teleports to the main dictionary
                TeleportLocations = _TeleportLocations;

                HelperInstance.SendLoggerMsg($"Cached all teleport locations: {TeleportLocations.Count} total");
            }
            catch (Exception ex)
            {
                HelperInstance.SendLoggerMsg($"Error caching teleports: {ex.Message}");
            }
        }

        /*
         * GetTeleportCategories
         * 
         * Returns a list of all teleport categories.
         * 
         * @return List<string> A list of all teleport categories.
         */
        public List<string> GetTeleportCategories()
        {
            return TeleportLocations.Keys.ToList();
        }

        /**
         * GetTeleportCategoryItems
         * 
         * Returns a list of all items in a given teleport category.
         * 
         * @param Category The category to get items from.
         */
        public Dictionary<string, Vector3> GetTeleportCategoryItems(string category)
        {
            if (TeleportLocations.ContainsKey(category))
            {
                return TeleportLocations[category];
            }
            else
            {
                HelperInstance.SendLoggerMsg($"Category '{category}' not found in TeleportItems.");
                return new Dictionary<string, Vector3>();
            }
        }

        /**
         * FilterTeleportItems
         * 
         * Filters the teleport items based on a given filter string.
         * 
         * @param Category The category to filter items from.
         * @param Filter Filter string to use.
         * @param Items Items to filter from.
         * @return Dictionary<string, string> Dictionary of filtered items.
         */
        public Dictionary<string, Vector3> FilterTeleportItems(string Category, string Filter, Dictionary<string, Vector3> Items)
        {
            // Check if the category exists in the TeleportFilterCache
            if (!TeleportFilterCache.ContainsKey(Category)) TeleportFilterCache[Category] = new Dictionary<string, Dictionary<string, Vector3>>();
            // Check and return the filter if it exists
            if (TeleportFilterCache[Category].ContainsKey(Filter)) return TeleportFilterCache[Category][Filter];
            // Create a new dictionary for the filtered items
            TeleportFilterCache[Category][Filter] = new Dictionary<string, Vector3>();
            Dictionary<string, Vector3> FilteredItems = new Dictionary<string, Vector3>();
            // Filter the items based on the filter string
            foreach (var Item in Items)
            {
                var FilterMatch = Item.Key.ToLower().Trim().Contains(Filter.ToLower().Trim());
                // MelonLogger.Msg($"Checking if item \"{Item.Value.ToLower()}\" contains \"{Filter.ToLower()}\"");
                if (FilterMatch) FilteredItems[Item.Key] = Item.Value;
            }
            // Cache the filtered items
            TeleportFilterCache[Category][Filter] = FilteredItems;
            return FilteredItems;
        }


        /**
         * TeleportToLocation
         * 
         * Teleports the player to a given location.
         * 
         * @param Location The location to teleport to.
         * @return void
         */
        public void TeleportToLocation(string Category, string Location)
        {
            // Get the location from the TeleportItems dictionary
            if (!TeleportLocations.ContainsKey(Category)) return;
            var LocationData = TeleportLocations[Category];

            if (LocationData == null || !LocationData.ContainsKey(Location)) return;

            // Get the position of the location
            Vector3 Position = LocationData[Location];

            // Teleport the player to the location
            PlayerMovement.Instance.Teleport(Position);

            HelperInstance.SendLoggerMsg($"Teleported to {Location} ({Category}) at {Position}");
        }
        #endregion

        #region Employees mods
        /*
         * GetEmployeesCategories
         * 
         * Returns a list of all employees.
         * 
         * @return List<string> A list of all employees.
         */
        public List<string> GetEmployeesCategories()
        {
            return Employees.Keys.ToList();
        }

        /**
         * GetEmployeesCategoryItems
         * 
         * Returns a list of all employees in a given category.
         * 
         * @param Category The category to get employees from.
         */
        public Dictionary<string, List<Employee>> GetEmployeesCategoryItems(string category)
        {
            if (Employees.ContainsKey(category))
            {
                return Employees[category];
            }
            else
            {
                HelperInstance.SendLoggerMsg($"Employees for property '{category}' not found in Employees.");
                return new Dictionary<string, List<Employee>>();
            }
        }

        /**
         * FilterEmployeesItems
         * 
         * Filters the employees items based on a given filter string.
         * 
         * @param Filter Filter string to use.
         * @param Items Items to filter from.
         * @return Dictionary<string, string> Dictionary of filtered items.
         */
        public Dictionary<string, List<Employee>> FilterEmployeesItems(string Filter, Dictionary<string, List<Employee>> Items)
        {
            // Create a new dictionary for the filtered items
            Dictionary<string, List<Employee>> FilteredItems = new Dictionary<string, List<Employee>>();

            // Filter the items based on the filter string
            foreach (var Item in Items)
            {
                foreach (var TmpEmployee in Item.Value)
                {
                    var FilterMatch = TmpEmployee.fullName.ToLower().Trim().Contains(Filter.ToLower().Trim()) || TmpEmployee.EmployeeType.ToString().ToLower().Trim().Contains(Filter.ToLower().Trim());
                    // MelonLogger.Msg($"Checking if item \"{Item.Value.ToLower()}\" contains \"{Filter.ToLower()}\"");
                    if (FilterMatch)
                    {
                        if (!FilteredItems.ContainsKey(Item.Key))
                        {
                            FilteredItems[Item.Key] = new List<Employee>();
                        }
                        FilteredItems[Item.Key].Add(TmpEmployee);
                    }
                }
            }

            return FilteredItems;
        }

        /**
         * GetEmployeeSpeeds
         * 
         * Gets the speed of a given employee.
         * 
         * @param Employee The employee to get the speed of.
         * @return Dictionary<string, float> A dictionary of the employee's speed.
         */
        public Dictionary<string, float> GetEmployeeSpeeds(Il2CppSystem.Guid Employee)
        {
            if (EmployeesSpeed.ContainsKey(Employee))
            {
                return EmployeesSpeed[Employee];
            }

            return new Dictionary<string, float>();
        }

        /**
         * SetEmployeeSpeed
         * 
         * Sets the speed of a given employee.
         * 
         * @param Employee The employee to set the speed of.
         * @param Key The speed identifier value
         * @param Value The value to set the speed to.
         * @return void
         */
        public void SetEmployeeSpeed(Il2CppSystem.Guid Employee, string EmployeeType, string Key, float Value)
        {
            if (EmployeesSpeed.ContainsKey(Employee))
            {
                HelperInstance.SendLoggerMsg($"Trying to find out which Type - '{EmployeeType}'");

                EmployeesSpeed[Employee][Key] = Value;
                switch (EmployeeType)
                {
                    case "Botanist":
                        HelperInstance.SendLoggerMsg($"Switch case botanist matched for '{EmployeeType}'");
                        SetBotanistSpeed(Employee, BotanistEmployees[Employee], Key, Value);
                        HelperInstance.SendLoggerMsg($"Set {Key} for employee '{Employee}' to {Value}");
                        break;
                    case "Handler":
                        HelperInstance.SendLoggerMsg($"Switch case handler matched for '{EmployeeType}'");
                        SetPackagerSpeed(Employee, PackagerEmployees[Employee], Key, Value);
                        HelperInstance.SendLoggerMsg($"Set {Key} for employee '{Employee}' to {Value}");
                        break;
                    default:
                        break;
                }
            } 
            else
            {
                HelperInstance.SendLoggerMsg($"Employee '{Employee}' not found in EmployeesSpeed.");
            }
        }

        /**
         * ResetEmployeeSpeed
         * 
         * Resets the speed of a given employee.
         * 
         * @param Employee The employee to reset the speed of.
         * @param Key The speed identifier value
         * @return float
         */
        public float ResetEmployeeSpeed(Il2CppSystem.Guid Employee, string EmployeeType, string Key)
        {
            float returnValue = 0f;
            if (EmployeesSpeed.ContainsKey(Employee))
            {
                EmployeesSpeed[Employee][Key] = EmployeesDefaultSpeed[Key];
                switch (EmployeeType)
                {
                    case "Botanist":
                        Botanist BotanistEmployee = BotanistEmployees[Employee];
                        returnValue = ResetBotanistSpeed(Employee, BotanistEmployee, Key);
                        break;
                    case "Handler":
                        Packager PackagerEmployee = PackagerEmployees[Employee];
                        returnValue = ResetPackagerSpeed(Employee, PackagerEmployee, Key);
                        break;
                    default:
                        break;
                }
            }

            HelperInstance.SendLoggerMsg($"Reset {Key} for employee '{Employee}' to {returnValue}");
            return returnValue;
        }

        /**
         * SetBotanistSpeed
         * 
         * Sets a botanist's speed value for a given key.
         * 
         * @param Botanist The botanist to set the speed of.
         * @param Key The speed identifier value
         * @param Value The value to set the speed to.
         * @return void
         */
        public void SetBotanistSpeed(Il2CppSystem.Guid Employee, Botanist Botanist, string Key, float Value)
        {
            switch (Key)
            {
                case "Speed":
                    Botanist.Movement.MoveSpeedMultiplier = Value;
                    EmployeesSpeed[Employee]["Speed"] = Botanist.Movement.MoveSpeedMultiplier;
                    break;
                case "Soil Pour Time":
                    Botanist.SOIL_POUR_TIME = Value;
                    EmployeesSpeed[Employee]["Soil Pour Time"] = Botanist.SOIL_POUR_TIME;
                    break;
                case "Water Pour Time":
                    Botanist.WATER_POUR_TIME = Value;
                    EmployeesSpeed[Employee]["Water Pour Time"] = Botanist.WATER_POUR_TIME;
                    break;
                case "Additive Pour Time":
                    Botanist.ADDITIVE_POUR_TIME = Value;
                    EmployeesSpeed[Employee]["Additive Pour Time"] = Botanist.ADDITIVE_POUR_TIME;
                    break;
                case "Seed Sow Time":
                    Botanist.SEED_SOW_TIME = Value;
                    EmployeesSpeed[Employee]["Seed Sow Time"] = Botanist.SEED_SOW_TIME;
                    break;
                default:
                    break;
            }
        }

        /**
         * ResetBotanistSpeed
         * 
         * Resets a botanist's speed value for a given key.
         * 
         * @param Botanist The botanist to reset the speed of.
         * @param Key The speed identifier value
         * @return float
         */
        public float ResetBotanistSpeed(Il2CppSystem.Guid Employee, Botanist Botanist, string Key)
        {
            float returnValue = 0f;

            switch (Key)
            {
                case "Speed":
                    Botanist.Movement.MoveSpeedMultiplier = Botanist.Movement.MoveSpeedMultiplier;
                    EmployeesSpeed[Employee]["Speed"] = Botanist.Movement.MoveSpeedMultiplier;
                    returnValue = Botanist.Movement.MoveSpeedMultiplier;
                    break;
                case "Soil Pour Time":
                    Botanist.SOIL_POUR_TIME = Botanist.SOIL_POUR_TIME;
                    EmployeesSpeed[Employee]["Soil Pour Time"] = Botanist.SOIL_POUR_TIME;
                    returnValue = Botanist.SOIL_POUR_TIME;
                    break;
                case "Water Pour Time":
                    Botanist.WATER_POUR_TIME = Botanist.WATER_POUR_TIME;
                    EmployeesSpeed[Employee]["Water Pour Time"] = Botanist.WATER_POUR_TIME;
                    returnValue = Botanist.WATER_POUR_TIME;
                    break;
                case "Additive Pour Time":
                    Botanist.ADDITIVE_POUR_TIME = Botanist.ADDITIVE_POUR_TIME;
                    EmployeesSpeed[Employee]["Additive Pour Time"] = Botanist.ADDITIVE_POUR_TIME;
                    returnValue = Botanist.ADDITIVE_POUR_TIME;
                    break;
                case "Seed Sow Time":
                    Botanist.SEED_SOW_TIME = Botanist.SEED_SOW_TIME;
                    EmployeesSpeed[Employee]["Seed Sow Time"] = Botanist.SEED_SOW_TIME;
                    returnValue = Botanist.SEED_SOW_TIME;
                    break;
                default:
                    break;
            }

            return returnValue;
        }

        /**
         * SetPackagerSpeed
         * 
         * Sets a packager's speed value for a given key.
         * 
         * @param Packager The packager to set the speed of.
         * @param Key The speed identifier value
         * @param Value The value to set the speed to.
         * @return void
         */
        public void SetPackagerSpeed(Il2CppSystem.Guid Employee, Packager Packager, string Key, float Value)
        {
            switch (Key)
            {
                case "Speed":
                    Packager.Movement.MoveSpeedMultiplier = Value;
                    EmployeesSpeed[Employee]["Speed"] = Packager.Movement.MoveSpeedMultiplier;
                    break;
                case "Packaging Speed":
                    Packager.PackagingSpeedMultiplier = Value;
                    EmployeesSpeed[Employee]["Packaging Speed"] = Packager.PackagingSpeedMultiplier;
                    break;
                default:
                    break;
            }
        }

        /**
         * ResetPackagerSpeed
         * 
         * Resets a packager's speed value for a given key.
         * 
         * @param Packager The packager to reset the speed of.
         * @param Key The speed identifier value
         * @return float
         */
        public float ResetPackagerSpeed(Il2CppSystem.Guid Employee, Packager Packager, string Key)
        {
            float returnValue = 0f;

            switch (Key)
            {
                case "Speed":
                    Packager.Movement.MoveSpeedMultiplier = Packager.Movement.MoveSpeedMultiplier;
                    EmployeesSpeed[Employee]["Speed"] = Packager.Movement.MoveSpeedMultiplier;
                    returnValue = Packager.Movement.MoveSpeedMultiplier;
                    break;
                case "Packaging Speed":
                    Packager.PackagingSpeedMultiplier = Packager.PackagingSpeedMultiplier;
                    EmployeesSpeed[Employee]["Packaging Speed"] = Packager.PackagingSpeedMultiplier;
                    returnValue = Packager.PackagingSpeedMultiplier;
                    break;
                default:
                    break;
            }

            return returnValue;
        }

        /**
         * FireEmployee
         * 
         * Fires a given employee.
         * 
         * @param EmployeeType The type of employee to fire.
         * @param Employee The employee to fire.
         * @return void
         */
        public void FireEmployee(string Property, string EmployeeType, Il2CppSystem.Guid Employee)
        {
            // Check if the property exists
            if (!Employees.ContainsKey(Property)) return;

            // Check if the employee type exists
            if (!Employees[Property].ContainsKey(EmployeeType)) return;

            // Find the employee in the list
            foreach (var EmployeeList in Employees[Property][EmployeeType])
            {
                if (EmployeeList.GUID == Employee)
                {
                    // Fire the employee
                    EmployeeList.SendFire();
                    HelperInstance.SendLoggerMsg($"Fired {EmployeeList.FirstName} ({Employee}) from {Property}");
                    break;
                }
            }
        }
        #endregion


        /**
         * CheckProperties
         * 
         * Gets all properties and their employees.
         * 
         * 
        /**
         * CheckEmployees
         * 
         * Gets all employees for each property
         * 
         * @return void
         */
        public IEnumerator CheckEmployees()
        {
            // Wait for 5 seconds before checking employees
            yield return new WaitForSeconds(5f);

            // Clear current employees
            Employees.Clear();

            // Get all properties
            var Properties = PropertyManager.FindObjectsOfType<Property>();
            foreach (var Property in Properties) {
                // Get the property name and employees
                var PropertyName = Property.propertyName;
                var PropertyEmployees = Property.Employees;

                // Add the property to the employee dictionary
                if (!Employees.ContainsKey(PropertyName))
                {
                    Employees[PropertyName] = new Dictionary<string, List<Employee>>();
                }

                if (PropertyEmployees == null || PropertyEmployees.Count == 0) continue;

                foreach (var Employee in PropertyEmployees)
                {
                    // Get the employee guid, name and type;
                    var EmployeeGuid = Employee.GUID;
                    var EmployeeName = Employee.FirstName;
                    var EmployeeType = Employee.Type.ToString();

                    // Add type specific employees
                    switch (EmployeeType)
                    {
                        case "Botanist":
                            var BotanistEmployee = NPCManager.FindObjectsOfType<Botanist>().FirstOrDefault(x => x.GUID == EmployeeGuid);
                            HelperInstance.SendLoggerMsg($"Found botanist employee: {EmployeeName} ({EmployeeGuid})");
                            BotanistEmployees[EmployeeGuid] = BotanistEmployee;
                            break;
                        case "Handler":
                            var PackagerEmployee = NPCManager.FindObjectsOfType<Packager>().FirstOrDefault(x => x.GUID == EmployeeGuid);
                            HelperInstance.SendLoggerMsg($"Found packager employee: {EmployeeName} ({EmployeeGuid})");
                            PackagerEmployees[EmployeeGuid] = PackagerEmployee;
                            break;
                        default:
                            break;
                    }

                    // Add the employee type to the property
                    if (!Employees[PropertyName].ContainsKey(EmployeeType))
                    {
                        Employees[PropertyName][EmployeeType] = new List<Employee>();
                    }

                    // Add the employee to the property
                    if (!Employees[PropertyName][EmployeeType].Contains(Employee))
                    {
                        Employees[PropertyName][EmployeeType].Add(Employee);
                    }
                }
            }

            yield return CheckEmployees();
        }

        /**
         * CheckEmployeeSpeeds
         * 
         * Gets all employee speeds for all properties
         *
         * @return void
         */
        public IEnumerator CheckEmployeeSpeeds()
        {
            yield return new WaitForSeconds(5f);

            // Get all botanist employees
            var BotanistEmployees = Resources.FindObjectsOfTypeAll<Botanist>();
            foreach (Botanist Botanist in BotanistEmployees)
            {
                // Filter invalid botanists
                if (Botanist == null || Botanist.gameObject == null || Botanist.fullName == "Botanist") continue;

                var BotanistGuid = Botanist.GUID;

                if (!EmployeesSpeed.ContainsKey(BotanistGuid))
                {
                    EmployeesSpeed[BotanistGuid] = new Dictionary<string, float>();
                }
                EmployeesSpeed[BotanistGuid]["Speed"] = Botanist.Movement.MoveSpeedMultiplier;
                EmployeesSpeed[BotanistGuid]["Soil Pour Time"] = Botanist.SOIL_POUR_TIME;
                EmployeesSpeed[BotanistGuid]["Water Pour Time"] = Botanist.WATER_POUR_TIME;
                EmployeesSpeed[BotanistGuid]["Additive Pour Time"] = Botanist.ADDITIVE_POUR_TIME;
                EmployeesSpeed[BotanistGuid]["Seed Sow Time"] = Botanist.SEED_SOW_TIME;
                EmployeesSpeed[BotanistGuid]["Harvest Time"] = Botanist.HARVEST_TIME;
            }

            // Get all packager employees
            var PackagerEmployees = Resources.FindObjectsOfTypeAll<Packager>();
            foreach (Packager Packager in PackagerEmployees)
            {
                // Filter invalid packagers
                if (Packager == null || Packager.gameObject == null || Packager.fullName == "Packager") continue;

                var PackagerGuid = Packager.GUID;

                if (!EmployeesSpeed.ContainsKey(PackagerGuid))
                {
                    EmployeesSpeed[PackagerGuid] = new Dictionary<string, float>();
                }
                EmployeesSpeed[PackagerGuid]["Speed"] = Packager.Movement.MoveSpeedMultiplier;
                EmployeesSpeed[PackagerGuid]["Packaging Speed"] = Packager.PackagingSpeedMultiplier;
            }

            yield break;
        }

        /**
         * CacheDelayed
         * 
         * Calls the CacheGameItems as well as CacheTeleports methods.
         * 
         * @return void
         */
        public IEnumerator CacheDelayed()
        {
            yield return new WaitForSeconds(3f);

            CacheGameItems();
            CacheTeleports();
            CacheStackSizes();

            yield break;
        }

        /**
         * CheckMods
         * 
         * Checks all mods and applies them if necessary.
         * 
         * @return void
         */
        public void CheckMods()
        {
            try
            {
                CheckPlayerMods();
                CheckMiscMods();
            }
            catch (Exception ex)
            {
                // silently catch exceptions
            }
        }

        /**
         * CheckGuiMods
         * 
         * Checks all GUI mods and applies them if necessary.
         * 
         * @return void
         */
        public void CheckGuiMods()
        {
            try
            {
                CheckWorldGuiMods();
            }
            catch (Exception ex)
            {
                // silently catch exceptions
            }
        }

        /**
         * DoNothing
         * 
         * Does literally nothing.
         * 
         * @return void
         */
        public void DoNothing() { }
    }
}
