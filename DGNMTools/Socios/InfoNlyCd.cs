using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGNMTools.Socios
{
    public class InfoNlyCd
    {
        private string folio;
        private int idGen;
        private string oficina;
        private string nombre;
        private string paterno;
        private string materno;
        private int numAcciones;
        private int valorAcciones;
        private int total;
        private int mainId;

        public string Folio
        {
            get
            {
                return this.folio;
            }
            set
            {
                this.folio = value;
            }
        }

        public int IdGen
        {
            get
            {
                return this.idGen;
            }
            set
            {
                this.idGen = value;
            }
        }

        public string Oficina
        {
            get
            {
                return this.oficina;
            }
            set
            {
                this.oficina = value;
            }
        }

        public string Nombre
        {
            get
            {
                return this.nombre;
            }
            set
            {
                this.nombre = value;
            }
        }

        public string Paterno
        {
            get
            {
                return this.paterno;
            }
            set
            {
                this.paterno = value;
            }
        }

        public string Materno
        {
            get
            {
                return this.materno;
            }
            set
            {
                this.materno = value;
            }
        }

        public int NumAcciones
        {
            get
            {
                return this.numAcciones;
            }
            set
            {
                this.numAcciones = value;
            }
        }

        public int ValorAcciones
        {
            get
            {
                return this.valorAcciones;
            }
            set
            {
                this.valorAcciones = value;
            }
        }

        public int Total
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

        public int MainId
        {
            get
            {
                return this.mainId;
            }
            set
            {
                this.mainId = value;
            }
        }
    }
}
