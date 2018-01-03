using System;
using VoronoiEngine.Elements;
using VoronoiEngine.Events;

namespace VoronoiEngine.Structures
{
    public class BeachLine
    {        
        public INode Root { get; private set; }
      
        public CircleEvent FindCircleEventAbove(Point site)
        {
            if (Root == null)
                return null;

            var arc = Root.Find(site) as Leaf;
            if (arc.CircleEvent == null)
                return null;

            return arc.CircleEvent;
        }

        public void InsertSite(Point point)
        {
            var leaf = new Leaf(point);

            // Add new site to empty tree
            if (Root == null)
            {
                Root = leaf;
                return;
            }

            // Add site to existing tree
            var rootNode = Root as Node;
            if (rootNode != null)
            {
                rootNode.Insert(point, ReplaceLeaf);
                return;
            }

            var rootLeaf = Root as Leaf;
            var node = new Node();
            Root = node;
            ReplaceLeaf(node, leaf, rootLeaf);
        }
        
        public void Remove(INode leaf)
        {
            throw new NotImplementedException();
        }

        private static void ReplaceLeaf(Node subRoot, Leaf newLeaf, Leaf arc)
        {
            // Build subtree
            var node = new Node();

            subRoot.Left = node;
            subRoot.Right = arc;
            subRoot.Breakpoint = new Tuple { Left = newLeaf.Site, Right = arc.Site };

            node.Left = arc;
            node.Right = newLeaf;
            node.Breakpoint = new Tuple { Left = arc.Site, Right = newLeaf.Site };           
        }
    }
}