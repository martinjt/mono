using System;

namespace System.Web.Hosting
{
	public class ISAPIApplicationHost : MarshalByRefObject, IApplicationHost
	{
		public ISAPIApplicationHost()
		{
		}

		public string ResolveRootWebConfigPath()
		{
			throw new NotImplementedException();
		}

		public IProcessHostSupportFunctions SupportFunctions()
		{
			throw new NotImplementedException();
		}
	}
}

