using System;
using System.Collections.Generic;

//package net.richardlord.ash.core
namespace com.copperbyte.ash.core
{
	/**
	 * Base class for a component.
	 * 
	 * <p>A component is an object that can be stored in an Entity and a Node</p>
	 */
	public class Component
	{
		/**
		 * The component
		 */
		//public Entity entity;
		
		public Component Clone() { return new Component(); }
		
	}
}
