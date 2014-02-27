using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tibo.fr.SharpMail.Gmail
{
    public class GmailMail
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }
        public string Author { get; set; }
        public string ID { get; set; }
        public int Pos { get; set; }

        public override string ToString()
        {
            return Title + " from " + Author;
        }

        public override bool Equals(object obj)
        {
            GmailMail other = obj as GmailMail;
            if (other != null)
            {
                return String.Equals(this.ID, other.ID);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
