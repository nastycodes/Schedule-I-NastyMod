using System.Collections.Generic;
using NastyMod_v2.Core;
using MelonLoader;
using UnityEngine;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Employees;
using System;

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

        private static bool IsDragging = false;
        private static Vector2 DragOffset;

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
            ("PLAYER", RenderPlayerTab),
            ("WORLD", RenderWorldTab),
            ("SPAWNER", RenderSpawnerTab),
            ("MISC", RenderMiscTab),
            ("TELEPORT", RenderTeleportTab),
            ("EMPLOYEES", RenderEmployeesTab)
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
        private static GUIStyle CharLabelStyle;
        private static GUIStyle BoldLabelStyle;
        private static GUIStyle EmptyLabelStyle;
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

            ModInstance.SetInstanceReference();

            MelonCoroutines.Start(ModInstance.CacheDelayed());
            MelonCoroutines.Start(ModInstance.CheckEmployees());
            MelonCoroutines.Start(ModInstance.CheckEmployeeSpeeds());
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

            CharLabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fixedWidth = 12,
                fixedHeight = 24
            };

            BoldLabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                fixedWidth = 180,
                fixedHeight = 24
            };

            EmptyLabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fixedWidth = 0,
                fixedHeight = 24
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
            TextfieldStyle.normal.background = MenuBgTexture;
            TextfieldStyle.focused.background = TabBgTexture;
            TextfieldStyle.hover.background = TabBgTexture;
            TextfieldStyle.onNormal.background = MenuBgTexture;
            TextfieldStyle.onHover.background = TabBgTexture;
            TextfieldStyle.onFocused.background = TabBgTexture;

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
            SliderStyle.focused.background = SliderBgTexture;
            SliderStyle.onNormal.background = SliderBgTexture;
            SliderStyle.onHover.background = SliderBgTexture;
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

            // Handle dragging
            Event currentEvent = Event.current;
            Rect dragArea = new Rect(MenuX, MenuY, MenuWidth, 30); // Nur der obere Bereich des Fensters ist verschiebbar

            if (currentEvent.type == EventType.MouseDown && dragArea.Contains(currentEvent.mousePosition))
            {
                IsDragging = true;
                DragOffset = new Vector2(currentEvent.mousePosition.x - MenuX, currentEvent.mousePosition.y - MenuY);
                currentEvent.Use();
            }
            else if (currentEvent.type == EventType.MouseDrag && IsDragging)
            {
                MenuX = (int)(currentEvent.mousePosition.x - DragOffset.x);
                MenuY = (int)(currentEvent.mousePosition.y - DragOffset.y);
                currentEvent.Use();
            }
            else if (currentEvent.type == EventType.MouseUp)
            {
                IsDragging = false;
            }

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
                    MenuTabButtonStyle.hover.background = TabBgTexture;
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
         * @param GUIStyle _LabelStyle The label style.
         * @param bool EndOption Whether to end the option layout.
         * @return void
         */
        private static void BeginOption(string OptionName, GUIStyle _LabelStyle, bool EndOption = false)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(OptionName, _LabelStyle);
            if (EndOption) GUILayout.EndHorizontal();
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

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("Infinite Stamina", LabelStyle);
            Helper.AddButton(ModInstance.PlayerInfiniteStamina ? "Enabled" : "Disabled", ButtonStyle, ModInstance.TogglePlayerInfiniteStamina);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("Never Wanted", LabelStyle);
            Helper.AddButton(ModInstance.PlayerNeverWanted ? "Enabled" : "Disabled", ButtonStyle, ModInstance.TogglePlayerNeverWanted);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("No Clip", LabelStyle);
            Helper.AddButton(ModInstance.PlayerNoClip ? "Enabled" : "Disabled", ButtonStyle, ModInstance.TogglePlayerNoClip);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            Helper.AddSpace(MenuSpacing);
            CurrentYPos += MenuSpacing;

            BeginOption("Move Speed Multiplier", LabelStyle);
            string PlayerMoveSpeedMultiplierRef = ModInstance.PlayerMoveSpeedMultiplier.ToString();
            Helper.AddInput(ref Textfields, ref PlayerMoveSpeedMultiplierRef, "PlayerMoveSpeedMultiplierInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.PlayerMoveSpeedMultiplier = float.TryParse(PlayerMoveSpeedMultiplierRef, out var PlayerMoveSpeedMultiplierResult) ? PlayerMoveSpeedMultiplierResult : 1;
            Helper.AddButton("Set", ButtonStyle, ModInstance.SetPlayerMoveSpeedMultiplier);
            Helper.AddButton("Reset", ButtonStyle, () => {
                Textfields["PlayerMoveSpeedMultiplierInput"].Value = ModInstance.ResetPlayerMoveSpeedMultiplier().ToString();
            });
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("Crouch Speed Multiplier", LabelStyle);
            string PlayerCrouchSpeedMultiplierRef = ModInstance.PlayerCrouchSpeedMultiplier.ToString();
            Helper.AddInput(ref Textfields, ref PlayerCrouchSpeedMultiplierRef, "PlayerCrouchSpeedMultiplierInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.PlayerCrouchSpeedMultiplier = float.TryParse(PlayerCrouchSpeedMultiplierRef, out var PlayerCrouchSpeedMultiplierResult) ? PlayerCrouchSpeedMultiplierResult : 1;
            Helper.AddButton("Set", ButtonStyle, ModInstance.SetPlayerCrouchSpeedMultiplier);
            Helper.AddButton("Reset", ButtonStyle, () =>
            {
                Textfields["PlayerCrouchSpeedMultiplierInput"].Value = ModInstance.ResetPlayerCrouchSpeedMultiplier().ToString();
            });
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("Jump Height Multiplier", LabelStyle);
            string PlayerJumpMultiplierRef = ModInstance.PlayerJumpMultiplier.ToString();
            Helper.AddInput(ref Textfields, ref PlayerJumpMultiplierRef, "PlayerJumpMultiplierInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.PlayerJumpMultiplier = float.TryParse(PlayerJumpMultiplierRef, out var PlayerJumpMultiplierResult) ? PlayerJumpMultiplierResult : 1;
            Helper.AddButton("Set", ButtonStyle, ModInstance.SetPlayerJumpMultiplier);
            Helper.AddButton("Reset", ButtonStyle, () =>
            {
                Textfields["PlayerJumpMultiplierInput"].Value = ModInstance.ResetPlayerJumpMultiplier().ToString();
            });
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            Helper.AddSpace(MenuSpacing);
            CurrentYPos += MenuSpacing;

            BeginOption($"Add Experience", LabelStyle);
            string PlayerExpAmountRef = ModInstance.PlayerExpAmount.ToString();
            Helper.AddInput(ref Textfields, ref PlayerExpAmountRef, "PlayerExpInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.PlayerExpAmount = int.TryParse(PlayerExpAmountRef, out var PlayerExpAmountResult) ? PlayerExpAmountResult : 0;
            Helper.AddButton("Add", ButtonStyle, ModInstance.AddPlayerExp);
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption($"Change Cash", LabelStyle);
            string PlayerCashAmountRef = ModInstance.PlayerCashAmount.ToString();
            Helper.AddInput(ref Textfields, ref PlayerCashAmountRef, "PlayerCashInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.PlayerCashAmount = int.TryParse(PlayerCashAmountRef, out var PlayerCashAmountResult) ? PlayerCashAmountResult : 0;
            Helper.AddButton("Add", ButtonStyle, ModInstance.AddPlayerCash);
            Helper.AddButton("Remove", ButtonStyle, ModInstance.RemovePlayerCash);
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption($"Change Balance", LabelStyle);
            string PlayerBalanceAmountRef = ModInstance.PlayerBalanceAmount.ToString();
            Helper.AddInput(ref Textfields, ref PlayerBalanceAmountRef, "PlayerBalanceInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.PlayerBalanceAmount = int.TryParse(PlayerBalanceAmountRef, out var PlayerBalanceAmountResult) ? PlayerBalanceAmountResult : 0;
            Helper.AddButton("Add", ButtonStyle, ModInstance.AddPlayerBalance);
            Helper.AddButton("Remove", ButtonStyle, ModInstance.RemovePlayerBalance);
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

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

            Helper.AddLabel("Box ESP", BoldLabelStyle);
            CurrentYPos += (int)BoldLabelStyle.fixedHeight;

            BeginOption("NPC", LabelStyle);
            Helper.AddButton(ModInstance.WorldNpcEsp ? "Enabled" : "Disabled", ButtonStyle, ModInstance.ToggleWorldNpcEsp);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("Player", LabelStyle);
            Helper.AddButton(ModInstance.WorldPlayerEsp ? "Enabled" : "Disabled", ButtonStyle, ModInstance.ToggleWorldPlayerEsp);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("Range", LabelStyle);
            string WorldEspRangeRef = ModInstance.WorldEspRange.ToString();
            Helper.AddInput(ref Textfields, ref WorldEspRangeRef, "WorldEspRangeInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.WorldEspRange = int.TryParse(WorldEspRangeRef, out var WorldEspRangeResult) ? WorldEspRangeResult : 0;
            Helper.AddButton("Set", ButtonStyle, ModInstance.SetWorldEspRange);
            Helper.AddButton("Reset", ButtonStyle, () =>
            {
                Textfields["WorldEspRangeInput"].Value = ModInstance.ResetWorldEspRange().ToString();
            });
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            Helper.AddSpace(MenuSpacing);
            CurrentYPos += MenuSpacing;

            Helper.AddLabel("Time", BoldLabelStyle);
            CurrentYPos += (int)BoldLabelStyle.fixedHeight + MenuTabButtonSpacing;

            BeginOption("Scale", LabelStyle);
            string WorldTimeScaleRef = ModInstance.WorldTimeScale.ToString();
            Helper.AddInput(ref Textfields, ref WorldTimeScaleRef, "WorldTimescaleInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.WorldTimeScale = float.TryParse(WorldTimeScaleRef, out var WorldTimeScaleResult) ? WorldTimeScaleResult : 1;
            Helper.AddButton("Set", ButtonStyle, ModInstance.SetWorldTimeScale);
            Helper.AddButton("Reset", ButtonStyle, () =>
            {
                Textfields["WorldTimescaleInput"].Value = ModInstance.ResetWorldTimeScale().ToString();
            });
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("Freeze", LabelStyle);
            Helper.AddButton(ModInstance.WorldFreezeTime ? "Enabled" : "Disabled", ButtonStyle, ModInstance.ToggleWorldFreezeTime);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("Change", LabelStyle);
            string WorldTimeRef = ModInstance.WorldTime;
            Helper.AddInput(ref Textfields, ref WorldTimeRef, "WorldTimeInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.WorldTime = WorldTimeRef;
            Helper.AddButton("Set", ButtonStyle, ModInstance.SetWorldTime);
            Helper.AddButton("+1", ButtonStyle, ModInstance.WorldTimeForwardHour);
            Helper.AddButton("-1", ButtonStyle, ModInstance.WorldTimeBackwardHour);
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

            // Default spawner category
            if (ModInstance.SpawnerSelectedCategory == "") ModInstance.SpawnerSelectedCategory = ModInstance.GetSpawnerCategories()[0];

            // Categories variables
            SidebarWidth = (int)((MenuTabWidth - (2 * MenuSpacing)) * .25);
            SidebarButtonStyle.fixedWidth = Mathf.Floor(SidebarWidth - (2 * MenuTabButtonSpacing));

            // Categories
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

            // Category items
            BeginSidebarContent(SidebarContentWidth);
            BeginOption("Filter", MediumLabelStyle);
            string SpawnerItemFilterRef = ModInstance.SpawnerItemFilter;
            var CustomTextfieldStyle = TextfieldStyle;
            CustomTextfieldStyle.fixedWidth = ((3 * (int)SidebarContentButtonStyle.fixedWidth) + (2 * MenuTabButtonSpacing)) - (int)MediumLabelStyle.fixedWidth - MenuTabButtonSpacing;
            Helper.AddInput(
                ref Textfields,
                ref SpawnerItemFilterRef,
                "SpawnerItemFilterInput",
                MediumLabelStyle.fixedWidth + (2 * MenuTabButtonSpacing),
                0,
                ((3 * (int)SidebarContentButtonStyle.fixedWidth) + (2 * MenuTabButtonSpacing)) - (int)MediumLabelStyle.fixedWidth - MenuTabButtonSpacing,
                24,
                MediumLabelStyle,
                CustomTextfieldStyle,
                ModInstance.DoNothing
            );
            ModInstance.SpawnerItemFilter = SpawnerItemFilterRef;
            Helper.AddButton("Clear", ButtonStyle, () => {
                Textfields["SpawnerItemFilterInput"].Clear();
            });
            EndOption();
            if (!ScrollPositions.ContainsKey("SpawnerContentScroll")) ScrollPositions["SpawnerContentScroll"] = Vector2.zero;
            Vector2 TmpSpawnerContentScrollPosition = ScrollPositions["SpawnerContentScroll"];
            BeginScrollContainer(ref TmpSpawnerContentScrollPosition, SidebarContentWidth, MenuTabHeight - MenuSpacing - 24 - MenuTabButtonSpacing);
            ScrollPositions["SpawnerContentScroll"] = TmpSpawnerContentScrollPosition;
            var SpawnerItems = ModInstance.GetSpawnerCategoryItems(ModInstance.SpawnerSelectedCategory);
            var CurrentItemCount = 0;
            if (ModInstance.SpawnerItemFilter != "" && ModInstance.SpawnerItemFilter != " " && ModInstance.SpawnerItemFilter.Length >= 3)
            {
                SpawnerItems = ModInstance.FilterSpawnerItems(ModInstance.SpawnerSelectedCategory, ModInstance.SpawnerItemFilter, SpawnerItems);
            }
            BeginHorizontal();
            foreach (var Item in SpawnerItems)
            {
                if (CurrentItemCount > SidebarContentButtons - 1)
                {
                    EndHorizontal();
                    BeginHorizontal();
                    CurrentItemCount = 0;
                }
                Helper.AddButton(Item.Key, SidebarContentButtonStyle, () => ModInstance.SpawnItem(Item.Value, ModInstance.SpawnerItemAmount));
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

            var CurrentYPos = 0;

            Helper.AddLabel("General", BoldLabelStyle);
            CurrentYPos += (int)BoldLabelStyle.fixedHeight;

            BeginOption("Stack Size", LabelStyle);
            string MiscStackSizeRef = ModInstance.MiscStackSize.ToString();
            Helper.AddInput(ref Textfields, ref MiscStackSizeRef, "WorldStackSize", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.MiscStackSize = int.TryParse(MiscStackSizeRef, out var MiscStackSizeResult) ? MiscStackSizeResult : 20;
            ModInstance.SetMiscStackSize();
            Helper.AddButton("Reset", ButtonStyle, ModInstance.ResetMiscStackSize);
            Helper.AddButton(ModInstance.MiscUseStackSize ? "Enabled" : "Disabled", ButtonStyle, ModInstance.ToggleMiscStackSize);
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("Deal Success Chance", LabelStyle);
            string MiscDealSuccessChanceRef = ModInstance.MiscDealSuccessChance.ToString();
            Helper.AddInput(ref Textfields, ref MiscDealSuccessChanceRef, "MiscDealSuccessChanceInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.MiscDealSuccessChance = float.TryParse(MiscDealSuccessChanceRef, out var MiscDealSuccessChanceResult) ? MiscDealSuccessChanceResult : 0.5f;
            ModInstance.SetMiscDealSuccessChance();
            Helper.AddLabel("%", CharLabelStyle);
            Helper.AddButton("Reset", ButtonStyle, ModInstance.ResetMiscDealSuccessChance);
            Helper.AddButton(ModInstance.MiscUseDealSuccessChance ? "Enabled" : "Disabled", ButtonStyle, ModInstance.ToggleMiscDealSuccessChance);
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("Growth Speed", LabelStyle);
            string MiscGrowthSpeedRef = ModInstance.MiscPlantGrowSpeedMultiplier.ToString();
            Helper.AddInput(ref Textfields, ref MiscGrowthSpeedRef, "MiscGrowthSpeedInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            ModInstance.MiscPlantGrowSpeedMultiplier = float.TryParse(MiscGrowthSpeedRef, out var MiscGrowthSpeedResult) ? MiscGrowthSpeedResult : 1;
            ModInstance.SetMiscPlantGrowSpeed();
            Helper.AddButton("Reset", ButtonStyle, () =>
            {
                Textfields["MiscGrowthSpeedInput"].Value = ModInstance.ResetMiscPlantGrowSpeed().ToString();
            });
            Helper.AddButton(ModInstance.MiscUsePlantGrowSpeedMultiplier ? "Enabled" : "Disabled", ButtonStyle, ModInstance.ToggleMiscPlantGrowSpeed);
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            Helper.AddSpace(MenuSpacing);
            CurrentYPos += MenuSpacing;

            BeginOption("Instant Dead Drops", LabelStyle);
            Helper.AddButton(ModInstance.MiscInstantDeadDrop ? "Enabled" : "Disabled", ButtonStyle, ModInstance.ToggleMiscInstantDeadDrop);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("Instant Laundering", LabelStyle);
            Helper.AddButton(ModInstance.MiscInstantLaundering ? "Enabled" : "Disabled", ButtonStyle, ModInstance.ToggleMiscInstantLaundering);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("Instant Mixing", LabelStyle);
            Helper.AddButton(ModInstance.MiscInstantMixing ? "Enabled" : "Disabled", ButtonStyle, ModInstance.ToggleMiscInstantMixing);
            EndOption();
            CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            // BeginOption("Trash Grabber Capacity", LabelStyle);
            // string MiscTrashGrabberCapacityRef = ModInstance.MiscTrashGrabberCapacity.ToString();
            // Helper.AddInput(ref Textfields, ref MiscTrashGrabberCapacityRef, "MiscTrashGrabberCapacityInput", LabelStyle.fixedWidth + MenuTabButtonSpacing, CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
            // ModInstance.MiscTrashGrabberCapacity = int.TryParse(MiscTrashGrabberCapacityRef, out var MiscTrashGrabberCapacityResult) ? MiscTrashGrabberCapacityResult : 21;
            // ModInstance.SetMiscTrashGrabberCapacity();
            // Helper.AddButton("Reset", ButtonStyle, ModInstance.ResetMiscTrashGrabberCapacity);
            // Helper.AddButton(ModInstance.MiscUseTrashGrabberCapacity ? "Disabled" : "Enabled", ButtonStyle, ModInstance.ToggleMiscTrashGrabberCapacity);
            // EndOption();
            // CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuSpacing);
            CurrentYPos += MenuSpacing;

            Helper.AddLabel("Equipped Item", BoldLabelStyle);
            CurrentYPos += (int)BoldLabelStyle.fixedHeight;

            BeginOption("Change Quality", LabelStyle);
            foreach (var Quality in ModInstance.GetProductQualities())
            {
                Helper.AddButton(Quality, ButtonStyle, () => ModInstance.SetMiscEquippedProductQuality(Quality));
            }
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            BeginOption("Package Product", LabelStyle);
            foreach (var Packaging in ModInstance.GetProductPackagings())
            {
                Helper.AddButton(Packaging, ButtonStyle, () => ModInstance.SetMiscEquippedProductPackaging(Packaging));
            }
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

            Helper.AddSpace(MenuTabButtonSpacing);
            CurrentYPos += MenuTabButtonSpacing;

            Helper.AddSpace(MenuSpacing);
            CurrentYPos += MenuSpacing;

            BeginOption("Unlock All", LabelStyle);
            var UnlockAllButtonStyle = ButtonStyle;
            UnlockAllButtonStyle.fixedWidth = 120;
            Helper.AddButton("NPCs", UnlockAllButtonStyle, () => ModInstance.MiscUnlockAllNpcs());
            Helper.AddButton("Properties", UnlockAllButtonStyle, () => ModInstance.MiscUnlockAllProperties());
            Helper.AddButton("Achievements", UnlockAllButtonStyle, () => ModInstance.MiscUnlockAllAchievements());
            EndOption();
            CurrentYPos += (int)ButtonStyle.fixedHeight + MenuTabButtonSpacing;

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

            // Default teleport category
            if (ModInstance.TeleportSelectedCategory == "") ModInstance.TeleportSelectedCategory = ModInstance.GetTeleportCategories()[0];

            // Categories variables
            SidebarWidth = (int)((MenuTabWidth - (2 * MenuSpacing)) * .25);
            SidebarButtonStyle.fixedWidth = Mathf.Floor(SidebarWidth - (2 * MenuTabButtonSpacing));

            // Categories
            BeginSidebar(SidebarWidth);
            if (!ScrollPositions.ContainsKey("TeleportScroll")) ScrollPositions["TeleportScroll"] = Vector2.zero;
            Vector2 TmpTeleportScrollPosition = ScrollPositions["TeleportScroll"];
            BeginScrollContainer(ref TmpTeleportScrollPosition, SidebarWidth, MenuTabHeight - MenuSpacing);
            ScrollPositions["TeleportScroll"] = TmpTeleportScrollPosition;
            foreach (var Category in ModInstance.GetTeleportCategories())
            {
                Helper.AddButton(Category, SidebarButtonStyle, () =>
                {
                    ModInstance.TeleportSelectedCategory = Category;
                });
            }
            EndScrollContainer();
            EndSidebar();

            SidebarContentWidth = (int)((MenuTabWidth - (2 * MenuSpacing)) * .75) - MenuSpacing;
            SidebarContentButtonStyle.fixedWidth = Mathf.Floor(((SidebarContentWidth - (MenuTabButtonSpacing * SidebarContentButtons)) / SidebarContentButtons) - (2 * MenuTabButtonSpacing));

            // Category items
            BeginSidebarContent(SidebarContentWidth);
            BeginOption("Filter", MediumLabelStyle);
            string TeleportLocationFilterRef = ModInstance.TeleportLocationFilter;
            var CustomTextfieldStyle = TextfieldStyle;
            CustomTextfieldStyle.fixedWidth = ((3 * (int)SidebarContentButtonStyle.fixedWidth) + (2 * MenuTabButtonSpacing)) - (int)MediumLabelStyle.fixedWidth - MenuTabButtonSpacing;
            Helper.AddInput(
                ref Textfields,
                ref TeleportLocationFilterRef,
                "TeleportLocationFilterInput",
                MediumLabelStyle.fixedWidth + (2 * MenuTabButtonSpacing),
                0,
                ((3 * (int)SidebarContentButtonStyle.fixedWidth) + (2 * MenuTabButtonSpacing)) - (int)MediumLabelStyle.fixedWidth - MenuTabButtonSpacing,
                24,
                MediumLabelStyle,
                CustomTextfieldStyle,
                ModInstance.DoNothing
            );
            ModInstance.TeleportLocationFilter = TeleportLocationFilterRef;
            Helper.AddButton("Clear", ButtonStyle, () =>
            {
                Textfields["TeleportLocationFilterInput"].Clear();
            });
            EndOption();
            if (!ScrollPositions.ContainsKey("TeleportContentScroll")) ScrollPositions["TeleportContentScroll"] = Vector2.zero;
            Vector2 TmpTeleportContentScrollPosition = ScrollPositions["TeleportContentScroll"];
            BeginScrollContainer(ref TmpTeleportContentScrollPosition, SidebarContentWidth, MenuTabHeight - MenuSpacing - 24 - MenuTabButtonSpacing);
            ScrollPositions["TeleportContentScroll"] = TmpTeleportContentScrollPosition;
            var TeleportItems = ModInstance.GetTeleportCategoryItems(ModInstance.TeleportSelectedCategory);
            var CurrentItemCount = 0;
            if (ModInstance.TeleportLocationFilter != "" && ModInstance.TeleportLocationFilter != " " && ModInstance.TeleportLocationFilter.Length >= 3)
            {
                TeleportItems = ModInstance.FilterTeleportItems(ModInstance.TeleportSelectedCategory, ModInstance.TeleportLocationFilter, TeleportItems);
            }
            BeginHorizontal();
            foreach (var Item in TeleportItems)
            {
                if (CurrentItemCount > SidebarContentButtons - 1)
                {
                    EndHorizontal();
                    BeginHorizontal();
                    CurrentItemCount = 0;
                }
                Helper.AddButton(Item.Key, SidebarContentButtonStyle, () =>
                {
                    ModInstance.TeleportToLocation(ModInstance.TeleportSelectedCategory, Item.Key);
                });
                CurrentItemCount++;
            }
            EndHorizontal();
            EndScrollContainer();
            EndSidebarContent();

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

            // Default employees selected property
            if (ModInstance.EmployeesSelectedProperty == "") ModInstance.EmployeesSelectedProperty = ModInstance.GetEmployeesCategories()[0];

            // Categories variables
            SidebarWidth = (int)((MenuTabWidth - (2 * MenuSpacing)) * .25);
            SidebarButtonStyle.fixedWidth = Mathf.Floor(SidebarWidth - (2 * MenuTabButtonSpacing));

            // Categories
            BeginSidebar(SidebarWidth);
            if (!ScrollPositions.ContainsKey("EmployeesScroll")) ScrollPositions["EmployeesScroll"] = Vector2.zero;
            Vector2 TmpEmployeesScrollPosition = ScrollPositions["EmployeesScroll"];
            BeginScrollContainer(ref TmpEmployeesScrollPosition, SidebarWidth, MenuTabHeight - MenuSpacing);
            ScrollPositions["EmployeesScroll"] = TmpEmployeesScrollPosition;
            foreach (var Category in ModInstance.GetEmployeesCategories())
            {
                Helper.AddButton(Category, SidebarButtonStyle, () =>
                {
                    ModInstance.EmployeesSelectedProperty = Category;
                });
            }
            EndScrollContainer();
            EndSidebar();

            // Employees variables
            SidebarContentWidth = (int)((MenuTabWidth - (2 * MenuSpacing)) * .75) - MenuSpacing;
            SidebarContentButtonStyle.fixedWidth = Mathf.Floor(((SidebarContentWidth - (MenuTabButtonSpacing * SidebarContentButtons)) / SidebarContentButtons) - (2 * MenuTabButtonSpacing));

            // Category items
            BeginSidebarContent(SidebarContentWidth);
            BeginOption("Filter", MediumLabelStyle);
            string EmployeesFilterRef = ModInstance.EmployeesFilter;
            var CustomTextfieldStyle = TextfieldStyle;
            CustomTextfieldStyle.fixedWidth = ((3 * (int)SidebarContentButtonStyle.fixedWidth) + (2 * MenuTabButtonSpacing)) - (int)MediumLabelStyle.fixedWidth - MenuTabButtonSpacing;
            Helper.AddInput(
                    ref Textfields,
                    ref EmployeesFilterRef,
                    "EmployeesFilterInput",
                    MediumLabelStyle.fixedWidth + (2 * MenuTabButtonSpacing),
                    0,
                    ((3 * (int)SidebarContentButtonStyle.fixedWidth) + (2 * MenuTabButtonSpacing)) - (int)MediumLabelStyle.fixedWidth - MenuTabButtonSpacing,
                    24,
                    MediumLabelStyle,
                    CustomTextfieldStyle,
                    ModInstance.DoNothing
                );
            ModInstance.EmployeesFilter = EmployeesFilterRef;
            Helper.AddButton("Clear", ButtonStyle, () =>
            {
                Textfields["EmployeesFilterInput"].Clear();
            });
            EndOption();
            if (!ScrollPositions.ContainsKey("EmployeesContentScroll")) ScrollPositions["EmployeesContentScroll"] = Vector2.zero;
            Vector2 TmpEmployeesContentScrollPosition = ScrollPositions["EmployeesContentScroll"];
            BeginScrollContainer(ref TmpEmployeesContentScrollPosition, SidebarContentWidth, MenuTabHeight - MenuSpacing - 24 - MenuTabButtonSpacing);
            ScrollPositions["EmployeesContentScroll"] = TmpEmployeesContentScrollPosition;
            var EmployeesItems = ModInstance.GetEmployeesCategoryItems(ModInstance.EmployeesSelectedProperty);
            var CurrentItemCount = 0;
            if (ModInstance.EmployeesFilter != "" && ModInstance.EmployeesFilter != " " && ModInstance.EmployeesFilter.Length >= 3)
            {
                EmployeesItems = ModInstance.FilterEmployeesItems(ModInstance.EmployeesFilter, EmployeesItems);
            }

            var CurrentYPos = MenuTabButtonSpacing;

            if (EmployeesItems.Count > 0) {
                foreach (var Item in EmployeesItems)
                {
                    if (Item.Value == null) continue;

                    Helper.AddLabel(Item.Key, BoldLabelStyle);
                    CurrentYPos += (int)BoldLabelStyle.fixedHeight + MenuTabButtonSpacing;

                    foreach (var EmployeeItem in Item.Value)
                    {
                        if (EmployeeItem == null) continue;

                        BeginHorizontal();
                        Helper.AddLabel(EmployeeItem.fullName, LabelStyle);
                        Helper.AddButton("Fire", ButtonStyle, () => ModInstance.FireEmployee(ModInstance.EmployeesSelectedProperty, Item.Key, EmployeeItem.GUID));
                        EndHorizontal();

                        CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;

                        var EmployeeSpeeds = ModInstance.GetEmployeeSpeeds(EmployeeItem.GUID);
                        if (EmployeeSpeeds != null && EmployeeSpeeds.Count > 0)
                        {
                            foreach (var EmployeeSpeed in EmployeeSpeeds)
                            {
                                BeginHorizontal();
                                Helper.AddLabel($"     {EmployeeSpeed.Key}", LabelStyle);

                                string EmployeeSpeedRef = EmployeeSpeed.Value.ToString();
                                Helper.AddInput(ref Textfields, ref EmployeeSpeedRef, $"{EmployeeItem.fullName}_{EmployeeSpeed.Key}", LabelStyle.fixedWidth + (2 * MenuTabButtonSpacing), CurrentYPos + MenuTabButtonSpacing - 4, 80, 24, LabelStyle, TextfieldStyle, ModInstance.DoNothing);
                                float EmployeeSpeedTmp = float.TryParse(EmployeeSpeedRef, out var EmployeeSpeedResult) ? EmployeeSpeedResult : 1;
                                Helper.AddButton("Set", ButtonStyle, () => ModInstance.SetEmployeeSpeed(EmployeeItem.GUID, EmployeeItem.EmployeeType.ToString(), EmployeeSpeed.Key, EmployeeSpeedTmp));
                                Helper.AddButton("Reset", ButtonStyle, () =>
                                {
                                    Textfields[$"{EmployeeItem.fullName}_{EmployeeSpeed.Key}"].Value = ModInstance.ResetEmployeeSpeed(EmployeeItem.GUID, EmployeeItem.EmployeeType.ToString(), EmployeeSpeed.Key).ToString();
                                });
                                CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;
                                EndHorizontal();
                            }
                        }
                    }

                    Helper.AddSpace(MenuSpacing);
                    CurrentYPos += MenuSpacing;
                }
            } else
            {
                Helper.AddLabel("No employees found", LabelStyle);
                CurrentYPos += (int)LabelStyle.fixedHeight + MenuTabButtonSpacing;
            }
            EndScrollContainer();
            EndSidebarContent();

            EndTab();
        }
    }
}
