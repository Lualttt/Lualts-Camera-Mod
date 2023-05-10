using ComputerInterface;
using ComputerInterface.ViewLib;

using LualtsCameraMod.Plugin;

namespace LualtsCameraMod
{
    public class SettingsView : ComputerView
    {
        // This is called when you view is opened
        public override void OnShow(object[] args)
        {
            base.OnShow(args);
            Text = "Monkey Computer\nMonkey Computer\nMounkey Computer";
        }

        public void UpdateScreen()
        {
            object value = SetText(str =>
            {
                str.BeginCenter();
                str.MakeBar('-', SCREEN_WIDTH, 0, "ffffff10");
                str.AppendClr("Lualt's Camera Mod", titleColour).EndColor().AppendLine();
                str.AppendLine("2.0.0");
                str.MakeBar('-', SCREEN_WIDTH, 0, "ffffff10");
                str.EndAlign().AppendLines(1);
                str.MakeBar(' ', SCREEN_WIDTH, 0, "ffffff10");

                str.AppendLine(selectionHandler.GetIndicatedText(0, "Back to Menu"));
                str.AppendLine(SelectionHandler.GetIndicatedText(1, "Keybinds"));
                str.AppendLine();

                str.AppendLines(1);
                str.MakeBar(' ', SCREEN_WIDTH, 0, "ffffff10");
            }
            );
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            if (selectionHandler.HandleKeypress(key))
            {
                UpdateScreen();
                return;
            }

            switch (key)
            {
                case EKeyboardKey.Delete:
                    // "ReturnToMainMenu" will basically switch to the main menu again
                    ShowView<MainView();
                    break;

                case EKeyboardKey.Up:
                    selectionHandler.MoveSelectionUp();
                    UpdateScreen();
                    break;
                case EKeyboardKey.Down:
                    selectionHandler.MoveSelectionDown();
                    UpdateScreen();
                    break;
            }
        }

        private void SelectionHandler_OnSelected(int obj)
        {
            switch (obj)
            {
                case 0:
                    ShowView<MainView>;
                    break;

                case 1:
                    ShowView<KeybindsView>;
                    break;
            }

            
        }
    }
}