using System;

namespace Polymics_Portable
{
    public interface IClient
    {
        bool ConnectTo(String s);
        void FlushConnection();
        bool login(string username, string password);
        String getError();
    }

    public partial class FibeClient : IClient 
    {
        public extern bool ConnectTo(string s);
        public extern void FlushConnection();
        public extern bool login(string username, string password);
        public extern string getError();
    }
}

