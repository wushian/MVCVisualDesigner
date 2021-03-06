﻿using Microsoft.VisualStudio.Modeling.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Model
namespace MVCVisualDesigner
{
    public partial class VDSeparator
    {
    }

    public partial class VDHoriSeparator
    {
    }

    public partial class VDVertSeparator
    {
    }
}


// Shape
namespace MVCVisualDesigner
{
    public partial class VDHoriSeparatorShape
    {
        protected override void SetAbsoluteBoundsValue(RectangleD newValue)
        {
            base.SetAbsoluteBoundsValue(newValue);
            if (!this.Store.InUndoRedoOrRollback)
            {
                this.PerformShapeAnchoringRule();
            }
        }

        public override NodeSides ResizableSides { get { return NodeSides.None; } }

        public override BoundsRules BoundsRules { get { return HorizontalSeparatorShapeBoundsRules.Instance; } }

        internal class HorizontalSeparatorShapeBoundsRules : BoundsRules
        {
            internal static readonly HorizontalSeparatorShapeBoundsRules Instance = new HorizontalSeparatorShapeBoundsRules();

            public override RectangleD GetCompliantBounds(ShapeElement shape, RectangleD proposedBounds)
            {
                VDHoriSeparatorShape separatorShape = shape as VDHoriSeparatorShape;
                if (separatorShape == null) { throw new ArgumentNullException("horizontalSeparatorShape"); }

                VDWidgetShape parentShape = separatorShape.ParentShape as VDWidgetShape;
                if (parentShape == null || separatorShape.Store.InUndoRedoOrRollback) { return proposedBounds; }

                VDHoriSeparator model = separatorShape.ModelElement as VDHoriSeparator;
                if (model == null) return proposedBounds;

                bool bFlagBoundsSet = false;
                PointD location = proposedBounds.Location;
                SizeD size = proposedBounds.Size;

                // anchor to parent widget
                if (!separatorShape.Anchoring.HasLeftAnchor) separatorShape.Anchoring.SetLeftAnchor(0);
                if (!separatorShape.Anchoring.HasRightAnchor) separatorShape.Anchoring.SetRightAnchor(1);
                
                // anchor to top widget
                if (model.TopWidget != null)
                {
                    NodeShape topWidgetShape = parentShape.NestedChildShapes.Find(c => c.ModelElement == model.TopWidget) as NodeShape;
                    if (topWidgetShape != null)
                    {
                        if (!topWidgetShape.Anchoring.HasBottomAnchor)
                            topWidgetShape.Anchoring.SetBottomAnchor(separatorShape, AnchoringBehavior.Edge.Top, model.TopMargin);
                        //if (!separatorShape.Anchoring.HasLeftAnchor)
                        //    separatorShape.Anchoring.SetLeftAnchor(topWidgetShape, AnchoringBehavior.Edge.Left, 0);
                        //if (!separatorShape.Anchoring.HasRightAnchor)
                        //    separatorShape.Anchoring.SetRightAnchor(topWidgetShape, AnchoringBehavior.Edge.Right, 0);

                        // set bounds of HorizonalSeparator according to top widget
                        //location.X = topWidgetShape.Bounds.Left;
                        if (location.Y < VDConstants.DOUBLE_DIFF || location.Y + size.Height + VDConstants.DOUBLE_DIFF > parentShape.Bounds.Height)
                            location.Y = topWidgetShape.Bounds.Bottom + model.TopMargin;
                        bFlagBoundsSet = true;
                    }
                }

                // anchor to bottom widget
                if (model.BottomWidget != null)
                {
                    NodeShape bottomWidgetShape = parentShape.NestedChildShapes.Find(c => c.ModelElement == model.BottomWidget) as NodeShape;
                    if (bottomWidgetShape != null)
                    {
                        if (!bottomWidgetShape.Anchoring.HasTopAnchor)
                            bottomWidgetShape.Anchoring.SetTopAnchor(separatorShape, AnchoringBehavior.Edge.Bottom, model.BottomMargin);
                        //if (!separatorShape.Anchoring.HasLeftAnchor)
                        //    separatorShape.Anchoring.SetLeftAnchor(bottomWidgetShape, AnchoringBehavior.Edge.Left, 0);
                        //if (!separatorShape.Anchoring.HasRightAnchor)
                        //    separatorShape.Anchoring.SetRightAnchor(bottomWidgetShape, AnchoringBehavior.Edge.Right, 0);

                        // set bounds of handle according to bottom widget
                        if (!bFlagBoundsSet)
                        {
                            //location.X = bottomWidgetShape.Bounds.Left;
                            if (location.Y < VDConstants.DOUBLE_DIFF || location.Y + size.Height + VDConstants.DOUBLE_DIFF > parentShape.Bounds.Height)
                                location.Y = bottomWidgetShape.Bounds.Top - size.Height - model.BottomMargin;
                        }
                    }
                }

                // set initial Y position
                if (model.DefaultY >= 0)
                {
                    location.Y = model.DefaultY;
                    model.DefaultY = -1;
                }

                return new RectangleD(location, size);
            }
        }
    }

