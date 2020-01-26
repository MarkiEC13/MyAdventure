using System;

namespace Adventure.Core.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException() : this("Item not found.") { }

        public ItemNotFoundException(string message) : base(message) { }
    }
}
