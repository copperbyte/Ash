using System;
using System.Collections.Generic;

using com.copperbyte.ash.core;

using NUnit.Framework;

namespace com.copperbyte.ash.test
{
	public class Id : Component
	{
		public int Value;
		/*public override bool Equals(object obj)
		{
			if(obj is Id) {
				Id other = (Id)obj;
				return (other.Value == this.Value);
			}
			return false;
		}
		
		public override int GetHashCode() {
			return ( Value );
		}*/
	}

	public class Name : Component
	{
		public string Value;
	}

	public class TestNode : Node
	{
		public Id Pos  {
			get { return (Id)entity.Get<Id>(); }
			set { entity.Add( value ); }
		}
		
		public Name Name  {
			get { return entity.Get<Name>(); }
			set { entity.Add( value ); }
		}
		
		public TestNode ()
		{
			;
		}
	} 



	[TestFixture]
	public class AshTest
	{
		Ash Ash = new Ash(); 

		[Test]
		public void testEntityHas() {
			Entity entity = new Entity();
			Id id = new Id();
			id.Value = 1;
			entity.Add(id);
			Assert.AreEqual(entity.Has<Id>(), true);
		}

		[Test]
		public void testEntityGet() {
			Entity entity = new Entity();
			Id id = new Id();
			id.Value = 1;
			entity.Add(id);
			Assert.AreEqual(entity.Get<Id>(), id);
		}


		[Test]
		public void testEntityNodeList() {
			Entity entity = new Entity();
			Id id = new Id();
			id.Value = 1;
			entity.Add(id);
			Name name = new Name();
			name.Value = "bob";
			entity.Add(name);

			NodeList<TestNode> nodeList = Ash.GetNodeList<TestNode>();
			Assert.AreEqual(nodeList.Count, 0);

			Ash.AddEntity(entity);
			Assert.AreEqual(nodeList.Count, 1);

			Ash.RemoveEntity(entity);
			Assert.AreEqual(nodeList.Count, 0);
		}

		[Test]
		public void testRemoveComponent() {
			Entity entity = new Entity();
			Id id = new Id();
			id.Value = 1;
			entity.Add(id);
			Name name = new Name();
			name.Value = "bob";
			entity.Add(name);
			
			NodeList<TestNode> nodeList = Ash.GetNodeList<TestNode>();

			Ash.AddEntity(entity);
			//Ash.RemoveEntity(entity);
			Assert.AreEqual(nodeList.Count, 1);

			entity.Remove(typeof(Name));
			Assert.AreEqual(nodeList.Count, 0); // entity should no longer be in nodeList
		}

		[Test]
		public void testRemoveEntity() {
			Entity entity = new Entity();
			Id id = new Id();
			id.Value = 1;
			entity.Add(id);
			Name name = new Name();
			name.Value = "bob";
			entity.Add(name);
			
			NodeList<TestNode> nodeList = Ash.GetNodeList<TestNode>();

			Ash.AddEntity(entity);
			Assert.AreEqual(nodeList.Count, 1);
			
			Ash.RemoveEntity(entity);
			Assert.AreEqual(nodeList.Count, 0); // entity should no longer be in nodeList
		}

		[Test]
		public void testFamilyIs() {
			Entity entity = new Entity();

			NodeList<TestNode> nodeList = Ash.GetNodeList<TestNode>();
			Assert.AreEqual(nodeList.Count, 0);

			bool IsNode = false;

			IsNode = Ash.Is<TestNode>(entity);
			Assert.AreEqual(IsNode, false);

			Id id = new Id();
			id.Value = 1;
			entity.Add(id);

			IsNode = Ash.Is<TestNode>(entity);
			Assert.AreEqual(IsNode, false);

			Name name = new Name();
			name.Value = "bob";
			entity.Add(name);
			
						
			Ash.AddEntity(entity);
			Assert.AreEqual(nodeList.Count, 1);
			
			IsNode = Ash.Is<TestNode>(entity);
			Assert.AreEqual(IsNode, true);

			Ash.RemoveEntity(entity);
			Assert.AreEqual(nodeList.Count, 0); // entity should no longer be in nodeList
		}


		[Test]
		public void testFamilyAs() {
			Entity entity = new Entity();
			
			NodeList<TestNode> nodeList = Ash.GetNodeList<TestNode>();
			Assert.AreEqual(nodeList.Count, 0);
			
			bool IsNode = false;
			
			IsNode = Ash.Is<TestNode>(entity);
			Assert.AreEqual(IsNode, false);
			
			Id id = new Id();
			id.Value = 1;
			entity.Add(id);
			
			IsNode = Ash.Is<TestNode>(entity);
			Assert.AreEqual(IsNode, false);

			Ash.AddEntity(entity);

			IsNode = Ash.Is<TestNode>(entity);
			Assert.AreEqual(IsNode, false);

			TestNode node = null;
			node = Ash.As<TestNode>(entity);
			Assert.AreEqual(node, null);

			Name name = new Name();
			name.Value = "bob";
			entity.Add(name);

			Assert.AreEqual(nodeList.Count, 1);
			
			IsNode = Ash.Is<TestNode>(entity);
			Assert.AreEqual(IsNode, true);

			node = Ash.As<TestNode>(entity);
			Assert.AreNotEqual(node, null);

			Ash.RemoveEntity(entity);
			Assert.AreEqual(nodeList.Count, 0); // entity should no longer be in nodeList
		}


		[Test]
		public void testEntityCount() {
			Entity entity = new Entity();
			Id id = new Id();
			id.Value = 1;
			entity.Add(id);
			Name name = new Name();
			name.Value = "bob";
			entity.Add(name);
			
			int Count = Ash.EntityCount();
			Assert.AreEqual(Count, 0);
			
			Ash.AddEntity(entity);
			Count = Ash.EntityCount();
			Assert.AreEqual(Count, 1);
			
			Ash.RemoveEntity(entity);
			Count = Ash.EntityCount();
			Assert.AreEqual(Count, 0);
		}
	}
}
