using System;
using System.Collections.Generic;

//package net.richardlord.ash.core
namespace com.copperbyte.ash.core
{
	/**
	 * Used internally, this is an ordered list of Systems for use by the game loop.
	 */
	internal class ProcessList
	{
		// snip linked list implimentation
		internal SortedDictionary<int, List<Process> > mProcesses;	
		
		internal ProcessList() 
		{
			mProcesses = new SortedDictionary<int, List<Process>>();
		}
		
		internal void Add( Process process )
		{
			int priority = process.priority;
			if(!mProcesses.ContainsKey(priority)) 
				mProcesses.Add(priority, new List<Process>());
			if(!mProcesses[priority].Contains(process))
				mProcesses[priority].Add(process);
			
			// snip linked list implimentation
		}
		
		internal void Remove( Process process )
		{
			int priority = process.priority;
			if(mProcesses.ContainsKey(priority)) {
				mProcesses[priority].Remove(process);
				if(mProcesses[priority].Count == 0)
					mProcesses.Remove(priority);
			}
			
			// snip linked list implimentation
		}
		
		internal void removeAll() 
		{
			mProcesses.Clear();
			// snip linked list implimentation
		}
	}
}
