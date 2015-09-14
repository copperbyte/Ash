using System;
using System.Collections.Generic;

//package net.richardlord.ash.core
namespace com.copperbyte.ash.core
{
	//import net.richardlord.ash.signals.Signal2;

	
	/**
	 * A game entity is a collection object for components. Sometimes, the entities in a game
	 * will mirror the actual characters and objects in the game, but this is not necessary.
	 * 
	 * <p>Components are simple value objects that contain data relevant to the entity. Entities
	 * with similar functionality will have instances of the same components. So we might have 
	 * a position component</p>
	 * 
	 * <p><code>public class PositionComponent
	 * {
	 *   public var x : Number;
	 *   public var y : Number;
	 * }</code></p>
	 * 
	 * <p>All entities that have a position in the game world, will have an instance of the
	 * position component. Systems operate on entities based on the components they have.</p>
	 */
	public class Entity
	{
		/**
		 * Optional, give the entity a name. This can help with debugging and with serialising the entity.
		 */
		public String name;
		/**
		 * This signal is dispatched when a component is added to the entity.
		 */
		//public var componentAdded : Signal2;
		/**
		 * This signal is dispatched when a component is removed from the entity.
		 */
		//public var componentRemoved : Signal2;
		internal delegate void ComponentChanged(Entity entity, Type componentType);
		internal ComponentChanged componentAdded, componentRemoved;
		
		internal Dictionary<Type, Component> mComponents;

		public Entity()
		{
			//componentAdded = new Signal2( Entity, Class );
			//componentRemoved = new Signal2( Entity, Class );
			mComponents = new Dictionary<Type, Component>();
		}

		/**
		 * Add a component to the entity.
		 * 
		 * @param component The component object to add.
		 * @param componentClass The class of the component. This is only necessary if the component
		 * extends another component class and you want the framework to treat the component as of 
		 * the base class type. If not set, the class type is determined directly from the component.
		 */
		public void Add( Component component )
		{
			//if ( componentClass != null)
			//{
				Type componentClass = component.GetType();
			//}
			if ( mComponents.ContainsKey( componentClass ) )
			{
				Remove( componentClass );
			}
			mComponents[ componentClass ] = component;
			if(componentAdded != null)
				componentAdded( this, componentClass );
		}

		public void Add<CT>( CT component ) where CT : Component
		{
			//if ( componentClass != null)
			//{
			Type componentClass = typeof(CT);
			//}
			if ( mComponents.ContainsKey( componentClass ) )
			{
				Remove( componentClass );
			}
			mComponents[ componentClass ] = (Component)component;
			if(componentAdded != null)
				componentAdded( this, componentClass );
		}

		/**
		 * Remove a component from the entity.
		 * 
		 * @param componentClass The class of the component to be removed.
		 * @return the component, or null if the component doesn't exist in the entity
		 */
		public Component Remove( Type componentClass )
		{
			if ( mComponents.ContainsKey(componentClass) )
			{
				Component component = mComponents[ componentClass ];
				mComponents.Remove(componentClass);
				if(componentRemoved != null)
					componentRemoved( this, componentClass );
				return component;
			}
			return null;
		}

		public Component Remove<CT>() where CT : Component
		{
			Type componentClass = typeof(CT);
			if ( mComponents.ContainsKey(componentClass) )
			{
				Component component = mComponents[ componentClass ];
				mComponents.Remove(componentClass);
				if(componentRemoved != null)
					componentRemoved( this, componentClass );
				return component;
			}
			return null;
		}

		/**
		 * Get a component from the entity.
		 * 
		 * @param componentClass The class of the component requested.
		 * @return The component, or null if none was found.
		 */
		public Component Get( Type componentClass )
		{
			return mComponents[ componentClass ];
		}
		
		public CT Get<CT>() where CT:Component 
		{
			return (CT)mComponents [ typeof(CT) ];	
		}

		/**
		 * Does the entity have a component of a particular type.
		 * 
		 * @param componentClass The class of the component sought.
		 * @return true if the entity has a component of the type, false if not.
		 */
		public bool Has( Type componentClass )
		{
			return mComponents.ContainsKey(componentClass);
		}
		
		public bool Has<CT>( ) where CT : Component
		{
			return mComponents.ContainsKey(typeof(CT));
		}
		
		/**
		 * Make a copy of the entity
		 * 
		 * @return A new entity with new components that are copies of the components on the
		 * original entity.
		 */
		public Entity Clone()
		{
			Entity copy = new Entity();
			foreach( KeyValuePair<Type, Component> curr in mComponents )
			{
				//var names : XMLList = describeType( component ).variable.@name;
				Component newComponent = curr.Value.Clone();
				//curr.Key newComponent = new curr.Key(curr.Value);
				copy.Add(newComponent);
				//for each( var key : String in names )
				//{
				//	newComponent[key] = component[key];
				//}
			}
			return copy;
		}
	}
}
