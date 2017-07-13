using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGNMTools.Dto
{
    public class GeneroPorAnio
    {
        private int idSocio;
        private int genero;
        private DateTime? fechaSocio;
        private int fechaInt;
        public int IdSocio
        {
            get
            {
                return this.idSocio;
            }
            set
            {
                this.idSocio = value;
            }
        }

        public int Genero
        {
            get
            {
                return this.genero;
            }
            set
            {
                this.genero = value;
            }
        }

        public DateTime? FechaSocio
        {
            get
            {
                return this.fechaSocio;
            }
            set
            {
                this.fechaSocio = value;
            }
        }

        public int FechaInt
        {
            get
            {
                return this.fechaInt;
            }
            set
            {
                this.fechaInt = value;
            }
        }
    }
}
