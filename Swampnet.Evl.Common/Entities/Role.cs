//using System.Linq;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Swampnet.Evl.Common.Entities
//{
//    public class Role
//    {
//        public string Name { get; set; }
//		public IEnumerable<Permission> Permissions { get; set; }

//		public bool HasPermission(string permission)
//		{
//			return Permissions == null
//				? false
//				: Permissions.Any(p => p.Name.EqualsNoCase(permission));
//		}

//		public override string ToString()
//		{
//			return Name;
//		}
//	}
//}
