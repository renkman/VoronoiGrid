using System;
using System.Collections.Generic;
using System.Linq;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;
using VoronoiEngine.Geomerty;
using VoronoiEngine.Models;
using VoronoiEngine.Utilities;

namespace VoronoiEngine.Structures
{
    public class BeachLine
    {
        private readonly ICircleEventCalculationService _circleEventCalculationService;
        private readonly IBreakpointCalculationService _breakpointCalculationService;
        private readonly Logger _logger;

        public BeachLine() : this(new CircleEventCalculationService(), new BreakpointCalculationService())
        {
        }

        public BeachLine(ICircleEventCalculationService circleEventCalculationService, IBreakpointCalculationService breakpointCalculationService)
        {
            _circleEventCalculationService = circleEventCalculationService;
            _breakpointCalculationService = breakpointCalculationService;
            _logger = Logger.Instance;
        }

        public INode Root { get; private set; }

        public CircleEvent FindCircleEventAbove(Point site)
        {
            if (Root == null)
                return null;

            var arc = Root.Find(site) as Leaf;
            if (arc?.CircleEvent == null)
                return null;

            return arc.CircleEvent;
        }

        public InsertSiteModel InsertSite(Point point)
        {
            var leaf = new Leaf(point);

            // Add new site to empty tree
            if (Root == null)
            {
                Root = leaf;
                _logger.Log($"Add {leaf.Site.ToString()} as Root");
                return null;
            }

            var root = Root;
            var result = root.Insert(point);

            // Reset root if root was a leaf
            if (root.IsLeaf)
                Root = root.Parent;
            return result;
        }

        public ICollection<CircleEvent> GenerateCircleEvent(ICollection<Leaf> sites, double y)
        {
            if (sites == null)
                return null;
            return sites.Select(s => _circleEventCalculationService.DetermineCircleEvent(s, y)).Where(e => e != null).ToList();
        }
               
        public Node RemoveLeaf(Leaf leaf)
        {
            //VParabola* gparent = p1->parent->parent;
            //if (p1->parent->Left() == p1)
            //{
            //    if (gparent->Left() == p1->parent) gparent->SetLeft(p1->parent->Right());
            //    if (gparent->Right() == p1->parent) gparent->SetRight(p1->parent->Right());
            //}
            //else
            //{
            //    if (gparent->Left() == p1->parent) gparent->SetLeft(p1->parent->Left());
            //    if (gparent->Right() == p1->parent) gparent->SetRight(p1->parent->Left());
            //}

            var parent = leaf.Parent as Node;
            var parentParent = parent.Parent as Node;
            leaf.Parent = null;
            parent.Parent = null;

            RemoveLeaf(leaf, parent, parentParent, parent.Left == leaf);
            if (parentParent == null)
                return null;

            return parentParent;
        }

        public override string ToString()
        {
            var output = "";
            BuildStringFromTree(Root, output, 0);
            return output;
        }

        private void RemoveLeaf(Leaf leaf, Node parent, Node parentParent, bool isLeft)
        {
            if (parentParent == null)
            {
                Root = isLeft ? parent.Right : parent.Left;
                return;
            }

            //if (isLeft)
            //    parent.Left = null;
            //else
            //    parent.Right = null;

            var sibling = isLeft ? parent.Right : parent.Left;
            sibling.Parent = parentParent;

            //if (isLeft)
            //    parent.Right = null;
            //else
            //    parent.Left = null;

            if (parentParent.Left == parent)
                parentParent.Left = sibling;
            else
                parentParent.Right = sibling;
        }

        private void BuildStringFromTree(INode node, string output, int level)
        {
            if (node == null)
                return;

            var tabs = string.Join("", Enumerable.Range(0, level).Select(n => "\t").ToArray());
            output += $"{tabs}{node.ToString()}{Environment.NewLine}";
            if (node.IsLeaf)
                return;

            var innerNode = node as Node;
            BuildStringFromTree(innerNode.Left, output, ++level);
            BuildStringFromTree(innerNode.Right, output, level);
            level--;
        }
    }
}