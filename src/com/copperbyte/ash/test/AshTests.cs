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

	}
}
