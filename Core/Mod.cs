using System;
using System.Linq;
using System.Collections.Generic;
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
using MelonLoader;

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
        // Helper instance
        public Helper HelperInstance { get; private set; } = new Helper();

        // Renderer instance
        public UI.Renderer RendererInstance { get; private set; } = new UI.Renderer();

        // Player variables
        public bool PlayerInfiniteHealth = Properties.Settings.Default.PlayerInfiniteHealth;
        public bool PlayerInfiniteEnergy = Properties.Settings.Default.PlayerInfiniteEnergy;
        public bool PlayerInfiniteStamina = Properties.Settings.Default.PlayerInfiniteStamina;
        public bool PlayerNeverWanted = Properties.Settings.Default.PlayerNeverWanted;
        public float PlayerMoveSpeedMultiplier = Properties.Settings.Default.PlayerMoveSpeedMultiplier;
        public float PlayerCrouchSpeedMultiplier = Properties.Settings.Default.PlayerCrouchSpeedMultiplier;
        public float PlayerJumpMultiplier = Properties.Settings.Default.PlayerJumpMultiplier;
        public int PlayerExpAmount = 1000;
        public int PlayerCashAmount = 1000;
        public int PlayerBalanceAmount = 1000;

        // World variables
        public bool WorldNpcEsp = Properties.Settings.Default.WorldNpcEsp;
        public bool WorldPlayerEsp = Properties.Settings.Default.WorldPlayerEsp;
        public float WorldEspRange = Properties.Settings.Default.WorldEspRange;
        public float WorldTimeScale = Properties.Settings.Default.WorldTimeScale;
        public string WorldTime = Properties.Settings.Default.WorldTime;

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
        public bool MiscUseStackSize = Properties.Settings.Default.MiscUseStackSize;
        public int MiscStackSize = Properties.Settings.Default.MiscStackSize;
        public bool MiscUseDealSuccessChance = Properties.Settings.Default.MiscUseDealSuccessChance;
        public float MiscDealSuccessChance = Properties.Settings.Default.MiscDealSuccessChance;
        public bool MiscUseTrashGrabberCapacity = Properties.Settings.Default.MiscUseTrashGrabberCapacity;
        public int MiscTrashGrabberCapacity = Properties.Settings.Default.MiscTrashGrabberCapacity;
        public bool MiscUsePlantGrowSpeedMultiplier = Properties.Settings.Default.MiscUsePlantGrowSpeedMultiplier;
        public float MiscPlantGrowSpeedMultiplier = Properties.Settings.Default.MiscPlantGrowSpeedMultiplier;

        // Employees variables
        public string EmployeesSelectedProperty = Properties.Settings.Default.EmployeesSelectedProperty;
        public EEmployeeType EmployeesSelectedType = EEmployeeType.Handler;
        public Dictionary<Employee, float> EmployeesEmployeeSpeed = new Dictionary<Employee, float>();

        // Teleport variables
        private string TeleportSelectedCategory = Properties.Settings.Default.TeleportSelectedCategory;

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

            if (PlayerInfiniteEnergy)
            {
                if (Player.Local.Energy.CurrentEnergy < PlayerEnergy.MAX_ENERGY)
                {
                    Player.Local.Energy.SetEnergy(PlayerEnergy.MAX_ENERGY);
                }
            }

            if (PlayerInfiniteStamina)
            {
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
         * TogglePlayerInfiniteEnergy
         * 
         * Toggles the player infinite energy state.
         * 
         * @return void
         */
        public void TogglePlayerInfiniteEnergy()
        {
            PlayerInfiniteEnergy = !PlayerInfiniteEnergy;
            Properties.Settings.Default.PlayerInfiniteEnergy = PlayerInfiniteEnergy;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"Player - Infinite Energy toggled! (now {PlayerInfiniteEnergy})");
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
         * @return void
         */
        public void ResetPlayerMoveSpeedMultiplier()
        {
            PlayerMoveSpeedMultiplier = 1f;

            Properties.Settings.Default.PlayerMoveSpeedMultiplier = PlayerMoveSpeedMultiplier;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"Player - Move Speed Multiplier reset to {PlayerMoveSpeedMultiplier}");
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
         * @return void
         */
        public void ResetPlayerCrouchSpeedMultiplier()
        {
            PlayerCrouchSpeedMultiplier = 0.6f;

            Properties.Settings.Default.PlayerCrouchSpeedMultiplier = PlayerCrouchSpeedMultiplier;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"Player - Crouch Speed Multiplier reset to {PlayerCrouchSpeedMultiplier}");
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
         * @return void
         */
        public void ResetPlayerJumpMultiplier()
        {
            PlayerJumpMultiplier = 1f;

            Properties.Settings.Default.PlayerJumpMultiplier = PlayerJumpMultiplier;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"Player - Jump Multiplier reset to {PlayerJumpMultiplier}");
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
         * @return void
         */
        public void ResetWorldEspRange()
        {
            WorldEspRange = 100f;

            Properties.Settings.Default.WorldEspRange = WorldEspRange;
            Properties.Settings.Default.Save();

            HelperInstance.SendLoggerMsg($"World - ESP Range reset to {WorldEspRange}");
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
         * @return void
         */
        public void ResetWorldTimeScale()
        {
            var CommandArguments = new Il2CppSystem.Collections.Generic.List<string>();
            CommandArguments.Add("1");

            var Command = new SetTimeScale();
            Command.Execute(CommandArguments);

            HelperInstance.SendLoggerMsg($"World - Time Scale reset to 1");
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
            if (!SpawnerFilterCache.ContainsKey(Category))
            {
                SpawnerFilterCache[Category] = new Dictionary<string, Dictionary<string, string>>();
            }

            // Check if the filter exists in the SpawnerFilterCache
            if (SpawnerFilterCache[Category].ContainsKey(Filter))
            {
                // Return the cached filtered items
                return SpawnerFilterCache[Category][Filter];
            }

            SpawnerFilterCache[Category][Filter] = new Dictionary<string, string>();
            Dictionary<string, string> FilteredItems = new Dictionary<string, string>();

            foreach (var Item in Items)
            {
                MelonLogger.Msg($"Checking if item \"{Item.Value.ToLower()}\" contains \"{Filter.ToLower()}\"");
                if (Item.Value.ToLower().Trim().Contains(Filter.ToLower().Trim()))
                {
                    FilteredItems[Item.Key] = Item.Value;
                }
                else
                {
                    MelonLogger.Msg("!Does not contain!");
                    MelonLogger.Msg("");
                }
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

        #endregion

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
