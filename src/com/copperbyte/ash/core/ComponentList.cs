using System;
using System.Collections.Generic;

//package net.richardlord.ash.core
namespace com.copperbyte.ash.core
{
	/**
	 * Public class for making a component that is a list of other components
	 * so ComponentList<MyComponent> can be a component itself
	 */
	public class ComponentList<T> : Component, IEnumerable<T> where T:Component
	{
		internal List<T> mComponents;

		public int Count {
			get {
				return mComponents.Count;
			}
		}

		public ComponentList() {
			mComponents = new List<T>();
		}

		internal void Add( T component )
		{
			mComponents.Add(component);
		}

		internal void Remove( T component )
		{
			mComponents.Remove(component);
		}

		internal void Clear()
		{
			mComponents.Clear();
		}


		#region IEnumerable implementation

		public IEnumerator<T> GetEnumerator()
		{
			return mComponents.GetEnumerator();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return mComponents.GetEnumerator();
		}

		#endregion
				
	}
}
