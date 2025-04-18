using System.Collections.Generic;
using NastyMod_v2.Core;
using UnityEngine;
using Il2CppScheduleOne.PlayerScripts;

namespace NastyMod_v2.UI
{
    /**
     * Menu
     * 
     * This is the menu class for the NastyMod project.
     * 
     * Author: nastycodes
     * Version: 1.0.0
     */
    public class Menu
    {
        // Mod instance
        public static Mod ModInstance { get; private set; } = new Mod();

        // Toggle hotkey
        private static KeyCode ToggleHotkey = KeyCode.F11;
        private static KeyCode AltToggleHotkey = KeyCode.RightAlt;

        // Initialized flag
        private static bool Initialized = false;

        // Visible flag
        private static bool Visible = false;

        // Menu variables
        private static int MenuWidth = 1100;
        private static int MenuHeight = 560;
        private static int MenuX = 50;
        private static int MenuY = 50;
        private static int MenuCurrentY = 0;
        private static int MenuSpacing = 14;

        // Tab debug
        private static bool TabDebug = false;

        // Tab variables
        private static int MenuTabWidth = MenuWidth - (MenuSpacing * 2);
        private static int MenuTabHeight = 0;
        public List<(string, System.Action)> MenuTabs = new List<(string, System.Action)>
        {
            ("Player", RenderPlayerTab),
            ("World", RenderWorldTab),
            ("Spawner", RenderSpawnerTab),
            ("Misc", RenderMiscTab),
            ("Teleport", RenderTeleportTab),
            ("Employees", RenderEmployeesTab),
            ("About", RenderAboutTab)
        };
        private static int MenuTabButtonSpacing = 4;
        private static int MenuTabButtonWidth = 0;
        private int CurrentTab = 0;

        // Sidebar variables
        private static int SidebarWidth = 0;
        private static int SidebarHeight = 0;
        private static int SidebarContentWidth = 0;
        private static int SidebarContentHeight = 0;
        private static int SidebarContentButtons = 4;

        // Scroll positions
        private static Dictionary<string, Vector2> ScrollPositions = new Dictionary<string, Vector2>();

        // Textfields
        private static Dictionary<string, Textfield> Textfields = new Dictionary<string, Textfield>();

        // GUI styles
        private static GUIStyle MenuStyle;
        private static Texture2D MenuBgTexture;
        private static GUIStyle TabStyle;
        private static Texture2D TabBgTexture;
        private static GUIStyle TitleStyle;
        private static GUIStyle HeaderStyle;
        private static GUIStyle SubHeaderStyle;
        private static GUIStyle MenuTabButtonStyle;
        private static GUIStyle ButtonStyle;
        private static GUIStyle SidebarButtonStyle;
        private static GUIStyle SidebarContentButtonStyle;
        private static GUIStyle LabelStyle;
        private static GUIStyle MediumLabelStyle;
        private static GUIStyle SmallLabelStyle;
        private static GUIStyle TextfieldStyle;
        private static GUIStyle SliderStyle;

        // Colors
        public static Color BackgroundColor = new Color(0.09f, 0.09f, 0.09f);         // Dark gray background
        public static Color TabColor = new Color(0.18f, 0.18f, 0.18f);                // Gray panel
        public static Color accentColor = new Color(1f, 0.843f, 0f);                  // Gold accent
        public static Color warningColor = new Color(0.95f, 0.55f, 0.15f);            // Orange warning
        public static Color dangerColor = new Color(0.95f, 0.25f, 0.25f);             // Red danger
        public static Color successColor = new Color(0.25f, 0.95f, 0.25f);            // Green success
        public static Color textColor = new Color(0.94f, 0.94f, 0.94f);               // White text
        public static Color dimTextColor = new Color(0.82f, 0.82f, 0.82f);            // Dim white text

        /**
         * GetToggleHotkey
         * 
         * Returns the toggle hotkey for the menu.
         * 
         * @return KeyCode The toggle hotkey.
         */
        public KeyCode GetToggleHotkey() => ToggleHotkey;

        /**
         * GetAltToggleHotkey
         * 
         * Returns the alternate toggle hotkey for the menu.
         * 
         * @return KeyCode The alternate toggle hotkey.
         */
        public KeyCode GetAltToggleHotkey() => AltToggleHotkey;

