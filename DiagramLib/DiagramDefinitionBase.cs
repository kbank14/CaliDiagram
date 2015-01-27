﻿using DiagramLib.Model;
using DiagramLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiagramLib
{
    internal class NodeBehaviour
    {
        public string Name
        {
            get;set;
        }
        public string Caption
        {
            get;set;
        }
        public Func<NodeBaseViewModel, DiagramNodeBase> ConvertViewModelToModel;
        public Func<DiagramNodeBase, NodeBaseViewModel> ConvertModelToViewModel;
        public Func<Point, NodeBaseViewModel> CreateNode;
        public Type TypeViewModel
        {
            get;
            set;
        }
        public Type TypeModel
        {
            get;
            set;
        }
    }
  
    public abstract class DiagramDefinitionBase
    {
        public DiagramDefinitionBase()
        {
            ConnectorSideStrategy = new VerticalFavourizedConnectionSrategy();
        }
        /// <summary>
        /// Returns System.Type array of models used in diagram
        /// </summary>
        public Type[] NodeTypes
        {
            get
            {
                return nodeBehaviours.Select(n => n.Value.TypeModel).ToArray();
            }
        }

        /// <summary>
        /// Returns true if node can be connected to itself
        /// </summary>
        public virtual bool CanConnectNodeToItself
        {
            get
            {
                return false;
            }
        }

        internal Dictionary<string, NodeBehaviour> nodeBehaviours = new Dictionary<string, NodeBehaviour>();
        
        public void AddModelFor<TViewModel, TModel>(string nodeTypeName,
            Func<Point, NodeBaseViewModel> createFunc,
            Func<NodeBaseViewModel, DiagramNodeBase> vmToM,
            Func<DiagramNodeBase, NodeBaseViewModel> modelToVm) 
            where TViewModel: NodeBaseViewModel
            where TModel: DiagramNodeBase
        {
            nodeBehaviours.Add(nodeTypeName, new NodeBehaviour() {
                TypeViewModel = typeof(TViewModel), 
                TypeModel = typeof(TModel),
                Name = nodeTypeName, 
                Caption = "to do",
                CreateNode = createFunc,
                ConvertViewModelToModel = vmToM,
                ConvertModelToViewModel = modelToVm
            });
        }

        /// <summary>
        /// Converts model to view model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal NodeBaseViewModel ModelToViewModel(DiagramNodeBase model)
        {
            var ctx = nodeBehaviours.FirstOrDefault(b => b.Value.TypeModel == model.GetType());
            if (ctx.Value == null)
                return null;
            if (ctx.Value.ConvertModelToViewModel == null)
                return null;
            return ctx.Value.ConvertModelToViewModel(model);
            
        }
        /// <summary>
        /// Converts view model to model
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        internal DiagramNodeBase ViewModelToModel(NodeBaseViewModel viewModel)
        {
            var ctx = nodeBehaviours.FirstOrDefault(b => b.Value.TypeViewModel == viewModel.GetType());
            if (ctx.Value == null)
                return null;
            if (ctx.Value.ConvertViewModelToModel == null)
                return null;
            return ctx.Value.ConvertViewModelToModel(viewModel);
        }
        /// <summary>
        /// Returns instance of connection view model that should be used for connection between from and to
        /// </summary>
        /// <param name="from">From node view model</param>
        /// <param name="to">To node view model</param>
        /// <returns>Connection view model or null if connection cannot be created</returns>
        public abstract ConnectionViewModel CreateConnection(NodeBaseViewModel from, NodeBaseViewModel to);
        public IConnectorSideStrategy ConnectorSideStrategy {get; set;}

        public virtual FrameworkElement CreateVisualForPacket(object packet)
        {          

            Rectangle aRectangle = new Rectangle();
            aRectangle.Width = 10;
            aRectangle.Height = 10;
            aRectangle.Fill = Brushes.Red;
            aRectangle.Stroke = Brushes.Black;
            aRectangle.StrokeThickness = 2;
            return aRectangle;
           
        }
    }
}
