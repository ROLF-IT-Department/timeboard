using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;



/// generic Singleton<T> (потокобезопасный и с отложенной инициализацией)

/// <typeparam name="T">Singleton class</typeparam>
public class Singleton<T> where T : class, new()
{
    /// «ащищенный конструктор по умолчанию необходим дл€ того, чтобы
    /// предотвратить создание экземпл€ра класса Singleton
    protected Singleton() { }

    /// ‘абрика используетс€ дл€ отложенной инициализации экземпл€ра класса
    private sealed class SingletonCreator<S> where S : class, new()
    {
        private static readonly S instance = new S();

        public static S CreatorInstance
        {
            get { return instance; }
        }
    }

    public static T Instance
    {
        get { return SingletonCreator<T>.CreatorInstance; }
    }

}

