using System;
using System.Collections.Generic;

//package net.richardlord.ash.core
namespace com.copperbyte.ash.core
{
	//import net.richardlord.ash.signals.Signal0;

	/**
	 * The Ash class is the central point for creating and managing your game state. Add
	 * entities and processes to the game, and fetch families of nodes from the game.
	 */
	public class Ash
	{
		private EntityList entities;
		private ProcessList processes;
		private Dictionary<Type, IFamily> families;
		
		/**
		 * Indicates if the game is currently in its update loop.
		 */
		public bool updating;
		
		/**
		 * Dispatched when the update loop ends. If you want to add and remove processes from the
		 * game it is usually best not to do so during the update loop. To avoid this you can
		 * listen for this signal and make the change when the signal is dispatched.
		 */
		//public var updateComplete : Signal0;
		public delegate void UpdateComplete();
		public UpdateComplete updateComplete;
		
		public Ash()
		{
			entities = new EntityList();
			processes = new ProcessList();
			families = new Dictionary<Type, IFamily>();
			//updateComplete = new Signal0();
		}
		
		/**
		 * Add an entity to the game.
		 * 
		 * @param entity The entity to add.
		 */
		public void AddEntity( Entity entity )
		{
			entities.Add( entity );
			entity.componentAdded += this.ComponentAdded;
			foreach( KeyValuePair<Type, IFamily> pair in families )
			{
				pair.Value.AddIfMatch( entity );
			}
		}
		
		/**
		 * Remove an entity from the game.
		 * 
		 * @param entity The entity to remove.
		 */
		public void RemoveEntity( Entity entity )
		{
			foreach( KeyValuePair<Type, IFamily> pair in families )
			{
				pair.Value.Remove( entity );
			}
			entity.componentAdded -= this.ComponentAdded;
			entities.Remove( entity );
		}

		/**
		 * How many entities exist in the system. Mainly for debugging/testing, 
		 *   seeing if a section cleans up everything it creates. 
		 */
		public int EntityCount() {
			return entities.mEntitys.Count;
		}

		/**
		 * @private
		 */
		private void ComponentAdded( Entity entity, Type componentClass )
		{
			foreach( KeyValuePair<Type, IFamily> pair in families )
			{
				pair.Value.AddIfMatch( entity );
			}
		}

		/**
		 *  Check if an entity is also a node 
		 *  This is a 'light' function, that only checks already known entities
		 *  It will return false for any Entity that was not Added to the Ash
		 */
		public bool Is<NT>(Entity entity) where NT:Node, new() 
		{
			Type nodeClass = typeof(NT);
			if( families.ContainsKey(nodeClass) ) {  
				Family<NT> family = (Family<NT>)families[nodeClass];
				return family.Is(entity);
			} else {
				Family<NT> newFamily = new Family<NT>( nodeClass, this );
				families[nodeClass] = newFamily;
				foreach( Entity old_entity in entities.mEntitys ) {
					newFamily.AddIfMatch( old_entity );
				}
				return newFamily.Is(entity);
			}
		}

		/**
		 *  Get an entity as a node if it qualifies
		 *  This is a 'light' function, that only checks already known entities
		 *  It will return null for any Entity that was not Added to the Ash
		 */
		public NT As<NT>(Entity entity) where NT:Node, new() 
		{
			Type nodeClass = typeof(NT);
			if( families.ContainsKey(nodeClass) ) {  
				Family<NT> family = (Family<NT>)families[nodeClass];
				return family.As(entity);
			} else {
				Family<NT> newFamily = new Family<NT>( nodeClass, this );
				families[nodeClass] = newFamily;
				foreach( Entity old_entity in entities.mEntitys ) {
					newFamily.AddIfMatch( old_entity );
				}
				return newFamily.As(entity);
			}
		}

		/**
		 * Get a collection of nodes from the game, based on the type of the node required.
		 * 
		 * <p>The game will create the appropriate NodeList if it doesn't already exist and 
		 * will keep its contents up to date as entities are added to and removed from the
		 * game.</p>
		 * 
		 * <p>If a NodeList is no longer required, release it with the releaseNodeList method.</p>
		 * 
		 * @param nodeClass The type of node required.
		 * @return A linked list of all nodes of this type from all entities in the game.
		 */
		public NodeList<NT> GetNodeList<NT> () where NT:Node, new()
		{
			Type nodeClass = typeof(NT);
			if( families.ContainsKey(nodeClass) )
			{  
				Family<NT> family = (Family<NT>)families[nodeClass];
				return family.nodes;
			}
			Family<NT> newFamily = new Family<NT>( nodeClass, this );
			families[nodeClass] = newFamily;
			foreach( Entity entity in entities.mEntitys )
			{
				newFamily.AddIfMatch( entity );
			}
			return newFamily.nodes;
		}

