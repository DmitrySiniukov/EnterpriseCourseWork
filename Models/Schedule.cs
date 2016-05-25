using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Enterprise.Infrastructure;
using Enterprise.Models;

namespace Enterprise.Models
{
	/// <summary>
	/// Represents a schedule
	/// </summary>
	public class Schedule<T> : List<T> where T : MachineSchedule, new()
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public Schedule()
		{
		}

		/// <summary>
		/// Constructor based on machines
		/// </summary>
		/// <param name="machines"></param>
		public Schedule(IEnumerable<Machine> machines)
		{
			var instance = new T();
			foreach (var machine in machines)
			{
				Add(instance.Create(machine) as T);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="machineSchedules"></param>
		public Schedule(IEnumerable<T> machineSchedules)
		{
			AddRange(machineSchedules);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="tasks"></param>
		/// <param name="machines"></param>
		/// <returns></returns>
		public static Schedule<MachineSchedule> BuildSchedule(IEnumerable<ScheduleTask> tasks, IEnumerable<Machine> machines)
		{
			return new Schedule<MachineSchedule>();
			//#region Validate arguments

			//if (tasks == null)
			//{
			//	throw new ArgumentNullException();
			//}

			//if (machines == null)
			//{
			//	throw new ArgumentNullException();
			//}

			//var tasksList = (tasks as List<Task>) ?? tasks.ToList();
			//var machinesList = (machines as List<Machine>) ?? machines.ToList();

			//if (tasksList.Count == 0 || machinesList.Count == 0)
			//{
			//	return new Schedule<MachineSchedule>(machinesList);
			//}

			//#endregion

			//tasksList.Sort((x, y) =>
			//{
			//	var result = x.ExtremeTime.CompareTo(y.ExtremeTime);
			//	return result == 0 ? x.Duration.CompareTo(y.Duration) : result;
			//});

			//var initSchedule = initialSchedule(tasksList, machinesList);

			//// Initial schedule has been found
			//if (initSchedule != null)
			//{
			//	if (initSchedule.NextTaskIndex == tasksList.Count)
			//	{
			//		return initSchedule.Convert();
			//	}

			//	// Algorithm A1.1
			//	var success = true;
			//	var sortedSet = new SortedSet<InitialMachineSchedule>(initSchedule, new EndTimeComparer());
			//	for (var i = initSchedule.NextTaskIndex; i < tasksList.Count; ++i)
			//	{
			//		var firstMachine = sortedSet.Min;
			//		var currentTask = tasksList[i];
			//		var newEndTime = firstMachine.EndTime + currentTask.Duration;
			//		if (newEndTime > currentTask.Deadline)
			//		{
			//			success = false;
			//			break;
			//		}

			//		sortedSet.Remove(firstMachine);
			//		firstMachine.Tasks.AddLast(currentTask);
			//		firstMachine.EndTime += currentTask.Duration;
			//		sortedSet.Add(firstMachine);
			//	}

			//	if (success)
			//	{
			//		return (new InitialSchedule(sortedSet).Convert());
			//	}
			//}

			//// Algorithm A2.1

			//// Sorg by (d, l)
			//tasksList.Sort((x, y) =>
			//{
			//	var result = x.Deadline.CompareTo(y.Deadline);
			//	return result == 0 ? x.Duration.CompareTo(y.Duration) : result;
			//});

			//var machineSchedule = new Schedule<MachineSchedule>(machinesList);
			//var n = tasksList.Count;
			//var m = machinesList.Count;

			//// Building schedule
			//var scheduleSet = new SortedSet<MachineSchedule>(machineSchedule, new StartTimeComparer());
			//for (var i = n - 1; i >= 0; --i)
			//{
			//	// Find unallowable with minimal start time
			//	var currentTask = tasksList[i];
			//	MachineSchedule targetMachine = null;
			//	foreach (var schedule in scheduleSet)
			//	{
			//		if (!(currentTask.Deadline > schedule.StartTime) &&
			//			(targetMachine == null || schedule.StartTime < targetMachine.StartTime))
			//		{
			//			targetMachine = schedule;
			//		}
			//	}

			//	// If founded
			//	if (targetMachine != null)
			//	{
			//		scheduleSet.Remove(targetMachine);
			//		targetMachine.Tasks.AddFirst(currentTask);
			//		targetMachine.StartTime = currentTask.ExtremeTime;
			//		scheduleSet.Add(targetMachine);
			//		continue;
			//	}

			//	// Else take machine with max start time, find allowable task with max duration
			//	var lastMachine = scheduleSet.Max;
			//	var longestTask = currentTask;
			//	var index = i;
			//	for (var j = i; j >= 0; --j)
			//	{
			//		if (!(tasksList[j].Deadline < lastMachine.StartTime) && tasksList[j].Duration > longestTask.Duration)
			//		{
			//			longestTask = tasksList[j];
			//			index = j;
			//		}
			//	}

			//	scheduleSet.Remove(lastMachine);
			//	lastMachine.Tasks.AddFirst(longestTask);
			//	lastMachine.StartTime -= longestTask.Duration;
			//	scheduleSet.Add(lastMachine);

			//	// Remove appointed task
			//	tasksList.RemoveAt(index);
			//}

			//return new Schedule<MachineSchedule>(scheduleSet);
		}

		private static InitialSchedule initialSchedule(List<ScheduleTask> tasks, List<Machine> machines)
		{
			var schedule = new InitialSchedule(machines);

			//var i = 0;
			//var j = 0;
			//var n = tasks.Count;
			//var m = machines.Count;
			//while (i < n && j < m)
			//{
			//	var currentTask = tasks[i];

			//	int? target = null;
			//	for (var k = 0; k < j; ++k)
			//	{
			//		if (schedule[k].EndTime > currentTask.ExtremeTime)
			//		{
			//			continue;
			//		}

			//		if (target != null)
			//		{
			//			return null;
			//		}

			//		target = k;
			//	}

			//	++i;

			//	if (target != null)
			//	{
			//		var t = (int)target;
			//		schedule[t].Tasks.AddLast(currentTask);
			//		schedule[t].EndTime += currentTask.Duration;
			//		continue;
			//	}

			//	// engage next processor
			//	schedule[j].StartTime = currentTask.ExtremeTime;
			//	schedule[j].Tasks.AddLast(currentTask);
			//	schedule[j].EndTime = currentTask.Deadline;
			//	++j;
			//}

			//schedule.NextTaskIndex = i;

			return schedule;
		}


		/// <summary>
		/// Initial machine schedule
		/// </summary>
		private class InitialMachineSchedule : MachineSchedule
		{
			public decimal EndTime { get; set; }


			public InitialMachineSchedule()
			{
			}

			private InitialMachineSchedule(Machine machine) : base(machine)
			{
			}


			public override MachineSchedule Create(Machine machine)
			{
				return new InitialMachineSchedule(machine);
			}
		}

		/// <summary>
		/// Initial schedule
		/// </summary>
		private class InitialSchedule : Schedule<InitialMachineSchedule>
		{
			/// <summary>
			/// 
			/// </summary>
			public int NextTaskIndex { get; set; }


			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="machines"></param>
			public InitialSchedule(IEnumerable<Machine> machines) : base(machines)
			{
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="machineSchedules"></param>
			public InitialSchedule(IEnumerable<Schedule<T>.InitialMachineSchedule> machineSchedules) : base(machineSchedules)
			{
			}


			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public Schedule<MachineSchedule> Convert()
			{
				var result = new Schedule<MachineSchedule>();
				result.AddRange(this.Cast<MachineSchedule>());
				return result;
			}
		}

		private class EndTimeComparer : IComparer<InitialMachineSchedule>
		{
			public int Compare(InitialMachineSchedule x, InitialMachineSchedule y)
			{
				return x.EndTime == y.EndTime ? x.Machine.Id.CompareTo(y.Machine.Id) : x.EndTime.CompareTo(y.EndTime);
			}
		}

		private class StartTimeComparer : IComparer<MachineSchedule>
		{
			public int Compare(MachineSchedule x, MachineSchedule y)
			{
				return x.StartTime == y.StartTime ? x.Machine.Id.CompareTo(y.Machine.Id) : x.StartTime.CompareTo(y.StartTime);
			}
		}
	}
}