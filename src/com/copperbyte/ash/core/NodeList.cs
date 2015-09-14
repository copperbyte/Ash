using System;
using System.Collections.Generic;

//package net.richardlord.ash.core
namespace com.copperbyte.ash.core
{
	//import net.richardlord.ash.signals.Signal1;
	
	/**
	 * A collection of nodes.
	 * 
	 * <p>Systems within the game access the components of entities via NodeLists. A NodeList contains
	 * a node for each Entity in the game that has all the components required by the node. To iterate
	 * over a NodeList, start from the head and step to the next on each loop, until the returned value
	 * is null.</p>
	 * 
	 * <p>for( var node : Node = nodeList.head; node; node = node.next )
	 * {
	 *   // do stuff
	 * }</p>
	 * 
	 * <p>It is safe to remove items from a nodelist during the loop. When a Node is removed form the 
	 * NodeList it's previous and next properties still point to the nodes that were before and after
	 * it in the NodeList just before it was removed.</p>
	 */
	public class NodeList<NT> : /*IReadOnlyList<NT>,*/ IEnumerable<NT> where NT:Node
	{
		// snip linked list implimentation
		internal List<NT> mNodes;	
		
		/**
		 * A signal that is dispatched whenever a node is added to the node list.
		 * 
		 * <p>The signal will pass a single parameter to the listeners - the node that was added.
		 */
		//public var nodeAdded : Signal1;
		/**
		 * A signal that is dispatched whenever a node is removed from the node list.
		 * 
		 * <p>The signal will pass a single parameter to the listeners - the node that was removed.
		 */
		//public var nodeRemoved : Signal1;
		internal delegate void NodeChanged( NT node );
		internal NodeChanged nodeAdded, nodeRemoved;
		
		public NodeList()
		{
			mNodes = new List<NT>();
			//nodeAdded = new Signal1( Node );
			//nodeRemoved = new Signal1( Node );
		}
		
		internal void Add( NT node )
		{
			mNodes.Add(node);
			if(nodeAdded != null)
				nodeAdded( node );
		}
		
		internal bool Remove( NT node )
		{
			bool result = mNodes.Remove(node);
			if(nodeRemoved != null) 
				nodeRemoved( node );
			return result;
		}
		
		internal void Clear()
		{
			foreach(NT node in mNodes) {
				mNodes.Remove(node);
				nodeRemoved( node );
			}
		}
		
		public List<NT> Get() 
		{
			return mNodes;
		}
		
		/**
		 * true if the list is empty, false otherwise.
		 */
		public bool Empty()
		{
			return mNodes.Count == 0;		
		}
		
		/**
		 * Swaps the positions of two nodes in the list. Useful when sorting a list.
		 */
		public void Swap( NT node1, NT node2 )
		{
			int index1, index2;
			index1 = mNodes.IndexOf(node1);
			index2 = mNodes.IndexOf(node2);
			
			if(index1 == index2)
				return;
			
			if(index1 < index2) {
				mNodes.Insert(index2, node1);
				mNodes.RemoveAt(index1);
				mNodes.Insert(index1, node2);
				mNodes.RemoveAt(index2);
			} else if(index1 > index2) {
				mNodes.Insert(index1, node2);
				mNodes.RemoveAt(index2);
				mNodes.Insert(index2, node1);
				mNodes.RemoveAt(index1);
			}			
		}


		public int IndexOf(NT item)
		{
			return mNodes.IndexOf(item);
		}

		#region IReadOnlyList<T> implementation

		public int Count {
			get {
				return mNodes.Count;
			}

		}

		public NT this[int index] {
			get {
				return mNodes[index];
			}
			//set {
			//	mNodes[index] = value;
			//}
		}
		#endregion

		#region IEnumerable implementation

		public IEnumerator<NT> GetEnumerator()
		{
			return mNodes.GetEnumerator();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return mNodes.GetEnumerator();
		}

		#endregion
	}
}
