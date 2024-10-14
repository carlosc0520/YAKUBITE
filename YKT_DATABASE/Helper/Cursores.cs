using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YKT.DATABASE.Helper
{
    public class Cursores<T1, T2>
    {
        public List<T1> Cursor1 { get; set; }
        public List<T2> Cursor2 { get; set; }
    }
    public class CursorEntidad<T1, T2>
    {
        public T1 Cursor1 { get; set; }
        public T2 Cursor2 { get; set; }
    }
    public class Cursores<T1, T2, T3>
    {
        public List<T1> Cursor1 { get; set; }
        public List<T2> Cursor2 { get; set; }
        public List<T3> Cursor3 { get; set; }
    }
}