		public NodeMap<CK,NT> GetNodeMap<CK,NT> () where CK:Component where NT:Node, new()
		{
			Type compClass = typeof(CK);
			Type nodeClass = typeof(NT);
			if( families.ContainsKey(nodeClass) )
			{  
				Family<NT> family = (Family<NT>)families[nodeClass];
				return family.GetMap<CK>();
			}
			Family<NT> newFamily = new Family<NT>( nodeClass, this );
			families[nodeClass] = newFamily;
			NodeMap<CK,NT> newMap = newFamily.GetMap<CK>();
			foreach( Entity entity in entities.mEntitys )
			{
				newFamily.AddIfMatch( entity );
			}
			return newMap;
		}

		public NodeMultiMap<CK,NT> GetNodeMultiMap<CK,NT> () where CK:Component where NT:Node, new()
		{
			Type compClass = typeof(CK);
			Type nodeClass = typeof(NT);
			if( families.ContainsKey(nodeClass) )
			{  
				Family<NT> family = (Family<NT>)families[nodeClass];
				return family.GetMultiMap<CK>();
			}
			Family<NT> newFamily = new Family<NT>( nodeClass, this );
			families[nodeClass] = newFamily;
			NodeMultiMap<CK,NT> newMultiMap = newFamily.GetMultiMap<CK>();
			foreach( Entity entity in entities.mEntitys )
			{
				newFamily.AddIfMatch( entity );
			}
			return newMultiMap;
		}

		
		/**
		 * If a NodeList is no longer required, this method will stop the game updating
		 * the list and will release all references to the list within the framework
		 * classes, enabling it to be garbage collected.
		 * 
		 * <p>It is not essential to release a list, but releasing it will free
		 * up memory and processor resources.</p>
		 * 
		 * @param nodeClass The type of the node class if the list to be released.
		 */
		public void ReleaseNodeList( Type nodeClass ) 
		{
			// Ref-count Families?
			if( families.ContainsKey(nodeClass) )
			{
				if(!families[nodeClass].InUse())
					families[nodeClass].CleanUp();
			}
			families.Remove(nodeClass);
		}
		public void ReleaseNodeMap( Type nodeClass ) 
		{
			/* ??? */ ;
		}
		public void ReleaseNodeMultiMap( Type nodeClass ) 
		{
			/* ??? */ ;
		}
		
		/**
		 * Add a system to the game, and set its priority for the order in which the
		 * processes are updated by the game loop.
		 * 
		 * <p>The priority dictates the order in which the processes are updated by the game 
		 * loop. Lower numbers for priority are updated first. i.e. a priority of 1 is 
		 * updated before a priority of 2.</p>
		 * 
		 * @param system The system to add to the game.
		 * @param priority The priority for updating the processes during the game loop. A 
		 * lower number means the system is updated sooner.
		 */
		public void AddProcess( Process process, int priority )
		{
			process.priority = priority;
			process.AddToAsh( this );
			processes.Add( process );
		}
		
		/**
		 * Remove a system from the game.
		 * 
		 * @param system The system to remove from the game.
		 */
		public void RemoveProcess( Process process )
		{
			processes.Remove( process );
			process.RemoveFromAsh( this );
		}

		/**
		 * Update the game. This causes the game loop to run, calling update on all the
		 * processes in the game.
		 * 
		 * <p>The package net.richardlord.ash.tick contains classes that can be used to provide
		 * a steady or variable tick that calls this update method.</p>
		 * 
		 * @time The duration, in seconds, of this update step.
		 */
		public void Update( Double time )
		{
			updating = true;
			foreach( KeyValuePair<int, List<Process> > priority in processes.mProcesses )
			{
				foreach( Process process in priority.Value) {
					process.Update( time );
				}
			}
			updating = false;
			
			if(updateComplete != null) {
				updateComplete();
			}
		}
	}
}
