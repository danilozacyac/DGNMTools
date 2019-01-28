using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGNMTools.Reportes
{
    public class Inscripcion
    {
        private int id;
        private DateTime? fechaingreso;
        private DateTime? fechainscripcion;
        private double horasInscripcion;
        private double horasInscripcionSinfines;
        private double diasInscripcion;
        private double diasInscripcionSinFines;

        public double DiasInscripcionSinFines
        {
            get
            {
                return this.diasInscripcionSinFines;
            }
            set
            {
                this.diasInscripcionSinFines = value;
            }
        }

        public double HorasInscripcionSinfines
        {
            get
            {
                return this.horasInscripcionSinfines;
            }
            set
            {
                this.horasInscripcionSinfines = value;
            }
        }

        public double HorasInscripcion
        {
            get
            {
                return this.horasInscripcion;
            }
            set
            {
                this.horasInscripcion = value;
            }
        }

        public double DiasInscripcion
        {
            get
            {
                return this.diasInscripcion;
            }
            set
            {
                this.diasInscripcion = value;
            }
        }

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public DateTime? Fechaingreso
        {
            get
            {
                return this.fechaingreso;
            }
            set
            {
                this.fechaingreso = value;
            }
        }

        public DateTime? Fechainscripcion
        {
            get
            {
                return this.fechainscripcion;
            }
            set
            {
                this.fechainscripcion = value;
            }
        }
    }
}
