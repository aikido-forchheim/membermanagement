using System.Threading.Tasks;

namespace AVF.CourseParticipation.Extensions
{
    public static class TaskExtensions
    {
        public static void IgnoreResult(this Task task)
        {
            // This just silences the warnings when tasks are not awaited.
        }
    }
}