    public partial class VDVertSeparatorShape
    {
        protected override void SetAbsoluteBoundsValue(RectangleD newValue)
        {
            base.SetAbsoluteBoundsValue(newValue);
            this.PerformShapeAnchoringRule();
        }

        public override NodeSides ResizableSides { get { return NodeSides.None; } }

        public override BoundsRules BoundsRules { get { return VerticalSeparatorShapeBoundsRules.Instance; } }

        internal class VerticalSeparatorShapeBoundsRules : BoundsRules
        {
            internal static readonly VerticalSeparatorShapeBoundsRules Instance = new VerticalSeparatorShapeBoundsRules();

            public override RectangleD GetCompliantBounds(ShapeElement shape, RectangleD proposedBounds)
            {
                VDVertSeparatorShape separatorShape = shape as VDVertSeparatorShape;
                if (separatorShape == null) { throw new ArgumentNullException("horizontalSeparatorShape"); }

                VDWidgetShape parentShape = separatorShape.ParentShape as VDWidgetShape;
                if (parentShape == null || separatorShape.Store.InUndoRedoOrRollback) { return proposedBounds; }

                VDVertSeparator model = separatorShape.ModelElement as VDVertSeparator;
                if (model == null) return proposedBounds;

                bool bFlagBoundsSet = false;
                PointD location = proposedBounds.Location;
                SizeD size = proposedBounds.Size;

                // anchor to parent 
                if (!separatorShape.Anchoring.HasTopAnchor) separatorShape.Anchoring.SetTopAnchor(0);
                if (!separatorShape.Anchoring.HasBottomAnchor) separatorShape.Anchoring.SetBottomAnchor(1);

                // anchor to left widget
                if (model.LeftWidget != null)
                {
                    NodeShape leftWidgetShape = parentShape.NestedChildShapes.Find(c => c.ModelElement == model.LeftWidget) as NodeShape;
                    if (leftWidgetShape != null)
                    {
                        if (!leftWidgetShape.Anchoring.HasRightAnchor)
                            leftWidgetShape.Anchoring.SetRightAnchor(separatorShape, AnchoringBehavior.Edge.Left, model.LeftMargin);
                        //if (!separatorShape.Anchoring.HasTopAnchor)
                        //    separatorShape.Anchoring.SetTopAnchor(leftWidgetShape, AnchoringBehavior.Edge.Top, 0);
                        //if (!separatorShape.Anchoring.HasBottomAnchor)
                        //    separatorShape.Anchoring.SetBottomAnchor(leftWidgetShape, AnchoringBehavior.Edge.Bottom, 0);

                        // set bounds of separator according to left widget
                        if (location.X < VDConstants.DOUBLE_DIFF || location.X + size.Width + VDConstants.DOUBLE_DIFF > parentShape.Bounds.Width)
                            location.X = leftWidgetShape.Bounds.Right + model.LeftMargin;
                        //location.Y = leftWidgetShape.Bounds.Top;
                        bFlagBoundsSet = true;
                    }
                }

                // anchor to right widget
                if (model.RightWidget != null)
                {
                    NodeShape rightWidgetShape = parentShape.NestedChildShapes.Find(c => c.ModelElement == model.RightWidget) as NodeShape;
                    if (rightWidgetShape != null)
                    {
                        if (!rightWidgetShape.Anchoring.HasLeftAnchor)
                            rightWidgetShape.Anchoring.SetLeftAnchor(separatorShape, AnchoringBehavior.Edge.Right, model.RightMargin);
                        //if (!separatorShape.Anchoring.HasTopAnchor)
                        //    separatorShape.Anchoring.SetTopAnchor(rightWidgetShape, AnchoringBehavior.Edge.Top, 0);
                        //if (!separatorShape.Anchoring.HasBottomAnchor)
                        //    separatorShape.Anchoring.SetBottomAnchor(rightWidgetShape, AnchoringBehavior.Edge.Bottom, 0);

                        // set bounds of handle according to bottom widget
                        if (!bFlagBoundsSet)
                        {
                            if (location.X < VDConstants.DOUBLE_DIFF || location.X + size.Width + VDConstants.DOUBLE_DIFF > parentShape.Bounds.Width)
                                location.X = rightWidgetShape.Bounds.Left - size.Width - model.RightMargin; ;
                            //location.Y = rightWidgetShape.Bounds.Top; 
                        }
                    }
                }

                // set initial Y position
                if (model.DefaultX >= 0)
                {
                    location.Y = model.DefaultX;
                    model.DefaultX = -1;
                }

                return new RectangleD(location, size);
            }
        }
    }
}
