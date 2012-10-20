using System;
using System.Collections.Generic;
using System.Reflection;

//package net.richardlord.ash.core
namespace com.copperbyte.ash.core
{
	//import flash.utils.Dictionary;
	//import flash.utils.describeType;
	//import flash.utils.getDefinitionByName;
	
	internal interface IFamily
	{
		//internal NodeList nodes;
		//internal Dictionary<Entity, Node> entities;
		//private Type nodeClass;
		//private Dictionary<Type, String> components;
		//private NodePool nodePool;
		//private Ash ash;

		//public Family(Ash ash)
		//{
		//	this.nodeClass = nodeClass;
		//	this.ash = ash;
		//	init();
		//}
		
		//NodeList<NT> GetList<NT>() where NT:Node;
		
		void AddIfMatch( Entity entity );
		
		void Remove( Entity entity );
		
		void CleanUp();


		bool InUse();
		
	}
	
	/**
	 * An internal class for managing a NodeList. This class creates the NodeList and adds and removes
	 * nodes to/from the list as the entities and the components in the game change.
	 */
	internal class Family<NT> : IFamily where NT : Node, new()
	{
		internal NodeList<NT> nodes;
		internal Dictionary<Entity, NT> entities;
		private Type nodeClass;
		private Dictionary<Type, String> components;
		private NodePool<NT> nodePool;
		private Ash ash;

		internal Dictionary<System.Type, INodeMap > maps;
		internal Dictionary<System.Type, INodeMultiMap > multimaps;


		public Family( Type nodeClass, Ash ash )
		{
			this.nodeClass = nodeClass;
			this.ash = ash;
			init();
		}

		private void init() 
		{
			nodePool = new NodePool<NT>( nodeClass );
			nodes = new NodeList<NT>();
			entities = new Dictionary<Entity, NT>();
			maps = new Dictionary<System.Type, INodeMap >();
			multimaps = new Dictionary<System.Type, INodeMultiMap >();


			components = new Dictionary<Type, String>();
			nodePool.dispose( nodePool.get() ); // create a dummy instance to ensure describeType works.
			
			System.Console.WriteLine(" Family.init nodeClass : {0} ", nodeClass.ToString());

			// I like how it solves my problems, but I don't like that it uses Reflection
			PropertyInfo[] props = nodeClass.GetProperties();
			foreach(PropertyInfo prop in props) {
				Type type = prop.PropertyType;
				if(type.IsClass && type.IsPublic &&
				   components.ContainsKey(type) == false) {
					if(type.Name == "Entity" && prop.Name == "entity")
						continue;
					System.Console.WriteLine(" Family.init props : {0} : {1} ", type.Name, prop.Name);

					components.Add(type, type.Name);
				}
			}
			
			/*
			FieldInfo[] fields = nodeClass.GetFields();
			foreach(FieldInfo field in fields) {
				Type declType = field.FieldType;
				
				//member.DeclaringType.GetFields
				if(declType.IsClass && declType.IsPublic && 
				   components.ContainsKey(declType) == false) {
					if(declType.Name == "Entity" && field.Name == "entity")
						continue;
					System.Console.WriteLine(" Family.init fields : {0} : {1} ", declType.Name, field.Name);

					components.Add(declType, declType.Name);
				}
			}
			*/
			
			/*
			var variables : XMLList = describeType( nodeClass ).factory.variable;
			for each ( var atom:XML in variables )
			{
				if ( atom.@name != "entity" && atom.@name != "previous" && atom.@name != "next" )
				{
					var componentClass : Class = getDefinitionByName( atom.@type ) as Class;
					components[componentClass] = atom.@name.toString();
				}
			}*/
		}		
		
		public NodeList<NT> GetList()
		{
			return nodes;
		}

		public NodeMap<CK, NT> GetMap<CK>() where CK:Component
		{
			if( maps.ContainsKey( typeof(CK) ) ) {
				NodeMap<CK,NT> map = (NodeMap<CK,NT>)maps[typeof(CK)];
				return map;
			} 
			else {
				NodeMap<CK, NT> newMap = new NodeMap<CK, NT>();

				foreach( NT node in nodes.mNodes) {
					newMap.Add(node);
				}

				maps.Add(typeof(CK), (INodeMap)newMap);

				return newMap;
			}
		}

		public NodeMultiMap<CK, NT> GetMultiMap<CK>() where CK:Component
		{
			if( multimaps.ContainsKey( typeof(CK) ) ) {
				NodeMultiMap<CK,NT> multimap = (NodeMultiMap<CK,NT>)multimaps[typeof(CK)];
				return multimap;
			} 
			else {
				NodeMultiMap<CK, NT> newMultiMap = new NodeMultiMap<CK, NT>();

				foreach( NT node in nodes.mNodes) {
					newMultiMap.Add(node);
				}

				multimaps.Add(typeof(CK), (INodeMultiMap)newMultiMap);

				return newMultiMap;
			}
		}




		public void AddIfMatch( Entity entity )
		{
			if( !entities.ContainsKey(entity) )
			{
				foreach ( KeyValuePair<Type, String> pair in components )
				{
					if ( !entity.Has( pair.Key ) )
					{
						return;
					}
				}
				NT node = nodePool.get();
				node.entity = entity;
				foreach ( KeyValuePair<Type, String> pair in components )
				{
					node.mComponents.Add(pair.Key, entity.Get( pair.Key ) );
				}
				entities[entity] = node;
				entity.componentRemoved += componentRemoved;
				nodes.Add( node );
				foreach( KeyValuePair<Type, INodeMap> pair in maps) {
					pair.Value.Add(node);
				}
				foreach( KeyValuePair<Type, INodeMultiMap> pair in multimaps) {
					pair.Value.Add(node);
				}
			}
		}
		
		public void Remove( Entity entity )
		{
			if( entities.ContainsKey(entity) )
			{
				NT node = entities[entity];
				entity.componentRemoved -= componentRemoved;
				entities.Remove(entity);
				nodes.Remove( node );
				foreach( KeyValuePair<Type, INodeMap> pair in maps) {
					pair.Value.Remove(node);
				}
				foreach( KeyValuePair<Type, INodeMultiMap> pair in multimaps) {
					pair.Value.Remove(node);
				}
				if( ash.updating )
				{
					nodePool.cache( node );
					ash.updateComplete += releaseNodePoolCache;
				}
				else
				{
					nodePool.dispose( node );
				}
			}
		}


		public bool InUse() 
		{
			return ( maps.Count + multimaps.Count ) > 0;
		}
		
		private void releaseNodePoolCache()
		{
			ash.updateComplete -= releaseNodePoolCache;
			nodePool.releaseCache();
		}
		
		public void CleanUp()
		{
			foreach( NT node in nodes.mNodes) {
				node.entity.componentRemoved -= this.componentRemoved;
				entities.Remove(node.entity);
			}
			foreach( KeyValuePair<Type, INodeMap> pair in maps) {
				pair.Value.RemoveAll();
			}
			foreach( KeyValuePair<Type, INodeMultiMap> pair in multimaps) {
				pair.Value.RemoveAll();
			}
			maps.Clear();
			nodes.Clear();
		}
		
		private void componentRemoved( Entity entity, Type componentClass )
		{
			if( components.ContainsKey(componentClass) )
			{
				Remove( entity );
			}
		}
	}
}
