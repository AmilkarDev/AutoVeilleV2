using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Outils.Logging
{
    /// <summary>
    /// This is used to help corrolate traces under an activity id.
    /// 
    /// One difference with the standard microsoft approach is that
    /// we elected to support multiple activies happening at a time
    /// on a thread.
    /// 
    /// this class allows for more than one activity to exist, 
    /// even though this should be a rare occurance.
    /// 
    /// when ever activities are set on a thread, trace logs will include
    /// the activities in the logs, so that each log can be corrolated to 
    /// a set of activities.
    /// 
    /// When a thread is created, the activities in effect on the creating
    /// thread are automaticaly transfered to the new thread. But from that
    /// point on, they each have their own list, so one thread does not
    /// alter the other threads activity set.
    /// </summary>
    public class ActivitySet
    {
        /// <summary>
        /// synchro object.
        /// </summary>
        private static readonly Object _lock = new object();

        /// <summary>
        /// slot name used to store the activity list in the thread local storage / call context.
        /// </summary>
        private const string SlotName = "Suly.Tracing.Activities";

        /// <summary>
        /// Retreaves the list of activities currently known to the thread.
        /// note that this list SHOULD NEVER be modified directly, 
        /// the member methods of this class should be used instead.
        /// </summary>
        public static HashSet<Guid> Activities
        {
            get
            {
                lock (_lock)
                {
                    var set = CallContext.GetData(SlotName) as HashSet<Guid> ?? new HashSet<Guid>() { Guid.Empty };
                    if (set.Count == 0 || !set.Contains(Guid.Empty))
                        set.Add(Guid.Empty);
                    return set;
                }
            }

            private set
            {
                CallContext.SetData(SlotName, value);
            }
        }

        /// <summary>
        /// Add's an activity to the current thread.
        /// </summary>
        /// <param name="activityId"></param>
        public static void Add(Guid activityId)
        {
            lock (_lock)
            {
                var newSet = new HashSet<Guid>(Activities);

                newSet.Add(activityId);

                Activities = newSet;
            }
        }

        /// <summary>
        /// Remove an activity from the current thread's list of activities
        /// </summary>
        /// <param name="activityId"></param>
        public static void Remove(Guid activityId)
        {
            lock (_lock)
            {
                var newSet = new HashSet<Guid>(Activities);
                newSet.Remove(activityId);

                if (newSet.Count == 0)
                    newSet.Add(Guid.Empty);

                Activities = newSet;
            }
        }

        /// <summary>
        /// Clears the currents threads activity list and set the passed activity id as
        /// the sole activity.
        /// </summary>
        /// <param name="activityId"></param>
        public static void Set(Guid activityId)
        {
            lock (_lock)
            {
                var newSet = new HashSet<Guid>() { Guid.Empty, activityId };
                Activities = newSet;
            }
        }

        /// <summary>
        /// Clears all activities on the current thread.
        /// </summary>
        public static void Clear()
        {
            Set(Guid.Empty);
        }

        /// <summary>
        /// dump a comma seperated string containing all the activities of the current thread.
        /// </summary>
        /// <returns></returns>
        public static new String ToString()
        {
            lock (_lock)
            {
                var activites = Activities;
                return String.Join(",", activites.Select(x => x.ToString()).ToArray());
            }
        }
    }
}
