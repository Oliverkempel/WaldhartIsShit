namespace WaldhartIsShit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Capitech
    {
        public CapitechRequestHandler RequestHandler { get; set; }

        public Capitech()
        {
            RequestHandler = new CapitechRequestHandler();
        }
        public void login(string user, string pass)
        {
            RequestHandler.RefreshTokens(user, pass);
        }
    }
}
