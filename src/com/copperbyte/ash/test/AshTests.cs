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



	}
}
