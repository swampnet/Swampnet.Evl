
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Common.Entities
{
    public class Profile
    {
        public long Id { get; set; }

        // Some kind of unique key we get from the JWT I expect
        public string Key { get; set; }

        public List<Group> Groups { get; set; }

        public Name Name { get; set; }

        public Organisation Organisation { get; set; }

		/// <summary>
		/// Return true if any of the groups profile belongs to has the sa permission
		/// </summary>
		/// <param name="permission"></param>
		/// <returns></returns>
		public bool HasPermission(string permission)
		{
			// HACK: Always true
			return true;
		}

		public bool IsInGroup(string group)
		{
			return Groups == null
				? false
				: Groups.Any(g => g.Name.EqualsNoCase(group));
		}
	}


    public class Name
    {
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string KnownAs { get; set; }
    }


    public class ProfileSummary
    {
        public long Id { get; set; }

        // Some kind of unique key we get from the JWT I expect
        public string Key { get; set; }

        public Name Name { get; set; }
    }
}
