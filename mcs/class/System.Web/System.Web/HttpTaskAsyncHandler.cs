using System;
using System.ComponentModel;
using System.Runtime;
using System.Threading.Tasks;

namespace System.Web {
	public abstract class HttpTaskAsyncHandler : IHttpAsyncHandler, IHttpHandler {
		public virtual bool IsReusable
		{
			get
			{
				return false;
			}
		}

		protected HttpTaskAsyncHandler ()
		{
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		public virtual void ProcessRequest (HttpContext context)
		{
			throw
				new NotSupportedException (
					String.Format ("This type can't execute synchronously: {0}", this.GetType ()));
		}

		public abstract Task ProcessRequestAsync (HttpContext context);
		static Task<TResult> ToApm<TResult> (Task<TResult> task, AsyncCallback callback, object state)
		{
			var tcs = new TaskCompletionSource<TResult> (state);

			task.ContinueWith (delegate
			{
				if (task.IsFaulted) tcs.TrySetException (task.Exception.InnerExceptions);
				else if (task.IsCanceled) tcs.TrySetCanceled ();
				else tcs.TrySetResult (task.Result);
			}, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);

			return tcs.Task;
		}

		IAsyncResult IHttpAsyncHandler.BeginProcessRequest (HttpContext context, AsyncCallback cb, object extraData)
		{
			return (IAsyncResult) ToApm<int> (this.ProcessRequestAsync (context).ContinueWith<int> ((t) => 0), cb, extraData);
		}

		void IHttpAsyncHandler.EndProcessRequest (IAsyncResult result)
		{
			int r = ((Task<int>) result).Result;
			return;
		}
	}
}

