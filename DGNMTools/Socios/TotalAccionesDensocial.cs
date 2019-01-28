using System;
using System.Linq;

namespace DGNMTools.Socios
{
    public class TotalAccionesDensocial
    {
        private string folio;
        private string denSocial;
        private Int64 totalAcciones;
        private Int64 totalValor;
        private Int64 porcentajeAcciones;
        private Int64 porcentajevalor;
       
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

        public string DenSocial
        {
            get
            {
                return this.denSocial;
            }
            set
            {
                this.denSocial = value;
            }
        }

        public Int64 TotalAcciones
        {
            get
            {
                return this.totalAcciones;
            }
            set
            {
                this.totalAcciones = value;
            }
        }

        public Int64 TotalValor
        {
            get
            {
                return this.totalValor;
            }
            set
            {
                this.totalValor = value;
            }
        }

        public Int64 PorcentajeAcciones
        {
            get
            {
                return this.porcentajeAcciones;
            }
            set
            {
                this.porcentajeAcciones = value;
            }
        }

        public Int64 Porcentajevalor
        {
            get
            {
                return this.porcentajevalor;
            }
            set
            {
                this.porcentajevalor = value;
            }
        }
    }
}
