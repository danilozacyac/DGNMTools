using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGNMTools.Socios
{
    public class Socio
    {
        private Int64 accion;
        private Int64 total;

        public Int64 Accion
        {
            get
            {
                return this.accion;
            }
            set
            {
                this.accion = value;
            }
        }

        public Int64 Total
        {
            get
            {
                return this.total;
            }
            set
            {
                this.total = value;
            }
        }
    }
}
