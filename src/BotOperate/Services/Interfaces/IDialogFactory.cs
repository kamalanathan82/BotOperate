using System.Collections.Generic;

namespace BotOperate.Services.Interfaces
{
    public interface IDialogFactory
    {
        T Create<T>();

        T Create<T, TU>(TU parameter);

        T Create<T>(IDictionary<string, object> parameters);
    }
}
