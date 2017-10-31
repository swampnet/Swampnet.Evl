using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Common.Entities
{
    public class Permission
    {
		public string Name { get; set; }
		public bool IsEnabled { get; set; }

		public override string ToString()
		{
			return Name;
		}

        public const string organisation_view = "organisation.view";
        public const string organisation_create = "organisation.create";
        public const string organisation_edit = "organisation.edit";
        public const string organisation_delete = "organisation.delete";
        public const string organisation_view_all = "organisation.view-all";
    }
}
