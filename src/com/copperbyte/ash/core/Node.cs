using System;
using System.Collections.Generic;

//package net.richardlord.ash.core
namespace com.copperbyte.ash.core
{
	/**
	 * Base class for a node.
	 * 
	 * <p>A node is a set of different components that are required by a system.
	 * A system can request a collection of nodes from the game. Subsequently the Game object creates
	 * a node for every entity that has all of the components in the node class and adds these nodes
	 * to the list obtained by the system. The game keeps the list up to date as entities are added
	 * to and removed from the game and as the components on entities change.</p>
	 */
	public class Node
	{
		/**
		 * The entity whose components are included in the node.
		 */
		public Entity entity;
		
		// snip linked list implimentation
		
		//internal Dictionary<Type, Component> mComponents;
		public Node()
		{
			;//mComponents = new Dictionary<Type, Component>();
		}

		// For other components than the mandatory properties
		public CT Get<CT>() where CT:Component {
			return entity.Get<CT>();
		}
		public bool Has<CT>( ) where CT : Component {
			return entity.Has<CT>();
		}
		
		/* Example Node Class, must use Properties like this
		 * public class TileRenderNode : Node
		{
			public TilePosition TilePos  {
				get { return (TilePosition)entity.Get(typeof(TilePosition)); }
				set { entity.Add( value, value.GetType() ); }	
			}
			public PixSpriteRect Sprite {
				get { return (PixSpriteRect)entity.Get(typeof(PixSpriteRect)); }
				set { entity.Add( value, value.GetType() ); }	
			}
		} */
	}
}
