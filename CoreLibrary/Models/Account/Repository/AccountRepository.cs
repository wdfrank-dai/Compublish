using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersMS.Models.Repository
{
	public partial class AccountRepository
	{
		public static PersMS.Models.Account.Account Account { get; private set; }
		public static int SystemID { get; private set; }

		public PersMS.Models.Account.Account Account2 { get; private set; }
		public int SystemID2 { get; private set; }



	}
}