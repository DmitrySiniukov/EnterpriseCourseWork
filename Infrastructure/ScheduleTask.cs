using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Enterprise.Infrastructure
{
    /// <summary>
	/// Represents a task for processing
	/// </summary>
	public class ScheduleTask
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Duration
        /// </summary>
        public double Duration { get; }

        /// <summary>
        /// Deadline
        /// </summary>
        public DateTime Deadline { get; }

        /// <summary>
        /// Extreme start time
        /// </summary>
        public DateTime ExtremeTime
        {
            get { return Deadline.AddSeconds(Duration); }
        }

        /// <summary>
        /// Task name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Compatible machines
        /// </summary>
        public List<int> CompatibleMachines { get; }


        public ScheduleTask(int id, double duration, DateTime deadline, string name, string description)
        {
            Id = id;
            Duration = duration;
            Deadline = deadline;
            Name = name;
            Description = description;
            CompatibleMachines = new List<int>();
        }
    }
}