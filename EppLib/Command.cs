using System;

namespace EasyEPP
{
    public interface ICommand<T>
    {
        Response<T> Call(T obj);
    }

    public class Command<T>:ICommand<T>
    {
        public Response<T> Call(T obj)
        {
            throw new NotImplementedException();
        }
    }

    public class Response<T>
    {
    }
}
