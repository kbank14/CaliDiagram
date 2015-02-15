using Caliburn.Micro;
using CaliDiagram.Serialization;
using CaliDiagram.ViewModels;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;
using System.Windows.Media;
using RaftDemo.Views;
using RaftDemo.ViewModels;

namespace RaftDemo
{
    public class AppViewModel : PropertyChangedBase, IShell, IViewAware
    {
        public AppViewModel(IWindowManager wm)
        {
            Diagram1 = new DiagramViewModel(new RaftDiagramDefinition());
            modelLoader = new DiagramXmlSerializer(Diagram1);
            RightPanel = new WorldSettingsViewModel(this);
        }
        DiagramXmlSerializer modelLoader;

        public async void About()
        {
            await window.ShowMessageAsync("Raft algorithm visualization", "Copyright Tomasz Ścisłowicz(toomasz@gmail.com)");
        }
       
        public void SaveDiagram()
        {
            modelLoader.SaveDiagram("RaftModel.xml");
        }
        public void LoadDiagram()
        {
            modelLoader.LoadDiagram("RaftModel.xml");
        }
        
        public void ClearDiagram()
        {
            Diagram1.ClearDiagram();
        }

        public DiagramViewModel Diagram1 { get; set; }

        private bool _CanEditNames;
        public bool CanEditNames
        {
            get { return _CanEditNames; }
            set
            {
                if (_CanEditNames != value)
                {
                    _CanEditNames = value;
                    Diagram1.CanEditNames = value;
                    NotifyOfPropertyChange(() => CanEditNames);
                }
            }
        }



        public void Close()
        {
            Diagram1.Dispose();
        }
        MetroWindow window;
        public void AttachView(object view, object context = null)
        {
            window = view as MetroWindow;
        }

        public object GetView(object context = null)
        {
            return window;
        }
        private PropertyChangedBase _RightPanel;
        public PropertyChangedBase RightPanel
        {
            get { return _RightPanel; }
            set
            {
                if (_RightPanel != value)
                {
                    _RightPanel = value;
                    NotifyOfPropertyChange(() => RightPanel);
                }
            }
        }
        public void WorldSettings()
        {
            RightPanel = new WorldSettingsViewModel(this);
        }
        public event System.EventHandler<ViewAttachedEventArgs> ViewAttached;
    }

 
}