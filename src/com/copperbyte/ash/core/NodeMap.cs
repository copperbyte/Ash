using System;
using System.Collections.Generic;

//package net.richardlord.ash.core
namespace com.copperbyte.ash.core
{
	//import net.richardlord.ash.signals.Signal1;
	internal interface INodeMap
	{
		void Add(Node node);
		void Remove(Node node);
		void RemoveAll();

		bool Empty();

	}

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
	public class NodeMap<CK, NT> : INodeMap, IEnumerable<KeyValuePair<CK, NT>> where CK:Component where NT:Node
	{
		// snip linked list implimentation
		internal Dictionary<CK, NT> mNodes;	
		
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
		
		public NodeMap()
		{
			mNodes = new Dictionary<CK, NT>();
			//nodeAdded = new Signal1( Node );
			//nodeRemoved = new Signal1( Node );
		}
		
		public void Add( Node node )
		{
			mNodes.Add( (CK)node.entity.Get(typeof(CK)), (NT)node);
			if(nodeAdded != null)
				nodeAdded( (NT)node );
		}
		
		public void Remove( Node node )
		{
			mNodes.Remove( (CK)node.entity.Get(typeof(CK)) );
			if(nodeRemoved != null)
				nodeRemoved((NT)node);
		}
		
		public void RemoveAll()
		{
			foreach(CK key in mNodes.Keys) {
				NT node = mNodes[key];
				mNodes.Remove(key);
				if(nodeRemoved != null)
					nodeRemoved(node);
			}
		}
		
		public Dictionary<CK, NT>  Get() 
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
		/*
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
		*/

		#region IReadOnlyDictionary<TKey, TValue> implementation
		public int Count {
			get {
				return mNodes.Count;
			}	
		}
		
		public NT this[CK index] {
			get {
				return mNodes[index];
			}
			//set {
			//	mNodes[index] = value;
			//}
		}

		public IEnumerable<CK> Keys { 
			get {
				return mNodes.Keys;
			}
		}

		public IEnumerable<NT> Values { 
			get {
				return mNodes.Values;
			}
		}

		public bool ContainsKey(CK key) 
		{
			return mNodes.ContainsKey(key);
		}

		public bool TryGetValue(CK key, out NT value) {
			return mNodes.TryGetValue(key, out value);
		}
		#endregion

		#region IEnumerable implementation

		public IEnumerator<KeyValuePair<CK, NT>> GetEnumerator()
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
