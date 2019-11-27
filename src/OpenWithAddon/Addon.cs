using Infragistics.Win.UltraWinToolbars;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Linq;

namespace OpenWith
{
    public class Addon : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Button_OpenWith = "SwissAcademic.Addons.OpenWith.Command";

        #endregion

        #region Fields

        CommandbarButton _commandbarButton;

        #endregion

        #region Methods

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            if (mainForm.ReferenceEditorElectronicLocationsToolbarsManager?.Tools.Cast<ToolBase>().FirstOrDefault(tool => tool.Key.Equals("ReferenceEditorUriLocationsContextMenu")) is PopupMenuTool popupMenu)
                _commandbarButton = CommandbarMenu.Create(popupMenu).InsertCommandbarButton(4, Key_Button_OpenWith, Properties.OpenWithAddonResources.MenuCaption);


            base.OnHostingFormLoaded(mainForm);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            if (_commandbarButton != null)
                _commandbarButton.Text = Properties.OpenWithAddonResources.MenuCaption;

            base.OnLocalizing(mainForm);
        }

        public override void OnApplicationIdle(MainForm mainForm)
        {
            if (_commandbarButton != null)
                _commandbarButton.Visible = mainForm.GetPathOfSelectedElectronicLocations().IsNotNullOrEmptyOrWhiteSpace();

            base.OnApplicationIdle(mainForm);
        }

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_Button_OpenWith, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;

                Program.ClosePreview(mainForm.PreviewControl.ActiveUri);

                var path = mainForm.GetPathOfSelectedElectronicLocations();
                var args = $"{System.IO.Path.Combine(Environment.SystemDirectory, "shell32.dll")},OpenAs_RunDLL {path}";
                System.Diagnostics.Process.Start("rundll32.exe", args);
            }

            base.OnBeforePerformingCommand(mainForm, e);
        }

        #endregion
    }
}