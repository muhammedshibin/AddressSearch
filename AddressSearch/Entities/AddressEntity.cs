using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AddressSearch.Entities
{
    public class AddressEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
    }
}
1:

public class CommandDispatcher<TCommand>
{
    private readonly Dictionary<Type, ICommandHandler<TCommand>> _handlers = new Dictionary<Type, ICommandHandler<TCommand>>();

    public void RegisterHandler<T>(ICommandHandler<T> handler) where T : TCommand
    {
        _handlers.Add(typeof(T), (ICommandHandler<TCommand>)handler);
    }

    public void Dispatch(TCommand command)
    {
        var type = command.GetType();
        if (_handlers.ContainsKey(type))
        {
            var handler = _handlers[type];
            handler.Handle(command);
        }
        else
        {
            throw new InvalidOperationException($"No handler registered for {type.Name}");
        }
    }
}
