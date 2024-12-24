namespace VOEConsulting.Flame.Common.Core.Exceptions
{

    [Serializable]
	public class FlameApplicationException : Exception
	{
		public FlameApplicationException() { }
		public FlameApplicationException(string message) : base(message) { }
		public FlameApplicationException(string message, Exception inner) : base(message, inner) { }
		protected FlameApplicationException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
