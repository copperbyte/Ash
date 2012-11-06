using System;
using System.Collections.Generic;

//package net.richardlord.ash.core
namespace com.copperbyte.ash.core
{
	/**
	 * This internal class maintains a pool of deleted nodes for reuse by framework. This reduces the overhead
	 * from object creation and garbage collection.
	 */
	internal class NodePool<NT> where NT:Node, new()
	{
		private Queue<NT> mQueue;
		private List<NT> mCache;
		
		//private Node tail;
		private Type nodeType;
		//private Node cacheTail;

		public NodePool( Type nodeType )
		{
			this.nodeType = nodeType;
			mQueue = new Queue<NT>();
			mCache = new List<NT>();
		}

		internal NT get()
		{
			if ( mQueue.Count > 0 )
			{
				NT node = mQueue.Dequeue();
				return node;
			}
			else
			{
				return new NT();
			}
		}

		internal void dispose( NT node ) 
		{
			mQueue.Enqueue(node);
		}
		
		internal void cache( NT node )
		{
			mCache.Add(node);
		}
		
		internal void releaseCache()
		{
			foreach(NT node in mCache) {
				mQueue.Enqueue(node);
			}
			mCache.Clear();	
		}
	}
}
