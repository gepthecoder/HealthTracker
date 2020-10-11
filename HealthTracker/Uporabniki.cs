using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthTracker
{
    class Uporabniki
    {
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string ime;

        public string Ime
        {
            get { return ime; }
            set { ime = value; }
        }

        private string priimek;

        public string Priimek
        {
            get { return priimek; }
            set { priimek = value; }
        }

    }
}
