using System;
using System.Collections.Generic;

//package net.richardlord.ash.core
namespace com.copperbyte.ash.core
{
	/**
	 * An internal class for a linked list of entities. Used inside the framework for
	 * managing the entities.
	 */
	internal class EntityList
	{
		// snip linked list implimentation
		internal List<Entity> mEntitys;
		
		internal EntityList () {
			mEntitys = new List<Entity>();
		}
		
		internal void Add( Entity entity )
		{
			mEntitys.Add(entity);
		}
		
		internal void Remove( Entity entity )
		{
			mEntitys.Remove(entity);
		}
		
		internal void Clear()
		{
			mEntitys.Clear();
		}
	}
}