        /**
         * IsInitialized
         * 
         * Checks if the menu is initialized.
         * 
         * @return bool True if the menu is initialized, false otherwise.
         */
        public bool IsInitialized() => Initialized;

        /**
         * SetInitialized
         * 
         * Sets the initialized flag to true and caches game items.
         * 
         * @return void
         */
        public void SetInitialized()
        {
            Initialized = true;

            ModInstance.CacheGameItems();
        }

        /**
         * IsVisible
         * 
         * Checks if the menu is visible.
         * 
         * @return bool True if the menu is visible, false otherwise.
         */
        public bool IsVisible() => Visible;

        /**
         * ToggleVisible
         * 
         * Toggles the visibility of the menu.
         * 
         * @return void
         */
        public void ToggleVisible()
        {
            Visible = !Visible;
            ToggleUI();
        }

        /**
         * ToggleUI
         * 
         * Toggles the UI elements based on the visibility of the menu.
         * 
         * @return void
         */
        private void ToggleUI()
        {
            // Reset animation timers
            if (Visible)
            {
                // Show cursor and unlock
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                // Temporarily disable the player movement
                if (PlayerMovement.Instance != null) PlayerMovement.Instance.enabled = false;

                // Temporarily disable the player camera
                if (PlayerCamera.Instance != null) PlayerCamera.Instance.enabled = false;
            }
            else
            {
                // Hide cursor and lock
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                // Re-enable the player movement
                if (PlayerMovement.Instance != null) PlayerMovement.Instance.enabled = true;

                // Re-enable the player camera
                if (PlayerCamera.Instance != null) PlayerCamera.Instance.enabled = true;
            }
        }

        /**
         * SetCurrentTab
         * 
         * This method sets the current tab.
         * 
         * @param int Tab The tab to set.
         * @return void
         */
        private void SetCurrentTab(int Tab) => CurrentTab = Tab;

        /**
         * BuildGuiStyles
         * 
         * This method builds the GUI styles for the menu.
         * 
         * @return void
         */
        private void BuildGuiStyles()
        {
            MenuStyle = new GUIStyle(GUI.skin.box)
            {
                fontSize = 16,
                fixedWidth = MenuWidth,
                fixedHeight = MenuHeight,
            };
            MenuBgTexture = new Texture2D(1, 1);
            MenuBgTexture.SetPixel(0, 0, BackgroundColor);
            MenuBgTexture.Apply();
            MenuStyle.normal.background = MenuBgTexture;

            TabStyle = new GUIStyle(GUI.skin.box)
            {
                fontSize = 14,
            };
            TabBgTexture = new Texture2D(1, 1);
            TabBgTexture.SetPixel(0, 0, TabColor);
            TabBgTexture.Apply();
            TabStyle.normal.background = TabBgTexture;

            TitleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                fixedHeight = 30,
            };

            HeaderStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                fixedHeight = 24,
                fontStyle = FontStyle.Bold,
            };

            SubHeaderStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
            };

            MenuTabButtonWidth = (MenuWidth - (2 * MenuSpacing) - ((MenuTabs.Count - 1) * MenuTabButtonSpacing)) / MenuTabs.Count;
            MenuTabButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 14,
                fixedWidth = MenuTabButtonWidth,
                fixedHeight = 24,
            };
            var MenuTabButtonBgTexture = new Texture2D(1, 1);
            MenuTabButtonBgTexture.SetPixel(0, 0, TabColor);
            MenuTabButtonBgTexture.Apply();
            MenuTabButtonStyle.normal.background = MenuBgTexture;
            MenuTabButtonStyle.hover.background = TabBgTexture;
            MenuTabButtonStyle.active.background = TabBgTexture;
            MenuTabButtonStyle.focused.background = TabBgTexture;
            MenuTabButtonStyle.onNormal.background = MenuBgTexture;
            MenuTabButtonStyle.onHover.background = TabBgTexture;
            MenuTabButtonStyle.onActive.background = TabBgTexture;
            MenuTabButtonStyle.onFocused.background = TabBgTexture;

            ButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 14,
                fixedWidth = 80,
                fixedHeight = 24
            };
            var ButtonBgTexture = new Texture2D(1, 1);
            ButtonBgTexture.SetPixel(0, 0, BackgroundColor);
            ButtonBgTexture.Apply();
            ButtonStyle.normal.background = MenuBgTexture;
            ButtonStyle.hover.background = MenuBgTexture;
            ButtonStyle.active.background = MenuBgTexture;
            ButtonStyle.focused.background = MenuBgTexture;
            ButtonStyle.onNormal.background = MenuBgTexture;
            ButtonStyle.onHover.background = MenuBgTexture;
            ButtonStyle.onActive.background = MenuBgTexture;
            ButtonStyle.onFocused.background = MenuBgTexture;

            SidebarButtonStyle = ButtonStyle;
            SidebarContentButtonStyle = ButtonStyle;

            LabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fixedWidth = 180,
                fixedHeight = 24,
            };

            MediumLabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fixedWidth = 50,
                fixedHeight = 24
            };

            SmallLabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 13,
                fixedWidth = 35,
                fixedHeight = 24
            };

            TextfieldStyle = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 14,
                fixedHeight = 24
            };
            var TextfieldBgTexture = new Texture2D(1, 1);
            TextfieldBgTexture.SetPixel(0, 0, BackgroundColor);
            TextfieldBgTexture.Apply();
            TextfieldStyle.normal.background = TabBgTexture;
            TextfieldStyle.focused.background = MenuBgTexture;
            TextfieldStyle.hover.background = MenuBgTexture;
            TextfieldStyle.active.background = MenuBgTexture;
            TextfieldStyle.onNormal.background = TabBgTexture;
            TextfieldStyle.onHover.background = MenuBgTexture;
            TextfieldStyle.onActive.background = MenuBgTexture;
            TextfieldStyle.onFocused.background = MenuBgTexture;

            SliderStyle = new GUIStyle(GUI.skin.horizontalSlider)
            {
                fontSize = 14,
                fixedHeight = 10
            };
            var SliderBgTexture = new Texture2D(1, 1);
            SliderBgTexture.SetPixel(0, 0, BackgroundColor);
            SliderBgTexture.Apply();
            SliderStyle.normal.background = SliderBgTexture;
            SliderStyle.hover.background = SliderBgTexture;
            SliderStyle.active.background = SliderBgTexture;
            SliderStyle.focused.background = SliderBgTexture;
            SliderStyle.onNormal.background = SliderBgTexture;
            SliderStyle.onHover.background = SliderBgTexture;
            SliderStyle.onActive.background = SliderBgTexture;
            SliderStyle.onFocused.background = SliderBgTexture;
        }

        /**
         * Render
         * 
         * This method renders the menu.
         * 
         * @return void
         */
        public void Render()
        {
            MenuCurrentY = 0;

            // Build GUI styles
            BuildGuiStyles();

            GUILayout.BeginArea(new Rect(MenuX, MenuY, MenuWidth, MenuHeight), MenuStyle);
            MenuCurrentY += MenuSpacing;

            // Title
            GUILayout.BeginArea(new Rect(MenuSpacing, MenuCurrentY, MenuTabWidth, TitleStyle.fixedHeight));
            GUILayout.Label("NastyMod v2", TitleStyle);
            GUILayout.EndArea();
            MenuCurrentY += (int)TitleStyle.fixedHeight + MenuSpacing;

            // Tab navigation
            GUILayout.BeginArea(new Rect(MenuSpacing, MenuCurrentY, MenuTabWidth, MenuTabButtonStyle.fixedHeight));
            GUILayout.BeginHorizontal();
            for (int i = 0; i < MenuTabs.Count; i++)
            {
                if (CurrentTab == i)
                {
                    MenuTabButtonStyle.normal.background = TabBgTexture;
                    MenuTabButtonStyle.hover.background = TabBgTexture;
                    MenuTabButtonStyle.active.background = TabBgTexture;
                    MenuTabButtonStyle.focused.background = TabBgTexture;
                }
                else
                {
                    MenuTabButtonStyle.normal.background = MenuBgTexture;
                    MenuTabButtonStyle.hover.background = MenuBgTexture;
                    MenuTabButtonStyle.active.background = MenuBgTexture;
                    MenuTabButtonStyle.focused.background = MenuBgTexture;
                }

                if (GUILayout.Button(MenuTabs[i].Item1, MenuTabButtonStyle))
                {
                    SetCurrentTab(i);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            MenuCurrentY += (int)ButtonStyle.fixedHeight + MenuSpacing;

            // Tab content
            GUILayout.BeginArea(new Rect(MenuSpacing, MenuCurrentY, MenuTabWidth, MenuHeight - MenuCurrentY - MenuSpacing), TabStyle);
            try
            {
                MenuTabs[CurrentTab].Item2.Invoke();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error rendering tab: {e}");
            }
            GUILayout.EndArea();
            
            GUILayout.EndArea();
        }

        /**
         * BeginTab
         * 
         * Begins a new tab area.
         * 
         * @return void
         */
        private static void BeginTab()
        {
            MenuTabHeight = MenuHeight - MenuCurrentY - (MenuSpacing * 2);
            GUILayout.BeginArea(new Rect(MenuSpacing, MenuSpacing, MenuTabWidth, MenuTabHeight));
        }

        /**
         * EndTab
         * 
         * Ends the current tab area.
         * 
         * @return void
         */
        private static void EndTab()
        {
            GUILayout.EndArea();
        }

        /**
         * BeginSidebar
         * 
         * Begins a new sidebar area.
         * 
         * @param int SidebarWidth The width of the sidebar.
         * @return void
         */
        private static void BeginSidebar(int SidebarWidth)
        {
            GUILayout.BeginArea(new Rect(0, 0, SidebarWidth, MenuTabHeight));
        }

        /**
         * EndSidebar
         * 
         * Ends the current sidebar area.
         * 
         * @return void
         */
        private static void EndSidebar()
        {
            GUILayout.EndArea();
        }

        /**
         * BeginSidebarContent
         * 
         * Begins a new sidebar content area.
         * 
         * @param int SidebarContentWidth The width of the sidebar content.
         * @return void
         */
        private static void BeginSidebarContent(int SidebarContentWidth)
        {
            GUILayout.BeginArea(new Rect(SidebarWidth + MenuSpacing, 0, SidebarContentWidth, MenuTabHeight));
        }

        /**
         * EndSidebarContent
         * 
         * Ends the current sidebar content area.
         * 
         * @return void
         */
        private static void EndSidebarContent()
        {
            GUILayout.EndArea();
        }

        /**
         * BeginScrollContainer
         * 
         * Begins a new scroll container.
         * 
         * @param Vector2 ScrollPosition The current scroll position.
         * @param int Width The width of the scroll container.
         * @param int Height The height of the scroll container.
         * @return void
         */
        private static void BeginScrollContainer(ref Vector2 ScrollPosition, int Width, int Height)
        {
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, GUILayout.Width(Width), GUILayout.Height(Height));
        }

        /**
         * EndScrollContainer
         * 
         * Ends the current scroll container.
         * 
         * @return void
         */
        private static void EndScrollContainer()
        {
            GUILayout.EndScrollView();
        }

        /**
         * BeginHorizontal
         * 
         * Begins a new horizontal layout.
         * 
         * @return void
         */
        private static void BeginHorizontal()
        {
            GUILayout.BeginHorizontal();
        }

        /**
         * EndHorizontal
         * 
         * Ends the current horizontal layout.
         * 
         * @return void
         */
        private static void EndHorizontal()
        {
            GUILayout.EndHorizontal();
        }

        /**
         * BeginOption
         * 
         * Begins a new option layout.
         * 
         * @param string OptionName The name of the option.
         * @return void
         */
        private static void BeginOption(string OptionName, GUIStyle _LabelStyle)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(OptionName, LabelStyle);
        }

        /**
         * EndOption
         * 
         * Ends the current option layout.
         * 
         * @return void
         */
        private static void EndOption()
        {
            GUILayout.EndHorizontal();
        }

        /**
         * OnUpdate
         * 
         * Called once per frame.
         * 
         * @return void
         */
        public void OnUpdate()
        {
            ModInstance.CheckMods();
        }

        /**
         * OnGUI
         * 
         * Called once per frame to render and handle GUI events.
         * 
         * @return void
         */
        public void OnGUI()
        {
            ModInstance.CheckGuiMods();
        }

        /**
         * RenderPlayerTab
         * 
         * Renders the player tab.
         * 
         * @return void
         */
        private static void RenderPlayerTab()
        {
            BeginTab();

            var CurrentYPos = 0;

            BeginOption("Infinite Health", LabelStyle);
            Helper.AddButton(ModInstance.PlayerInfiniteHealth ? "Enabled" : "Disabled", ButtonStyle, ModInstance.TogglePlayerInfiniteHealth);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            BeginOption("Infinite Energy", LabelStyle);
            Helper.AddButton(ModInstance.PlayerInfiniteEnergy ? "Enabled" : "Disabled", ButtonStyle, ModInstance.TogglePlayerInfiniteEnergy);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            BeginOption("Infinite Stamina", LabelStyle);
            Helper.AddButton(ModInstance.PlayerInfiniteStamina ? "Enabled" : "Disabled", ButtonStyle, ModInstance.TogglePlayerInfiniteStamina);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            BeginOption("Never Wanted", LabelStyle);
            Helper.AddButton(ModInstance.PlayerNeverWanted ? "Enabled" : "Disabled", ButtonStyle, ModInstance.TogglePlayerNeverWanted);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuSpacing);
            CurrentYPos += MenuSpacing;

            BeginOption("Move Speed Multiplier", LabelStyle);
            string PlayerMoveSpeedMultiplierRef = ModInstance.PlayerMoveSpeedMultiplier.ToString();
            Helper.AddInput(ref Textfields, ref PlayerMoveSpeedMultiplierRef, "PlayerMoveSpeedMultiplierInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.PlayerMoveSpeedMultiplier = float.TryParse(PlayerMoveSpeedMultiplierRef, out var PlayerMoveSpeedMultiplierResult) ? PlayerMoveSpeedMultiplierResult : 1;
            Helper.AddButton("Set", ButtonStyle, ModInstance.SetPlayerMoveSpeedMultiplier);
            Helper.AddButton("Reset", ButtonStyle, ModInstance.ResetPlayerMoveSpeedMultiplier);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            BeginOption("Crouch Speed Multiplier", LabelStyle);
            string PlayerCrouchSpeedMultiplierRef = ModInstance.PlayerCrouchSpeedMultiplier.ToString();
            Helper.AddInput(ref Textfields, ref PlayerCrouchSpeedMultiplierRef, "PlayerCrouchSpeedMultiplierInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.PlayerCrouchSpeedMultiplier = float.TryParse(PlayerCrouchSpeedMultiplierRef, out var PlayerCrouchSpeedMultiplierResult) ? PlayerCrouchSpeedMultiplierResult : 1;
            Helper.AddButton("Set", ButtonStyle, ModInstance.SetPlayerCrouchSpeedMultiplier);
            Helper.AddButton("Reset", ButtonStyle, ModInstance.ResetPlayerCrouchSpeedMultiplier);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            BeginOption("Jump Height Multiplier", LabelStyle);
            string PlayerJumpMultiplierRef = ModInstance.PlayerJumpMultiplier.ToString();
            Helper.AddInput(ref Textfields, ref PlayerJumpMultiplierRef, "PlayerJumpMultiplierInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.PlayerJumpMultiplier = float.TryParse(PlayerJumpMultiplierRef, out var PlayerJumpMultiplierResult) ? PlayerJumpMultiplierResult : 1;
            Helper.AddButton("Set", ButtonStyle, ModInstance.SetPlayerJumpMultiplier);
            Helper.AddButton("Reset", ButtonStyle, ModInstance.ResetPlayerJumpMultiplier);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuSpacing);
            CurrentYPos += MenuSpacing;

            BeginOption($"Add Experience", LabelStyle);
            string PlayerExpAmountRef = ModInstance.PlayerExpAmount.ToString();
            Helper.AddInput(ref Textfields, ref PlayerExpAmountRef, "PlayerExpInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.PlayerExpAmount = int.TryParse(PlayerExpAmountRef, out var PlayerExpAmountResult) ? PlayerExpAmountResult : 0;
            Helper.AddButton("Add", ButtonStyle, ModInstance.AddPlayerExp);
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            BeginOption($"Change Cash", LabelStyle);
            string PlayerCashAmountRef = ModInstance.PlayerCashAmount.ToString();
            Helper.AddInput(ref Textfields, ref PlayerCashAmountRef, "PlayerCashInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.PlayerCashAmount = int.TryParse(PlayerCashAmountRef, out var PlayerCashAmountResult) ? PlayerCashAmountResult : 0;
            Helper.AddButton("Add", ButtonStyle, ModInstance.AddPlayerCash);
            Helper.AddButton("Remove", ButtonStyle, ModInstance.RemovePlayerCash);
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            BeginOption($"Change Balance", LabelStyle);
            string PlayerBalanceAmountRef = ModInstance.PlayerBalanceAmount.ToString();
            Helper.AddInput(ref Textfields, ref PlayerBalanceAmountRef, "PlayerBalanceInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.PlayerBalanceAmount = int.TryParse(PlayerBalanceAmountRef, out var PlayerBalanceAmountResult) ? PlayerBalanceAmountResult : 0;
            Helper.AddButton("Add", ButtonStyle, ModInstance.AddPlayerBalance);
            Helper.AddButton("Remove", ButtonStyle, ModInstance.RemovePlayerBalance);
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuSpacing);
            CurrentYPos += MenuSpacing;

            EndTab();
        }

        /**
         * RenderWorldTab
         * 
         * Renders the world tab.
         * 
         * @return void
         */
        private static void RenderWorldTab()
        {
            BeginTab();

            var CurrentYPos = 0;

            BeginOption("NPC Box ESP", LabelStyle);
            Helper.AddButton(ModInstance.WorldNpcEsp ? "Enabled" : "Disabled", ButtonStyle, ModInstance.ToggleWorldNpcEsp);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            BeginOption("Player Box ESP", LabelStyle);
            Helper.AddButton(ModInstance.WorldPlayerEsp ? "Enabled" : "Disabled", ButtonStyle, ModInstance.ToggleWorldPlayerEsp);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            BeginOption("Box ESP Range", LabelStyle);
            string WorldEspRangeRef = ModInstance.WorldEspRange.ToString();
            Helper.AddInput(ref Textfields, ref WorldEspRangeRef, "WorldEspRangeInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.WorldEspRange = int.TryParse(WorldEspRangeRef, out var WorldEspRangeResult) ? WorldEspRangeResult : 0;
            Helper.AddButton("Set", ButtonStyle, ModInstance.SetWorldEspRange);
            Helper.AddButton("Reset", ButtonStyle, ModInstance.ResetWorldEspRange);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuSpacing);
            CurrentYPos += MenuSpacing;

            BeginOption("World Time Scale", LabelStyle);
            string WorldTimeScaleRef = ModInstance.WorldTimeScale.ToString();
            Helper.AddInput(ref Textfields, ref WorldTimeScaleRef, "WorldTimescaleInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.WorldTimeScale = float.TryParse(WorldTimeScaleRef, out var WorldTimeScaleResult) ? WorldTimeScaleResult : 1;
            Helper.AddButton("Set", ButtonStyle, ModInstance.SetWorldTimeScale);
            Helper.AddButton("Reset", ButtonStyle, ModInstance.ResetWorldTimeScale);
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            BeginOption("World Time", LabelStyle);
            string WorldTimeRef = ModInstance.WorldTime;
            Helper.AddInput(ref Textfields, ref WorldTimeRef, "WorldTimeInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.WorldTime = WorldTimeRef;
            Helper.AddButton("Set", ButtonStyle, ModInstance.SetWorldTime);
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            EndTab();
        }

        /**
         * RenderSpawnerTab
         * 
         * Renders the spawner tab.
         * 
         * @return void
         */
        private static void RenderSpawnerTab()
        {
            BeginTab();

            if (ModInstance.SpawnerSelectedCategory == "") ModInstance.SpawnerSelectedCategory = ModInstance.GetSpawnerCategories()[0];

            SidebarWidth = (int)((MenuTabWidth - (2 * MenuSpacing)) * .25);
            SidebarButtonStyle.fixedWidth = Mathf.Floor(SidebarWidth - (2 * MenuTabButtonSpacing));

            BeginSidebar(SidebarWidth);
            if (!ScrollPositions.ContainsKey("SpawnerScroll")) ScrollPositions["SpawnerScroll"] = Vector2.zero;
            Vector2 TmpSpawnerScrollPosition = ScrollPositions["SpawnerScroll"];
            BeginScrollContainer(ref TmpSpawnerScrollPosition, SidebarWidth, MenuTabHeight - MenuSpacing);
            ScrollPositions["SpawnerScroll"] = TmpSpawnerScrollPosition;
            foreach (var Category in ModInstance.GetSpawnerCategories())
            {
                Helper.AddButton(Category, SidebarButtonStyle, () =>
                {
                    ModInstance.SpawnerSelectedCategory = Category;
                });
            }
            EndScrollContainer();
            EndSidebar();

            SidebarContentWidth = (int)((MenuTabWidth - (2 * MenuSpacing)) * .75) - MenuSpacing;
            SidebarContentButtonStyle.fixedWidth = Mathf.Floor(((SidebarContentWidth - (MenuTabButtonSpacing * SidebarContentButtons)) / SidebarContentButtons) - (2 * MenuTabButtonSpacing));

            BeginSidebarContent(SidebarContentWidth);

            BeginOption("Filter", MediumLabelStyle);
            string SpawnerItemFilterRef = ModInstance.SpawnerItemFilter;
            Helper.AddInput(ref Textfields, ref SpawnerItemFilterRef, "SpawnerItemFilterInput", MediumLabelStyle.fixedWidth + MenuTabButtonSpacing, 0, (2 * (int)ButtonStyle.fixedWidth) + (int)MediumLabelStyle.fixedWidth, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.SpawnerItemFilter = SpawnerItemFilterRef;
            Helper.AddButton("Clear", ButtonStyle, () =>
            {
                ModInstance.SpawnerItemFilter = "";
            });
            EndOption();

            if (!ScrollPositions.ContainsKey("SpawnerContentScroll")) ScrollPositions["SpawnerContentScroll"] = Vector2.zero;
            Vector2 TmpSpawnerContentScrollPosition = ScrollPositions["SpawnerContentScroll"];
            BeginScrollContainer(ref TmpSpawnerContentScrollPosition, SidebarContentWidth, MenuTabHeight - MenuSpacing - 24 - MenuTabButtonSpacing);
            ScrollPositions["SpawnerContentScroll"] = TmpSpawnerContentScrollPosition;
            var SpawnerItems = ModInstance.GetSpawnerCategoryItems(ModInstance.SpawnerSelectedCategory);
            var CurrentItemCount = 0;
            BeginHorizontal();
            foreach (var Item in SpawnerItems)
            {
                if (CurrentItemCount > SidebarContentButtons - 1)
                {
                    EndHorizontal();
                    BeginHorizontal();
                    CurrentItemCount = 0;
                }

                Helper.AddButton(Item.Key, SidebarContentButtonStyle, () =>
                {
                    ModInstance.SpawnItem(Item.Value, ModInstance.SpawnerItemAmount);
                });

                CurrentItemCount++;
            }
            EndHorizontal();
            EndScrollContainer();
            EndSidebarContent();

            EndTab();
        }

        /**
         * RenderMiscTab
         * 
         * Renders the misc tab.
         * 
         * @return void
         */
        private static void RenderMiscTab()
        {
            BeginTab();



            EndTab();
        }

        /**
         * RenderTeleportTab
         * 
         * Renders the teleport tab.
         * 
         * @return void
         */
        private static void RenderTeleportTab()
        {
            BeginTab();



            EndTab();
        }

        /**
         * RenderEmployeesTab
         * 
         * Renders the employees tab.
         * 
         * @return void
         */
        private static void RenderEmployeesTab()
        {
            BeginTab();



            EndTab();
        }

        /**
         * RenderAboutTab
         * 
         * Renders the about tab.
         * 
         * @return void
         */
        private static void RenderAboutTab()
        {
            BeginTab();

            GUILayout.Label("NastyMod v2.0.0", SubHeaderStyle);
            GUILayout.Label(" made by nastycodes", LabelStyle);

            GUILayout.Space(MenuSpacing);

            GUILayout.Label("Thanks to my discord supporters for reporting bugs and suggesting features!", SubHeaderStyle);

            EndTab();
        }
    }
}
